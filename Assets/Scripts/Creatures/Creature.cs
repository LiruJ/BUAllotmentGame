﻿using Assets.Scripts.Seeds;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Creatures
{
    /// <summary> The main creature controller. </summary>
    [DisallowMultipleComponent]
    public class Creature : MonoBehaviour
    {
        #region Inspector Fields
        [Range(1, 200)]
        [Tooltip("How much health this creature starts with.")]
        [SerializeField]
        private float health = 1;
        #endregion

        #region Fields
        /// <summary> The seed of the plant that spawned this creature. </summary>
        private Seed seed = null;

        /// <summary> The manager that spawned this creature. </summary>
        private CreatureManager creatureManager = null;

        /// <summary> Is true if the creature is alive; false otherwise. </summary>
        private bool isAlive;

        /// <summary> How long in seconds the creature has been alive. </summary>
        private float aliveTime = 0;

        /// <summary> The creature's behaviours. </summary>
        private readonly List<ICreatureBehaviour> creatureBehaviours = new List<ICreatureBehaviour>();
        #endregion

        #region Properties
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
                if (health <= 0) IsAlive = false;
            }
        }

        /// <summary> Is true if the creature is alive; false otherwise. </summary>
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

        /// <summary> The object that the creature is trying to reach. </summary>
        public Transform GoalObject { get; private set; }
        #endregion

        #region Stat Functions
        /// <summary> Finds and stores every behaviour of the creature. </summary>
        private void findBehaviours() => creatureBehaviours.AddRange(GetComponents<ICreatureBehaviour>());

        /// <summary> Initialises the creature using the given <paramref name="seed"/>. </summary>
        /// <param name="creatureManager"> The manager that spawned this creature. </param>
        /// <param name="seed"> The seed from which this creature was made. </param>
        /// <param name="goalObject"> The object that the creature wishes to reach. </param>
        public void InitialiseFromStats(CreatureManager creatureManager, Seed seed, Transform goalObject)
        {
            // Set fields and properties.
            this.creatureManager = creatureManager;
            this.seed = seed;
            GoalObject = goalObject;

            // Start off being alive.
            isAlive = true;

            // Add the behaviours.
            findBehaviours();

            // Initialise each behaviour.
            foreach (ICreatureBehaviour creatureBehaviour in creatureBehaviours)
                creatureBehaviour.InitialiseFromStats(this, seed.GeneticStats);
        }
        #endregion

        #region Seed Dropping Functions
        /// <summary> Creates and returns a seed with this creature's stats. </summary>
        /// <returns> The created seed. </returns>
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

        /// <summary> Adds all generic stats to the given <paramref name="droppedSeed"/>. </summary>
        /// <param name="droppedSeed"> The seed to fill with stats. </param>
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