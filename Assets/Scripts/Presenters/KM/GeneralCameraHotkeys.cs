using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Presenters.KM
{
    public class GeneralCameraHotkeys : MonoBehaviour
    {
        public UnityEvent onResetCamera;
        
        public void Update()
        {
            if (Keyboard.current.rKey.wasPressedThisFrame)
            {
                onResetCamera.Invoke();
            }
        }
    }
}