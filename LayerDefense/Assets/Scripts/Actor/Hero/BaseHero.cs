using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHero : BaseActor
{
    #region :   Cached Manager

    protected StageManager _stageManager = null;

    #endregion

    protected BaseActor _cachedTarget = null;
    protected HeroStatusData _cachedHeroStatusData = null;

    private float _currentAttackDelay = 0;

    public void Initialize(HeroStatusData heroStatusData)
    {
        _cachedHeroStatusData = heroStatusData;
    }

    public void SetTarget()
    {
        _cachedTarget = _stageManager.GetMonster();
    }

    public bool HasTarget()
    {
        return _cachedTarget != null;
    }

    public void Attack()
    {
        if (_cachedTarget is BaseMonster == false)
            return;

        var monster = _cachedTarget as BaseMonster;
        monster.Impact(_cachedHeroStatusData.Attack_Damage);
    }

    public bool IsAttack()
    {
        _currentAttackDelay += Time.deltaTime;
        if (_currentAttackDelay < _cachedHeroStatusData.Attack_Speed)
            return false;

        _currentAttackDelay = 0;
        return true;
    }

    public int GetCharacterID()
    {
        return _cachedHeroStatusData.Character_ID;
    }

    #region :   Unity Message

    protected override void Awake()
    {
        base.Awake();

        _stageManager = StageManager.getInstance;
    }

    protected virtual void OnEnable()
    {
        BaseMonster.OnDeath += OnTargetDisable;
        BaseMonster.OnGoal += OnTargetDisable;
    }

    protected virtual void OnDisable()
    {
        BaseMonster.OnDeath -= OnTargetDisable;
        BaseMonster.OnGoal -= OnTargetDisable;
    }

    #endregion

    protected virtual void OnTargetDisable(BaseMonster obj)
    {
        if (_cachedTarget != obj)
            return;

        _cachedTarget = null;
    }
}
