using System.Collections.Generic;
using UnityEngine;
using System;


public class UIManager : LazySingleton<UIManager>
{
    private const int BASE_SORTING_ORDER = 100;

    private Dictionary<Type, UIBaseController> panelDict = null;

    private Stack<UIBaseController> panelStack = null;

    //private NetworkManager_chat chattingNetMgr = null;

    public UIManager()
    {
        panelDict = new Dictionary<Type, UIBaseController>();
        panelDict.Clear();

        panelStack = new Stack<UIBaseController>();
        panelStack.Clear();

        //chattingNetMgr = NetworkManager.ChatManager;
    }

    ~UIManager()
    {
        panelDict.Clear();
        panelDict = null;

        panelStack.Clear();
        panelStack = null;
    }

    /// <summary>
    /// 캐싱된 Panel 출력 / Scene에 배치 된 Panel은 초기화 단계에서 AddCachePanel<T>(string _panelName = "") 를 사용하여 미리 캐싱해주어야 한다. 
    /// </summary>
    /// <typeparam name="T">캐싱할 Panel Type</typeparam>
    /// <param name="_panelName">Panel 이름</param>
    /// <returns></returns>
    public T Show<T>(string _panelName = "", int order = 0) where T : UIBaseController
    {
#if UNITY_EDITOR
#endif

        var panel = GetCachedPanel<T>(_panelName);

        if (!panel.IsShow())
        {
            panel.Show();
            panelStack.Push(panel);
        }
#if UNITY_EDITOR
        Debug.Log($"show panel : {typeof(T).Name} > Count : {panelStack.Count}");
#endif

        panel.SetSortOrder(BASE_SORTING_ORDER + panelStack.Count + order);

        return panel;
    }

    public void Hide()
    {
        if (panelStack.Count > 0)
        {
            var panel = panelStack.Pop();
            panel.Hide();

#if UNITY_EDITOR
            Debug.Log($"hide panel : {panel.GetType().Name} > Remaind Count : {panelStack.Count}");
#endif
        }
    }

    private T GetCachedPanel<T>(string _panelName = "") where T : UIBaseController
    {
        Type type = typeof(T);
        if (panelDict.ContainsKey(type) == false)
        {
            AddCachePanel<T>(_panelName);
            //throw new Exception($"not cached panel : {type.Name}");
        }

        return (T)panelDict[type];
    }

    private T FindPanel<T>(string _panelName = "") where T : UIBaseController
    {
        Type type = typeof(T);
        string panelName = _panelName == string.Empty ? type.Name : _panelName;
        GameObject panelObj = UnityExtension.Find(panelName);
        if (panelObj == null)
        {
            return null;
        }

        T panel = panelObj.GetComponent<T>();
        if (panel == null)
        {
            throw new Exception($"must have {type.Name} component");
        }

        return panel;
    }

    /// <summary>
    /// UIManager에 캐싱 되어있는 Panel을 가져온다.
    /// </summary>
    /// <typeparam name="T">Panel Type</typeparam>
    /// <returns></returns>
    public T GetCachedPanel<T>() where T : UIBaseController
    {
        Type type = typeof(T);
        UIBaseController panel;
        panelDict.TryGetValue(type, out panel);
        return (T)panel;
    }

    public UIBaseController GetLatestPanel()
    {
        UIBaseController panel = null;

        if (panelStack.Count > 0)
            panel = panelStack.Peek();

        return panel;
    }

    /// <summary>
    /// Scene에 배치된 Panel을 UIManager에 미리 캐싱 한다.
    /// </summary>
    /// <typeparam name="T">캐싱할 Panel Type</typeparam>
    /// <param name="_panelName">캐싱할 Scene에 배치된 Panel Object의 이름</param>
    public void AddCachePanel<T>(string _panelName = "") where T : UIBaseController
    {
        T panel = FindPanel<T>(_panelName);
        if (panel != null)
        {
            AddCachePanel(panel);
            return;
        }

        Type type = typeof(T);
        string prefabName = _panelName == string.Empty ? type.Name : _panelName;
        GameObject panelPrefab = (GameObject)Resources.Load($"Prefabs/UI/{prefabName}", typeof(GameObject));
        GameObject panelObj = GameObject.Instantiate(panelPrefab);
        string[] nameArr = _panelName.Split('/');
        panelObj.name = $"{nameArr[nameArr.Length - 1]}";
        panel = panelObj.GetComponent<T>();

        AddCachePanel(panel);
    }

    private void AddCachePanel<T>(T _panel) where T : UIBaseController
    {
        Type type = typeof(T);
        if (panelDict.ContainsKey(type) == true)
        {
            throw new Exception($"already cached panel : {type.Name}");
        }

        panelDict.Add(type, _panel);
    }

    /// <summary>
    /// clear all cached panels
    /// </summary>
    public void ClearAllCachedPanel()
    {
        //var enumerator = panelDict.GetEnumerator();
        //while(enumerator.MoveNext())
        //{
        //    var cur = enumerator.Current;
        //    panelDict.Remove(cur.Key);
        //}

        panelDict.Clear();
        //panelDict = null;
    }

    public void ClearAllPanelStack()
    {
        panelStack.Clear();
    }

    public int PanelSortingOrder()
    {
        return BASE_SORTING_ORDER + panelStack.Count;
    }


    public int GetPanelStackCount()
    {
        return panelStack.Count;
    }
}
