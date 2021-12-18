using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelLotVisuals : MonoBehaviour, IPointerDownHandler,IPointerUpHandler
{
    public void setColor(Color color)
    {
        _myColor = color;
        _myName.color = color;
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

    public void OnPointerDown(PointerEventData eventData)
    {
        _myName.color = Color.white;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _myName.color = _myColor;
    }

    [SerializeField] Text _myName;
    [SerializeField] Text _myResults;
    Color _myColor;
}
