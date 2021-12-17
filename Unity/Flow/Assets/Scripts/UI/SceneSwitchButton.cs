using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSwitchButton : MonoBehaviour
{
    public void onClickSwitchScene()
    {
        GameManager.instance.SwitchSceneTo(_sceneToSwitch);
    }

    [SerializeField]
    private GameManager.SceneEnum _sceneToSwitch;
}
