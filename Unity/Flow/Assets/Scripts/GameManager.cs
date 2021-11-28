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
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            instance._levelManager = _levelManager;
            Destroy(this.gameObject);            
        }

        #if UNITY_EDITOR
                instance._levelManager.startGame(selectedLevelLotDebug.ToString(), selectedLevelDebug);
        #endif
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
        return _selectedSection;
    }

    public LevelLot getSelectedLot()
    {
        return _selectedLevelLot;
    }

    public int getSelectedLevel()
    {
        return _selectedLevel;
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


    #if UNITY_EDITOR
        [SerializeField] Section selectedSectionDebug;
        [SerializeField] LevelLot selectedLevelLotDebug;
        [SerializeField] int selectedLevelDebug;
    #endif
}
