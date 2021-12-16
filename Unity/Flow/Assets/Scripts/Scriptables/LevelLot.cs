using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
[CreateAssetMenu(menuName ="Flow/LevelLot")]
public class LevelLot : ScriptableObject
{
    public string LevelLotName;
    public TextAsset LevelLotFile;
    public bool UnlockAll;
}
