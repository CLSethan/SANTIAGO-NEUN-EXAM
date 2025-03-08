using NF.Main.Core;
using System;
using UnityEngine;

public class UnitRagdollSpawner : MonoExt
{
    [SerializeField]
    private GameObject _unitRagdollPrefab;
    [SerializeField]
    private Transform _originalRootBone;
    private HealthSystem _healthSystem;

    private void Awake()
    {
        _healthSystem = GetComponent<HealthSystem>();
    }

    private void Start()
    {
        Initialize();
        OnSubscriptionSet();
    }

    public override void OnSubscriptionSet()
    {
        base.OnSubscriptionSet();
        //subscribe to events

        AddEvent(_healthSystem.OnDeath, _ => SpawnRagdoll());

    }
    private void SpawnRagdoll()
    {
        GameObject ragdollGO = Instantiate(_unitRagdollPrefab, transform.position, transform.rotation);
        UnitRagdoll unitRagdoll = ragdollGO.GetComponent<UnitRagdoll>();
        unitRagdoll.Setup(_originalRootBone);
    }
}
