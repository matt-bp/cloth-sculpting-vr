using System;
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

        public event Action<float> OnTimeUpdate;
        private float _timeSinceLastUpdate = 0;
        
        public Dictionary<int, Result> UserGeneratedMeshes { get; } = new();
        public float ElapsedTime { get; private set; }

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

        public void AddTime(float dt)
        {
            ElapsedTime += dt;
            _timeSinceLastUpdate += dt;

            if (_timeSinceLastUpdate >= 1)
            {
                _timeSinceLastUpdate -= 1;
                OnTimeUpdate?.Invoke(ElapsedTime);
            }
        }

    }
}