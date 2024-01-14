using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class StagePanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI starText;

    public void SetStarText(int curStar, int maxStar)
    {
        starText.text = $"{curStar}<color=#beb5b6> / {maxStar}</color>";
    }
}
