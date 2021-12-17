using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        _transformer = new InputTransformer();
        _lastModifiedMapFlowId = -1;
        _onMenu = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_levelDone) return;

        if (_map != null && _flowsConnected == _map.getTotalFlows())
        {
            _levelDone = true;
            foreach (GameObject StaticPoint in _flowPointsBox)
            {
                StaticPoint.GetComponent<GameBoxAnimController>().grow();
            }
            _levelManager.setLevelDone(true);

        }

        if(!_onMenu)
            HandleInput();
    }

    //------------------------------------------------------------------------------------------------------------------------------------//
    //--------------------------------------------------------------------------------------------------------INPUT  --------------------//
    //----------------------------------------------------------------------------------------------------------------------------------//
    public GameObject getTileFromInput(Vector2 inputPosition)
    {
        _inputTilePos = _transformer.getTilePos(inputPosition, _grid.transform, Rows, Cols);
        if (_inputTilePos.x == -1 || _inputTilePos.y == -1) return null;
        _inputTileRowCol = _inputTilePos;
        return _board[_inputTileRowCol.x, _inputTileRowCol.y];
    }

    private void HandleInput()
    {
        Vector2 inputPosition = Vector2.one * -1;
        bool justDown=false, justUp=false;
#if UNITY_EDITOR
        inputPosition = Input.mousePosition;
        justDown = Input.GetMouseButtonDown(0);
        justUp = Input.GetMouseButtonUp(0);

#else
        if (Input.touchCount > 0)
        {

            Touch myTouch = Input.GetTouch(0);
            inputPosition = myTouch.position;
            justDown = myTouch.phase == TouchPhase.Began;
            justUp = myTouch.phase == TouchPhase.Ended;
        }
#endif
        if (inputPosition.x != -1)
        {

            if (_cursor.activeSelf)
            {
                Vector2 inputPosToWorld = _cam.ScreenToWorldPoint(inputPosition);
                _cursor.transform.position = inputPosToWorld;
            }

            //Just pressed
            if (justDown && !_pressed)
            {
                GameObject currentTile = getTileFromInput(inputPosition);
                //Check if it is a valid Tile
                if (currentTile == null) return;

                GameBox currentGameBox = currentTile.GetComponent<GameBox>();
                //Chcek if is flow or flow point, so we could drag
                if (currentGameBox.getFlow() == null) return;
                //Start dragging
                else
                {
                    _currentFlowSelected = currentGameBox.getFlow();
                    _pressed = true;
                    _lastPressed = currentTile;
                    _cursor.SetActive(true);
                    _cursor.GetComponent<SpriteRenderer>().color = _currentFlowSelected.GetColor();
                    _currentFlowSelected.startDragging(currentGameBox);
                }
            }
            else if (justUp && _pressed)
            {
                endInput();
            }
            //While dragging
            else if (_pressed)
            {
                Vector2Int lastInputRowCol = _inputTileRowCol;
                GameObject currentTile = getTileFromInput(inputPosition);
                //Check if it is a valid Tile
                if (currentTile == null)
                {
                    endInput();
                    return;
                }
                //Check if it is the same Tile
                if (currentTile == _lastPressed) return;

                GameBox currentGameBox = currentTile.GetComponent<GameBox>();
                GameBox lastGameBox = _lastPressed.GetComponent<GameBox>();

                GameBox.BoxType currentType = currentGameBox.getType();

                if (currentType == GameBox.BoxType.Empty || currentType == GameBox.BoxType.FlowPoint)
                {
                    Vector2Int direction = new Vector2Int(_inputTileRowCol.y - lastInputRowCol.y,
                                                          _inputTileRowCol.x - lastInputRowCol.x);

                    if (_currentFlowSelected.addTile(currentGameBox, lastInputRowCol, direction))
                    {
                        _lastPressed = currentTile;
                    }
                    else
                    {
                        endInput();
                    }
                }
                else
                {
                    endInput();
                }
            }
        }
    }

    /// <summary>
    /// Finish and reset input
    /// </summary>
    private void endInput()
    {
        _pressed = false;
        _lastPressed = null;
        if (_currentFlowSelected != null)
        {
            _currentFlowSelected.stopDragging();
            _currentFlowSelected = null;
        }
        if (_cursor.activeSelf) _cursor.SetActive(false);
    }



    //------------------------------------------------------------------------------------------------------------------------------------//
    //--------------------------------------------------------------------------------------------------------UI GESTION ----------------//
    //----------------------------------------------------------------------------------------------------------------------------------//

    public void setLevelManager(LevelManager lvlManager)
    {
        _levelManager = lvlManager;
    }

    public void resetLevel()
    {
        for (int x = 0; x < _map.getRows(); ++x)
            for (int j = 0; j < _map.getCols(); ++j)
                _board[x, j].GetComponent<GameBox>().restore();

        for (int x = 0; x < _flows.Length; x++)
            _flows[x].resetFlow();

        //Reset info
        resetInfo();
        setUIinfo();
    }

    public void useHint()
    {
        bool hasCandidate = false;
        for (int x = 0; x < _flows.Length && !hasCandidate; x++)
        {
            if (!_flows[x].getUsedHintInThisFlow())
            {
                if (_flows[x].useHintOnFlow())
                {
                    _levelManager.substractRemainingHint();
                    hasCandidate = true;
                }
            }
        }
    }

    public void updateFlowsConnected(int add)
    {
        if (add > 0 && _flowsConnected < _map.getTotalFlows() || add < 0 && _flowsConnected > 0)
        {
            _flowsConnected += add;
            _levelManager.setFlowsText(_flowsConnected, _map.getTotalFlows());
        }
    }

    public void updatePipesNumber(int add)
    {
        if (_pipes + add >= 0 && _pipes + add <= _totalInitiallyEmpty)
        {
            _pipes += add;
            _levelManager.setPipeText((int)((_pipes/_totalInitiallyEmpty) * 100.0f));
        }
    }

    /// <summary>
    /// Called if map has been modified
    /// </summary>
    /// <param name="flowId"></param>
    public void mapModified(int flowId)
    {
        if (_lastModifiedMapFlowId != flowId)
        {
            _movements++;
            _levelManager.setMovementsText(_movements);
            _lastModifiedMapFlowId = flowId;
        }
    }

    public int getMovements()
    {
        return _movements;
    }

    private void resetInfo()
    {
        _lastModifiedMapFlowId = -1;
        _levelDone = false;
        _movements = 0;
        _flowsConnected = 0;
        _pipes = 0;
        _totalInitiallyEmpty = (Cols * Rows) - (_flows.Length * 2);
        _levelManager.setLevelDone(false);
    }

    private void setUIinfo()
    {
        _levelManager.setSizeText(Rows, Cols);
        _levelManager.setFlowsText(_flowsConnected, _map.getTotalFlows());
        _levelManager.setPipeText((int)_pipes);
        _levelManager.setMovementsText(_movements);
    }

    //------------------------------------------------------------------------------------------------------------------------------------//
    //-------------------------------------------------------------------------------------------------------- MAP INSTANCE--------------//
    //----------------------------------------------------------------------------------------------------------------------------------//
    public void loadMap(Map m)
    {
        _map = m;
        configureBoard();
        resetInfo();
        setUIinfo();
    }

    private void configureBoard()
    {
        if (_board != null)
            for (int row = 0; row < _board.GetLength(0); ++row)
                for (int col = 0; col < _board.GetLength(1); col++)
                    Destroy(_board[row, col]);

        if (_map != null && _grid != null)
        {
            Rows = _map.getRows();
            Cols = _map.getCols();
            _flowPointsBox = new GameObject[2 * _map.getTotalFlows()];
            _board = new GameObject[Rows, Cols];
            Color sectionColor = GameManager.instance.getSelectedSection().themeColor;
            for (int row = 0; row < Rows; ++row)
            {
                for (int col = 0; col < Cols; ++col)
                {                   
                    GameObject go = Instantiate(gameBoxPrefab, _grid.transform) as GameObject;
                    go.transform.localPosition = new Vector3(col + .5f, -row - .5f, 0);
                    go.GetComponent<SpriteRenderer>().color = sectionColor;
                    go.GetComponent<GameBox>().setType(GameBox.BoxType.Empty);
                    go.GetComponent<GameBox>().initDirs();

                    //go.GetComponent<GameBox>().setFigureImageSize(boxWidth, boxHeight);
                    _board[row, col] = go;
                }
            }
            createFlowPoints();
            checkHollows();
            checkWalls();
            //checkBridges();
        }

    }

    /// <summary>
    /// Create the start and end point for every color flow.
    /// </summary>
    private void createFlowPoints()
    {
        int[][] flows = _map.getFlows();
        int numFlows = _map.getTotalFlows();
        Flow.setMapNumFlows((uint)numFlows);
        _flows = new Flow[numFlows];

        if (flows != null)
        {
            int colorIndex = 0;
            foreach (int[] flowColor in flows)
            {
                GameBox[] starEndPoints = new GameBox[2];
                _flows[colorIndex] = new Flow(this);
                //Start index
                int index = flowColor[0];
                int row = index / Cols;
                int column = index % Cols;
                GameObject auxOb = _board[row, column];
                GameBox gb = auxOb.GetComponent<GameBox>();
                
                gb.setType(GameBox.BoxType.FlowPoint);
                gb.setFigureSprite(_sprites[0]);
                gb.setColor(GameManager.instance.getSelectedSkin().colors[colorIndex]);
                gb.setFlow(_flows[colorIndex]);
                _flowPointsBox[2 * colorIndex] = auxOb;
                starEndPoints[0] = gb;

                //Final index
                index = flowColor[flowColor.Length - 1];
                row = index / Cols;
                column = index % Cols;
                auxOb = _board[row, column];
                gb = auxOb.GetComponent<GameBox>();
                gb.setType(GameBox.BoxType.FlowPoint);
                gb.setFigureSprite(_sprites[0]);
                gb.setColor(GameManager.instance.getSelectedSkin().colors[colorIndex]);
                gb.setFlow(_flows[colorIndex]);
                _flowPointsBox[2 * colorIndex + 1] = auxOb;
                starEndPoints[1] = gb;

                _flows[colorIndex].setStartEndFlowPoints(starEndPoints);
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
        if (_map.getHollows() != null)
            for (int x = 0; x < _map.getHollows().Length; x++)
            {

            }
    }

    private void checkWalls()
    {
        if(_map.getWalls() != null)
            for(int x  = 0; x < _map.getWalls().Length; x++)
            {
                int tile1 = _map.getWalls()[x].Key;
                int tile2 = _map.getWalls()[x].Value;                

                int tile1Row = tile1 / _map.getCols();
                int tile1Col = tile1 % _map.getCols();
                int tile2Row = tile2 / _map.getCols();
                int tile2Col = tile2 % _map.getCols();

                _board[tile1Row, tile1Col].GetComponent<GameBox>().setInvalidDir(tile2 - tile1);
                _board[tile2Row, tile2Col].GetComponent<GameBox>().setInvalidDir(tile1 - tile2);

                _board[tile1Row, tile1Col].GetComponent<GameBox>().setWallActive(tile2 - tile1);
                _board[tile2Row, tile2Col].GetComponent<GameBox>().setWallActive(tile1 - tile2);
            }
    }

    public GameObject[,] getBoard() { return _board; }
    public Map getMap() { return _map; }
    public int Rows { get; private set; }
    public int Cols { get; private set; }
    public void setOnMenu(bool m) { _onMenu = m; }

    [SerializeField] Sprite[] _sprites;
    [SerializeField] GameObject gameBoxPrefab;
    [SerializeField] GameObject _grid;
    [SerializeField] GameObject _cursor;
    [SerializeField] Camera _cam;

    //MAP
    private Map _map;
    private LevelManager _levelManager;
    private GameObject[,] _board;

    //INFO
    private int _flowsConnected;
    private int _movements;
    private float _pipes;
    private float _totalInitiallyEmpty;
    private bool _levelDone;

    //INPUT
    private InputTransformer _transformer;
    private bool _onMenu;
    private bool _pressed;
    private GameObject _lastPressed;
    private Vector2Int _inputTileRowCol;
    Vector2Int _inputTilePos = Vector2Int.zero;
    

    //FLOWS
    GameObject[] _flowPointsBox;
    private Flow[] _flows;
    private Flow _currentFlowSelected;
    private int _lastModifiedMapFlowId;
}