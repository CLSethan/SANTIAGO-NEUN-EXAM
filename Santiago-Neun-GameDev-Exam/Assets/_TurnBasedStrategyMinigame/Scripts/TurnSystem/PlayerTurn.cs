using NF.Main.Gameplay;
using UnityEngine;

namespace NF.Main.Core.GameStateMachine
{
    public class PlayerTurn : TurnSystemBaseState
    {
        public PlayerTurn(TurnSystem turnSystem, TurnState turnState) : base(turnSystem, turnState)
        {

        }

        public override void OnEnter()
        {
            base.OnEnter();
            Debug.Log("Player's Turn!");
            _turnSystem.SetIsPlayerTurn(true);

        }

        public override void OnExit()
        {
            base.OnExit();
            Debug.Log("Player Turn Finished");
            _turnSystem.SetIsPlayerTurn(false);

        }
    }
}