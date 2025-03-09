using NF.Main.Core;
using UnityEngine;

public class BulletProjectile : MonoExt
{
    private Vector3 _targetPositon;
    [SerializeField]
    private float _projectileSpeed = 200f;
    [SerializeField]
    private TrailRenderer _trailRenderer;
    [SerializeField]
    private GameObject _bulletHitVFX;
   public void Setup(Vector3 targetPosition)
   {
        _targetPositon = targetPosition;   
   }

    private void Update()
    {
        MoveTowardsTarget();
    }

    private void MoveTowardsTarget()
    {

        Vector3 moveDir = (_targetPositon - transform.position).normalized;
        float distanceBeforeMove = Vector3.Distance(transform.position, _targetPositon);
        transform.position += moveDir * _projectileSpeed * Time.deltaTime;
        float distanceAfterMove = Vector3.Distance(transform.position, _targetPositon);

        //check for overshot
        if (distanceBeforeMove < distanceAfterMove)
        {
            transform.position = _targetPositon;
            _trailRenderer.transform.parent = null;
            Destroy(gameObject);
            Instantiate(_bulletHitVFX, _targetPositon, Quaternion.identity);
        }
    }
}
