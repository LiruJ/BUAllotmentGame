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
        /// <summary> The collection of seed generations keyed by their plant types. </summary>
        private readonly Dictionary<string, SortedList<uint, SeedGeneration>> seedsByPlantType = new Dictionary<string, SortedList<uint, SeedGeneration>>();
        #endregion

        #region Events
        [Serializable]
        private class seedGenerationEvent : UnityEvent<SeedGeneration> { }

        [Tooltip("Is fired once the seed manager is finished setting up.")]
        [SerializeField]
        private UnityEvent onInitialised = new UnityEvent();

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

            // Invoke the initialised event.
            onInitialised.Invoke();
        }
        #endregion

        #region Seed Functions
        public void AddSeed(Seed seed)
        {
            // Get or create the list for the crop type.
            if (!seedsByPlantType.TryGetValue(seed.CropTileName, out SortedList<uint, SeedGeneration> cropTypeGenerations))
            {
                // Create a new list of seed generations.
                cropTypeGenerations = new SortedList<uint, SeedGeneration>(1);

                // Add the generations list to the dictionary keyed by the crop name.
                seedsByPlantType.Add(seed.CropTileName, cropTypeGenerations);
            }

            // If the seed generation list doesn't contain a seed generation for the given seed, create one.
            if (!cropTypeGenerations.TryGetValue(seed.Generation, out SeedGeneration seedGeneration))
            {
                // Create a new seed generation using the seed's data.
                seedGeneration = new SeedGeneration(seed.CropTileName, seed.Generation);

                // Add the seed generation to the list.
                cropTypeGenerations.Add(seed.Generation, seedGeneration);
                
                // If the seed has a generation prior to the new one, copy the filter from it to the new one.
                if (cropTypeGenerations.IndexOfKey(seed.Generation) > 0)
                    seedGeneration.SetFilterFrom(cropTypeGenerations.Values[cropTypeGenerations.IndexOfKey(seed.Generation) - 1]);
            }

            // Add the seed to the generation and invoke the event.
            seedGeneration.Add(seed);
            onSeedAdded.Invoke(seedGeneration);
        }

        public SeedGeneration GetLatestGenerationOfCropType(string cropTileName)
        {
            // If the given crop tile name has no generations associated with it, return null.
            if (!seedsByPlantType.TryGetValue(cropTileName, out SortedList<uint, SeedGeneration> cropTypeGenerations)) return null;

            // Return the last seed generation in the list.
            return cropTypeGenerations.Values[cropTypeGenerations.Count - 1];
        }

        public void RemoveSeed(Seed seed)
        {
            // If the generation is 0 or is not part of this manager, do nothing.
            if (seed.Generation == 0 
                || !seedsByPlantType.TryGetValue(seed.CropTileName, out SortedList<uint, SeedGeneration> cropTypeGenerations) || !cropTypeGenerations.TryGetValue(seed.Generation, out SeedGeneration seedGeneration)) 
                return;

            // Remove the seed from the generation, if this fails then do nothing.
            if (!seedGeneration.Remove(seed)) return;

            // If the entire generation is now empty, remove it.
            if (seedGeneration.Count == 0) cropTypeGenerations.Remove(seedGeneration.Generation);

            // If the crop type's list is now empty, remove it.
            if (cropTypeGenerations.Count == 0) seedsByPlantType.Remove(seed.CropTileName);

            // Invoke the removed event.
            onSeedRemoved.Invoke(seedGeneration);
        }
        #endregion
    }
}