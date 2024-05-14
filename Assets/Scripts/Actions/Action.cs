using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action 
{
    public int Cost { get; protected set; } = 0;
    public Vector3Int ActionTarget { get; protected set; }

    public Action(int cost, Vector3Int actionTarget)
    {
        Cost = cost;
        ActionTarget = actionTarget;
    }
}
