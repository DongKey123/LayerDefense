using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = NAME, menuName = MENU_PATH + NAME)]
public class NextWaveDecision : BaseDecision
{
    protected const string NAME = "NextWaveDecision";

    public override bool Do<T>(T actor, BehaviourTree tree)
    {
        if (actor is PVESceneController == false)
            return false;

        var sceneController = actor as PVESceneController;
        return sceneController.IsNextWave();
    }
}
