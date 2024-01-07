using Events;
using UnityEngine;
using Wright.Library.Messages;

namespace Models.Global
{
    public class Startup : MonoBehaviour
    {
        private void Start()
        {
            Messenger.Broadcast(GameEvents.START);
        }
    }
}