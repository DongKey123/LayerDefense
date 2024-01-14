using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//Todo: View?? ???? ???????? ????
public class CardElementView : MonoBehaviour
{
    [SerializeField] private Button cardButton;
    [SerializeField] private GameObject isSelectedUI;

    [SerializeField] private TextMeshProUGUI costText;

    [SerializeField] private Image cell1Image;
    [SerializeField] private Image cell2Image;
    [SerializeField] private Image cell3Image;
    [SerializeField] private Image cell4Image;
    [SerializeField] private Image cell5Image;
    [SerializeField] private Image cell6Image;

    public void SetSelectCard(bool isSelect)
    {
        isSelectedUI.SetActive(isSelect);
    }

    public void SetMapCard(bool enable)
    {
        this.gameObject.SetActive(enable);
        isSelectedUI.SetActive(!enable);
    }

    public void SetEnableCard(bool enable)
    {
        cardButton.enabled = enable;
    }

    public void SetData(
        Sprite cell1Sprite,
        Sprite cell2Sprite,
        Sprite cell3Sprite,
        Sprite cell4Sprite,
        Sprite cell5Sprite,
        Sprite cell6Sprite,
        int cost)
    {
        //Todo: 수정 필요
        {
            bool cell1 = cell1Sprite == null ? false : true;
            bool cell2 = cell2Sprite == null ? false : true;
            bool cell3 = cell3Sprite == null ? false : true;
            bool cell4 = cell4Sprite == null ? false : true;
            bool cell5 = cell5Sprite == null ? false : true;
            bool cell6 = cell6Sprite == null ? false : true;

            cell1Image.gameObject.SetActive(cell1);
            cell2Image.gameObject.SetActive(cell2);
            cell3Image.gameObject.SetActive(cell3);
            cell4Image.gameObject.SetActive(cell4);
            cell5Image.gameObject.SetActive(cell5);
            cell6Image.gameObject.SetActive(cell6);
        }


        cell1Image.sprite = cell1Sprite;
        cell2Image.sprite = cell2Sprite;
        cell3Image.sprite = cell3Sprite;
        cell4Image.sprite = cell4Sprite;
        cell5Image.sprite = cell5Sprite;
        cell6Image.sprite = cell6Sprite;

        costText.text = $"{cost}";
    }
}
