using System;
using _01_Script._00_Core.EventChannel;
using _01_Script._03_Enemy;
using _01_Script._04_Combat;
using _01_Script._100_Misc.Effects;
using _01_Script.Core.ETC;
using Ami.BroAudio;
using MinLibrary.Dependencies;
using MinLibrary.ObjectPool.Runtime;
using UnityEngine;

public class ClickAttack : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO eventChannel;
    [SerializeField] private PlayerInputSO playerInput;
    [SerializeField] private LayerMask whatIsEnemy;
    [SerializeField] private float coolTime = 0.1f;
    [SerializeField] private float damage = 1f;
    [SerializeField] private SoundID hitSound;

    private float _lastTime;

    private void Awake()
    {
        playerInput.OnClickEvent += HandleClick;
        eventChannel.AddListener<ClickAttackPowerEvent>(HandleClickAttack);
    }

    private void OnDestroy()
    {
        playerInput.OnClickEvent -= HandleClick;
        eventChannel.RemoveListener<ClickAttackPowerEvent>(HandleClickAttack);
    }

    private void Start()
    {
        damage = 1;
    }

    private void HandleClickAttack(ClickAttackPowerEvent channel)
    {
        damage += 3;
    }

    private void HandleClick()
    {
        Vector3 worldPosition = playerInput.GetWorldPosition(out RaycastHit hitInfo, whatIsEnemy);
        if (Time.time - _lastTime < coolTime || hitInfo.collider == null) return;

        if (hitInfo.collider.TryGetComponent(out IDamageable damageable))
        {
            _lastTime = Time.time;
            BroAudio.Play(hitSound);
            
            DamageData data = new DamageData();
            data.damage = damage;
            data.hitNormal = worldPosition;
            damageable.ApplyDamage(data, Vector3.forward);
        }
    }
}
