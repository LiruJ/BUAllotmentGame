using Assets.Scripts.Seeds;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Creatures
{
    /// <summary> Allows a creature to move. </summary>
    [RequireComponent(typeof(Creature))]
    [DisallowMultipleComponent]
    public class MovementBehaviour : MonoBehaviour, ICreatureBehaviour
    {
        #region Inspector Fields
        [Header("Dependencies")]
        [Tooltip("The agent used for pathfinding around the map.")]
        [SerializeField]
        private NavMeshAgent navMeshAgent = null;

        [Header("Mutation Stats")]
        [Tooltip("The movement stats of this creature.")]
        [SerializeField]
        private List<CreatureStat> creatureStats = new List<CreatureStat>();
        #endregion

        #region Fields
        /// <summary> The creature who owns this behaviour. </summary>
        private Creature creature = null;
        #endregion

        #region Properties
        /// <summary> How much health is lost per second due to movement. </summary>
        public float EnergyCost { get; set; }
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

        /// <summary> Sets the stats of this behaviour from the given <paramref name="stats"/> dictionary. </summary>
        /// <param name="creature"> The base creature. </param>
        /// <param name="stats"> The stats of the creature. </param>
        public void InitialiseFromStats(Creature creature, IReadOnlyDictionary<string, float> stats)
        {
            // Set the base creature.
            this.creature = creature;
            
            // Initialise each stat.
            foreach (CreatureStat creatureStat in creatureStats)
                creatureStat.InitialiseFromStats(stats);

            // Set the destination to the goal so the creature moves immediately.
            navMeshAgent.SetDestination(creature.GoalObject.position);
        }

        /// <summary> Adds the lifetime stats from this behaviour to the given <paramref name="seed"/>. </summary>
        /// <param name="seed"> The seed to populate. </param>
        private void populateLifetimeStats(Seed seed)
        {
            // Add each stat of this behaviour to the lifetime stats of the seed.
            seed.LifetimeStats.Add("DistanceFromGoal", Vector3.Distance(transform.position, creature.GoalObject.position));
        }
        #endregion

        #region Update Functions
        private void FixedUpdate() => creature.Health -= EnergyCost * Time.fixedDeltaTime;
        #endregion
    }
}