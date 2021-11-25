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

            Destroy(this.gameObject);
        }
    }

    public void goToLevelSelection(LevelLot myLevelLot, Section mySection)
    {
        _selectedSection = mySection;
        _selectorLevelLot = myLevelLot;
    }
    public void ChangeScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public Section[] GetSections() { return _sections; }
    public Skin[] GetSkins() { return _skins; }
    [SerializeField] Section[] _sections;
    [SerializeField] Skin[] _skins;
    public static GameManager instance { get; private set; } = null;
    LevelLot _selectorLevelLot;
    Section _selectedSection;

}
