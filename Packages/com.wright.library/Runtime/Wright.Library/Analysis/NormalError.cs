using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace Wright.Library.Analysis
{
    public static class NormalError
    {
        public static float GetError(UnityEngine.Mesh goal, UnityEngine.Mesh generated)
        {
            Assert.IsTrue(goal.vertices.Length == generated.vertices.Length);
            Assert.IsTrue(goal.triangles.Length == generated.triangles.Length);
            Assert.IsTrue(goal.normals.Length == generated.normals.Length);
            Assert.IsTrue(goal.triangles.Zip(generated.triangles, (a, b) => a == b).All(x => x));

            float CalculateError(Vector3 n, Vector3 m)
            {
                return 0.5f * Math.Abs(Vector3.Dot(n, m) - 1);
            }
            
            return goal.normals.Zip(generated.normals, CalculateError).Sum();
        }
    }
}