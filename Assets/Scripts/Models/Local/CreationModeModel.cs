using UnityEngine;

namespace Models.Local
{
    public class CreationModeModel : MonoBehaviour
    {
        [SerializeField] private bool creatingGoals;
        public bool CreatingGoals => creatingGoals;
    }
}