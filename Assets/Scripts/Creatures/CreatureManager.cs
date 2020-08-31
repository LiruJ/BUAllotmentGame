using Assets.Scripts.Crops;
using Assets.Scripts.Seeds;
using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Creatures
{
    /// <summary> Manages spawning and destroying creatures for a player. </summary>
    public class CreatureManager : MonoBehaviour
    {
        #region Inspector Fields
        [Header("Dependencies")]
        [Tooltip("The crop tilemap of the player.")]
        [SerializeField]
        private CropTilemap cropTilemap = null;

        [Tooltip("The seed manager of the player.")]
        [SerializeField]
        private SeedManager seedManager = null;

        [Tooltip("The endpoint that all creatures belonging to this player wish to reach.")]
        [SerializeField]
        private Transform goalObject = null;

        [Tooltip("The object into which the creatures are instantiated.")]
        [SerializeField]
        private Transform creatureContainer = null;
        #endregion

        #region Fields
        private readonly Dictionary<int, Creature> creaturesByInstanceID = new Dictionary<int, Creature>();
        #endregion

        #region Events
        [Serializable]
        private class uintEvent : UnityEvent<uint> { }

        [Tooltip("Is fired when the last creature in a generation dies.")]
        [SerializeField]
        private uintEvent onLastCreatureDeath = new uintEvent();
        #endregion

        #region Creature Functions
        public void SpawnCreature(Seed seed, Creature creaturePrefab, int plantX, int plantY)
        {
            // Create an instance of the given prefab and position it in the middle of the plant tile.
            GameObject creatureInstance = Instantiate(creaturePrefab.gameObject, 
                cropTilemap.Grid.CellToWorld(new Vector3Int(plantX, 0, plantY)) + new Vector3(cropTilemap.Grid.cellSize.x * 0.5f, 0, cropTilemap.Grid.cellSize.z * 0.5f), Quaternion.identity, creatureContainer);

            // Set the name of the creature.
            creatureInstance.name = $"{seed.CropTileName} creature generation {seed.Generation}";

            // Get the creature script of the instance.
            Creature creatureScript = creatureInstance.GetComponent<Creature>();

            // Add the creature to the collection.
            creaturesByInstanceID.Add(creatureInstance.GetInstanceID(), creatureScript);

            // Intitialise the stats of the creature based on the given seed.
            creatureScript.InitialiseFromStats(this, seed, goalObject);
        }

        public void CreatureDied(Creature creature)
        {
            // Get the dropped seed from the creature.
            Seed seed = creature.DropSeed();

            // Add the seed to the seed manager.
            seedManager.AddSeed(seed);

            // Remove the creature from the collection.
            if (!creaturesByInstanceID.Remove(creature.gameObject.GetInstanceID())) Debug.LogError($"Creature {creature.gameObject.GetInstanceID()} could not be removed.", creature.gameObject);

            // If there are no creatures left, invoke the event.
            if (creaturesByInstanceID.Count == 0) onLastCreatureDeath.Invoke(creature.Seed.Generation);

            // Destroy the creature.
            Destroy(creature.gameObject);
        }
        #endregion
    }
}