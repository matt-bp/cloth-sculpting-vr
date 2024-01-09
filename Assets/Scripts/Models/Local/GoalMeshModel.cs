using System;
using System.Collections.Generic;
using UnityEngine;

namespace Models.Local
{
    public class GoalMeshModel : MonoBehaviour
    {
        public Dictionary<int, Mesh> GoalMeshes { get; private set; }

        public void SetGoals(Dictionary<int, Mesh> goals)
        {
            GoalMeshes = goals;

            OnShow += Console.WriteLine;

            OnShow?.Invoke("hi");
        }

        public event Action<string> OnShow;
    }
}