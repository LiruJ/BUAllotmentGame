using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Seeds
{
    /// <summary> Stores and manages the player's seeds. </summary>
    public class SeedManager : MonoBehaviour
    {
        #region Inspector Fields

        #endregion

        #region Fields
        private readonly Dictionary<string, SeedGeneration> seedGenerations = new Dictionary<string, SeedGeneration>();
        #endregion

        #region Properties
        public IReadOnlyDictionary<string, SeedGeneration> SeedsByGeneration => seedGenerations;
        #endregion

        #region Events
        [Serializable]
        private class seedGenerationEvent : UnityEvent<SeedGeneration> { }

        [SerializeField]
        private seedGenerationEvent onSeedAdded = new seedGenerationEvent();

        [SerializeField]
        private seedGenerationEvent onSeedRemoved = new seedGenerationEvent();
        #endregion

        #region Initialisation Functions
        private void Start()
        {
            AddSeed(new Seed(0, "Tomato"));
        }

        private void Awake()
        {

        }
        #endregion

        #region Seed Functions
        public void AddSeed(Seed seed)
        {
            // Get or create the list for the seed.
            if (!seedGenerations.TryGetValue(SeedGeneration.CalculateUniqueIdentifier(seed), out SeedGeneration seedGeneration))
            {
                seedGeneration = new SeedGeneration(seed.CropTileName, seed.Generation);
                seedGenerations.Add(seedGeneration.UniqueIdentifier, seedGeneration);
            }

            seedGeneration.Add(seed);
            onSeedAdded.Invoke(seedGeneration);
        }

        public void RemoveSeed(Seed seed)
        {
            // If the generation is 0 or has no list of seeds, do nothing.
            if (seed.Generation == 0 || !seedGenerations.TryGetValue(SeedGeneration.CalculateUniqueIdentifier(seed), out SeedGeneration seedGeneration)) return;
            
            seedGeneration.Remove(seed);

            // If the entire generation is now empty, remove it.
            if (seedGeneration.Count == 0) seedGenerations.Remove(seedGeneration.UniqueIdentifier);

            onSeedRemoved.Invoke(seedGeneration);
        }
        #endregion
    }
}