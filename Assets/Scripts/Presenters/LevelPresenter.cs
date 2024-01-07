using System;
using System.Linq;
using Events;
using Models.Local;
using TMPro;
using UnityEngine;
using Wright.Library.Mesh;
using Wright.Library.Messages;

namespace Presenters
{
    public class LevelPresenter : MonoBehaviour
    {
        [Header("View")]
        [SerializeField] private TMP_Text timeLabel;
        [SerializeField] private MeshFilter currentCloth;
        [SerializeField] private MeshFilter goalMesh;
        
        [Header("Model")]
        [SerializeField] private TaskResultModel taskResultModel;
        [SerializeField] private GoalMeshDataModel goalMeshDataModel;
        [SerializeField] private GoalMeshModel goalMeshModel;
        
        private void OnEnable()
        {
            Debug.Log("LP Enable");
            Messenger<string>.AddListener(ModelToPresenter.CURRENT_INPUT, OnUsingInput);
            Messenger<double>.AddListener(ModelToPresenter.TIME_UPDATE, OnTimeUpdate);
        }

        private void OnDisable()
        {
            Debug.Log("LP Disable");
            Messenger<string>.RemoveListener(ModelToPresenter.CURRENT_INPUT, OnUsingInput);
            Messenger<double>.AddListener(ModelToPresenter.TIME_UPDATE, OnTimeUpdate);
        }

        private void OnUsingInput(string input)
        {
            Debug.Log($"Using input {input} for the task.");
            
            // LoadedModels.Measurement.ResetTime();
        }

        private void Start()
        {
            var goals = goalMeshDataModel.LoadFromDisk();

            goalMeshModel.SetGoals(goals);

            if (!goalMeshModel.GoalMeshes.Any()) return;
            // Communicate with view to show current goal mesh!
            goalMesh.sharedMesh = goalMeshModel.GoalMeshes.First().Value;
            goalMesh.GetComponent<MeshRenderer>().enabled = true;
        }

        private void OnTimeUpdate(double dt)
        {
            timeLabel.text = $"Time is now: {TimeSpan.FromSeconds(dt):mm\\:ss}";
        }

        private void Update()
        {
            // if (LoadedModels.Measurement is null)
            // {
            //     timeLabel.text = "Time Measurement model not setup.";
            //     return;
            // }
            //
            // LoadedModels.Measurement.IncrementTime();
        }

        public void OnSubmitClicked()
        {
            var copy = MeshCopier.MakeCopy(currentCloth.sharedMesh);
            copy.vertices = copy.vertices.Select(v => currentCloth.transform.TransformPoint(v)).ToArray();
            
            // Save add generated mesh to the data model
            taskResultModel.AddUserGeneratedMesh(0, copy);
            
            // Save corresponding reference mesh to the data model
            goalMeshDataModel.SaveToDisk();
            
            // Compute differences between the two meshes, save that to the data model as well

            // Call data export model
            
            // Submit for each 

            Debug.Log("Submitted!");
            // Messenger.Broadcast(PresenterToModel.SUBMITTED);
        }
    }
}