using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

//Maps with hollows: 0-29
//Map with walls:  30 - 59
public class MapParser
{    
    public Map createLevelMap(string codeMap)
    {
        //Map map = new global::Map();
        Map map = new Map();
        string[] separator = { ";" };
        string[] data = codeMap.Split(separator, System.StringSplitOptions.RemoveEmptyEntries);        
        string[] header = data[0].Split(',');

        //HEAD FORMAT: ROWS[:COLS][+B], RESERVED, LEVEL, HOWMANYFLOWS [,[BRIDGE[:BRIDGE]]] [,[HOLLOWS[:HOLLOWS]]] [,[WALL|WALL[:WALL|WALL]]];
        //LEVEL FORMAT: HEAD; FLOW(1); FLOW(2); FLOW(HOWMANYFLOWS-1);
        if (header[0].Length == 1)
        {
            map.setCols(int.Parse(header[0]));
            map.setRows(map.getCols());
            map.setPlusB(false);
        }
        else
        {
            string[] dimsSeparator = { ":", "+" };            
            string[] dims = header[0].Split(dimsSeparator, System.StringSplitOptions.RemoveEmptyEntries);
            if(dims.Length == 2)
            {
                map.setCols(int.Parse(dims[0]));
                map.setRows(int.Parse(dims[1]));
                map.setPlusB(false);
            }
            else if(dims.Length == 3 && dims[2].ToLower() == "b")
            {
                map.setCols(int.Parse(dims[0]));
                map.setRows(int.Parse(dims[1]));
                map.setPlusB(true);
            }           
        }

        map.setLevel(int.Parse(header[2]));
        _totalFlows = int.Parse(header[3]);
        map.setTotalFlows(_totalFlows);
        int i = 4;
        while (i < header.Length)
        {            
            if (i == 4 && header[i] != "") map.setBridges(readBridges(header[i]));
            if (i == 5 && header[i] != "") map.setHollows(readHollows(header[i]));
            else if (i == 6 && header[i] != "") map.setWalls(readWalls(header[i]));
            i++;
        }

        map.setFlows(readFlows(data));

        return map;
    }

    private int[] readBridges(string data)
    {
        return System.Array.ConvertAll(data.Split(':'), int.Parse);
    }

    private int[] readHollows(string data)
    {
        return System.Array.ConvertAll(data.Split(':'), int.Parse);
    }

    private KeyValuePair<int,int>[] readWalls(string data)
    {
        string[] dataSplit = data.Split(':');        
        KeyValuePair<int, int>[] walls = new KeyValuePair<int, int>[dataSplit.Length];
        int[] wallsplit = new int[2];

        for (int x = 0; x < walls.Length; x++)
        {
            wallsplit = System.Array.ConvertAll(dataSplit[x].Split('|'), int.Parse);           
            walls[x] = new KeyValuePair<int, int>(wallsplit[0], wallsplit[1]);
        }
        return walls;
    }

    private int[][] readFlows(string[] data)
    {
        int[][] flows = new int[_totalFlows][];
        for (int x = 0; x < _totalFlows; ++x)
            flows[x] = System.Array.ConvertAll(data[x + 1].Split(','), int.Parse);
        return flows;
    }

    private int _totalFlows;   
}
