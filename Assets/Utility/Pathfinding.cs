using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public static Pathfinding Instance { get; private set; }

    #region UnityMethods

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    #endregion

    #region PathNode - Data structure

    private class PathNode
    {
        public Vector3 Position { get; }
        public float G { get; }
        public float H { get; }
        public PathNode Parent { get; }

        public PathNode(Vector3 nodePos, float g, float h, PathNode nodeParent)
        {
            Position = nodePos;
            G = g;
            H = h;
            Parent = nodeParent;
        }

        public float GetF()
        {
            return G + H;
        }

        public bool Equals(Vector3 position)
        {
            return Position == position;
        }
    }

    #endregion

    private List<PathNode> _openNodes = new();
    private List<PathNode> _closedNodes = new();

    private PathNode _currentNode;
    private PathNode _startNode;

    private Vector3 _targetPos;

    private float _unitRadius = 0.3f;
    private float _moveThreshold = 1f;

    public List<Vector3> GetPath(Vector3 startPos, Vector3 targetPos)
    {
        return GetPath(startPos, targetPos, _unitRadius);
    }

    public List<Vector3> GetPath(Vector3 startPos, Vector3 targetPos, float unitRadius)
    {
        List<Vector3> path = new();

        if (startPos == targetPos)
        {
            return path;
        }

        _unitRadius = unitRadius;
        ResetPath();


        PathNode endNode = CalculatePath(startPos, targetPos);
        if (endNode != null)
        {
            PathNode currNode = endNode;
            while (currNode.Parent != null)
            {
                Debug.DrawLine(currNode.Position, currNode.Parent.Position, Color.blue, 10f); // TODO: Remove
                path.Add(currNode.Position);
                currNode = currNode.Parent;
            }

            path.Reverse();
            path.RemoveAt(0); // Remove start node, because that is the units current position    
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
            _currentNode = GetCurrentNode();
            _openNodes.Remove(_currentNode);
            _closedNodes.Add(_currentNode);
            SearchForNeighbours(_currentNode);
        } while (!ReachedTargetPosition() && _openNodes.Count > 0);

        return GetEndNode();
    }

    private bool ReachedTargetPosition()
    {
        return Vector3.Distance(_targetPos, _currentNode.Position) < _moveThreshold;
    }

    private PathNode GetCurrentNode()
    {
        _openNodes = _openNodes.OrderBy(x => x.GetF()).ThenBy(x => x.H).ToList();
        return _openNodes.ElementAt(0);
    }

    private void SearchForNeighbours(PathNode parent)
    {
        int layerMask = GetLayerMask();
        List<Vector3> directions = GetDirections();

        foreach (Vector3 direction in directions)
        {
            if (!Physics.SphereCast(parent.Position, _unitRadius, direction, out _, direction.magnitude, layerMask))
            {
                Vector3 nodePosition = parent.Position + direction;

                if (NodeIsClosed(nodePosition) ||
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
        if (!ReachedTargetPosition())
        {
            return null;
        }

        if (Vector3.Distance(_currentNode.Position, _targetPos) > _moveThreshold / 4)
        {
            return CreateNode(_targetPos, _currentNode);
        }

        return _currentNode;
    }

    private int GetLayerMask()
    {
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

        for (int i = 0; i < directions.Count; i++)
        {
            directions[i] *= _moveThreshold;
        }

        return directions;
    }

    private bool NodeIsClosed(Vector3 nodePosition)
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

    private void ResetPath()
    {
        _openNodes.Clear();
        _closedNodes.Clear();
    }
}