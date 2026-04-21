using _01_Script._00_Core.EventChannel;
using _01_Script._01_Entities;
using _01_Script._02_Unit;
using _01_Script._02_Unit.Combat;
using _01_Script._02_Unit.Skill;
using _01_Script._04_Combat;
using _01_Script._100_Misc.Effects;
using _01_Script.Entities;
using DG.Tweening;
using MinLibrary.Dependencies;
using MinLibrary.ObjectPool.Runtime;
using Unity.Cinemachine;
using Unity.Mathematics;
using UnityEngine;

public class EntitySkill : MonoBehaviour, IEntityComponent
{
    [SerializeField] private DamageCaster damageCaster;
    [SerializeField] private CinemachineImpulseSource impulseSource;
    
    [Inject] private PoolManagerMono _poolManager;
    private Unit _unit;

    [field:SerializeField] public SkillSO SkillSO { get; private set; }
    public float UsedTime { get; private set; }
    public bool IsActive { get; set; }
    
    private void Awake()
    {
        if (_poolManager == null)
        {
            Injector.InjectTo(this);
        }

        UsedTime = Time.time;
    }

    public void Initialize(Entity entity)
    {
        _unit = entity as Unit;
        damageCaster.InitCaster(entity);
    }

    public void UseSkill()
    {
        if (!CanUse() || _unit.Target == null) return;
        
        UnitAttackMethod prevMethod = _unit.AttackMethod;
        _unit.SetAttackMethod(UnitAttackMethod.Strongest);

        DamageData data = new DamageData();
        data.damage = SkillSO.damage;
        data.hitPoint = _unit.Target.transform.position;
        damageCaster.CastDamage(data, data.hitPoint, Vector3.forward);
        
        UsedTime = Time.time;
        PlayEffct();
        _unit.SetAttackMethod(prevMethod);
    }

    private async void PlayEffct()
    {
        PoolingEffect effect = _poolManager.Pop<PoolingEffect>(SkillSO.effectPool);
        Vector3 pos = _unit.Target.transform.position;
        pos.y = 0;
        
        effect.PlayVFX(pos, quaternion.identity);
        
        await Awaitable.WaitForSecondsAsync(SkillSO.attackTime);
        impulseSource.GenerateImpulse(1f);

        await Awaitable.WaitForSecondsAsync(SkillSO.skillDuration);
        _poolManager.Push(effect);
    }

    public bool CanUse()
    {
        return Time.time - UsedTime >= SkillSO.cooltime;
    }
}
