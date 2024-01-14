using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = NAME, menuName = MENU_PATH + NAME)]
public class PlayAnimationAction : BaseAction
{
    protected const string NAME = "PlayAnimationAction";

    [SerializeField] int _animationType;
    [SerializeField] string _animationName;

    public override bool Do<T>(T actor, BehaviourTree tree)
    {
        if (actor is BaseActor == false)
            return false;

        var baseActor = actor as BaseActor;
        if (_animationType != baseActor.GetAnimationType())
        {
            baseActor.SetAnimationType(_animationType);

            var sPUM_Prefabs = baseActor.GetSPUM_Prefabs();
            sPUM_Prefabs.PlayAnimation(_animationName);
        }

        return true;
    }
}
