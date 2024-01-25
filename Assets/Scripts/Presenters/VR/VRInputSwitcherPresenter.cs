using System;
using Events;
using Models.Global;
using Models.Local;
using TMPro;
using UnityEngine;
using Wright.Library.Logging;
using Wright.Library.Messages;

namespace Presenters.VR
{
    [AddComponentMenu("Wright/VR/VR Input Switcher Presenter")]
    public class VRInputSwitcherPresenter : MonoBehaviour
    {
        [Header("Model")] [SerializeField] private VRStateModel vrStateModel;

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

        private void OnEnable()
        {
            MDebug.Log("Starting VR...");
            StartCoroutine(vrStateModel.StartVR());
        }

        public void SwitchedInput()
        {
            MDebug.Log("Notify that we switched input.");
            Messenger.Broadcast(PresenterToModel.SWITCHED_INPUT);
        }
    }
}