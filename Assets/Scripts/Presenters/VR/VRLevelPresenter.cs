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
        [SerializeField] private TaskResultModel taskResultModel;
        [SerializeField] private GoalMeshModel goalMeshModel;
        [SerializeField] private GoalMeshDataModel goalDataModel;
        [SerializeField] private TaskRunnerModel taskRunner;
        [SerializeField] private DataExportModel dataModel;
        
        [Header("View")] [SerializeField] private XROrigin xrOrigin;
        [SerializeField] private MeshFilter currentCloth;
        [SerializeField] private MeshFilter goalMesh;
        
        private void OnEnable()
        {
            MDebug.Log("Enabled");
            StartCoroutine(vrStateModel.StartVR());
        }
        
        private void OnDisable()
        {
            MDebug.Log("Disabled");
            vrStateModel.StopVR();
        }

        public void HelloWorld()
        {
            MDebug.Log("Hello world!");
        }

        public void ResetPosition()
        {
            xrOrigin.MoveCameraToWorldLocation(Vector3.zero);
        }
    }
}