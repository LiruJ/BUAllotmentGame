using Assets.Scripts.Seeds;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Creatures
{
    [RequireComponent(typeof(Creature))]
    public abstract class CreatureBehaviour : MonoBehaviour
    {
        #region Inspector Fields
        [Header("Mutation Stats")]
        [Tooltip("The stats of this behaviour.")]
        [SerializeField]
        private List<CreatureStat> creatureStats = new List<CreatureStat>();
        #endregion

        #region Fields

        #endregion

        #region Properties
        /// <summary> The collection of mutatable stats. </summary>
        public IReadOnlyList<CreatureStat> CreatureStats => creatureStats;

        /// <summary> The <see cref="Creature"/> who owns this behaviour. </summary>
        public Creature Creature { get; private set; }
        #endregion

        #region Initialisation Functions
        private void Start()
        {

        }

        private void Awake()
        {

        }
        #endregion

        #region Stat Functions
        /// <summary> Adds each stat to the given <paramref name="seed"/>. </summary>
        /// <param name="seed"> The seet to fill with stats. </param>
        public void PopulateSeed(Seed seed)
        {
            // Add each stat to the seed.
            foreach (CreatureStat creatureStat in creatureStats)
                creatureStat.PopulateSeed(seed);

            // Add each lifetime stat to the seed.
            populateLifetimeStats(seed);
        }

        /// <summary> Adds the lifetime stats from this behaviour to the given <paramref name="seed"/>. </summary>
        /// <param name="seed"> The seed to populate. </param>
        protected virtual void populateLifetimeStats(Seed seed) { }

        /// <summary> Sets the stats of this behaviour from the given <paramref name="stats"/> dictionary. </summary>
        /// <param name="creature"> The base creature. </param>
        /// <param name="stats"> The stats of the creature. </param>
        public void InitialiseFromStats(Creature creature, IReadOnlyDictionary<string, float> stats)
        {
            // Set the base creature.
            Creature = creature;

            // Initialise each stat.
            foreach (CreatureStat creatureStat in creatureStats)
                creatureStat.InitialiseFromStats(stats);

            // Call the stats initialised function for any deriving class.
            statsInitialised();
        }

        /// <summary> Is called immediately after <see cref="InitialiseFromStats(Creature, IReadOnlyDictionary{string, float})"/>, meaning that every stat should be initialised. </summary>
        protected virtual void statsInitialised() { }
        #endregion

        #region Update Functions
        private void Update()
        {

        }

        private void FixedUpdate()
        {

        }
        #endregion
    }
}