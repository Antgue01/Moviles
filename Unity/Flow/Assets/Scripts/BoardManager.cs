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

        //Just pressed
        if (Input.GetMouseButtonDown(0) && !_pressed)
        {
            GameObject currentTile = getTileFromInput();
            //Check if it is a valid Tile
            if (currentTile == null) return;

            GameBox currentGameBox = currentTile.GetComponent<GameBox>();
            //Chcek if is flow or flow point, so we could drag
            if (currentGameBox.getType() != GameBox.BoxType.Flow &&
                currentGameBox.getType() != GameBox.BoxType.FlowPoint) return;
            //Start dragging
            else
            {
                _pressed = true;
                _lastPressed = currentTile;
                currentGameBox.cutFromThisTile();
            }
        }
        else if (Input.GetMouseButtonUp(0) && _pressed)
        {
            _pressed = false;
            _lastPressed = null;
        }
        //While dragging
        else if (_pressed)
		{
            Vector2Int lastInputRowCol = _inputTileRowCol;
            GameObject currentTile = getTileFromInput();
            //Check if it is a valid Tile
            if (currentTile == null)
			{
                _pressed = false;
                _lastPressed = null;
                return;
			}
            //Check if it is not the same Tile
            if (currentTile == _lastPressed) return;

            GameBox currentGameBox = currentTile.GetComponent<GameBox>();
            GameBox lastGameBox = _lastPressed.GetComponent<GameBox>();

            if (currentGameBox.getType() == GameBox.BoxType.Empty ||
                currentGameBox.getType() == GameBox.BoxType.Bridge)
			{
                if(connectGameBox(lastGameBox, currentGameBox, lastInputRowCol))
				{
                    _lastPressed = currentTile;
                }
				else
				{
                    _pressed = false;
                    _lastPressed = null;
                }
			}
            else if(currentGameBox.getType() == GameBox.BoxType.Flow ||
                currentGameBox.getType() == GameBox.BoxType.FlowPoint)
			{
                //GameBox with the same color
                if (currentGameBox.getFigureColor() == lastGameBox.getFigureColor())
				{
                    _lastPressed = currentTile;
                    //New start of this flow, cut from here and continue
                    if (currentGameBox.getNextGB() != null)
                        currentGameBox.cutFromThisTile();
                    //Connect to a flow point as final
					else
					{
                        //If we cannot connect GameBox
                        if (!connectGameBox(lastGameBox, currentGameBox, lastInputRowCol))
						{
                            _pressed = false;
                            _lastPressed = null;
                        }
                    }
                }
			}
			else
			{
                _pressed = false;
                _lastPressed = null;
                return;
            }
        }

        _pruebaCoordenadas.text = "row: " + _inputTilePos.x + ", col: " + _inputTilePos.y;
        _pruebaCanvasSize.text = "W: " + _canvasRT.rect.width + ", H: " + _canvasRT.rect.height;

    }

    /// <summary>
    /// Tries to connect two tiles, treating diagonal case
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
        to.setType(GameBox.BoxType.Flow);
        to.setFigureColor(from.getFigureColor());
        to.setPathFrom(dir);
    }

    public GameObject getTileFromInput()
	{
        _inputTilePos = _transformer.getTilePos(Input.mousePosition, _grid.transform, Rows, Cols);
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
                _board[x, j].GetComponent<GameBox>().reset();


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
                    go.GetComponent<GameBox>().setInitType(GameBox.BoxType.Empty);
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
                gb.setInitType(GameBox.BoxType.FlowPoint);
                gb.setFigureSprite(_sprites[0]);
                gb.setFigureColor(GameManager.instance.getSelectedSkin().colors[colorIndex]);

                GameBox tempGB = gb;

                //Final index
                index = flowColor[flowColor.Length - 1];
                row = index / Cols;
                column = index % Cols;
                auxOb = _board[row, column];
                gb = auxOb.GetComponent<GameBox>();
                gb.setInitType(GameBox.BoxType.FlowPoint);
                gb.setFigureSprite(_sprites[0]);
                gb.setFigureColor(GameManager.instance.getSelectedSkin().colors[colorIndex]);

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
    private Vector2Int _inputTileRowCol;
    public int Rows { get; private set; }
    public int Cols { get; private set; }

    //PRUEBAS, BORRAR AL TERMINAR
    [SerializeField] Text _pruebaCoordenadas;
    [SerializeField] Text _pruebaCanvasSize;
}