using UnityEngine;
using System;
using System.Collections.Generic;

public class SpinAction : BaseAction
{
    private float _totalSpinAmount;

    private void Update()
    {
        if (!_isActive) return;

        PerformSpin();
    }

    public void PerformSpin()
    {
        float spinAddAmount = 360f * Time.deltaTime; // Calculate rotation step
        transform.eulerAngles += new Vector3(0, spinAddAmount, 0); // Apply rotation

        _totalSpinAmount += spinAddAmount;

        if (_totalSpinAmount >= 360f)
        {
            ActionComplete(); // Mark action as complete when full rotation is reached
        }
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        _totalSpinAmount = 0f; // Reset spin progress
        ActionStart(onActionComplete);
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        return new List<GridPosition> { _unit.GetGridPosition() };
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 0,
        };
    }
}
