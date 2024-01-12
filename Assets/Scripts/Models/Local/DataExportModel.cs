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
                { "type", "result" },
                { "num_meshes", model.UserGeneratedMeshes.Count },
                { "time", model.ElapsedTime }
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
            var filename = $"new_results_{id}.json";

            DictionaryFileHelper.WriteToFile(overallData, filename);
        }
    }
}