using Assets.Scripts.Crops;
using Assets.Scripts.Seeds;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Creatures
{
    public class CreatureManager : MonoBehaviour
    {
        #region Inspector Fields
        [Header("Dependencies")]
        [SerializeField]
        private CropTilemap cropTilemap = null;

        [SerializeField]
        private SeedManager seedManager = null;

        [SerializeField]
        private Transform goalObject = null;
        #endregion

        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Initialisation Functions
        private void Start()
        {

        }

        private void Awake()
        {

        }
        #endregion

        #region Creature Functions
        public void SpawnCreature(Seed seed, Creature creaturePrefab, int plantX, int plantY)
        {
            // Create an instance of the given prefab.
            GameObject creatureInstance = Instantiate(creaturePrefab.gameObject, transform);

            // Get the creature script of the instance.
            Creature creatureScript = creatureInstance.GetComponent<Creature>();

            creatureInstance.transform.position = cropTilemap.Grid.CellToWorld(new Vector3Int(plantX, 0, plantY)) + new Vector3(0.5f, 0, 0.5f);

            // Intitialise the stats of the creature based on the given stats.
            creatureScript.InitialiseFromStats(this, seed, goalObject);
        }

        public void CreatureDied(Creature creature)
        {
            // Create a new seed and populate it from the creature.
            Seed seed = new Seed();
            creature.PopulateSeed(seed);

            // Add the seed to the seed manager.
            seedManager.AddSeed(seed);

            // Destroy the creature.
            Destroy(creature.gameObject);
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