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
        [Header("View")] [SerializeField] private TMP_Text participantStatus;
        [SerializeField] private TMP_Text progressionText;

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

                var (completed, total) = models.GetComponent<LevelProgressionModel>().GetProgression();
                progressionText.text = $"You have completed: {completed}/{total}";
            }
        }

        public void OnInputSwitched()
        {
            MDebug.Log("Notify that we switched input.");
            Messenger.Broadcast(PresenterToModel.SWITCHED_INPUT);
        }
    }
}