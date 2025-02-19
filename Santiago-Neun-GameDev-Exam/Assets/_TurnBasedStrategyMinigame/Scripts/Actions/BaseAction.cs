using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{

    public static event EventHandler OnAnyActionStarted;
    public static event EventHandler OnAnyActionCompleted;

    protected BaseUnit _unit;
    protected bool _isActive;

    //use delegate to clear unit actions
    protected Action _onActionComplete;

    protected virtual void Awake()
    {
        _unit = GetComponent<BaseUnit>();
    }

    public EnemyAIAction GetBestEnemyAIAction()
    {

        List<EnemyAIAction> enemyAIActionList = new List<EnemyAIAction>();
        List<GridPosition> validActionGridPositionList = GetValidActionGridPositionList();

        //cycle through valid actions at grid position
        foreach (GridPosition gridPosition in validActionGridPositionList)
        {
            EnemyAIAction enemyAIAction = GetEnemyAIAction(gridPosition);
            enemyAIActionList.Add(enemyAIAction);
        }

        // sort enemy actions based on action value
        if (enemyAIActionList.Count > 0)
        {
            enemyAIActionList.Sort((EnemyAIAction a, EnemyAIAction b) => b.actionValue - a.actionValue);
            return enemyAIActionList[0];
        }

        else
        {
            // No possible Enemy AI Actions
            return null;
        }

    }

    public abstract EnemyAIAction GetEnemyAIAction(GridPosition gridPosition);
    
    // return action name
    public abstract string GetActionName();
    
    // Action Behaviour
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

    // call on any take action function
    protected void ActionStart(Action onActionComplete)
    {
        _isActive = true;
        this._onActionComplete = onActionComplete;
        OnAnyActionStarted?.Invoke(this, EventArgs.Empty);

    }

    //clear actions
    protected void ActionComplete()
    {
        _isActive = false;
        _onActionComplete();
        OnAnyActionCompleted?.Invoke(this, EventArgs.Empty);

    }

    public BaseUnit GetUnit()
    {
        return _unit;
    }


}
