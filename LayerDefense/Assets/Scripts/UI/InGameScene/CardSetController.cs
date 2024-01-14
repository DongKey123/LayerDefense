using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardSetController : UIBaseController
{
    [SerializeField] RectTransform uiRectTransform;
    [SerializeField] Button setButton;
    [SerializeField] Button rotateButton;

    public Action OnSetCard;
    public Action OnRotateCard;

    private RectTransform rectTransform;

    protected override void Awake()
    {
        base.Awake();

        rectTransform = this.GetComponent<RectTransform>();

        setButton.onClick.AddListener(OnClickSetCard);
        rotateButton.onClick.AddListener(OnClickRotateCard);
    }

    public void SetPostion(Vector3 worldPos)
    {
        Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(worldPos);
        Vector2 WorldObject_ScreenPosition = new Vector2(
        ((ViewportPosition.x * rectTransform.sizeDelta.x) - (rectTransform.sizeDelta.x * 0.5f)),
        ((ViewportPosition.y * rectTransform.sizeDelta.y) - (rectTransform.sizeDelta.y * 0.5f)));

        uiRectTransform.anchoredPosition = WorldObject_ScreenPosition;

    }


    private void OnClickSetCard()
    {
        OnSetCard?.Invoke();
    }

    private void OnClickRotateCard()
    {
        OnRotateCard?.Invoke();
    }
}
