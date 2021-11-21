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
        public LevelLot[] levelLots;

    }
    public struct LevelLot
    {
        public string name;
        public int maxLevels;
        public int playedLevels;
        public int[] movesMadePerLevel;
    }
    public Section[] Parse(string filename)
    {
        StreamReader reader = new StreamReader(filename);
        string[] separators = { "\n", "\r", "\r\n", "\n\r" };
        string[] sectionsString = reader.ReadToEnd().Split(separators, System.StringSplitOptions.RemoveEmptyEntries);
        Section[] sections = new Section[sectionsString.Length];
        reader.Close();
        return sections;
    }
}
