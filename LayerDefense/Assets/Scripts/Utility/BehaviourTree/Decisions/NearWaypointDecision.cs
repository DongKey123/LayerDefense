using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = NAME, menuName = MENU_PATH + NAME)]
public class NearWaypointDecision : BaseDecision
{
    protected const string NAME = "NearWaypointDecision";

    [SerializeField] float _distance;

    public override bool Do<T>(T actor, BehaviourTree tree)
    {
        if (actor is BaseMonster == false)
            return false;

        var monster = actor as BaseMonster;

        return monster.IsNearWaypoint(_distance);
    }
}
