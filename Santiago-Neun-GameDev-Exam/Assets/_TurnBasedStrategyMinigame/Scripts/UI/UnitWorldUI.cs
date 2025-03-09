using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using NF.Main.Core;

public class UnitWorldUI : MonoExt
{
    [SerializeField] 
    private TextMeshProUGUI _actionPointsText;
    [SerializeField] 
    private BaseUnit _unit;
    [SerializeField] 
    private Image _healthBarImage;
    [SerializeField] 
    private HealthSystem _healthSystem;

    private void Start()
    {

        Initialize();
        OnSubscriptionSet();
        UpdateActionPointsText();
        UpdateHealthBar();
    }

    public override void OnSubscriptionSet()
    {
        base.OnSubscriptionSet();
        //subscribe to events
        AddEvent(BaseUnit.OnAnyActionPointsChanged, _ => UpdateActionPointsText());
        AddEvent(_healthSystem.OnDamaged, _ => UpdateHealthBar());

    }

    private void UpdateActionPointsText()
    {
        _actionPointsText.text = _unit.GetActionPoints().ToString();
    }

    private void UpdateHealthBar()
    {
        _healthBarImage.fillAmount = _healthSystem.GetHealthNormalized();
    }
}
