using System;
using UnityEngine;
using UnityEngine.UI;

public enum EStageUIState
{
    Complete,
    Current,
    Default,
    Lock
}

public class StagePanelController : UIBaseController
{
    //Temp Data
    public int curClearedStageNum = 2;
    //End TempData


    private StagePanelModel stagePanelModel = null;
    private StagePanel stagePanelView = null;

    private const string gameScene = "GameScene";


    [SerializeField] private StageElement[] gameStartButtons = null;
    [SerializeField] private Button prevPageButton = null;
    [SerializeField] private Button nextPageButton = null;
    [SerializeField] private Button backButton = null;

    private int stagePage = 0;
    private int maxStagePage = 0;

    private int stageCount = 0;
    private int stageCurStars = 0;
    private int stageMaxStars = 0;

    private const int stageStarCount = 3;
    private const int stageButtonCount = 18;

    //Cashing
    private StageManager stageManager = null;

    protected override void Awake()
    {
        base.Awake();

        stageManager = StageManager.getInstance;

        stageCount = stageManager.StageDictionary.Count;
        stageMaxStars = stageMaxStars * stageStarCount;
        maxStagePage = stageCount / stageButtonCount + 1;

        //View Setting
        stagePanelModel = GetComponent<StagePanelModel>();
        stagePanelView = GetComponent<StagePanel>();
        int buttonCount = gameStartButtons.Length;
        for (int i = 0; i < buttonCount; i++)
        {
            gameStartButtons[i].StageButton.onClick.AddListener(() =>
            {
                GameStart(i);
            });
        }
        backButton.onClick.AddListener(Hide);
    }

    public override void Show()
    {
        base.Show();

        InitData();
        SetStageButtons();
    }

    private void InitData()
    {
        stagePage = 0;
    }

    private void SetStageButtons()
    {
        if (stagePage < 1)
        {
            prevPageButton.gameObject.SetActive(false);
            nextPageButton.gameObject.SetActive(true);
        }
        else if (stagePage < maxStagePage)
        {
            prevPageButton.gameObject.SetActive(true);
            nextPageButton.gameObject.SetActive(false);
        }
        else
        {
            prevPageButton.gameObject.SetActive(true);
            nextPageButton.gameObject.SetActive(true);
        }

        for (int i = 0; i < stageButtonCount; i++)
        {
            int curStage = i + (stagePage * stageButtonCount);
            if (curStage < curClearedStageNum)
            {
                gameStartButtons[i].SetButton(
                    EStageUIState.Complete,
                    GetStageButtonSprite(EStageUIState.Complete),
                    curStage ,
                    3// temp
                    );
            }
            else if (curStage == curClearedStageNum)
            {
                gameStartButtons[i].SetButton(
                    EStageUIState.Current,
                    GetStageButtonSprite(EStageUIState.Current),
                    curStage);
            }
            else if (curStage > curClearedStageNum)
            {
                if (curStage < stageCount)
                {
                    gameStartButtons[i].SetButton(
                        EStageUIState.Default,
                        GetStageButtonSprite(EStageUIState.Default),
                        curStage);

                }
                else
                {
                    gameStartButtons[i].SetButton(
                        EStageUIState.Lock,
                        GetStageButtonSprite(EStageUIState.Lock),
                        curStage);
                }
            }
        }
    }

    private void GameStart(int stage)
    {
        stageManager.SetStageIndex(stage);

        UnityEngine.SceneManagement.SceneManager.LoadScene(gameScene);
    }

    private Sprite GetStageButtonSprite(EStageUIState uiState)
    {
        switch (uiState)
        {
            case EStageUIState.Complete:
                return stagePanelModel.stageCompletedSprite;
            case EStageUIState.Current:
                return stagePanelModel.stageCurrentSprite;
            case EStageUIState.Default:
                return stagePanelModel.stageDefaultSprite;
            case EStageUIState.Lock:
                return stagePanelModel.stageLockSprite;
            default:
                throw new NotImplementedException($"unhandled switch case: {uiState}");
        }
    }
}