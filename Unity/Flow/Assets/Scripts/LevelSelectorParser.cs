using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelSelectorParser
{
    public struct Section
    {
        public string name;
        public Color themeColor;
        public LevelLotProgressParser.LevelLot[] levelLots;

    }
    /// <summary>
    /// Tries to parse a level lot selector and fills the sections variable
    /// </summary>
    /// <param name="sectionFilename"> file where the sections info is</param>
    /// <param name="sections"> variable to fill the info. If an error occurs during parsing, there will be default values in it</param>
    /// <returns>True if the parsing was successfull, False if an error occurred</returns>
    public bool TryParse(string sectionFilename, out Section[] sections)
    {
        if (File.Exists(sectionFilename))
        {

            StreamReader reader = new StreamReader(sectionFilename);
            string[] separators = { "\n", "\r", "\r\n", "\n\r" };
            //we read the full file
            string[] sectionsString = reader.ReadToEnd().Split(separators, System.StringSplitOptions.RemoveEmptyEntries);
            bool returnValue = true;
            sections = new Section[sectionsString.Length];
            for (int i = 0; i < sections.Length; i++)
            {
                //we set the name, color and level lots names
                Section s = new Section();
                string[] line = sectionsString[i].Split(';');
                s.name = line[0];
                if (!ColorUtility.TryParseHtmlString(line[1], out s.themeColor))
                {
                    returnValue = false;
                    s.themeColor = new Color();
                    Debug.LogWarning("Invalid color format");
                }
                s.levelLots = new LevelLotProgressParser.LevelLot[line.Length - 2];
                for (int j = 0; j < s.levelLots.Length; j++)
                {
                    s.levelLots[j].name = line[2 + j];
                }
                sections[i] = s;
            }
            reader.Close();
            return returnValue;
        }
        else
        {
            sections = new Section[0];
            return false;
        }
    }
}
