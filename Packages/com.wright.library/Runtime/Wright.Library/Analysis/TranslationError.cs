using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace Wright.Library.Analysis
{
    public static class TranslationError
    {
        public static float GetError(UnityEngine.Mesh goal, UnityEngine.Mesh generated)
        {
            Assert.IsTrue(goal.vertices.Length == generated.vertices.Length);
            Assert.IsTrue(goal.triangles.Length == generated.triangles.Length);
            Assert.IsTrue(goal.triangles.Zip(generated.triangles, (a, b) => a == b).All(x => x));

            return 0;
        }
    }
}