using Assets.Scripts.Seeds;
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

        #region Properties
        /// <summary> The collection of mutatable stats. </summary>
        public IReadOnlyList<CreatureStat> CreatureStats => creatureStats;

        protected Dictionary<string, float> lifetimeStats { get; } = new Dictionary<string, float>();

        public IReadOnlyDictionary<string, float> LifetimeStats => lifetimeStats;

        /// <summary> The <see cref="Creature"/> who owns this behaviour. </summary>
        public Creature Creature { get; private set; }
        #endregion

        #region Initialisation Functions
        public void InitialiseFirstTime() => Creature = GetComponent<Creature>();
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
            seedCreated();
        }

        /// <summary> Is called when the creature is creating a seed, commonly used to set any lifetime stats that need to be calculated at the end of the creature's life. </summary>
        protected virtual void seedCreated() { }

        /// <summary> Sets the stats of this behaviour from the given <paramref name="stats"/> dictionary. </summary>
        /// <param name="stats"> The stats of the creature. </param>
        public void InitialiseFromStats(IReadOnlyDictionary<string, float> stats)
        {
            // Clear the lifetime stats.
            lifetimeStats.Clear();

            // Initialise each stat.
            foreach (CreatureStat creatureStat in creatureStats)
                creatureStat.InitialiseFromStats(stats);

            // Call the stats initialised function for any deriving class.
            statsInitialised();
        }

        /// <summary> Is called immediately after <see cref="InitialiseFromStats(IReadOnlyDictionary{string, float})"/>, meaning that every stat should be initialised. </summary>
        protected virtual void statsInitialised() { }

        /// <summary> Adds a new stat with the given <paramref name="name"/> and starting with the given <paramref name="value"/>, or 0 if no value is given. </summary>
        /// <param name="name"> The name of the new stat. </param>
        /// <param name="value"> The starting value of the new stat, defaulting to 0. </param>
        protected void addLifetimeStat(string name, float value = 0) => lifetimeStats.Add(name, value);

        /// <summary> Adds the given <paramref name="change"/> to the stat with the given <paramref name="name"/>, or does nothing if the stat does not exist. </summary>
        /// <param name="name"> The name of the stat to change. </param>
        /// <param name="change"> The amount of change to make. </param>
        protected void changeLifetimeStat(string name, float change) { if (lifetimeStats.ContainsKey(name)) lifetimeStats[name] += change; }

        /// <summary> Sets the stat with the given <paramref name="name"/> to the given <paramref name="value"/>, or does nothing if the stat does not exist. </summary>
        /// <param name="name"> The name of the stat to set. </param>
        /// <param name="value"> The value to set the stat to. </param>
        protected void setLifetimeStat(string name, float value) { if (lifetimeStats.ContainsKey(name)) lifetimeStats[name] = value; }
        #endregion
    }
}