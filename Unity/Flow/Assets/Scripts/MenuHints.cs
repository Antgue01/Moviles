using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuHints : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void close()
    {
        _hintsMenu.SetActive(false);
    }

    [SerializeField] GameObject _hintsMenu;
    [SerializeField] LevelManager _lvlManager;
}
