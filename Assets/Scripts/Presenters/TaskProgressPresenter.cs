using UnityEngine;

namespace Presenters
{
    public class TaskProgressPresenter : MonoBehaviour
    {
        public void UpdateProgressVisualization((int current, int total) value)
        {
            Debug.Log(value.current);
        }
    }
}