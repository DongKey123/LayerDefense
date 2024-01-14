using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = NAME, menuName = MENU_PATH + NAME)]
public class SetTargetAction : BaseAction
{
    protected const string NAME = "SetTargetAction";

    public override bool Do<T>(T actor, BehaviourTree tree)
    {
        if (actor is BaseHero == false)
            return false;

        var baseHero = actor as BaseHero;
        baseHero.SetTarget();
        return true;
    }
}
