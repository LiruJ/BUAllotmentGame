using Assets.Scripts.Creatures;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Projectiles
{
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour
    {
        #region Inspector Fields
        [Header("Settings")]
        [SerializeField]
        private float destroyLevel = -100;

        [Header("Dependencies")]
        [SerializeField]
        private Rigidbody projectileRigidbody = null;

        [SerializeField]
        private Transform tip = null;
        #endregion

        #region Fields
        private Action<float, Creature> callback;
        #endregion

        #region Properties
        public Rigidbody Rigidbody => projectileRigidbody;
        #endregion

        #region Events
        [SerializeField]
        private UnityEvent onHitCreature = new UnityEvent();

        [SerializeField]
        private UnityEvent onHitTerrain = new UnityEvent();
        #endregion

        #region Initialisation Functions
        public void InitialiseProjectile(Action<float, Creature> callback)
        {
            this.callback = callback;
        }
        #endregion

        #region Update Functions
        private void Update()
        {
            if (Rigidbody.velocity.magnitude != 0) transform.rotation = Quaternion.LookRotation(Rigidbody.velocity.normalized);
        }

        private void FixedUpdate()
        {
            if (transform.position.y < destroyLevel) Destroy(gameObject);

            float rayLength = Rigidbody.velocity.magnitude * Time.fixedDeltaTime;

            Debug.DrawLine(tip.position, tip.position + (tip.forward * rayLength), Color.red, 10);
            if (Physics.Raycast(new Ray(tip.position, tip.forward), out RaycastHit hitInfo, rayLength, LayerMask.GetMask("Terrain", "Creatures")))
            {
                // Invoke the callback event.
                callback(projectileRigidbody.velocity.magnitude, hitInfo.collider.gameObject.GetComponent<Creature>());

                // Move the spear to the hit position.
                transform.position = hitInfo.point;

                // If the hit item has a rigidbody, attach to it using a fixed joint.
                if (hitInfo.rigidbody != null)
                {
                    FixedJoint joint = gameObject.AddComponent<FixedJoint>();
                    joint.connectedBody = hitInfo.rigidbody;

                    // Invoke the hit event.
                    onHitCreature.Invoke();
                }
                // Otherwise, freeze the position.
                else
                {
                    // Invoke the hit event.
                    onHitTerrain.Invoke();

                    projectileRigidbody.constraints = RigidbodyConstraints.FreezePosition;
                }

                // Kill the speed and stop being affected by gravity.
                projectileRigidbody.velocity = Vector3.zero;
                projectileRigidbody.useGravity = false;



                // Disable this script.
                enabled = false;

                // Destroy the projectile after a set amount of time.
                Destroy(gameObject, 4);
            }
        }
        #endregion
    }
}