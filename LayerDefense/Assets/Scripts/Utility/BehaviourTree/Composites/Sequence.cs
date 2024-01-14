using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = NAME, menuName = MENU_PATH + NAME)]
public class Sequence : Node
{
    protected const string NAME = "Sequence";

    [SerializeField] Node[] _nodes;

    public override bool Do<T>(T actor, BehaviourTree tree)
    {
        if (_nodes == null)
            return true;

        for (int i = 0, length = _nodes.Length; i < length; ++i)
        {
            if (_nodes[i].Do(actor, tree) == false)
                return false;
        }

        return true;
    }
}
