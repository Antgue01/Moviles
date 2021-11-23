using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Flow/Skin")]
public class Skin : ScriptableObject
{
    const int NumColors= 16;
    public string skinName;
    public Color[] colors = new Color[NumColors];
}
