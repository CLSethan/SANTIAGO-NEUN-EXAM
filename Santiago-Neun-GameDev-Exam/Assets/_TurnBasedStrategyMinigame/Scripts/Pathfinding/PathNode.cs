using UnityEngine;

public class PathNode
{
    private GridPosition _gridPosition;
    // walking cost from start node
    private int _gCost;
    // heuristic cost of end node
    private int _hCost;
    //g + h
    private int _fCost;
    private PathNode _cameFromPathNode;
    private bool _isWalkable = true;

    public PathNode(GridPosition gridPosition)
    {
        _gridPosition = gridPosition;
    }
    
    public bool IsWalkable() => _isWalkable;

    public void SetIsWalkable(bool isWalkable) => _isWalkable = isWalkable;

    public override string ToString() => _gridPosition.ToString();

    public int GetGCost() => _gCost;

    public int GetHCost() => _hCost;

    public int GetFCost() => _fCost;

    public void SetGCost(int gCost) => _gCost = gCost;

    public void SetHCost(int hCost) => _hCost = hCost;

    public void CalculateFCost() => _fCost = _gCost + _hCost;

    public void ResetCameFromPathNode() => _cameFromPathNode = null;

    public void SetCameFromPathNode(PathNode pathNode) => _cameFromPathNode = pathNode;

    public PathNode GetCameFromPathNode() => _cameFromPathNode;

    public GridPosition GetGridPosition() => _gridPosition;

}
