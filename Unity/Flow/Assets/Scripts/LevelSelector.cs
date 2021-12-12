using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    private void Awake()
    {
        _tr = new InputTransformer();
        _rect = GetComponent<RectTransform>();
        _textSize = _textTr.rect.height;
        _display = GetComponent<LevelDisplayer>();
    }
    void SelectLevel()
    {
        GameManager.instance.setSelectedLevel(_selectedLevel);
        //todo FIJO QUE HAY UNA FORMA MÁS ELEGANTE DE CAMBIAR DE ESCENA
        GameManager.instance.ChangeScene(2);
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            Vector2 myInputPos = _tr.getInputPos(Input.mousePosition, _rect);
            myInputPos.y -= _textSize;
            int inputNormX = (int)(myInputPos.x / _levelsSizeX);
            int inputNormY = (int)(myInputPos.y / _levelsSizeY);
            int x = inputNormX > 0 ? (int)((myInputPos.x - (_layout.spacing.x * inputNormX - 1)) / _levelsSizeX) : inputNormX;
            int y = inputNormY > 0 ? (int)((myInputPos.y - (_layout.spacing.y * inputNormY - 1)) / _levelsSizeY) : inputNormY;
            //print("x: " + x + ", y: " + y);
            if (x >= 0 && x <= _layout.constraintCount - 1 && myInputPos.y >= 0 && y <= _display.getLevelsPerPage() / (_layout.constraintCount) - 1)
            {

                _selectedLevel = _initialLevel + (y * _layout.constraintCount + x);
                SelectLevel();
            }
        }
    }
    int _initialLevel;
    int _selectedLevel;
    float _levelsSizeX;
    float _levelsSizeY;
    float _textSize;

    public void setLevelsSize(float x, float y)
    {
        _levelsSizeX = Mathf.Abs(x);
        _levelsSizeY = Mathf.Abs(y);
    }
    public void setInitialLevel(int lvl)
    {
        _initialLevel = lvl;

    }
    InputTransformer _tr;
    RectTransform _rect;
    [SerializeField] GridLayoutGroup _layout;
    [SerializeField] RectTransform _textTr;
     LevelDisplayer _display;

}
