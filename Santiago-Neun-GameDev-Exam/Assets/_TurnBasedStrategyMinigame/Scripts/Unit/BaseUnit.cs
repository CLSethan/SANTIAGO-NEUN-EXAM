using System;
using UnityEngine;

public class BaseUnit : MonoBehaviour
{

    private GridPosition _gridPosition;
    private HealthSystem _healthSystem;

    // actions
    private MoveAction _moveAction;
    private SpinAction _spinAction;
    private BaseAction[] _baseActionArray;

    public static event EventHandler OnAnyActionPointsChanged;
    [SerializeField]
    private int _actionPoints = 2;
    [SerializeField]
    private int _maxActionPoints;
    [SerializeField] private bool _isEnemy;

    private void Awake()
    {
        _healthSystem = GetComponent<HealthSystem>();
        _moveAction = GetComponent<MoveAction>();
        _spinAction = GetComponent<SpinAction>();
        _baseActionArray = GetComponents<BaseAction>();
    }

    private void Start()
    {
        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(_gridPosition, this);

        //subscribe to events
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        _healthSystem.OnDeath += HealthSystem_OnDeath;
    }

    void Update()
    {
        //check if unit changed grid position
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != _gridPosition)
        {
            LevelGrid.Instance.UnitMovedGridPosition(this, _gridPosition, newGridPosition);
            //update grid position
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
        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }

    // reset action points on next turn
    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if((IsEnemy() && !TurnSystem.Instance.IsPlayerTurn()) || !IsEnemy() && TurnSystem.Instance.IsPlayerTurn())
        {
            _actionPoints = _maxActionPoints;

            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private void HealthSystem_OnDeath(object sender, EventArgs e)
    {
        // cleanup grid and destroy gameobject
        LevelGrid.Instance.RemoveUnitAtGridPosition(_gridPosition, this);
        Destroy(gameObject);
    }

    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }

    public MoveAction GetMoveAction()
    {
        return _moveAction;
    }

    public SpinAction GetSpinAction() 
    {
        return _spinAction;
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

    public bool IsEnemy()
    {
        return _isEnemy;
    }
}
