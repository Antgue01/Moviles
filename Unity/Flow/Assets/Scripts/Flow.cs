using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// When cutting, make sure the tile is cutted before adding to the new flow and when restoring, make sure the tile is not in the flow anymore (the flow that cutted it)
/// </summary>
public class Flow
{
    public Flow(BoardManager bm)
    {
        _boardManager = bm;
        _id = _nextExpectedId;
        _flowColor = GameManager.instance.getSelectedSkin().colors[_id];
        _nextExpectedId++;
        _tiles = new LinkedList<GameBox>();
        _confirmedTiles = new LinkedList<GameBox>();
        _lastConfirmedTiles = new LinkedList<GameBox>();
        //we cannot create an extra flow
        if (_nextExpectedId > _maxExpectedId)
        {
            Debug.LogWarning("Maximum index exceeded");
        }
        _hintUsedInThisFlow = false;
    }
    //? preguntar a marco y will
    public static void setMapNumFlows(uint numFlows)
    {
        if (numFlows <= maxId)
            _maxExpectedId = numFlows;
        else _maxExpectedId = maxId;
        _nextExpectedId = 0;
    }

    /// <summary>
    /// sets the starting direction of the flow depending of the position of the point in the board
    /// </summary>
    /// <param name="tile">The starting tile</param>
    public void startDragging(GameBox tile)
    {
        disconfirmTiles();
        //If its a flow point, we set it as first
		if (tile.getType() == GameBox.BoxType.FlowPoint)
		{
            //If there is already a path created in other flow point, we clear it
            if(_tiles.First != null)
			{
                clearTileList();
			}
            tile.setNode(_tiles.AddLast(tile));
		}
        //If its a normal flow, we cut from here to continue
		else
		{
            cutFromTile(tile);
		}
    }

    public void stopDragging()
	{
        if (willMapBeModified())
            _boardManager.mapModified(_id);
        confirmTiles();
	}

    /// <summary>
    /// Clears the _tiles list from origin
    /// </summary>
    public void clearTileList()
	{
        if (_tiles.Count == 0) return;

        LinkedListNode<GameBox> tileNode = _tiles.First;
        LinkedListNode<GameBox> tileNextNodeAux = tileNode.Next;
        GameBox tile = tileNode.Value;

        tile.setNode(null);
        _tiles.Remove(tileNode);

        while (tileNextNodeAux != null)
        {
            tileNode = tileNextNodeAux;
            tileNextNodeAux = tileNode.Next;
            tile = tileNode.Value;

            tile.setPathActive(false);
            //If its a flow point, we keep the reference of Flow
            tile.setFlow((tile.getType() == GameBox.BoxType.FlowPoint) ? this : null);
            tile.setNode(null);
            _tiles.Remove(tileNode);
        }
    }

    /// <summary>
    /// Tries to connect two tiles by flows, treating diagonal case
    /// </summary>
    /// <returns>true if success</returns>
    public bool connectFlow(GameBox newFlow, Vector2Int lastInputRowCol, Vector2Int direction)
    {
        GameBox currentGameBox = newFlow;

        //Diagonal case
        if (direction.x != 0 && direction.y != 0)
        {
            //First check with one of the components of direction
            GameBox auxGB = _boardManager.getBoard()[lastInputRowCol.x + direction.y, lastInputRowCol.y].GetComponent<GameBox>();
            Vector2Int auxDir = new Vector2Int(0, direction.y);
            bool valid = (auxGB.getType() == GameBox.BoxType.Empty);
            //If not valid, we try again in another direction (with the other component of direction)
            if (!valid)
            {
                auxGB = _boardManager.getBoard()[lastInputRowCol.x, lastInputRowCol.y + direction.x].GetComponent<GameBox>();
                auxDir = new Vector2Int(direction.x, 0);
                valid = (auxGB.getType() == GameBox.BoxType.Empty);
            }

            if (valid)
            {
                //We save the component not choosen at auxDir
                direction = (auxDir.x != 0) ? new Vector2Int(0, direction.y) : new Vector2Int(direction.x, 0);
                linkGameBox(auxGB, auxDir);
            }
            //We cant treat diagonal case
            else
            {
                return false;
            }
        }

        linkGameBox(currentGameBox, direction);

        return true;
    }

    /// <summary>
    /// Auxiliar method to link a GameBox with direction dir
    /// </summary>
    public void linkGameBox(GameBox newFlow, Vector2Int dir)
    {
        newFlow.setFlow(this);
        newFlow.setNode(_tiles.AddLast(newFlow));
        newFlow.setPathColor(_flowColor);
        newFlow.setPathFrom(dir);
    }

    /// <summary>
    /// Adds the tile to the list and connects it to the flow
    /// </summary>
    /// <returns>true if success</returns>
    public bool addTile(GameBox tile, Vector2Int lastInputRowCol, Vector2Int direction)
    {
        bool success;
        //No flow assigned yet
		if (tile.getFlow() == null)
		{
            success = connectFlow(tile, lastInputRowCol, direction);
		}
		else
		{
            //Same flow component
			if (tile.getFlow() == this)
			{
                //Already in list
				if (_tiles.Contains(tile))
				{
                    cutFromTile(tile);
                    success = true;
				}
                //Must be the other flow point
				else
				{
                    connectFlow(tile, lastInputRowCol, direction);
                    //Force to stop input
                    success = false;
                }
			}
            //Different flow component
            else
            {
                //And its a flow point
                if (tile.getType() == GameBox.BoxType.FlowPoint)
                {
                    success = false;
                }
				else
				{
                    tile.getFlow().cutFromTile(tile.getNode().Previous.Value);
                    success = connectFlow(tile, lastInputRowCol, direction);
                }
            }
		}

        return success;
    }

    /// <summary>
    /// Always before confirmTiles if needed, to check if the map will be modified
    /// </summary>
    /// <returns></returns>
    public bool willMapBeModified()
	{
        if (_tiles.Count == 1 && _lastConfirmedTiles.Count < 1) return false;
        if (_tiles.Count != _lastConfirmedTiles.Count) return true;

        LinkedListNode<GameBox> tileNode = _tiles.First;

        while (tileNode != null)
		{
            if (!_lastConfirmedTiles.Contains(tileNode.Value)) return true;
            tileNode = tileNode.Next;
        }

        return false;
    }

    public void saveLastConfirmedTiles()
	{
        LinkedListNode<GameBox> tileNode = _confirmedTiles.First;

        _lastConfirmedTiles.Clear();

        while (tileNode != null)
        {
            _lastConfirmedTiles.AddLast(tileNode.Value);
            tileNode = tileNode.Next;
        }
    }

    /// <summary>
    /// Adds tiles to the confirmed list
    /// </summary>
    public void confirmTiles()
	{
        LinkedListNode<GameBox> tileNode = _tiles.First;

        if (tileNode.Next == null)
            _tiles.Remove(tileNode);
		else
		{
            while (tileNode != null)
            {
                GameBox tile = tileNode.Value;
                Flow confirmedFlow = tile.getConfirmedFlow();
                if (confirmedFlow != null && confirmedFlow != this)
                {
                    confirmedFlow.disconfirmTiles();
                    confirmedFlow.confirmTiles();
                }
                tile.setBackgroundActive(true);
                tile.setColor(_flowColor);
                tile.setConfirmedFlowDir(tile.getFlowDir());
                tile.setConfirmedNode(_confirmedTiles.AddLast(tileNode.Value));
                tile.confirmFlow();
                tileNode = tileNode.Next;
            }
        }

        saveLastConfirmedTiles();

        if(_confirmedTiles.Count>0)
            _connected = _confirmedTiles.Last.Value.getType()==GameBox.BoxType.FlowPoint;
        if(_connected) _boardManager.updateFlowsConnected(1);
    }

    public void disconfirmTiles()
	{
        LinkedListNode<GameBox> tileNode = _confirmedTiles.First;

        while(tileNode != null)
		{
            GameBox tile = tileNode.Value;
            tile.setBackgroundActive(false);
            tile.setConfirmedNode(null);
            tile.disconfirmFlow();
            LinkedListNode<GameBox> tileNodeNextAux = tileNode.Next;
            _confirmedTiles.Remove(tileNode);
            tileNode = tileNodeNextAux;
		}

        if (_connected)
        {
            _boardManager.updateFlowsConnected(-1);
            _connected = false;
        }        
    }

    /// <summary>
    /// cuts the flow from the tile given on the flow direction, but leaves the confirmed tiles in order to be able to restore the flows
    /// </summary>
    /// <param name="fromTile">The tile from where to cut</param>
    public void cutFromTile(GameBox fromTile)
    {
        LinkedListNode<GameBox> tileNode = fromTile.getNode().Next;
        while (tileNode != null)
        {
            GameBox tile = tileNode.Value;
            tile.setPathActive(false);
            //If its a flow point, we keep the reference of Flow
			tile.setFlow((tile.getType()==GameBox.BoxType.FlowPoint) ? this : null);
            tile.setNode(null);
            LinkedListNode<GameBox> tileNextNodeAux = tileNode.Next;
            _tiles.Remove(tileNode);

            Flow confirmedFlow = tile.getConfirmedFlow();
            if (confirmedFlow != null && confirmedFlow != this)
                confirmedFlow.tryToRestoreCuts();

            tileNode = tileNextNodeAux;
        }
    }

    /// <summary>
    /// Tries to restore confirmed flows
    /// </summary>
    public void tryToRestoreCuts()
	{
        LinkedListNode<GameBox> tileNode = _tiles.Last;
        tileNode = _confirmedTiles.Find(tileNode.Value);
        tileNode = tileNode.Next;
		while (tileNode != null)
		{
            GameBox tile = tileNode.Value;
            if (tile.getFlow() != null && tile.getFlow() != this) break;
            tile.setNode(_tiles.AddLast(tile));
            tile.setFlow(tile.getConfirmedFlow());
            tile.setPathActive(true);
            tile.setPathColor(tile.getColor());
            tile.setPathFrom(tile.getConfirmedFlowDir());
            tileNode = tileNode.Next;
		}
	}

    public bool useHintOnFlow()
    {        
        if(!_hintUsedInThisFlow && _connected)
        {
            //if(else CAMINO QUE YA ESTÁ HECHO COINCIDE CON EL DE LA HINT NO HAY QUE HACERLE HINT)
            return false;
        }

        if (!_hintUsedInThisFlow)
        {
            _hintUsedInThisFlow = true;

            disconfirmTiles();
            clearTileList();
            int[] flowPath = _boardManager.getMap().getFlows()[_id];


            Vector2Int lastTileRowCol = new Vector2Int();
            lastTileRowCol.x = flowPath[0] / _boardManager.Cols;
            lastTileRowCol.y = flowPath[0] % _boardManager.Cols;

            _tiles.AddLast(_boardManager.getBoard()[lastTileRowCol.x, lastTileRowCol.y].GetComponent<GameBox>());

            Vector2Int currentTileRowCol = new Vector2Int();
            for (int x=1; x < flowPath.Length; x++)
            {
                currentTileRowCol.x = flowPath[x] / _boardManager.Cols;
                currentTileRowCol.y = flowPath[x] % _boardManager.Cols;               

                Vector2Int direction = new Vector2Int(currentTileRowCol.y - lastTileRowCol.y,
                                                      currentTileRowCol.x - lastTileRowCol.x);

                GameBox currentGameBox = _boardManager.getBoard()[currentTileRowCol.x, currentTileRowCol.y].GetComponent<GameBox>();
                addTile(currentGameBox, lastTileRowCol, direction);
                lastTileRowCol = currentTileRowCol;
            }

            if (willMapBeModified())
                _boardManager.mapModified(_id);
            confirmTiles();
        }
        return true;
    }

    public void setUsedHintInThisFlow(bool u) { _hintUsedInThisFlow = u; }
    public bool getUsedHintInThisFlow() { return _hintUsedInThisFlow; }
    public Color GetColor() { return _flowColor; }
    public bool getConnected() { return _connected; }
    public void setConnected(bool value) { _connected = value; }

    


    int _id = -1;
    const int maxId = 15;
    private static uint _maxExpectedId = maxId - 1;
    private static int _nextExpectedId = 0;
    private bool _connected;
    private Color _flowColor;
    private LinkedList<GameBox> _tiles;
    private LinkedList<GameBox> _confirmedTiles;
    private LinkedList<GameBox> _lastConfirmedTiles;
    private BoardManager _boardManager;
    private bool _hintUsedInThisFlow;
}
