using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuEnd : MonoBehaviour
{
  
    public void nextLevel()
    {
        _lvlManager.nextLevel();
        close();
    }

    public void close()
    {
        _menu.SetActive(false);
    }

    [SerializeField] GameObject _menu;
    [SerializeField] LevelManager _lvlManager;
}
