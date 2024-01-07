using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Wright.Library.File;

namespace Models.Local
{
    [RequireComponent(typeof(TaskResultModel))]
    public class GoalMeshDataModel : MonoBehaviour
    {
        public void SaveToDisk()
        {
            // Get all the generated meshes
            var model = GetComponent<TaskResultModel>();

            var overallData = new Dictionary<string, object>();
            
            overallData.Add("num_goals", model.UserGeneratedMeshes.Count);

            // For each user generated mesh, serialize it to a file
            foreach (var userGeneratedMesh in model.UserGeneratedMeshes)
            {
                var goalMesh = new Dictionary<string, object>
                {
                    { "verts", userGeneratedMesh.Value.vertices.Take(2).Select(v => (v.x, v.y, v.z)).ToList() }
                };
                
                overallData.Add(MakeGoalKey(userGeneratedMesh.Key), goalMesh);
            }
            
            var filename = "goal_task_1.dat";

            DictionaryFileHelper.WriteToFile(overallData, filename);
        }

        public Dictionary<int, Mesh> LoadFromDisk()
        {
            var results = new Dictionary<int, Mesh>();
            
            var filename = "goal_task_1.dat";

            if (!DictionaryFileHelper.LoadFromDisk(filename, out var data))
            {
                return results;
            }

            Debug.Assert(data.ContainsKey("num_goals"));
            var numGoals = (int)data["num_goals"];
            Debug.Log("Num goals" + numGoals);

            foreach (var i in Enumerable.Range(0, numGoals))
            {
                Debug.Assert(data.ContainsKey(MakeGoalKey(i)));
                var d2 = (Dictionary<string, object>)data[MakeGoalKey(i)];
                Debug.Assert(d2.ContainsKey("verts"));
                var verts = (List<(float, float, float)>)d2["verts"];
                Debug.Log($"Size is: {verts.Count}");
                
                // Reconstruct meshes here, and add them to something?
            }

            return results;
        }

        private string MakeGoalKey(int i) => $"goal_{i}";
    }
}