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
        else if(GameManager.instance.isUnlockedLevel(n-1))
        {
            
            GameManager.instance.setSelectedLevel(n-1);
            GameManager.instance.SwitchSceneTo(GameManager.SceneEnum.Game);
        }
    }
}
