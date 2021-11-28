using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        _boardManager.setLevelManager(this);       
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

    /**-- */
    public void startGame(string selectedLot, int selectedLevel)
    {
        _mapParser = new MapParser();

        _lot = selectedLot.Split('\n');
        _currentLevel = selectedLevel;
        if (_currentLevel >= 0 && _currentLevel < _lot.Length)
        {
            Map m = _mapParser.createLevelMap(_lot[_currentLevel]);
            _boardManager.loadMap(m);
            updateButtonsInfo(m);
            checkLevelCompleted();
        }

        _isLevelDone = false;
    }

    /* ------------------------------------------------------ UINTERFACE INFO ---------------------------------------------------*/
    public void setFlowsText(int howManyFlows)
    {
        /*flows.text = howManyFlows + "/" + _totalFlows;*/ 
    }

    public void setMovementsText(int howManyMov)
    {
        /*movemments.text = howManyMov;*/
    }    

    public void setPipeText(int howMany)
    {
        /*pipe.text = howMany;*/
    }

    public void levelDone()
    {
        _isLevelDone = true;
    }

    private void checkLevelCompleted()
    {
        //Comprobar si el _currentLevel estaba ya completo y mostrar estrella, si no est� completado y la estrella est� visible ocultarla      
    }
    private void setRemainingHintsText()
    {
        /*pistas.text = _remainingHints;;*/
    }

    private void updateButtonsInfo(Map m)
    {
        /*name.text = nombreDelLote*/
        /*nivel.text = _currentLevel;*/
        //bestMovements.text = comprobar en el progreso los mejroes movimientos
        _totalFlows = m.getTotalFlows();
        setFlowsText(0);
        setPipeText(0);
        setMovementsText(0);
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
            updateButtonsInfo(m);
        }
    }

    public void previousLevel()
    {
        if (_currentLevel > 0)
        {
            Map m = _mapParser.createLevelMap(_lot[_currentLevel]);
            _currentLevel--;
            _boardManager.loadMap(_mapParser.createLevelMap(_lot[_currentLevel]));
            updateButtonsInfo(m);
        }       
    }

    public void useHint()
    {
        if (_remainingHints > 0)
        {
            _remainingHints--;                     
            _boardManager.useHint();
            setRemainingHintsText();
        }       
    }

    [SerializeField] BoardManager _boardManager;
    private MapParser _mapParser;

    private string[] _lot;
    private bool _isLevelDone;
    private int _currentLevel;
    private int _totalFlows;
    private int _remainingHints;
}
