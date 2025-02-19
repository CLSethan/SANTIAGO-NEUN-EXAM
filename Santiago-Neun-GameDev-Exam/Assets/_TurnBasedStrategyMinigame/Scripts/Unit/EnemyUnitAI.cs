using NF.Main.Core.GameStateMachine;
using System;
using System.Threading;
using UnityEngine;

public class EnemyUnitAI : MonoBehaviour
{
    public TurnState _turnState;
    private float _timer;

    private void Awake()
    {
        _turnState = TurnState.WaitingForEnemyTurn;
    }

    private void Start()
    {
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    private void Update()
    {
        if(TurnSystem.Instance.IsPlayerTurn())
        {
            return;
        }
        switch(_turnState)
        {
            case TurnState.WaitingForEnemyTurn:
                break;
            case TurnState.TakingTurn:
                _timer -= Time.deltaTime;
                if (_timer <= 0f)
                {
                    //set busy state and take action
                    if (TryTakeEnemyAIAction(SetStateTakingTurn))
                    {
                        _turnState = TurnState.Busy;
                    }
                    else
                    {
                        // No more enemies have actions they can take, end enemy turn
                        TurnSystem.Instance.NextTurn();
                    }

                }
                break;
            case TurnState.Busy:
                break;
        }
    }

    private void SetStateTakingTurn()
    {
        _timer = 0.5f;
        _turnState = TurnState.TakingTurn;
    }

    private bool TryTakeEnemyAIAction(Action onEnemyAIActionComplete)
    {
        Debug.Log("Take Enemy AI Action");

        // have enemy unit in list to take action
        foreach (BaseUnit enemyUnit in UnitManager.Instance.GetEnemyUnitList())
        {
            if (TryTakeEnemyAIAction(enemyUnit, onEnemyAIActionComplete))
            {
                return true;
            }
        }

        return false;
    }

    // take enemy action for individual enemy units
    private bool TryTakeEnemyAIAction(BaseUnit enemyUnit, Action onEnemyAIActionComplete)
    {
        EnemyAIAction bestEnemyAIAction = null;
        BaseAction bestBaseAction = null;

        foreach (BaseAction baseAction in enemyUnit.GetBaseActionArray())
        {
            if (!enemyUnit.CanSpendAP(baseAction))
            {
                // Enemy cannot afford this action
                continue;
            }

            // get best enemy action
            if (bestEnemyAIAction == null)
            {
                bestEnemyAIAction = baseAction.GetBestEnemyAIAction();
                bestBaseAction = baseAction;
            }

            else
            {
                EnemyAIAction testEnemyAIAction = baseAction.GetBestEnemyAIAction();
                // if enemy can take action and compare values
                if (testEnemyAIAction != null && testEnemyAIAction.actionValue > bestEnemyAIAction.actionValue)
                {
                    bestEnemyAIAction = testEnemyAIAction;
                    bestBaseAction = baseAction;
                }
            }
        }

        // check if enemy has enough AP and has a best enemy action
        if (bestEnemyAIAction != null && enemyUnit.TrySpendAP(bestBaseAction))
        {
            bestBaseAction.TakeAction(bestEnemyAIAction.gridPosition, onEnemyAIActionComplete);
            return true;
        }

        else
        {
            return false;
        }
    }


    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if(!TurnSystem.Instance.IsPlayerTurn())
        {
            _turnState = TurnState.TakingTurn;
            _timer = 2f;

        }
    }


}
