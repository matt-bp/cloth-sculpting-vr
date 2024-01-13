using System;
using System.Collections.Generic;
using UnityEngine;

namespace Models.Local
{
    public class GoalMeshCreationModel : MonoBehaviour
    {
        public List<Mesh> GoalMeshes { get; } = new();
        public event Action OnMeshAdded;

        public void AddGoalMesh(Mesh goalMesh)
        {
            GoalMeshes.Add(goalMesh);

            OnMeshAdded?.Invoke();
        }
    }
}