using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Wright.Library.File;

namespace Models.Local
{
    [RequireComponent(typeof(TaskResultModel))]
    public class GoalMeshExportModel : MonoBehaviour
    {
        public void SaveMeshesToDisk()
        {
            // Get all the generated meshes
            var model = GetComponent<TaskResultModel>();

            var overallData = new Dictionary<string, object>();

            // For each user generated mesh, serialize it to a file
            foreach (var userGeneratedMesh in model.UserGeneratedMeshes)
            {

                var goalMesh = new Dictionary<string, object>
                {
                    { "verts", userGeneratedMesh.Value.vertices.Take(2).Select(v => (v.x, v.y, v.z)).ToList() }
                };
                
                overallData.Add($"goal_{userGeneratedMesh.Key}", goalMesh);
                

            }
            
            var filename = $"goal_task_1.dat";

            DictionaryWriter.WriteToFile(overallData, filename);
        }
    }
}