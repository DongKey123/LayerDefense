using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = NAME, menuName = MENU_PATH + NAME)]
public class NextStageAction : BaseAction
{
    protected const string NAME = "NextStageAction";

    public override bool Do<T>(T actor, BehaviourTree tree)
    {
        var manager = InGameDataManager.getInstance;
        if (manager == null)
            return false;

        ++manager.currentStage;

        return true;
    }
}
