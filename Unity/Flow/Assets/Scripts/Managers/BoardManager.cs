using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    void Start()
    {
        _transformer = new InputTransformer();
        _lastModifiedMapFlowId = -1;
        _onMenu = false;
    }

    void Update()
    {
        if (_levelDone) return;

        if (_map != null && _flowsConnected == _map.getTotalFlows())
        {
            _levelDone = true;
            foreach (GameObject staticPoint in _flowPointsBox)
            {
                staticPoint.GetComponent<GameBoxAnimController>().grow();
            }
            _levelManager.setLevelDone(true);

        }

        if(!_onMenu)
            HandleInput();
    }

	#region INPUT
    /// <summary>
    /// Obtains the board tile from input position
    /// </summary>
    /// <param name="inputPosition"></param>
    /// <returns></returns>
	public GameBox getTileFromInput(Vector2 inputPosition)
    {
        _inputTilePos = _transformer.getTilePos(inputPosition, _grid.transform, Rows, Cols, _cam);
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

            if (_cursor.gameObject.activeSelf)
            {
                Vector2 inputPosToWorld = _cam.ScreenToWorldPoint(inputPosition);
                _cursor.transform.position = inputPosToWorld;
            }

            //Just pressed
            if (justDown && !_pressed)
            {
                GameBox currentTile = getTileFromInput(inputPosition);
                //Check if it is a valid Tile
                if (currentTile == null) return;

              
                //Start dragging
                else
                {
                    _currentFlowSelected = currentTile.getFlow();
                    _pressed = true;
                    _lastPressed = currentTile;
                    _cursor.color = _currentFlowSelected.GetColor();
                    Vector2 inputPosToWorld = _cam.ScreenToWorldPoint(inputPosition);
                    _cursor.transform.position = inputPosToWorld;
                    _cursor.gameObject.SetActive(true);
                    _currentFlowSelected.startDragging(currentTile);
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
                GameBox currentTile = getTileFromInput(inputPosition);
                //Check if it is a valid Tile
                if (currentTile == null)
                {
                    endInput();
                    return;
                }
                //Check if it is the same Tile
                if (currentTile == _lastPressed) return;

                GameBox currentGameBox = currentTile;
                GameBox lastGameBox = _lastPressed;

                GameBox.BoxType currentType = currentGameBox.getBoxType();

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
        if (_cursor.gameObject.activeSelf) _cursor.gameObject.SetActive(false);
    }

	#endregion

	#region UI MANAGMENT
	public void resetLevel()
    {
        for (int x = 0; x < _map.getRows(); ++x)
            for (int j = 0; j < _map.getCols(); ++j)
                _board[x, j].restore();

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
        _levelManager.updateUIInfo();
    }

	#endregion

	#region MAP MANAGMENT
	public void loadMap(Map m)
    {
        _map = m;
        configureBoard();
        resetInfo();
        setUIinfo();
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

    private void configureBoard()
    {
        if (_board != null)
            for (int row = 0; row < _board.GetLength(0); ++row)
                for (int col = 0; col < _board.GetLength(1); col++)
                    Destroy(_board[row, col].gameObject);

        if (_map != null && _grid != null)
        {
            Rows = _map.getRows();
            Cols = _map.getCols();
            _flowPointsBox = new GameObject[2 * _map.getTotalFlows()];
            _board = new GameBox[Rows, Cols];
            Color sectionColor = GameManager.instance.getSelectedSection().themeColor;
            for (int row = 0; row < Rows; ++row)
            {
                for (int col = 0; col < Cols; ++col)
                {                   
                    GameObject go = Instantiate(gameBoxPrefab, _grid.transform) as GameObject;
                    go.transform.localPosition = new Vector3(col + .5f, -row - .5f, 0);
                    sectionColor.a = 0.25f;
                    go.GetComponent<SpriteRenderer>().color = sectionColor;
                    GameBox box = go.GetComponent<GameBox>();
                    box.setType(GameBox.BoxType.Empty);
                    box.initWallDirsAndColor();
                    _board[row, col] = box;
                }
            }
            createFlowPoints();            
            createWalls();
            //checkBridges();
            createPlusB();
            createHollows();
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
                GameBox gb = _board[row, column];
                GameObject auxOb = gb.gameObject;
                
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
                gb = _board[row, column];
                auxOb = gb.gameObject;
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

    //private void checkBridges()
    //{
    //    if (_map.getBridges() != null)
    //        for (int x = 0; x < _map.getBridges().Length; x++)
    //        {

    //        }
    //}

    private void createHollows()
    {
        int currentTileRow = 0, currentTileCol = 0;

        if (_map.getHollows() != null)
        {
            for (int x = 0; x < _map.getHollows().Length; x++)
            {
                currentTileRow = _map.getHollows()[x] / _map.getCols();
                currentTileCol = _map.getHollows()[x] % _map.getCols();
                GameBox currentTile = _board[currentTileRow, currentTileCol];
                currentTile.setType(GameBox.BoxType.Hollow);
                currentTile.setActiveAllWalls(true);

                _board[currentTileRow, currentTileCol].GetComponent<SpriteRenderer>().enabled = false;

                //Check external walls
                if (_map.getPlusB())
                {
                    if (currentTileCol == 0) currentTile.setWallLeftActive(false);
                    else if (currentTileCol == _map.getCols() - 1) currentTile.setWallRightActive(false);

                    if (currentTileRow == 0) currentTile.setWallUpActive(false);
                    else if (currentTileRow == _map.getRows() - 1) currentTile.setWallDownActive(false);
                }
            }

            for (int x = 0; x < _map.getHollows().Length; x++)
            {
                currentTileRow = _map.getHollows()[x] / _map.getCols();
                currentTileCol = _map.getHollows()[x] % _map.getCols();
                GameBox currentTile = _board[currentTileRow, currentTileCol];

                //Check other hollows

                //Check right hollow
                int auxRow, auxCol;
                GameBox auxTile;
                if (currentTileCol < _map.getCols() - 1)
                {
                    auxRow = currentTileRow;
                    auxCol = currentTileCol + 1;
                    auxTile = _board[auxRow, auxCol];
                    if (auxTile.getBoxType() == GameBox.BoxType.Hollow)  //Current in the left of the last
                    {
                        currentTile.setWallRightActive(false);
                        auxTile.setWallLeftActive(false);
                    }
                }

                if(currentTileCol > 0)
                {
                    //Check left hollow
                    auxRow = currentTileRow;
                    auxCol = currentTileCol - 1;
                    auxTile = _board[auxRow, auxCol];
                    if (auxTile.getBoxType() == GameBox.BoxType.Hollow)  //Current in the right of the last
                    {
                        currentTile.setWallLeftActive(false);
                        auxTile.setWallRightActive(false);
                    }
                }
                
                if(currentTileRow > 0)
                {
                    //Check up hollow
                    auxRow = currentTileRow - 1;
                    auxCol = currentTileCol;
                    auxTile = _board[auxRow, auxCol];
                    if (auxTile.getBoxType() == GameBox.BoxType.Hollow)  //Current in the down of the last
                    {
                        currentTile.setWallUpActive(false);
                        auxTile.setWallDownActive(false);
                    }
                }
                
                if(currentTileRow < _map.getRows() - 1)
                {
                    //Check down hollow
                    auxRow = currentTileRow + 1;
                    auxCol = currentTileCol;
                     auxTile = _board[auxRow, auxCol];
                    if (auxTile.getBoxType() == GameBox.BoxType.Hollow) //Current in the up of the last
                    {
                        currentTile.setWallDownActive(false);
                        auxTile.setWallUpActive(false);
                    }
                }                
                
            }
        }

        
    }

    private void createWalls()
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

                _board[tile1Row, tile1Col].setInvalidDir(tile2 - tile1);
                _board[tile2Row, tile2Col].setInvalidDir(tile1 - tile2);

                _board[tile1Row, tile1Col].setWallActive(tile2 - tile1);
            }
    }

    private void createPlusB()
    {
        if (_map.getPlusB())
        {            
            //Left walls
            for (int row = 0; row < _map.getRows(); row++)
                _board[row, 0].setWallLeftActive(true);

            //Right walls
            for (int row = 0; row < _map.getRows(); row++)
                _board[row, _map.getCols() - 1].setWallRightActive(true);

            //Sup walls
            for (int col = 0; col < _map.getCols(); col++)
                _board[0, col].setWallUpActive(true);

            //Down walls
            for (int col = 0; col < _map.getCols(); col++)
                _board[_map.getRows() - 1, col].setWallDownActive(true);
        }
    }

	#endregion

	public void setLevelManager(LevelManager lvlManager) { _levelManager = lvlManager; }
    public int getMovements() { return _movements; }
    public GameBox[,] getBoard() { return _board; }
    public Map getMap() { return _map; }
    public int Rows { get; private set; }
    public int Cols { get; private set; }
    public void setOnMenu(bool m) { _onMenu = m; }

    [Tooltip("Main sprites for tiles.")]
    [SerializeField] Sprite[] _sprites;
    [Tooltip("GameBox Prefab.")]
    [SerializeField] GameObject gameBoxPrefab;
    [Tooltip("Grid GameObject reference.")]
    [SerializeField] GameObject _grid;
    [Tooltip("Cursor SpriteRenderer reference.")]
    [SerializeField] SpriteRenderer _cursor;
    [Tooltip("Main Camera reference.")]
    [SerializeField] Camera _cam;

    //MAP
    private Map _map;
    private LevelManager _levelManager;
    private GameBox[,] _board;

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
    private GameBox _lastPressed;
    private Vector2Int _inputTileRowCol;
    Vector2Int _inputTilePos = Vector2Int.zero;
    

    //FLOWS
    GameObject[] _flowPointsBox;
    private Flow[] _flows;
    private Flow _currentFlowSelected;
    private int _lastModifiedMapFlowId;
}