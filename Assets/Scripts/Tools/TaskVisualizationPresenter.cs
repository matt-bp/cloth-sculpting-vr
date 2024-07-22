using System;
using System.Collections.Generic;
using System.Linq;
using Models.Local;
using UnityEditor;
using UnityEngine;

namespace Tools
{
    public class TaskVisualizationPresenter : MonoBehaviour
    {
        private List<GameObject> _visualizedGoalMeshes = new();
        
        [Header("Prefabs")] [SerializeField] private GameObject goalMeshPrefab;

        [Header("Model")] [SerializeField] private GoalMeshDataModel goalMeshDataModel;
        
        [Header("View")] [SerializeField] private MeshFilter currentCloth;

        private void Start()
        {
            goalMeshDataModel.OnMeshesFound += HandleGoalMeshDataModelOnOnMeshesFound;
            
            goalMeshDataModel.LoadFromDisk(0);
        }

        private void HandleGoalMeshDataModelOnOnMeshesFound(Dictionary<int, Mesh> obj)
        {
            Debug.Log($"Found {obj.Count}");

            VisualizeGoalMeshes(obj.Select(x => x.Value));
        }
        
        private void VisualizeGoalMeshes(IEnumerable<Mesh> meshes)
        {
            foreach (var meshObject in _visualizedGoalMeshes)
            {
                Destroy(meshObject);
            }

            _visualizedGoalMeshes = new List<GameObject>();

            var count = 0;

            foreach (var mesh in meshes)
            {
                if (count == 0)
                {
                    currentCloth.sharedMesh = mesh;
                }   
                else
                {
                    var instance = Instantiate(goalMeshPrefab);
                    instance.GetComponent<MeshFilter>().sharedMesh = mesh;
                    _visualizedGoalMeshes.Add(instance);
                }

                count++;
            }
        }
    }
}