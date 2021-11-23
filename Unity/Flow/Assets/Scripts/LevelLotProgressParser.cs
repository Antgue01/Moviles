using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelLotProgressParser
{
    public struct LevelLot
    {
        public string name;
        public int maxLevels;
        public int playedLevels;
    }
    /// <summary>
    /// Tries to read a level progress and store the data internally
    /// </summary>
    /// <param name="filename">The name of the file where the progress is stored</param>
    /// <returns>True on success, False otherwhise</returns>
    public bool TryRead(string filename)
    {
        bool returnValue = true;
        if (File.Exists(filename))
        {

            //we fill the name, the max levels and the played levels variables
            StreamReader reader = new StreamReader(filename);
            string[] separators = { "\n", "\r", "\r\n", "\n\r" };
            //we read the full file
            _data = reader.ReadToEnd().Split(separators, System.StringSplitOptions.RemoveEmptyEntries);

            reader.Close();
            return returnValue;
        }
        else return false;
    }
    /// <summary>
    /// Tries to parse the level progression for one section
    /// </summary>
    /// <param name="sectionIndex">the section index in the menu</param>
    /// <param name="levels">the level lots of the section where we will store the info. If an error ocurs some data could not be
    /// filled</param>
    /// <returns>True on success, False otherwhise</returns>
    public bool TryParse(int sectionIndex, ref LevelLot[] levels)
    {
        bool returnValue = true;
        //we get the section data
        string[] lots = _data[sectionIndex].Split(';');
        for (int i = 0; i < lots.Length; i++)
        {

            string[] levelLotData = lots[i].Split(' ');
            //we fill the fields
            if (!int.TryParse(levelLotData[0], out levels[i].playedLevels))
            {
                returnValue = false;
                Debug.LogWarning("incorrect played levels");
            }
            if (!int.TryParse(levelLotData[1], out levels[i].maxLevels))
            {
                returnValue = false;
                Debug.LogWarning("incorrect max levels");
            }
        }
        return returnValue;

    }
    private string[] _data;
}
