using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sectionManager : MonoBehaviour
{
    [SerializeField] RectTransform _scrollContentTransform;
    [SerializeField] RectTransform _layoutZone;
    [SerializeField] GameObject _headerPrefab;
    [SerializeField] GameObject _levelLotPrefab;
    void Start()
    {
        float totalHeight = 0;

        Section[] sections = GameManager.instance.GetSections();
        for (int i = 0; i < sections.Length; i++)
        {
            //we instantiate the header of the section
            GameObject header = Instantiate<GameObject>(_headerPrefab, _layoutZone);
            HeaderManager headerManager = header.GetComponent<HeaderManager>();
            totalHeight += header.GetComponent<RectTransform>().rect.height;
            headerManager.setName(sections[i].SectionName);
            headerManager.setTheme(sections[i].themeColor);
            //we fill the progress data for each level lot and then we instantiate them
            string[] separators = { "\n", "\r", "\r\n", "\n\r" };
            foreach (LevelLot levellot in sections[i].levelLots)
            {
                GameObject levelLotObject = Instantiate(_levelLotPrefab, _layoutZone);
                LevelLotVisuals levelLotVisuals = levelLotObject.GetComponent<LevelLotVisuals>();
                totalHeight += levelLotObject.GetComponent<RectTransform>().rect.height;
                levelLotVisuals.setColor(sections[i].themeColor);
                levelLotVisuals.setName(levellot.LevelLotName);
                //todo CUANDO SEPAMOS GUARDAR EL PROGRESO QUE SE MIRE AQUI

                levelLotVisuals.setLevelsInfo(levellot.LevelLotFile.ToString().Split(separators,System.StringSplitOptions.RemoveEmptyEntries).Length, /*levelLot.playedLevels*/0);
            }
        }
        _scrollContentTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, totalHeight);
    }
}



