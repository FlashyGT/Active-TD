using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogisticMovement : UnitMovement
{
    protected override Vector3 GetDestination()
    {
        return Unit.Action.CurrentDestination;
    }

    protected override Queue<Vector3> GetPath()
    {
        return Pathfinding.GetPath(Unit.transform.position, Destination, GetLayerMask());
    }

    private int GetLayerMask()
    {
        string[] layers =
        {
            Constants.LayerEnvironment
        };
        return LayerMask.GetMask(layers);
    }
}