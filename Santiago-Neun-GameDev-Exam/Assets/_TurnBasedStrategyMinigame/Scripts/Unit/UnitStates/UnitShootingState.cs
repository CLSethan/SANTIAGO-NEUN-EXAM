using NF.Main.Gameplay;
using NF.Main.Gameplay.PlayerInput;
using UnityEngine;

namespace NF.Main.Core.PlayerStateMachine
{
    //Handles all logic for when player goes in, out, and during idle state
    public class UnitShootingState : UnitBaseState
    {
        private GameObject _bulletPrefab;
        private Transform _shootPoint;
        private BaseUnit _targetUnit;

        public UnitShootingState(BaseUnitController unitAnimator, Animator animator, GameObject bulletPrefab, Transform shootPoint) : base(unitAnimator, animator)
        {
            _bulletPrefab = bulletPrefab;
            _shootPoint = shootPoint;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            //Use this for transitioning between different animator hashes
            Debug.Log("Unit entered Shoot State");

            _animator.CrossFade(ShootingHash, 0f);

            GameObject bulletGO = UnityEngine.Object.Instantiate(_bulletPrefab, _shootPoint.position, Quaternion.identity);
            BulletProjectile bullet = bulletGO.GetComponent<BulletProjectile>();

            // Adjust bullet to shoot at the same height as the shoot point
            Vector3 targetPosition = _targetUnit.GetWorldPosition();
            targetPosition.y = _shootPoint.position.y;

            bullet.Setup(targetPosition);
        }

        public override void Update()
        {

            base.Update();
            Debug.Log("Unit Shooting");
        }

        public override void OnExit()
        {
            base.OnExit();
            Debug.Log("Shooting Finished");
        }

        public void SetTarget(BaseUnit targetUnit)
        {
            _targetUnit = targetUnit;
        }
    }
}