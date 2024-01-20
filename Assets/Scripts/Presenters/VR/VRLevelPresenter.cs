using Events;
using Models.Local;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using Wright.Library.Logging;
using Wright.Library.Messages;

namespace Presenters.VR
{
    [AddComponentMenu("Wright/VR/VR Level Presenter")]
    public class VRLevelPresenter : MonoBehaviour
    {
        [Header("Model")] [SerializeField] private VRStateModel vrStateModel;

        [Header("View")] [SerializeField] private XROrigin xrOrigin;
        
        private void OnEnable()
        {
            MDebug.Log("Starting VR...");
            StartCoroutine(vrStateModel.StartVR());
        }

        private void Update()
        {
            // var action = addAndNext.action;
            //
            // if (action.WasPressedThisFrame() && action.IsPressed())
            // {
            //     TaskComplete();
            // }
        }
        
        public void TaskComplete()
        {
            MDebug.Log("Task complete");
 
            Messenger.Broadcast(PresenterToModel.TASK_COMPLETE);
        }

        public void ResetPosition()
        {
            xrOrigin.transform.position = Vector3.zero;
        }

        private void OnDisable()
        {
            MDebug.Log("Stopping VR...");
            vrStateModel.StopVR();
        }
    }
}