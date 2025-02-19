using NF.Main.Core;
using System;

public class TurnSystem : Singleton<TurnSystem>
{
    private int _turnNumber = 1;
    private bool _isPlayerTurn = true;
    
    public TurnState _turnState;

    public event EventHandler OnTurnChanged;
    private void Awake()
    {
        Instance = this;
    }

    public void NextTurn()
    {
        _turnNumber++;
        _isPlayerTurn = !_isPlayerTurn;
        OnTurnChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetTurnNumber()
    { 
        return _turnNumber; 
    }

    public bool IsPlayerTurn()
    {
        return _isPlayerTurn;
    }
}
