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
        RectTransform levelTransform = null;
        float w = Camera.main.aspect * Camera.main.orthographicSize * 2;
        int i = 0;
        for (; i < numPages; i++)
        {
            Vector3 pos = new Vector3(_startPoint.position.x, _startPoint.position.y, 0);
            GameObject levelGridObject = Instantiate<GameObject>(_levelPagePrefab, pos, Quaternion.identity, _scroll);
            LevelDisplayer lvlDisplayer = levelGridObject.GetComponent<LevelDisplayer>();
            lvlDisplayer.Display(lvlLot, section, i, levelsPerPage);
            levelTransform = levelGridObject.GetComponent<RectTransform>();
            levelTransform.Translate(Vector3.right * i * w);
            //levelTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _scroll.rect.width);
            //levelTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _scroll.rect.height);
            //w += _canvasTransform.rect.width;
        }
        _scroll.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, -2 * _canvasTransform.rect.x * i);
        _scroll.transform.Translate(Vector3.left * _canvasTransform.rect.x / 2);
        //_scroll.SetPositionAndRotation(Vector3.right * w,Quaternion.identity );
    }
    [SerializeField] GameObject _levelPagePrefab;
    [SerializeField] RectTransform _canvasTransform;
    [SerializeField] Text _sectionName;
    [SerializeField] RectTransform _scroll;
    [SerializeField] RectTransform _startPoint;
    public const int levelsPerPage = 30;
}
