using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = NAME, menuName = MENU_PATH + NAME)]
public class Reverse : Node
{
    protected const string NAME = "Reverse";

    [SerializeField] Node _node;

    public override bool Do<T>(T actor, BehaviourTree tree)
    {
        return _node.Do(actor, tree) == false;
    }
}
