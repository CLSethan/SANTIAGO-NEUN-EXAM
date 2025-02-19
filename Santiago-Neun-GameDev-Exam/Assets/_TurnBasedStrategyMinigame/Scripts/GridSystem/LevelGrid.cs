using UnityEngine;
using System.Collections.Generic;
using NF.Main.Core;
using System;


public class LevelGrid : Singleton<LevelGrid>
{

    public event EventHandler OnAnyUnitMovedGridPosition;

    private GridSystem<GridObject> _gridSystem;
    [SerializeField]
    private int _levelGridWidth;
    [SerializeField] 
    private int _levelGridHeight;
    [SerializeField]
    private float _levelGridCellsize = 2f;
    [SerializeField]
    private GameObject _gridDebugObjectPrefab;


    private void Awake()
    {
        Instance = this;

        _gridSystem = new GridSystem<GridObject>(_levelGridWidth, _levelGridHeight, _levelGridCellsize,
                        (GridSystem<GridObject> g, GridPosition gridPosition) => new GridObject(g, gridPosition));
        //_gridSystem.CreateDebugObjects(_gridDebugObjectPrefab);
    }

    private void Start()
    {
        Pathfinding.Instance.Setup(_levelGridWidth, _levelGridHeight, _levelGridCellsize);
    }

    // add unit to list of units at current grid position
    public void AddUnitAtGridPosition(GridPosition gridPosition, BaseUnit unit)
    {
        GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
        gridObject.AddUnit(unit);
    }
    // get units at current grid position
    public List<BaseUnit> GetUnitListAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnitList();

    }

    // remove unit from list at current grid position
    public void RemoveUnitAtGridPosition(GridPosition gridPosition, BaseUnit unit)
    {
        GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
        gridObject.RemoveUnit(unit);
    }

    // update grid position when unit moves
    public void UnitMovedGridPosition(BaseUnit unit, GridPosition fromGridPosition, GridPosition toGridPosition)
    {
        RemoveUnitAtGridPosition(fromGridPosition, unit);
        AddUnitAtGridPosition(toGridPosition, unit);
        OnAnyUnitMovedGridPosition?.Invoke(this, EventArgs.Empty);
    }

    // get grid position of world position
    public GridPosition GetGridPosition(Vector3 worldPos)
    {
        return _gridSystem.GetGridPosition(worldPos);
    }

    // get world position of grid position
    public Vector3 GetWorldPosition(GridPosition gridPosition)
    {
        return _gridSystem.GetWorldPosition(gridPosition);
    }

    // check if grid position is valid 
    public bool IsValidGridPosition(GridPosition gridPosition)
    {
        return _gridSystem.IsValidGridPosition(gridPosition);
    }

    public int GetWidth()
    {
        return _gridSystem.GetWidth();
    }

    public int GetHeight()
    {
        return _gridSystem.GetHeight();
    }

    //check if grid position is occupied by another unit
    public bool HasAnyUnitOnGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
        return gridObject.HasAnyUnit();
    }

    //Get first unit at grid position
    public BaseUnit GetUnitAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnit();
    }
}
