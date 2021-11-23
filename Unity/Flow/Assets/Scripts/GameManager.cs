using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public Section[] GetSections() { return _sections; }
    public Skin[] GetSkins() { return _skins; }
    [SerializeField] Section[] _sections;
    [SerializeField] Skin[] _skins;
    public static GameManager instance { get; private set; } = null;

}
