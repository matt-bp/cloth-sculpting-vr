using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Wright.Library.Events
{
    /// <summary>
    /// Checks for button input on an input action
    /// </summary>
    [AddComponentMenu("Wright/Events/On Button Press")]
    public class OnButtonPress : MonoBehaviour
    {
        [SerializeField] [Tooltip("The Input System Action that will be used to read button press data.")]
        private InputActionProperty buttonAction = new InputActionProperty(new InputAction("Button Press"));

        // When the button is pressed
        public UnityEvent onPress = new();

        // When the button is released
        public UnityEvent onRelease = new();


        /// <summary>
        /// See <see cref="MonoBehaviour"/>.
        /// </summary>
        protected void OnEnable()
        {
            if (buttonAction.reference != null)
                return;
            
            buttonAction.action?.Enable();
        }

        /// <summary>
        /// See <see cref="MonoBehaviour"/>.
        /// </summary>
        protected void OnDisable()
        {
            if (buttonAction.reference != null)
                return;
            
            buttonAction.action?.Disable();
        }

        private void Update()
        {
            var action = buttonAction.action;

            if (action.WasPressedThisFrame() && action.IsPressed())
            {
                onPress.Invoke();
            }

            if (action.WasReleasedThisFrame() && !action.IsPressed())
            {
                onRelease.Invoke();
            }
        }
    }
}