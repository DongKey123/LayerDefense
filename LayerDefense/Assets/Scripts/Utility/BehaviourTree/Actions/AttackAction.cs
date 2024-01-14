using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = NAME, menuName = MENU_PATH + NAME)]
public class AttackAction : BaseAction
{
    protected const string NAME = "AttackAction";

    [SerializeField] string _animationName;

    public override bool Do<T>(T actor, BehaviourTree tree)
    {
        if (actor is BaseHero == false)
            return false;

        var baseHero = actor as BaseHero;
        baseHero.Attack();

        var sPUM_Prefabs = baseHero.GetSPUM_Prefabs();
        sPUM_Prefabs.PlayAnimation(_animationName);

        return true;
    }
}
