using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = NAME, menuName = MENU_PATH + NAME)]
public class MonsterCreateAction : BaseAction
{
    protected const string NAME = "MonsterCreateAction";

    public override bool Do<T>(T actor, BehaviourTree tree)
    {
        if (actor is PVESceneController == false)
            return false;

        var sceneController = actor as PVESceneController;
        sceneController.CreateMonster();

        return true;
    }
}
