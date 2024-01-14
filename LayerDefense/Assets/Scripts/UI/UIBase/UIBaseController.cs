using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIBaseController : MonoBehaviour, IPanel
{
    protected const int AUDIO_CLICK = 100003;
    protected const int AUDIO_CLICK_2 = 100004;
    protected const int AUDIO_CLICK_3 = 100001;
    protected const int AUDIO_CLICK_CLOSE = 100019;
    protected readonly int[] INPUT_FIELD_AUDIO_IDS = new int[] { 100023, 100024, 100025 };

    protected Canvas canvas = null;
    private GraphicRaycaster raycaster = null;

    private bool isShow = false;

    protected virtual void Awake()
    {
        canvas = GetComponent<Canvas>();
        raycaster = GetComponent<GraphicRaycaster>();

        isShow = canvas.enabled;
    }

    protected virtual void Initialize()
    {

    }

    public virtual void Show()
    {
        isShow = true;
        canvas.enabled = true;
        raycaster.enabled = true;
    }

    public virtual void Hide()
    {
        isShow = false;
        canvas.enabled = false;
        raycaster.enabled = false;
    }

    public virtual bool IsShow()
    {
        return canvas.enabled && isShow;
    }

    public void SetSortOrder(int _order)
    {
        canvas.sortingOrder = _order;
    }

}