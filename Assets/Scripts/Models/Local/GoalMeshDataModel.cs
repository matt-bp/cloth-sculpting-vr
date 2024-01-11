using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Wright.Library.File;
using Wright.Library.GoalMeshes;
using static Models.Local.GoalMeshCreationDataModel;

namespace Models.Local
{
    [RequireComponent(typeof(GoalMeshModel))]
    public class GoalMeshDataModel : MonoBehaviour
    {
        public event Action OnMeshesMissing;
        public event Action<Dictionary<int, MeshesTimePair>> OnMeshesFound;
        
        public void LoadFromDisk(int task)
        {
            var results = new Dictionary<int, MeshesTimePair>();
        
            var filename = $"tasks/task_{task}.dat";
        
            if (!DictionaryFileHelper.LoadFromDisk(filename, out var data))
            {
                OnMeshesMissing?.Invoke();
                return;
            }
        
            Debug.Assert(data.ContainsKey("num_goals"));
            var numGoals = (int)data["num_goals"];
            Debug.Log("Num goals" + numGoals);
        
            foreach (var i in Enumerable.Range(0, numGoals))
            {
                Debug.Assert(data.ContainsKey(MakeGoalKey(i)));
                var d2 = (Dictionary<string, object>)data[MakeGoalKey(i)];
                Debug.Assert(d2.ContainsKey("verts"));
                var vertices = (((float, float, float)[])d2["verts"]).Select(TupleToVector).ToArray();
                Debug.Log($"Size is: {vertices.Length}, {vertices[0]}");
                
                var triangles = (int[])d2["tris"];
        
                var mesh = new Mesh
                {
                    name = $"Imported Goal {i}",
                    vertices = vertices,
                    triangles = triangles
                };
                
                mesh.RecalculateNormals();
        
                // Reconstruct meshes here, and add them to something?
                results.Add(i, new MeshesTimePair
                {
                    Time = (float)d2["time"],
                    Mesh = mesh
                });
            }
        
            OnMeshesFound?.Invoke(results);
        }

        private static Vector3 TupleToVector((float, float, float) t) => new(t.Item1, t.Item2, t.Item3);
    }
}