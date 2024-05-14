using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractAction : Action
{
    const int INTERACTION_COST = 1;

    // TODO: Change game object to Interactable component
    private GameObject target;
    public InteractAction(GameObject target, Vector3Int objectLoaction) : this(target, INTERACTION_COST, objectLoaction) {}
    public InteractAction(GameObject interactionTarget, int interactionCost, Vector3Int objectLoaction) : base(interactionCost, objectLoaction)
    {
        Cost = interactionCost;
        target = interactionTarget;
    }


}
