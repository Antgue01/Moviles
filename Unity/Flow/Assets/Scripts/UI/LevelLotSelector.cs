using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLotSelector : MonoBehaviour
{

    public void setLvlLot(LevelLot lvlLot)
    {
        _myLevelLot = lvlLot;
    }
    public void setSection(Section section)
    {
        _mySection = section;
    }

    public void select()
    {
        GameManager.instance.goToLevelSelection(_myLevelLot, _mySection);
        //todo SEGURAMENTE HAY ALGUNA MANERA MÁS SOFISTICADA DE CAMBIAR DE ESCENA. IGUAL SE EXPLICA EN CLASE
        GameManager.instance.ChangeScene(1);
    }
    private LevelLot _myLevelLot;
    private Section _mySection;
}
