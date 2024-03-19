using System;
using UnityEngine;

namespace Wright.Library.VR
{
    [RequireComponent(typeof(MeshRenderer))]
    public class SwapIfCameraInside : MonoBehaviour
    {
        [SerializeField] private Material outside;
        [SerializeField] private Material inside;

        private MeshRenderer _meshRenderer;
        private UnityEngine.Camera _camera;
        

        private void Start()
        {
            _camera = UnityEngine.Camera.main;
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        private void Update()
        {
            var currentRadius = transform.lossyScale.x / 2.0f;
            
            Debug.Log(currentRadius);

            if (Vector3.Distance(_camera.transform.position, transform.position) > currentRadius)
            {
                Debug.Log("Outside");
                _meshRenderer.material = outside;
            }
            else
            {
                Debug.Log("Inside");
                _meshRenderer.material = inside;
            }
            {
                
            }
        }
    }
}