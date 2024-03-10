using UnityEngine;
using UnityEngine.Events;

namespace Presenters
{
    public class PauseToDisabledAdapter : MonoBehaviour
    {
        public UnityEvent<bool> disable;

        public void Pause()
        {
            disable.Invoke(true);
        }

        public void Resume()
        {
            disable.Invoke(false);
        }
    }
}