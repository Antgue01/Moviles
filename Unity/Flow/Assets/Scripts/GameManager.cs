using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    void Awake()
    {
        if (instance == null)
        {
            _adManager = new AdManager();
            _banner = new BannerAd(_secondsToNextAd);
            _adManager.init(_banner);
            _adManager.setBannerPosition(BannerPosition.BOTTOM_CENTER);
            instance = this;
            _saveData = new SaveDataManager();
            Application.wantsToQuit += _saveData.save;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            _adManager = instance._adManager;
            _banner = instance._banner;
            _selectedLevel = instance._selectedLevel;
            _selectedLevelLot = instance._selectedLevelLot;
            _selectedSection = instance._selectedSection;
            _selectedSkin = instance._selectedSkin;
#if UNITY_EDITOR
            instance.selectedLevelDebug = selectedLevelDebug;
            instance.selectedLevelLotDebug = selectedLevelLotDebug;
            instance.selectedSectionDebug = selectedSectionDebug;
            instance._selectedSkin = _selectedSkin;
#endif
            Destroy(this.gameObject);
        }
    }



    public bool isUnlockedLevel(int lv)
    {
        KeyValuePair<int, int> data = getSaveInfo(_selectedSection, _selectedLevelLot);
        bool isInRange = lv >= 0 && lv < _saveData.sections[data.Key].levelLots[data.Value].bestMovesPerLevel.Length;
        if (!isInRange)
            Debug.LogWarning("Level out of range. isUnlockedLevel will return false");
        return isInRange &&
            (_saveData.sections[data.Key].levelLots[data.Value].lastUnlockedLevel >= lv || _selectedLevelLot.UnlockAll);
    }

    public bool isLevelCompleted(int lv)
    {
        KeyValuePair<int, int> data = getSaveInfo(_selectedSection, _selectedLevelLot);
        bool isInRange = lv >= 0 && lv < _saveData.sections[data.Key].levelLots[data.Value].bestMovesPerLevel.Length;
        if (!isInRange)
            Debug.LogWarning("Level out of range. IsLevelCompleted will return false");
        return isInRange && _saveData.sections[data.Key].levelLots[data.Value].bestMovesPerLevel[lv] > -1;
    }
    public int getBestMoves(int lv)
    {
        KeyValuePair<int, int> data = getSaveInfo(_selectedSection, _selectedLevelLot);
        bool isInRange = lv >= 0 && lv < _saveData.sections[data.Key].levelLots[data.Value].bestMovesPerLevel.Length;
        if (!isInRange)
        {
            Debug.LogWarning("Level out of range");
            return -1;
        }
        return _saveData.sections[data.Key].levelLots[data.Value].bestMovesPerLevel[lv];

    }
    public int getNumPlayedLevels(Section section, LevelLot levellot)
    {
        KeyValuePair<int, int> data = getSaveInfo(section, levellot);

        return _saveData.sections[data.Key].levelLots[data.Value].playedLevels;
    }

    public int getRemainingHints()
    {
        return _saveData.numHints;
    }

    /// <summary>
    /// Gets the last completed level from a certain level lot in a section
    /// </summary>
    /// <param name="section">Section we will search the level from</param>
    /// <param name="lvlot">level lot we will search the level from</param>
    /// <returns>The last level (beginning from 0) we completed</returns>
    public int getLastCompletedLevel(Section section, LevelLot lvlot)
    {

        KeyValuePair<int, int> data = getSaveInfo(section, lvlot);
        return _saveData.sections[data.Key].levelLots[data.Value].lastUnlockedLevel - 1;
    }
    KeyValuePair<int, int> getSaveInfo(Section s, LevelLot l)
    {
        int i = 0;
        while (_saveData.sections[i].name != s.SectionName)
        {
            i++;
        }
        int j = 0;
        while (_saveData.sections[i].levelLots[j].name != l.LevelLotName)
        {
            j++;
        }
        return new KeyValuePair<int, int>(i, j);
    }

    public void goToLevelSelection(LevelLot myLevelLot, Section mySection)
    {
        _selectedSection = mySection;
        _selectedLevelLot = myLevelLot;

    }

    public void SwitchSceneTo(SceneEnum scene)
    {
        SceneManager.LoadScene((int)scene);
    }

    public Section getSelectedSection()
    {
        //#if UNITY_EDITOR
        //        _selectedSection = selectedSectionDebug;
        //#endif
        return _selectedSection;
    }

    public LevelLot getSelectedLot()
    {
        //#if UNITY_EDITOR
        //        _selectedLevelLot = selectedLevelLotDebug;
        //#endif
        return _selectedLevelLot;
    }

    public int getSelectedLevel()
    {
        //#if UNITY_EDITOR
        //        _selectedLevel = selectedLevelDebug;
        //#endif
        return _selectedLevel;
    }

    public Skin getSelectedSkin()
    {



        return _selectedSkin;
    }


    public Section[] GetSections() { return _sections; }
    public Skin[] GetSkins() { return _skins; }
    public void setSelectedLevel(int lvl) { _selectedLevel = lvl; }
    public void updateNumHints(int numHints)
    {
        _saveData.numHints = numHints;
    }

    public AdManager GetAdManager() { return _adManager; }
    [SerializeField] Section[] _sections;
    [SerializeField] Skin[] _skins;

    /// <summary>
    /// updates the current level moves if we beat the record. If the current level does not have a 
    /// record yet, creates the record
    /// </summary>
    /// <param name="moves">The number of moves we did on the current level</param>

    public void UpdateLevel(int moves)
    {
        //we search our level
        KeyValuePair<int, int> data = getSaveInfo(_selectedSection, _selectedLevelLot);
        //if we have beaten our record, we update it ya. 
        int bestMoves = _saveData.sections[data.Key].levelLots[data.Value].bestMovesPerLevel[_selectedLevel];
        if (bestMoves == -1 || bestMoves > moves)
        {
            _saveData.sections[data.Key].levelLots[data.Value].bestMovesPerLevel[_selectedLevel] = moves;
            if (bestMoves == -1)
                _saveData.sections[data.Key].levelLots[data.Value].playedLevels++;
        }
        //if we just completed the last level we unlocked we unlock the next one
        if (!_selectedLevelLot.UnlockAll)
        {
            int lastLevel = _saveData.sections[data.Key].levelLots[data.Value].lastUnlockedLevel;
            string[] separators = { "\n", "\r", "\r\n", "\n\r" };
            int numLevels = _selectedLevelLot.LevelLotFile.ToString().Split(separators, StringSplitOptions.RemoveEmptyEntries).Length;
            if (lastLevel == _selectedLevel && lastLevel < numLevels)
                _saveData.sections[data.Key].levelLots[data.Value].lastUnlockedLevel++;

        }
    }

    [SerializeField] LevelManager _levelManager;
    [SerializeField] float _secondsToNextAd;
    [SerializeField] Skin _selectedSkin;
    public static GameManager instance { get; private set; } = null;
    LevelLot _selectedLevelLot;
    Section _selectedSection;
    int _selectedLevel;
    SaveDataManager _saveData;
    AdManager _adManager;
    BannerAd _banner;
#if UNITY_EDITOR
    [SerializeField] Section selectedSectionDebug;
    [SerializeField] LevelLot selectedLevelLotDebug;
    [SerializeField] int selectedLevelDebug;
#endif

    public enum SceneEnum
    {
        MainMenu = 0,
        LevelLotSelector = 1,
        LevelSelector = 2,
        Game = 3
    }

}
