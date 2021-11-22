using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sectionManager : MonoBehaviour
{
    [SerializeField] RectTransform _scrollTransform;
    [SerializeField] RectTransform _layoutZone;
    [SerializeField] GameObject _headerPrefab;
    [SerializeField] GameObject _levelLotPrefab;
    void Start()
    {
        LevelSelectorParser levelSelectorParser = new LevelSelectorParser();

        LevelSelectorParser.Section[] sections;
        if (levelSelectorParser.TryParse(Application.persistentDataPath + "/Levels/Sections.txt", out sections))
        {

            foreach (LevelSelectorParser.Section section in sections)
            {
                GameObject header = Instantiate<GameObject>(_headerPrefab, _layoutZone);
                HeaderManager headerManager = header.GetComponent<HeaderManager>();
                headerManager.setName(section.name);
                headerManager.setTheme(section.themeColor);
                foreach (LevelLotProgressParser.LevelLot levelLot in section.levelLots)
                {
                    GameObject levelLotObject = Instantiate(_levelLotPrefab, _layoutZone);
                    LevelLotVisuals levelLotVisuals = levelLotObject.GetComponent<LevelLotVisuals>();
                    levelLotVisuals.setColor(section.themeColor);
                    levelLotVisuals.setName(levelLot.name);
                    levelLotVisuals.setLevelsInfo(levelLot.maxLevels, levelLot.playedLevels);
                }
            }
        }
        else Debug.LogError("sections couldn't be parsed");
    }


}
