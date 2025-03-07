using NF.Main.Core;
using System;
using NF.Main.Core.GameStateMachine;

namespace NF.Main.Gameplay
{
    public class TurnSystem : Singleton<TurnSystem>
    {
        public TurnState TurnState;
        public StateMachine _stateMachine;
        public event EventHandler OnTurnChanged;

        private int _turnNumber = 1;
        private bool _isPlayerTurn = true;

        private void Awake()
        {
            Initialize();
            Instance = this;

        }

        private void Update()
        {
            _stateMachine.Update();
        }

        public override void Initialize(object data = null)
        {
            base.Initialize(data);
            TurnState = TurnState.PlayerTurn;
            SetupStateMachine();
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


            //At(playingState, gameOverState, new FuncPredicate(() => GameState == GameState.GameOver));


            // Any(playingState, new FuncPredicate(() => GameState == GameState.Playing));
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
                _turnNumber++;
                SetStateEnemyTurn();
            }

            else
            {
                SetStatePlayerTurn();
            }

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

        public bool SetIsPlayerTurn(bool isPlayerTurn)
        {
            return _isPlayerTurn = isPlayerTurn;
        }

        public void SetStateEnemyTurn()
        {
            TurnState = TurnState.EnemyTurn;
        }

        public void SetStateBusy()
        {
            TurnState = TurnState.Busy;

        }

        public void SetStatePlayerTurn()
        {
            TurnState = TurnState.PlayerTurn;

        }

        public TurnState GetCurrentState()
        {
            return TurnState;
        }
    }
}