#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

using UnityEngine;

namespace Wright.Library.Camera
{
    [RequireComponent(typeof(UnityEngine.Camera))]
    public class MayaCameraController : MonoBehaviour
    {
        const float k_MouseSensitivityMultiplier = 0.01f;

        private CameraState _targetCameraState = new CameraState();
        private CameraState _interpolatingCameraState = new CameraState();

        [Header("Movement Settings")] [Tooltip("Exponential boost factor on translation.")]
        public float boost = 3.5f;

        [Tooltip("Time it takes to interpolate camera position 99% of the way to the target."), Range(0.001f, 1f)]
        public float positionLerpTime = 0.2f;

        [Header("Rotation Settings")] [Tooltip("Multiplier for the sensitivity of the rotation.")]
        public float mouseSensitivity = 60.0f;

        [Tooltip("X = Change in mouse position.\nY = Multiplicative factor for camera rotation.")]
        public AnimationCurve mouseSensitivityCurve =
            new AnimationCurve(new Keyframe(0f, 0.5f, 0f, 5f), new Keyframe(1f, 2.5f, 0f, 0f));

        [Tooltip("Time it takes to interpolate camera rotation 99% of the way to the target."), Range(0.001f, 1f)]
        public float rotationLerpTime = 0.01f;

        [Tooltip("Whether or not to invert our Y axis for mouse input to rotation.")]
        public bool invertY = false;

        private void OnEnable()
        {
            _targetCameraState.SetFromTransformAndPoint(transform, Vector3.zero);
            _interpolatingCameraState.SetFromTransformAndPoint(transform, Vector3.zero);
        }

#if ENABLE_INPUT_SYSTEM
        private InputAction _mouseAction;
        // private InputAction _altAction;

        private void Start()
        {
            var map = new InputActionMap("Unity Camera Controller");

            _mouseAction = map.AddAction("look", binding: "<Mouse>/delta");
            // _altAction = map.AddAction("look", binding: "<Keyboard>/alt");

            _mouseAction.Enable();
            // _altAction.Enable();
        }
#endif

        private void Update()
        {
            if (IsAltButtonUpThisFrame())
            {
                Debug.Log("Alt button up");

                // Reset shared access

                return;
            }

            if (!IsAltButtonDown())
                return;

            if (IsMiddleMouseButtonDown())
            {
                var translation = GetMouseMovement() * k_MouseSensitivityMultiplier * mouseSensitivity * Time.deltaTime;
                translation *= Mathf.Pow(2.0f, boost);
                _targetCameraState.Translate(translation);
            }

            // Framerate-independent interpolation
            // How we get from the current camera state to the target camera state.
            // Calculate the lerp amount, such that we get 99% of the way to our target in the specified time
            var positionLerpPct = 1f - Mathf.Exp((Mathf.Log(1f - 0.99f) / positionLerpTime) * Time.deltaTime);
            var rotationLerpPct = 1f - Mathf.Exp((Mathf.Log(1f - 0.99f) / rotationLerpTime) * Time.deltaTime);
            _interpolatingCameraState.LerpTowards(_targetCameraState, positionLerpPct, rotationLerpPct);
            _interpolatingCameraState.UpdateTransform(transform);
        }

        private Vector2 GetMouseMovement()
        {
            // try to compensate the diff between the two input systems by multiplying with empirical values
#if ENABLE_INPUT_SYSTEM
            var delta = _mouseAction.ReadValue<Vector2>();
            delta *= 0.5f; // Account for scaling applied directly in Windows code by old input system.
            delta *= 0.1f; // Account for sensitivity setting on old Mouse X and Y axes.
            return delta;
#else
            return new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
#endif
        }

        #region Keyboard Button Pressed Status Getters

        private static bool IsAltButtonDown()
        {
#if ENABLE_INPUT_SYSTEM
            return Keyboard.current != null && Keyboard.current.altKey.isPressed;
#else
            return Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);
#endif
        }

        private static bool IsAltButtonUpThisFrame()
        {
#if ENABLE_INPUT_SYSTEM
            return Keyboard.current != null && Keyboard.current.altKey.wasReleasedThisFrame;
#else
            return Input.GetKeyUp(KeyCode.LeftAlt) || Input.GetKeyUp(KeyCode.RightAlt);
#endif
        }

        private static bool IsMiddleMouseButtonDown()
        {
#if ENABLE_INPUT_SYSTEM
            return Mouse.current != null && Mouse.current.middleButton.isPressed;
#else
            return Input.GetMouseButton(2);
#endif
        }

        #endregion

        private class CameraState
        {
            private float _yaw;
            private float _pitch;
            private float _roll;
            private float _x;
            private float _y;
            private float _z;
            private Vector3 _target;

            public void SetFromTransformAndPoint(Transform t, Vector3 target)
            {
                t.LookAt(target);

                _pitch = t.eulerAngles.x;
                _yaw = t.eulerAngles.y;
                _roll = t.eulerAngles.z;
                _x = t.position.x;
                _y = t.position.y;
                _z = t.position.z;
                _target = target;
            }

            public void Translate(Vector3 translation)
            {
                var rotatedTranslation = Quaternion.Euler(_pitch, _yaw, _roll) * translation;

                _x += rotatedTranslation.x;
                _y += rotatedTranslation.y;
                _z += rotatedTranslation.z;

                _target = new Vector3(
                    _target.x + rotatedTranslation.x, 
                    _target.y + rotatedTranslation.y,
                    _target.z + rotatedTranslation.z);
            }

            public void LerpTowards(CameraState target, float positionLerpPct, float rotationLerpPct)
            {
                // yaw = Mathf.Lerp(yaw, target.yaw, rotationLerpPct);
                // pitch = Mathf.Lerp(pitch, target.pitch, rotationLerpPct);
                // roll = Mathf.Lerp(roll, target.roll, rotationLerpPct);

                _x = Mathf.Lerp(_x, target._x, positionLerpPct);
                _y = Mathf.Lerp(_y, target._y, positionLerpPct);
                _z = Mathf.Lerp(_z, target._z, positionLerpPct);

                _target = new Vector3(
                    Mathf.Lerp(_target.x, target._x, positionLerpPct), 
                    Mathf.Lerp(_target.y, target._y, positionLerpPct), 
                    Mathf.Lerp(_target.z, target._z, positionLerpPct));
            }

            public void UpdateTransform(Transform t)
            {
                t.position = new Vector3(_x, _y, _z);
                t.LookAt(_target);
            }
        }
    }
}