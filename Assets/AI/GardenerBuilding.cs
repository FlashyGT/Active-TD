using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardenerBuilding : Building
{
    public override Queue<Vector3> GetActionDestinations(KeyValuePair<UnitActionType, UnitActionItem> action)
    {
        throw new System.NotImplementedException();
    }
}
