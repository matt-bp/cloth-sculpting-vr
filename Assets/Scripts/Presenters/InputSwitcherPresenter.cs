using Events;
using TMPro;
using UnityEngine;
using Wright.Library.Messages;
using Wright.Library.Study;

namespace Presenters
{
    public class InputSwitcherPresenter : MonoBehaviour
    {
        [Header("View")]
        [SerializeField] private TMP_Text currentInput;
        
        private void Start()
        {
            currentInput.text = "Switch to XXX (change to actual scene that will do this)";
        }
        
        public void OnInputSwitched()
        {
            Messenger.Broadcast(PresenterToModel.SWITCHED_INPUT);
        }
    }
}