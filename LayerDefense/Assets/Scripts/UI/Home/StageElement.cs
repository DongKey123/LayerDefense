using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class StageElement : MonoBehaviour
{
    public Button StageButton { get { return stageButton; } }
    public int CurrentStageNumber { get { return _curStageNumber; } }

    [SerializeField] private Button stageButton = null;

    [SerializeField] private Image frameImage = null;



    [SerializeField] private GameObject starsParentObject = null;
    [SerializeField] private GameObject[] starsGameObject = null;
    [SerializeField] private TextMeshProUGUI stageNumber = null;

    [SerializeField] private GameObject lockGameObject = null;
    //
    private const int starCount = 3;
    private int _curStageNumber = 0;


    public void SetButton(EStageUIState buttonState, Sprite frameSprite, int curStageNumber, int clearStar = -1)
    {
        _curStageNumber = curStageNumber + 1;

        frameImage.sprite = frameSprite;

        //Star Setting
        stageNumber.text = $"{curStageNumber + 1}";

        switch (buttonState)
        {
            case EStageUIState.Complete:
                SetStars(clearStar);
                stageNumber.gameObject.SetActive(true);
                lockGameObject.SetActive(false);
                stageButton.enabled = true;
                break;
            case EStageUIState.Current:
                stageNumber.gameObject.SetActive(true);
                starsParentObject.SetActive(false);
                lockGameObject.SetActive(false);
                stageButton.enabled = true;
                break;
            case EStageUIState.Default:
                stageNumber.gameObject.SetActive(true);
                starsParentObject.SetActive(false);
                lockGameObject.SetActive(false);
                stageButton.enabled = false;
                break;
            case EStageUIState.Lock:
                stageNumber.gameObject.SetActive(false);
                starsParentObject.SetActive(false);
                lockGameObject.SetActive(true);
                stageButton.enabled = false;
                break;
            default:
                break;
        }

    }

    private void SetStars(int clearStar)
    {
        starsParentObject.SetActive(true);
        for (int i = 0; i < starCount; i++)
        {
            bool starActive = (i < clearStar);
            starsGameObject[i].SetActive(starActive);
        }
    }
}
