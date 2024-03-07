using System;
using GrabTool.Mesh;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using Wright.Library.Logging;

namespace Presenters
{
    public class SizeChanger : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float radiusChangeRate = 0.1f;
        [SerializeField] private float exponent = 1.0f;
        [SerializeField] private float minimumValue = 0.01f;
        [SerializeField] private float maximumValue = 1000.0f;

        [Header("Input")]
        [SerializeField] private InputActionProperty changeValue;
        
        [Header("Events")]
        public UnityEvent<float> onUpdateSize;

        
        private float _currentValue = 0.2f;
        
        private void OnEnable()
        {
            changeValue.EnableDirectAction();
        }

        private void OnDisable()
        {
            changeValue.DisableDirectAction();
        }

        private void Update()
        {
            var value = changeValue.action.ReadValue<Vector2>();

            if (value != Vector2.zero)
            {
                MDebug.Log($"Values is {value}");
            }
            else
            {
                MDebug.Log("Is zero");
                return;
            }

            _currentValue += radiusChangeRate * value.y * Time.deltaTime;
            _currentValue = Mathf.Max(minimumValue, Mathf.Min(maximumValue, _currentValue));
            
            onUpdateSize.Invoke(Mathf.Pow(_currentValue, exponent));
        }
    }
}