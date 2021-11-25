using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    public GameManager gameManager;
    [SerializeField]
    public BoardManager boardManager;


    // Start is called before the first frame update
    void Start()
    {
        boardManager.setLevelManager(this);
        boardManager.setLotAndLevel(gameManager.getSelectedLot(), gameManager.getSelectedLevel());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
