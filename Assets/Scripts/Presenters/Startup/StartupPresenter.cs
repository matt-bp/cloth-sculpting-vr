using Models.Global;
using UnityEngine;
using UnityEngine.Assertions;
using Wright.Library.Logging;

namespace Presenters.Startup
{
    public class StartupPresenter : MonoBehaviour
    {
        [SerializeField] private ParticipantModel participantModel;
        [SerializeField] private LevelProgressionModel levelProgressionModel;
        
        public void OnParticipantNumberChange(int p)
        {
            MDebug.Log($"Participant number is {p}");
            participantModel.ParticipantNumber = p;
        }

        public void OnParticipantReady()
        {
            Assert.IsTrue(participantModel.ParticipantNumber is >= 0 and < 20);
            
            levelProgressionModel.StartFirstSwitchScene();
        }

        public void OnHandPreferenceChanged(bool preference)
        {
            participantModel.UseLeftHand = preference;
        }
    }
}