using UnityEngine;

public class PathNode
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