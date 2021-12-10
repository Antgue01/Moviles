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
    }

    // Update is called once per frame
    void Update()
    {
        if (_levelDone) return;

        if (_map != null && _flowsConnected == _map.getTotalFlows())
        {
            _levelDone = true;
            _levelManager.setLevelDone(true);
        }

        HandleInput();

        _pruebaCoordenadas.text = "row: " + _inputTilePos.x + ", col: " + _inputTilePos.y;
        _pruebaCanvasSize.text = "W: " + _canvasRT.rect.width + ", H: " + _canvasRT.rect.height;

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
            Vector2 inputPosToWorld = Camera.main.ScreenToWorldPoint(inputPosition);
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
            if (!currentGameBox.getPathActive() &&
                currentGameBox.getType() != GameBox.BoxType.FlowPoint) return;
            //Start dragging
            else
            {
                _pressed = true;
                _lastPressed = currentTile;
                _cursor.SetActive(true);
                _cursor.GetComponent<SpriteRenderer>().color = currentGameBox.getColor();

                _lastFlowPointOrigin = (currentGameBox.getType() == GameBox.BoxType.FlowPoint) ? currentGameBox : currentGameBox.getOriginFlowPoint();
                _lastFlowPointOrigin.disconfirmFlows();
                currentGameBox.cutFromThisTile();
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

            if (currentType == GameBox.BoxType.Empty)
            {
                //No flow display in this Tile
				if (!currentGameBox.getPathActive())
				{
                    if (connectGameBox(lastGameBox, currentGameBox, lastInputRowCol))
                    {
                        _lastPressed = currentTile;
                    }
                    else
                    {
                        endInput();
                    }
                }
                //Path active in currentGameBox
				else
				{
                    //GameBox flow with the same color
                    if (currentGameBox.getPathColor() == lastGameBox.getPathColor())
					{
                        _lastPressed = currentTile;
                        //New start of this flow, cut from here and continue
                        if (currentGameBox.getNextGB() != null)
                            currentGameBox.cutFromThisTile();
                    }
                    //GameBox flow with different color
					else
					{
                        currentGameBox.hideConfirmedFromThisTile();
                        connectGameBox(lastGameBox, currentGameBox, lastInputRowCol);
                    }
                }
            }
            else if (currentType == GameBox.BoxType.FlowPoint)
            {
                //GameBox flow point with the same color
                if (currentGameBox.getColor() == lastGameBox.getPathColor())
                {
                    _lastPressed = currentTile;
                    //New start of this flow, cut from here and continue
                    if (currentGameBox.getNextGB() != null)
                        currentGameBox.cutFromThisTile();
                    //Connect to a flow point as final
                    else
                    {
                        connectGameBox(lastGameBox, currentGameBox, lastInputRowCol);
                        endInput();
                    }
                }
                //GameBox flow point with different color
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
		if (_lastFlowPointOrigin != null)
		{
            _lastFlowPointOrigin.confirmFlows();
            _lastFlowPointOrigin = null;
        }
        if (_cursor.activeSelf) _cursor.SetActive(false);
    }

    /// <summary>
    /// Tries to connect two tiles by flows, treating diagonal case
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="lastInputRowCol"></param>
    /// <returns></returns>
    public bool connectGameBox(GameBox from, GameBox to, Vector2Int lastInputRowCol)
    {
        GameBox lastGameBox = from;
        GameBox currentGameBox = to;
        //From last tile to current tile
        Vector2Int direction = new Vector2Int(_inputTileRowCol.y - lastInputRowCol.y,
            _inputTileRowCol.x - lastInputRowCol.x);

        //Diagonal case
        if (direction.x != 0 && direction.y != 0)
        {
            //First check with one of the components of direction
            GameBox auxGB = _board[lastInputRowCol.x + direction.y, lastInputRowCol.y].GetComponent<GameBox>();
            Vector2Int auxDir = new Vector2Int(0, direction.y);
            bool valid = (auxGB.getType() == GameBox.BoxType.Empty);
            //If not valid, we try again in another direction (with the other component of direction)
            if (!valid)
            {
                auxGB = _board[lastInputRowCol.x, lastInputRowCol.y + direction.x].GetComponent<GameBox>();
                auxDir = new Vector2Int(direction.x, 0);
                valid = (auxGB.getType() == GameBox.BoxType.Empty);
            }

            if (valid)
            {
                //We save the component not choosen at auxDir
                direction = (auxDir.x != 0) ? new Vector2Int(0, direction.y) : new Vector2Int(direction.x, 0);
                linkGameBox(lastGameBox, auxGB, auxDir);

                lastGameBox = auxGB;
            }
            //We cant treat diagonal case
            else
            {
                return false;
            }
        }

        linkGameBox(lastGameBox, currentGameBox, direction);

        return true;
    }

    /// <summary>
    /// Auxiliar method to link two GameBox with direction dir
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="dir"></param>
    public void linkGameBox(GameBox from, GameBox to, Vector2Int dir)
	{
        from.setNextGB(to);
        to.setPathColor(from.getPathColor());
        to.setPathFrom(dir);
    }

    public GameObject getTileFromInput(Vector2 inputPosition)
	{
        _inputTilePos = _transformer.getTilePos(inputPosition, _grid.transform, Rows, Cols);
        if (_inputTilePos.x == -1 || _inputTilePos.y == -1) return null;
        _inputTileRowCol = _inputTilePos;
        return _board[_inputTileRowCol.x, _inputTileRowCol.y];
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
        setUIinfo();
    }


    public void resetLevel()
    {
        for (int x = 0; x < _map.getRows(); ++x)
            for (int j = 0; j < _map.getCols(); ++j)
                _board[x, j].GetComponent<GameBox>().restore();


        //Reset info
        resetInfo();
    }

    public void useHint()
    {
        //Do hint
    }

    private void configureBoard()
    {
        if (_map != null && _grid != null)
        {
            //float boxWidth = _grid.GetComponent<RectTransform>().rect.width / _map.getCols();
            //float boxHeight = _grid.GetComponent<RectTransform>().rect.width / _map.getRows();
            Rows = _map.getRows();
            Cols = _map.getCols();
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


            if (Cols >= Rows)
            {
                //float scale = _canvasRT.rect.width / (_grid.GetComponent<GridLayoutGroup>().cellSize.x * _map.getCols());
                //_grid.GetComponent<RectTransform>().localScale = new Vector3(scale, scale, 1);

                //Hay que cambiar esta parte para reposicionar porque el cellsize despu�s de escalar sigue valiendo lo mismo que antes de escalar
                //float height = _map.getRows() * _grid.GetComponent<GridLayoutGroup>().cellSize.y * scale;
                //RectTransform gridRT = _grid.GetComponent<RectTransform>();
                //gridRT.transform.position = new Vector3(gridRT.position.x, height / 2 , gridRT.position.z);
            }
            else
            {
                //float scale = _canvasRT.rect.height / (_grid.GetComponent<GridLayoutGroup>().cellSize.y * _map.getRows());
                //_grid.GetComponent<RectTransform>().localScale = new Vector3(scale, scale, 1);

                ////Hay que cambiar esta parte para reposicionar porque el cellsize despu�s de escalar sigue valiendo lo mismo que antes de escalar
                //float height = _map.getRows() * _grid.GetComponent<GridLayoutGroup>().cellSize.y;
                //RectTransform gridRT = _grid.GetComponent<RectTransform>();
                //gridRT.position = new Vector3(gridRT.position.x, 500, gridRT.position.z);
            }
        }

    }

    //private void Canvas_preWillRenderCanvases()
    //{
    //    throw new System.NotImplementedException();
    //}

    /// <summary>
    /// Create the start and end point for every color flow.
    /// </summary>
    private void createFlowPoints()
    {
        int[][] flows = _map.getFlows();
        if (flows != null)
        {
            int colorIndex = 0;
            foreach (int[] flowColor in flows)
            {
                //Start index
                int index = flowColor[0];
                int row = index / Cols;
                int column = index % Cols;
                GameObject auxOb = _board[row, column];
                GameBox gb = auxOb.GetComponent<GameBox>();
                gb.setType(GameBox.BoxType.FlowPoint);
                gb.setFigureSprite(_sprites[0]);
                gb.setColor(GameManager.instance.getSelectedSkin().colors[colorIndex]);

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

                gb.setOtherFlowPoint(tempGB);
                tempGB.setOtherFlowPoint(gb);

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
        _levelManager.setFlowsText(0, _map.getTotalFlows());
        _levelManager.setPipeText(_pipe);
        _levelManager.setMovementsText(_movements);
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
    private List<Vector2>[] _flows;

    private int _flowsConnected;
    private int _movements;
    private int _pipe;
    private bool _levelDone;
    private InputTransformer _transformer;

    private bool _pressed;
    private GameObject _lastPressed;
    private GameBox _lastFlowPointOrigin;
    private Vector2Int _inputTileRowCol;
    public int Rows { get; private set; }
    public int Cols { get; private set; }

    //PRUEBAS, BORRAR AL TERMINAR
    [SerializeField] Text _pruebaCoordenadas;
    [SerializeField] Text _pruebaCanvasSize;
    [SerializeField] GameObject _cursor;
}