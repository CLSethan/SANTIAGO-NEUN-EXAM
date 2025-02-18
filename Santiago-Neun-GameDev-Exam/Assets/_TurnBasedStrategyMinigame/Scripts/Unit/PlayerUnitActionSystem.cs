using System;
using UnityEngine;
using NF.Main.Core;
using UnityEngine.EventSystems;


public class PlayerUnitActionSystem : Singleton<PlayerUnitActionSystem>
{

    [SerializeField] private PlayerUnit _selectedUnit;
    [SerializeField] private BaseAction _selectedAction;
    [SerializeField] private LayerMask _unitLayerMask;

    private bool _isBusy;

    //event handlers
    public event EventHandler OnSelectedUnitChanged;
    public event EventHandler OnSelectedActionChanged;
    public event EventHandler<bool> OnBusyChanged;
    public event EventHandler OnActionStarted;


    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        SetSelectedUnit(_selectedUnit);
    }

    void Update()
    {
        if(_isBusy)
        {
            return;
        }

        // for checking if there is UI over a position
        if(EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (TryHandleUnitSelection())
        {
            return;
        }

        HandleSelectedAction();

    }

    private void HandleSelectedAction()
    {
        if(Input.GetMouseButton(0))
        {
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

            //refactored action code block
            /*
            switch(_selectedAction)
            {
                case MoveAction moveAction:
                    if(moveAction.IsValidActionGridPosition(mouseGridPosition))
                    {
                        SetBusy();
                        moveAction.Move(mouseGridPosition, ClearBusy);
                    }
                    break;
                case SpinAction spinAction:
                    SetBusy();
                    spinAction.Spin(ClearBusy);
                    break;

            }
            */

            if(_selectedAction.IsValidActionGridPosition(mouseGridPosition))
            {
                //take action if selected unit has enough action points
                if (_selectedUnit.TrySpendAP(_selectedAction))
                {
                    SetBusy();
                    // take action then clear busy using base action delegate
                    _selectedAction.TakeAction(mouseGridPosition, ClearBusy);

                    //check for event subscribers and fire event
                    OnActionStarted?.Invoke(this, EventArgs.Empty);

                }
            }
        }
    }

    // select unit on raycast
    private bool TryHandleUnitSelection()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, _unitLayerMask))
            {
                if (hitInfo.transform.TryGetComponent<PlayerUnit>(out PlayerUnit unit))
                {
                    // return false if unit is same as selected unit
                    if(unit == _selectedUnit)
                    {
                        return false;
                    }
                    SetSelectedUnit(unit);
                    return true;
                }
            }
        }
        return false;
    }

    //set selected unit
    private void SetSelectedUnit(PlayerUnit unit)
    {
        _selectedUnit = unit;
        if(_selectedUnit!= null)
        {
            SetSelectedAction(_selectedUnit.GetMoveAction());
        }

        //check for event subscribers and fire event
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    // get selected unit
    public PlayerUnit GetSelectedUnit()
    {
        return _selectedUnit;
    }

    public void SetSelectedAction(BaseAction baseAction)
    {
        _selectedAction = baseAction;
        //check for event subscribers and fire event
        OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);

    }
    public BaseAction GetSelectedAction()
    {
        return _selectedAction;
    }

    private void SetBusy()
    {
        _isBusy = true;
        //check for event subscribers and fire event
        OnBusyChanged?.Invoke(this, _isBusy);

    }

    private void ClearBusy()
    {
        _isBusy = false;
        //check for event subscribers and fire event
        OnBusyChanged?.Invoke(this, _isBusy);
    }
}
