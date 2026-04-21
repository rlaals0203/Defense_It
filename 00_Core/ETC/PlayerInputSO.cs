using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _01_Script.Core.ETC
{
    [CreateAssetMenu(fileName = "PlayerInput", menuName = "SO/PlayerInput", order = 0)]
    public class PlayerInputSO : ScriptableObject, Controls.IPlayerActions
    {
        [SerializeField] private LayerMask whatIsGround;
        private Controls _controls;
        private Vector2 _screenPosition;
        private Vector3 _worldPosition;
        
        public Vector2 MovementKey { get; private set; }
        public event Action OnClickEvent;
        public event Action OnCancelEvent;
        public event Action OnSettingEvent;
        public event Action<float> OnZoomPressed;
        
        private void OnEnable()
        {
            if (_controls == null)
            {
                _controls = new Controls();
                _controls.Player.SetCallbacks(this);
            }
            _controls.Player.Enable();
        }
        

        private void OnDisable()
        {
            _controls.Player.Disable();
        }
        
        public void OnPointer(InputAction.CallbackContext context)
        {
            _screenPosition = context.ReadValue<Vector2>();
        }

        public void OnClick(InputAction.CallbackContext context)
        {
            if(context.performed)
                OnClickEvent?.Invoke();
        }

        public void OnMovement(InputAction.CallbackContext context)
        {
            MovementKey = context.ReadValue<Vector2>();
        }

        public void OnZoom(InputAction.CallbackContext context)
        {
            OnZoomPressed?.Invoke(context.ReadValue<float>());
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
            if(context.performed)
                OnCancelEvent?.Invoke();
        }

        public void OnSetting(InputAction.CallbackContext context)
        {
            if(context.performed)
                OnSettingEvent?.Invoke();
        }   

        public Vector3 GetWorldPosition()
        {
            Camera mainCam = Camera.main;
            Debug.Assert(mainCam != null, "No main camera in this scene");
            Ray camRay = mainCam.ScreenPointToRay(_screenPosition);
            if (Physics.Raycast(camRay, out RaycastHit hit, mainCam.farClipPlane, whatIsGround))
            {
                _worldPosition = hit.point;
            }

            return _worldPosition;
        }
        
        public Vector3 GetWorldPosition(out RaycastHit hit, LayerMask whatIsTarget = default)
        {
            Camera mainCam = Camera.main;
            Debug.Assert(mainCam != null, "No main camera in this scene");
            Ray camRay = mainCam.ScreenPointToRay(_screenPosition);
            bool isHit = Physics.Raycast(camRay, out hit, mainCam.farClipPlane, whatIsTarget);
            if (isHit)
            {
                _worldPosition = hit.point;
            }
            
            return _worldPosition;
        }
    }
}