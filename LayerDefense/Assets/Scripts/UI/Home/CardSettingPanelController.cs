using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardSettingPanelController : UIBaseController
{
    public enum ECardSetting
    {
        All,
        Hero,
        Tower
    }

    //Button
    [Header ("UI Button")]
    [SerializeField] private Button backButton = null;
    [SerializeField] private Button categoryAllButton = null;
    [SerializeField] private Button categoryHeroButton = null;
    [SerializeField] private Button categoryTowerButton = null;

    private CardSettingPanel cardSettingPanelView = null;

    //Popup
    private const string detailPopupPath = "Home/CardDetailPanel";

    //Element
    [SerializeField] private Transform cardElementParent = null;
    private GameObjectPool<CardSettingElement> cardElementPool = null;
    private List<CardSettingElement> cardSettingElementList = new List<CardSettingElement>();
    
    private CardDetailPanelController cardDetailPanelController = null;
    private const string cardElementPath = "Prefabs/UI/Home/CardSettingElement";

    //Setting Card
    private CharacterData[] settingCharacterData = null;
    private const int maxCharcterSetNum = 8;

    //Caching
    private CharacterManager characterManager = null;
    private UIManager uiManager = null;


    protected override void Awake()
    {
        base.Awake();

        uiManager = UIManager.getInstance;
        characterManager = CharacterManager.getInstance;

        //Interaction Setting
        cardSettingPanelView = this.GetComponent<CardSettingPanel>();
        backButton.onClick.AddListener(Hide);

        categoryAllButton.onClick.AddListener(()=>SelectCardType(ECardSetting.All));
        categoryHeroButton.onClick.AddListener(()=>SelectCardType(ECardSetting.Hero));
        categoryTowerButton.onClick.AddListener(()=>SelectCardType(ECardSetting.Tower));

        cardElementPool = new GameObjectPool<CardSettingElement>();
        var prefab = Resources.Load<CardSettingElement>(cardElementPath);
        cardElementPool.Initialize(prefab, cardElementParent);

        cardDetailPanelController = uiManager.Show<CardDetailPanelController>(detailPopupPath);
        cardDetailPanelController.Hide();

        cardDetailPanelController.onClickEquip += EquipHero;

        settingCharacterData = new CharacterData[maxCharcterSetNum];
    }

    

    public override void Show()
    {
        base.Show();

        int count = characterManager.HeroDictionary.Count;
        while(cardSettingElementList.Count != count)
        {
            var Element = cardElementPool.Pop();
            Element.transform.localScale = Vector3.one;
            cardSettingElementList.Add(Element);
        }
        
        var enumerator = characterManager.HeroDictionary.GetEnumerator();
        int i = 0;
        while(enumerator.MoveNext())
        {
            KeyValuePair<int, CharacterData> Item = enumerator.Current;
            cardSettingElementList[i].SetCallBack(SelectCard);
            cardSettingElementList[i++].SetData(Item.Key,characterManager.GetCharacterSprite(Item.Value));
            
        }
    }

    public override void Hide()
    {
        base.Hide();

        int count = cardSettingElementList.Count;
        for(int i =0;i < count; i++)
        {
            cardElementPool.Push(cardSettingElementList[i]);
        }
        cardSettingElementList.Clear();
    }

    private void SelectCard(int dataKey)
    {
        CharacterData data = characterManager.HeroDictionary[dataKey];
        uiManager.Show<CardDetailPanelController>(detailPopupPath).SetData(data);
    }

    private void SelectCardType(ECardSetting cardType)
    {
        switch (cardType)
        {
            case ECardSetting.All:
                cardSettingPanelView.FocusAllType(true);
                cardSettingPanelView.FocusHero(false);
                cardSettingPanelView.FocusTower(false);
                break;
            case ECardSetting.Hero:
                cardSettingPanelView.FocusAllType(false);
                cardSettingPanelView.FocusHero(true);
                cardSettingPanelView.FocusTower(false);
                break;
            case ECardSetting.Tower:
                cardSettingPanelView.FocusAllType(false);
                cardSettingPanelView.FocusHero(false);
                cardSettingPanelView.FocusTower(true);
                break;
        }
    }

    private void EquipHero(CharacterData obj)
    {
        bool isEquip = false;
        int count = settingCharacterData.Length;

        Sprite[] sprites = new Sprite[maxCharcterSetNum];

        for(int i =0;i<count;i++)
        {
            sprites[i] = characterManager.GetCharacterSprite(settingCharacterData[i]);
                
            if (settingCharacterData[i] == null)
            {
                sprites[i] = characterManager.GetCharacterSprite(obj);
                settingCharacterData[i] = obj;
                isEquip = true;
                break;
            }
        }

        if(isEquip == false)
        {
            Debug.Log("ÆË¾÷ ¶ç¿ö Áà¾ß ÇÕ´Ï´Ù.\n¿µ¿õ ¸ø µé¾î°¡µµ·Ï ¼öÁ¤");
            return;
        }

        cardSettingPanelView.SetHero(sprites);
    }
}