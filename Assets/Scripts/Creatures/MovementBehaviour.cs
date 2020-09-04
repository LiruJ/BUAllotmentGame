using Assets.Scripts.Seeds;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Creatures
{
    /// <summary> Allows a creature to move. </summary>
    public class MovementBehaviour : CreatureBehaviour
    {
        #region Inspector Fields
        [Header("Dependencies")]
        [Tooltip("The agent used for pathfinding around the map.")]
        [SerializeField]
        private NavMeshAgent navMeshAgent = null;

        [Tooltip("The animator.")]
        [SerializeField]
        private Animator animator = null;
        #endregion

        #region Fields

        #endregion

        #region Properties
        /// <summary> How much health is lost per second due to movement. </summary>
        public float EnergyCost { get; set; }

        public float StoppingDistance { get => navMeshAgent.stoppingDistance; set => navMeshAgent.stoppingDistance = value; }
        #endregion

        #region Stat Functions
        protected override void populateLifetimeStats(Seed seed)
        {
            // Add each stat of this behaviour to the lifetime stats of the seed.
            seed.LifetimeStats.Add("DistanceFromGoal", Vector3.Distance(transform.position, Creature.GoalObject.position));
        }

        protected override void statsInitialised()
        {
            // Set the destination to the goal so the creature moves immediately.
            SetTarget(Creature.GoalObject);
        }
        #endregion

        #region Movement Functions
        public void SetTarget(Transform transform)
        {
            navMeshAgent.destination = transform.position;
            navMeshAgent.isStopped = false;
        }

        public void StopMoving()
        {
            navMeshAgent.isStopped = true;
            navMeshAgent.velocity = Vector3.zero;
        }
        #endregion

        #region Update Functions
        private void FixedUpdate()
        {
            animator.SetFloat("WalkSpeed", navMeshAgent.velocity.magnitude);
            Creature.Health -= EnergyCost * Time.fixedDeltaTime;
        }
        #endregion
    }
}