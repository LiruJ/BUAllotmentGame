using Assets.Scripts.Crops;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Seeds
{
    /// <summary> Holds all information about a seed, including the lifetime stats of the creature who dropped it, the genetic stats, and the type of plant whence it came. </summary>
    [Serializable]
    public class Seed
    {
        #region Inspector Fields

        #endregion

        #region Properties
        public string CropTileName { get; }

        public IReadOnlyDictionary<string, float> GeneticStats { get; }
        #endregion

        #region Constructors
        public Seed(string cropTileName, IReadOnlyDictionary<string, float> geneticStats)
        {
            CropTileName = cropTileName ?? throw new ArgumentNullException(nameof(cropTileName));
            GeneticStats = geneticStats ?? throw new ArgumentNullException(nameof(geneticStats));
        }
        #endregion

        #region Functions

        #endregion
    }
}
