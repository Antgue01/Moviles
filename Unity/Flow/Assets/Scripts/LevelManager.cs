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
            AdjustGridToScreen();
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
            AdjustGridToScreen();
        }
    }


    void AdjustGridToScreen()
    {
        //From camera
        float height = _cam.orthographicSize * 2;
        float width = (height * _cam.aspect);

        //Every tile is 1 Unit
        float gridHeight = _map.getRows();
        float gridWidth = _map.getCols();

        //UI limits
        float topfreePosY = _UITop.TransformPoint(_UITop.anchoredPosition).y;
        float botfreePosY = _UIBot.TransformPoint(_UIBot.anchoredPosition + _UIBot.sizeDelta).y;

        //UI free space
        float freeHeight = Mathf.Abs(topfreePosY - botfreePosY);
        float freeWidth = width;
        //Apect in height
        float freeAspect = freeHeight / freeWidth;
        //Aspect in height
        float gridAspect = gridHeight / gridWidth;

        float scale = 1;
        float translationX = 0;
        float translationY = 0;

        if (freeAspect >= gridAspect)
		{
            //fit in width
            scale = freeWidth / gridWidth;
            translationX = freeWidth / 2;
            translationY = (gridWidth * scale) / 2;
        }
		else
		{
            //fit in height
            scale = freeHeight / gridHeight;
            translationX = (gridHeight * scale) / 2;
            translationY = freeHeight / 2;
        }

        //offset from centre of vertical ui free space
        float deltaVerticalCentre = topfreePosY - freeHeight / 2;
        _grid.transform.localScale = Vector3.one * scale;
        _grid.transform.position = new Vector3(-translationX, translationY + deltaVerticalCentre, 0);
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
