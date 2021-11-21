using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sectionManager : MonoBehaviour
{
    [SerializeField] RectTransform _scrollTransform;
    LevelSelectorParser _parser;
    [SerializeField] RectTransform _layoutZone;
    [SerializeField] GameObject _headerPrefab;
    [SerializeField] GameObject _levelLotPrefab;
    void Start()
    {
        _parser = new LevelSelectorParser();

       foreach(LevelSelectorParser.Section section in _parser.Parse(Application.persistentDataPath + "sections.txt")){
            GameObject header= Instantiate<GameObject>(_headerPrefab,_layoutZone);
            HeaderManager headerManager= header.GetComponent<HeaderManager>();
            headerManager.setName(section.name);
            headerManager.setTheme(section.themeColor);
            foreach (LevelSelectorParser.LevelLot levelLot in section.levelLots)
            {
                GameObject levelLotObject = Instantiate(_levelLotPrefab, _layoutZone);
                LevelLotVisuals levelLotVisuals = levelLotObject.GetComponent<LevelLotVisuals>();
                levelLotVisuals.setColor(section.themeColor);
                levelLotVisuals.setName(levelLot.name);
                levelLotVisuals.setLevelsInfo(levelLot.maxLevels, levelLot.playedLevels);
            }
        }
    }

   
}
