using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using Wright.Library.File;
using Wright.Library.File.SaveClasses;

namespace Models.Local
{
    [RequireComponent(typeof(GoalMeshModel))]
    public class GoalMeshDataModel : MonoBehaviour
    {
        public event Action OnMeshesMissing;
        public event Action<Dictionary<int, Mesh>> OnMeshesFound;
        
        public void LoadFromDisk(int task)
        {
            var results = new Dictionary<int, Mesh>();
        
            var filename = $"tasks/task_{task}.json";

            var jsonTextFile = Resources.Load<TextAsset>(filename);

            if (jsonTextFile == null)
            {
                OnMeshesMissing?.Invoke();
                return; 
            }

            var data = JsonConvert.DeserializeObject<FileGoalMeshes>(jsonTextFile.text);
            
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
                
                results.Add(index, mesh);
            }

            OnMeshesFound?.Invoke(results);
        }

        private static Vector3 TupleToVector((float, float, float) t) => new(t.Item1, t.Item2, t.Item3);
    }
}