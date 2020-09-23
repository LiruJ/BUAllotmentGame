using Assets.Scripts.BUCore.Maths;
using Assets.Scripts.Projectiles;
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

        private uint thrownShots = 0;

        private uint collidedShots = 0;
        #endregion

        #region Properties
        public float MaxPower { get; set; }

        public float AttackInterval { get; set; }

        public float PitchDeviation { get; set; }

        public float YawDeviation { get; set; }
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
        #endregion

        #region Stat Functions
        protected override void statsInitialised()
        {
            addLifetimeStat("EnemyKills");
            addLifetimeStat("FriendlyKills");
            addLifetimeStat("EnemyDamageDealt");
            addLifetimeStat("FriendlyDamageDealt");
            addLifetimeStat("ClosestHit", 10000);
            addLifetimeStat("BestAngle", -1);
            addLifetimeStat("OffMap");
        }
        #endregion

        #region Update Functions
        private void FixedUpdate()
        {
            handleAttackLogic();

            setLifetimeStat("Offmap", thrownShots - collidedShots);
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
                    float power = Mathf.Clamp01(PowerBias + creatureTarget.NormalisedHealth * PowerHealthWeight + creatureTarget.NormalisedDistance * PowerDistanceWeight);

                    // Calculate the pitch of the throw.
                    float pitch = Mathf.Clamp(PitchBias + creatureTarget.NormalisedDistance * PitchDistanceWeight + NeuralNetworkHelper.ExpandRangeToNegative(creatureTarget.TargetRigidbody.velocity.magnitude / 1000) * PitchSpeedWeight
                        + power * PitchPowerWeight, -1, 1);

                    // Calculate the yaw of the throw.
                    float yaw = Mathf.Clamp(YawBias + creatureTarget.NormalisedDistance * YawDistanceWeight + power * YawPowerWeight
                        + NeuralNetworkHelper.ExpandRangeToNegative(creatureTarget.TargetRigidbody.velocity.magnitude / 1000) * YawSpeedWeight, -1, 1);

                    // Calcualte the normalised direction to the target, then the angles (in degrees) from that.
                    Vector3 direction = Quaternion.LookRotation((creatureTarget.Transform.position - transform.position).normalized).eulerAngles;
                    direction.y += yaw * Mathf.Clamp(YawDeviation, 0, 180);
                    direction.x += pitch * Mathf.Clamp(PitchDeviation, 0, 180);

                    // Create a new spear.
                    GameObject spearObject = Instantiate(projectilePrefab.gameObject, heldWeapon.position, Quaternion.Euler(direction), Creature.ProjectileContainer);

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
                // Calculate the damage dealt based on the speed of the projectile.
                float finalDamage = projectileHitInfo.Speed * 2;

                // Remove the damage from the hit creature's health.
                projectileHitInfo.HitCreature.Health -= finalDamage;

                // Keep track of the dealt damage.
                if (projectileHitInfo.HitCreature.Player == Creature.Player) changeLifetimeStat("FriendlyDamageDealt", finalDamage);
                else changeLifetimeStat("EnemyDamageDealt", finalDamage);

                // If the creature is now dead, track the kill.
                if (!projectileHitInfo.HitCreature.IsAlive)
                {
                    if (projectileHitInfo.HitCreature.Player == Creature.Player) changeLifetimeStat("FriendlyKills", 1);
                    else changeLifetimeStat("EnemyKills", 1);
                }
            }

            // If this hit was closer than the previous hit, keep track of it.
            Vector3 targetPosition = (projectileHitInfo.TargetCreature != null) ? projectileHitInfo.TargetCreature.transform.position : projectileHitInfo.TargetPosition;
            float distanceFromTarget = Vector3.Distance(projectileHitInfo.HitPosition, targetPosition);
            if (distanceFromTarget < LifetimeStats["ClosestHit"]) setLifetimeStat("ClosestHit", distanceFromTarget);

            // Calculate the angle between the origin of the throw and the hit location, as well as the angle between the origin and the target.
            Vector2 originHitDirection = (new Vector2(projectileHitInfo.HitPosition.x, projectileHitInfo.HitPosition.z) - new Vector2(projectileHitInfo.OriginPosition.x, projectileHitInfo.OriginPosition.z)).normalized;
            Vector2 originTargetDirection = (new Vector2(targetPosition.x, targetPosition.z) - new Vector2(projectileHitInfo.OriginPosition.x, projectileHitInfo.OriginPosition.z)).normalized;
            Debug.DrawLine(projectileHitInfo.OriginPosition, projectileHitInfo.HitPosition, Color.green, 10);
            Debug.DrawLine(projectileHitInfo.OriginPosition, targetPosition, Color.blue, 10);

            // Calculate the dot product between the desired throw and the actual throw. If this value is higher than the current best, set the current best to it.
            float dotProduct = Vector2.Dot(originHitDirection, originTargetDirection);
            if (dotProduct > LifetimeStats["BestAngle"]) setLifetimeStat("BestAngle", dotProduct);

            // Increment the collided shots counter, as the shot hit something.
            collidedShots++;
        }
        #endregion
    }
}