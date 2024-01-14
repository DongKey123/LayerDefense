using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get { return _instance; } }

    private static T _instance;

    #region :   Protected

    protected virtual void OnInitialize()
    {
        _instance = this as T;

        DontDestroyOnLoad(_instance.gameObject);
    }

    protected virtual void OnRelease()
    {

    }

    protected virtual void OnShutdown()
    {

    }

    #endregion

    #region :   Unity Message

    private void Awake()
    {
        if (_instance == null)
        {
            OnInitialize();
        }
        else
        {
            if (_instance.GetInstanceID() != this.GetInstanceID())
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void OnDestroy()
    {
        OnRelease();
    }

    private void OnApplicationQuit()
    {
        OnShutdown();
    }

    #endregion
}
