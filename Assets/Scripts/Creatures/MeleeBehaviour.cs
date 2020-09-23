using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Creatures
{
    [RequireComponent(typeof(TargetingBehaviour))]
    [RequireComponent(typeof(MovementBehaviour))]
    public class MeleeBehaviour : CreatureBehaviour
    {
        #region Inspector Fields

        #endregion

        #region Fields
        private float timeSinceLastAttack = 0;

        private TargetingBehaviour targetingBehaviour = null;

        private MovementBehaviour movementBehaviour = null;
        #endregion

        #region Properties
        public float Range { get; set; }

        public float AttackInterval { get; set; }

        public float Damage { get; set; }
        #endregion

        #region Events
        [SerializeField]
        private UnityEvent onAttack = new UnityEvent();
        #endregion

        #region Initialisation Functions
        private void Start()
        {
            targetingBehaviour = GetComponent<TargetingBehaviour>();
            movementBehaviour = GetComponent<MovementBehaviour>();
        }
        #endregion

        #region Stat Functions
        protected override void statsInitialised()
        {
            addLifetimeStat("EnemyKills");
            addLifetimeStat("EnemyDamageDealt");
        }
        #endregion

        #region Update Functions
        private void FixedUpdate()
        {
            handleAttackLogic();
        }

        private void handleAttackLogic()
        {
            // If the creature has a target, attempt to move towards it.
            if (targetingBehaviour.HasCurrentTarget)
            {
                CreatureTarget creatureTarget = targetingBehaviour.CurrentTarget.Value;

                // If the target is out of range, move towards it.
                if (Vector3.Distance(creatureTarget.Transform.position, transform.position) > Range)
                {
                    movementBehaviour.StoppingDistance = Range;
                    movementBehaviour.SetTarget(creatureTarget.Transform);
                }
                // Otherwise; attempt to attack the target.
                else
                {
                    // Stop moving.
                    movementBehaviour.StopMoving();

                    // Face the target.
                    transform.LookAt(creatureTarget.Transform);

                    // If it has been long enough since the last attack, attack again.
                    if (timeSinceLastAttack >= AttackInterval && creatureTarget.Target.IsAlive)
                    {
                        // Deal the damage to the target.
                        creatureTarget.Target.Health -= Damage;

                        // Add the dealt damage to the stat.
                        changeLifetimeStat("EnemyDamageDealt", Damage);

                        // If the target is now dead, increment the kill counter.
                        if (!creatureTarget.Target.IsAlive) changeLifetimeStat("EnemyKills", 1);

                        // Invoke the attack event.
                        onAttack.Invoke();

                        // Reset the attack timer.
                        timeSinceLastAttack = 0;
                    }
                }
            }
            // Otherwise, just move towards the goal.
            else
            {
                movementBehaviour.SetTarget(Creature.GoalObject);
                movementBehaviour.StoppingDistance = 0;
            }

            // Add the elapsed time to the attack timer.
            timeSinceLastAttack += Time.fixedDeltaTime;
        }
        #endregion
    }
}