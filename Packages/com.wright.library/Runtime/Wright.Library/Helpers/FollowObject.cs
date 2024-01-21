using System;
using UnityEngine;

namespace Wright.Library.Helpers
{
    public class FollowObject : MonoBehaviour
    {
        [SerializeField] private Transform transformToFollow;

        private void Update()
        {
            transform.position = transformToFollow.position;
        }
    }
}