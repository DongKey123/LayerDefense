using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroMainPanelController : UIBaseController
{
    private IntroMainPanel introMainPanelView = null;


    private const string homeSceneName = "HomeScene";

    protected override void Awake()
    {
        base.Awake();

        //Button 설정
        introMainPanelView = this.GetComponent<IntroMainPanel>();
        introMainPanelView.SetStartButton(GameStart);
    }

    private void GameStart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(homeSceneName);
    }
}
