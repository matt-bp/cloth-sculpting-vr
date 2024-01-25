using Events;
using Models.Global;
using TMPro;
using UnityEngine;
using Wright.Library.Logging;
using Wright.Library.Messages;

namespace Presenters
{
    public class InputSwitcherPresenter : MonoBehaviour
    {
        [Header("View")]
        [SerializeField] private TMP_Text participantStatus;
        
        private void Start()
        {
            var models = GameObject.FindWithTag("Global Models");

            if (models == null)
            {
                participantStatus.color = Color.red;
                participantStatus.text = "Can't find em!";
            }
            else
            {
                var participant = models.GetComponent<ParticipantModel>();
                participantStatus.text = $"P {participant.DisplayParticipantNumber}";
            }
        }
        
        public void OnInputSwitched()
        {
            MDebug.Log("Notify that we switched input.");
            Messenger.Broadcast(PresenterToModel.SWITCHED_INPUT);
        }
    }
}