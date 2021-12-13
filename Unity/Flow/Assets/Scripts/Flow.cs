using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flow
{
    int _id = -1;
    const int maxId = 15;
    private static uint _maxExpectedId = maxId - 1;
    private static int _nextExpectedId = 0;
    int _dir;
    bool _connected;
    LinkedList<GameBox> _tiles;
    LinkedList<GameBox> _confirmedTiles;
    public Flow()
    {
        _id = _nextExpectedId;
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
            LinkedListNode<GameBox> n = _tiles.AddLast(tile);
            tile.setFlow(this, n);
            if (_tiles.Count == 0)
                tile.setAsFirst();
            else tile.setAsLast();
            tile.setConfirmedNode(_confirmedTiles.AddLast(tile));
        }
        else Debug.LogWarning("There are already 2 fixed points. This tile won't be added");
    }
    /// <summary>
    /// sets the starting direction of the flow depending of the position of the point in the board
    /// </summary>
    /// <param name="tile">The starting tile node</param>
    public void startDragging(LinkedListNode<GameBox> tileNode)
    {
        if (tileNode.Value.isFirst())
            _dir = 1;
        else if (tileNode.Value.isLast())
            _dir = -1;
        else Debug.LogError("This is not a fixed point");
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
    /// Depending on the flow direction adds the tile before the last fixed point or after the first one
    /// </summary>
    /// <param name="tile">The tile to add</param>
    public void addTile(GameBox tile)
    {
        if (_dir == 1)
            tile.setFlow(this, _tiles.AddBefore(_tiles.Last, tile));
        else if (_dir == -1)
            tile.setFlow(this, _tiles.AddAfter(_tiles.First, tile));
        else
            Debug.LogError("Invalid direction on addTile");
    }
    /// <summary>
    /// Depending on the flow direction adds the tile in the confirmed list before the last fixed point or after the first one
    /// </summary>
    /// <param name="tile">The tile to confirm</param>
    public void confirmTile(GameBox tile)
    {
        if (_dir == 1)
            tile.setConfirmedNode(_confirmedTiles.AddBefore(_tiles.Last, tile));
        else if (_dir == -1)
            tile.setConfirmedNode(_confirmedTiles.AddAfter(_tiles.First, tile));
        else
            Debug.LogError("Invalid direction on addTile");
    }
    public void cutFromTile(GameBox tile)
    {
        if (tile.getFlow()._id == _id)
        {

        }
        else Debug.LogError("Can't cut from other flow tile");
    }
    public void Restore()
    {

    }
    public bool getConnected() { return _connected; }
    public void setConnected(bool value) { _connected = value; }
}
