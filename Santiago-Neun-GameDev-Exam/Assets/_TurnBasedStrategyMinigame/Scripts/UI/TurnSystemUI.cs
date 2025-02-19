using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField]
    private Button _endTurnButton;
    [SerializeField]
    private TextMeshProUGUI _turnNumberText;
    [SerializeField]
    private GameObject _enemyTurnVisual;

    private void Start()
    {
        _endTurnButton.onClick.AddListener(() =>
        {
            TurnSystem.Instance.NextTurn();
        });

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        UpdateTurnText();
        UpdateEnemyTurnVisual();
        UpdateEndTurnButton();
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
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
