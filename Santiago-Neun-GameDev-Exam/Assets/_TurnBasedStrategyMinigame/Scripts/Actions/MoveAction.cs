using System;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{
    private Vector3 _targetPosition;

    //movement variables
    [SerializeField]
    private float _moveSpeed;
    [SerializeField]
    private float _rotateSpeed;
    [SerializeField]
    private float _stoppingDistance;
    [SerializeField]
    private int _maxMoveDistance;

    //animation
    [SerializeField]
    private Animator unitAnimator;
    

    protected override void Awake()
    {
        base.Awake();
        _targetPosition = transform.position;
    }

    private void Update()
    {
        if (!_isActive)
        {
            return;
        }
        Vector3 moveDirection = (_targetPosition - transform.position).normalized;

        // if distance between current position and target position is further than stopping position, move to target position.
        if (Vector3.Distance(transform.position, _targetPosition) > _stoppingDistance)
        {
            transform.position += moveDirection * _moveSpeed * Time.deltaTime;

            //rotate towards move direction
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, _rotateSpeed * Time.deltaTime);
            unitAnimator.SetBool("isWalking", true);
            //clear action
            _onActionComplete();
        }

        else
        {
            unitAnimator.SetBool("isWalking", false);
            _isActive = false;
        }
    }

    // move to grid position
    public void Move(GridPosition gridPosition, Action onActionComplete)
    {
        this._onActionComplete = onActionComplete;
        this._targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
        _isActive = true;
    }

    //check list if grid position is valid
    public bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionList = GetValidActionGridPositionList();
        return validGridPositionList.Contains(gridPosition);
    }

    //get all valid grid positions within range
    public List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition unitGridPosition = _unit.GetGridPosition();

        //cycle through moveable grid positions within max range
        for (int x = -_maxMoveDistance; x <= _maxMoveDistance; x++)
        {
            for(int z = -_maxMoveDistance; z <= _maxMoveDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                //final grid position
                GridPosition testGridPosition = offsetGridPosition + unitGridPosition;

                //invalid if testgrid position is outside of levelgrid bounds
                if(!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }
                
                // invalid if unit is already on testgridposition
                if(unitGridPosition == testGridPosition)
                {
                    continue;
                }

                // invalid if test grid position is occupied
                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    continue;
                }
                // valid grid position
                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }
}
