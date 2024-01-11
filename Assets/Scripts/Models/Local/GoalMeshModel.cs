using System;
using System.Collections.Generic;
using UnityEngine;
using Wright.Library.GoalMeshes;

namespace Models.Local
{
    public class GoalMeshModel : MonoBehaviour
    {
        public Dictionary<int, MeshesTimePair> GoalMeshes { get; set; }
    }
}