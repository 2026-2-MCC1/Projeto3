using UnityEngine;
using UnityEngine.Events;

namespace Parkour
{
    /// Coloque num objeto com um Collider "Is Trigger" no final da fase.
    public class GoalZone : MonoBehaviour
    {
        public UnityEvent onGoal;

        bool reached;
        float startTime, finalTime;

        void Start() => startTime = Time.time;

        void OnTriggerEnter(Collider other)
        {
            if (reached || !other.GetComponent<PlayerRespawn>()) return;
            reached = true;
            finalTime = Time.time - startTime;
            onGoal?.Invoke();
        }

        void OnGUI()
        {
            if (!reached) return;
            var style = new GUIStyle(GUI.skin.label) { fontSize = 36, alignment = TextAnchor.MiddleCenter };
            GUI.Label(new Rect(0, Screen.height / 2f - 40, Screen.width, 80), $"CHEGOU!  Tempo: {finalTime:0.0}s", style);
        }
    }
}
