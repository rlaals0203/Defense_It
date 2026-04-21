using _01_Script._00_Core.EventChannel;
using _01_Script.Core.ETC;
using Unity.Burst.Intrinsics;
using Unity.Cinemachine;
using UnityEngine;

namespace _01_Script._100_Misc
{
    [RequireComponent(typeof(CinemachineCamera))]
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO cameraChannel;
        [SerializeField] private PlayerInputSO playerInput;
        [SerializeField] private float minFOV, maxFOV;
        [SerializeField] private float zoomAmount;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float acceleration = 5f;
        [SerializeField] private float deceleration = 5f;

        private CinemachineCamera _cam;
        private bool _isCamMode = true;
        private Vector3 _currentVelocity = Vector3.zero;

        private Camera _mainCam;

        private void Awake()
        {
            playerInput.OnZoomPressed += HandleZoomPressed;
            cameraChannel.AddListener<CameraModeChangeEvent>(HandleCameraModeChange);
            _cam = GetComponent<CinemachineCamera>();
            _mainCam = Camera.main;
            
        }

        private void OnDestroy()
        {
            playerInput.OnZoomPressed -= HandleZoomPressed;
            cameraChannel.RemoveListener<CameraModeChangeEvent>(HandleCameraModeChange);
        }

        private void HandleCameraModeChange(CameraModeChangeEvent evt)
        {
            _isCamMode = evt.isCamMode;
        }

        private void Update()
        {
            if (_isCamMode == false) return;

            Vector2 input = playerInput.MovementKey;
            Vector3 targetDir = Vector3.zero;
            
            if (input != Vector2.zero)
            {
                Vector3 camForward = _mainCam.transform.forward;
                Vector3 camRight = _mainCam.transform.right;
                camForward.y = 0;
                camRight.y = 0;

                camForward.Normalize();
                camRight.Normalize();

                targetDir = (camForward * input.y + camRight * input.x).normalized;
            }

            float speed = input != Vector2.zero ? moveSpeed : 0f;
            float lerpRate = input != Vector2.zero ? acceleration : deceleration;

            _currentVelocity = Vector3.Lerp(_currentVelocity, targetDir * speed, Time.deltaTime * lerpRate);

            transform.position += (_currentVelocity * Time.deltaTime) / Time.timeScale;
            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, -22f, 22f),
                transform.position.y,
                Mathf.Clamp(transform.position.z, -50f, 2.5f));
        }

        private void HandleZoomPressed(float yMovement)
        {
            float res = _cam.Lens.FieldOfView - yMovement * zoomAmount;
            _cam.Lens.FieldOfView = Mathf.Clamp(res, minFOV, maxFOV);
        }
    }
}