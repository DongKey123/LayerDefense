using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PVEGamePanelController : UIBaseController
{
    //Interaction
    [SerializeField] private CardElementController[] cards = null;
    [SerializeField] private Button rorollButton = null;
    [SerializeField] private Button addIncomeButton = null;

    //

    //Caching
    private PVEGamePanel pveGamePanelView = null;
    private InGameDataManager inGameData = null;
    private CardManager cardManager = null;
    private CharacterManager characterManager = null;

    //Data
    private Dictionary<int, CardData> cardDictionary = new Dictionary<int, CardData>();
    private CardElementController curSelectCardController = null;

    //Delegate
    public Action<Card> OnSelectCard = null;

    protected override void Awake()
    {
        base.Awake();

        inGameData = InGameDataManager.getInstance;
        cardManager = CardManager.getInstance;
        characterManager = CharacterManager.getInstance;

        //Todo: Move to BootScene
        cardDictionary = cardManager.CardDictionary;


        pveGamePanelView = this.GetComponent<PVEGamePanel>();
        rorollButton.onClick.AddListener(ReRoll);
        addIncomeButton.onClick.AddListener(AddRecome);
        for(int i = 0; i < cards.Length; i++)
        {
            cards[i].OnClick += OnClickCard;
        }
    }

    public override void Show()
    {
        base.Show();

        SetUpCard();
        SetMaxHP(inGameData.HP);
    }

    #region Income
    private void AddRecome()
    {
        inGameData.InCome += inGameData.ADD_RECOME;
        UpdateIncomeData();
    }

    private void ReRoll()
    {
        inGameData.InCome -= inGameData.REROLL_COST;
        SetUpCard();
        UpdateIncomeData();
    }


    public void UpdateIncomeData()
    {
        int income = inGameData.InCome;

        bool buttonEnable = income >= inGameData.REROLL_COST;

        pveGamePanelView.SetIncomeMoney(inGameData.InCome);
        pveGamePanelView.SetEnableIncomeButton(buttonEnable);

        int count = cards.Length;
        for (int i = 0; i < count; i++)
        {
            cards[i].UpdateCard();
        }
    }

    #endregion

    #region Card
    public void OnClickCard(CardElementController cardController)
    {
        bool isSelectSameObject = (curSelectCardController == cardController);
        curSelectCardController?.SetSelectCard(false);
        
        curSelectCardController = isSelectSameObject ? null : cardController;
        curSelectCardController?.SetSelectCard(true);

        var card = curSelectCardController?.GetCard();
        OnSelectCard?.Invoke(card);
    }

    public void SetCardInMap()
    {
        if (curSelectCardController == null)
            return;

        curSelectCardController.SetCardOnMap(false);
    }

    public void SetUpCard()
    {
        int count = cards.Length;
        for (int i = 0; i < count; i++)
        {
            int key = cardManager.GetRandomCardKey();

            cards[i].SetUpCard(cardDictionary[key]);
        }
    }
    #endregion

    #region Monster

    /// <summary>
    /// Todo: 몬스터 관련 정보 수정 시 체인 걸기
    /// </summary>
    public void UpdateViewMonsterData(int count)
    {
        pveGamePanelView.SetMonsterCount(count);
    }

    public void AddViewMonsterCount()
    {
        pveGamePanelView.AddMonsterCount();
    }

    public void RemoveViewMonsterCount()
    {
        pveGamePanelView.RemoveMonsterCount();
    }
    #endregion

    public void SetMaxHP(int hp)
    {
        pveGamePanelView.SetMaxHp(hp);
    }

    public void SetHP(int hp)
    {
        pveGamePanelView.SetHp(hp);
    }
}
