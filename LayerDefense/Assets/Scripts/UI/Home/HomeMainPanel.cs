using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class HomeMainPanel : MonoBehaviour
{
    [SerializeField] private Button gameStartButton;
    [SerializeField] private Button cardSettingButton;


    public void SetStartButton(UnityAction onStart)
    {
        gameStartButton.onClick.AddListener(onStart);
    }

    public void SetCardSettingButton(UnityAction onCardSetting)
    {
        cardSettingButton.onClick.AddListener(onCardSetting);
    }
}
