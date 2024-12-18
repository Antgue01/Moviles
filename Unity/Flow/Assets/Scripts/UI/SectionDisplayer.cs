using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SectionDisplayer : MonoBehaviour
{

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
        float totalHeight = _text.rect.height + _image.rect.height + maxBannerHeightSize;

        RectTransform rect = null;
        for (int i = 0; i < sections.Length; i++)
        {
            //we instantiate the header of the section
            GameObject header = Instantiate(_headerPrefab, _contentTransform.transform);
            HeaderVisuals headervisuals = header.GetComponent<HeaderVisuals>();
            totalHeight += header.GetComponent<RectTransform>().rect.height;
            headervisuals.setName(sections[i].SectionName);
            headervisuals.setTheme(sections[i].themeColor);
            //we fill the progress data for each level lot and then we instantiate them
            string[] separators = { "\n", "\r", "\r\n", "\n\r" };
            foreach (LevelLot levellot in sections[i].levelLots)
            {
                GameObject levelLotObject = Instantiate(_levelLotPrefab, _contentTransform.transform);
                LevelLotVisuals levelLotVisuals = levelLotObject.GetComponent<LevelLotVisuals>();
                LevelLotSelector selector = levelLotObject.GetComponent<LevelLotSelector>();
                selector.setSection(sections[i]);
                selector.setLvlLot(levellot);
                rect = levelLotObject.GetComponent<RectTransform>();
                totalHeight += rect.rect.height;
                levelLotVisuals.setColor(sections[i].themeColor);
                levelLotVisuals.setName(levellot.LevelLotName);

                levelLotVisuals.setLevelsInfo(levellot.LevelLotFile.ToString().Split(separators, System.StringSplitOptions.RemoveEmptyEntries).Length, GameManager.instance.getNumPlayedLevels(sections[i],levellot));
            }

        }
        totalHeight += rect.rect.height;
        _contentTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, totalHeight);

    }
    [SerializeField] GameObject _headerPrefab;
    [SerializeField] GameObject _levelLotPrefab;
    [SerializeField] RectTransform _image;
    [SerializeField] RectTransform _text;
    [SerializeField] RectTransform _contentTransform;
    private const float maxBannerHeightSize = 150;
}



