using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Seeds
{
    /// <summary> Holds all information about a seed, including the lifetime stats of the creature who dropped it, the genetic stats, and the type of plant whence it came. </summary>
    [Serializable]
    public class Seed
    {
        #region Properties
        /// <summary> The generation from which this seed originates. </summary>
        public uint Generation { get; set; } = 0;

        /// <summary> The name of the plant that this seed will create. </summary>
        public string CropTileName { get; set; } = string.Empty;

        /// <summary> The base genetic stats (movement speed, health, etc.) of any creature created from this seed. </summary>
        public Dictionary<string, float> GeneticStats { get; } = new Dictionary<string, float>();

        /// <summary> The lifetime stats (damage dealt, kills, distance travelled, etc.) of the creature from whom this seed was created. </summary>
        public Dictionary<string, float> LifetimeStats { get; } = new Dictionary<string, float>();
        #endregion

        #region Constructors
        /// <summary> Creates a seed with the given generation and crop. </summary>
        /// <param name="generation"> The generation of the seed. </param>
        /// <param name="cropTileName"></param>
        public Seed(uint generation, string cropTileName)
        {
            // Set the generation.
            Generation = generation;

            // If the crop name is invalid, throw an exception. Otherwise; set the crop tile.
            if (string.IsNullOrWhiteSpace(cropTileName)) Debug.LogException(new ArgumentException("Seed cannot have an empty or null crop name."));
            CropTileName = cropTileName ?? throw new ArgumentNullException(nameof(cropTileName));
        }
        #endregion

        #region Score Functions
        /// <summary> Calculates the score of this seed using the given <paramref name="scoreFilter"/> and <paramref name="bestWorstByName"/>. </summary>
        /// <param name="scoreFilter"> The stat names and weights used to calculate the score. </param>
        /// <param name="bestWorstByName"> The best and worst stats of the whole generaiton, used to normalise. </param>
        /// <returns> The score of this seed. </returns>
        public float CalculateScore(IReadOnlyDictionary<string, float> scoreFilter, IReadOnlyDictionary<string, BestWorst> bestWorstByName)
        {
            // Start the score as 0.
            float score = 0;

            // Go over each stat in the filter, if the lifetime stats contains the filter stat, add it to the score with the weight.
            foreach (KeyValuePair<string, float> nameWeightPair in scoreFilter)
                if (LifetimeStats.TryGetValue(nameWeightPair.Key, out float stat) && stat > 0 && bestWorstByName.TryGetValue(nameWeightPair.Key, out BestWorst bestWorst) && bestWorst.Best != bestWorst.Worst)
                    score += ((stat - bestWorst.Worst) / (bestWorst.Best - bestWorst.Worst)) * nameWeightPair.Value;

            // Return the final score.
            if (float.IsNaN(score)) Debug.LogError("Score is NaN");
            return score;
        }
        #endregion

        #region String Functions
        public override string ToString() => $"{CropTileName} seed generation {Generation}";
        #endregion
    }
}