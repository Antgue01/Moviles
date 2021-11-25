using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelLotDisplayer : MonoBehaviour

{
    const int _width = 5;
    const int _height = 6;
    const float deltaColor = .35f;
    [SerializeField] GameObject _levelPrefab;
    [SerializeField] Color _lockedBorderColor;
    [SerializeField] Color _unlockedBorderColor;
    [SerializeField] Color _lockedBackGroundColor;
    [SerializeField] Color _unlockedBackGroundColor;
    [SerializeField] Transform _gridParent;
    [SerializeField] Text _sectionName;
    private void Start()
    {
        LevelLot lvlLot = GameManager.instance.getSelectedLot();
        Section section = GameManager.instance.getSelectedSection();
        display(lvlLot, section);
    }
    void display(LevelLot lvlLot, Section section)
    {
        _sectionName.text = lvlLot.LevelLotName;
        _sectionName.color = section.themeColor;
        string[] separators = { "\n", "\r", "\r\n", "\n\r" };
        string[] levels = lvlLot.LevelLotFile.ToString().Split(separators, System.StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < _height; i++)
        {
            for (int j = 0; j < _width; j++)
            {
                GameObject levelObject = Instantiate<GameObject>(_levelPrefab, _gridParent);
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
                //    visuals.setLevel(loqsea)

            }
        }
    }
}
