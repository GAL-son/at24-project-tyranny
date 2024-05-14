using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAction : Action
{
    const int ITEM_USAGE_COST = 10;

    private GameObject itemToUse;
    public ItemAction(GameObject item, Vector3Int usageLoaction) : this(item, ITEM_USAGE_COST, usageLoaction) {}
    public ItemAction(GameObject item, int itemUsageCost, Vector3Int usageLoaction) : base(itemUsageCost, usageLoaction)
    {
        itemToUse = item;
    }
}
