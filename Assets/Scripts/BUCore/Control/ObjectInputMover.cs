using UnityEngine;

namespace Assets.Scripts.BUCore.Control
{
    /// <summary> Allows an object to be moved around the scene using the Unity input system. </summary>
    public class ObjectInputMover : MonoBehaviour
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
        #endregion

        #region Update Functions
        private void Update()
        {
            // The distance in each axis to move.
            Vector3 deltaPosition = Vector3.zero;

            // Forwards/Backwards.
            if (Input.GetKey(forwardKey)) deltaPosition.z += moveSpeed;
            else if (Input.GetKey(backwardKey)) deltaPosition.z -= moveSpeed;

            // Left/Right.
            if (Input.GetKey(leftKey)) deltaPosition.x -= moveSpeed;
            else if (Input.GetKey(rightKey)) deltaPosition.x += moveSpeed;

            // Up/Down.
            if (Input.GetKey(upKey)) deltaPosition.y += moveSpeed;
            else if (Input.GetKey(downKey)) deltaPosition.y -= moveSpeed;

            // If the speedup key is being held, apply the speedup amount to the delta position.
            if (Input.GetKey(speedUpKey)) deltaPosition *= speedUpMultiplier;

            // Apply the delta position multipled by the delta time.
            transform.position += (useScaledTime) ? deltaPosition * UnityEngine.Time.deltaTime : deltaPosition * UnityEngine.Time.unscaledDeltaTime;
        }
        #endregion
    }
}