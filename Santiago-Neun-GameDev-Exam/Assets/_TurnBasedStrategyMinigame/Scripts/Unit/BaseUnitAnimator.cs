using System;
using UnityEngine;

public class BaseUnitAnimator : MonoBehaviour
{
    [SerializeField]
    private Animator _unitAnimator;
    [SerializeField]
    private GameObject _bulletProjectilePrefab;
    [SerializeField]
    private Transform _shootPoint;


    private void Awake()
    {
        if(TryGetComponent<MoveAction>(out MoveAction moveAction))
        {
            moveAction.OnStartMoving += MoveAction_OnStartMoving;
            moveAction.OnStopMoving += MoveAction_OnStopMoving;
        }

        if (TryGetComponent<ShootAction>(out ShootAction shootAction))
        {
            shootAction.OnShoot += ShootAction_OnShoot;

        }
    }

    private void ShootAction_OnShoot(object sender, ShootAction.OnShootEventArgs e)
    {
        _unitAnimator.SetTrigger("Shoot");

        GameObject bulletProjectileGO = Instantiate(_bulletProjectilePrefab, _shootPoint.position, Quaternion.identity);
        BulletProjectile bulletProjectile = bulletProjectileGO.GetComponent<BulletProjectile>();

        //shoot at same level as shoot point
        Vector3 targetUnitShootAtPoint = e.targetUnit.GetWorldPosition();
        targetUnitShootAtPoint.y = _shootPoint.position.y;

        bulletProjectile.Setup(targetUnitShootAtPoint);

    }

    private void MoveAction_OnStartMoving(object sender, EventArgs e)
    {
        _unitAnimator.SetBool("isWalking", true);
    }

    private void MoveAction_OnStopMoving(object sender, EventArgs e)
    {
        _unitAnimator.SetBool("isWalking", false);
    }
}
