using System;
using UnityEngine;
using NF.Main.Core;


public class PlayerUnitActionSystem : Singleton<PlayerUnitActionSystem>
{
    [SerializeField] private PlayerUnit _selectedUnit;
    [SerializeField] private LayerMask _unitLayerMask;


    public event EventHandler OnSelectedUnitChanged;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // avoid unit from moving simultaneously to selecting by returning if unit is already selected
            if (TryHandleUnitSelection()) return;

            if (_selectedUnit != null)
            {
                // convert mouse world position to grid position
                GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

                //move selected unit if grid position is valid
                if (_selectedUnit.GetMoveAction().IsValidActionGridPosition(mouseGridPosition))
                {
                    // get selected unit's move action and move to mouse position
                    _selectedUnit.GetMoveAction().Move(mouseGridPosition);
                }
            }
        }
    }

    // select unit on raycast
    private bool TryHandleUnitSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, _unitLayerMask))
        {
            if (hitInfo.transform.TryGetComponent<PlayerUnit>(out PlayerUnit unit))
            {
                SetSelectedUnit(unit);
                return true;
            }
        }
        return false;
    }
    //set selected unit
    private void SetSelectedUnit(PlayerUnit unit)
    {
        _selectedUnit = unit;
        //check for event subscribers and fire event
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);

    }
    // get selected unit
    public PlayerUnit GetSelectedUnit()
    {
        return _selectedUnit;
    }
}
