using Assets.Scripts.Seeds;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameInterface.FilterWindow
{
    public class FilterWindowController : MonoBehaviour
    {
        #region Inspector Fields
        [Header("Elements")]
        [SerializeField]
        private AvailableStatsPane availableStatsPane = null;

        [SerializeField]
        private StatValuePane statValuePane = null;

        [SerializeField]
        private Text titleText = null;
        #endregion

        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Initialisation Functions
        public void CreateFrom(SeedGeneration seedGeneration)
        {
            // Set the title.
            titleText.text = $"{seedGeneration.CropTileName} seed generation {seedGeneration.Generation} filter options";

            // Populate the stat lists.
            availableStatsPane.Populate(seedGeneration.UnsortedSeeds);
            statValuePane.Populate(seedGeneration.ScoreFilter);
        }
        #endregion
    }
}