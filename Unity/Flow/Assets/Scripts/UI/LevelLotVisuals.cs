using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelLotVisuals : MonoBehaviour
{
    [SerializeField] Text _myName;
    [SerializeField] Text _myResults;
    public void setColor(Color color)
    {
        if (_myName != null)
            _myName.color = color;
        else Debug.LogWarning("Pack name not set");
    }
    public void setName(string name)
    {
        if (_myName != null)
            _myName.text = name;
        else Debug.LogWarning("Pack name not set");
    }
    public void setLevelsInfo(int totalLevels, int playedLevels)
    {

        if (_myResults != null)
            _myResults.text = playedLevels + "/" + totalLevels;
        else Debug.LogWarning("Pack name not set");
    }
}
