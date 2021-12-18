using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelLotDisplayer : MonoBehaviour
{

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
        int numLevels = levels.Length;
        int numPages = numLevels / levelsPerPage;
        float w = Camera.main.aspect * Camera.main.orthographicSize * 2;
        int i = 0;
        for (; i < numPages; i++)
        {
            GameObject levelGridObject = Instantiate(_levelPagePrefab, _contentTransform.transform);
            LevelDisplayer lvlDisplayer = levelGridObject.GetComponent<LevelDisplayer>();
            lvlDisplayer.display(lvlLot, section, i, levelsPerPage);
        }
        _contentTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (_canvasTransform.rect.width + _levelPagePrefab.GetComponent<RectTransform>().rect.width * i));
        _contentTransform.Translate(Vector3.left * _contentTransform.rect.x / 2);
    }

    [SerializeField] GameObject _levelPagePrefab;
    [SerializeField] Text _sectionName;
    [SerializeField] RectTransform _canvasTransform;
    [SerializeField] RectTransform _contentTransform;
    public const int levelsPerPage = 30;
}
