using System.Collections.Generic;
using System.Linq;
using Models.Local;
using TMPro;
using UnityEngine;
using Wright.Library.Mesh;

namespace Presenters
{
    public class GoalMeshCreationPresenter : MonoBehaviour
    {
        [Header("Prefabs")] [SerializeField] private GameObject goalMeshPrefab;

        [Header("View")] [SerializeField] private MeshFilter currentCloth;
        private List<GameObject> _visualizedGoalMeshes = new();

        [Header("Model")] [SerializeField] private CreationModeModel creationModeModel;
        [SerializeField] private TMP_Text currentModeLabel;
        [SerializeField] private GoalMeshCreationModel goalMeshCreationModel;
        [SerializeField] private GoalMeshCreationDataModel dataModel;
        
        private void Start()
        {
            currentModeLabel.canvas.enabled = creationModeModel.CreatingGoals;
            currentModeLabel.text = "Goals";

            goalMeshCreationModel.OnMeshAdded += HandleMeshAdded;
        }

        public void OnExport()
        {
            dataModel.SaveToDisk();
        }

        public void OnAdd()
        {
            if (!creationModeModel.CreatingGoals) return;

            var copy = MeshCopier.MakeCopy(currentCloth.sharedMesh);
            
            // Transform to world space for later use in the tasks
            copy.vertices = copy.vertices
                .Select(v => currentCloth.transform.TransformPoint(v)).ToArray();

            goalMeshCreationModel.AddGoalMesh(copy);
        }

        private void HandleMeshAdded()
        {
            // Update visualization
            VisualizeGoalMeshes(goalMeshCreationModel.GoalMeshes);
        }

        private void VisualizeGoalMeshes(IEnumerable<Mesh> meshes)
        {
            foreach (var meshObject in _visualizedGoalMeshes)
            {
                Destroy(meshObject);
            }

            _visualizedGoalMeshes = new List<GameObject>();

            foreach (var mesh in meshes)
            {
                var instance = Instantiate(goalMeshPrefab);
                instance.GetComponent<MeshFilter>().sharedMesh = mesh;
                _visualizedGoalMeshes.Add(instance);
            }
        }
    }
}