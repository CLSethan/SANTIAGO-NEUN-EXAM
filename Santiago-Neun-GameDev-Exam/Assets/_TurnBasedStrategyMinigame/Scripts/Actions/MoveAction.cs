using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class MoveAction : BaseAction
{
    // create Subject Events
    public Subject<Unit> OnStartMoving;
    public Subject<Unit> OnStopMoving;


    //movement variables
    [SerializeField]
    private float _moveSpeed;
    [SerializeField]
    private float _rotateSpeed;
    [SerializeField]
    private float _stoppingDistance;
    [SerializeField]
    private int _maxMoveDistance;

    // Movement State
    private List<Vector3> positionList;
    private int currentPositionIndex;

    protected override void Awake()
    {
        base.Awake();
        OnStartMoving = new Subject<Unit>();
        OnStopMoving = new Subject<Unit>();
    }

    private void Update()
    {
        if (!_isActive)
        {
            return;
        }

        Vector3 targetPosition = positionList[currentPositionIndex];
        Vector3 moveDirection = (targetPosition - transform.position).normalized;

        // if distance between current position and target position is further than stopping position, move to target position.
        if (Vector3.Distance(transform.position, targetPosition) > _stoppingDistance)
        {
            // move and rotate towards target position
            transform.position += moveDirection * _moveSpeed * Time.deltaTime;
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, _rotateSpeed * Time.deltaTime);
        }

        else
        {
            // move to next position in path
            currentPositionIndex++;
            if (currentPositionIndex >= positionList.Count)
            {
                //reached end of movement
                OnStopMoving.OnNext(Unit.Default);
                ActionComplete();
            }

        }
    }

    // move to grid position
    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        List<GridPosition> pathGridPositionList = Pathfinding.Instance.FindPath(_unit.GetGridPosition(), gridPosition, out int pathLength);
        currentPositionIndex = 0;
        positionList = new List<Vector3>();

        foreach (GridPosition pathGridPosition in pathGridPositionList)
        {
            positionList.Add(LevelGrid.Instance.GetWorldPosition(pathGridPosition));
        }

        OnStartMoving.OnNext(Unit.Default);
        ActionStart(onActionComplete);
    }

    //get all valid grid positions within range
    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition unitGridPosition = _unit.GetGridPosition();

        //cycle through moveable grid positions within max range
        for (int x = -_maxMoveDistance; x <= _maxMoveDistance; x++)
        {
            for(int z = -_maxMoveDistance; z <= _maxMoveDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = offsetGridPosition + unitGridPosition;

                if (IsValidMovePosition(unitGridPosition, testGridPosition))
                {
                    validGridPositionList.Add(testGridPosition);
                }
            }
        }
        return validGridPositionList;
    }

    private bool IsValidMovePosition(GridPosition unitGridPosition, GridPosition testGridPosition)
    {
        //invalid if testgrid position is outside of levelgrid bounds
        if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) return false;
        // invalid if unit is already on testgridposition
        if (unitGridPosition == testGridPosition) return false;
        // invalid if test grid position is occupied
        if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition)) return false;
        // invalid if grid is not walkable
        if (!Pathfinding.Instance.IsWalkableGridPosition(testGridPosition)) return false;
        // invalid if no path
        if (!Pathfinding.Instance.HasPath(unitGridPosition, testGridPosition)) return false;
        //invalid if path is too long
        int pathfindingDistanceMultiplier = 10;
        if (Pathfinding.Instance.GetPathLength(unitGridPosition, testGridPosition) > _maxMoveDistance * pathfindingDistanceMultiplier) return false;
        else return true;

    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        int targetCountAtGridPosition = _unit.GetAction<ShootAction>().GetTargetCountAtPosition(gridPosition);

        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = targetCountAtGridPosition * 10,
        };
    }

}
