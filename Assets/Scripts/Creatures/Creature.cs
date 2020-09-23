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

        /// <summary> The creature's behaviours keyed by name (without "behaviour" on the end). </summary>
        private readonly Dictionary<string, CreatureBehaviour> creatureBehaviours = new Dictionary<string, CreatureBehaviour>();

        private readonly Dictionary<string, float> lifetimeStats = new Dictionary<string, float>();
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

        public float MaxHealth => healthStat.Value;

        /// <summary> Is true if the creature is alive; false otherwise. </summary>
        public bool IsAlive => Health > 0;

        public IReadOnlyDictionary<string, CreatureBehaviour> CreatureBehaviours => creatureBehaviours;

        /// <summary> The generic lifetime stats of the creature. </summary>
        public IReadOnlyDictionary<string, float> LifetimeStats => lifetimeStats;

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

        public void InitialiseFirstTime(Transform projectileContainer) => ProjectileContainer = projectileContainer;
        #endregion

        #region Stat Functions
        /// <summary> Finds and stores every behaviour of the creature. </summary>
        private void initialiseBehaviours()
        {
            foreach (CreatureBehaviour creatureBehaviour in GetComponents<CreatureBehaviour>())
            {
                // Initialise the behaviour.
                creatureBehaviour.InitialiseFirstTime();
                
                // Get the name of the behaviour without "Behaviour" on the end.
                string behaviourName = creatureBehaviour.GetType().Name;
                behaviourName = behaviourName.Remove(behaviourName.IndexOf("Behaviour"));

                // Add the behaviour keyed by the name.
                creatureBehaviours.Add(behaviourName, creatureBehaviour);
            }
        }

        /// <summary> Initialises the creature using the given <paramref name="seed"/>. </summary>
        /// <param name="creatureManager"> The manager that spawned this creature. </param>
        /// <param name="seed"> The seed from which this creature was made. </param>
        /// <param name="goalObject"> The object that the creature wishes to reach. </param>
        public void InitialiseFromSeed(CreatureManager creatureManager, Seed seed, Transform goalObject)
        {
            // Set fields and properties.
            this.creatureManager = creatureManager;
            Seed = seed;
            GoalObject = goalObject;

            // Clear the lifetime stats collection and add the generic stats.
            lifetimeStats.Clear();
            lifetimeStats.Add("AliveTime", 0);

            // Add the behaviours.
            if (creatureBehaviours?.Count == 0) initialiseBehaviours();

            // Initialise each behaviour.
            foreach (CreatureBehaviour creatureBehaviour in creatureBehaviours.Values)
                creatureBehaviour.InitialiseFromStats(seed.GeneticStats);

            // Initialise each generic genetic stat.
            healthStat.InitialiseFromStats(seed.GeneticStats);
        }
        #endregion

        #region Seed Dropping Functions
        /// <summary> Creates and returns a seed with this creature's stats. </summary>
        /// <returns> The created seed. </returns>
        public Seed DropSeed()
        {
            // Create a new dictionary to hold the lifetime stats of every behaviour.
            Dictionary<string, float> fullLifetimeStats = new Dictionary<string, float>();
            foreach (KeyValuePair<string, float> statNameValue in LifetimeStats)
                fullLifetimeStats.Add(statNameValue.Key, statNameValue.Value);

            // Create a new seed using the stats from this creature's seed.
            Seed droppedSeed = new Seed(Seed.Generation + 1, Seed.CropTileName, fullLifetimeStats);

            // Populate with the generic genetic stats.
            healthStat.PopulateSeed(droppedSeed);

            // Give each behaviour a chance to set the genetic and lifetime stats of the dropped seed.
            foreach (CreatureBehaviour creatureBehaviour in creatureBehaviours.Values)
            {
                creatureBehaviour.PopulateSeed(droppedSeed);
                foreach (KeyValuePair<string, float> statNameValue in creatureBehaviour.LifetimeStats)
                    fullLifetimeStats.Add(statNameValue.Key, statNameValue.Value);
            }

            // Return the dropped seed.
            return droppedSeed;
        }
        #endregion

        #region Update Functions
        private void FixedUpdate()
        {
            if (IsAlive) lifetimeStats["AliveTime"] += Time.fixedDeltaTime;
        }
        #endregion
    }
}