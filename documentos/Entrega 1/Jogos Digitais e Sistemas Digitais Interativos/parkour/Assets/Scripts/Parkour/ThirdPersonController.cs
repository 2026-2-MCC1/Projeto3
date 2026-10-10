using UnityEngine;

namespace Parkour
{
    [RequireComponent(typeof(CharacterController))]
    public class ThirdPersonController : MonoBehaviour
    {
        [Header("Referências")]
        [Tooltip("Se vazio, usa a Main Camera.")]
        public Transform cameraTransform;

        [Header("Movimento")]
        public float walkSpeed = 7f;
        public float sprintSpeed = 7f;   // Roblox não tem corrida; aumente se quiser Shift
        public float groundAcceleration = 200f;
        public float airAcceleration = 120f;
        public float turnSmoothTime = 0.03f;

        [Header("Pulo")]
        public float jumpHeight = 2.3f;
        public float gravity = -55f;
        [Tooltip("Gravidade extra ao soltar o botão de pulo (pulo curto/longo).")]
        public float lowJumpMultiplier = 1f;   // 1 = pulo sempre completo (Roblox)
        public float coyoteTime = 0.12f;
        public float jumpBufferTime = 0.12f;
        [Tooltip("Segurar Espaço = pular de novo assim que pisar no chão (como no Roblox).")]
        public bool autoJumpWhileHeld = true;
        public float maxFallSpeed = 40f;

        public Vector3 Velocity => horizontalVel + Vector3.up * verticalVel;
        public bool IsGrounded => cc.isGrounded;

        CharacterController cc;
        Vector3 horizontalVel;
        float verticalVel;
        float turnVel;
        float coyoteCounter;
        float bufferCounter;

        void Awake()
        {
            cc = GetComponent<CharacterController>();
            if (!cameraTransform && Camera.main) cameraTransform = Camera.main.transform;
        }

        void Update()
        {
            float dt = Time.deltaTime;
            bool grounded = cc.isGrounded;

            // ---- direção relativa à câmera ----
            Vector2 input = GameInput.Move;
            Vector3 camF = Vector3.forward, camR = Vector3.right;
            if (cameraTransform)
            {
                camF = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
                camR = Vector3.ProjectOnPlane(cameraTransform.right, Vector3.up).normalized;
            }
            Vector3 wishDir = camF * input.y + camR * input.x;

            float speed = GameInput.Sprint ? sprintSpeed : walkSpeed;
            Vector3 target = wishDir * speed;
            float accel = grounded ? groundAcceleration : airAcceleration;
            horizontalVel = Vector3.MoveTowards(horizontalVel, target, accel * dt);

            // ---- vira o personagem para onde está andando ----
            if (wishDir.sqrMagnitude > 0.01f)
            {
                float targetAngle = Mathf.Atan2(wishDir.x, wishDir.z) * Mathf.Rad2Deg;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnVel, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
            }

            // ---- pulo (coyote time + buffer) ----
            coyoteCounter = grounded ? coyoteTime : coyoteCounter - dt;
            bufferCounter = (GameInput.JumpPressed || (autoJumpWhileHeld && GameInput.JumpHeld)) ? jumpBufferTime : bufferCounter - dt;

            if (grounded && verticalVel < 0f) verticalVel = -2f;

            if (bufferCounter > 0f && coyoteCounter > 0f)
            {
                verticalVel = Mathf.Sqrt(jumpHeight * -2f * gravity);
                bufferCounter = 0f;
                coyoteCounter = 0f;
            }

            float g = gravity;
            if (verticalVel > 0f && !GameInput.JumpHeld) g *= lowJumpMultiplier;
            verticalVel = Mathf.Max(verticalVel + g * dt, -maxFallSpeed);

            cc.Move((horizontalVel + Vector3.up * verticalVel) * dt);
        }

        /// Usado pelo respawn/checkpoints.
        public void Teleport(Vector3 position, Quaternion rotation)
        {
            cc.enabled = false;
            transform.SetPositionAndRotation(position, rotation);
            cc.enabled = true;
            horizontalVel = Vector3.zero;
            verticalVel = 0f;
        }
    }
}
