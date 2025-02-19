using System.Collections.Generic;
using UnityEngine;

public class GridObject 
{
    private List<BaseUnit> _unitList;
    private GridSystem _gridSystem;
    private GridPosition _gridPosition;

    public GridObject(GridSystem gridSystem, GridPosition gridPosition)
    {
        this._gridSystem = gridSystem;
        this._gridPosition = gridPosition;
        _unitList = new List<BaseUnit>();
    }

    public override string ToString()
    {
        string unitString = "";
        foreach (BaseUnit unit in _unitList)
        {
            unitString += unit + "\n";
        }
        return _gridPosition.ToString() + "\n" + unitString;
    }

    public void AddUnit(BaseUnit unit)
    {
        _unitList.Add(unit);
    }
    public void RemoveUnit(BaseUnit unit) 
    { 
        _unitList.Remove(unit);
    }

    public List<BaseUnit> GetUnitList()
    {
        return this._unitList;
    }

    public bool HasAnyUnit()
    {
        return _unitList.Count > 0;
    }

    //get first unit on unit list
    public BaseUnit GetUnit()
    {
        if(HasAnyUnit())
        {
            return _unitList[0];
        }
        else
        {
            return null;
        }
    }
}
