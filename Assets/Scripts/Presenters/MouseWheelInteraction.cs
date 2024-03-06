using System;
using UnityEngine;
using UnityEngine.Events;
using Wright.Library.Logging;

namespace Presenters
{
    public class MouseWheelInteraction : MonoBehaviour
    {
        [SerializeField] private float rate = 500.0f;
        private float _currentValue = 0.2f;
        [SerializeField] private float exponent = 1.0f;
        public UnityEvent<float> onUpdateSize;

        private void Update()
        {
            if (Input.GetAxis("Mouse ScrollWheel") == 0) return;

            _currentValue += rate * Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime;
            _currentValue = MathF.Max(0.01f, MathF.Min(1000.0f, _currentValue));

            onUpdateSize.Invoke(MathF.Pow(_currentValue, exponent));
        }
    }
}