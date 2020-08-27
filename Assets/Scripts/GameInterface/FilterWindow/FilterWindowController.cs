using Assets.Scripts.Seeds;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameInterface.FilterWindow
{
    /// <summary> Manages changing the score filter for a seed generation. </summary>
    public class FilterWindowController : MonoBehaviour
    {
        #region Inspector Fields
        [Header("Elements")]
        [Tooltip("The pane showing all of the stats available to use.")]
        [SerializeField]
        private AvailableStatsPane availableStatsPane = null;

        [Tooltip("The pane showing the currently used stats.")]
        [SerializeField]
        private StatValuePane statValuePane = null;

        [Tooltip("The window title.")]
        [SerializeField]
        private Text titleText = null;
        #endregion

        #region Fields
        [Tooltip("The controller used to manage modal windows.")]
        private ModalWindowController modalWindowController = null;
        #endregion

        #region Properties
        /// <summary> The seed generation that this window is managing. </summary>
        public SeedGeneration SeedGeneration { get; private set; } = null;
        #endregion

        #region Initialisation Functions
        /// <summary> Initialises the window from the given <paramref name="seedGeneration"/>. </summary>
        /// <param name="modalWindowController"> The modal window controller used to close this window. </param>
        /// <param name="seedGeneration"> The generation to manage. </param>
        public void CreateFrom(ModalWindowController modalWindowController, SeedGeneration seedGeneration)
        {
            // Ensure this window is not being set up twice.
            if (SeedGeneration != null)
            {
                Debug.LogError("Cannot initialise window twice.");
                return;
            }

            // Set the window controller.
            this.modalWindowController = modalWindowController;

            // Set the seed generation.
            SeedGeneration = seedGeneration;

            // Set the title.
            titleText.text = $"{seedGeneration.CropTileName} seed generation {seedGeneration.Generation} filter options";

            // Populate the stat lists.
            availableStatsPane.Populate();
            statValuePane.Populate();
        }
        #endregion

        #region Button Functions
        /// <summary> Closes the window. </summary>
        public void Close() => modalWindowController.DestroyCurrentModalWindow();
        #endregion
    }
}