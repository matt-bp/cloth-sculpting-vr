using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Wright.Library.Camera
{
    public class CameraSystem : MonoBehaviour
    {
        public enum RequestResult
        {
            /// <summary>
            /// The request was successful.
            /// </summary>
            Success,

            /// <summary>
            /// The request failed due to the system being currently busy.
            /// </summary>
            Busy,

            /// <summary>
            /// The request failed due to an unknown error.
            /// </summary>
            Error,
        }

        private CameraController _currentExclusiveProvider;
        private CameraController _previousProvider;
        private float _timeMadeExclusive;

        [SerializeField] [Tooltip("The timeout (in seconds) for exclusive access to the XR Origin.")]
        private float timeout = 10f;

        /// <summary>
        /// The timeout (in seconds) for exclusive access to the camera.
        /// </summary>
        public float Timeout
        {
            get => timeout;
            set => timeout = value;
        }

        /// <summary>
        /// (Read Only) If this value is true, the camera's position should not be modified until this false.
        /// </summary>
        public bool Busy => _currentExclusiveProvider != null;

        public UnityEvent onCameraControlStarted;
        public UnityEvent onCameraControlStopped;
        

        /// <summary>
        /// See <see cref="MonoBehaviour"/>.
        /// </summary>
        protected void Update()
        {
            if (_currentExclusiveProvider != null && Time.time > _timeMadeExclusive + timeout)
            {
                ResetExclusivity();
            }
        }

        public bool HasExclusiveAccess(CameraController provider) => _currentExclusiveProvider == provider;

        public bool WasPrevious(CameraController provider) => _previousProvider == provider;

        /// <summary>
        /// Attempt to "lock" access to the camera for the <paramref name="provider"/>.
        /// </summary>
        /// <param name="provider">The locomotion provider that is requesting access.</param>
        /// <returns>Returns a <see cref="RequestResult"/> that reflects the status of the request.</returns>
        public RequestResult RequestExclusiveOperation(CameraController provider)
        {
            if (provider == null)
                return RequestResult.Error;

            if (_currentExclusiveProvider != null)
                return _currentExclusiveProvider != provider ? RequestResult.Busy : RequestResult.Error;
            
            _currentExclusiveProvider = provider;
            _timeMadeExclusive = Time.time;
            
            onCameraControlStarted.Invoke();
            
            return RequestResult.Success;
        }

        private void ResetExclusivity()
        {
            _previousProvider = _currentExclusiveProvider;
            _currentExclusiveProvider = null;
            _timeMadeExclusive = 0f;
            
            onCameraControlStopped.Invoke();
        }

        /// <summary>
        /// Informs the <see cref="LocomotionSystem"/> that exclusive access to the camera is no longer required.
        /// </summary>
        /// <param name="provider">The provider that is relinquishing access.</param>
        /// <returns>Returns a <see cref="RequestResult"/> that reflects the status of the request.</returns>
        public RequestResult FinishExclusiveOperation(CameraController provider)
        {
            if (provider == null || _currentExclusiveProvider == null)
                return RequestResult.Error;

            if (_currentExclusiveProvider != provider) return RequestResult.Error;

            ResetExclusivity();
            return RequestResult.Success;
        }
    }
}