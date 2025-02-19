using System;
using UnityEngine;

public class UnitRagdollSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _unitRagdollPrefab;
    [SerializeField]
    private Transform _originalRootBone;
    private HealthSystem _healthSystem;

    private void Awake()
    {
        _healthSystem = GetComponent<HealthSystem>();
        _healthSystem.OnDeath += HealthSystem_OnDeath;
    }

    private void HealthSystem_OnDeath(object sender, EventArgs e)
    {
        GameObject ragdollGO = Instantiate(_unitRagdollPrefab, transform.position, transform.rotation);
        UnitRagdoll unitRagdoll = ragdollGO.GetComponent<UnitRagdoll>();
        unitRagdoll.Setup(_originalRootBone);
    }
}
