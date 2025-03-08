using UnityEngine;

namespace NF.Main.Core.PlayerStateMachine
{
    //Handles all logic for when player goes in, out, and during idle state
    public class UnitWalkingState : UnitBaseState
    {
        public UnitWalkingState(BaseUnitController unitAnimator, Animator animator) : base(unitAnimator, animator)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _animator.CrossFade(WalkingHash, 0f);

            //Use this for transitioning between different animator hashes
            Debug.Log("Unit is Walking");
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
            //_animator.CrossFade(AimingHash, 0f);

        }
    }
}