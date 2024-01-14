using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = NAME, menuName = MENU_PATH + NAME)]
public class Selector : Node
{
    protected const string NAME = "Selector";

    [SerializeField] Node[] _nodes;

    public override bool Do<T>(T actor, BehaviourTree tree)
    {
        if (_nodes == null)
            return false;

        for (int i = 0, length = _nodes.Length; i < length; ++i)
        {
            if (_nodes[i].Do(actor, tree))
                return true;
        }

        return false;
    }
}
