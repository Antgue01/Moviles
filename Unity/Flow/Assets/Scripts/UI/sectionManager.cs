using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sectionManager : MonoBehaviour
{
    [SerializeField] RectTransform _scrollContentTransform;
    [SerializeField] RectTransform _layoutZone;
    [SerializeField] GameObject _headerPrefab;
    [SerializeField] GameObject _levelLotPrefab;
    void Start()
    {
        Section[] sections = GameManager.instance.GetSections();
        build(sections);
    }
    /// <summary>
    /// Instantiates the sections and level lots given from the GameManager
    /// </summary>
    /// <param name="sections">the sections info used to instantiate</param>
    void build(Section[] sections)
    {
        float totalHeight = 0;

        for (int i = 0; i < sections.Length; i++)
        {
            //we instantiate the header of the section
            GameObject header = Instantiate<GameObject>(_headerPrefab, _layoutZone);
            HeaderVisuals headervisuals = header.GetComponent<HeaderVisuals>();
            totalHeight += header.GetComponent<RectTransform>().rect.height;
            headervisuals.setName(sections[i].SectionName);
            headervisuals.setTheme(sections[i].themeColor);
            //we fill the progress data for each level lot and then we instantiate them
            string[] separators = { "\n", "\r", "\r\n", "\n\r" };
            foreach (LevelLot levellot in sections[i].levelLots)
            {
                GameObject levelLotObject = Instantiate(_levelLotPrefab, _layoutZone);
                LevelLotSelector selector = levelLotObject.GetComponent<LevelLotSelector>();
                selector.setLvlLot(levellot);
                selector.setSection(sections[i]);
                LevelLotVisuals levelLotVisuals = levelLotObject.GetComponent<LevelLotVisuals>();
                totalHeight += levelLotObject.GetComponent<RectTransform>().rect.height;
                levelLotVisuals.setColor(sections[i].themeColor);
                levelLotVisuals.setName(levellot.LevelLotName);
                //todo CUANDO SEPAMOS GUARDAR EL PROGRESO QUE SE CAMBIE LO DE AQUÍ ABAJO

                levelLotVisuals.setLevelsInfo(levellot.LevelLotFile.ToString().Split(separators, System.StringSplitOptions.RemoveEmptyEntries).Length, /*levelLot.playedLevels*/0);
            }
        }
        _scrollContentTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, totalHeight);

    }
  
}



