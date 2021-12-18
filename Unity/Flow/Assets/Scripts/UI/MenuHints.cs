using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuHints : MonoBehaviour
{
    public void close() { _hintsMenu.SetActive(false); }

    [SerializeField] GameObject _hintsMenu;
    [SerializeField] LevelManager _lvlManager;
}
