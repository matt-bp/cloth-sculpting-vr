using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;
using Wright.Library.Helpers;

namespace Presenters.VR
{
    public class PreferredHandPresenter : MonoBehaviour
    {
        [SerializeField] private GameObject grabLeftController;
        [SerializeField] private GameObject movementLeftController;
        [SerializeField] private GameObject grabRightController;
        [SerializeField] private GameObject movementRightController;

        // Set these to the direct interactor game object component of the grab controller game object
        [SerializeField] private FollowObject[] controllerColliderFollowers;
        
        public bool leftHandGrab;
        
        private void Start()
        {
            SetGrabControllerState(grabLeftController, leftHandGrab);
            SetMovementControllerState(movementRightController, leftHandGrab);
            
            SetGrabControllerState(grabRightController, !leftHandGrab);
            SetMovementControllerState(movementLeftController, !leftHandGrab);
            
            foreach (var follower in controllerColliderFollowers)
            {
                follower.transformToFollow =
                    leftHandGrab ? grabLeftController.transform : grabRightController.transform;
            }
        }

        private void SetGrabControllerState(GameObject controller, bool isEnabled)
        {
            controller.SetActive(isEnabled);
            controller.GetComponent<ActionBasedControllerManager>().enabled = isEnabled;
            controller.GetComponent<ActionBasedController>().enabled = isEnabled;
            // controller.GetComponentInChildren<XRDirectInteractor>().enabled = true;
        }

        private void SetMovementControllerState(GameObject controller, bool isEnabled)
        {
            controller.SetActive(isEnabled);
            controller.GetComponent<ActionBasedControllerManager>().enabled = isEnabled;
            controller.GetComponent<ActionBasedController>().enabled = isEnabled;
        }
    }
}