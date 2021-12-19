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
        if (instance != null)
        {
            instance._levelManager = _levelManager;
            instance._selectedSkin = _selectedSkin;
            Destroy(this.gameObject);
        }
        else
        {
            _adManager = new AdManager();
            _banner = new BannerAd();
            _adManager.init(_banner);
            instance = this;
            _saveData = new SaveDataManager();

#if UNITY_EDITOR
            if (selectedSectionDebug != null && selectedLevelLotDebug != null)
            {
                instance._selectedLevel = selectedLevelDebug;
                instance._selectedLevelLot = selectedLevelLotDebug;
                instance._selectedSection = selectedSectionDebug;
            }
#endif
            DontDestroyOnLoad(this.gameObject);
        }
    }
    public void save() { _saveData.save(); }

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

    public bool getIsPerfect(int lv)
    {
        KeyValuePair<int, int> data = getSaveInfo(_selectedSection, _selectedLevelLot);
        bool isInRange = lv >= 0 && lv < _saveData.sections[data.Key].levelLots[data.Value].bestMovesPerLevel.Length;
        if (!isInRange)
        {
            Debug.LogWarning("Level out of range.IsPerfect will return false");
        }
        return isInRange && _saveData.sections[data.Key].levelLots[data.Value].isPerfectPerLevel[lv];
    }
    public int getNumPlayedLevels(Section section, LevelLot levellot)
    {
        KeyValuePair<int, int> data = getSaveInfo(section, levellot);

        return _saveData.sections[data.Key].levelLots[data.Value].playedLevels;
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
    /// <summary>
    /// updates the current level moves if we beat the record. If the current level does not have a 
    /// record yet, creates the record
    /// </summary>
    /// <param name="moves">The number of moves we did on the current level</param>

    public void UpdateLevel(int moves, int totalFlows)
    {
        //we search our level
        KeyValuePair<int, int> data = getSaveInfo(_selectedSection, _selectedLevelLot);
        //if we have beaten our record, we update it ya. 
        int bestMoves = _saveData.sections[data.Key].levelLots[data.Value].bestMovesPerLevel[_selectedLevel];
        if (bestMoves == -1 || bestMoves > moves)
        {
            _saveData.sections[data.Key].levelLots[data.Value].isPerfectPerLevel[_selectedLevel] = moves == totalFlows;
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

    public void goToLevelSelection(LevelLot myLevelLot, Section mySection)
    {
        _selectedSection = mySection;
        _selectedLevelLot = myLevelLot;
    }

    public int getRemainingHints() { return _saveData.numHints; }
    public void SwitchSceneTo(SceneEnum scene) { SceneManager.LoadScene((int)scene); }
    public Section getSelectedSection() { return _selectedSection; }
    public LevelLot getSelectedLot() { return _selectedLevelLot; }
    public int getSelectedLevel() { return _selectedLevel; }
    public Skin getSelectedSkin() { return _selectedSkin; }
    public Section[] GetSections() { return _sections; }
    public Skin[] GetSkins() { return _skins; }
    public void setSelectedLevel(int lvl) { _selectedLevel = lvl; }
    public void updateNumHints(int numHints) { _saveData.numHints = numHints; }
    public AdManager GetAdManager() { return _adManager; }

    [SerializeField] Section[] _sections;
    [SerializeField] Skin[] _skins;

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
    [Tooltip("Section for debugging.")]
    [SerializeField] Section selectedSectionDebug;
    [Tooltip("LevelLot for debugging.")]
    [SerializeField] LevelLot selectedLevelLotDebug;
    [Tooltip("Level for debugging.")]
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
