using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    [SerializeField] Text _number;
    public void SelectLevel()
    {
        int n;
        if (!int.TryParse(_number.text, out n))
            Debug.LogError("The level number is not a number");
        else
        {
            GameManager.instance.setSelectedLevel(n);
            GameManager.instance.SwitchSceneTo(GameManager.SceneEnum.Game);
        }
    }
}
