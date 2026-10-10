using UnityEngine;

namespace Parkour
{
    [RequireComponent(typeof(ThirdPersonController))]
    public class PlayerRespawn : MonoBehaviour
    {
        [Tooltip("Se cair mais que isso abaixo do checkpoint, volta pro checkpoint.")]
        public float killDepth = 15f;

        ThirdPersonController ctrl;
        Vector3 point;
        Quaternion rot;

        void Awake()
        {
            ctrl = GetComponent<ThirdPersonController>();
            point = transform.position;   // posição inicial = primeiro checkpoint
            rot = transform.rotation;
        }

        public void SetCheckpoint(Transform t)
        {
            point = t.position;
            rot = t.rotation;
        }

        void Update()
        {
            if (GameInput.RespawnPressed || transform.position.y < point.y - killDepth)
                Respawn();
        }

        public void Respawn() => ctrl.Teleport(point, rot);
    }
}
