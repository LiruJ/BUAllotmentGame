using Assets.Scripts.Seeds;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Creatures
{
    public class TargetingBehaviour : CreatureBehaviour
    {
        #region Inspector Fields
        [Header("Settings")]
        [Tooltip("The origin from which the sight is checked.")]
        [SerializeField]
        private Transform eyeOrigin = null;

        [Range(100, 10000)]
        [Tooltip("The max health, used for normalising.")]
        private float maxHealth = 10000;

        [Range(0.1f, 10)]
        [Tooltip("The amount of time in seconds between each sight update.")]
        [SerializeField]
        private float timeBetweenSightChecks;
        #endregion

        #region Fields
        private uint maxConcurrentSeenCreatures = 0;

        private float timeSinceLastSightCheck = 0.0f;

        private readonly Collider[] seenColliders = new Collider[25];

        private readonly List<CreatureTarget> seenEnemies = new List<CreatureTarget>();
        #endregion

        #region Properties
        public float TimeBetweenSightChecks { get => timeBetweenSightChecks; set => timeBetweenSightChecks = value; }

        /// <summary> How many units around its centre this creature can see. </summary>
        public float SightRange { get; set; }

        public float Bias { get; set; }

        public float HealthWeight { get; set; }

        public float DistanceWeight { get; set; }

        public CreatureTarget? CurrentTarget { get; private set; }

        public bool HasCurrentTarget => CurrentTarget.HasValue && CurrentTarget.Value.Target != null;
        #endregion

        #region Stat Functions
        protected override void populateLifetimeStats(Seed seed)
        {
            seed.LifetimeStats.Add("MaxConcurrentSeenCreatures", maxConcurrentSeenCreatures);
        }
        #endregion

        #region Update Functions
        private void FixedUpdate()
        {
            if (CurrentTarget.HasValue && CurrentTarget.Value.Target == null) CurrentTarget = null;

            checkEnemies();
        }

        private void checkEnemies()
        {
            // If the sight range is negative, do nothing.
            if (SightRange <= 0) return;

            // Add the passed time to the time since the last check.
            timeSinceLastSightCheck += Time.fixedDeltaTime;

            if (timeSinceLastSightCheck >= TimeBetweenSightChecks)
            {
                // Finds all creature colliders in the area around this creature.
                int seenCreaturesCount = Physics.OverlapSphereNonAlloc(eyeOrigin.position, SightRange, seenColliders, 1 << LayerMask.NameToLayer("Creatures"));

                // Clear the seen enemies list and fill it with all enemies in the colliders collection.
                seenEnemies.Clear();
                for (int i = 0; i < seenCreaturesCount; i++)
                    if (seenColliders[i] != null && seenColliders[i].TryGetComponent(out Creature creature) && creature.Player != Creature.Player)
                        seenEnemies.Add(new CreatureTarget()
                        {
                            Target = creature,
                            TargetRigidbody = creature.GetComponent<Rigidbody>(),
                            TargetCollider = seenColliders[i],
                            NormalisedDistance = Vector3.Distance(eyeOrigin.position, seenColliders[i].ClosestPoint(eyeOrigin.position)) / SightRange,
                            NormalisedHealth = creature.Health / maxHealth
                        });

                // Sort the enemies based on the score using the mutated filters.
                seenEnemies.Sort((firstEnemy, secondEnemy) =>
                {
                    // Calculate the scores for both enemies.
                    float firstScore = (float)Math.Tanh(Bias + firstEnemy.NormalisedHealth * HealthWeight + firstEnemy.NormalisedDistance * DistanceWeight);
                    float secondScore = (float)Math.Tanh(Bias + secondEnemy.NormalisedHealth * HealthWeight + secondEnemy.NormalisedDistance * DistanceWeight);

                    // Return the result of the compare function.
                    return firstScore.CompareTo(secondScore);
                });

                // Set the max concurrent seen creatures if the number of seen creatures is higher than the old value.
                maxConcurrentSeenCreatures = (uint)Math.Max(maxConcurrentSeenCreatures, seenEnemies.Count);

                // If enemies were seen, take the last one (the one with the highest score) and set the current target to it, otherwise set the current target to null.
                CurrentTarget = (seenEnemies.Count > 0) ? seenEnemies[seenEnemies.Count - 1] : (CreatureTarget?)null;

                // Reset the timer.
                timeSinceLastSightCheck = 0;
            }
        }
        #endregion
    }
}