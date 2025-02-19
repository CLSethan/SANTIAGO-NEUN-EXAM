using UnityEngine;

public class UnitRagdoll : MonoBehaviour
{
    [SerializeField]
    private Transform _ragdollRootBone;
    [SerializeField]
    private float _explosionForce = 300f;
    [SerializeField]
    private float _explosionRange = 10f;

    public void Setup(Transform originalRootBone)
    {
        MatchAllChildTransforms(originalRootBone, _ragdollRootBone);
        ApplyExplosionToRagdoll(_ragdollRootBone, _explosionForce, transform.position, _explosionRange);
    }

    //match bones to ragdoll
    private void MatchAllChildTransforms(Transform root, Transform clone)
    {
        foreach (Transform child in root)
        {
            Transform cloneChild = clone.Find(child.name);
            if(cloneChild != null)
            {
                cloneChild.position = child.position;
                cloneChild.rotation = child.rotation;

                MatchAllChildTransforms (child, cloneChild);
            }
        }
    }

    private void ApplyExplosionToRagdoll(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRange)
    {
        foreach (Transform child in root)
        {
            if(child.TryGetComponent<Rigidbody>(out Rigidbody childRigidBody))
            {
                childRigidBody.AddExplosionForce(explosionForce, explosionPosition, explosionRange);
            }

            ApplyExplosionToRagdoll(child, explosionForce, explosionPosition, explosionRange);
        }

    }
}
