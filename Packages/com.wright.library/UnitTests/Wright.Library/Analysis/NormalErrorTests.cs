using NUnit.Framework;
using UnityEngine;
using Wright.Library.Analysis;

namespace UnitTests.Wright.Library.Analysis
{
    public class NormalErrorTests
    { 
        [Test]
        public void GetError_OnTheSameMesh_ReturnsZero()
        {
            var goal = MakeGoalMesh();
            var generated = MakeGoalMesh();

            var result = NormalError.GetError(goal, generated);
            
            Assert.That(result, Is.EqualTo(0.0f).Within(0.0000001f));
        }
        
        [Test]
        public void GetError_WithNormalsFacingOppositeDirections_ReturnsOneForEveryVertex()
        {
            var goal = MakeGoalMesh();
            var generated = Make180DegFromGoal();

            var result = NormalError.GetError(goal, generated);
            
            // We're expecting an error of 1.0f for each vertex, normals are defined at the vertex level.
            Assert.That(result, Is.EqualTo(generated.vertices.Length * 1.0f));
        }
        
        [Test]
        public void GetError_WithNormalOffset90Deg_ReturnsHalfForEveryVertex()
        {
            var goal = MakeGoalMesh();
            var generated = Make90DegFromGoal();

            var result = NormalError.GetError(goal, generated);
            
            // We're expecting an error of 1.0f for each vertex, normals are defined at the vertex level.
            Assert.That(result, Is.EqualTo(generated.vertices.Length * (1/2f)));
        }
        
        /// <summary>
        /// A 45 degree offset will not result in an error of 1/4. This is because on the unit circle, the normal
        /// corresponding to that 45 degrees actually has components equal to the sqrt(2). When computing the dot
        /// product, we will use those sqrt(2) values, instead of 1/2, or whatever vertex positions we give it.
        /// </summary>
        [Test]
        public void GetError_WithNormalOffset45Deg_ReturnsErrorFromCircleForEveryVertex()
        {
            var goal = MakeGoalMesh();
            var generated = Make45DegFromGoal();

            var result = NormalError.GetError(goal, generated);
            
            // We're expecting an error of 1.0f for each vertex, normals are defined at the vertex level.
            Assert.That(result, Is.EqualTo(generated.vertices.Length * (0.1464466155f)).Within(0.0000001f));
        }
        
        #region Helpers

        private static Mesh MakeMesh(Vector3[] vertices, int[] triangles)
        {
            var mesh = new Mesh
            {
                vertices = vertices,
                triangles = triangles
            };

            mesh.RecalculateNormals();

            return mesh;
        }

        private static Mesh MakeGoalMesh()
        {
            var vertices = new Vector3[]
            {
                new(1, 0, 1),
                new(1, 2, 0),
                new(1, 0, -1)
            };

            var triangles = new[]
            {
                0, 1, 2
            };

            return MakeMesh(vertices, triangles);
        }

        private static Mesh Make180DegFromGoal()
        {
            var vertices = new Vector3[]
            {
                new(1, 0, -1),
                new(1, 2, 0),
                new(1, 0, 1)
            };

            var triangles = new[]
            {
                0, 1, 2
            };

            return MakeMesh(vertices, triangles);
        }
        
        private static Mesh Make90DegFromGoal()
        {
            var vertices = new Vector3[]
            {
                new(1, 0, 1),
                new(2, 0, 0),
                new(1, 0, -1)
            };

            var triangles = new[]
            {
                0, 1, 2
            };

            return MakeMesh(vertices, triangles);
        }
        
        private static Mesh Make45DegFromGoal()
        {
            var vertices = new Vector3[]
            {
                new(1, 0, 1),
                new(3, 2, 0),
                new(1, 0, -1)
            };

            var triangles = new[]
            {
                0, 1, 2
            };

            return MakeMesh(vertices, triangles);
        }
    
        #endregion
    }
}