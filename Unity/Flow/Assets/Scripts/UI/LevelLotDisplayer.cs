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
        int numPages = numLevels / 30;
        float w = 0;
        for (int i = 0; i < numPages; i++)
        {
            Vector3 dir = new Vector3(1, 0, 0);
            Vector3 pos = _gridParent.position + (dir * i * _canvasTransform.rect.width);
            GameObject levelGridObject = Instantiate<GameObject>(_levelPagePrefab, pos, _gridParent.rotation, _gridParent);
            LevelDisplayer lvlDisplayer=levelGridObject.GetComponent<LevelDisplayer>();
            lvlDisplayer.Display(lvlLot, i);
            levelGridObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _scroll.rect.width);
            levelGridObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _scroll.rect.height);
            w += _canvasTransform.rect.width;
        }
        w -= _canvasTransform.rect.width / 2;
        _scroll.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, w);
        _scroll.Translate(Vector3.right * w / 2);
    }
    [SerializeField] GameObject _levelPagePrefab;
    [SerializeField] Transform _gridParent;
    [SerializeField] RectTransform _canvasTransform;
    [SerializeField] Text _sectionName;
    [SerializeField] RectTransform _scroll;
}
