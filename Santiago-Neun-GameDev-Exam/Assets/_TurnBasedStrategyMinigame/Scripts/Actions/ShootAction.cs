using NF.Main.Core.PlayerStateMachine;
using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEditor;

public class ShootAction : BaseAction
{
    // create Subject Events
    public Subject<OnShootEventArgs> OnShoot;
    public Subject<Unit> OnStopShooting;

    public class OnShootEventArgs
    {
        public BaseUnit TargetUnit { get; }
        public BaseUnit ShootingUnit { get; }

        public OnShootEventArgs(BaseUnit targetUnit, BaseUnit shootingUnit)
        {
            TargetUnit = targetUnit;
            ShootingUnit = shootingUnit;
        }
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
    [SerializeField] 
    private LayerMask _obstaclesLayerMask;

    private float _stateTimer;
    private BaseUnit _targetUnit;
    private bool _canShoot;
    private UnitState _state;

    protected override void Awake()
    {
        base.Awake();
        OnShoot = new Subject<OnShootEventArgs>();
        OnStopShooting = new Subject<Unit>();
    }

    private void Update()
    {
        if (!_isActive) return;

        _stateTimer -= Time.deltaTime;

        switch(_state)
        {
            case UnitState.Aiming:
                AimAtTarget();
                break;
            case UnitState.Shooting:
                if(_canShoot)
                {
                    Shoot();
                    _canShoot = false;
                }
                break;
            case UnitState.Cooldown:
                break;
        }

        if (_stateTimer <= 0f) NextState();
    }

    //handles transitioning to the next state in the shooting process.
    private void NextState()
    {
        switch (_state)
        {
            case UnitState.Aiming:
                TransitionToState(UnitState.Shooting, _shootStateTime);
                break;
            case UnitState.Shooting:
                TransitionToState(UnitState.Cooldown, _cooloffStateTime);
                break;
            case UnitState.Cooldown:
                OnStopShooting.OnNext(Unit.Default); //notify subscribers
                ActionComplete();
                break;
        }
    }

    //update state and duration
    private void TransitionToState(UnitState newState, float duration)
    {
        _state = newState;
        _stateTimer = duration;
    }

    // rotate unit to face target
    private void AimAtTarget()
    {
        Vector3 AimDir = (_targetUnit.GetWorldPosition() - _unit.GetWorldPosition()).normalized;
        float _rotateSpeed = 10f;
        transform.forward = Vector3.Lerp(transform.forward, AimDir, _rotateSpeed * Time.deltaTime);
    }

    private void Shoot()
    {
        OnShoot.OnNext(new OnShootEventArgs(_targetUnit, _unit)); //notify subscribers
        _targetUnit.Damage(_damage);
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        BaseUnit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 100 + Mathf.RoundToInt((1 - targetUnit.GetHealthNormalized()) * 100f),
        };
    }

    //gets the number of valid targets at a given grid position.
    public int GetTargetCountAtPosition(GridPosition gridPosition)
    {
        return GetValidActionGridPositionList(gridPosition).Count;
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = _unit.GetGridPosition();
        return GetValidActionGridPositionList(unitGridPosition);
    }

    //Determines valid grid positions for shooting based on range, obstacles, and target validation.
    public List<GridPosition> GetValidActionGridPositionList(GridPosition unitGridPosition)
    {
        List<GridPosition> validGridPositions = new List<GridPosition>();

        for (int x = -_maxShootDistance; x <= _maxShootDistance; x++)
        {
            for (int z = -_maxShootDistance; z <= _maxShootDistance; z++)
            {
                GridPosition testGridPosition = new GridPosition(x, z) + unitGridPosition;

                if (!IsValidShootingPosition(unitGridPosition, testGridPosition)) continue;

                validGridPositions.Add(testGridPosition);
            }
        }

        return validGridPositions;
    }

    // check if target is valid
    private bool IsValidShootingPosition(GridPosition unitGridPosition, GridPosition testGridPosition)
    {
        //invalid if testgrid position is outside of levelgrid bounds
        if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) return false;
        // create circular range based on max shoot distance
        int testDistance = Mathf.Abs(testGridPosition.x - unitGridPosition.x) + Mathf.Abs(testGridPosition.z - unitGridPosition.z);
        //invalid if out of range
        if (testDistance > _maxShootDistance) return false;
        // invalid if test grid position is not occupied
        if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition)) return false;
        // invalid if both units are on the same team
        BaseUnit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
        if (targetUnit.IsEnemy() == _unit.IsEnemy()) return false;
        // invalid if being blocked by an obstacle
        return !IsObstructedByObstacle(unitGridPosition, targetUnit);
    }

    // check if blocked by obstacle
    private bool IsObstructedByObstacle(GridPosition unitGridPosition, BaseUnit targetUnit)
    {
        Vector3 unitWorldPosition = LevelGrid.Instance.GetWorldPosition(unitGridPosition);
        Vector3 shootDirection = (targetUnit.GetWorldPosition() - unitWorldPosition).normalized;
        float unitShoulderHeight = 1.7f;

        return Physics.Raycast(
            unitWorldPosition + Vector3.up * unitShoulderHeight,
            shootDirection,
            Vector3.Distance(unitWorldPosition, targetUnit.GetWorldPosition()),
            _obstaclesLayerMask
        );
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        _targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        _state = UnitState.Aiming;
        _stateTimer = _aimingStateTime;
        _canShoot = true;

        ActionStart(onActionComplete);
    }

    public BaseUnit GetTargetUnit() => _targetUnit;
    public int GetMaxShootDistance() => _maxShootDistance;
}
