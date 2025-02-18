using System;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    protected PlayerUnit _unit;
    protected bool _isActive;

    //clear unit actions using delegate
    protected Action _onActionComplete;

    protected virtual void Awake()
    {
        _unit = GetComponent<PlayerUnit>();
    }

}
