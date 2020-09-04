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
        private readonly Collider[] seenColliders = new Collider[10];

        private readonly List<Tuple<Creature, Collider>> seenEnemies = new List<Tuple<Creature, Collider>>();
        #endregion

        #region Properties
        public float TimeBetweenSightChecks { get => timeBetweenSightChecks; set => timeBetweenSightChecks = value; }

        /// <summary> How many units around its centre this creature can see. </summary>
        public float SightRange { get; set; }

        public float Bias { get; set; }

        public float HealthWeight { get; set; }

        public float DistanceWeight { get; set; }

        public Creature CurrentTarget { get; private set; }
        #endregion

        #region Stat Functions
        protected override void populateLifetimeStats(Seed seed)
        {
            seed.LifetimeStats.Add("MaxConcurrentSeenCreatures", maxConcurrentSeenCreatures);
        }
        #endregion

        #region Update Functions
        private void FixedUpdate() => checkEnemies();

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
                        seenEnemies.Add(new Tuple<Creature, Collider>(creature, seenColliders[i]));

                // Sort the enemies based on the score using the mutated filters.
                seenEnemies.Sort((firstEnemy, secondEnemy) =>
                {
                    // Start by adding the health
                    float firstScore = (firstEnemy.Item1.Health / maxHealth) * HealthWeight;
                    float secondScore = (secondEnemy.Item1.Health / maxHealth) * HealthWeight;

                    // Add the normalised distance score to the enemy scores.
                    firstScore += (Vector3.Distance(eyeOrigin.position, firstEnemy.Item2.ClosestPoint(eyeOrigin.position)) / SightRange) * DistanceWeight;
                    secondScore += (Vector3.Distance(eyeOrigin.position, secondEnemy.Item2.ClosestPoint(eyeOrigin.position)) / SightRange) * DistanceWeight;

                    // Normalise the scores using an activation function.
                    firstScore = (float)Math.Tanh(Bias + firstScore);
                    secondScore = (float)Math.Tanh(Bias + secondScore);

                    // Return the result of the compare function.
                    return firstScore.CompareTo(secondScore);
                });

                // Set the max concurrent seen creatures if the number of seen creatures is higher than the old value.
                maxConcurrentSeenCreatures = (uint)Math.Max(maxConcurrentSeenCreatures, seenEnemies.Count);

                // If enemies were seen, take the last one (the one with the highest score) and set the current target to it, otherwise set the current target to null.
                CurrentTarget = (seenEnemies.Count > 0) ? seenEnemies[seenEnemies.Count - 1].Item1 : null;

                // Reset the timer.
                timeSinceLastSightCheck = 0;
            }
        }
        #endregion
    }
}