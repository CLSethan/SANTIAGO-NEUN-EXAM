using UnityEngine;
using System.Collections.Generic;
using NF.Main.Core;
using System;
using UniRx;


public class LevelGrid : Singleton<LevelGrid>
{
    // create Subject Events
    public Subject<GridPosition> OnAnyUnitMovedGridPosition;

    [SerializeField]
    private int _levelGridWidth;
    [SerializeField] 
    private int _levelGridHeight;
    [SerializeField]
    private float _levelGridCellsize = 2f;
    [SerializeField]
    private GameObject _gridDebugObjectPrefab;

    private GridSystem<GridObject> _gridSystem;



    private void Awake()
    {
        Instance = this;
        // Initialize Grid System
        _gridSystem = new GridSystem<GridObject>(_levelGridWidth, _levelGridHeight, _levelGridCellsize,
                        (GridSystem<GridObject> g, GridPosition gridPosition) => new GridObject(g, gridPosition));
        // Initialize Events
        OnAnyUnitMovedGridPosition = new Subject<GridPosition>();
        // uncomment for debugging
        //_gridSystem.CreateDebugObjects(_gridDebugObjectPrefab);
    }

    private void Start()
    {
        Pathfinding.Instance.Setup(_levelGridWidth, _levelGridHeight, _levelGridCellsize);
    }

    // Add unit to the grid
    public void AddUnitAtGridPosition(GridPosition gridPosition, BaseUnit unit)
    {
        GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
        gridObject.AddUnit(unit);
    }

    // Get all units at a grid position
    public List<BaseUnit> GetUnitListAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnitList();

    }

    // Remove a unit from a grid position
    public void RemoveUnitAtGridPosition(GridPosition gridPosition, BaseUnit unit)
    {
        GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
        gridObject.RemoveUnit(unit);
    }

    // Update unit's grid position when moving
    public void UnitMovedGridPosition(BaseUnit unit, GridPosition fromGridPosition, GridPosition toGridPosition)
    {
        RemoveUnitAtGridPosition(fromGridPosition, unit);
        AddUnitAtGridPosition(toGridPosition, unit);
        OnAnyUnitMovedGridPosition.OnNext(toGridPosition); 
    }

    // Convert world position to grid position
    public GridPosition GetGridPosition(Vector3 worldPos)
    {
        return _gridSystem.GetGridPosition(worldPos);
    }

    // Convert grid position to world position
    public Vector3 GetWorldPosition(GridPosition gridPosition)
    {
        return _gridSystem.GetWorldPosition(gridPosition);
    }

    // Check if a grid position is within valid bounds
    public bool IsValidGridPosition(GridPosition gridPosition)
    {
        return _gridSystem.IsValidGridPosition(gridPosition);
    }

    // Check if a grid position has a unit
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

    // Get grid dimensions
    public int GetWidth() => _gridSystem.GetWidth();
    public int GetHeight() => _gridSystem.GetHeight();
}
