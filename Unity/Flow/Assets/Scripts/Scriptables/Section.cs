using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(menuName ="Flow/Section")]
public class Section:ScriptableObject
{
        public string SectionName;
        public Color themeColor;
        public LevelLot[] levelLots;
}
