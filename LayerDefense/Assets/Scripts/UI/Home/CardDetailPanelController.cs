using System;
using UnityEngine;
using UnityEngine.UI;

public class CardDetailPanelController : UIBaseController
{
    private CardDetailPanel cardDetailPanelView = null;

    [SerializeField] private Button backButton = null;
    [SerializeField] private Button equipButton = null; 
    

    private CharacterData characterData = null;

    private CharacterManager characterManager;

    public Action<CharacterData> onClickEquip;

    protected override void Awake()
    {
        base.Awake();

        //Cashing
        characterManager = CharacterManager.getInstance;

        //View Setting
        cardDetailPanelView = this.GetComponent<CardDetailPanel>();
        
        //Button
        backButton.onClick.AddListener(Hide);
        equipButton.onClick.AddListener(OnClickEquip);
    }



    public void SetData(CharacterData Data)
    {
        characterData = Data;

        Sprite sprite = characterManager.GetCharacterSprite(characterData);
        cardDetailPanelView.SetData(sprite);
    }

    private void OnClickEquip()
    {
        onClickEquip?.Invoke(characterData);
        Hide();
    }
}
