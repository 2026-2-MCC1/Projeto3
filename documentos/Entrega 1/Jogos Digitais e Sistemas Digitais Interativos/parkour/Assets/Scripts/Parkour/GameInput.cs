using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace Parkour
{
    /// Leitura de input que funciona com o Input System novo E com o Input Manager antigo.
    public static class GameInput
    {
        public static Vector2 Move
        {
            get
            {
#if ENABLE_INPUT_SYSTEM
                Vector2 v = Vector2.zero;
                var k = Keyboard.current;
                if (k != null)
                {
                    if (k.wKey.isPressed || k.upArrowKey.isPressed) v.y += 1f;
                    if (k.sKey.isPressed || k.downArrowKey.isPressed) v.y -= 1f;
                    if (k.dKey.isPressed || k.rightArrowKey.isPressed) v.x += 1f;
                    if (k.aKey.isPressed || k.leftArrowKey.isPressed) v.x -= 1f;
                }
                var g = Gamepad.current;
                if (g != null)
                {
                    Vector2 s = g.leftStick.ReadValue();
                    if (s.sqrMagnitude > v.sqrMagnitude) v = s;
                }
                return Vector2.ClampMagnitude(v, 1f);
#else
                return Vector2.ClampMagnitude(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")), 1f);
#endif
            }
        }

        /// Giro da câmera (já em "graus por frame").
        public static Vector2 Look
        {
            get
            {
#if ENABLE_INPUT_SYSTEM
                Vector2 v = Vector2.zero;
                if (Mouse.current != null) v += Mouse.current.delta.ReadValue() * 0.15f;
                if (Gamepad.current != null) v += Gamepad.current.rightStick.ReadValue() * 150f * Time.deltaTime;
                return v;
#else
                return new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * 2f;
#endif
            }
        }

        public static bool RightMouseHeld
        {
            get
            {
#if ENABLE_INPUT_SYSTEM
                return Mouse.current != null && Mouse.current.rightButton.isPressed;
#else
                return Input.GetMouseButton(1);
#endif
            }
        }

        /// -1, 0 ou +1 (scroll do mouse)
        public static float Zoom
        {
            get
            {
#if ENABLE_INPUT_SYSTEM
                if (Mouse.current == null) return 0f;
                float y = Mouse.current.scroll.ReadValue().y;
#else
                float y = Input.mouseScrollDelta.y;
#endif
                return Mathf.Abs(y) > 0.01f ? Mathf.Sign(y) : 0f;
            }
        }

        public static bool JumpPressed
        {
            get
            {
#if ENABLE_INPUT_SYSTEM
                return (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
                    || (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame);
#else
                return Input.GetButtonDown("Jump");
#endif
            }
        }

        public static bool JumpHeld
        {
            get
            {
#if ENABLE_INPUT_SYSTEM
                return (Keyboard.current != null && Keyboard.current.spaceKey.isPressed)
                    || (Gamepad.current != null && Gamepad.current.buttonSouth.isPressed);
#else
                return Input.GetButton("Jump");
#endif
            }
        }

        public static bool Sprint
        {
            get
            {
#if ENABLE_INPUT_SYSTEM
                return (Keyboard.current != null && Keyboard.current.leftShiftKey.isPressed)
                    || (Gamepad.current != null && Gamepad.current.rightShoulder.isPressed);
#else
                return Input.GetKey(KeyCode.LeftShift);
#endif
            }
        }

        public static bool RespawnPressed
        {
            get
            {
#if ENABLE_INPUT_SYSTEM
                return (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
                    || (Gamepad.current != null && Gamepad.current.buttonNorth.wasPressedThisFrame);
#else
                return Input.GetKeyDown(KeyCode.R);
#endif
            }
        }
    }
}
