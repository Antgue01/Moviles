using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Board : MonoBehaviour
{
   

    [SerializeField]
    public GameObject gameBoxPrefab;

    // Start is called before the first frame update
    void Start()
    {
       // _current = BoxType.Empty;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setMap(Map p)
    {
        _map = p;
        _mapSize = _map.getRows() * _map.getCols();
        _boxes = new GameObject[_mapSize];
        configureBoard();
    }

    private void configureBoard()
    {
        for(int x = 0; x < _mapSize; x++)
        {
            GameObject go = Instantiate(gameBoxPrefab, new Vector3((float)0, 0, 0), Quaternion.identity) as GameObject;
            go.transform.localScale = Vector3.one;
            _boxes[x] = go;            
        }
    }

    private void checkBridges()
    {
        for(int x = 0; x < _map.getBridges().Length; x++)
        {

        }
    }

    private void checkHollows()
    {
        for (int x = 0; x < _map.getBridges().Length; x++)
        {

        }
    }

    //private BoxType _current;
    private int[] _board;
    private Map _map;
    private int _mapSize;
    private GameObject[] _boxes;
    private Image[] _sprites;
}
