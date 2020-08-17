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
        private readonly Dictionary<uint, List<Seed>> seedsByGeneration = new Dictionary<uint, List<Seed>>();
        #endregion

        #region Properties
        public IReadOnlyDictionary<uint, List<Seed>> SeedsByGeneration => seedsByGeneration;
        #endregion

        #region Events
        [Serializable]
        private class seedEvent : UnityEvent<Seed> { }

        [SerializeField]
        private seedEvent onSeedAdded = new seedEvent();

        [SerializeField]
        private seedEvent onSeedRemoved = new seedEvent();
        #endregion

        #region Initialisation Functions
        private void Start()
        {
            AddSeed(new Seed(0, float.PositiveInfinity, "Tomato", new Dictionary<string, float>()));
        }

        private void Awake()
        {

        }
        #endregion

        #region Seed Functions
        public void AddSeed(Seed seed)
        {
            // Get or create the list for the seed.
            if (!seedsByGeneration.TryGetValue(seed.Generation, out List<Seed> generationSeeds))
            {
                generationSeeds = new List<Seed>(1);
                seedsByGeneration.Add(seed.Generation, generationSeeds);
            }

            generationSeeds.Add(seed);
            onSeedAdded.Invoke(seed);
        }

        public void RemoveSeed(Seed seed)
        {
            // If the generation is 0 or has no list of seeds, do nothing.
            if (seed.Generation == 0 || !seedsByGeneration.TryGetValue(seed.Generation, out List<Seed> generationSeeds)) return;

            generationSeeds.Remove(seed);
            onSeedRemoved.Invoke(seed);
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