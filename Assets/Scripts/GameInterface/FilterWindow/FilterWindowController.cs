using Assets.Scripts.Seeds;
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
        private ModalWindowController modalWindowController = null;
        #endregion

        #region Properties
        public SeedGeneration SeedGeneration { get; private set; } = null;
        #endregion

        #region Initialisation Functions
        public void CreateFrom(ModalWindowController modalWindowController, SeedGeneration seedGeneration)
        {
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
        public void Close() => modalWindowController.DestroyCurrentModalWindow();
        #endregion
    }
}