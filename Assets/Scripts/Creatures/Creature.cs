using Assets.Scripts.Player;
using Assets.Scripts.Seeds;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Creatures
{
    /// <summary> The main creature controller. </summary>
    [DisallowMultipleComponent]
    public class Creature : MonoBehaviour
    {
        #region Inspector Fields
        [Header("Mutation Stats")]
        [Tooltip("How much health this creature has.")]
        [SerializeField]
        private CreatureStat healthStat = new CreatureStat();

        [SerializeField]
        private Transform eyeOrigin = null;
        #endregion

        #region Fields
        /// <summary> The creature's health. </summary>
        private float health = 1;

        /// <summary> The manager that spawned this creature. </summary>
        private CreatureManager creatureManager = null;

        /// <summary> How long in seconds the creature has been alive. </summary>
        private float aliveTime = 0;

        /// <summary> The creature's behaviours. </summary>
        private readonly List<CreatureBehaviour> creatureBehaviours = new List<CreatureBehaviour>();
        #endregion

        #region Properties
        /// <summary> The seed of the plant that spawned this creature. </summary>
        public Seed Seed { get; private set; }

        public Transform EyeOrigin => eyeOrigin;

        public Rigidbody Rigidbody { get; private set; }

        /// <summary> The current health of this creature. </summary>
        public float Health
        {
            get => health;
            set
            {
                // If the creature is dead, don't change the health.
                if (!IsAlive) return;

                // Change the health.
                health = value;

                // If the health has dropped to 0 or lower, the creature is dead.
                if (health <= 0) creatureManager.CreatureDied(this);
            }
        }

        /// <summary> Is true if the creature is alive; false otherwise. </summary>
        public bool IsAlive => Health > 0;

        /// <summary> The object that the creature is trying to reach. </summary>
        public Transform GoalObject { get; private set; }

        /// <summary> The object into which the projectiles are instantiated. </summary>
        public Transform ProjectileContainer { get; private set; }

        /// <summary> The player who owns this creature. </summary>
        public BasePlayer Player => creatureManager.Player;
        #endregion

        #region Initialisation Functions
        private void Start()
        {
            Rigidbody = GetComponent<Rigidbody>();
        }
        #endregion

        #region Stat Functions
        /// <summary> Finds and stores every behaviour of the creature. </summary>
        private void findBehaviours() => creatureBehaviours.AddRange(GetComponents<CreatureBehaviour>());

        /// <summary> Initialises the creature using the given <paramref name="seed"/>. </summary>
        /// <param name="creatureManager"> The manager that spawned this creature. </param>
        /// <param name="seed"> The seed from which this creature was made. </param>
        /// <param name="goalObject"> The object that the creature wishes to reach. </param>
        public void InitialiseFromStats(CreatureManager creatureManager, Seed seed, Transform goalObject, Transform projectileContainer)
        {
            // Set fields and properties.
            this.creatureManager = creatureManager;
            Seed = seed;
            GoalObject = goalObject;
            ProjectileContainer = projectileContainer;

            // Add the behaviours.
            findBehaviours();

            // Initialise each behaviour.
            foreach (CreatureBehaviour creatureBehaviour in creatureBehaviours)
                creatureBehaviour.InitialiseFromStats(this, seed.GeneticStats);

            // Initialise each generic genetic stat.
            healthStat.InitialiseFromStats(seed.GeneticStats);
        }
        #endregion

        #region Seed Dropping Functions
        /// <summary> Creates and returns a seed with this creature's stats. </summary>
        /// <returns> The created seed. </returns>
        public Seed DropSeed()
        {
            // Create a new seed using the stats from this creature's seed.
            Seed droppedSeed = new Seed(Seed.Generation + 1, Seed.CropTileName);

            // Populate with the generic genetic stats.
            healthStat.PopulateSeed(droppedSeed);

            // Give each behaviour a chance to set the genetic and lifetime stats of the dropped seed.
            foreach (CreatureBehaviour creatureBehaviour in creatureBehaviours)
                creatureBehaviour.PopulateSeed(droppedSeed);

            // Calculate the generic lifetime stats of this creature.
            populateLifetimeStats(droppedSeed);

            // Return the dropped seed.
            return droppedSeed;
        }

        /// <summary> Adds all generic stats to the given <paramref name="droppedSeed"/>. </summary>
        /// <param name="droppedSeed"> The seed to fill with stats. </param>
        private void populateLifetimeStats(Seed droppedSeed)
        {
            // Add each generic lifetime stat to the seed.
            droppedSeed.LifetimeStats.Add("AliveTime", aliveTime);
        }
        #endregion

        #region Update Functions
        private void FixedUpdate()
        {
            if (IsAlive) aliveTime += Time.fixedDeltaTime;
        }
        #endregion
    }
}