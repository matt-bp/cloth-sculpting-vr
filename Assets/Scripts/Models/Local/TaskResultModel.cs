using UnityEngine;
using System.Collections.Generic;
using Wright.Library.Mesh;

namespace Models.Local
{
    public class TaskResultModel : MonoBehaviour
    {
        public Dictionary<int, Mesh> UserGeneratedMeshes { get; } = new();

        public void AddUserGeneratedMesh(int keyframe, Mesh generatedMesh)
        {
            UserGeneratedMeshes.Add(keyframe, MeshCopier.MakeCopy(generatedMesh));
        }

    }
}