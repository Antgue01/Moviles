using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            _saveData = new SaveDataManager();
            Application.wantsToQuit += _saveData.save;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            instance._levelManager = _levelManager;
            Destroy(this.gameObject);            
        }
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

        #if UNITY_EDITOR
        _currentSkin = selectedSkinDebug;
        #endif

        return _currentSkin;
    }


    public Section[] GetSections() { return _sections; }
    public Skin[] GetSkins() { return _skins; }
    public void setSelectedLevel(int lvl) { _selectedLevel = lvl; }
    [SerializeField] Section[] _sections;
    [SerializeField] Skin[] _skins;
    [SerializeField] LevelManager _levelManager;
    public static GameManager instance { get; private set; } = null;
    LevelLot _selectedLevelLot;
    Section _selectedSection;
    int _selectedLevel;
    Skin _currentSkin;
    SaveDataManager _saveData;

#if UNITY_EDITOR
    [SerializeField] Section selectedSectionDebug;
        [SerializeField] LevelLot selectedLevelLotDebug;
        [SerializeField] int selectedLevelDebug;
        [SerializeField] Skin selectedSkinDebug;
    #endif
}
