using System;
using UnityEngine;
using UnityEngine.UI;


public delegate void CardSellectCallback(int index);

public class CardSettingElement : MonoBehaviour
{
    [SerializeField] private GameObject notifyGameObject = null;
    [SerializeField] private Image cardImage = null;
    [SerializeField] private Button elementButton = null;

    private CardSellectCallback onClickCallback;
    private int index;

    private void Awake()
    {
        elementButton.onClick.AddListener(OnClickElementButton);
    }

    public void SetCallBack(CardSellectCallback callBack)
    {
        onClickCallback = callBack;
    }

    public void SetData(int characterIndex, Sprite characterImage)
    {
        index = characterIndex;
        cardImage.sprite = characterImage;
    }

    private void OnClickElementButton()
    {
        onClickCallback(index);
    }
}
