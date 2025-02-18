using System.Collections.Generic;
using UnityEngine;

public class GridObject 
{
    private List<PlayerUnit> _unitList;
    private GridSystem _gridSystem;
    private GridPosition _gridPosition;

    public GridObject(GridSystem gridSystem, GridPosition gridPosition)
    {
        this._gridSystem = gridSystem;
        this._gridPosition = gridPosition;
        _unitList = new List<PlayerUnit>();
    }

    public override string ToString()
    {
        string unitString = "";
        foreach (PlayerUnit unit in _unitList)
        {
            unitString += unit + "\n";
        }
        return _gridPosition.ToString() + "\n" + unitString;
    }

    public void AddUnit(PlayerUnit unit)
    {
        _unitList.Add(unit);
    }
    public void RemoveUnit(PlayerUnit unit) 
    { 
        _unitList.Remove(unit);
    }

    public List<PlayerUnit> GetUnitList()
    {
        return this._unitList;
    }

    public bool HasAnyUnit()
    {
        return _unitList.Count > 0;
    }
}
