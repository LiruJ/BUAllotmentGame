using Assets.Scripts.Projectiles;
using Assets.Scripts.Seeds;
using System;
using UnityEngine;

namespace Assets.Scripts.Creatures
{
    [RequireComponent(typeof(TargetingBehaviour))]
    [RequireComponent(typeof(MovementBehaviour))]
    public class RangedBehaviour : CreatureBehaviour
    {
        #region Inspector Fields
        [Header("Dependencies")]
        [SerializeField]
        private Transform heldWeapon = null;

        [Header("Prefabs")]
        [SerializeField]
        private Projectile projectilePrefab = null;
        #endregion

        #region Fields
        private float timeSinceLastAttack = 0;

        private TargetingBehaviour targetingBehaviour = null;

        private MovementBehaviour movementBehaviour = null;

        private uint enemyKills = 0;

        private uint friendlyKills = 0;

        private float damageDealt = 0;
        #endregion

        #region Properties
        public float MaxPower { get; set; }

        public float AttackInterval { get; set; }
        #endregion

        #region ANN Properties
        public float PowerBias { get; set; }

        public float PowerHealthWeight { get; set; }

        public float PowerDistanceWeight { get; set; }

        public float PitchBias { get; set; }

        public float PitchSpeedWeight { get; set; }

        public float PitchDistanceWeight { get; set; }

        public float PitchPowerWeight { get; set; }

        public float YawBias { get; set; }

        public float YawAngleWeight { get; set; }

        public float YawPowerWeight { get; set; }

        public float YawSpeedWeight { get; set; }

        public float YawDistanceWeight { get; set; }

        #endregion

        #region Initialisation Functions
        private void Start()
        {
            targetingBehaviour = GetComponent<TargetingBehaviour>();
            movementBehaviour = GetComponent<MovementBehaviour>();
        }

        private void Awake()
        {

        }
        #endregion

        #region Stat Functions
        protected override void populateLifetimeStats(Seed seed)
        {
            seed.LifetimeStats.Add("EnemyKills", enemyKills);
            seed.LifetimeStats.Add("FriendlyKills", friendlyKills);
            seed.LifetimeStats.Add("DamageDealt", damageDealt);
        }
        #endregion

        #region Update Functions
        private void Update()
        {
            
        }

        private void FixedUpdate()
        {
            handleAttackLogic();
        }

        private void handleAttackLogic()
        {
            // If the creature has a target, attempt to move towards it.
            if (targetingBehaviour.HasCurrentTarget)
            {
                CreatureTarget creatureTarget = targetingBehaviour.CurrentTarget.Value;

                // Stop moving.
                movementBehaviour.StopMoving();

                // Face the target.
                transform.LookAt(creatureTarget.Transform);

                // If it has been long enough since the last attack, attack again.
                if (timeSinceLastAttack >= AttackInterval && creatureTarget.Target.IsAlive)
                {
                    // Calculate the power of the throw.
                    float power = (float)Math.Tanh(PowerBias + (creatureTarget.NormalisedHealth * PowerHealthWeight) + (creatureTarget.NormalisedDistance * PowerDistanceWeight));

                    // Only throw if the power is over 0, otherwise do nothing.
                    if (power > 0)
                    {
                        // Calculate the pitch of the throw.
                        float pitch = (float)Math.Tanh(PitchBias + (creatureTarget.NormalisedDistance * PitchDistanceWeight) + (creatureTarget.TargetRigidbody.velocity.magnitude * PitchSpeedWeight) + (power * PitchPowerWeight));

                        // Calculate the yaw of the throw.
                        float yaw = (float)Math.Tanh(YawBias + ((creatureTarget.Transform.position - transform.position).normalized.y * YawAngleWeight) + (creatureTarget.NormalisedDistance * YawDistanceWeight)
                            + (power * YawPowerWeight) + (creatureTarget.TargetRigidbody.velocity.magnitude * YawSpeedWeight));

                        // Create a new spear.
                        GameObject spearObject = Instantiate(projectilePrefab.gameObject, heldWeapon.position + (new Vector3(pitch, yaw, 0) * 0.02f), Quaternion.LookRotation(new Vector3(pitch, yaw, 0)), Creature.ProjectileContainer);

                        //spearObject.transform.position = heldWeapon.position;
                        spearObject.transform.localScale = Vector3.Scale(heldWeapon.localScale, transform.localScale);

                        // Get the projectile component from the created spear.
                        Projectile spearProjectile = spearObject.GetComponent<Projectile>();

                        spearProjectile.InitialiseProjectile(onProjectileHitEnemy);

                        // Add the force to the spear.
                        spearProjectile.Rigidbody.AddForce(spearObject.transform.forward * power * MaxPower, ForceMode.Impulse);
                    }

                    // Reset the attack timer.
                    timeSinceLastAttack = 0;
                }
            }
            // Otherwise, just move towards the goal.
            else
            {
                movementBehaviour.SetTarget(Creature.GoalObject);
                movementBehaviour.StoppingDistance = 0;
            }

            // Add the elapsed time to the attack timer.
            timeSinceLastAttack += Time.fixedDeltaTime;
        }

        private void onProjectileHitEnemy(float speed, Creature hitCreature)
        {
            if (hitCreature == null || !hitCreature.IsAlive) return;

            hitCreature.Health -= speed * 10;
            damageDealt += speed * 10;

            // If the creature is now dead, track the kill.
            if (!hitCreature.IsAlive)
            {
                if (hitCreature.Player == Creature.Player) friendlyKills++;
                else enemyKills++;
            }
        }
        #endregion
    }
}