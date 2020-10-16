using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.BUCore.Control
{
    public class CameraFreeController : MonoBehaviour
    {
        #region Inspector Fields
        [Header("Bindings")]
        [Tooltip("The key to move forwards.")]
        [SerializeField]
        private KeyCode forwardKey = KeyCode.W;

        [Tooltip("The key to move backwards.")]
        [SerializeField]
        private KeyCode backwardKey = KeyCode.S;

        [Tooltip("The key to move left.")]
        [SerializeField]
        private KeyCode leftKey = KeyCode.A;

        [Tooltip("The key to move right.")]
        [SerializeField]
        private KeyCode rightKey = KeyCode.D;

        [Tooltip("The key to move upwards.")]
        [SerializeField]
        private KeyCode upKey = KeyCode.Space;

        [Tooltip("The key to move downwards.")]
        [SerializeField]
        private KeyCode downKey = KeyCode.LeftControl;

        [Tooltip("The key to speed up.")]
        [SerializeField]
        private KeyCode speedUpKey = KeyCode.LeftShift;

        [Tooltip("The mouse button to drag the camera.")]
        [SerializeField]
        private MouseButton dragButton = MouseButton.LeftMouse;

        [Header("Settings")]
        [Tooltip("True if the camera should still be able to move while the game is paused. Also means that time scale will not change the speed of the camera.")]
        [SerializeField]
        private bool useScaledTime = true;

        [Range(0, 100)]
        [Tooltip("The amount of units per second to move in each direction.")]
        [SerializeField]
        private float moveSpeed = 1.0f;

        [Range(0, 10)]
        [Tooltip("The multiplier applied to the speed when the speedup button is held.")]
        [SerializeField]
        private float speedUpMultiplier = 2.0f;

        [Range(0, 10)]
        [Tooltip("The multiplier applied to the rotational speed.")]
        [SerializeField]
        private float rotationMultiplier = 1.0f;
        #endregion

        #region Fields
        private Vector3 lastMousePosition = Vector3.zero;
        #endregion

        #region Update Functions
        private void Update()
        {
            if (Input.GetMouseButton((int)dragButton) && (!useScaledTime || UnityEngine.Time.timeScale > 0))
            {
                // Calculate the distance between the mouse on the last frame and this frame.
                Vector3 mouseDelta = Input.mousePosition - lastMousePosition;

                // Apply the rotation to the rotation of the camera. This does not need to be scaled by the delta time since it's using mouse movements rather than digital key presses.
                transform.eulerAngles += new Vector3(-mouseDelta.y, mouseDelta.x, 0) * rotationMultiplier;
            }

            // The distance in each axis to move.
            Vector3 deltaPosition = Vector3.zero;

            // Forwards/Backwards.
            if (Input.GetKey(forwardKey)) deltaPosition += transform.forward * moveSpeed;
            else if (Input.GetKey(backwardKey)) deltaPosition -= transform.forward * moveSpeed;

            // Left/Right.
            if (Input.GetKey(leftKey)) deltaPosition -= transform.right * moveSpeed;
            else if (Input.GetKey(rightKey)) deltaPosition += transform.right * moveSpeed;

            // Up/Down.
            if (Input.GetKey(upKey)) deltaPosition += transform.up * moveSpeed;
            else if (Input.GetKey(downKey)) deltaPosition -= transform.up * moveSpeed;

            // If the speedup key is being held, apply the speedup amount to the delta position.
            if (Input.GetKey(speedUpKey)) deltaPosition *= speedUpMultiplier;

            // Apply the delta position multipled by the delta time.
            transform.position += deltaPosition * (useScaledTime ? UnityEngine.Time.deltaTime :  UnityEngine.Time.unscaledDeltaTime);

            // Save the last previous mouse position.
            lastMousePosition = Input.mousePosition;
        }
        #endregion
    }
}