using System;
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

        string noSplitLot = GameManager.instance.getSelectedLot().LevelLotFile.ToString();
        string[] separators = { "\n", "\r", "\r\n", "\n\r" };
        _lot = noSplitLot.Split(separators, System.StringSplitOptions.RemoveEmptyEntries);

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
        _userIsWatchingVideo = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    /* ------------------------------------------------------ UINTERFACE INFO ---------------------------------------------------*/
    public void setLevelText()
    {
        _levelText.text = "Level " + (_currentLevel + 1);
    }

    public void setSizeText(int rows, int cols)
    {
        _sizeText.text = cols + "x" + rows;
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
        if (_bestMovements == -1)
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
        if (_isLevelDone)
        {
            showEndMenu();
            _levelDoneText.text = "You complete the level in " + _boardManager.getMovements() + " moves.";
            GameManager.instance.setSelectedLevel(_currentLevel);
            GameManager.instance.UpdateLevel(_boardManager.getMovements());
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
        _bestMovements = GameManager.instance.getBestMoves(_currentLevel);
        setBestMovementsText();
        _remainingHints = GameManager.instance.getRemainingHints();
        setRemainingHintsText();
        checkLevelCompleted();
        checkPreviousNextButtons();
    }

    private void checkPreviousNextButtons()
    {
        if (_currentLevel == 0)
        {
            _previousLevelImage.color = Color.gray;
            _previousLevelButton.transition = Selectable.Transition.None;

            _nextLevelImage.color = Color.white;
            _nextLevelButton.transition = Selectable.Transition.SpriteSwap;
        }
        else if (_currentLevel == _lot.Length - 1 || (_currentLevel + 1 < _lot.Length && !GameManager.instance.isUnlockedLevel(_currentLevel + 1)))
        {
            _previousLevelImage.color = Color.white;
            _previousLevelButton.transition = Selectable.Transition.SpriteSwap;

            _nextLevelImage.color = Color.gray;
            _nextLevelButton.transition = Selectable.Transition.None;
        }
        else
        {
            _previousLevelImage.color = Color.white;
            _previousLevelButton.transition = Selectable.Transition.SpriteSwap;

            _nextLevelImage.color = Color.white;
            _nextLevelButton.transition = Selectable.Transition.SpriteSwap;
        }
    }

    /* ------------------------------------------------------ BUTTON ACTIONS -----------------------------------------------------*/

    public void goToSelectionLotScene()
    {
        //load previous scene
    }

    public void showHintMenu()
    {
        _boardManager.setOnMenu(true);
        _hintsMenu.SetActive(true);
    }

    public void closeHintMenu()
    {
        _boardManager.setOnMenu(false);
        _hintsMenu.SetActive(false);
    }

    public void userIsWatchingVideo()
    {
        _userIsWatchingVideo = true;
    }

    public void watchVideo()
    {
        if (_userIsWatchingVideo)
        {
            _userIsWatchingVideo = false;
            Debug.Log("Watch video");
            closeHintMenu();
            _remainingHints++;
            setRemainingHintsText();
            GameManager.instance.updateNumHints(_remainingHints);
        }
        
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

    public void showEndMenu()
    {
        _endMenu.SetActive(true);
        _boardManager.setOnMenu(true);
    }

    public void closeEndMenu()
    {
        _endMenu.SetActive(false);
        _boardManager.setOnMenu(false);
    }

    public void nextLevelFromMenu()
    {
        nextLevel();
        closeEndMenu();
    }

    public void resetLevel()
    {
        _boardManager.resetLevel();
        _bestMovements = GameManager.instance.getBestMoves(_currentLevel);
        setBestMovementsText();
    }

    public void nextLevel()
    {
        if (_currentLevel + 1 < _lot.Length && GameManager.instance.isUnlockedLevel(_currentLevel + 1))
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
            translationY = (gridHeight * scale) / 2;
        }
        else
        {
            //fit in height
            scale = freeHeight / gridHeight;
            translationX = (gridWidth * scale) / 2;
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
    [SerializeField] Image _previousLevelImage;
    [SerializeField] Image _nextLevelImage;
    [SerializeField] Button _previousLevelButton;
    [SerializeField] Button _nextLevelButton;
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
    private bool _userIsWatchingVideo;
}
