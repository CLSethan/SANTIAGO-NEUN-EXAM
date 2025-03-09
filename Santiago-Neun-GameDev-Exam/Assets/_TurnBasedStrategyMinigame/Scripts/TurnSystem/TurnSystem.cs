using NF.Main.Core;
using System;
using NF.Main.Core.GameStateMachine;
using UniRx;

namespace NF.Main.Gameplay
{
    public class TurnSystem : Singleton<TurnSystem>
    {
        public TurnState TurnState;
        public StateMachine _stateMachine;

        public Subject<Unit> OnTurnChanged;

        private int _turnNumber = 1;
        private bool _isPlayerTurn = true;

        private void Awake()
        {
            Instance = this;
            OnTurnChanged = new Subject<Unit>();
        }

        private void Start()
        {
            Initialize();
            TurnState = TurnState.PlayerTurn;
            SetupStateMachine();
        }

        private void Update()
        {
            _stateMachine.Update();
        }

        private void SetupStateMachine()
        {
            // State Machine
            _stateMachine = new StateMachine();

            // Declare states
            var busyState = new BusyState(this, TurnState.Busy);
            var playerTurn = new PlayerTurn(this, TurnState.PlayerTurn);
            var enemyTurn = new EnemyTurn(this, TurnState.EnemyTurn);


            // Define transitions
            At(playerTurn, enemyTurn, new FuncPredicate(() => TurnState == TurnState.EnemyTurn));
            At(enemyTurn, playerTurn, new FuncPredicate(() => TurnState == TurnState.PlayerTurn));
            At(enemyTurn, busyState, new FuncPredicate(() => TurnState == TurnState.Busy));
            At(busyState, enemyTurn, new FuncPredicate(() => TurnState == TurnState.EnemyTurn));

            Any(playerTurn, new FuncPredicate(() => TurnState == TurnState.PlayerTurn));
           
            // Set initial state
            _stateMachine.SetState(playerTurn);
        }

        private void At(IState from, IState to, IPredicate condition) => _stateMachine.AddTransition(from, to, condition);
        private void Any(IState to, IPredicate condition) => _stateMachine.AddAnyTransition(to, condition);

        public void NextTurn()
        {
            if (_isPlayerTurn)
            {
                _isPlayerTurn = false;
                SetStateEnemyTurn();
            }

            else
            {
                _turnNumber++;
                _isPlayerTurn = true;
                SetStatePlayerTurn();
            }

            OnTurnChanged.OnNext(Unit.Default);
        }

        public TurnState GetCurrentState() => TurnState;

        public int GetTurnNumber() => _turnNumber;

        public bool IsPlayerTurn() => _isPlayerTurn;

        public void SetIsPlayerTurn(bool isPlayerTurn) => _isPlayerTurn = isPlayerTurn;

        public void SetStateEnemyTurn() => TurnState = TurnState.EnemyTurn;

        public void SetStateBusy() => TurnState = TurnState.Busy;

        public void SetStatePlayerTurn() => TurnState = TurnState.PlayerTurn;
    }
}