using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Wright.Library.File;
using Wright.Library.File.SaveClasses;

namespace Models.Local
{
    [RequireComponent(typeof(GoalMeshCreationModel))]
    public class GoalMeshCreationDataModel : MonoBehaviour
    {
        public void SaveToDisk()
        {
            // Get all the generated meshes
            var model = GetComponent<GoalMeshCreationModel>();
            
            var fileGoalMeshes = new FileGoalMeshes();

            foreach (var (mesh, i) in model.GoalMeshes.Select((s, i) => (s, i)))
            {
                var goalMesh = new FileGoalMesh
                {
                    Vertices = mesh.vertices.Select(VectorToTuple).ToArray(),
                    Triangles = mesh.triangles
                };

                fileGoalMeshes.Meshes.Add(goalMesh);
            }

            const string filename = "new_task_goals.json";

            DictionaryFileHelper.WriteToFile(fileGoalMeshes, filename);
        }

        public static string MakeGoalKey(int i) => $"goal_{i}";

        public static (float, float, float) VectorToTuple(Vector3 v) => (v.x, v.y, v.z);
    }
}