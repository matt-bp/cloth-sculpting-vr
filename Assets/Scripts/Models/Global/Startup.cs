using Events;
using UnityEngine;
using Wright.Library.Messages;

namespace Models.Global
{
    [RequireComponent(typeof(LevelProgressionModel))]
    public class Startup : MonoBehaviour
    {
        private void Start()
        {
            DontDestroyOnLoad(gameObject); // Keeps the level progression model alive
            
            Messenger.Broadcast(GameEvents.START);
        }
    }
}