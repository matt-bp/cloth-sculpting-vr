using System;
using GrabTool.Mesh;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using Wright.Library.Logging;

namespace Presenters.VR
{
    public class VRSizeChanger : MonoBehaviour
    {

        [SerializeField] private VRMeshDragger vRMeshDragger;
        [SerializeField] private InputActionProperty toggle;
        [SerializeField] private InputActionProperty changeValue;
        [SerializeField] private float radiusChangeRate = 0.1f;
        [SerializeField] private GameObject collisionVisualization;
        [SerializeField] private Material updatingMaterial;
        private Material _previousMaterial;
        private MeshRenderer _meshRenderer;
        
        private bool _canUpdate;

        private void OnEnable()
        {
            toggle.EnableDirectAction();
            changeValue.EnableDirectAction();
        }

        private void OnDisable()
        {
            toggle.DisableDirectAction();
            changeValue.DisableDirectAction();
        }

        private void Start()
        {
            _meshRenderer = collisionVisualization.GetComponent<MeshRenderer>();
            _previousMaterial = _meshRenderer.material;
        }

        private void Update()
        {
            var toggleAction = toggle.action;

            if (toggleAction.WasPressedThisFrame() && toggleAction.IsPressed())
            {
                _canUpdate = !_canUpdate;
                MDebug.Log($"Can update? {_canUpdate}");
            }

            if (_canUpdate)
            {
                _meshRenderer.material = updatingMaterial;
                
                var value = changeValue.action.ReadValue<Vector2>();

                var updatedRadius = vRMeshDragger.CurrentRadius + radiusChangeRate * value.y * Time.deltaTime;
                
                vRMeshDragger.OnRadiusChanged(updatedRadius);
            }
            else
            {
                _meshRenderer.material = _previousMaterial;
            }
        }
    }
}