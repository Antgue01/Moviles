using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
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
            updateUIInfo();
            AdjustGridToScreen();
        }

        _isLevelDone = false;
        _userIsWatchingVideo = false;
    }

    /* ------------------------------------------------------ UINTERFACE INFO ---------------------------------------------------*/
    #region UI INFO
    public void setLevelText()
    {
        _levelText.text = "level " + (_currentLevel + 1);
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
            GameManager.instance.UpdateLevel(_boardManager.getMovements(), _map.getTotalFlows());
            GameManager.instance.save();
        }
    }

    private void setRemainingHintsText()
    {
        _hintText.text = _remainingHints + "X";
    }

    private void setLevelCompletedImage()
	{
        _levelCompletedImg.gameObject.SetActive(true);
        _levelCompletedImg.sprite = (GameManager.instance.getIsPerfect(_currentLevel)) ? _starSprite : _tickSprite;
	}

    public void updateUIInfo()
    {
        _levelText.color = GameManager.instance.getSelectedSection().themeColor;
        setLevelText();
        _bestMovements = GameManager.instance.getBestMoves(_currentLevel);
        setBestMovementsText();
        _remainingHints = GameManager.instance.getRemainingHints();
        setRemainingHintsText();
        if (GameManager.instance.isLevelCompleted(_currentLevel))
            setLevelCompletedImage();
        else _levelCompletedImg.gameObject.SetActive(false);
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
    #endregion
    /* ------------------------------------------------------ BUTTON ACTIONS -----------------------------------------------------*/
    #region BUTTON ACTIONS

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
            closeHintMenu();
            _remainingHints++;
            setRemainingHintsText();
            GameManager.instance.updateNumHints(_remainingHints);
            GameManager.instance.save();
        }
    }

    public void useHint()
    {
        if (_remainingHints > 0)
        {
            _boardManager.useHint();
            GameManager.instance.save();
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
            updateUIInfo();
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
            updateUIInfo();
            AdjustGridToScreen();
        }
    }
    #endregion

    /// <summary>
    /// Adjust grids position and scale
    /// </summary>
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

    [Tooltip("BoardManager reference.")]
    [SerializeField] BoardManager _boardManager;
    [Tooltip("End Menu UI.")]
    [SerializeField] GameObject _endMenu;
    [Tooltip("Hints Menu UI")]
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
    [SerializeField] Transform _grid;
    [Tooltip("Main Camera reference.")]
    [SerializeField] Camera _cam;
    [SerializeField] Image _levelCompletedImg;
    [SerializeField] Sprite _starSprite;
    [SerializeField] Sprite _tickSprite;

    private MapParser _mapParser;
    Map _map;

    private string[] _lot;
    private bool _isLevelDone;
    private int _currentLevel;
    private int _bestMovements;
    private int _remainingHints;
    private bool _userIsWatchingVideo;
}