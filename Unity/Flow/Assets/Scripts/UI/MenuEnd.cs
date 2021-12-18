using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuEnd : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
