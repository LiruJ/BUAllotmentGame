﻿using Assets.Scripts.BUCore.Maths;
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
        #endregion

        #region Lifetime Stats
        private uint enemyKills = 0;

        private uint friendlyKills = 0;

        private float enemyDamageDealt = 0;

        private float friendlyDamageDealt = 0;

        private float closestHit = 10000;

        private float bestAngle = -1;

        private uint thrownShots = 0;

        private uint collidedShots = 0;
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
            seed.LifetimeStats.Add("EnemyDamageDealt", enemyDamageDealt);
            seed.LifetimeStats.Add("FriendlyDamageDealt", friendlyDamageDealt);
            seed.LifetimeStats.Add("ClosestHit", closestHit);
            seed.LifetimeStats.Add("BestAngle", bestAngle);
            seed.LifetimeStats.Add("OffMap", thrownShots - collidedShots);
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
            // Add the elapsed time to the attack timer.
            timeSinceLastAttack += Time.fixedDeltaTime;

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
                    float power = (float)Math.Tanh(PowerBias + creatureTarget.NormalisedHealth * PowerHealthWeight + creatureTarget.NormalisedDistance * PowerDistanceWeight);

                    // Calculate the pitch of the throw.
                    float pitch = Mathf.Clamp(PitchBias + creatureTarget.NormalisedDistance * PitchDistanceWeight + NeuralNetworkHelper.ExpandRangeToNegative(creatureTarget.TargetRigidbody.velocity.magnitude / 1000) * PitchSpeedWeight
                        + power * PitchPowerWeight, -1, 1);

                    // Calcualte the normalised direction to the target, then the angles (in degrees) from that.
                    Quaternion rotationToTarget = Quaternion.LookRotation((creatureTarget.Transform.position - transform.position).normalized);

                    // Calculate the yaw of the throw.
                    float z = YawBias + creatureTarget.NormalisedDistance * YawDistanceWeight + power * YawPowerWeight 
                        + NeuralNetworkHelper.ExpandRangeToNegative(creatureTarget.TargetRigidbody.velocity.magnitude / 1000) * YawSpeedWeight;
                    float yaw = Mathf.Clamp(z, -1, 1);

                    Vector3 changedDirection = rotationToTarget.eulerAngles;
                    changedDirection.y += yaw * 180;
                    changedDirection.x += pitch * 180;

                    // Create a new spear.
                    GameObject spearObject = Instantiate(projectilePrefab.gameObject, heldWeapon.position, Quaternion.Euler(changedDirection), Creature.ProjectileContainer);

                    Debug.DrawRay(heldWeapon.position, spearObject.transform.forward, Color.red, 10);

                    //spearObject.transform.position = heldWeapon.position;
                    spearObject.transform.localScale = Vector3.Scale(heldWeapon.localScale, transform.localScale);

                    // Get the projectile component from the created spear.
                    Projectile spearProjectile = spearObject.GetComponent<Projectile>();

                    spearProjectile.InitialiseProjectile(onProjectileHitEnemy, Creature, creatureTarget.Target, creatureTarget.Transform.position, spearObject.transform.position);

                    // Add the force to the spear.
                    spearProjectile.Rigidbody.AddForce(spearObject.transform.forward * (power + 1) * MaxPower, ForceMode.Impulse);

                    // Increment the thrown shots counter.
                    thrownShots++;

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
        }

        private void onProjectileHitEnemy(ProjectileHitInfo projectileHitInfo)
        {
            // If a creature was hit, damage it
            if (projectileHitInfo.HitCreature != null && projectileHitInfo.HitCreature.IsAlive)
            {
                float finalDamage = projectileHitInfo.Speed * 10;
                projectileHitInfo.HitCreature.Health -= finalDamage;

                // Keep track of the dealt damage.
                if (projectileHitInfo.HitCreature.Player == Creature.Player) friendlyDamageDealt += finalDamage;
                else enemyDamageDealt += finalDamage;

                // If the creature is now dead, track the kill.
                if (!projectileHitInfo.HitCreature.IsAlive)
                {
                    if (projectileHitInfo.HitCreature.Player == Creature.Player) friendlyKills++;
                    else enemyKills++;
                }
            }

            // If this hit was closer than the previous hit, keep track of it.
            Vector3 targetPosition = (projectileHitInfo.TargetCreature != null) ? projectileHitInfo.TargetCreature.transform.position : projectileHitInfo.TargetPosition;
            float distanceFromTarget = Vector3.Distance(projectileHitInfo.HitPosition, targetPosition);
            if (distanceFromTarget < closestHit) closestHit = distanceFromTarget;

            // Calculate the angle between the origin of the throw and the hit location, as well as the angle between the origin and the target.
            Vector2 originHitDirection = (new Vector2(projectileHitInfo.HitPosition.x, projectileHitInfo.HitPosition.z) - new Vector2(projectileHitInfo.OriginPosition.x, projectileHitInfo.OriginPosition.z)).normalized;
            Vector2 originTargetDirection = (new Vector2(targetPosition.x, targetPosition.z) - new Vector2(projectileHitInfo.OriginPosition.x, projectileHitInfo.OriginPosition.z)).normalized;
            Debug.DrawLine(projectileHitInfo.OriginPosition, projectileHitInfo.HitPosition, Color.green, 10);
            Debug.DrawLine(projectileHitInfo.OriginPosition, targetPosition, Color.blue, 10);

            // Calculate the dot product between the desired throw and the actual throw. If this value is higher than the current best, set the current best to it.
            float dotProduct = Vector2.Dot(originHitDirection, originTargetDirection);
            if (dotProduct > bestAngle) bestAngle = dotProduct;

            // Increment the collided shots counter, as the shot hit something.
            collidedShots++;
        }
        #endregion
    }
}