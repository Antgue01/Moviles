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
        _lastMovementFlowId = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (_levelDone) return;

        if (_map != null && _flowsConnected == _map.getTotalFlows())
        {
            _levelDone = true;
            foreach (GameObject StaticPoint in _flowStartAndEndPoints)
            {
                StaticPoint.GetComponent<GameBoxAnimController>().grow(); 
            }
            _levelManager.setLevelDone(true);

        }

        HandleInput();

        _pruebaCoordenadas.text = "row: " + _inputTilePos.x + ", col: " + _inputTilePos.y;
        _pruebaCanvasSize.text = "W: " + _canvasRT.rect.width + ", H: " + _canvasRT.rect.height;
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
        Vector2 inputPosition;
        bool justDown, justUp;
#if UNITY_EDITOR
        inputPosition = Input.mousePosition;
        justDown = Input.GetMouseButtonDown(0);
        justUp = Input.GetMouseButtonUp(0);
#else
        Touch myTouch = Input.GetTouch(0);
        inputPosition = myTouch.position;
        justDown = myTouch.phase == TouchPhase.Began;
        justUp = myTouch.phase == TouchPhase.Ended;
#endif

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

            if (currentType == GameBox.BoxType.Empty ||
                currentType == GameBox.BoxType.FlowPoint)
            {
                Vector2Int direction = new Vector2Int(_inputTileRowCol.y - lastInputRowCol.y,
                _inputTileRowCol.x - lastInputRowCol.x);

                if (_currentFlowSelected.addTile(currentGameBox, lastInputRowCol, direction, _board))
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

        for(int x = 0; x < _flows.Length; x++)
        {
            _flows[x].disconfirmTiles();
        }

        //Reset info
        resetInfo();
        setUIinfo();
    }

    public void useHint()
    {
        //Do hint
    }

    public void updateFlowsConnected(int add)
    {
        if (add > 0 && _flowsConnected < _map.getTotalFlows() || add < 0 && _flowsConnected > 0)
        {
            _flowsConnected += add;
            _levelManager.setFlowsText(_flowsConnected, _map.getTotalFlows());
            float pipe = ((float)_flowsConnected / (float) _map.getTotalFlows()) * 100.0f;
            _levelManager.setPipeText((int)pipe);
        }            
    }

    public void flowConfirmTile(int flowId)
    {
        if(_lastMovementFlowId != flowId)
        {
            _movements++;
            _levelManager.setMovementsText(_movements);
            _lastMovementFlowId = flowId;
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

    private void setUIinfo()
    {
        _levelManager.setSizeText(Rows, Cols);
        _levelManager.setFlowsText(_flowsConnected, _map.getTotalFlows());
        _levelManager.setPipeText(_pipe);
        _levelManager.setMovementsText(_movements);
    }

    //------------------------------------------------------------------------------------------------------------------------------------//
    //-------------------------------------------------------------------------------------------------------- MAP INSTANCE--------------//
    //----------------------------------------------------------------------------------------------------------------------------------//
    public void loadMap(Map m)
    {
        _map = m;
        resetInfo();
        configureBoard();
        setUIinfo();
    }

    private void configureBoard()
    {   
        if(_board != null)
            for (int row = 0; row < _board.GetLength(0); ++row)
                for (int col = 0; col < _board.GetLength(1); col++)
                    Destroy(_board[row, col]);

        if (_map != null && _grid != null)
        {
            Rows = _map.getRows();
            Cols = _map.getCols();
            _flowStartAndEndPoints = new GameObject[2 * _map.getTotalFlows()];
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
                    //go.GetComponent<GameBox>().setFigureImageSize(boxWidth, boxHeight);
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
        int numFlows = _map.getTotalFlows();
        Flow.setMapNumFlows((uint)numFlows);
        _flows = new Flow[numFlows];

        if (flows != null)
        {
            int colorIndex = 0;
            foreach (int[] flowColor in flows)
            {
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
                _flowStartAndEndPoints[2 * colorIndex] = auxOb;
                GameBox tempGB = gb;

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
                _flowStartAndEndPoints[2 * colorIndex + 1] = auxOb;

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




    [SerializeField] Sprite[] _sprites;
    [SerializeField] GameObject gameBoxPrefab;
    [SerializeField] RectTransform _canvasRT;
    [SerializeField] GameObject _grid;
    private LevelManager _levelManager;

    Vector2Int _inputTilePos = Vector2Int.zero;


    private string[] _lot;
    private int _currentLevel;

    private Map _map;
    private GameObject[,] _board;

    private int _flowsConnected;
    private int _movements;
    private int _pipe;
    private bool _levelDone;
    private InputTransformer _transformer;

    private bool _pressed;
    private GameObject _lastPressed;
    private Vector2Int _inputTileRowCol;
    public int Rows { get; private set; }
    public int Cols { get; private set; }

    //PRUEBAS, BORRAR AL TERMINAR
    [SerializeField] Text _pruebaCoordenadas;
    [SerializeField] Text _pruebaCanvasSize;
    [SerializeField] GameObject _cursor;
    [SerializeField] Camera _cam;
    GameObject[] _flowStartAndEndPoints;
    private Flow[] _flows;
    private Flow _currentFlowSelected;
    private int _lastMovementFlowId;
}