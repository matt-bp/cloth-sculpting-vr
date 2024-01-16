using System;
using UnityEngine;
using UnityEngine.Events;

namespace Presenters
{
    public class MouseWheelInteraction : MonoBehaviour
    {
        [SerializeField] private float _rate = 1.0f;
        private float _start = 0.2f;
        
        public UnityEvent<float> onUpdateSize;
        
        private void Update()
        {
            if (Input.GetAxis("Mouse ScrollWheel") == 0) return;
            
            var updated = Input.GetAxis("Mouse ScrollWheel") * _rate;
            _start += updated;

            _start = MathF.Max(0.01f, _start);

            onUpdateSize.Invoke(_start);
        }
    }
}