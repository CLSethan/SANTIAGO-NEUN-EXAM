using UnityEngine;

namespace NF.Main.Core.PlayerStateMachine
{
    //Handles all logic for when player goes in, out, and during idle state
    public class UnitCooldownState : UnitBaseState
    {
        public UnitCooldownState(BaseUnitController unitAnimator, Animator animator) : base(unitAnimator, animator)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _animator.CrossFade(AimingHash, 0f);

            //Use this for transitioning between different animator hashes
            Debug.Log("Unit Cooldown");
        }

        public override void Update()
        {
            base.Update();
           // Debug.Log("Player is Idling");
        }

        public override void OnExit()
        {
            base.OnExit();
            //Debug.Log("Exiting Player Idle State");
        }
    }
}