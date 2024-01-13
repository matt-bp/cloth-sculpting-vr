using System.Linq;
using UnityEngine;

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

        private void Start()
        {
            _taskResultModel = GetComponent<TaskResultModel>();
            _goalMeshModel = GetComponent<GoalMeshModel>();
        }

        public bool ProgressToNextMesh()
        {
            var (keyframe, mesh) = _goalMeshModel.GoalMeshes
                .OrderBy(g => g.Key)
                .FirstOrDefault(g => !_taskResultModel.UserGeneratedMeshes.ContainsKey(g.Key));

            CurrentMesh = mesh;
            CurrentKeyframe = keyframe;
            
            return mesh != null;
        }
    }
}