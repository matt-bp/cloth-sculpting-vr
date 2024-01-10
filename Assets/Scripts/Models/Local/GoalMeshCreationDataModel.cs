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

        // public Dictionary<int, Mesh> LoadFromDisk()
        // {
        //     var results = new Dictionary<int, Mesh>();
        //
        //     var filename = "goal_task_1.dat";
        //
        //     if (!DictionaryFileHelper.LoadFromDisk(filename, out var data))
        //     {
        //         return results;
        //     }
        //
        //     Debug.Assert(data.ContainsKey("num_goals"));
        //     var numGoals = (int)data["num_goals"];
        //     Debug.Log("Num goals" + numGoals);
        //
        //     foreach (var i in Enumerable.Range(0, numGoals))
        //     {
        //         Debug.Assert(data.ContainsKey(MakeGoalKey(i)));
        //         var d2 = (Dictionary<string, object>)data[MakeGoalKey(i)];
        //         Debug.Assert(d2.ContainsKey("verts"));
        //         var vertices = (((float, float, float)[])d2["verts"]).Select(TupleToVector).ToArray();
        //         Debug.Log($"Size is: {vertices.Length}, {vertices[0]}");
        //         
        //         var triangles = (int[])d2["tris"];
        //
        //         var mesh = new Mesh
        //         {
        //             name = $"Imported {i}",
        //             vertices = vertices,
        //             triangles = triangles
        //         };
        //         
        //         mesh.RecalculateNormals();
        //
        //         // Reconstruct meshes here, and add them to something?
        //         results.Add(i, mesh);
        //     }
        //
        //     return results;
        // }

        public static string MakeGoalKey(int i) => $"goal_{i}";

        private (float, float, float) VectorToTuple(Vector3 v) => (v.x, v.y, v.z);
    }
}