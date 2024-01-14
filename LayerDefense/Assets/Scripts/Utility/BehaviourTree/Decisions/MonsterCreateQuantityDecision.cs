using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = NAME, menuName = MENU_PATH + NAME)]
public class MonsterCreateQuantityDecision : BaseDecision
{
    protected const string NAME = "MonsterCreateQuantityDecision";

    public override bool Do<T>(T actor, BehaviourTree tree)
    {
        if (actor is PVESceneController == false)
            return false;

        var sceneController = actor as PVESceneController;
        return sceneController.IsFullWaveMonster();
    }
}
