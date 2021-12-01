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

        if(_map != null && _flowsConnected == _map.getTotalFlows())
        {
            _levelDone = true;
            _levelManager.setLevelDone(true);
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
        if(_map != null && _grid != null)
        {           
            _grid.GetComponent<GridLayoutGroup>().constraintCount = _map.getCols();
           
            _board = new GameObject[_map.getRows(), _map.getCols()];
            Color sectionColor = GameManager.instance.getSelectedSection().themeColor;
            for (int row = 0; row < _map.getRows(); ++row)
            {
                for (int col = 0; col < _map.getCols(); ++col)
                {
                    GameObject go = Instantiate(gameBoxPrefab, _grid.transform) as GameObject;
                    go.GetComponent<Image>().color = sectionColor;
                    _board[row, col] = go;
                }
            }

            createFlowPoints();
            checkHollows();
            checkBridges();
        }
       
    }
    /// <summary>
    /// Create the start and end point for every color flow.
    /// </summary>
    private void createFlowPoints()
	{
        int[][] flows = _map.getFlows();
        if(flows != null)
		{
            int colorIndex = 0;
            foreach(int[] flowColor in flows)
			{
                //Start index
                int index = flowColor[0];
                int row = index / _map.getCols();
                int column = index % _map.getCols();
                GameObject auxOb = _board[row, column];
                GameBox gb = auxOb.GetComponent<GameBox>();
				gb.setFigureSprite(_sprites[0]);
                gb.setFigureColor(GameManager.instance.getSelectedSkin().colors[colorIndex]);


                //Final index
                index = flowColor[flowColor.Length - 1];
                row = index / _map.getCols();
                column = index % _map.getCols();
                auxOb = _board[row, column];
                gb = auxOb.GetComponent<GameBox>();
                gb.setFigureSprite(_sprites[0]);
                gb.setFigureColor(GameManager.instance.getSelectedSkin().colors[colorIndex]);

                colorIndex++;
            }
		}
	}

    private void checkBridges()
    {
        if (_map.getBridges() != null)
            for (int x = 0; x < _map.getBridges().Length; x++)
            {

            }
    }

    private void checkHollows()
    {
        if(_map.getHollows() != null)
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
        _levelManager.setLevelDone(false);
    }


    [SerializeField] Sprite[] _sprites;
    [SerializeField] GameObject gameBoxPrefab;
    [SerializeField] GameObject _grid;
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