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
        this._gridPosition = gridPosition;
    }
    public bool IsWalkable()
    {
        return _isWalkable;
    }

    public void SetIsWalkable(bool isWalkable)
    {
        this._isWalkable = isWalkable;
    }

    public override string ToString()
    {
        return _gridPosition.ToString();
    }

    public int GetGCost()
    {
        return _gCost;
    }

    public int GetHCost()
    {
        return _hCost;
    }

    public int GetFCost()
    {
        return _fCost;
    }

    public void SetGCost(int gCost)
    {
        this._gCost = gCost;
    }

    public void SetHCost(int hCost)
    {
        this._hCost = hCost;
    }

    public void CalculateFCost()
    {
        _fCost = _gCost + _hCost;
    }

    public void ResetCameFromPathNode()
    {
        _cameFromPathNode = null;
    }

    public void SetCameFromPathNode(PathNode pathNode)
    {
        _cameFromPathNode = pathNode;
    }

    public PathNode GetCameFromPathNode()
    {
        return _cameFromPathNode;
    }

    public GridPosition GetGridPosition()
    {
        return _gridPosition;
    }

}
