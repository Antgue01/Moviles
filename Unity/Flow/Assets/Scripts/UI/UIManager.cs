using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public void switchSceneFromUI(GameManager.SceneEnum scene)
	{
		GameManager.instance.SwitchSceneTo(scene);
	}
}
