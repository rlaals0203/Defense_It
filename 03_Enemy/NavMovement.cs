using System;
using _01_Script._00_Core.StatSystem;
using _01_Script._01_Entities;
using _01_Script._01_Entities.Stat;
using _01_Script.Entities;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

namespace _01_Script._03_Enemy
{
    public class NavMovement : MonoBehaviour, IEntityComponent, IAfterInitialize
    {
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private float stopOffset = 0.05f;
        [SerializeField] private float rotationSpeed = 20f;
        [SerializeField] private StatSO moveSpeedStat;

        private EntityStat _statCompo;
        private Entity _entity;
        
        public float RemainDistance => agent.pathPending ? -1 : agent.remainingDistance;
        
        public void Initialize(Entity entity)
        {
            _entity = entity;
            _statCompo = entity.GetCompo<EntityStat>();
        }
        
        public void AfterInitialize()
        {
            agent.speed = _statCompo.SubscribeStat(moveSpeedStat, HandleSpeedChanged, 1f);
            agent.speed = _statCompo.GetStat(moveSpeedStat).Value;
            agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
            agent.updateRotation = false;
        }

        private void HandleSpeedChanged(StatSO stat, float currentvalue, float prevvalue)
        {
            agent.speed = currentvalue;
        }

        private void OnDestroy()
        {
            _statCompo.UnSubscribeStat(moveSpeedStat, HandleSpeedChanged);
            _entity.transform.DOKill();
        }

        private void Update()
        {
            if (agent.hasPath && agent.isStopped == false && agent.path.corners.Length > 0)
            {
                LookAtTarget(agent.steeringTarget, true);
            }
        }
        
        public void LookAtTarget(Vector3 target, bool isSmooth = true)
        {
            Vector3 direction = target - _entity.transform.position;
            direction.y = 0;
            Quaternion lookRotation = Quaternion.LookRotation(direction.normalized);

            if (isSmooth)
            {
                _entity.transform.rotation = Quaternion.Slerp(_entity.transform.rotation, 
                    lookRotation, Time.deltaTime * rotationSpeed);
            }
            else
            {
                _entity.transform.rotation = lookRotation;
            }
        }

        public void SetStop(bool isStop)
        {
            if (agent.isOnNavMesh)
                agent.isStopped = isStop;
        }
        public void SetVelocity(Vector3 velocity) => agent.velocity = velocity;
        public void SetSpeed(float speed) => agent.speed = speed;

        public void SetDestination(Vector3 destination)
        {
            if (agent.isOnNavMesh)
                agent.SetDestination(destination);
        }
        
        public void KnockBack(Vector3 force, float time)
        {
            SetStop(true);
            Vector3 destination = GetKnockBackEndPosition(force);
            Vector3 delta = destination - _entity.transform.position;
            float knockBackDuration = delta.magnitude * time / force.magnitude;

            _entity.transform.DOMove(destination, knockBackDuration)
                .SetEase(Ease.OutCirc)
                .OnComplete(() =>
                {
                    agent.Warp(transform.position);
                    SetStop(false);
                });
        }

        private Vector3 GetKnockBackEndPosition(Vector3 force)
        {
            Vector3 startPosition = _entity.transform.position + new Vector3(0, 0.5f);
            if (Physics.Raycast(startPosition, force.normalized, out RaycastHit hit, force.magnitude))
            {
                Vector3 hitPoint = hit.point;
                hitPoint.y = _entity.transform.position.y;
                return hitPoint;
            }

            return _entity.transform.position + force;
        }
    }
}