using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CardController : MonoBehaviour
{
    [SerializeField] private CardElementController[] cards;
    [SerializeField] private GameObject selectUI;
    [SerializeField] private GameObject usedUI;

    private TileMapController tileMapController;

    private CardElementController curSelectedCard;
    public CardElementController CurSelectedCard
    {
        get { return curSelectedCard; }
    }

    private CardManager cardManager;
    private CharacterManager characterManager;

    private Dictionary<int, CardData> cardDictionary;

    private void Awake()
    {
        cardManager = CardManager.getInstance;
        characterManager = CharacterManager.getInstance;

        ////
        cardDictionary = cardManager.CardDictionary;

        tileMapController = FindObjectOfType<TileMapController>();
        
        for(int i = 0; i < cards.Length; i++)
        {
            //cards[i].OnClick += OnClickCard;

        }
    }

    private void Start()
    {
        tileMapController.SetTileHeroDefault();

        //Default SetUP
        SetUpCard();
    }


    public void OnClickCard(CardElementController card)
    {
        if (curSelectedCard == card)
        {
            curSelectedCard?.SetSelectCard(false);
            curSelectedCard = null;
            tileMapController.DeSelectHighLightTile();
        }
        else
        {
            curSelectedCard = card;
            curSelectedCard?.SetSelectCard(true);
            tileMapController.SetCardHighLightTile(0, 0);
        }
    }

    public void SetCard()
    {
        curSelectedCard?.SetCardOnMap(false);
    }

    public void SetUpCard()
    {
        int count = cards.Length;
        for(int i =0; i< count; i++)
        {
            int key = cardManager.GetRandomCardKey();

            cards[i].SetUpCard(cardDictionary[key]);
        }
    }

}
