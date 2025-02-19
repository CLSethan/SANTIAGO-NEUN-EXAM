namespace NF.Main.Core.GameStateMachine
{
    public class TurnSystemBaseState : BaseState
    {

    }
}

public enum TurnState
{
    WaitingForEnemyTurn,
    TakingTurn,
    Busy,
}