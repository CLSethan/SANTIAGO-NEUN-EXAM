using NF.Main.Gameplay;
using System;
using UnityEngine;

namespace NF.Main.Core.GameStateMachine
{
    public class EnemyTurn : TurnSystemBaseState
    {
        public static event Action<Action> OnEnemyTurnStart;

        private float _timer;

        public EnemyTurn(TurnSystem turnSystem, TurnState turnState) : base(turnSystem, turnState) { }

        public override void OnEnter()
        {
            base.OnEnter();
            Debug.Log("Enemy's Turn!");
            _timer = 2f; // Set initial timer duration


        }

        public override void Update()
        {
            if (_turnSystem.GetCurrentState() == TurnState.PlayerTurn)
            {
                return;
            }

            if(_turnSystem.GetCurrentState() == TurnState.Busy)
            {
                return;
            }

            _timer -= Time.deltaTime;
            if (_timer > 0f)
                return;

            if (TryTakeEnemyAIAction(_turnSystem.SetStateEnemyTurn))
            {
                _turnSystem.SetStateBusy();
            }

            //all enemies finished possible actions
            else
            {
                _turnSystem.NextTurn();
                // idk if this should be handled by TurnSystem or if enemy turn should transition to player turn
                //_turnSystem.SetStatePlayerTurn();

            }

            //switch (_turnSystem.TurnState)
            //{
            //    case TurnState.WaitingForEnemyTurn:
            //        Debug.Log("Still Player Turn");
            //        break;
            //    case TurnState.EnemyTakingTurn:
            //        Debug.Log("Still Enemy Turn");

            //        _timer -= Time.deltaTime;
            //        if (_timer <= 0f)
            //        {
            //            //set busy state and take action
            //            if (TryTakeEnemyAIAction(_turnSystem.SetStateEnemyTakingTurn))
            //            {
            //                _turnSystem.SetStateBusy();
            //            }
            //            else
            //            {
            //                // No more enemies have actions they can take, end enemy turn
            //                _turnSystem.NextTurn();
            //                _turnSystem.SetStateWaitingForEnemyTurn();
            //            }

            //        }
            //        break;
            //    case TurnState.Busy:
            //        Debug.Log("Enemy Busy");

            //        break;
            //}
        }

        private bool TryTakeEnemyAIAction(Action onEnemyAIActionComplete)
        {
            // have enemy unit in list to take action

            foreach (BaseUnit enemyUnit in UnitManager.Instance.GetEnemyUnitList())
            {
                if (TryTakeEnemyAIAction(enemyUnit, onEnemyAIActionComplete))
                    return true;
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
                    continue;

                EnemyAIAction testEnemyAIAction = baseAction.GetBestEnemyAIAction();
                if (testEnemyAIAction != null && (bestEnemyAIAction == null || testEnemyAIAction.actionValue > bestEnemyAIAction.actionValue))
                {
                    bestEnemyAIAction = testEnemyAIAction;
                    bestBaseAction = baseAction;
                }
            }

            if (bestEnemyAIAction != null && enemyUnit.TrySpendAP(bestBaseAction))
            {
                bestBaseAction.TakeAction(bestEnemyAIAction.gridPosition, () => _turnSystem.SetStateBusy());
                return true;
            }
            return false;
        }

        public override void OnExit()
        {
            base.OnExit();
            Debug.Log("Enemy Turn Finished");

        }
    }
}