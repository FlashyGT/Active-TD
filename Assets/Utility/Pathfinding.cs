using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    private List<PathNode> _openNodes = new();
    private List<PathNode> _closedNodes = new();

    private PathNode _currentNode;
    private PathNode _startNode;

    private Vector3 _targetPos;

    private float _unitRadius = 0.3f;
    private LayerMask _layerMask;

    private const float MinMoveThreshold = 0.5f;
    private const float MaxMoveThreshold = 2f;

    public Queue<Vector3> GetPath(Vector3 startPos, Vector3 targetPos)
    {
        return GetPath(startPos, targetPos, _unitRadius, GetDefaultLayerMask());
    }

    public Queue<Vector3> GetPath(Vector3 startPos, Vector3 targetPos, int layerMask)
    {
        return GetPath(startPos, targetPos, _unitRadius, layerMask);
    }

    public Queue<Vector3> GetPath(Vector3 startPos, Vector3 targetPos, float unitRadius, int layerMask)
    {
        Queue<Vector3> path = new();

        // If positions are too close to each other, don't do anything
        if (startPos == targetPos || Vector3.Distance(startPos, targetPos) < MinMoveThreshold)
        {
            return path;
        }

        InitValues(unitRadius, layerMask);

        // Get paths end node which is also the target position
        PathNode endNode = CalculatePath(startPos, targetPos);
        if (endNode != null)
        {
            PathNode currNode = endNode;
            while (currNode.Parent != null)
            {
                //Debug.DrawLine(currNode.Position, currNode.Parent.Position, Color.blue, 10f); // TODO: Remove
                path.Enqueue(currNode.Position);
                currNode = currNode.Parent;
            }

            path = new Queue<Vector3>(path.Reverse());
            path.Dequeue(); // Remove start node, because that is the units current position    
        }

        return path;
    }

    private PathNode CalculatePath(Vector3 startPos, Vector3 targetPos)
    {
        _startNode = CreateNode(startPos, null);
        _openNodes.Add(_startNode);
        _targetPos = targetPos;

        do
        {
            // Get the nearest node to the target position
            _currentNode = GetCurrentNode();
            
            // We need to add the current node to the closed nodes,
            // because we are finding neighbours for it
            _openNodes.Remove(_currentNode);
            _closedNodes.Add(_currentNode);
            
            // Finds nodes neighbours depending on the move threshold
            SearchForNeighbours(_currentNode);
        } while (!HasReachedTargetPosition() && _openNodes.Count > 0);

        return GetEndNode();
    }

    private bool HasReachedTargetPosition()
    {
        return Vector3.Distance(_targetPos, _currentNode.Position) < MaxMoveThreshold;
    }

    private PathNode GetCurrentNode()
    {
        _openNodes = _openNodes.OrderBy(x => x.GetF()).ThenBy(x => x.H).ToList();
        return _openNodes.ElementAt(0);
    }

    private void SearchForNeighbours(PathNode parent)
    {
        List<Vector3> directions = GetDirections();

        // Cast a sphere in each direction from a point to find valid directions,
        // they are valid if they don't collide with colliders
        foreach (Vector3 direction in directions)
        {
            if (!Physics.SphereCast(parent.Position, _unitRadius, direction, out _, direction.magnitude, _layerMask))
            {
                Vector3 nodePosition = parent.Position + direction;

                if (IsNodeClosed(nodePosition) ||
                    parent.Parent != null && Vector3.Dot(parent.Position - parent.Parent.Position, direction) == 0)
                    // 0 means that vectors are perpendicular
                {
                    continue;
                }

                PathNode newNode = GetNodeIfExists(nodePosition);
                
                if (newNode == null)
                {
                    newNode = CreateNode(nodePosition, parent);
                    _openNodes.Add(newNode);
                }
            }
        }
    }

    private PathNode GetEndNode()
    {
        // Not close enough to target, so return no end node
        if (!HasReachedTargetPosition())
        {
            return null;
        }

        // Distance within threshold, so create another node
        // to create a node at the target position
        if (Vector3.Distance(_currentNode.Position, _targetPos) > MinMoveThreshold)
        {
            return CreateNode(_targetPos, _currentNode);
        }

        return _currentNode;
    }

    private int GetDefaultLayerMask()
    {
        // Only collisions with objects that have one of these layers will get detected
        string[] layers =
        {
            Constants.LayerEnvironment,
            Constants.LayerBarricade,
            Constants.LayerUnitCollisionBlocker
        };
        return LayerMask.GetMask(layers);
    }

    private List<Vector3> GetDirections()
    {
        List<Vector3> directions = new List<Vector3>
        {
            Vector3.forward,
            Vector3.left,
            Vector3.right,
            new Vector3(1f, 0f, 1f), // left & forward (diagonal)
            new Vector3(-1f, 0f, 1f), // right & forward (diagonal)
            Vector3.back,
            new Vector3(1f, 0f, -1f), // left & back (diagonal)
            new Vector3(-1f, 0f, -1f) // right & back (diagonal)
        };

        // Depending on the threshold modify directions
        for (int i = 0; i < directions.Count; i++)
        {
            directions[i] *= MaxMoveThreshold;
        }

        return directions;
    }

    private bool IsNodeClosed(Vector3 nodePosition)
    {
        for (int x = 0; x < _closedNodes.Count; x++)
        {
            if (_closedNodes[x].Equals(nodePosition))
            {
                return true;
            }
        }

        return false;
    }

    private PathNode GetNodeIfExists(Vector3 nodePosition)
    {
        for (int x = 0; x < _openNodes.Count; x++)
        {
            if (_openNodes[x].Equals(nodePosition))
            {
                return _openNodes[x];
            }
        }

        return null;
    }

    private PathNode CreateNode(Vector3 nodePosition, PathNode parent)
    {
        if (parent == null)
        {
            return new PathNode(nodePosition, 0f, Vector3.Distance(nodePosition, _targetPos), null);
        }

        return new PathNode(
            nodePosition,
            Vector3.Distance(parent.Position, nodePosition) + parent.G,
            Vector3.Distance(nodePosition, _targetPos),
            parent
        );
    }

    private void InitValues(float unitRadius, int layerMask)
    {
        _unitRadius = unitRadius;
        _layerMask = layerMask;
        _openNodes.Clear();
        _closedNodes.Clear();
    }
}