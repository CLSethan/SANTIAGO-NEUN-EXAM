using NF.Main.Core.PlayerStateMachine;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShootAction : BaseAction
{

    public event EventHandler<OnShootEventArgs> OnShoot;

    public class OnShootEventArgs : EventArgs
    {
        public BaseUnit targetUnit;
        public BaseUnit shootingUnit;
    }

    [SerializeField]
    private int _maxShootDistance;
    [SerializeField]
    private float _aimingStateTime = 1f;
    [SerializeField]
    private float _shootStateTime = 0.1f;
    [SerializeField]
    private float _cooloffStateTime = 0.5f;
    [SerializeField]
    private int _damage = 40;
    private float _stateTimer;

    private BaseUnit _targetUnit;
    private bool _canShoot;

    private PlayerState _state;


    private void Update()
    {
        if (!_isActive)
        {
            return;
        }

        _stateTimer -= Time.deltaTime;

        switch(_state)
        {
            case PlayerState.Aiming:
                Vector3 AimDir = (_targetUnit.GetWorldPosition() - _unit.GetWorldPosition()).normalized;
                float _rotateSpeed = 10f;
                transform.forward = Vector3.Lerp(transform.forward, AimDir, _rotateSpeed * Time.deltaTime);
                break;
            case PlayerState.Shooting:
                if(_canShoot)
                {
                    Shoot();
                    _canShoot = false;
                }
                break;
            case PlayerState.Cooloff:
                break;
        }

        if (_stateTimer <= 0f)
        {
            NextState();
        }
    }

    private void NextState()
    {
        switch (_state)
        {
            case PlayerState.Aiming:
                _state = PlayerState.Shooting;
                _stateTimer = _shootStateTime;
                break;
            case PlayerState.Shooting:
                _state = PlayerState.Cooloff;
                _stateTimer = _cooloffStateTime;
                break;
            case PlayerState.Cooloff:
                ActionComplete();
                break;
        }
    }

    private void Shoot()
    {
        OnShoot?.Invoke(this, new OnShootEventArgs
        {
            targetUnit = _targetUnit,
            shootingUnit = _unit
        });
        _targetUnit.Damage(_damage);
    }

    public override string GetActionName()
    {
        return "Shoot";
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition unitGridPosition = _unit.GetGridPosition();

        //cycle through moveable grid positions within max range
        for (int x = -_maxShootDistance; x <= _maxShootDistance; x++)
        {
            for (int z = -_maxShootDistance; z <= _maxShootDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = offsetGridPosition + unitGridPosition;

                //invalid if testgrid position is outside of levelgrid bounds
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                // create circular range based on max shoot distance
                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if(testDistance > _maxShootDistance)
                {
                    continue;
                }

                // invalid if test grid position is not occupied
                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    continue;
                }
                // invalid if both units are the same kind
                BaseUnit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
                if(targetUnit.IsEnemy() == _unit.IsEnemy())
                {
                    continue;
                }

                // valid grid position
                validGridPositionList.Add(testGridPosition);
            }
        }
        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        _targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        _state = PlayerState.Aiming;
        _stateTimer = _aimingStateTime;
        _canShoot = true;
        ActionStart(onActionComplete);

    }

    public BaseUnit GetTargetUnit()
    {
        return _targetUnit;
    }

}
