using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Wright.Library.File;
using Wright.Library.File.SaveClasses;
using Wright.Library.GoalMeshes;

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
        
            var filename = $"tasks/task_{task}.json";
        
            if (!DictionaryFileHelper.LoadFromDisk<FileGoalMeshes>(filename, out var data))
            {
                OnMeshesMissing?.Invoke();
                return;
            }
            
            foreach (var (value, index) in data.Meshes.Select((v, i) => (v, i)))
            {
                var mesh = new Mesh()
                {
                    name = $"Imported Goal {index}",
                    vertices = value.Vertices.Select(TupleToVector).ToArray(),
                    triangles = value.Triangles
                };
                mesh.RecalculateNormals();
                mesh.RecalculateBounds();
                
                results.Add(index, new MeshesTimePair
                {
                    Time = value.Time,
                    Mesh = mesh
                });
            }

            OnMeshesFound?.Invoke(results);
        }

        private static Vector3 TupleToVector((float, float, float) t) => new(t.Item1, t.Item2, t.Item3);
    }
}