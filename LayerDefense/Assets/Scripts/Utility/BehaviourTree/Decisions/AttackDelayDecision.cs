using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = NAME, menuName = MENU_PATH + NAME)]
public class AttackDelayDecision : BaseDecision
{
    protected const string NAME = "AttackDelayDecision";

    public override bool Do<T>(T actor, BehaviourTree tree)
    {
        if (actor is BaseHero == false)
            return false;

        var baseHero = actor as BaseHero;
        return baseHero.IsAttack();
    }
}
