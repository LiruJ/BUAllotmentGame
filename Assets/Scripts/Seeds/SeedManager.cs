using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Seeds
{
    /// <summary> Stores and manages a player's seeds. </summary>
    public class SeedManager : MonoBehaviour
    {
        #region Fields
        /// <summary> The collection of seed generations keyed by their unique identifiers. </summary>
        private readonly Dictionary<string, SeedGeneration> seedGenerations = new Dictionary<string, SeedGeneration>();
        #endregion

        #region Events
        [Serializable]
        private class seedGenerationEvent : UnityEvent<SeedGeneration> { }

        [Tooltip("Is fired when a seed is added to any generation.")]
        [SerializeField]
        private seedGenerationEvent onSeedAdded = new seedGenerationEvent();

        [Tooltip("Is fired when a seed is removed from any generation.")]
        [SerializeField]
        private seedGenerationEvent onSeedRemoved = new seedGenerationEvent();
        #endregion

        #region Initialisation Functions
        private void Start()
        {
            // Add a tomato seed to give the player something to plant.
            // TODO: A better way of setting starting seeds.
            AddSeed(new Seed(0, "Tomato"));
        }
        #endregion

        #region Seed Functions
        public void AddSeed(Seed seed)
        {
            // Get or create the list for the seed.
            if (!seedGenerations.TryGetValue(SeedGeneration.CalculateUniqueIdentifier(seed), out SeedGeneration seedGeneration))
            {
                // Create a new seed generation using the seed's data.
                seedGeneration = new SeedGeneration(seed.CropTileName, seed.Generation);

                // If the seed has a generation prior to the new one, copy the filter from it to the new one.
                if (seedGenerations.TryGetValue(SeedGeneration.CalculateUniqueIdentifier(seed.CropTileName, seed.Generation - 1), out SeedGeneration priorGeneration))
                    seedGeneration.SetFilterFrom(priorGeneration);

                // Add the generation to the dictionary.
                seedGenerations.Add(seedGeneration.UniqueIdentifier, seedGeneration);
            }

            // Add the seed to the generation and invoke the event.
            seedGeneration.Add(seed);
            onSeedAdded.Invoke(seedGeneration);
        }

        public void RemoveSeed(Seed seed)
        {
            // If the generation is 0 or is not part of this manager, do nothing.
            if (seed.Generation == 0 || !seedGenerations.TryGetValue(SeedGeneration.CalculateUniqueIdentifier(seed), out SeedGeneration seedGeneration)) return;
            
            // Remove the seed from the generation, if this fails then do nothing.
            if (!seedGeneration.Remove(seed)) return;

            // If the entire generation is now empty, remove it.
            if (seedGeneration.Count == 0) seedGenerations.Remove(seedGeneration.UniqueIdentifier);

            // Invoke the removed event.
            onSeedRemoved.Invoke(seedGeneration);
        }
        #endregion
    }
}