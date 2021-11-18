using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map
{
    public void setRows(int r) { _rows = r; }
    public void setCols(int c) { _cols = c; }
    public void setLevel(int l) { _levelNumber = l; }
    public void setFlows(int f) { _flowsNumber = f; }
    public void setBridges(int[] b) { _bridges = b; }
    public void setHollows(int[] h) {  _hollows = h; }
    public void setWalls(KeyValuePair<int, int>[] w) { _walls = w; }

    public int getRows() { return _rows; }
    public int getCols() { return _cols; }
    public int getLevel() { return _levelNumber; }
    public int getFlows() { return _flowsNumber; }
    public int[] getBridges() { return _bridges; }
    public int[] getHollows() { return _hollows; }
    public KeyValuePair<int, int>[] getWalls() { return _walls; }

    private int _rows;
    private int _cols;
    private int _levelNumber;
    private int _flowsNumber;
    private int[] _bridges;
    private int[] _hollows;
    private KeyValuePair<int, int>[] _walls;
}
