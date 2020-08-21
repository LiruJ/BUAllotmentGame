using System;
using System.Collections.Generic;

namespace Assets.Scripts.Seeds
{
    /// <summary> Holds all information about a seed, including the lifetime stats of the creature who dropped it, the genetic stats, and the type of plant whence it came. </summary>
    [Serializable]
    public class Seed
    {
        #region Properties
        public uint Generation { get; set; } = 0;

        public string CropTileName { get; set; } = string.Empty;

        public Dictionary<string, float> GeneticStats { get; } = new Dictionary<string, float>();

        public Dictionary<string, float> LifetimeStats { get; } = new Dictionary<string, float>();
        #endregion

        #region Constructors
        public Seed() { }

        public Seed(uint generation, string cropTileName)
        {
            Generation = generation;
            CropTileName = cropTileName ?? throw new ArgumentNullException(nameof(cropTileName));
        }
        #endregion

        #region Score Functions
        public float CalculateScore(IReadOnlyDictionary<string, float> scoreFilter)
        {
            float score = 0;

            foreach (KeyValuePair<string, float> nameMultiplierPair in scoreFilter)
            {
                // If the lifetime stats contains the current stat, add it to the score with the multiplier.
                if (LifetimeStats.TryGetValue(nameMultiplierPair.Key, out float stat)) score += stat * nameMultiplierPair.Value;
            }

            return score;
        }
        #endregion
    }
}
