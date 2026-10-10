using UnityEngine;
using UnityEngine.Events;

namespace Parkour
{
    /// Coloque num objeto com um Collider "Is Trigger".
    public class Checkpoint : MonoBehaviour
    {
        public Transform respawnPoint;
        public UnityEvent onActivated;

        bool active;

        void OnTriggerEnter(Collider other)
        {
            var player = other.GetComponent<PlayerRespawn>();
            if (!player || !respawnPoint) return;
            player.SetCheckpoint(respawnPoint);
            if (!active) { active = true; onActivated?.Invoke(); }
        }
    }
}
