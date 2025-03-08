using UnityEngine;

namespace NF.Main.Core.PlayerStateMachine
{
    public class UnitBaseState : BaseState
    {
        protected readonly BaseUnitController _baseUnitAnimator;
        protected readonly Animator _animator;

        protected static readonly int AimingHash = Animator.StringToHash("Aiming");
        protected static readonly int ShootingHash = Animator.StringToHash("Shooting");
        //protected static readonly int CooloffHash = Animator.StringToHash("Cooldown");
        protected static readonly int WalkingHash = Animator.StringToHash("Walking");


        protected UnitBaseState(BaseUnitController playerController, Animator animator)
        {
            _baseUnitAnimator = playerController;
            _animator = animator;
        }
    }

    public enum UnitState
    {
        Aiming,
        Shooting,
        Cooldown,
        Walking
    }
}

