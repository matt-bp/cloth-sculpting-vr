using System;
using UnityEngine;
using UnityEngine.Events;

namespace Presenters
{
    public class MouseWheelInteraction : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float rate = 500.0f;
        [SerializeField] private float exponent = 1.0f;
        [SerializeField] private float minimumValue = 0.01f;
        [SerializeField] private float maximumValue = 1000.0f;
        
        [Header("Events")]
        public UnityEvent<float> onUpdateSize;

        private float _currentValue = 0.2f;

        private void Update()
        {
            if (Input.GetAxis("Mouse ScrollWheel") == 0) return;

            _currentValue += rate * Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime;
            _currentValue = Mathf.Max(minimumValue, Mathf.Min(maximumValue, _currentValue));

            onUpdateSize.Invoke(Mathf.Pow(_currentValue, exponent));
        }
    }
}