using Assets.Scripts.BUCore.UI;
using Assets.Scripts.Crops;
using Assets.Scripts.Seeds;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameInterface.Seeds
{
    public class SeedGenerationGroup : MonoBehaviour
    {
        #region Inspector Fields
        [Header("Dependencies")]
        [Tooltip("The text displaying the name of the crop group.")]
        [SerializeField]
        private TextMeshProUGUI generationName = null;

        [Tooltip("The image displaying the icon of the crop group.")]
        [SerializeField]
        private Image cropIcon = null;

        [Tooltip("The transform into which the prefabs are created.")]
        [SerializeField]
        private RectTransform generationListPane = null;

        [Header("Prefabs")]
        [Tooltip("The prefab of the element used to display a specific generation of a crop.")]
        [SerializeField]
        private SeedGenerationDisplay seedGenerationDisplayPrefab = null;
        #endregion

        #region Fields
        /// <summary> Each generation element keyed by the number of its generation. </summary>
        private readonly Dictionary<uint, SeedGenerationDisplay> generationsByNumber = new Dictionary<uint, SeedGenerationDisplay>();

        /// <summary> The list of all generation groups. </summary>
        private VerticalLayoutGroup parentList;

        /// <summary> The object that allows modal windows to be created. </summary>
        private ModalWindowController modalWindowController;

        /// <summary> The seed list. </summary>
        private ISelectionBar<SeedGeneration> seedListController;

        /// <summary> The tooltip to use. </summary>
        private Tooltip tooltip;
        #endregion

        #region Initialisation Functions
        public void InitialiseFromCrop(ModalWindowController modalWindowController, ISelectionBar<SeedGeneration> selectionBar, Tooltip tooltip, CropTile cropTile)
        {
            // Set dependencies.
            this.modalWindowController = modalWindowController;
            seedListController = selectionBar;
            this.tooltip = tooltip;
            // NOTE: You may think that it would be easier to use GetComponentInParent, however, this returns the vertical layout group that's part of this object instead.
            parentList = transform.parent.GetComponent<VerticalLayoutGroup>();

            // Set label and icon.
            generationName.text = cropTile.Name;
            cropIcon.sprite = cropTile.Icon;
        }
        #endregion

        #region Group Functions
        public void AddGeneration(SeedGeneration seedGeneration)
        {
            // If the given generation does not have a display, create one.
            if (!generationsByNumber.TryGetValue(seedGeneration.Generation, out SeedGenerationDisplay seedGenerationDisplay))
            {
                // Create the display object and add it to the list.
                GameObject generationObject = Instantiate(seedGenerationDisplayPrefab.gameObject, generationListPane.transform);

                // Get the generation display component from the created object.
                seedGenerationDisplay = generationObject.GetComponent<SeedGenerationDisplay>();

                // Initialise the display with the generation.
                seedGenerationDisplay.InitialiseFromGeneration(modalWindowController, seedListController, tooltip, seedGeneration);

                // Add the display to the collection.
                generationsByNumber.Add(seedGeneration.Generation, seedGenerationDisplay);

                // In true Unity fashion, the list does not automatically update. I spent an hour and a half on this, and this was the only way I could make it work. Change this at your own risk.
                parentList.enabled = false;
                LayoutRebuilder.ForceRebuildLayoutImmediate(parentList.transform as RectTransform);
                parentList.enabled = true;
            }
            // Otherwise; if the generation exists, just refresh it.
            else seedGenerationDisplay.Refresh();
        }

        public void RefreshGeneration(SeedGeneration seedGeneration)
        {
            // If the generation has a display, handle it.
            if (generationsByNumber.TryGetValue(seedGeneration.Generation, out SeedGenerationDisplay seedGenerationDisplay))
            {
                // If the number of seeds within the generation is 0 and the generation isn't the infinite seed pool (generation 0), destroy it.
                if (seedGeneration.Generation != 0 && seedGeneration.Count == 0)
                {
                    Destroy(seedGenerationDisplay.gameObject);
                    generationsByNumber.Remove(seedGeneration.Generation);
                }
                // Otherwise, just refresh it.
                else seedGenerationDisplay.Refresh();
            }
        }
        #endregion
    }
}