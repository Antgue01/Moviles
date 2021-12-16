using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSwitchButton : MonoBehaviour
{
    public void onClickSwitchScene()
	{
        _myUIManager.switchSceneFromUI(_sceneToSwitch);
	}

    [SerializeField]
    private GameManager.SceneEnum _sceneToSwitch;

    [SerializeField]
    private UIManager _myUIManager;
}
