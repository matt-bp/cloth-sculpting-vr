using UnityEngine;

namespace Models.Global
{
    public class ParticipantModel : MonoBehaviour
    {
        [SerializeField] [Tooltip("Number of this participant, for use in Latin Square design.")]
        private int participantNumber;

        /// <summary>The index of the participant, starting at 0.</summary>
        public int ParticipantNumber
        {
            get => participantNumber;
            set => participantNumber = value;
        }

        public int DisplayParticipantNumber => participantNumber + 1;
        
        [SerializeField] [Tooltip("Does the user prefer to use their left hand in VR?")]
        private bool useLeftHand;
        public bool UseLeftHand
        {
            get => useLeftHand;
            set => useLeftHand = value;
        }
    }
}