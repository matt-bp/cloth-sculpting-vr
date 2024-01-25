using UnityEngine;

namespace Models.Global
{
    public class ParticipantModel : MonoBehaviour
    {
        [SerializeField] [Tooltip("Number of this participant, for use in Latin Square design.")]
        private int participantNumber;

        /// <summary>The current active manager used to manage XR lifetime.</summary>
        public int ParticipantNumber
        {
            get => participantNumber;
            set => participantNumber = value;
        }
    }
}