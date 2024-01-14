using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class IntroMainPanel : MonoBehaviour
{
    [SerializeField] Button startButton = null;

    public void SetStartButton(UnityAction onStart)
    {
        startButton.onClick.AddListener(onStart);
    }
}
