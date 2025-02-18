using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class PlayerUnitActionSystemUI : MonoBehaviour
{
    [SerializeField]
    private GameObject _actionButtonPrefab;
    [SerializeField]
    private GameObject _actionButtonContainer;
    [SerializeField]
    private TextMeshProUGUI _actionPointsText;

    private List<ActionButtonUI> _actionButtonUIList;

    private void Awake()
    {
        _actionButtonUIList = new List<ActionButtonUI>();

        
    }

    private void Start()
    {
        //subscribe to OnSelectedUnitChanged event
        PlayerUnitActionSystem.Instance.OnSelectedUnitChanged += PlayerUnitActionSystem_OnSelectedUnitChanged;
        PlayerUnitActionSystem.Instance.OnSelectedActionChanged += PlayerUnitActionSystem_OnSelectedActionChanged;
        PlayerUnitActionSystem.Instance.OnActionStarted += PlayerUnitActionSystem_OnActionStarted;
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        PlayerUnit.OnAnyActionPointsChanged += PlayerUnit_OnAnyActionPointsChanged;

        CreateUnitActionButtons();
        UpdateSelectedVisual();
        UpdateAPText();
    }

    private void CreateUnitActionButtons()
    {
        //destroy existing action buttons
        foreach(Transform actionButton in _actionButtonContainer.transform)
        {
            Destroy(actionButton.gameObject);
        }

        _actionButtonUIList.Clear();

        // update button based on selected unit
        PlayerUnit selectedUnit = PlayerUnitActionSystem.Instance.GetSelectedUnit();
        if (selectedUnit != null)
        {
            // get all available action of selected unit
            foreach (BaseAction baseAction in selectedUnit.GetBaseActionArray())
            {
                GameObject actionButtonGO = Instantiate(_actionButtonPrefab, _actionButtonContainer.transform);
                ActionButtonUI actionButtonUI = actionButtonGO.GetComponent<ActionButtonUI>();
                actionButtonUI.SetBaseAction(baseAction);

                _actionButtonUIList.Add(actionButtonUI);
            }
        }
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        UpdateAPText();
    }

    private void PlayerUnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e)
    {
        CreateUnitActionButtons();
        UpdateSelectedVisual();
        UpdateAPText();
    }

    private void PlayerUnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
    {
        UpdateSelectedVisual();
    }

    private void PlayerUnitActionSystem_OnActionStarted(object sender, EventArgs e)
    {
        UpdateAPText();
    }
    private void PlayerUnit_OnAnyActionPointsChanged(object sender, EventArgs e)
    {
        UpdateAPText();
    }


    private void UpdateSelectedVisual()
    {
        foreach(ActionButtonUI actionButtonUI in _actionButtonUIList)
        {
            actionButtonUI.UpdateSelectedVisual();
        }
    }

    private void UpdateAPText()
    {
        PlayerUnit selectedUnit = PlayerUnitActionSystem.Instance.GetSelectedUnit();

        if (selectedUnit != null)
        {
            _actionPointsText.text = "Action Points: " + selectedUnit.GetActionPoints();
        }
    }
}
