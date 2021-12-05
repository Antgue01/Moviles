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
#if UNITY_EDITOR
        Vector2 input = Vector2.zero;
        if (Input.GetMouseButtonDown(0))
        {
            input = _transformer.getInputPos(Input.mousePosition, _grid.transform);
        }
#else
        Vector2 input= _transformer.getInputPos(Input.GetTouch(0);
#endif
        _pruebaCoordenadas.text = "x: " + input.x + ", y: " + input.y;
        _pruebaCanvasSize.text = "W: " + _canvasRT.rect.width + ", H: " + _canvasRT.rect.height;

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
                int row = index / Cols;
                int column = index % Cols;
                GameObject auxOb = _board[row, column];
                GameBox gb = auxOb.GetComponent<GameBox>();
                gb.setFigureSprite(_sprites[0]);
                gb.setFigureColor(GameManager.instance.getSelectedSkin().colors[colorIndex]);


                //Final index
                index = flowColor[flowColor.Length - 1];
                row = index / Cols;
                column = index % Cols;
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
    public int Rows { get; private set; }
    public int Cols { get; private set; }

    //PRUEBAS, BORRAR AL TERMINAR
    [SerializeField] Text _pruebaCoordenadas;
    [SerializeField] Text _pruebaCanvasSize;
}