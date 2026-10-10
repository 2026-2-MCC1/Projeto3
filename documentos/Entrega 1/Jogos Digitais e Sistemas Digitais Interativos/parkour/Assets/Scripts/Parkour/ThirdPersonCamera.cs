using UnityEngine;

namespace Parkour
{
    /// Câmera orbital em terceira pessoa, estilo Roblox:
    /// segure o BOTÃO DIREITO do mouse para girar, SCROLL para zoom. Tem colisão com paredes.
    public class ThirdPersonCamera : MonoBehaviour
    {
        public Transform target;
        public Vector3 pivotOffset = new Vector3(0f, 1.6f, 0f);

        [Header("Zoom (scroll)")]
        public float distance = 7f;       // distância inicial
        public float minDistance = 2f;
        public float maxDistance = 25f;
        public float zoomStep = 1f;

        [Header("Rotação")]
        [Tooltip("Sensibilidade do mouse. Pode mexer com o jogo rodando.")]
        public float sensitivity = 2f;
        public float minPitch = -30f;
        public float maxPitch = 70f;
        [Tooltip("ON = estilo Roblox (gira segurando o botão direito). OFF = gira sempre, com o mouse preso na tela.")]
        public bool rotateOnlyWithRightMouse = true;

        [Header("Colisão")]
        public float collisionRadius = 0.3f;
        public LayerMask collisionMask = ~0;

        float yaw, pitch = 20f;
        float targetDistance, zoomCurrent, currentDistance;
        bool wasRotating;
        bool cursorInit;

        void Start()
        {
            if (target) yaw = target.eulerAngles.y;
            targetDistance = zoomCurrent = currentDistance = distance;
        }

        void LateUpdate()
        {
            if (!target) return;

            // ---- cursor / modo de rotação ----
            bool rotating = !rotateOnlyWithRightMouse || GameInput.RightMouseHeld;
            bool changed = !cursorInit || rotating != wasRotating;
            if (changed)
            {
                Cursor.lockState = rotating ? CursorLockMode.Locked : CursorLockMode.None;
                Cursor.visible = !rotating;
                wasRotating = rotating;
                cursorInit = true;
            }

            if (rotating && !changed)
            {
                Vector2 look = GameInput.Look;
                yaw += look.x * sensitivity;
                pitch = Mathf.Clamp(pitch - look.y * sensitivity, minPitch, maxPitch);
            }

            // ---- zoom ----
            targetDistance = Mathf.Clamp(targetDistance - GameInput.Zoom * zoomStep, minDistance, maxDistance);
            zoomCurrent = Mathf.Lerp(zoomCurrent, targetDistance, 1f - Mathf.Exp(-12f * Time.deltaTime));

            Quaternion rot = Quaternion.Euler(pitch, yaw, 0f);
            Vector3 pivot = target.position + pivotOffset;
            Vector3 dir = rot * Vector3.back;

            // ---- não deixa a câmera atravessar paredes ----
            float wanted = zoomCurrent;
            var hits = Physics.SphereCastAll(pivot, collisionRadius, dir, zoomCurrent, collisionMask, QueryTriggerInteraction.Ignore);
            foreach (var h in hits)
            {
                if (h.distance <= 0f) continue;
                if (h.transform.IsChildOf(target)) continue;
                if (h.distance < wanted) wanted = h.distance;
            }
            wanted = Mathf.Max(wanted, 0.6f);

            if (wanted < currentDistance) currentDistance = wanted;
            else currentDistance = Mathf.Lerp(currentDistance, wanted, 1f - Mathf.Exp(-10f * Time.deltaTime));

            transform.SetPositionAndRotation(pivot + dir * currentDistance, rot);
        }
    }
}
