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
        //we crate the parsers
        LevelSelectorParser levelSelectorParser = new LevelSelectorParser();
        LevelLotProgressParser levelLotProgressParser = new LevelLotProgressParser();

        LevelSelectorParser.Section[] sections;
        //we read the level lots info
        if (!levelLotProgressParser.TryRead(Application.persistentDataPath + "/Levels/SectionsProgress.txt"))
        {
            Debug.LogWarning("LevelLot doesn't exists");
        }
        //we fill the sections info
        if (!levelSelectorParser.TryParse(Application.persistentDataPath + "/Levels/Sections.txt", out sections))
        {
            Debug.LogError("sections couldn't be parsed");
        }
        else
        {
            for (int i = 0; i < sections.Length; i++)
            {
                GameObject header = Instantiate<GameObject>(_headerPrefab, _layoutZone);
                HeaderManager headerManager = header.GetComponent<HeaderManager>();
                headerManager.setName(sections[i].name);
                headerManager.setTheme(sections[i].themeColor);
                if (!levelLotProgressParser.TryParse(i,ref sections[i].levelLots))
                    Debug.LogWarning("Level lot couldn't be parsed");
                foreach (LevelLotProgressParser.LevelLot levelLot in sections[i].levelLots)
                {
                    GameObject levelLotObject = Instantiate(_levelLotPrefab, _layoutZone);
                    LevelLotVisuals levelLotVisuals = levelLotObject.GetComponent<LevelLotVisuals>();
                    levelLotVisuals.setColor(sections[i].themeColor);
                    levelLotVisuals.setName(levelLot.name);
                    levelLotVisuals.setLevelsInfo(levelLot.maxLevels, levelLot.playedLevels);
                }
            }
        }
    }


}
