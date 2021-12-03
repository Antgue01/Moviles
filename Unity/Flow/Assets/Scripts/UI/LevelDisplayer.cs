using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelDisplayer : MonoBehaviour
{

    /// <summary>
    /// Creates all the levels from a level lot
    /// </summary>
    /// <param name="lvlot">the level lot we are creating the levels from</param>
    /// <param name="section">The section of the level lot</param>
    /// <param name="gridNumber">The page of the grid</param>
    public void Display(LevelLot lvlot, Section section, int gridNumber)
    {
        _levelRange.text = (gridNumber * _numLevels + 1).ToString() + " - " + ((gridNumber + 1) * _numLevels).ToString();
        int lastCompleted = GameManager.instance.getLastCompletedLevel(section, lvlot);
        GameObject levelObject = null;
        LevelVisuals visuals = null;
        int i = 0;
        //we create all completed levels
        for (; i <= lastCompleted; i++)
        {
            levelObject = Instantiate<GameObject>(_levelPrefab, _gridTransform);
            visuals = levelObject.GetComponent<LevelVisuals>();

            visuals.setVisualVisible(LevelVisuals.LevelVisualElement.Tick, true);
            visuals.setVisualColor(LevelVisuals.LevelVisualElement.Background, section.themeColor);
            visuals.setVisualColor(LevelVisuals.LevelVisualElement.Border, section.themeColor);
            Color tickColor = section.themeColor;
            tickColor.r -= deltaColor;
            tickColor.g -= deltaColor;
            tickColor.b -= deltaColor;
            visuals.setVisualColor(LevelVisuals.LevelVisualElement.Tick, tickColor);
            visuals.setLevel((gridNumber * _numLevels) + i + 1);

        }
        //we create the unlocked level
        levelObject = Instantiate<GameObject>(_levelPrefab, _gridTransform);
        visuals = levelObject.GetComponent<LevelVisuals>();

        visuals.setVisualColor(LevelVisuals.LevelVisualElement.Background, _unlockedBackGroundColor);
        visuals.setVisualColor(LevelVisuals.LevelVisualElement.Border, _unlockedBorderColor);

        visuals.setLevel((gridNumber * _numLevels) + i + 1);
        i++;
        //we create the locked levels
        for (; i < _numLevels; i++)
        {
            levelObject = Instantiate<GameObject>(_levelPrefab, _gridTransform);
            visuals = levelObject.GetComponent<LevelVisuals>();
            visuals.setVisualVisible(LevelVisuals.LevelVisualElement.Lock, true);
            visuals.setVisualColor(LevelVisuals.LevelVisualElement.Lock, _lockColor);
            visuals.setVisualColor(LevelVisuals.LevelVisualElement.Border, _lockedBorderColor);
            visuals.setVisualColor(LevelVisuals.LevelVisualElement.Border, _lockedBackGroundColor);
            visuals.setVisualColor(LevelVisuals.LevelVisualElement.Number, _lockedNumberColor);
            visuals.setLevel((gridNumber * _numLevels) + i + 1);
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
    [SerializeField] Color _lockColor;
    [SerializeField] Color _lockedNumberColor;
    [SerializeField] GameObject _levelPrefab;
    [SerializeField] RectTransform _gridTransform;
    [SerializeField] RectTransform _Transform;
    [SerializeField] Text _levelRange;

}
