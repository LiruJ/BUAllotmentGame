using Assets.Scripts.Crops;
using Assets.Scripts.GameInterface.FilterWindow;
using Assets.Scripts.Seeds;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameInterface.Seeds
{
    /// <summary> Controls the display of a seed generation within a seed list. </summary>
    public class SeedDetails : MonoBehaviour
    {
        #region Inspector Fields
        [Header("Elements")]
        [Tooltip("The text showing the number of the generation.")]
        [SerializeField]
        private Text seedGenerationLabel = null;

        [Tooltip("The text showing the number of seeds in this generation.")]
        [SerializeField]
        private Text seedCount = null;

        [Tooltip("The image showing the icon of the generation's crop.")]
        [SerializeField]
        private Image seedIcon = null;

        [Tooltip("The button used to select this generation.")]
        [SerializeField]
        private Button selectionButton = null;

        [Tooltip("The button used to open the score filter edit window.")]
        [SerializeField]
        private Button filterButton = null;

        [Header("Prefabs")]
        [Tooltip("The score filter edit window to create.")]
        [SerializeField]
        private FilterWindowController filterWindowPrefab = null;
        #endregion

        #region Properties
        /// <summary> The seed generation that this UI element represents. </summary>
        public SeedGeneration SeedGeneration { get; private set; }
        #endregion

        #region Initialisation Functions
        /// <summary> Initialise the UI element based on the given <paramref name="seedGeneration"/>. </summary>
        /// <param name="modalWindowController"> The controller used to open the score filter edit window. </param>
        /// <param name="selectionBar"> The parent controller. </param>
        /// <param name="cropTileset"> The tileset used to get icons. </param>
        /// <param name="seedGeneration"> The seed generation to represent. </param>
        public void InitialiseFromSeed(ModalWindowController modalWindowController, ISelectionBar<SeedDetails> selectionBar, CropTileset cropTileset, SeedGeneration seedGeneration)
        {
            // Set the seed generation that this UI element represents.
            SeedGeneration = seedGeneration;

            // Bind the main button to select this generation.
            selectionButton.onClick.AddListener(() => selectionBar.OnButtonSelected(this));

            // Bind the filter button to create a new filter window.
            filterButton.onClick.AddListener(() => 
            {
                FilterWindowController filterWindowController = modalWindowController.CreateModalWindow<FilterWindowController>(filterWindowPrefab.gameObject);
                filterWindowController.CreateFrom(modalWindowController, SeedGeneration);
            });

            seedCount.text = $"x{seedGeneration.Count}";
            seedGenerationLabel.text = $"Generation: {seedGeneration.Generation}";
            seedIcon.sprite = (cropTileset.GetTileFromName(seedGeneration.CropTileName) as CropTile).Icon;

            if (SeedGeneration.Generation == 0) filterButton.gameObject.SetActive(false);
        }

        public void Refresh() => seedCount.text = $"x{SeedGeneration.Count}";
        #endregion
    }
}