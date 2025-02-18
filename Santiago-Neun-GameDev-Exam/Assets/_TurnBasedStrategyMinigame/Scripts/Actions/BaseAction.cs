using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    protected PlayerUnit _unit;
    protected bool _isActive;

    //use delegate to clear unit actions
    protected Action _onActionComplete;

    protected virtual void Awake()
    {
        _unit = GetComponent<PlayerUnit>();
    }

    public abstract string GetActionName();

    public abstract void TakeAction(GridPosition gridPosition, Action onActionComplete);

    //check list if grid position is valid
    public bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionList = GetValidActionGridPositionList();
        return validGridPositionList.Contains(gridPosition);
    }
    // get valid positions to conduct action
    public abstract List<GridPosition> GetValidActionGridPositionList();

    public virtual int GetActionPointCost()
    {
        return 1;
    }

}
