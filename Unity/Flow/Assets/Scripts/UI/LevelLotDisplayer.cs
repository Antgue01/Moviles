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
        float w = 0;
        RectTransform levelTransform = null;
        for (int i = 0; i < numPages; i++)
        {
            Vector3 pos = new Vector3( i * _canvasTransform.rect.width+ _startPoint.anchoredPosition.x,
                _startPoint.anchoredPosition.y, 0);
            GameObject levelGridObject = Instantiate<GameObject>(_levelPagePrefab, pos, _scroll.rotation, _scroll);
            LevelDisplayer lvlDisplayer = levelGridObject.GetComponent<LevelDisplayer>();
            lvlDisplayer.Display(lvlLot,section, i,levelsPerPage);
            levelTransform = levelGridObject.GetComponent<RectTransform>();
            levelTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _scroll.rect.width);
            levelTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _scroll.rect.height);
            w += _canvasTransform.rect.width;
        }
        _scroll.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, w);
        _scroll.Translate(Vector3.right * w );
    }
    [SerializeField] GameObject _levelPagePrefab;
    [SerializeField] RectTransform _canvasTransform;
    [SerializeField] Text _sectionName;
    [SerializeField] RectTransform _scroll;
    [SerializeField] RectTransform _startPoint;
   public const int levelsPerPage= 30;
}
