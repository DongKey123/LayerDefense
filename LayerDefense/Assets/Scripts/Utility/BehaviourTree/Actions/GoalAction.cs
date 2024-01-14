using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = NAME, menuName = MENU_PATH + NAME)]
public class GoalAction : BaseAction
{
    protected const string NAME = "GoalAction";

    public override bool Do<T>(T actor, BehaviourTree tree)
    {
        if (actor is BaseMonster == false)
            return false;

        var monster = actor as BaseMonster;
        monster.Goal();

        return true;
    }
}
