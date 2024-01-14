using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = NAME, menuName = MENU_PATH + NAME)]
public class StageClearAction : BaseAction
{
    protected const string NAME = "StageClearAction";

    public override bool Do<T>(T actor, BehaviourTree tree)
    {
        if (actor is PVESceneController == false)
            return false;

        var sceneController = actor as PVESceneController;
        sceneController.ClearStage();

        return true;
    }
}
