using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        _boardManager.setLevelManager(this);
        _mapParser = new MapParser();

        _lot = GameManager.instance.getSelectedLot().LevelLotFile.ToString().Split('\n');
        _currentLevel = GameManager.instance.getSelectedLevel();
        if (_currentLevel >= 0 && _currentLevel < _lot.Length)
        {
            _map = _mapParser.createLevelMap(_lot[_currentLevel]);
            _boardManager.loadMap(_map);
            AdjustGridToScreen();
            updateButtonsInfo();
            checkLevelCompleted();
        }

        _isLevelDone = false;
        _bestMovements = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isLevelDone)
        {
            //Block buttons

            //Show win pannel menu
        }
    }

    /* ------------------------------------------------------ UINTERFACE INFO ---------------------------------------------------*/
    public void setLevelText()
    {
        _levelText.text = "Level " + _currentLevel;
    }

    public void setSizeText(int rows, int cols)
    {
        _sizeText.text = rows + "x" + cols;
    }

    public void setFlowsText(int howManyFlows, int totalFlows)
    {
        _flowText.text = "flows: " + howManyFlows + "/" + totalFlows;
    }

    public void setMovementsText(int howManyMov)
    {
        _moveText.text = "moves: " + howManyMov + "";
    }

    public void setBestMovementsText()
    {
        if (_bestMovements == 0)
            _bestText.text = "best: -";
        else _bestText.text = "best: " + _bestMovements;
    }

    public void setPipeText(int howMany)
    {
        _pipeText.text = "pipe: " + howMany + "%";
    }

    public void setLevelDone(bool isDone)
    {
        _isLevelDone = isDone;
        if(_isLevelDone)
        {
            _endMenu.SetActive(true);
            _levelDoneText.text = "You complete the level in " + _boardManager.getMovements() +" moves.";
            GameManager.instance.UpdateLevel(_bestMovements);
        }       
    }

    private void checkLevelCompleted()
    {
        //Comprobar si el _currentLevel estaba ya completo y mostrar estrella, si no est� completado y la estrella est� visible ocultarla      
    }
    private void setRemainingHintsText()
    {
        _hintText.text = _remainingHints + "X";
    }

    private void updateButtonsInfo()
    {
        setLevelText();
        setBestMovementsText();
        setRemainingHintsText();
        checkLevelCompleted();
    }

    /* ------------------------------------------------------ BUTTON ACTIONS -----------------------------------------------------*/

    public void goToSelectionLotScene()
    {
        //load previous scene
    }


    public void showHintMenu()
    {
        _hintsMenu.SetActive(true);
    }

    public void watchVideo()
    {
        //do watch video

        _hintsMenu.SetActive(false);
        _remainingHints++;
        setRemainingHintsText();
        GameManager.instance.updateNumHints(_remainingHints);
    }

    public void useHint()
    {
        if (_remainingHints > 0)
        {            
            _boardManager.useHint();     
        }
    }

    public void substractRemainingHint()
    {
        _remainingHints--;
        setRemainingHintsText();        
        GameManager.instance.updateNumHints(_remainingHints);
    }

    public void resetLevel()
    {
        _boardManager.resetLevel();
    }

    public void nextLevel()
    {
        if (_currentLevel < _lot.Length)
        {
            _currentLevel++;
            _map = _mapParser.createLevelMap(_lot[_currentLevel]);
            _boardManager.loadMap(_map);
            updateButtonsInfo();
        }
    }

    public void previousLevel()
    {
        if (_currentLevel > 0)
        {
            _currentLevel--;
            _map = _mapParser.createLevelMap(_lot[_currentLevel]);
            _boardManager.loadMap(_map);
            updateButtonsInfo();
        }
    }


    void AdjustGridToScreen()
    {

        float height = _cam.orthographicSize * 2;
        float width = (height * _cam.aspect);
        //we substract 1 because of the origin point
        float topSize = (1 - _UITop.anchorMin.y) * height + _topOffset;
        float botSize = (1 - (1 - _UIBot.anchorMax.y)) * height + _botOffset;
        //we calculate the height by substracting the top lowest point and the bottom upper point from the original height
        float gridHeight = height - botSize - topSize;
        //we calculate the rule of threet to check if it would fit

        double newH = _map.getRows() * width / _map.getCols();
        double newW = _map.getCols() * gridHeight / _map.getRows();
        float translationX = 0;
        float translationY = 0;
        //If scaling the Y it wouldn't fit
        float scale = 1;
        if (newH >= gridHeight)
        {
            //Scale factor
            scale = gridHeight / _map.getRows();
            translationX = (_map.getCols() * scale) / 2;
            translationY = gridHeight / 2;
        }
        else if (newW >= width)
        {
            //Scale factor
            scale = width / _map.getCols();
            translationX = width / 2;
            translationY = (_map.getRows() * scale) / 2;
        }
        /*todo probar con un tablero en horizontal. Tengo la corazonada de que en ese caso habría que intercambiar translationx
        y translationY*/
        _grid.transform.localScale = Vector3.one * scale;
        _grid.transform.Translate(new Vector3(-translationX, translationY, 0));
    } 


    [SerializeField] BoardManager _boardManager;
    [SerializeField] GameObject _endMenu;
    [SerializeField] GameObject _hintsMenu;
    [SerializeField] Text _levelText;
    [SerializeField] Text _sizeText;
    [SerializeField] Text _flowText;
    [SerializeField] Text _moveText;
    [SerializeField] Text _bestText;
    [SerializeField] Text _pipeText;
    [SerializeField] Text _hintText;
    [SerializeField] Text _levelDoneText;
    [SerializeField] RectTransform _UITop;
    [SerializeField] RectTransform _UIBot;
    [SerializeField] float _topOffset;
    [SerializeField] float _botOffset;
    [SerializeField] Transform _grid;
    [SerializeField] Camera _cam;
    private MapParser _mapParser;
    Map _map;

    private string[] _lot;
    private bool _isLevelDone;
    private int _currentLevel;
    private int _bestMovements;
    private int _remainingHints;
}
