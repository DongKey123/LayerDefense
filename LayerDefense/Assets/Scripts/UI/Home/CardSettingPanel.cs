using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class CardSettingPanel : MonoBehaviour
{
    [SerializeField] private Image[] selectedHeroImages =null;


    [SerializeField] private GameObject focusAllTypeGameObject = null;
    [SerializeField] private TextMeshProUGUI focusAllTypeText = null;

    private Color focusColor = new Color(0.9647059f, 0.8823529f, 0.6117647f);
    private Color normalColor = Color.white;

    [SerializeField] private GameObject normalHeroGameObject = null;
    [SerializeField] private GameObject focusHeroGameObject = null;

    [SerializeField] private GameObject normalTowerGameObject = null;
    [SerializeField] private GameObject focusTowerGameObject = null;

    public void SetHero(Sprite[] sprites)
    {
        int count = selectedHeroImages.Length;
        for(int i=0;i<count; i++)
        {
            selectedHeroImages[i].sprite = sprites[i];
            
            bool enable = sprites[i] != null;
            selectedHeroImages[i].enabled = enable;
        }
    }

    public void FocusAllType(bool isFocus)
    {
        focusAllTypeGameObject.SetActive(isFocus);
        focusAllTypeText.color = isFocus ? focusColor : normalColor;
    }

    public void FocusHero(bool isFocus)
    {
        focusHeroGameObject.SetActive(isFocus);
        normalHeroGameObject.SetActive(!isFocus);
    }

    public void FocusTower(bool isFocus)
    {
        focusTowerGameObject.SetActive(isFocus);
        normalTowerGameObject.SetActive(!isFocus);
    }
}
