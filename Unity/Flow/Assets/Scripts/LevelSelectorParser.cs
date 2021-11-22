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
    /// <param name="filename"> file where the sections info is</param>
    /// <param name="sections"> variable to fill the info. If an error occurs during parsing, there will be default values in it</param>
    /// <returns>True if the parsing was successfull, False if an error occurred</returns>
    public bool TryParse(string filename, out Section[] sections)
    {
        LevelLotProgressParser levelLotProgressParser = new LevelLotProgressParser();
        if (File.Exists(filename))
        {

            StreamReader reader = new StreamReader(filename);
            string[] separators = { "\n", "\r", "\r\n", "\n\r" };
            //we read the full file
            string[] sectionsString = reader.ReadToEnd().Split(separators, System.StringSplitOptions.RemoveEmptyEntries);
            bool returnValue = true;
            sections = new Section[sectionsString.Length];
            for (int i = 0; i < sections.Length; i++)
            {
                //we set the name an color
                Section s = new Section();
                string[] line = sectionsString[i].Split(';');
                s.name = line[0];
                if (!ColorUtility.TryParseHtmlString(line[1], out s.themeColor))
                {
                    returnValue = false;
                    s.themeColor = new Color();
                    Debug.LogWarning("Invalid color format");
                }
                //we fill the level lots
                if (!levelLotProgressParser.TryParse(Application.persistentDataPath+ "/Levels/SectionsProgress.txt", out s.levelLots))
                {
                    returnValue = false;
                    Debug.LogWarning("LevelLot incorrect format");
                }
                for (int j = 0; j < line.Length-2; j++)
                {
                    s.levelLots[i*(line.Length-2)+j].name = line[j + 2];
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
