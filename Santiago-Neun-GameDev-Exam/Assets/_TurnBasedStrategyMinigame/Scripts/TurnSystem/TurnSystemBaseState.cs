using NF.Main.Gameplay;

namespace NF.Main.Core.GameStateMachine
{
    public class TurnSystemBaseState : BaseState
    {
        protected readonly TurnSystem _turnSystem;
        protected readonly TurnState _turnState;

        protected TurnSystemBaseState(TurnSystem turnSystem, TurnState turnState)
        {
            _turnSystem = turnSystem;
            _turnState = turnState;
        }
    }
}

public enum TurnState
{
    PlayerTurn,
    EnemyTurn,
    Busy,
}