using Assets.Scripts.Seeds;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Creatures
{
    [DisallowMultipleComponent]
    public class Creature : MonoBehaviour
    {
        #region Inspector Fields
        [Range(1, 200)]
        [SerializeField]
        private float health = 1;
        #endregion

        #region Fields
        /// <summary> The seed of the plant that spawned this creature. </summary>
        private Seed seed = null;

        private CreatureManager creatureManager = null;

        private bool isAlive;


        private float aliveTime = 0;

        private readonly List<ICreatureBehaviour> creatureBehaviours = new List<ICreatureBehaviour>();
        #endregion

        #region Properties
        public float Health
        {
            get => health;
            set
            {
                if (!IsAlive) return;

                health = value;

                if (health <= 0) IsAlive = false;
            }
        }

        public bool IsAlive
        {
            get => isAlive;
            private set
            {
                // If the creature is alive, set the value. The creature can't be brought back to life.
                if (isAlive) isAlive = value;
                else return;

                // Tell the creature manager that this creature is dead, if it died.
                if (!isAlive) creatureManager.CreatureDied(this);
            }
        }

        public Transform GoalObject { get; private set; }
        #endregion

        #region Stat Functions
        private void findBehaviours() => creatureBehaviours.AddRange(GetComponents<ICreatureBehaviour>());

        public void InitialiseFromStats(CreatureManager creatureManager, Seed seed, Transform goalObject)
        {
            this.creatureManager = creatureManager;
            this.seed = seed;
            GoalObject = goalObject;

            isAlive = true;

            // Add the behaviours.
            findBehaviours();

            // Initialise each behaviour.
            foreach (ICreatureBehaviour creatureBehaviour in creatureBehaviours)
                creatureBehaviour.InitialiseFromStats(this, seed.GeneticStats);
        }
        #endregion

        #region Seed Dropping Functions
        public Seed DropSeed()
        {
            // Create a new seed using the stats from this creature's seed.
            Seed droppedSeed = new Seed(seed.Generation + 1, seed.CropTileName);

            // Give each behaviour a chance to set the genetic and lifetime stats of the dropped seed.
            foreach (ICreatureBehaviour creatureBehaviour in creatureBehaviours)
                creatureBehaviour.PopulateSeed(droppedSeed);

            // Calculate the generic lifetime stats of this creature.
            populateLifetimeStats(droppedSeed);

            // Return the dropped seed.
            return droppedSeed;
        }

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