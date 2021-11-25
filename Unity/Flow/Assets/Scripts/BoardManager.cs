using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        if (_levelDone) return;

        if(_flowsConnected == _map.getTotalFlows())
        {
            _levelDone = true;
            _levelManager.levelDone();
        }
    }

    public void setLevelManager(LevelManager lvlManager)
    {
        _levelManager = lvlManager;
    }

    public void loadMap(Map m)
    {
        _map = m;
        resetInfo();
        configureBoard();
    }


    public void resetLevel()
    {
        for(int x = 0; x< _map.getRows(); ++x)
            for(int j = 0; j<_map.getCols(); ++j)
                _board[x,j].GetComponent<GameBox>().reset();


        //Reset info
        resetInfo();
    }

    public void useHint()
    {
        //Do hint
    }    

    private void configureBoard()
    {
        _board = new GameObject[_map.getRows(), _map.getCols()];
        for (int row = 0; row < _map.getRows(); ++row)
        {
            for(int col = 0; col<_map.getCols(); ++col)
            {
                GameObject go = Instantiate(gameBoxPrefab, new Vector3((float)0, 0, 0), Quaternion.identity) as GameObject;
                go.transform.localScale = Vector3.one;
                _board[row,col] = go;
            }            
        }

        checkHollows();
        checkBridges();
    }

    private void checkBridges()
    {
        for (int x = 0; x < _map.getBridges().Length; x++)
        {

        }
    }

    private void checkHollows()
    {
        for (int x = 0; x < _map.getHollows().Length; x++)
        {

        }
    }

    private void resetInfo()
    {
        _levelDone = false;
        _movements = 0;
        _flowsConnected = 0;
        _pipe = 0;
    }


    [SerializeField] Image[] _sprites;
    [SerializeField] GameObject gameBoxPrefab;
    private LevelManager _levelManager;

    private string[] _lot;
    private int _currentLevel;
    
    private Map _map;
    private GameObject[,] _board;

    private int _flowsConnected;
    private int _movements;
    private int _pipe;
    private bool _levelDone;
}