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

        #region Properties
        /// <summary> How much health is lost per second due to movement. </summary>
        public float EnergyCost { get; set; }

        /// <summary> How far from the target the creature will stop. </summary>
        public float StoppingDistance { get => navMeshAgent.stoppingDistance; set => navMeshAgent.stoppingDistance = value; }
        #endregion

        #region Stat Functions
        protected override void statsInitialised()
        {
            // Set the destination to the goal so the creature moves immediately.
            SetTarget(Creature.GoalObject);

            // Add the distance to the lifetime stats.
            addLifetimeStat("DistanceFromGoal");
        }
        #endregion

        #region Movement Functions
        /// <summary> Sets the target of the creature to the position of the given <paramref name="transform"/> and begins moving. </summary>
        /// <param name="transform"> The transform whose position will be used as the new target. </param>
        public void SetTarget(Transform transform)
        {
            navMeshAgent.destination = transform.position;
            navMeshAgent.isStopped = false;
        }

        /// <summary> Completely stops the creature from moving. Does not clear the creature's target. </summary>
        public void StopMoving()
        {
            navMeshAgent.isStopped = true;
            navMeshAgent.velocity = Vector3.zero;
        }
        #endregion

        #region Update Functions
        private void FixedUpdate()
        {
            // Update the distance from the goal.
            setLifetimeStat("DistanceFromGoal", Vector3.Distance(transform.position, Creature.GoalObject.position));

            // Update the walk speed of the animator.
            animator.SetFloat("WalkSpeed", navMeshAgent.velocity.magnitude);

            // Subtract the energy cost from the creature's health.
            Creature.Health -= EnergyCost * Time.fixedDeltaTime;
        }
        #endregion
    }
}