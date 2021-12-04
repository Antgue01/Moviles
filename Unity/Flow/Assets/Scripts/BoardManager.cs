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

        if (_map != null && _flowsConnected == _map.getTotalFlows())
        {
            _levelDone = true;
            _levelManager.setLevelDone(true);
        }

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Pressed primary button.");
            // Debug.Log(Input.mousePosition);
            _pruebaCoordenadas.text = "x: " + Input.mousePosition.x + ", y: " + Input.mousePosition.y;
            //Debug.Log(_cam.ScreenToWorldPoint(Input.mousePosition));

            // Debug.Log("Screen Height : " + Screen.height);

            _pruebaCanvasSize.text = "W: " + _canvasRT.rect.width + ", H: " + _canvasRT.rect.height;
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

            _board = new GameObject[_map.getRows(), _map.getCols()];
            Color sectionColor = GameManager.instance.getSelectedSection().themeColor;
            for (int row = 0; row < _map.getRows(); ++row)
            {
                for (int col = 0; col < _map.getCols(); ++col)
                {
                    GameObject go = Instantiate(gameBoxPrefab, _grid.transform) as GameObject;
                    go.transform.localPosition = new Vector3(col + .5f, -row - .5f, 0);
                    go.GetComponent<SpriteRenderer>().color = sectionColor;
                    //go.GetComponent<GameBox>().setFigureImageSize(boxWidth, boxHeight);
                    _board[row, col] = go;

                }
            }
            adjustToScreen();
            createFlowPoints();
            checkHollows();
            checkBridges();


            if (_map.getCols() >= _map.getRows())
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

    private void Canvas_preWillRenderCanvases()
    {
        throw new System.NotImplementedException();
    }

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
        if (_map.getHollows() != null)
            for (int x = 0; x < _map.getHollows().Length; x++)
            {

            }
    }
    //todo esto ira en otra clase para ajustarlo cuando se cambie el size
    private void adjustToScreen()
    {
        float height = Camera.main.orthographicSize * 2;
        float width = (height * Camera.main.aspect);
        //we lessen 1 because of the origin point
        float topSize = (1 - _UITop.anchorMin.y) * height;
        float botSize = (1 - (1 - _UIBot.anchorMax.y)) * height;
        //we calculate the height by substracting the top lowest point and the bottom upper point from the original height
        float gridHeight = height - botSize - topSize;
        Debug.Log("top: " + topSize);
        Debug.Log("bot: " + botSize);
        Debug.Log("h: " + gridHeight);
        //Hacemos la regla de tres para ver si cabría

        //double newH = _logicHeight * frameW / _logicWidth;
        //double newW = _logicWidth * frameH / _logicHeight;

        ////Si escalando la Y no cabríamos

        //if (newH >= frameH)
        //{
        //    //Factor de escala
        //    _scaleY = frameH / (double)_logicHeight;
        //    _scaleX = _scaleY;
        //}
        //else if (newW >= frameW)
        //{
        //    //Factor de escala
        //    _scaleX = frameW / (double)_logicWidth;
        //    _scaleY = _scaleX;
        //}
        //_canvasWidth = _scaleX * _logicWidth;
        //_canvasHeight = _scaleY * _logicHeight;

        //_translationX = ((double)frameW - _canvasWidth) / 2;
        //_translationY = ((double)frameH - _canvasHeight) / 2;
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
        _levelManager.setSizeText(_map.getRows(), _map.getCols());
        _levelManager.setFlowsText(0, _map.getTotalFlows());
        _levelManager.setPipeText(_pipe);
        _levelManager.setMovementsText(_movements);
    }


    [SerializeField] Sprite[] _sprites;
    [SerializeField] GameObject gameBoxPrefab;
    [SerializeField] RectTransform _canvasRT;
    [SerializeField] GameObject _grid;
    [SerializeField] RectTransform _UITop;
    [SerializeField] RectTransform _UIBot;
    private LevelManager _levelManager;

    private string[] _lot;
    private int _currentLevel;

    private Map _map;
    private GameObject[,] _board;

    private int _flowsConnected;
    private int _movements;
    private int _pipe;
    private bool _levelDone;

    //PRUEBAS, BORRAR AL TERMINAR
    [SerializeField] Text _pruebaCoordenadas;
    [SerializeField] Text _pruebaCanvasSize;
}