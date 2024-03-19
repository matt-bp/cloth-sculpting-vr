using UnityEngine;
using Wright.Library.Logging;

namespace Wright.Library.Analysis
{
    public class AnalyticsController : MonoBehaviour
    {
        public void OnBrushOrientationToggle(bool newValue)
        {
            MDebug.Log($"Toggled brush orientation setting to {newValue}");
        }

        public void OnDraggingSet(int newValue)
        {
            MDebug.Log($"Changed on dragging value (brush movement type) to {newValue}");
        }
    }
}