using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// When cutting, make sure the tile is cutted before adding to the new flow and when restoring, make sure the tile is not in the flow anymore (the flow that cutted it)
/// </summary>
public class Flow
{

    public Flow()
    {
        _id = _nextExpectedId;
        _myColor = GameManager.instance.getSelectedSkin().colors[_id];
        _nextExpectedId++;
        //we cannot create an extra flow
        if (_nextExpectedId > _maxExpectedId)
        {
            Debug.LogWarning("Maximum index exceeded");
        }
    }
    //? preguntar a marco y will
    public static void setMapNumFlows(uint numFlows)
    {
        if (numFlows <= maxId)
            _maxExpectedId = numFlows;
        else _maxExpectedId = maxId;

    }
    /// <summary>
    /// Adds the point as a starter or last point and sets its first or last flag
    /// </summary>
    /// <param name="tile">the tile to be added</param>
    public void addFlowFixedPoint(GameBox tile)
    {
        if (_tiles.Count < 2)
        {
            //if it's from other flow we do nothing
            if (tile.getFlow() == null)
            {

                LinkedListNode<GameBox> n = _tiles.AddLast(tile);
                tile.setFlow(this, n);
                if (_tiles.Count == 0)
                    tile.setAsFirst();
                else tile.setAsLast();
                tile.setConfirmedNode(_confirmedTiles.AddLast(tile));
                tile.setPathColor(_myColor);
            }
            else Debug.LogError("this fixed point is from other flow. It won't be added");
        }
        else Debug.LogWarning("There are already 2 fixed points. This tile won't be added");
    }
    /// <summary>
    /// sets the starting direction of the flow depending of the position of the point in the board
    /// </summary>
    /// <param name="tile">The starting tile node</param>
    public void startDragging(LinkedListNode<GameBox> tileNode)
    {
        if (tileNode.Value.getFlow()._id == _id)
        {
            if (tileNode.Value.isFirst())
            {
                _dir = 1;
                _connected = false;
                _currentEnd = _tiles.First;
                if (tileNode.Next != null)
                    cutFromTile(tileNode.Next.Value);
            }
            else if (tileNode.Value.isLast())
            {
                _connected = false;
                _dir = -1;
                _currentEnd = _tiles.Last;
                if (tileNode.Previous != null)
                    cutFromTile(tileNode.Previous.Value);
            }
            else Debug.LogError("This is not a fixed point");
        }
        else Debug.LogError("Can't start dragging from other flow tile");

    }
    /// <summary>
    /// sets the starting direction of the flow depending of the position of the point in the board
    /// </summary>
    /// <param name="tile">The starting tile</param>
    public void startDragging(GameBox tile)
    {
        startDragging(tile.getNode());
    }
    /// <summary>
    /// adds the tile to the list and connects it to the flow if it`s neccesary
    /// </summary>
    /// <param name="tile"></param>
    public void addTile(GameBox tile)
    {
        if (tile.getFlow() == null)
        {
            if (_dir != -1 && _dir != 1)
                Debug.LogError("invalid direction on addTile");
            else if(!_connected)
            {
                if (_dir == 1)
                {
                    LinkedListNode<GameBox> node = _tiles.AddAfter(_currentEnd, tile);
                    tile.setFlow(this, node);
                    tile.setPathActive(true);
                    tile.setPathColor(_myColor);
                    _currentEnd = _currentEnd.Next;
                    if (_currentEnd.Value.isLast())
                        _connected = true;
                }
                else
                {
                    LinkedListNode<GameBox> node = _tiles.AddBefore(_currentEnd, tile);
                    tile.setFlow(this, node);
                    tile.setPathActive(true);
                    tile.setPathColor(_myColor);
                    _currentEnd = _currentEnd.Previous;
                    if (_currentEnd.Value.isFirst())
                        _connected = true;
                }
            }

        }
        else Debug.LogError("this tile is from another flow! Can't add it");
    }
    public void removeLastTile()
    {
        if (_currentEnd != _tiles.First && _currentEnd != _tiles.Last)
        {
            if (_dir != -1 && _dir != 1)
                Debug.LogError("invalid direction on removeLastTile");
            else
            {
                _connected = false;
                if (_dir == 1)
                {
                    _currentEnd.Value.setPathActive(false);
                    LinkedListNode<GameBox> aux = _currentEnd;
                    _currentEnd = aux.Previous;
                    aux.Value.setFlow(null, null);

                }
                else
                {
                    _currentEnd.Value.setPathActive(false);
                    LinkedListNode<GameBox> aux = _currentEnd;
                    _currentEnd = aux.Next;
                    aux.Value.setFlow(null, null);
                }
            }
        }
    }

    /// <summary>
    /// Depending on the flow direction adds the tile in the confirmed list before the last fixed point or after the first one
    /// </summary>
    /// <param name="tile">The tile to confirm</param>
    public void confirmTile(GameBox tile)
    {
        //if the tile isn't from this flow, we do nothing
        if (tile.getFlow()._id == _id)
        {

            if (_dir != -1 && _dir != 1)
                Debug.LogError("invalid direction on confirmTile");
            else
            {
                if (_dir == 1)
                    tile.setConfirmedNode(_confirmedTiles.AddBefore(_tiles.Last, tile));
                else
                    tile.setConfirmedNode(_confirmedTiles.AddAfter(_tiles.First, tile));
                tile.setBackgroundColor(_myColor);
                tile.setBackgroundActive(true);
            }
        }
        else Debug.LogError("Can't confirm from other flow tile");
    }
    /// <summary>
    /// removes the flows from the tile given on the flow direction from the confirmed list
    /// </summary>
    /// <param name="tile">The tile from where to disconfirm</param>
    public void disconfirmFromTile(GameBox tile)
    {
        disconfirmFromTile(tile, _dir);
    }

    /// <summary>
    /// removes the flow from the tile given on a specified direction from the confirmed list
    /// </summary>
    /// <param name="tile">The tile from where to disconfirm</param>
    /// <param name="direction">The direction to disconfirm</param>
    public void disconfirmFromTile(GameBox tile, int direction)
    {
        //if the tile isn't from this flow, we do nothing
        if (tile.getFlow()._id == _id)
        {
            if (direction != -1 && direction != 1)
                Debug.LogError("invalid direction given");
            else
            {
                _connected = false;
                tile.setBackgroundActive(false);
                if (direction == 1)
                {
                    disconfirmForwards(tile);
                }
                else
                {
                    disconfirmBackwards(tile);
                }
                //we remove the tile from the confirmed list
                _confirmedTiles.Remove(tile.getNode());

            }
        }
        else Debug.LogError("Can't disconfirm from other flow tile");
    }



    /// <summary>
    /// cuts the flow from the tile given on the flow direction, but leaves the confirmed tiles in order to be able to restore the flows
    /// </summary>
    /// <param name="tile">The tile from where to cut</param>
    public void cutFromTile(GameBox tile)
    {
        cutFromTile(tile, _dir);
    }
    /// <summary>
    /// cuts the flow from the tile given on a specified direction, but leaves the confirmed tiles in order to be able to restore the flows
    /// </summary>
    /// <param name="tile">The tile from where to cut</param>
    /// <param name="direction">The direction to cut</param>
    public void cutFromTile(GameBox tile, int direction)
    {
        //if the tile isn't from this flow, we do nothing
        if (tile.getFlow()._id == _id)
        {
            if (direction != -1 && direction != 1)
                Debug.LogError("invalid direction given");
            else
            {
                //we remove the references of the current tile
                tile.setPathActive(false);
                tile.setFlow(null, null);
                if (direction == 1)
                {
                    cutForwards(tile);
                }
                else
                {
                    cutBackwards(tile);
                }
                //we remove the tile
                _tiles.Remove(tile.getNode());

            }
        }
        else Debug.LogError("Can't cut from other flow tile");
    }
    //-----------------------------------------Privates------------------------------------------
    private void cutBackwards(GameBox tile)
    {
        LinkedListNode<GameBox> prev;
        prev = tile.getNode().Previous;
        while (!prev.Value.isFirst())
        {
            //we remove the references of the current tile
            prev.Value.setPathActive(false);
            prev.Value.setFlow(null, null);
            LinkedListNode<GameBox> aux = prev;
            prev = aux.Previous;
            _tiles.Remove(aux);
        }
    }

    private void cutForwards(GameBox tile)
    {
        LinkedListNode<GameBox> next;
        next = tile.getNode().Next;
        while (!next.Value.isLast())
        {
            next.Value.setPathActive(false);
            next.Value.setFlow(null, null);
            LinkedListNode<GameBox> aux = next;
            next = aux.Next;
            _tiles.Remove(aux);
        }
    }

    public void restore(GameBox tile, int direction)
    {
        //if the tile isn't from this flow, we do nothing
        if (tile.getFlow()._id == _id)
        {
            if (direction != 1 && direction != -1)
                Debug.LogError("Invalid direction to restore");
            else
            {
                if (tile.getFlow() != null)
                    Debug.LogWarning("This tile already has another flow, restoring it before removing from the other flow " +
                        "could result on unexpected errors");
                tile.setColor(_myColor);
                if (tile.getConfirmedNode() == null)
                    Debug.LogError("Couldn't restore from null confirmed node");
                else
                    addTileFromOther(tile, tile.getConfirmedNode(), direction);
                if (direction == 1)
                {
                    restoreForwards(tile);
                }
                else
                {
                    restoreBackWards(tile);
                }
            }
        }
        else Debug.LogError("Can't restore from other flow tile");
    }

    private void disconfirmBackwards(GameBox tile)
    {
        LinkedListNode<GameBox> confirmedNode = tile.getConfirmedNode();
        if (confirmedNode != null)
        {

            LinkedListNode<GameBox> prev = confirmedNode.Previous;
            while (!prev.Value.isLast())
            {
                prev.Value.setBackgroundActive(false);
                LinkedListNode<GameBox> aux = prev;
                prev = aux.Previous;
                _confirmedTiles.Remove(aux);
            }
        }
        else Debug.LogError("This tile isn't confirmed!");
    }

    private void disconfirmForwards(GameBox tile)
    {
        LinkedListNode<GameBox> confirmedNode = tile.getConfirmedNode();
        if (confirmedNode != null)
        {
            LinkedListNode<GameBox> next = confirmedNode.Next;
            while (!next.Value.isLast())
            {
                next.Value.setBackgroundActive(false);
                LinkedListNode<GameBox> aux = next;
                next = aux.Next;
                _confirmedTiles.Remove(aux);
            }
        }
        else Debug.LogError("This tile isn't confirmed!");
    }
    /// <summary>
    /// adds the tile from a specified node in the desired direction
    /// </summary>
    /// <param name="tile">the tile to add</param>
    /// <param name="from">the node from where to add</param>
    /// <param name="direction">the direction to add</param>
    private void addTileFromOther(GameBox tile, LinkedListNode<GameBox> from, int direction)
    {
        if (direction != -1 && direction != 1)
            Debug.LogError("invalid direction on addTile");
        else
        {
            if (direction == 1)
                tile.setFlow(this, _tiles.AddAfter(from, tile));
            else
                tile.setFlow(this, _tiles.AddBefore(from, tile));
            tile.setColor(_myColor);
        }
    }
    /// <summary>
    /// restores the flow backwards from a tile
    /// </summary>
    /// <param name="tile"></param>
    private void restoreBackWards(GameBox tile)
    {
        LinkedListNode<GameBox> aux = tile.getConfirmedNode();
        LinkedListNode<GameBox> prev = aux.Previous;
        while (!prev.Value.isFirst())
        {
            prev.Value.setColor(_myColor);
            prev.Value.setPathActive(true);
            if (prev.Value.getConfirmedNode() == null)
                Debug.LogError("Couldn't restore from null confirmed node");
            else
                addTileFromOther(prev.Value, aux, -1);
            prev = prev.Previous;
            aux = aux.Previous;
        }
    }

    /// <summary>
    /// restores the flow forwards from a tile
    /// </summary>
    /// <param name="tile"></param>
    private void restoreForwards(GameBox tile)
    {
        LinkedListNode<GameBox> aux = tile.getConfirmedNode();
        LinkedListNode<GameBox> next = aux.Next;
        while (!next.Value.isLast())
        {
            next.Value.setColor(_myColor);
            next.Value.setPathActive(true);
            if (next.Value.getConfirmedNode() == null)
                Debug.LogError("Couldn't restore from null confirmed node");
            else
                addTileFromOther(next.Value, aux, 1);
            next = next.Next;
            aux = aux.Next;
        }
    }

    public Color GetColor() { return _myColor; }
    public bool getConnected() { return _connected; }
    public void setConnected(bool value) { _connected = value; }

    int _id = -1;
    const int maxId = 15;
    private static uint _maxExpectedId = maxId - 1;
    private static int _nextExpectedId = 0;
    int _dir;
    bool _connected;
    Color _myColor;
    LinkedListNode<GameBox> _currentEnd = null;
    LinkedList<GameBox> _tiles;
    LinkedList<GameBox> _confirmedTiles;
}
