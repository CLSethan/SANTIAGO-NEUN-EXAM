using System;
using UnityEngine;
using NF.Main.Core;
using NF.Main.Core.PlayerStateMachine;
using NF.Main.Core.GameStateMachine;
using UniRx;


public class BaseUnitController : MonoExt 
{
    [SerializeField]
    private Animator _unitAnimator;
    [SerializeField]
    private GameObject _bulletProjectilePrefab;
    [SerializeField]
    private Transform _shootPoint;

    private StateMachine _stateMachine;

    public UnitState UnitState { get; set; }

    private void Awake()    
    {
        //SetupStateMachine();
    }

    private void Start()
    {
        Initialize();
        SetupStateMachine();

    }

    private void SetupStateMachine()
    {
        _stateMachine = new StateMachine();

        // Create states
        var idleState = new UnitAimingState(this, _unitAnimator);
        var walkingState = new UnitWalkingState(this, _unitAnimator);
        var shootingState = new UnitShootingState(this, _unitAnimator, _bulletProjectilePrefab, _shootPoint);
        var cooldownState = new UnitCooldownState(this, _unitAnimator);
        
        At(idleState, walkingState, new FuncPredicate(() => UnitState == UnitState.Walking));
        At(walkingState, idleState, new FuncPredicate(() => UnitState == UnitState.Aiming));
        At(idleState, shootingState, new FuncPredicate(() => UnitState == UnitState.Shooting));
        At(shootingState, idleState, new FuncPredicate(() => UnitState == UnitState.Aiming));
        At(shootingState, cooldownState, new FuncPredicate(() => UnitState == UnitState.Cooldown));
        At(cooldownState, idleState, new FuncPredicate(() => UnitState == UnitState.Aiming));


        if (TryGetComponent<MoveAction>(out MoveAction moveAction))
        {
            AddEvent(moveAction.OnStartMoving, _ => _stateMachine.SetState(walkingState));
            AddEvent(moveAction.OnStopMoving, _ => _stateMachine.SetState(idleState));
        }

        if (TryGetComponent<ShootAction>(out ShootAction shootAction))
        {
            AddEvent<ShootAction.OnShootEventArgs>(shootAction.OnShoot, (e) =>
            {
                shootingState.SetTarget(e.TargetUnit);
                _stateMachine.SetState(shootingState);
            });

            AddEvent(shootAction.OnStopShooting, (e) => _stateMachine.SetState(idleState));
        }


        // Initialize state machine with default state
        _stateMachine.SetState(idleState);
    }

    private void At(IState from, IState to, IPredicate condition) => _stateMachine.AddTransition(from, to, condition);
    private void Any(IState to, IPredicate condition) => _stateMachine.AddAnyTransition(to, condition);
}
