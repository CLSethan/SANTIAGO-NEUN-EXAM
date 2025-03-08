using NF.Main.Core.GameStateMachine;
using System;
using UnityEngine;
using NF.Main.Gameplay;
using UniRx;
using NF.Main.Core;


public class BaseUnit : MonoExt
{
    [SerializeField] private UnitData unitData;

    private GridPosition _gridPosition;
    private HealthSystem _healthSystem;
    private BaseAction[] _baseActionArray;

    //unit events
    public static Subject<Unit> OnAnyActionPointsChanged;
    public static readonly Subject<BaseUnit> OnAnyUnitSpawned = new Subject<BaseUnit>();
    public static readonly Subject<BaseUnit> OnAnyUnitDead = new Subject<BaseUnit>();

    private int _actionPoints;
    private int _currentHealth;

    private void Awake()
    {
        _healthSystem = GetComponent<HealthSystem>();
        _baseActionArray = GetComponents<BaseAction>();

        _actionPoints = unitData.maxActionPoints;
        _currentHealth = unitData.maxHealth;
        OnAnyActionPointsChanged = new Subject<Unit>();
    }

    private void Start()
    {
        Initialize();
        OnSubscriptionSet();

        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(_gridPosition, this);

        //subscribe to events
        OnAnyUnitSpawned.OnNext(this);
    }


    public override void OnSubscriptionSet()
    {
        base.OnSubscriptionSet();
        //subscribe to events

        AddEvent(_healthSystem.OnDeath, _ => HealthSystem_OnDeath());
        AddEvent(TurnSystem.Instance.OnTurnChanged, _ => TurnSystem_OnTurnChanged());

    }

    void Update()
    {
        //check if unit changed grid position
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != _gridPosition)
        {
            //update grid position

            GridPosition oldGridPosition = _gridPosition;
            _gridPosition = newGridPosition;
            LevelGrid.Instance.UnitMovedGridPosition(this, oldGridPosition, newGridPosition);

            _gridPosition = newGridPosition;
        }
    }

    public void Damage(int damageAmount)
    {
        _healthSystem.Damage(damageAmount);
    }

    // check if player can spend action points then decrease it, otherwise return false
    public bool TrySpendAP(BaseAction baseAction)
    {
        if (CanSpendAP(baseAction))
        {
            SpendAP(baseAction.GetActionPointCost());
            return true;
        }
        else
        {
            return false;
        }
    }

    // check if action points is greater than action cost
    public bool CanSpendAP(BaseAction baseAction)
    {
        if (_actionPoints >= baseAction.GetActionPointCost())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // decrease action points
    private void SpendAP(int amount)
    {
        _actionPoints -= amount;
        OnAnyActionPointsChanged.OnNext(Unit.Default);

    }

    // reset action points on next turn
    private void TurnSystem_OnTurnChanged()
    {
        if((IsEnemy() && !TurnSystem.Instance.IsPlayerTurn()) || !IsEnemy() && TurnSystem.Instance.IsPlayerTurn())
        {
            _actionPoints = unitData.maxActionPoints;
            OnAnyActionPointsChanged.OnNext(Unit.Default);
        }
    }

    private void HealthSystem_OnDeath()
    {
        // cleanup grid and destroy gameobject
        Debug.Log("Destroying Unit");
        LevelGrid.Instance.RemoveUnitAtGridPosition(_gridPosition, this);
        Destroy(gameObject);
        OnAnyUnitDead.OnNext(this);
    }

    public T GetAction<T>() where T : BaseAction
    {
        foreach (BaseAction baseAction in _baseActionArray)
        {
            if (baseAction is T)
            {
                return (T)baseAction;
            }
        }
        return null;
    }

    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }
    public GridPosition GetGridPosition()
    {
        return _gridPosition;
    }

    public BaseAction[] GetBaseActionArray()
    {
        return _baseActionArray;
    }

    public int GetActionPoints()
    {
        return _actionPoints;
         
    }
    public float GetHealthNormalized()
    {
        return _healthSystem.GetHealthNormalized();
    }

    public bool IsEnemy()
    {
        return unitData.isEnemy;
    }
}
