using Assets.Scripts.Crops;
using Assets.Scripts.GameInterface.FilterWindow;
using Assets.Scripts.Seeds;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameInterface.Seeds
{
    public class SeedDetails : MonoBehaviour
    {
        #region Inspector Fields
        [Header("Elements")]
        [SerializeField]
        private Text seedGenerationLabel = null;

        [SerializeField]
        private Text seedCount = null;

        [SerializeField]
        private Image seedIcon = null;

        [SerializeField]
        private Button selectionButton = null;

        [SerializeField]
        private Button filterButton = null;

        [Header("Prefabs")]
        [SerializeField]
        private FilterWindowController filterWindowPrefab = null;
        #endregion

        #region Properties
        public SeedGeneration SeedGeneration { get; private set; }
        #endregion

        #region Initialisation Functions
        public void InitialiseFromSeed(ModalWindowController modalWindowController, ISelectionBar<SeedDetails> selectionBar, CropTileset cropTileset, SeedGeneration seedGeneration)
        {
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