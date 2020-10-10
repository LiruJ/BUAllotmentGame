using Assets.Scripts.BUCore.UI;
using Assets.Scripts.GameInterface.FilterWindow;
using Assets.Scripts.Seeds;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameInterface.Seeds
{
    public class SeedGenerationDisplay : MonoBehaviour
    {
        #region Inspector Fields
        [Header("Dependencies")]
        [SerializeField]
        private TextMeshProUGUI label = null;

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

        #region Fields
        /// <summary> The seed generation that this UI element represents. </summary>
        private SeedGeneration seedGeneration;
        #endregion

        #region Properties

        #endregion

        #region Initialisation Functions
        private void Start()
        {

        }

        private void Awake()
        {

        }

        public void InitialiseFromGeneration(ModalWindowController modalWindowController, ISelectionBar<SeedGeneration> selectionBar, Tooltip tooltip, SeedGeneration seedGeneration)
        {
            // Set the seed generation.
            this.seedGeneration = seedGeneration;

            // Bind the main button to select this generation.
            selectionButton.onClick.AddListener(() => selectionBar.OnButtonSelected(seedGeneration));

            // Bind the filter button to create a new filter window.
            filterButton.onClick.AddListener(() =>
            {
                FilterWindowController filterWindowController = modalWindowController.CreateModalWindow<FilterWindowController>(filterWindowPrefab.gameObject);
                filterWindowController.CreateFrom(modalWindowController, this.seedGeneration);
            });

            // Hide the filter button if the generation is 0, as that's the infinite pool of seeds.
            if (this.seedGeneration.Generation == 0) filterButton.gameObject.SetActive(false);

            // Set the tooltip object of anything that needs it.
            foreach (TooltipActivator tooltipActivator in GetComponentsInChildren<TooltipActivator>())
                tooltipActivator.Tooltip = tooltip;

            // Refresh the display.
            Refresh();
        }

        public void Refresh() => label.text = $"Generation {seedGeneration.Generation} (x{seedGeneration.Count})";
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