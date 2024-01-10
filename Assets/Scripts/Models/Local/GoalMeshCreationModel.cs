using System;
using System.Collections.Generic;
using UnityEngine;
using Wright.Library.GoalMeshes;

namespace Models.Local
{
    public class GoalMeshCreationModel : MonoBehaviour
    {
        public float Time { get; private set; }
        public List<MeshesTimePair> GoalMeshes { get; } = new();
        public event Action OnMeshAdded;

        public void AddGoalMeshAtCurrentTime(Mesh goalMesh)
        {
            GoalMeshes.Add(new MeshesTimePair
            {
                Time = Time,
                Mesh = goalMesh
            });

            OnMeshAdded?.Invoke();
        }

        public void SetTime(float newTime)
        {
            Time = newTime;
        }
    }
}