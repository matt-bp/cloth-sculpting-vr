using System;
using GrabTool.Mesh;
using UnityEngine;
using MeshHistory = GrabTool.Models.MeshHistory;

namespace Models.Local
{
    [RequireComponent(typeof(MeshHistory))]
    public class ClothInteractionGlue : MonoBehaviour
    {
        [SerializeField] private MouseMeshDragger mouseMeshDragger;
        [SerializeField] private MeshRenderer clothRenderer;
        [SerializeField] private Material disabledMaterial;
        [SerializeField] private Cloth.Behaviour.Cloth cloth;
        
        private MeshHistory _meshHistory;
        private Material _previous;

        private void Start()
        {
            _meshHistory = GetComponent<MeshHistory>();
        }

        public void HandleDragComplete()
        {
            mouseMeshDragger.SetDisabled(true);
            _previous = clothRenderer.material;
            clothRenderer.material = disabledMaterial;
            
            cloth.MatchClothPositionsToMesh();
            
            cloth.ToggleSimulation();
        }

        public void HandleClothSimulationComplete()
        {
            // We need to undo and then add back the mesh back to the history so that the manipulation and simulation
            // are counted as one thing in the history.
            _meshHistory.Undo();
            var meshFilter = clothRenderer.gameObject.GetComponent<MeshFilter>();
            _meshHistory.AddMesh(meshFilter.sharedMesh);
            
            mouseMeshDragger.SetDisabled(false);
            clothRenderer.material = _previous;
        }
    }
}