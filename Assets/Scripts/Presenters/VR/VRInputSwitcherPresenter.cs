using System;
using Events;
using Models.Local;
using UnityEngine;
using UnityEngine.InputSystem;
using Wright.Library.Logging;
using Wright.Library.Messages;

namespace Presenters.VR
{
    [AddComponentMenu("Wright/VR/VR Input Switcher Presenter")]
    public class VRInputSwitcherPresenter : MonoBehaviour
    {
        [Header("Model")] [SerializeField] private VRStateModel vrStateModel;

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