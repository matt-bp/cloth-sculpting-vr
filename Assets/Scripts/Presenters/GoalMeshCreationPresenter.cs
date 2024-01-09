using System;
using System.Linq;
using System.Runtime.InteropServices;
using Models.Local;
using TMPro;
using UnityEngine;
using Wright.Library.Mesh;

namespace Presenters
{
    public class GoalMeshCreationPresenter : MonoBehaviour
    {
        [Header("View")] [SerializeField] private MeshFilter currentCloth;

        [Header("Model")] [SerializeField] private CreationModeModel creationModeModel;
        [SerializeField] private TMP_Text currentModeLabel;
        [SerializeField] private GoalMeshCreationModel goalMeshCreationModel;

        private void Start()
        {
            currentModeLabel.canvas.enabled = creationModeModel.CreatingGoals;
            currentModeLabel.text = "Goals";
        }

        public void OnTimeChanged(string newTime)
        {
            if (!float.TryParse(newTime, out var value)) return;

            Debug.Log($"New time: {value}");
            goalMeshCreationModel.SetTime(value);
        }

        public void OnExport()
        {
            Debug.Log("Going to export");
        }

        public void OnAdd()
        {
            if (!creationModeModel.CreatingGoals) return;

            var copy = MeshCopier.MakeCopy(currentCloth.sharedMesh);
            copy.vertices = copy.vertices
                .Select(v => currentCloth.transform.TransformPoint(v)).ToArray();

            goalMeshCreationModel.AddGoalMeshAtCurrentTime(copy);
            
            Debug.Log("Added!");
        }
    }
}