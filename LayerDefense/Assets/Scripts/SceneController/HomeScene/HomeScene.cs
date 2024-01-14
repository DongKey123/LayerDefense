using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeScene : BaseSceneController
{
    private const string defaultPanelPath = "Home/MainPanel";

    private void Awake()
    {
        UIManager.getInstance.Show<HomeMainPanelController>(defaultPanelPath);
    }


    
}
