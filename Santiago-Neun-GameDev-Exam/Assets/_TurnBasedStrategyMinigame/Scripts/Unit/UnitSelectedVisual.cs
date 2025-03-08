using System;
using UnityEngine;
using UniRx;
using NF.Main.Core;

public class UnitSelectedVisual : MonoExt
{
    [SerializeField]
    private BaseUnit _unit;
    private MeshRenderer _meshRenderer;

    public override void Initialize(object data = null)
    {
        base.Initialize(data);
        _meshRenderer = GetComponent<MeshRenderer>();
        OnSubscriptionSet();
        UpdateVisual();

    }

    public override void OnSubscriptionSet()
    {
        base.OnSubscriptionSet();
        AddEvent(BaseUnitActionSystem.Instance.OnSelectedUnitChanged, _ => UpdateVisual());
    }

    private void UpdateVisual()
    {
        if (BaseUnitActionSystem.Instance.GetSelectedUnit() == _unit)
        {
            _meshRenderer.enabled = true;
        }
        else
        {
            _meshRenderer.enabled = false;
        }
    }
}
