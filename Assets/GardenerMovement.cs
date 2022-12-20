using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardenerMovement : UnitMovement
{
    protected override Vector3 GetDestination()
    {
        return Unit.Action.CurrentDestination;
    }

    protected override Queue<Vector3> GetPath()
    {
        return Pathfinding.Instance.GetPath(Unit.transform.position, Destination, GetGardenerLayerMask());
    }

    private int GetGardenerLayerMask()
    {
        string[] layers =
        {
            Constants.LayerEnvironment
        };
        return LayerMask.GetMask(layers);
    }
}