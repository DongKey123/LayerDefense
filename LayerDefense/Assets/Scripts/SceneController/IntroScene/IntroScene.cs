using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroScene : BaseSceneController
{
    private const string defaultPanelPath = "Intro/MainPanel";

    private void Awake()
    {
        UIManager.getInstance.Show<IntroMainPanelController>(defaultPanelPath);
    }

}   
