using System;
using System.Collections.Generic;
using System.Linq;
using GrabTool.Mesh;
using Models.Global;
using Models.Local;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;
using Wright.Library.Helpers;
using Wright.Library.Logging;

namespace Presenters.VR
{
    public class PreferredHandPresenter : MonoBehaviour
    {
        [Header("Controller Objects")] [SerializeField]
        private GameObject grabLeftController;

        [SerializeField] private GameObject movementLeftController;
        [SerializeField] private GameObject leftControllerProgressParent;
        [SerializeField] private GrabMoveProvider leftControllerGrabMoveProvider;
        [SerializeField] private GameObject grabRightController;
        [SerializeField] private GameObject movementRightController;
        [SerializeField] private GameObject rightControllerProgressParent;
        [SerializeField] private GrabMoveProvider rightControllerGrabMoveProvider;

        [Header("Miscellaneous things to update")]
        // Set these to the direct interactor game object component of the grab controller game object
        [SerializeField]
        private FollowObject[] controllerColliderFollowers;

        [Header("Dynamic Action References")] [SerializeField]
        private InputActionReference grabActionReference;

        [SerializeField] private InputActionReference toggleSizeActionReference;
        [SerializeField] private InputActionReference changeSizeActionReference;
        [SerializeField] private VRMeshDragger vrMeshDragger;

        [Header("Model")] [SerializeField] private TaskProgressPositionsModel progressPositionsModel;

        public bool defaultToLeftHandGrab;

        private void Start()
        {
            var models = GameObject.FindWithTag("Global Models");
            if (models != null)
            {
                MDebug.Log("Got global models");
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
            MDebug.Log($"Is using left hand as grab: {isLeftHandGrab}");

            SetActions(isLeftHandGrab);

            SetGrabControllerState(grabLeftController, isLeftHandGrab);
            SetMovementControllerState(movementRightController, isLeftHandGrab);

            SetGrabControllerState(grabRightController, !isLeftHandGrab);
            SetMovementControllerState(movementLeftController, !isLeftHandGrab);

            leftControllerGrabMoveProvider.controllerTransform =
                isLeftHandGrab ? grabLeftController.transform : movementLeftController.transform;

            rightControllerGrabMoveProvider.controllerTransform =
                !isLeftHandGrab ? grabRightController.transform : movementRightController.transform;

            progressPositionsModel.positions = isLeftHandGrab
                ? rightControllerProgressParent.transform.Cast<Transform>().ToArray()
                : leftControllerProgressParent.transform.Cast<Transform>().ToArray();

            foreach (var follower in controllerColliderFollowers)
            {
                follower.transformToFollow =
                    isLeftHandGrab ? grabLeftController.transform : grabRightController.transform;
            }

            vrMeshDragger.SphereCollider = isLeftHandGrab
                ? grabLeftController.GetComponentInChildren<SphereCollider>()
                : grabRightController.GetComponentInChildren<SphereCollider>();
        }

        private void SetGrabControllerState(GameObject controller, bool isEnabled)
        {
            controller.SetActive(isEnabled);
            controller.GetComponent<ActionBasedControllerManager>().enabled = isEnabled;
            controller.GetComponent<ActionBasedController>().enabled = isEnabled;
        }

        private void SetMovementControllerState(GameObject controller, bool isEnabled)
        {
            controller.SetActive(isEnabled);
            controller.GetComponent<ActionBasedControllerManager>().enabled = isEnabled;
            controller.GetComponent<ActionBasedController>().enabled = isEnabled;
        }

        private void SetActions(bool isLeftHandGrab)
        {
            var controller = isLeftHandGrab ? "LeftHand" : "RightHand";

            grabActionReference.action.AddBinding($"<XRController>{{{controller}}}/{{GripButton}}");
            toggleSizeActionReference.action.AddBinding($"<XRController>{{{controller}}}/{{Primary2DAxisClick}}");
            var writeAccessBinding = changeSizeActionReference.action.ChangeBinding(0);
            writeAccessBinding.WithPath($"<XRController>{{{controller}}}/{{Primary2DAxis}}");
        }
    }
}