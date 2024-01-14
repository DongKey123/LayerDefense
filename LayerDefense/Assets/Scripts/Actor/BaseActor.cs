using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseActor : MonoBehaviour, IActor
{
    protected GameObject _cachedGameObject = null;
    protected Transform _cachedTransform = null;
    protected SPUM_Prefabs _sPUM_Prefabs = null;
    protected BehaviourTree _behaviourTree = null;
    protected int _currentAnimationType = 0;

    public SPUM_Prefabs GetSPUM_Prefabs()
    {
        return _sPUM_Prefabs;
    }

    public void SetAnimationType(int animationType)
    {
        _currentAnimationType = animationType;
    }

    public int GetAnimationType()
    {
        return _currentAnimationType;
    }

    public void FlipX(bool flip)
    {
        _cachedTransform.rotation = Quaternion.Euler(0, flip ? 180 : 0, 0);
    }

    #region :   Unity Message

    protected virtual void Awake()
    {
        _cachedGameObject = this.gameObject;
        _cachedTransform = this.transform;

        _sPUM_Prefabs = this.GetComponent<SPUM_Prefabs>();
        _behaviourTree = this.GetComponent<BehaviourTree>();
    }

    protected virtual void Update()
    {
        if (_behaviourTree == null)
            return;

        _behaviourTree.Do(this);
    }

    #endregion
}
