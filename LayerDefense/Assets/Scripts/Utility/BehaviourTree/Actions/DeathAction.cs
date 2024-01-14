using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = NAME, menuName = MENU_PATH + NAME)]
public class DeathAction : BaseAction
{
    protected const string NAME = "DeathAction";

    [SerializeField] string _animationName;
    [SerializeField] float _delayedDeathTime;

    public override bool Do<T>(T actor, BehaviourTree tree)
    {
        if (actor is BaseMonster == false)
            return false;

        var monster = actor as BaseMonster;
        monster.Death(_delayedDeathTime);

        var sPUM_Prefabs = monster.GetSPUM_Prefabs();
        sPUM_Prefabs.PlayAnimation(_animationName);

        return true;
    }
}
