using System;
using System.Linq;
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

        private void Start()
        {
            currentModeLabel.canvas.enabled = creationModeModel.CreatingGoals;
            currentModeLabel.text = "Goals";
        }

        public void OnAddGoalMesh()
        {
            if (!creationModeModel.CreatingGoals) return;
            
            // var copy = MeshCopier.MakeCopy(currentCloth.sharedMesh);
            // copy.vertices = copy.vertices
            //     .Select(v => currentCloth.transform.TransformPoint(v)).ToArray();
            //
            //
            // if (creationModeModel.CreatingGoals)
            // {
            //     
            // }
                // Save add generated mesh to the data model
                // taskResultModel.AddUserGeneratedMesh(0, copy);

            // Save corresponding reference mesh to the data model
            // goalMeshDataModel.SaveToDisk();

            // Compute differences between the two meshes, save that to the data model as well

            // Call data export model

            // Submit for each 

            Debug.Log("Added! (NOT)");
            // Messenger.Broadcast(PresenterToModel.SUBMITTED);
        }
    }
}