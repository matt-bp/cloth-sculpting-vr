using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Wright.Library.File;
using Wright.Library.GoalMeshes;

namespace Models.Local
{
    [RequireComponent(typeof(GoalMeshCreationModel))]
    public class GoalMeshCreationDataModel : MonoBehaviour
    {
        public void SaveToDisk()
        {
            // Get all the generated meshes
            var model = GetComponent<GoalMeshCreationModel>();

            var overallData = new Dictionary<string, object> { { "num_goals", model.GoalMeshes.Count } };
            
            foreach (var (mtp, i) in model.GoalMeshes.Select((s, i) => (s, i)))
            {
                var goalMesh = new Dictionary<string, object>
                {
                    { "verts", mtp.Mesh.vertices.Select(VectorToTuple).ToArray() },
                    { "tris", mtp.Mesh.triangles },
                    { "time", mtp.Time }
                };

                overallData.Add(MakeGoalKey(i), goalMesh);
            }

            const string filename = "new_task_goals.dat";

            DictionaryFileHelper.WriteToFile(overallData, filename);
        }

        public static string MakeGoalKey(int i) => $"goal_{i}";

        public static (float, float, float) VectorToTuple(Vector3 v) => (v.x, v.y, v.z);
    }
}