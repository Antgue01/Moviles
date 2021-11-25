using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // _current = BoxType.Empty;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setLevelManager(LevelManager lvlMan)
    {
        _lvlManager = lvlMan;
    }



    public void changeLevel(int level)
    {

        configureBoard();
    }

    public void resetLevel()
    {
        for(int x = 0; x< _mapSize; ++x)
        {
            _board[x].GetComponent<GameBox>().reset();
        }
    }

    public void useHint()
    {

    }


    public void setLotAndLevel(LevelLot lot, int level)
    {
        Map p = new Map();
        _map = p;
        _mapSize = _map.getRows() * _map.getCols();
        _board = new GameObject[_mapSize];
        configureBoard();
    }

    public int getFlowsConnected() { return _flowsConnected; }
    public int getMovements() { return _movements; }
    public int getBestMovements() { return _bestMovements; }
    public int getPipe() { return _pipe; }
    public int getRemainingHints() { return _remainingHints; }


    private void configureBoard()
    {
        for (int x = 0; x < _mapSize; x++)
        {
            GameObject go = Instantiate(gameBoxPrefab, new Vector3((float)0, 0, 0), Quaternion.identity) as GameObject;
            go.transform.localScale = Vector3.one;
            _board[x] = go;
        }
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


    [SerializeField] Image[] _sprites;
    [SerializeField] GameObject gameBoxPrefab;
    private LevelManager _lvlManager;
    private GameObject[] _board;
    private Map _map;
    private int _mapSize;
    private int _flowsConnected;
    private int _movements;
    private int _bestMovements;
    private int _pipe;
    private int _remainingHints;    
}