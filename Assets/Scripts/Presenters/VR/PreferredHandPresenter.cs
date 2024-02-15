using System;
using System.Collections.Generic;
using System.Linq;
using Models.Global;
using Models.Local;
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
        [SerializeField] private GameObject leftControllerProgressParent;
        [SerializeField] private GameObject grabRightController;
        [SerializeField] private GameObject movementRightController;
        [SerializeField] private GameObject rightControllerProgressParent;

        // Set these to the direct interactor game object component of the grab controller game object
        [SerializeField] private FollowObject[] controllerColliderFollowers;
        [SerializeField] private TaskProgressPositionsModel progressPositionsModel;
        
        public bool defaultToLeftHandGrab;
        
        private void Start()
        {
            var models = GameObject.FindWithTag("Global Models");
            if (models != null)
            {
                var participant = models.GetComponent<ParticipantModel>();
                ApplyPreference(participant.UseLeftHand);
            }
            else
            {
                ApplyPreference(defaultToLeftHandGrab);
            }
        }

        private void ApplyPreference(bool isLeftHandGrab)
        {
            SetGrabControllerState(grabLeftController, isLeftHandGrab);
            SetMovementControllerState(movementRightController, isLeftHandGrab);
            
            SetGrabControllerState(grabRightController, !isLeftHandGrab);
            SetMovementControllerState(movementLeftController, !isLeftHandGrab);
            
            progressPositionsModel.positions = isLeftHandGrab
                ? rightControllerProgressParent.transform.Cast<Transform>().ToArray()
                : leftControllerProgressParent.transform.Cast<Transform>().ToArray();

            foreach (var follower in controllerColliderFollowers)
            {
                follower.transformToFollow =
                    isLeftHandGrab ? grabLeftController.transform : grabRightController.transform;
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