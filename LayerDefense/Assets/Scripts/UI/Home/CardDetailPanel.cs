using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CardDetailPanel : MonoBehaviour
{
    [SerializeField] private Image iconImage;

    public void SetData(Sprite sprite)
    {
        iconImage.sprite = sprite;
    }
}
