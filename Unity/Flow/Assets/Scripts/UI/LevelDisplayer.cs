using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelDisplayer : MonoBehaviour
{


    public void Display(LevelLot lvlot, int gridNumber)
    {
        _levelRange.text = (gridNumber * _numLevels + 1).ToString() + " - " + ((gridNumber + 1) * _numLevels).ToString();
        for (int j = 0; j < _numLevels; j++)
        {
            GameObject levelObject = Instantiate<GameObject>(_levelPrefab,_gridTransform);
            LevelVisuals visuals = levelObject.GetComponent<LevelVisuals>();
            //todo NECESITO LA PROGRESION
            //if(lvlLot.bloqueado)
            //{
            //    visuals.setVisualVisible(LevelVisuals.LevelVisualElement.Lock, true);
            //    visuals.setVisualColor(LevelVisuals.LevelVisualElement.Border, _lockedBorderColor);
            //    visuals.setVisualColor(LevelVisuals.LevelVisualElement.Border, _lockedBackGroundColor);
            //}
            //else if (lvlLot.completed)
            //{
            //    visuals.setVisualVisible(LevelVisuals.LevelVisualElement.Tick,true);
            //    visuals.setVisualColor(LevelVisuals.LevelVisualElement.Background, section.themeColor);
            //    visuals.setVisualColor(LevelVisuals.LevelVisualElement.Border, section.themeColor);
            //    Color tickColor = section.themeColor;
            //    tickColor.r -= deltaColor;
            //    tickColor.g -= deltaColor;
            //    tickColor.b -= deltaColor;
            //    visuals.setVisualColor(LevelVisuals.LevelVisualElement.Tick, tickColor);

            //}
            //else
            //{
            visuals.setVisualColor(LevelVisuals.LevelVisualElement.Background, _unlockedBackGroundColor);
            visuals.setVisualColor(LevelVisuals.LevelVisualElement.Border, _unlockedBorderColor);

            //}
            visuals.setLevel((gridNumber * _numLevels) + j+1);

        }
    }
    public float getSize()
    {
        return _Transform.rect.size.x;
    }
    const float deltaColor = .35f;
    const int _numLevels = 30;
    [SerializeField] Color _lockedBorderColor;
    [SerializeField] Color _unlockedBorderColor;
    [SerializeField] Color _lockedBackGroundColor;
    [SerializeField] Color _unlockedBackGroundColor;
    [SerializeField] GameObject _levelPrefab;
    [SerializeField] RectTransform _gridTransform;
    [SerializeField] RectTransform _Transform;
    [SerializeField] Text _levelRange;

}
