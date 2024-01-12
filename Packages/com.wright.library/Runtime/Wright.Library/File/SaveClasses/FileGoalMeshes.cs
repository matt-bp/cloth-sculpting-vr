using System.Collections.Generic;

namespace Wright.Library.File.SaveClasses
{
    public class FileGoalMesh
    {
        public (float, float, float)[] Vertices { get; set; }
        public int[] Triangles { get; set; }
        public float Time { get; set; }
    }
    
    public class FileGoalMeshes
    {
        public List<FileGoalMesh> Meshes = new();
    }
}