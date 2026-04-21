using System;
using _01_Script._01_Entities;
using _01_Script.Entities;
using DG.Tweening;
using UnityEngine;

namespace _01_Script._02_Unit
{
    public class UnitRangeGizmo : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private GameObject gizmoObj;
        [SerializeField] private Material gizmoMat;
        
        private Outline _outline;
        private readonly string colorHash = "_BaseColor";
        private readonly Color _redCol = new Color(1f, 0.25f, 0.25f, 0.1f);

        public void Initialize(Entity entity)
        {
            gizmoMat = gizmoObj.GetComponent<MeshRenderer>().material;
        }

        private void Awake()
        {
            _outline = GetComponentInChildren<Outline>();
        }

        public void ChangeGizmoColor(Color color)
        {
            gizmoMat.SetColor(colorHash, color);
            
            color.a = 1f;
            _outline.OutlineColor = color;
        }

        public void ChangeGizmoStatus(bool isActive, float range = 0f)
        {
            if (isActive)
            {
                ChangeGizmoSize(range);
                gizmoMat.SetColor(colorHash, _redCol);
            }
            else
                ChangeGizmoSize(0);
        }

        private void ChangeGizmoSize(float size)
        {
            gizmoObj.transform.DOScaleX(size, 0.4f).SetEase(Ease.OutCubic);
            gizmoObj.transform.DOScaleZ(size, 0.4f).SetEase(Ease.OutCubic);
        }
    }
}