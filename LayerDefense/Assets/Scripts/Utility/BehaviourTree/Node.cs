using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node : BaseScriptable
{
    protected const string MENU_PATH = "LayerDefense/Scriptables/BehaviourTree/";

    public abstract bool Do<T>(T actor, BehaviourTree tree) where T : IActor;
}
