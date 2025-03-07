using NF.Main.Gameplay;
using UnityEngine;

namespace NF.Main.Core.GameStateMachine
{
    public class BusyState : TurnSystemBaseState
    {
        public BusyState(TurnSystem turnSystem, TurnState turnState) : base(turnSystem, turnState)
        {

        }

        public override void OnEnter()
        {
            base.OnEnter();
            Debug.Log("Doing Action!");

            if (!_turnSystem.IsPlayerTurn())
            {
                _turnSystem.SetStateEnemyTurn();
            }

            else
            {
                _turnSystem.SetStatePlayerTurn();
            }
        }

        public override void OnExit()
        {
            base.OnEnter();
            Debug.Log("Finished Action!");
        }
    }
}