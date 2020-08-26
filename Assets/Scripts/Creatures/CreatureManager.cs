using Assets.Scripts.Crops;
using Assets.Scripts.Seeds;
using UnityEngine;

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

        [Tooltip("The endpoint that all creatures belonging to this player which to reach.")]
        [SerializeField]
        private Transform goalObject = null;
        #endregion

        #region Creature Functions
        public void SpawnCreature(Seed seed, Creature creaturePrefab, int plantX, int plantY)
        {
            // Create an instance of the given prefab.
            GameObject creatureInstance = Instantiate(creaturePrefab.gameObject, transform);

            // Get the creature script of the instance.
            Creature creatureScript = creatureInstance.GetComponent<Creature>();

            // Position the creature in the middle of the plant tile.
            creatureInstance.transform.position = cropTilemap.Grid.CellToWorld(new Vector3Int(plantX, 0, plantY)) + new Vector3(0.5f, 0, 0.5f);

            // Intitialise the stats of the creature based on the given seed.
            creatureScript.InitialiseFromStats(this, seed, goalObject);
        }

        public void CreatureDied(Creature creature)
        {
            // Get the dropped seed from the creature.
            Seed seed = creature.DropSeed();

            // Add the seed to the seed manager.
            seedManager.AddSeed(seed);

            // Destroy the creature.
            Destroy(creature.gameObject);
        }
        #endregion
    }
}