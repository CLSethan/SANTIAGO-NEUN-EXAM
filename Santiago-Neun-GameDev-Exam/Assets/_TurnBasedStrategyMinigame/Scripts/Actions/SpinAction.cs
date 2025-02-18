using UnityEngine;
using System;
using System.Collections.Generic;

public class SpinAction : BaseAction
{

    private float _totalSpinAmount;


    private void Update()
    {
        if (!_isActive)
        {
            return;
        }

        float spinAddAmount = 360f * Time.deltaTime;
        transform.eulerAngles += new Vector3(0, spinAddAmount, 0);

        _totalSpinAmount += spinAddAmount;
        if (_totalSpinAmount >= 360f)
        {
            _isActive = false;
            _onActionComplete();
        }
    }

    public override void TakeAction(GridPosition gridPosition, Action OnActionComplete)
    {
        this._onActionComplete = OnActionComplete;
        _isActive = true;
        _totalSpinAmount = 0f;
    }

    public override string GetActionName()
    {
        return "Spin";
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        // get current grid position
        GridPosition unitGridPosition = _unit.GetGridPosition();

        return new List<GridPosition>
        {
            unitGridPosition
        };
    }

    public override int GetActionPointCost()
    {
        return 2;
    }
}
