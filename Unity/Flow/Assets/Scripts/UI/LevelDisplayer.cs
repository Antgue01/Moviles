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
    public void display(LevelLot lvlot, Section section, int gridNumber, int levelsPerPage)
    {
        _levelsPerPage = levelsPerPage;
        int min = gridNumber * _levelsPerPage;
        int max = (gridNumber + 1) * _levelsPerPage;
        _levelRange.text = lvlot.pagesTexts[gridNumber];

        LevelVisuals visuals = null;
        GameObject levelObject = null;
        for (int i = min; i < max; i++)
        {
            levelObject = Instantiate<GameObject>(_levelPrefab, _gridTransform);
            visuals = levelObject.GetComponent<LevelVisuals>();
            if (GameManager.instance.isLevelCompleted(i))
                visualizeComplete(visuals, section, i, lvlot, gridNumber, (i-min) / 5);
            else if (GameManager.instance.isUnlockedLevel(i))
                visualizeUnlocked(visuals, lvlot, gridNumber, (i - min) / 5);
            else
                visualizeLocked(visuals);
            visuals.setLevel(i + 1);

        }
    }

    private void visualizeLocked(LevelVisuals visuals)
    {
        visuals.setVisualVisible(LevelVisuals.LevelVisualElement.Lock, true);

        visuals.setVisualColor(LevelVisuals.LevelVisualElement.Lock, _lockColor);
        visuals.setVisualColor(LevelVisuals.LevelVisualElement.Border, _lockedBorderColor);
        visuals.setVisualColor(LevelVisuals.LevelVisualElement.Border, _lockedBackGroundColor);
        visuals.setVisualColor(LevelVisuals.LevelVisualElement.Number, _lockedNumberColor);
    }

    private void visualizeUnlocked(LevelVisuals visuals, LevelLot lvlLot, int page, int row)
    {
        Color unlockedBackGroundColor = _unlockedBackGroundColor;
        Color unlockedBorderColor = _unlockedBorderColor;
        LevelLot.ColorBehaviour behaviour = lvlLot.behaviour;
        Color myColor;
        getColorDependingOnBehaviour(behaviour, out myColor, lvlLot, page, row);

        unlockedBackGroundColor *= myColor;
        unlockedBorderColor *= myColor;

        visuals.setVisualColor(LevelVisuals.LevelVisualElement.Background, unlockedBackGroundColor);
        visuals.setVisualColor(LevelVisuals.LevelVisualElement.Border, unlockedBorderColor);
        visuals.setVisualColor(LevelVisuals.LevelVisualElement.Number, _unlockedNumberColor);
    }

    void getColorDependingOnBehaviour(LevelLot.ColorBehaviour behaviour, out Color myColor, LevelLot lvlLot, int page, int row)
    {
        myColor = Color.white;
        //Color by blocks
        if (behaviour == LevelLot.ColorBehaviour.PagesColor)
        {
            int index = page % lvlLot.colors.Length;
            myColor = lvlLot.colors[index];
        }
        //Color by rows
        else if (behaviour == LevelLot.ColorBehaviour.LevelRowColor)
        {

            int index = (row + page) % lvlLot.colors.Length;
            myColor = lvlLot.colors[index];
        }
    }
    void visualizeComplete(LevelVisuals visuals, Section section, int level, LevelLot lvlLot, int page, int row)
    {
        Color bg = Color.white;
        Color tickColor = _tickBaseColor;
        Color starColor = _starBaseColor;
        LevelLot.ColorBehaviour behaviour = lvlLot.behaviour;
        Color myColor = Color.white;

        getColorDependingOnBehaviour(behaviour, out myColor, lvlLot, page, row);
        if (behaviour == LevelLot.ColorBehaviour.Default)
        {
            myColor *= section.themeColor;
        }

        bg *= myColor;

        tickColor *= myColor;

        starColor *= myColor;

        visuals.setVisualColor(LevelVisuals.LevelVisualElement.Background, bg);
        visuals.setVisualColor(LevelVisuals.LevelVisualElement.Border, bg);
        tickColor.r -= deltaColor;
        tickColor.g -= deltaColor;
        tickColor.b -= deltaColor;
        if (GameManager.instance.getIsPerfect(level))
        {
            starColor.r -= deltaColor;
            starColor.g -= deltaColor;
            starColor.b -= deltaColor;
            visuals.setVisualColor(LevelVisuals.LevelVisualElement.Star, starColor);
            visuals.setVisualVisible(LevelVisuals.LevelVisualElement.Star, true);
        }
        else
        {
            visuals.setVisualColor(LevelVisuals.LevelVisualElement.Tick, tickColor);
            visuals.setVisualVisible(LevelVisuals.LevelVisualElement.Tick, true);
        }
        visuals.setVisualColor(LevelVisuals.LevelVisualElement.Number, Color.white);

    }

    public float getSize() { return _Transform.rect.size.x; }
    public int getLevelsPerPage() { return _levelsPerPage; }

    const float deltaColor = .05f;
    int _levelsPerPage;
    [SerializeField] Color _lockedBorderColor;
    [SerializeField] Color _unlockedBorderColor;
    [SerializeField] Color _lockedBackGroundColor;
    [SerializeField] Color _unlockedBackGroundColor;
    [SerializeField] Color _unlockedNumberColor;
    [SerializeField] Color _lockColor;
    [SerializeField] Color _lockedNumberColor;
    [SerializeField] Color _tickBaseColor;
    [SerializeField] Color _starBaseColor;
    [SerializeField] GameObject _levelPrefab;
    [SerializeField] RectTransform _gridTransform;
    [SerializeField] RectTransform _Transform;
    [SerializeField] Text _levelRange;

}
