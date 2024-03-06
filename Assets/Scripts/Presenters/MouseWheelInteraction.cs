using System;
using UnityEngine;
using UnityEngine.Events;
using Wright.Library.Logging;

namespace Presenters
{
    public class MouseWheelInteraction : MonoBehaviour
    {
        private float _rate = 1.0f;
        private float _currentValue = 0.2f;
        [SerializeField] private float exponent = 1.0f;
        public UnityEvent<float> onUpdateSize;

        private void Update()
        {
            if (Input.GetAxis("Mouse ScrollWheel") == 0) return;

            var updated = Input.GetAxis("Mouse ScrollWheel") * _rate;

            _currentValue += updated;
            _currentValue = MathF.Max(0.01f, MathF.Min(1000.0f, _currentValue));

            // Scale our current value to grow quadratically.
            var outValue = MathF.Pow(_currentValue, exponent);
            onUpdateSize.Invoke(outValue);
        }
    }
}