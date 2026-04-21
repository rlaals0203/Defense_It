using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _01_Script._00_Core.EventChannel;
using _01_Script._00_Core.StatSystem;
using _01_Script._01_Entities;
using _01_Script._01_Entities.Stat;
using _01_Script._02_Unit;
using _01_Script.Entities;
using UnityEngine;

public class ApplyUnitBuff : MonoBehaviour, IEntityComponent, IAfterInitialize
{
    [SerializeField] private GameEventChannelSO eventChannel;
    [SerializeField] private StatSO attackStat;
    [SerializeField] private StatSO rangeStat;
    [SerializeField] private float upgradeValue;
    [SerializeField] private LayerMask whatIsUnit;

    private bool _updateFlag = false;
    private float _range;
    private BuffUnit _unit;
    private EntityStat _statCompo;
    private HashSet<Unit> _unitList;
    private readonly Collider[] _targets = new Collider[16];

    public void Initialize(Entity entity)
    {
        _statCompo = entity.GetCompo<EntityStat>();
        _unit = entity as BuffUnit;
        _unitList = new HashSet<Unit>();
    }

    public void AfterInitialize()
    {
        eventChannel.AddListener<UnitPlaceEndEvent>(HandleUnitPlace);
    }
    
    private void Start()
    {
        _unit.ShowRangeGizmo(true, _statCompo.GetStat(rangeStat).Value);
    }
    
    private void OnDestroy()
    {
        eventChannel.RemoveListener<UnitPlaceEndEvent>(HandleUnitPlace);
        _statCompo.UnSubscribeStat(rangeStat, HandleRangeChange);
        _unitList.ToList().ForEach(u => u.GetCompo<EntityStat>().ClearAllStatModifier());
    }
    
    private void HandleRangeChange(StatSO stat, float currentvalue, float prevvalue)
    {
        _range = currentvalue;
        _updateFlag = true;
    }

    private void HandleUnitPlace(UnitPlaceEndEvent evt)
    {
        _updateFlag = true;
    }

    private void LateUpdate()
    {
        if(_updateFlag)
            ApplyBuff(_range);
    }

    public void ApplyBuff(float range)
    {
        RemoveBuffs();
        int count = Physics.OverlapSphereNonAlloc(_unit.transform.position,
            range / 2f, _targets, whatIsUnit);
        
        var buffKey = $"BuffUnit_{_unit.GetInstanceID()}";
    
        if (count < 1)
        {
            foreach (var unit in _unitList)
            {
                UnitStatManager statManager = unit.GetCompo<UnitStatManager>();
                statManager.RemoveUnitStat(attackStat, buffKey);
            }
            _unitList.Clear();
            return;
        }

        var currentUnits = new HashSet<Unit>();

        for (int i = 0; i < count; i++)
        {
            Collider col = _targets[i];
            if (col == null) continue;

            if (col.TryGetComponent(out Unit unit))
            {
                currentUnits.Add(unit);
                
                if (_unitList.Contains(unit) == false)
                {
                    UnitStatManager statManager = unit.GetCompo<UnitStatManager>();
                    statManager.ModifyUnitStat(attackStat, buffKey, upgradeValue);
                }
            }
        }

        foreach (var unit in _unitList)
        {
            if (currentUnits.Contains(unit) == false)
            {
                UnitStatManager statManager = unit.GetCompo<UnitStatManager>();
                statManager.RemoveUnitStat(attackStat, buffKey);
            }
        }

        _unitList = currentUnits;
    }

    private void RemoveBuffs()
    {
        _unitList?.ToList().ForEach(u => u.GetCompo<EntityStat>().ClearAllStatModifier());
        _unitList?.Clear();
    }
}
