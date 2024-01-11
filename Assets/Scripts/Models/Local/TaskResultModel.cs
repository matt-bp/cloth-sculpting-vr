using UnityEngine;
using System.Collections.Generic;
using Wright.Library.Mesh;

namespace Models.Local
{
    public class TaskResultModel : MonoBehaviour
    {
        public class Result
        {
            public Mesh Mesh { get; set; }
            public double EuclideanError { get; set; }
            public double AngularError { get; set; }
        }
        
        public Dictionary<int, Result> UserGeneratedMeshes { get; } = new();
        
        // TODO: Include time here as well

        public void AddUserGeneratedMesh(int keyframe, Mesh generatedMesh, double euclideanError, double angularError)
        {
            var result = new Result
            {
                Mesh = MeshCopier.MakeCopy(generatedMesh),
                EuclideanError = euclideanError,
                AngularError = angularError
            };
            
            UserGeneratedMeshes.Add(keyframe, result);
        }

    }
}