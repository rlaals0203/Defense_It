using _01_Script._00_Core.EventChannel;
using _01_Script._01_Entities;
using _01_Script._02_Unit;
using UnityEngine;

public class UnitGameEventChannel : MonoBehaviour
{
    public static readonly ChangeSelectStatement ChangeSelectStatement = new ChangeSelectStatement();
    public static readonly UnitSelectEvent UnitSelectEvent = new UnitSelectEvent();
    public static readonly UnitPlaceStartEvent UnitPlaceStartEvent = new UnitPlaceStartEvent();
    public static readonly UnitPlaceEndEvent UnitPlaceEndEvent = new UnitPlaceEndEvent();
    public static readonly UnitUpgradeEvent UnitUpgradeEvent = new UnitUpgradeEvent();
    public static readonly UnitPlaceCancelEvent UnitPlaceCancelEvent = new UnitPlaceCancelEvent();
    public static readonly UnitSellEvent UnitSellEvent = new UnitSellEvent();
    public static readonly UnitMergeSelectEvent UnitMergeSelectEvent = new UnitMergeSelectEvent();
    public static readonly UnitMergeEvent UnitMergeEvent = new UnitMergeEvent();
    public static readonly UnitSkillUseEvent UnitSkillUseEvent = new UnitSkillUseEvent();
    public static readonly ChangeSFXEvent ChangeSFXEvent = new ChangeSFXEvent();
}
    
public class UnitSelectEvent : GameEvent
{
    public Unit selectedUnit;
    public EntitySkill unitSkill;

    public UnitSelectEvent Initializer(Unit unit, EntitySkill skill)
    {
        selectedUnit = unit;
        unitSkill = skill;
        return this;
    }
}
public class UnitPlaceStartEvent : GameEvent
{
    public EntitySO selectedUnit;
    public GameObject gizmo;
    
    public UnitPlaceStartEvent Initializer(EntitySO unit)
    {
        selectedUnit = unit;
        return this;
    }
}
public class UnitUpgradeEvent : GameEvent
{
    public Unit selectedUnit;
    
    public UnitUpgradeEvent Initializer(Unit unit)
    {
        selectedUnit = unit;
        return this;
    }
}
public class ChangeSelectStatement : GameEvent
{
    public bool isSelectMode;
    public ChangeSelectStatement Initializer(bool selectMode = false)
    {
        isSelectMode = selectMode;
        return this;
    }
}
public class UnitPlaceEndEvent : GameEvent { }
public class UnitMergeSelectEvent : GameEvent { }
public class UnitMergeEvent : GameEvent { }
public class UnitPlaceCancelEvent : GameEvent { }
public class UnitSkillUseEvent : GameEvent { }
public class UnitSellEvent : GameEvent
{
    public Unit selectedUnit;
    
    public UnitSellEvent Initializer(Unit unit)
    {
        selectedUnit = unit;
        return this;
    }
}

public class ChangeSFXEvent : GameEvent
{
    public bool isActive;
    
    public ChangeSFXEvent Initializer(bool _isActive)
    {
        isActive = _isActive;
        return this;
    }
}

