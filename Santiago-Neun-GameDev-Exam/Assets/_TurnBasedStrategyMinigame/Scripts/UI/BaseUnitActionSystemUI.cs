using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using NF.Main.Gameplay;
using NF.Main.Core;

public class BaseUnitActionSystemUI : MonoExt
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
        Initialize();

        OnSubscriptionSet();
        UpdateUI();
    }

    public override void OnSubscriptionSet()
    {
        base.OnSubscriptionSet();

        AddEvent(BaseUnitActionSystem.Instance.OnSelectedUnitChanged, _ => UpdateUI());
        AddEvent(BaseUnitActionSystem.Instance.OnSelectedActionChanged, _ => UpdateSelectedVisual());
        AddEvent(BaseUnitActionSystem.Instance.OnActionStarted, _ => UpdateAPText());
        AddEvent(BaseUnit.OnAnyActionPointsChanged, _ => UpdateAPText());
        AddEvent(TurnSystem.Instance.OnTurnChanged, _ => UpdateAPText());

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
        BaseUnit selectedUnit = BaseUnitActionSystem.Instance.GetSelectedUnit();
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

    private void UpdateUI()
    {
        CreateUnitActionButtons();
        UpdateSelectedVisual();
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
        BaseUnit selectedUnit = BaseUnitActionSystem.Instance.GetSelectedUnit();

        if (selectedUnit != null)
        {
            _actionPointsText.text = "Action Points: " + selectedUnit.GetActionPoints();
        }
    }
}
