using System;
using UnityEngine;

namespace UnityTemplateProjects.Temp
{
    public class ApplyCameraOrientation : MonoBehaviour
    {
        private Camera _camera;
        private Quaternion _originalRotation;

        private void Start()
        {
            _camera = Camera.main;
            _originalRotation = transform.rotation;
        }

        private void Update()
        {
            transform.rotation = Quaternion.Inverse(_camera.transform.rotation);
        }
    }
}