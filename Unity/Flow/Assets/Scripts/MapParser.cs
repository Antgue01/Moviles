using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class MapParser
{    
    public MapParser(StreamReader file)
    {
        //Map map = new global::Map();
        _map = new Map();
        string[] data = file.ReadLine().Split(';');
        string[] header = data[0].Split(',');


        //HEAD FORMAT: ROWS[:COLS], RESERVED, LEVEL, HOWMANYFLOWS [,[BRIDGE[:BRIDGE]]] [,[HOLLOWS[:HOLLOWS]]] [,[WALL|WALL[:WALL|WALL]]];
        //LEVEL FORMAT: HEAD; FLOW(1); FLOW(2); FLOW(HOWMANYFLOWS-1);
        if (header[0].Length == 1){
            _map.setCols(int.Parse(header[0]));
            _map.setRows(_map.getCols());
        }else{
            _map.setCols((int)(header[0][0] - 0));
            _map.setRows((int)(header[0][2] - 0));
        }

        _map.setLevel(int.Parse(header[2]));
        _flows = int.Parse(header[3]);
        int i = 4;
        while(i < header.Length)
        {
            if(i == 4) _map.setBridges(System.Array.ConvertAll(header[i].Split(':'), int.Parse)); //SI FUNCIONA BORRAR MÉTODO readBridges, SI NO UTILIZAR MÉTODO
            //if (i == 4) _map.setBridges(readBridges(header[i]));
            else if (i == 5) _map.setHollows(readHollows(header[i]));
            else if (i == 6) _map.setWalls(readWalls(header[i])); 
        }     

        _map.setFlows(readFlows(data));
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
        int[][] flows = new int[_flows][];
        for (int x = 1; x < data.Length; ++x)
            flows[x - 1] = System.Array.ConvertAll(data[x].Split(','), int.Parse);
        return flows;
    }

    Map _map;
    private int _flows;   
}
