using System;
using UnityEngine;

namespace Wright.Library.Helpers
{
    public class FollowObject : MonoBehaviour
    {
        public Transform transformToFollow;

        private void Update()
        {
            transform.position = transformToFollow.position;
        }
    }
}