using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLotSelector : MonoBehaviour
{
    public void setLvlLot(LevelLot lvlLot) { _myLevelLot = lvlLot; }
    public void setSection(Section section) { _mySection = section; }

    public void select()
    {
        GameManager.instance.goToLevelSelection(_myLevelLot, _mySection);
        GameManager.instance.SwitchSceneTo(GameManager.SceneEnum.LevelSelector);
    }

    private LevelLot _myLevelLot;
    private Section _mySection;
}
