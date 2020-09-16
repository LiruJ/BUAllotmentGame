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
        private Action<ProjectileHitInfo> callback;

        private Creature owner;

        private Vector3 originPosition;

        private Creature targetCreature;

        private Vector3 targetPosition;
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
        public void InitialiseProjectile(Action<ProjectileHitInfo> callback, Creature owner, Creature targetCreature, Vector3 targetPosition, Vector3 originPosition)
        {
            this.callback = callback;
            this.owner = owner;
            this.originPosition = originPosition;
            this.targetCreature = targetCreature;
            this.targetPosition = targetPosition;
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

            if (Physics.Raycast(new Ray(tip.position, Rigidbody.velocity.normalized), out RaycastHit hitInfo, Rigidbody.velocity.magnitude * Time.fixedDeltaTime * 2, LayerMask.GetMask("Terrain", "Creatures")))
            {
                // If the hit object is a creature, check to see if it's the creature who threw the projectile. If it is, do nothing.
                if (hitInfo.collider.TryGetComponent(out Creature hitCreature) && hitCreature == owner) return;

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

                // Keep track of the speed of the projectile before it gets stuck.
                float speed = projectileRigidbody.velocity.magnitude;

                // Kill the speed and stop being affected by gravity.
                projectileRigidbody.velocity = Vector3.zero;
                projectileRigidbody.useGravity = false;

                // Create the hit info.
                ProjectileHitInfo projectileHitInfo = new ProjectileHitInfo()
                {
                    Speed = speed,
                    OriginPosition = originPosition,
                    TargetPosition = targetPosition,
                    HitPosition = hitInfo.point,
                    TargetCreature = targetCreature,
                    HitCreature = hitCreature,
                    HitCollder = hitInfo.collider
                };

                // Invoke the callback event.
                callback(projectileHitInfo);

                // Disable this script.
                enabled = false;

                // Destroy the projectile after a set amount of time.
                Destroy(gameObject, 4);
            }
        }
        #endregion
    }
}