using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTree : MonoBehaviour
{
    [SerializeField] Node _node;

    private GameObject _cachedGameObject = null;
    private Transform _cachedTransform = null;

    public void Do(IActor actor)
    {
        if (_node == null)
            return;

        _node.Do(actor, this);
    }

    #region :   Unity Message

    private void Awake()
    {
        _cachedGameObject = this.gameObject;
        _cachedTransform = this.transform;
    }

    #endregion
}
