using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using DamageNumbersPro.Demo;
using DamageNumbersPro;

public class BaseMonster : BaseActor
{
    public static event Action<BaseMonster> OnGoal = null;
    public static event Action<BaseMonster> OnDeath = null;

    protected MapController _cachedMapController = null;
    protected MonsterStatusData _cachedMonsterStatusData = null;
    protected int _currentHP = 0;
    protected bool _isDeath = false;
    protected bool _isGoal = false;

    [SerializeField] Slider _healthBar;
    [SerializeField] DamageNumber _damageNumber;
    [SerializeField] DNP_PrefabSettings _prefabSettings;
    [SerializeField] Vector3 _damageOffSet;

    private CancellationTokenSource _delayDeathTokenSource;
    private Vector3 _currentWaypoint = Vector3.zero;
    private int _currentWaypointIndex = 0;

    public void Initialize(MapController mapController, MonsterStatusData statusData)
    {
        _cachedMapController = mapController;
        _cachedMonsterStatusData = statusData;
        _currentHP = statusData.HP;
        _healthBar.maxValue = statusData.HP;
        _healthBar.value = statusData.HP;
        _isDeath = false;
        _isGoal = false;

        _currentWaypointIndex = 0;
        _currentWaypoint = _cachedMapController.GetWaypoint(_currentWaypointIndex);
        _cachedTransform.position = _currentWaypoint;
    }

    public void NextWaypoint()
    {
        if (_cachedMapController == null)
            return;

        _currentWaypoint = _cachedMapController.GetWaypoint(++_currentWaypointIndex);
    }

    public void MoveWaypoint()
    {
        var direction = _currentWaypoint - _cachedTransform.position;
        var position = direction.normalized * Time.deltaTime * _cachedMonsterStatusData.Speed;
        _cachedTransform.position += position;

        FlipX(position.x > 0);
    }

    public bool IsNearWaypoint(float distance)
    {
        return Vector3.Distance(_currentWaypoint, _cachedTransform.position) <= distance * _cachedMonsterStatusData.Speed;
    }

    public bool IsNextWaypoint()
    {
        if (_cachedMapController == null)
            return false;

        return _cachedMapController.GetWaypoint(_currentWaypointIndex + 1) != Vector3.zero;
    }

    public bool IsDeath()
    {
        return _currentHP <= 0;
    }

    public void Impact(int damage)
    {
        _currentHP -= damage;
        _healthBar.value = _currentHP;

        var newDamageNumber = _damageNumber.Spawn(_cachedTransform.position + _damageOffSet, damage);
        newDamageNumber.SetFollowedTarget(_cachedTransform);

        _prefabSettings.Apply(newDamageNumber);
    }

    public void Death(float delayedDeathTime)
    {
        if (_isDeath)
            return;

        _isDeath = true;

        OnDelayedDeath(delayedDeathTime);

        if (OnDeath == null)
            return;

        OnDeath(this);
    }

    public bool IsGoal()
    {
        return _isGoal;
    }

    public void Goal()
    {
        if (_isGoal)
            return;

        _isGoal = true;

        _cachedGameObject.SetActive(false);

        if (OnGoal == null)
            return;

        OnGoal(this);
    }

    public MonsterStatusData GetStatusData()
    {
        return _cachedMonsterStatusData;
    }

    private async void OnDelayedDeath(float delayedDeathTime)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(delayedDeathTime));

        _cachedGameObject.SetActive(false);
    }
}
