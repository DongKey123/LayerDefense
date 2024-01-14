using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = NAME, menuName = MENU_PATH + NAME)]
public class NextWaveAction : BaseAction
{
    protected const string NAME = "NextWaveAction";

    public override bool Do<T>(T actor, BehaviourTree tree)
    {
        var manager = InGameDataManager.getInstance;
        if (manager == null)
            return false;

        ++manager.currentWave;

        return true;
    }
}
