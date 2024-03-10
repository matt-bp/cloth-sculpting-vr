#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

using UnityEngine;
using Wright.Library.Math;

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

        [Header("Mouse Settings")] [Tooltip("Multiplier for the pan sensitivity of the translation.")]
        public float panSensitivity = 60.0f;

        [Tooltip("Multiplier for the tumble sensitivity of the rotation.")]
        public float tumbleSensitivity = 60.0f;

        [Tooltip("Multiplier for the dolly sensitivity of the translation.")]
        public float dollySensitivity = 60.0f;

        [Tooltip("Multiplier for the scroll dolly sensitivity of the translation.")]
        public float scrollDollySensitivity = 60.0f;
        
        [Tooltip("X = Change in mouse position.\nY = Multiplicative factor for camera rotation.")]
        public AnimationCurve mouseSensitivityCurve =
            new AnimationCurve(new Keyframe(0f, 0.5f, 0f, 5f), new Keyframe(1f, 2.5f, 0f, 0f));

        [Tooltip("Time it takes to interpolate camera rotation 99% of the way to the target."), Range(0.001f, 1f)]
        public float rotationLerpTime = 0.01f;

        [Tooltip("Whether or not to invert our axes for mouse input to rotation.")]
        public bool invert = false;

        [Header("Focus Settings")] [Tooltip("The minimum distance to maintain between the camera and focus point.")]
        public float minimumRadialDistance = 2.0f;

        // public GameObject focusVisualizer;

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

            // focusVisualizer.transform.position = _interpolatingCameraState.Focus;

            // Pan the camera
            if (IsMiddleMouseButtonDown())
            {
                var translation = GetMouseMovement() * (k_MouseSensitivityMultiplier * panSensitivity);
                translation *= Mathf.Pow(2.0f, boost);
                translation *= invert ? -1 : 1;
                var mouseSensitivityFactor = mouseSensitivityCurve.Evaluate(translation.magnitude);
                _targetCameraState.Translate(transform.eulerAngles, translation * mouseSensitivityFactor);
            }
            // Tumble the camera
            else if (IsLeftMouseButtonDown())
            {
                var mouseMovement = GetMouseMovement() * (k_MouseSensitivityMultiplier * tumbleSensitivity);
                mouseMovement *= invert ? -1 : 1;
                var mouseSensitivityFactor = mouseSensitivityCurve.Evaluate(mouseMovement.magnitude);
                _targetCameraState.Rotate(mouseMovement * mouseSensitivityFactor);
            }
            // Dolly the camera
            else if (IsRightMouseButtonDown())
            {
                var mouseMovement = GetMouseMovement() * (k_MouseSensitivityMultiplier * dollySensitivity);
                var mouseSensitivityFactor = mouseSensitivityCurve.Evaluate(mouseMovement.magnitude);
                _targetCameraState.Slide((mouseMovement.y - mouseMovement.x) * mouseSensitivityFactor);
            }
            // Dolly the camera with the scroll wheel
            else if (IsScrolling())
            {
                var scrollMovement = GetScrollMovement() * (k_MouseSensitivityMultiplier * scrollDollySensitivity);
                scrollMovement *= invert ? -1 : 1;
                _targetCameraState.Slide(scrollMovement.y);
            }

            // Framerate-independent interpolation
            // How we get from the current camera state to the target camera state.
            // Calculate the lerp amount, such that we get 99% of the way to our target in the specified time
            var positionLerpPct = 1f - Mathf.Exp((Mathf.Log(1f - 0.99f) / positionLerpTime) * Time.deltaTime);
            var rotationLerpPct = 1f - Mathf.Exp((Mathf.Log(1f - 0.99f) / rotationLerpTime) * Time.deltaTime);
            _interpolatingCameraState.LerpTowards(_targetCameraState, positionLerpPct, rotationLerpPct);

            PushFocusAwayFromCameraIfTooClose();
            
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

        private Vector2 GetScrollMovement()
        {
#if ENABLE_INPUT_SYSTEM
            return Mouse.current != null ? Mouse.current.scroll.ReadValue() : Vector2.zero;
#else
            return Vector2.zero;
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

        private static bool IsLeftMouseButtonDown()
        {
#if ENABLE_INPUT_SYSTEM
            return Mouse.current != null && Mouse.current.leftButton.isPressed;
#else
            return Input.GetMouseButton(0);
#endif
        }

        private static bool IsRightMouseButtonDown()
        {
#if ENABLE_INPUT_SYSTEM
            return Mouse.current != null && Mouse.current.rightButton.isPressed;
#else
            return Input.GetMouseButton(0);
#endif
        }

        private static bool IsScrolling()
        {
#if ENABLE_INPUT_SYSTEM
            return Mouse.current != null && Mouse.current.scroll.ReadValue().sqrMagnitude > 0;
#else
            return false;
#endif
        }

        #endregion


        /// <summary>
        /// <para>If after interpolation we're too close, push out the focus point in front of the camera</para>
        /// <para>Also match the target and interpolating state, so this change happens immediately.</para>
        /// </summary>
        private void PushFocusAwayFromCameraIfTooClose()
        {
            var currentDistance = Vector3.Distance(transform.position, _interpolatingCameraState.Focus);
            if (currentDistance > minimumRadialDistance) return;
            var adjustment = (_interpolatingCameraState.Focus - transform.position).normalized * minimumRadialDistance;
            var newFocus = _interpolatingCameraState.Focus + adjustment;
            var radialDistance = Vector3.Distance(transform.position, newFocus);
            _interpolatingCameraState.Focus = newFocus;
            _interpolatingCameraState.RadialDistance = radialDistance;
            _targetCameraState.Focus = newFocus;
            _targetCameraState.RadialDistance = radialDistance;
        }
        
        /// <summary>
        /// <para>This camera state is concerned only about a focus point, and spherical coordinates to update the camera.</para>
        /// <para>From what I gathered from Maya, manipulating the camera also involves manipulating a focus point, and when you tumble the camera, it rotates around that focus point.</para>
        /// <para>That means we only need to update the focus point, and what the rotations should be, and then we can convert those rotations to coordinates that the camera's position could then be updated.</para>
        /// </summary>
        private class CameraState
        {
            private SphericalCoordinates _sc;
            private Vector3 _focus;

            public Vector3 Focus
            {
                get => _focus;
                set => _focus = value;
            }

            public float RadialDistance
            {
                set => _sc.RadialDistance = value;
            }

            public void SetFromTransformAndPoint(Transform t, Vector3 focus)
            {
                _sc = SphericalCoordinates.FromCartesian(t.position);
                _focus = focus;
            }

            public void Translate(Vector3 cameraEulerAngles, Vector3 translation)
            {
                var rotatedTranslation = Quaternion.Euler(cameraEulerAngles) * translation;

                _focus = new Vector3(
                    _focus.x + rotatedTranslation.x,
                    _focus.y + rotatedTranslation.y,
                    _focus.z + rotatedTranslation.z);
            }

            public void Rotate(Vector2 polarElevationDelta)
            {
                _sc.Polar += polarElevationDelta.x;
                _sc.Elevation += polarElevationDelta.y;
            }

            public void Slide(float radialDistanceDelta)
            {
                _sc.RadialDistance += radialDistanceDelta;
            }

            public void LerpTowards(CameraState target, float positionLerpPct, float rotationLerpPct)
            {
                _sc.RadialDistance = Mathf.Lerp(_sc.RadialDistance, target._sc.RadialDistance, positionLerpPct);
                _sc.Polar = Mathf.Lerp(_sc.Polar, target._sc.Polar, rotationLerpPct);
                _sc.Elevation = Mathf.Lerp(_sc.Elevation, target._sc.Elevation, rotationLerpPct);

                _focus = new Vector3(
                    Mathf.Lerp(_focus.x, target._focus.x, positionLerpPct),
                    Mathf.Lerp(_focus.y, target._focus.y, positionLerpPct),
                    Mathf.Lerp(_focus.z, target._focus.z, positionLerpPct));
            }
            
            public void UpdateTransform(Transform t)
            {
                // Translate the spherical coordinates to be centered on our focus point.
                t.position = _focus + _sc.ToCartesian();

                t.LookAt(_focus);
            }
        }
    }
}