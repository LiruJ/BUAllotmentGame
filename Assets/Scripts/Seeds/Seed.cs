using System;
using System.Collections.Generic;

namespace Assets.Scripts.Seeds
{
    /// <summary> Holds all information about a seed, including the lifetime stats of the creature who dropped it, the genetic stats, and the type of plant whence it came. </summary>
    [Serializable]
    public class Seed
    {
        #region Inspector Fields

        #endregion

        #region Properties
        public uint Generation { get; set; }

        public string CropTileName { get; set; }

        public Dictionary<string, float> GeneticStats { get; }

        public float DistanceFromGoal { get; set; }
        #endregion

        #region Constructors
        public Seed() : this(0, float.PositiveInfinity, string.Empty, new Dictionary<string, float>()) { }

        public Seed(uint generation, float distance, string cropTileName, Dictionary<string, float> geneticStats)
        {
            Generation = generation;
            DistanceFromGoal = distance;
            CropTileName = cropTileName ?? throw new ArgumentNullException(nameof(cropTileName));
            GeneticStats = geneticStats ?? throw new ArgumentNullException(nameof(geneticStats));
        }
        #endregion
    }
}
