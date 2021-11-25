using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        _boardManager.setLevelManager(this);
        _boardManager.setLotAndLevel(_gameManager.getSelectedLot(), _gameManager.getSelectedLevel());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [SerializeField] GameManager _gameManager;
    [SerializeField] BoardManager _boardManager;
}
