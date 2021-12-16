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
            _adManager.init();
            _banner = new BannerAd(_secondsToNextAd);
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

    /// <summary>
    /// Gets the last completed level from a certain level lot in a section
    /// </summary>
    /// <param name="section">Section we will search the level from</param>
    /// <param name="lvlot">level lot we will search the level from</param>
    /// <returns>The last level (beginning from 0) we completed</returns>
    public int getLastCompletedLevel(Section section, LevelLot lvlot)
    {

        int i = 0;
        while (_saveData.sections[i].name != section.SectionName)
        {
            i++;
        }
        int j = 0;
        while (_saveData.sections[i].levelLots[j].name != lvlot.LevelLotName)
        {
            j++;
        }
        return _saveData.sections[i].levelLots[j].lastUnlockedLevel - 1;
    }

    public void goToLevelSelection(LevelLot myLevelLot, Section mySection)
    {
        _selectedSection = mySection;
        _selectedLevelLot = myLevelLot;

    }
    public void ChangeScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public Section getSelectedSection()
    {
#if UNITY_EDITOR
        _selectedSection = selectedSectionDebug;
#endif
        return _selectedSection;
    }

    public LevelLot getSelectedLot()
    {
#if UNITY_EDITOR
        _selectedLevelLot = selectedLevelLotDebug;
#endif
        return _selectedLevelLot;
    }

    public int getSelectedLevel()
    {
#if UNITY_EDITOR
        _selectedLevel = selectedLevelDebug;
#endif
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
    private void Update()
    {
        _banner.Update();
    }
    public void UpdateLevel(int moves)
    {
        //we search our level
        int i = 0;
        while (_saveData.sections[i].name != _selectedSection.SectionName)
        {
            i++;
        }
        int j = 0;
        while (_saveData.sections[i].levelLots[j].name != _selectedLevelLot.LevelLotName)
        {
            j++;
        }
        //if we have beaten our record, we update it
        if (_saveData.sections[i].levelLots[j].bestMovesPerLevel.Count == _selectedLevel)
            _saveData.sections[i].levelLots[j].bestMovesPerLevel.Add(moves);
        else if (_saveData.sections[i].levelLots[j].bestMovesPerLevel[_selectedLevel] > moves)
            _saveData.sections[i].levelLots[j].bestMovesPerLevel[_selectedLevel] = moves;
        //if we just completed the last level we unlocked we unlock the next one
        int lastLevel = _saveData.sections[i].levelLots[j].lastUnlockedLevel;
        string[] separators = { "\n", "\r", "\r\n", "\n\r" };
        int numLevels = _selectedLevelLot.LevelLotFile.ToString().Split(separators, StringSplitOptions.RemoveEmptyEntries).Length;
        if (lastLevel == _selectedLevel && lastLevel < numLevels)
            _saveData.sections[i].levelLots[j].lastUnlockedLevel++;

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
}
