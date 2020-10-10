using Assets.Scripts.BUCore.Maths;
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
        #endregion

        #region Fields
        private float timeSinceLastSightCheck = 0.0f;

        private Collider[] seenColliders = new Collider[40];

        private readonly List<CreatureTarget> seenEnemies = new List<CreatureTarget>();
        #endregion

        #region Properties
        public float TimeBetweenSightChecks { get; set; }

        /// <summary> How many units around its centre this creature can see. </summary>
        public float SightRange { get; set; }

        public float Bias { get; set; }

        public float HealthWeight { get; set; }

        public float DistanceWeight { get; set; }

        public CreatureTarget? CurrentTarget { get; private set; }

        public bool HasCurrentTarget => CurrentTarget.HasValue && CurrentTarget.Value.Target != null;
        #endregion

        #region Stat Functions
        protected override void statsInitialised()
        {
            // Add the seen creatures count to the lifetime stats.
            addLifetimeStat("MaxConcurrentSeenCreatures" +
                "");
        }
        #endregion

        #region Update Functions
        private void FixedUpdate()
        {
            // Clear the target if it died.
            if (CurrentTarget.HasValue && CurrentTarget.Value.Target == null) CurrentTarget = null;

            // Check for enemies in the surrounding area.
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
                            NormalisedDistance = NeuralNetworkHelper.ExpandRangeToNegative(Vector3.Distance(eyeOrigin.position, seenColliders[i].ClosestPoint(eyeOrigin.position)) / SightRange),
                            NormalisedHealth = NeuralNetworkHelper.ExpandRangeToNegative(creature.Health / maxHealth)
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
                setLifetimeStat("MaxConcurrentSeenCreatures", (uint)Math.Max(LifetimeStats["MaxConcurrentSeenCreatures"], seenEnemies.Count));

                // If enemies were seen, take the last one (the one with the highest score) and set the current target to it, otherwise set the current target to null.
                CurrentTarget = (seenEnemies.Count > 0) ? seenEnemies[seenEnemies.Count - 1] : (CreatureTarget?)null;

                if (seenCreaturesCount == seenColliders.Length) Array.Resize(ref seenColliders, seenColliders.Length + 10);

                // Reset the timer.
                timeSinceLastSightCheck = 0;
            }
        }
        #endregion
    }
}