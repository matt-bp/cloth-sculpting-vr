using System.Linq;
using System.Security;
using UnityEngine;
using UnityEngine.Events;
using Wright.Library.Logging;

namespace Models.Local
{
    [RequireComponent(typeof(TaskResultModel))]
    [RequireComponent(typeof(GoalMeshModel))]
    public class TaskRunnerModel : MonoBehaviour
    {
        private TaskResultModel _taskResultModel;
        private GoalMeshModel _goalMeshModel;
        public Mesh CurrentMesh { get; private set; }
        public int CurrentKeyframe { get; private set; }
        public UnityEvent<(int current, int total)> onNextMesh;
        public UnityEvent onMeshesFinished;
        public UnityEvent onNoMeshesFound;
        
        private void Start()
        {
            _taskResultModel = GetComponent<TaskResultModel>();
            _goalMeshModel = GetComponent<GoalMeshModel>();
        }

        public void ProgressToNextMesh()
        {
            if (_goalMeshModel.GoalMeshes == null)
            {
                onNoMeshesFound.Invoke();
                return;
            }
            
            var (keyframe, mesh) = _goalMeshModel.GoalMeshes
                .OrderBy(g => g.Key)
                .FirstOrDefault(g => !_taskResultModel.UserGeneratedMeshes.ContainsKey(g.Key));
            
            CurrentMesh = mesh;
            CurrentKeyframe = keyframe;

            if (mesh != null)
            {
                onNextMesh.Invoke((keyframe, _goalMeshModel.GoalMeshes.Count));
            }
            else
            {
                onMeshesFinished.Invoke();
            }
        }
    }
}