using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using NF.Main.Gameplay;
using NF.Main.Core;

public class TurnSystemUI : MonoExt
{
    [SerializeField]
    private Button _endTurnButton;
    [SerializeField]
    private TextMeshProUGUI _turnNumberText;
    [SerializeField]
    private GameObject _enemyTurnVisual;

    private void Start()
    {

        Initialize();
        OnSubscriptionSet();

        _endTurnButton.onClick.AddListener(() =>
        {
            TurnSystem.Instance.NextTurn();
        });

        UpdateTurnSystemUI();
    }

    public override void OnSubscriptionSet()
    {
        base.OnSubscriptionSet();
        AddEvent(TurnSystem.Instance.OnTurnChanged, _ => UpdateTurnSystemUI());
    }

    private void UpdateTurnSystemUI()
    {
        UpdateTurnText();
        UpdateEnemyTurnVisual();
        UpdateEndTurnButton();
    }

    private void UpdateTurnText()
    {
        _turnNumberText.text = "TURN: " + TurnSystem.Instance.GetTurnNumber();
    }

    private void UpdateEnemyTurnVisual()
    {
        _enemyTurnVisual.SetActive(!TurnSystem.Instance.IsPlayerTurn());
    }

    private void UpdateEndTurnButton()
    {
        _endTurnButton.gameObject.SetActive(TurnSystem.Instance.IsPlayerTurn());
    }
}
