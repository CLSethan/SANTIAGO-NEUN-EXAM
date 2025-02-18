using UnityEngine;
using NF.Main.Core;
using System;

public class TurnSystem : Singleton<TurnSystem>
{
    private int _turnNumber = 1;

    public event EventHandler OnTurnChanged;

    private void Awake()
    {
        Instance = this;
    }

    public void NextTurn()
    {
        _turnNumber++;
        OnTurnChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetTurnNumber()
    { 
        return _turnNumber; 
    }

}
