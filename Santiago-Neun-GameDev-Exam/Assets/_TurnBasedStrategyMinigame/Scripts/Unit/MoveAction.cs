using System.Collections.Generic;
using UnityEngine;

public class MoveAction : MonoBehaviour
{
    private Vector3 _targetPosition;
    private PlayerUnit _unit;

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
    

    private void Awake()
    {
        _unit = GetComponent<PlayerUnit>();
        _targetPosition = transform.position;
    }

    private void Update()
    {
        // if distance between current position and target position is further than stopping position, move to target position.
        if (Vector3.Distance(transform.position, _targetPosition) > _stoppingDistance)
        {
            Vector3 moveDirection = (_targetPosition - transform.position).normalized;
            transform.position += moveDirection * _moveSpeed * Time.deltaTime;

            //smooth rotation
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, _rotateSpeed * Time.deltaTime);
            unitAnimator.SetBool("isWalking", true);
        }

        else
        {
            unitAnimator.SetBool("isWalking", false);
        }
    }

    // move to grid position
    public void Move(GridPosition gridPosition)
    {
        this._targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
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
