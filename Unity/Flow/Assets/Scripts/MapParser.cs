using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MapParser
{
    public MapParser(StreamReader read)
    {
        //StreamReader read = new StreamReader(Application.persistentDataPath + "Assets/Maps/" + filename);
        string[] map = read.ReadLine().Split(';');
        string[] header = map[0].Split(',');
        int i = 0;
        cols = int.Parse(header[i]);
        i++;
        if (header[i] == ":")
        {
            i++;
            rows = int.Parse(header[i]);
        }
        else rows = cols;
        i++;
        levelNumber = int.Parse(header[i]);
        i++;
        flowsNumber = int.Parse(header[i]);
        i++;
        if (i < header.Length)
            if (header[i] != "")
            {
                string[] bridgesString = header[i].Split(':');
                bridges = new int[bridgesString.Length];
                for (int j = 0; j < bridges.Length; j++)
                {
                    bridges[j] = int.Parse(bridgesString[j]);
                }
                i++;
            }
        if (i < header.Length)
            if (header[i] != "")
            {
                string[] hollowsStrings = header[i].Split(':');
                hollows = new int[hollowsStrings.Length];
                for (int j = 0; j < hollows.Length; j++)
                {
                    hollows[j] = int.Parse(hollowsStrings[j]);
                }
                i++;
            }
        if (i < header.Length)
        {
            string[] wallsStrings = header[i].Split(':');
            walls = new KeyValuePair<int, int>[wallsStrings.Length];
            for (int j = 0; j < walls.Length; j++)
            {
                string a = wallsStrings[j][0].ToString();
                string b = wallsStrings[j][2].ToString();
                walls[j] = new KeyValuePair<int, int>(int.Parse(a),int.Parse(b));
            }
            i++;

        }


    }
    int rows;
    int cols;
    int levelNumber;
    int flowsNumber;
    int[] bridges;
    int[] hollows;
    KeyValuePair<int, int>[] walls;
}
