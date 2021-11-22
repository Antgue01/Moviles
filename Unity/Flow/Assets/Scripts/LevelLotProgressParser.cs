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
    /// Tries to parse a level progress and fills the level variable
    /// </summary>
    /// <param name="levelPackName">The name of the pack. The path will be completed automatically</param>
    /// <param name="levels">the variable to fill. If an error occurred, it will be filled with some default values </param>
    /// <returns>True on success, False otherwhise</returns>
    public bool TryParse(string filename, out LevelLot[] levels)
    {
        bool returnValue = true;
        if (File.Exists(filename))
        {

            //we fill the name, the max levels and the played levels variables
            StreamReader reader = new StreamReader(filename);
            string[] separators = { "\n", "\r", "\r\n", "\n\r" };
            //we read the full file
            string[] progressString = reader.ReadToEnd().Split(separators, System.StringSplitOptions.RemoveEmptyEntries);
            if (!int.TryParse(progressString[0], out int numLots))
            {
                returnValue = false;
                Debug.LogWarning("Number of lots format is incorrect");
                //invalid value
                levels = new LevelLot[0];
            }
            else
            {

                levels = new LevelLot[numLots];
                //for every line
                for (int i = 1; i < progressString.Length; i++)
                {
                    string[] lot = progressString[i].Split(';');
                    for (int j = 0; j < lot.Length; j++)
                    {
                        string[] data = lot[j].Split(' ');
                        if (!int.TryParse(data[0], out levels[(i - 1) * lot.Length + j].playedLevels))
                        {
                            returnValue = false;
                            Debug.LogWarning("incorrect played levels");
                        }
                        if (!int.TryParse(data[1], out levels[(i - 1) * lot.Length + j].maxLevels))
                        {
                            returnValue = false;
                            Debug.LogWarning("incorrect max levels");
                        }
                    }
                }
            }
            reader.Close();
            return returnValue;
        }
        else
        {
            levels = new LevelLot[0];
            return false;
        }
    }
}
