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

        [Header("View")] [SerializeField] private InputActionReference temp;
        private void OnEnable()
        {
            MDebug.Log("Starting VR...");
            StartCoroutine(vrStateModel.StartVR());
        }

        private void Update()
        {
            var action = temp.action;

            if (action.WasPressedThisFrame() && action.IsPressed())
            {
                MDebug.Log("Notify that we switched input.");
                Messenger.Broadcast(PresenterToModel.SWITCHED_INPUT);
            }
        }

        private void OnDisable()
        {
            // MDebug.Log("Stopping VR...");
            // vrStateModel.StopVR();
        }
    }
}