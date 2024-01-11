using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Wright.Library.File;
using static Models.Local.GoalMeshCreationDataModel;

namespace Models.Local
{
    [RequireComponent(typeof(TaskResultModel))]
    public class DataExportModel : MonoBehaviour
    {
        public void SaveResults()
        {
            var model = GetComponent<TaskResultModel>();
            
            var overallData = new Dictionary<string, object>
            {
                { "num_meshes", model.UserGeneratedMeshes.Count },
                { "time", 1058 }
            };

            foreach (var (key, result) in model.UserGeneratedMeshes)
            {
                var goalMesh = new Dictionary<string, object>
                {
                    { "verts", result.Mesh.vertices.Select(VectorToTuple).ToArray() },
                    { "tris", result.Mesh.triangles },
                    { "e_error", result.EuclideanError },
                    { "a_error", result.AngularError }
                };

                overallData.Add(MakeGoalKey(key), goalMesh);
            }

            var id = Guid.NewGuid();
            Debug.Log($"Id is {id}");
            var filename = $"new_results_{id}.dat";

            DictionaryFileHelper.WriteToFile(overallData, filename);
        }
    }
}