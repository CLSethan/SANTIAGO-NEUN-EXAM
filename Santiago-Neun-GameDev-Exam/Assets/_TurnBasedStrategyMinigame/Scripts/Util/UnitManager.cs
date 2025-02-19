using NF.Main.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : Singleton<UnitManager>
{

    private List<BaseUnit> _unitList;
    private List<BaseUnit> _friendlyUnitList;
    private List<BaseUnit> _enemyUnitList;

    private void Awake()
    {
        Instance = this;

        _unitList = new List<BaseUnit>();
        _friendlyUnitList = new List<BaseUnit>();
        _enemyUnitList = new List<BaseUnit>();
    }

    private void Start()
    {
        //subscribe to events
        BaseUnit.OnAnyUnitSpawned += Unit_OnAnyUnitSpawned;
        BaseUnit.OnAnyUnitDead += Unit_OnAnyUnitDead;
    }

    private void Unit_OnAnyUnitSpawned(object sender, EventArgs e)
    {
        BaseUnit unit = sender as BaseUnit;

        _unitList.Add(unit);
        if (unit.IsEnemy())
        {
            _enemyUnitList.Add(unit);
        }
        else
        {
            _friendlyUnitList.Add(unit);
        }
    }

    private void Unit_OnAnyUnitDead(object sender, EventArgs e)
    {
        BaseUnit unit = sender as BaseUnit;

        _unitList.Remove(unit);

        if (unit.IsEnemy())
        {
            _enemyUnitList.Remove(unit);
        }
        else
        {
            _friendlyUnitList.Remove(unit);
        }
    }

    public List<BaseUnit> GetUnitList()
    {
        return _unitList;
    }

    public List<BaseUnit> GetFriendlyUnitList()
    {
        return _friendlyUnitList;
    }

    public List<BaseUnit> GetEnemyUnitList()
    {
        return _enemyUnitList;
    }

}
