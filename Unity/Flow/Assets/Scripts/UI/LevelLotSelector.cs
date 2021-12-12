using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLotSelector : MonoBehaviour
{

    private void Awake()
    {
        _transformer = new InputTransformer();
        _myLevelLots = new List<LevelLot>();
        _rect = GetComponent<RectTransform>();
    }
    public void addLvlLot(LevelLot lvlLot)
    {
        _myLevelLots.Add(lvlLot);
    }
    public void setSection(Section section)
    {
        _mySection = section;
    }
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector2 pos = _transformer.getInputPos(Input.mousePosition, _rect);
            int norm = (int)(pos.y / _levelLotHeight);
            //if norm is 0, pos.y could be 0 or negative because of the Integer trunk
            if (pos.y >= 0 && norm < _myLevelLots.Count)
                select(norm);
        }    

    }
     void select(int selected)
    {
        GameManager.instance.goToLevelSelection(_myLevelLots[selected], _mySection);
        //todo SEGURAMENTE HAY ALGUNA MANERA MÁS SOFISTICADA DE CAMBIAR DE ESCENA. IGUAL SE EXPLICA EN CLASE
        GameManager.instance.ChangeScene(1);
    }
    public void setLevelLotHeight(float h) { _levelLotHeight = h; }
    private List<LevelLot> _myLevelLots;
    private Section _mySection;
    private RectTransform _rect;
    private InputTransformer _transformer;
    private float _levelLotHeight;
}
