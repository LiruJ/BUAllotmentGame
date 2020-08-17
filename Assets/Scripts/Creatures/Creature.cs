using Assets.Scripts.Seeds;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Creatures
{
    [DisallowMultipleComponent]
    public class Creature : MonoBehaviour
    {
        #region Inspector Fields

        #endregion

        #region Fields
        /// <summary> The seed of the plant that spawned this creature. </summary>
        private Seed seed = null;

        private CreatureManager creatureManager = null;

        private bool isAlive;
        private float health;

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

        #region Initialisation Functions
        private void findBehaviours() => creatureBehaviours.AddRange(GetComponents<ICreatureBehaviour>());

        public void PopulateSeed(Seed seed)
        {
            // Set the plant type and generation of the seed.
            seed.CropTileName = this.seed.CropTileName;
            seed.Generation = this.seed.Generation + 1;

            // Set the stats of the seed.
            foreach (ICreatureBehaviour creatureBehaviour in creatureBehaviours)
                creatureBehaviour.PopulateSeed(seed);
        }

        public void InitialiseFromStats(CreatureManager creatureManager, Seed seed, Transform goalObject)
        {
            this.creatureManager = creatureManager;
            this.seed = seed;
            GoalObject = goalObject;

            isAlive = true;
            Health = 100;

            // Add the behaviours.
            findBehaviours();

            // Initialise each behaviour.
            foreach (ICreatureBehaviour creatureBehaviour in creatureBehaviours)
                creatureBehaviour.InitialiseFromStats(this, seed.GeneticStats);
        }
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