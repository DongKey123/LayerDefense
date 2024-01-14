using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CardElementController : MonoBehaviour
{
    [SerializeField] private Button uiButton = null;
    [SerializeField] private CardElementView cardButton = null;


    private CardData curCardData;
    public CardData CurCardData
    {
        get { return curCardData; }
    }
    private ECardSetType curSelectSetType = ECardSetType.Up;
    public ECardSetType CurSelectSetType
    {
        get { return curSelectSetType; }
    }


    public int cardCost;
    private CharacterData[] characterDatas = new CharacterData[6];
    
    private Card card = null;
    
    private CharacterManager characterManager;
    private InGameDataManager ingameDataManager;

    public Action<CardElementController> OnClick;

    private void Awake()
    {
        uiButton.onClick.AddListener(OnClickButton);
        characterManager = CharacterManager.getInstance;
        ingameDataManager = InGameDataManager.getInstance;
    }

    public void OnClickButton()
    {
        OnClick?.Invoke(this);
    }

    public void SetUpCard(CardData cardData)
    {
        card = null;

        card = new Card();
        card.Initialize(cardData);

        SetCardOnMap(true);
        SetCardView(card);
        UpdateCard();
    }

    public void SetSelectCard(bool isSelect)
    {
        cardButton.SetSelectCard(isSelect);
    }

    public void SetCardOnMap(bool isSetupCard)
    {
        if (isSetupCard == false)
            ingameDataManager.InCome -= cardCost;

        cardButton.SetMapCard(isSetupCard);


    }

    public void SetEnableCard(bool enable)
    {
        cardButton.SetEnableCard(enable);
    }

    public void UpdateCard()
    {
        bool enable = cardCost < ingameDataManager.InCome;
        SetEnableCard(enable);
    }

    public Card GetCard()
    {
        return card;
    }

    private void SetCardView(Card card)
    {
        curCardData = card.CardData;
        characterDatas = card.GetCharacterDatas();

        Sprite sp1 = characterManager.GetCharacterSprite(characterDatas[0]);
        Sprite sp2 = characterManager.GetCharacterSprite(characterDatas[1]);
        Sprite sp3 = characterManager.GetCharacterSprite(characterDatas[2]);
        Sprite sp4 = characterManager.GetCharacterSprite(characterDatas[3]);
        Sprite sp5 = characterManager.GetCharacterSprite(characterDatas[4]);
        Sprite sp6 = characterManager.GetCharacterSprite(characterDatas[5]);

        cardCost = 0;

        int count = characterDatas.Length;
        for(int i =0; i< count;i++)
        {
            if (characterDatas[i] == null)
                continue;

            cardCost += characterManager.HeroStatusDictionary[characterDatas[i].Character_ID].Cost;
        }

        bool enable = cardCost < ingameDataManager.InCome;
        SetEnableCard(enable);

        cardButton.SetData(sp1, sp2, sp3, sp4, sp5, sp6, cardCost);
    }


    //TEMP
    public bool GetIncludedTile(int x, int y)
    {
        switch (curSelectSetType)
        {
            case ECardSetType.Up:
                //Todo: 수정해야함 테이블을 수정해야할라...?
                if (x == 0 && y == 0)
                {
                    return curCardData.cell4 > 0; 
                }
                else if (x == 1 && y == 0)
                {
                    return curCardData.cell5 > 0;
                }
                else if (x == 2 && y == 0)
                {
                    return curCardData.cell6 > 0;
                }
                else if (x == 0 && y == 1)
                {
                    return curCardData.cell1 > 0;
                }
                else if (x == 1 && y == 1)
                {
                    return curCardData.cell2 > 0;
                }
                else if (x == 2 && y == 1)
                {
                    return curCardData.cell3 > 0;
                }
                break;
            case ECardSetType.Right:
                if (x == 0 && y == 0)
                {
                    return curCardData.cell6 > 0;
                }
                else if (x == 0 && y == 1)
                {
                    return curCardData.cell5 > 0;
                }
                else if (x == 0 && y == 2)
                {
                    return curCardData.cell4 > 0;
                }
                else if (x == 1 && y == 0)
                {
                    return curCardData.cell3 > 0;
                }
                else if (x == 1 && y == 1)
                {
                    return curCardData.cell2 > 0;
                }
                else if (x == 1 && y == 2)
                {
                    return curCardData.cell1 > 0;
                }
                break;
            case ECardSetType.Bottom:
                if (x == 0 && y == 0)
                {
                    return curCardData.cell3 > 0;
                }
                else if (x == 1 && y == 0)
                {
                    return curCardData.cell2 > 0;
                }
                else if (x == 2 && y == 0)
                {
                    return curCardData.cell1 > 0;
                }
                else if (x == 0 && y == 1)
                {
                    return curCardData.cell6 > 0;
                }
                else if (x == 1 && y == 1)
                {
                    return curCardData.cell5 > 0;
                }
                else if (x == 2 && y == 1)
                {
                    return curCardData.cell4 > 0;
                }
                break;
            case ECardSetType.Left:
                if (x == 0 && y == 0)
                {
                    return curCardData.cell1 > 0;
                }
                else if (x == 1 && y == 0)
                {
                    return curCardData.cell4 > 0;
                }
                else if (x == 0 && y == 1)
                {
                    return curCardData.cell2 > 0;
                }
                else if (x == 1 && y == 1)
                {
                    return curCardData.cell5 > 0;
                }
                else if (x == 0 && y == 2)
                {
                    return curCardData.cell3 > 0;
                }
                else if (x == 1 && y == 2)
                {
                    return curCardData.cell6 > 0;
                }
                break;
        }

        return false;
    }   
    public void Rotate()
    {
        switch (curSelectSetType)
        {
            case ECardSetType.Up:
                curSelectSetType = ECardSetType.Right;
                break;
            case ECardSetType.Right:
                curSelectSetType = ECardSetType.Bottom;
                break;
            case ECardSetType.Bottom:
                curSelectSetType = ECardSetType.Left;
                break;
            case ECardSetType.Left:
                curSelectSetType = ECardSetType.Up;
                break;
        }
    }
    
    public CharacterData GetCardData(int x,int y)
    {
        switch (curSelectSetType)
        {
            case ECardSetType.Up:
                //Todo: 수정해야함 테이블을 수정해야할라...?
                if (x == 0 && y == 0)
                {
                    return characterDatas[3];
                }
                else if (x == 1 && y == 0)
                {
                    return characterDatas[4];
                }
                else if (x == 2 && y == 0)
                {
                    return characterDatas[5];
                }
                else if (x == 0 && y == 1)
                {
                    return characterDatas[0];
                }
                else if (x == 1 && y == 1)
                {
                    return characterDatas[1];
                }
                else if (x == 2 && y == 1)
                {
                    return characterDatas[2];
                }
                break;
            case ECardSetType.Right:
                if (x == 0 && y == 0)
                {
                    return characterDatas[5];
                }
                else if (x == 0 && y == 1)
                {
                    return characterDatas[4];
                }
                else if (x == 0 && y == 2)
                {
                    return characterDatas[3];
                }
                else if (x == 1 && y == 0)
                {
                    return characterDatas[2];
                }
                else if (x == 1 && y == 1)
                {
                    return characterDatas[1];
                }
                else if (x == 1 && y == 2)
                {
                    return characterDatas[0];
                }
                break;
            case ECardSetType.Bottom:
                if (x == 0 && y == 0)
                {
                    return characterDatas[2];
                }
                else if (x == 1 && y == 0)
                {
                    return characterDatas[1];
                }
                else if (x == 2 && y == 0)
                {
                    return characterDatas[0];
                }
                else if (x == 0 && y == 1)
                {
                    return characterDatas[5];
                }
                else if (x == 1 && y == 1)
                {
                    return characterDatas[4];
                }
                else if (x == 2 && y == 1)
                {
                    return characterDatas[3];
                }
                break;
            case ECardSetType.Left:
                if (x == 0 && y == 0)
                {
                    return characterDatas[0];
                }
                else if (x == 1 && y == 0)
                {
                    return characterDatas[3];
                }
                else if (x == 0 && y == 1)
                {
                    return characterDatas[1];
                }
                else if (x == 1 && y == 1)
                {
                    return characterDatas[4];
                }
                else if (x == 0 && y == 2)
                {
                    return characterDatas[2];
                }
                else if (x == 1 && y == 2)
                {
                    return characterDatas[5];
                }
                break;
        }

        return null;
    }
}
