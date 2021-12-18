using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
[CreateAssetMenu(menuName ="Flow/LevelLot")]
public class LevelLot : ScriptableObject
{
    public enum ColorBehaviour { Default,PagesColor,LevelRowColor}
    public ColorBehaviour behaviour;
    public Color[] colors;
    public string[] pagesTexts;
    public string LevelLotName;
    public TextAsset LevelLotFile;
    public bool UnlockAll;
}
