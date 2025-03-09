using NF.Main.Core;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public abstract class BaseAction : MonoExt
{
    // create Subject Events
    public static readonly Subject<BaseAction> OnAnyActionStarted = new Subject<BaseAction>();
    public static readonly Subject<BaseAction> OnAnyActionCompleted = new Subject<BaseAction>();

    protected BaseUnit _unit;
    protected bool _isActive;

    //use delegate to clear unit actions
    protected Action _onActionComplete;

    [SerializeField]
    protected int _actionPoint;
    [SerializeField]
    protected string _actionName;


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
            enemyAIActionList.Sort((a, b) => b.actionValue - a.actionValue);
            return enemyAIActionList[0];
        }

        else
        {
            // No possible Enemy AI Actions
            return null;
        }

    }

    //Retrieves the AI action associated with a specific grid position.
    public abstract EnemyAIAction GetEnemyAIAction(GridPosition gridPosition);

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

    // call on any take action function
    protected void ActionStart(Action onActionComplete)
    {
        _isActive = true;
        _onActionComplete = onActionComplete;

        // notify all subscribers that an action has started
        OnAnyActionStarted.OnNext(this);
    }

    //clear actions
    protected void ActionComplete()
    {
        _isActive = false;
        _onActionComplete();
        // notify all subscribers that an action has completed
        OnAnyActionCompleted.OnNext(this);
    }

    // return action name
    public string GetActionName() => _actionName;

    // return AP cost
    public virtual int GetActionPointCost() => _actionPoint;

    //return unit
    public BaseUnit GetUnit() => _unit;
}
