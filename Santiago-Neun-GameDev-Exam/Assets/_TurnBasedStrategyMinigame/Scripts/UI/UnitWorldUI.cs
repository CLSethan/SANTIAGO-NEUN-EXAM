using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitWorldUI : MonoBehaviour
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
        //subscribe to events
        BaseUnit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;
        _healthSystem.OnDamaged += HealthSystem_OnDamaged;

        UpdateActionPointsText();
        UpdateHealthBar();
    }

    private void UpdateActionPointsText()
    {
        _actionPointsText.text = _unit.GetActionPoints().ToString();
    }

    private void Unit_OnAnyActionPointsChanged(object sender, EventArgs e)
    {
        UpdateActionPointsText();
    }

    private void UpdateHealthBar()
    {
        _healthBarImage.fillAmount = _healthSystem.GetHealthNormalized();
    }

    private void HealthSystem_OnDamaged(object sender, EventArgs e)
    {
        UpdateHealthBar();
    }

}
