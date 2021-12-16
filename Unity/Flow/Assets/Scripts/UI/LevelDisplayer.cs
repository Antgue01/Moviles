using System;
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
    public void Display(LevelLot lvlot, Section section, int gridNumber, int levelsPerPage)
    {
        _levelsPerPage = levelsPerPage;
        _levelRange.text = (gridNumber * _levelsPerPage + 1).ToString() + " - " + ((gridNumber + 1) * _levelsPerPage).ToString();
        _selector.setInitialLevel(gridNumber * _levelsPerPage);

        LevelVisuals visuals = null;
        GameObject levelObject = null;
        for (int i = 0; i < levelsPerPage; i++)
        {
            levelObject = Instantiate<GameObject>(_levelPrefab, _gridTransform);
            visuals = levelObject.GetComponent<LevelVisuals>();
            if (GameManager.instance.isLevelCompleted(i * levelsPerPage))
                visualizeComplete(visuals, section);
            else if (lvlot.UnlockAll || GameManager.instance.isUnlockedLevel(i * levelsPerPage))
                visualizeUnlocked(visuals);
            else
                visualizeLocked(visuals);
            visuals.setLevel((gridNumber * _levelsPerPage) + i + 1);

        }
        RectTransform levelTr = levelObject.GetComponent<RectTransform>();
        _selector.setLevelsSize(levelTr.rect.width, levelTr.rect.height);
    }

    private void visualizeLocked(LevelVisuals visuals)
    {
        visuals.setVisualVisible(LevelVisuals.LevelVisualElement.Lock, true);
        visuals.setVisualColor(LevelVisuals.LevelVisualElement.Lock, _lockColor);
        visuals.setVisualColor(LevelVisuals.LevelVisualElement.Border, _lockedBorderColor);
        visuals.setVisualColor(LevelVisuals.LevelVisualElement.Border, _lockedBackGroundColor);
        visuals.setVisualColor(LevelVisuals.LevelVisualElement.Number, _lockedNumberColor);
    }

    private void visualizeUnlocked(LevelVisuals visuals)
    {
        visuals.setVisualColor(LevelVisuals.LevelVisualElement.Background, _unlockedBackGroundColor);
        visuals.setVisualColor(LevelVisuals.LevelVisualElement.Border, _unlockedBorderColor);
    }

    public float getSize()
    {
        return _Transform.rect.size.x;
    }

    void visualizeComplete(LevelVisuals visuals,Section section)
    {
        visuals.setVisualVisible(LevelVisuals.LevelVisualElement.Tick, true);
        visuals.setVisualColor(LevelVisuals.LevelVisualElement.Background, section.themeColor);
        visuals.setVisualColor(LevelVisuals.LevelVisualElement.Border, section.themeColor);
        Color tickColor = section.themeColor;
        tickColor.r -= deltaColor;
        tickColor.g -= deltaColor;
        tickColor.b -= deltaColor;
        visuals.setVisualColor(LevelVisuals.LevelVisualElement.Tick, tickColor);
    }

    public int getLevelsPerPage() { return _levelsPerPage; }
    const float deltaColor = .35f;
    int _levelsPerPage;
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
    [SerializeField] LevelSelector _selector;

}
