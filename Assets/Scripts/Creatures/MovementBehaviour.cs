using Assets.Scripts.Seeds;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Creatures
{
    [RequireComponent(typeof(Creature))]
    [DisallowMultipleComponent]
    public class MovementBehaviour : MonoBehaviour, ICreatureBehaviour
    {
        #region Inspector Fields
        [Header("Dependencies")]
        [SerializeField]
        private NavMeshAgent navMeshAgent = null;

        [Header("Mutation Stats")]
        [SerializeField]
        private List<CreatureStat> creatureStats = new List<CreatureStat>();
        #endregion

        #region Fields
        private Creature creature = null;
        #endregion

        #region Properties

        #endregion

        #region Stat Functions
        public void PopulateSeed(Seed seed)
        {
            foreach (CreatureStat creatureStat in creatureStats)
                creatureStat.PopulateSeed(seed);

            populateLifetimeStats(seed);
        }

        public void InitialiseFromStats(Creature creature, IReadOnlyDictionary<string, float> stats)
        {
            this.creature = creature;

            foreach (CreatureStat creatureStat in creatureStats)
                creatureStat.InitialiseFromStats(stats);

            navMeshAgent.SetDestination(creature.GoalObject.position);
        }

        private void populateLifetimeStats(Seed seed)
        {
            // Add each stat of this behaviour to the lifetime stats of the seed.
            seed.LifetimeStats.Add("DistanceFromGoal", Vector3.Distance(transform.position, creature.GoalObject.position));
        }
        #endregion

        #region Update Functions
        private void Update()
        {

        }

        private void FixedUpdate()
        {
            creature.Health -= 0.025f;
        }
        #endregion
    }
}