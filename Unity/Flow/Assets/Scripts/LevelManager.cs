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
            Map m = _mapParser.createLevelMap(_lot[_currentLevel]);
            _boardManager.loadMap(m);
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
        _moveText.text = "moves: " + howManyMov +"";
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
        GameManager.instance.UpdateLevel(_bestMovements);
            
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

    public void watchVideo()
    {
        //do watch video

        _remainingHints++;
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
            Map m = _mapParser.createLevelMap(_lot[_currentLevel]);
            _boardManager.loadMap(_mapParser.createLevelMap(_lot[_currentLevel]));
            updateButtonsInfo();
        }
    }

    public void previousLevel()
    {
        if (_currentLevel > 0)
        {
            _currentLevel--;
            Map m = _mapParser.createLevelMap(_lot[_currentLevel]);            
            _boardManager.loadMap(_mapParser.createLevelMap(_lot[_currentLevel]));
            updateButtonsInfo();
        }       
    }

    public void useHint()
    {
        if (_remainingHints > 0)
        {
            _remainingHints--;
            GameManager.instance.updateNumHints(_remainingHints);
            _boardManager.useHint();
            setRemainingHintsText();
        }       
    }

    [SerializeField] BoardManager _boardManager;
    [SerializeField] Text _levelText;
    [SerializeField] Text _sizeText;
    [SerializeField] Text _flowText;
    [SerializeField] Text _moveText;
    [SerializeField] Text _bestText;
    [SerializeField] Text _pipeText;
    [SerializeField] Text _hintText;
    private MapParser _mapParser;

    private string[] _lot;
    private bool _isLevelDone;
    private int _currentLevel;
    private int _bestMovements;
    private int _remainingHints;
}
