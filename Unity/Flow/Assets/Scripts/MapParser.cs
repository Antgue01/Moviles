using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class MapParser
{    
    public Map createLevelMap(string codeMap)
    {
        //Map map = new global::Map();
        Map map = new Map();
        string[] data = codeMap.Split(';');
        string[] header = data[0].Split(',');

        //HEAD FORMAT: ROWS[:COLS], RESERVED, LEVEL, HOWMANYFLOWS [,[BRIDGE[:BRIDGE]]] [,[HOLLOWS[:HOLLOWS]]] [,[WALL|WALL[:WALL|WALL]]];
        //LEVEL FORMAT: HEAD; FLOW(1); FLOW(2); FLOW(HOWMANYFLOWS-1);
        if (header[0].Length == 1)
        {
            map.setCols(int.Parse(header[0]));
            map.setRows(map.getCols());
        }
        else
        {
            map.setCols((int)(header[0][0] - '0'));
            map.setRows((int)(header[0][2] - '0'));
        }

        map.setLevel(int.Parse(header[2]));
        _totalFlows = int.Parse(header[3]);
        map.setTotalFlows(_totalFlows);
        int i = 4;
        while (i < header.Length)
        {
            if (i == 4) map.setBridges(System.Array.ConvertAll(header[i].Split(':'), int.Parse)); //SI FUNCIONA BORRAR MÉTODO readBridges, SI NO UTILIZAR MÉTODO
            //if (i == 4) _map.setBridges(readBridges(header[i]));
            else if (i == 5) map.setHollows(readHollows(header[i]));
            else if (i == 6) map.setWalls(readWalls(header[i]));
        }

        map.setFlows(readFlows(data));

        return map;
    }

    private int[] readBridges(string data)
    {
        string[] dataSplit = data.Split(':');
        int[] bridges = new int[dataSplit.Length];

        for (int x = 0; x < bridges.Length; x++)
            bridges[x] = int.Parse(dataSplit[x]);

        return bridges;
    }

    private int[] readHollows(string data)
    {
        string[] dataSplit = data.Split(':');
        int[] hollows = new int[dataSplit.Length];

        for (int x = 0; x < hollows.Length; x++)
            hollows[x] = (int) (dataSplit[x][0] - 0); // CARE WITH ADDITIONAL INFO

        return hollows;
    }

    private KeyValuePair<int,int>[] readWalls(string data)
    {
        string[] dataSplit = data.Split(':');        
        KeyValuePair<int, int>[] walls = new KeyValuePair<int, int>[dataSplit.Length];
        for (int j = 0; j < walls.Length; j++)
        {
            string a = dataSplit[j][0].ToString();
            string b = dataSplit[j][2].ToString();
            walls[j] = new KeyValuePair<int, int>(int.Parse(a), int.Parse(b));
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
