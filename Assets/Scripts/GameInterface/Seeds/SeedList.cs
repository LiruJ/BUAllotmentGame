using Assets.Scripts.BUCore.UI;
using Assets.Scripts.Crops;
using Assets.Scripts.Seeds;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.GameInterface.Seeds
{
    /// <summary> The controller for the list of seed generations. </summary>
    public class SeedList : MonoBehaviour, ISelectionBar<SeedGeneration>
    {
        #region Inspector Fields
        [Header("Dependencies")]
        [Tooltip("The object used to create modal windows.")]
        [SerializeField]
        private ModalWindowController modalWindowController = null;

        [Tooltip("The object into which the seed detail prefabs will be created.")]
        [SerializeField]
        private RectTransform contentPane = null;

        [Tooltip("The tileset for the crops, used to obtain icons.")]
        [SerializeField]
        private CropTileset cropTileset = null;

        [Tooltip("The tooltip object to pass on.")]
        [SerializeField]
        private Tooltip tooltip = null;

        [Header("Prefabs")]
        [Tooltip("The generation group UI element prefab.")]
        [SerializeField]
        private SeedGenerationGroup generationGroupPrefab = null;
        #endregion

        #region Fields
        /// <summary> The UI SeedGenerationGroup elements keyed by the crop tile name of a seed generation. </summary>
        private readonly Dictionary<string, SeedGenerationGroup> seedGroupsByCropName = new Dictionary<string, SeedGenerationGroup>();
        #endregion

        #region Events
        [Serializable]
        private class seedGenerationEvent : UnityEvent<SeedGeneration> { }

        [Tooltip("Is fired when the player clicks on a seed generation in the list.")]
        [SerializeField]
        private seedGenerationEvent onSeedGenerationSelected = new seedGenerationEvent();
        #endregion

        #region List Functions
        /// <summary> Adds the given <paramref name="seedGeneration"/> to the UI list. </summary>
        /// <param name="seedGeneration"> The seed generation to add. </param>
        public void SeedAdded(SeedGeneration seedGeneration)
        {
            // If the generation has no group, create one.
            if (!seedGroupsByCropName.TryGetValue(seedGeneration.CropTileName, out SeedGenerationGroup generationGroup))
            {
                // Create the group object and add it to the UI list.
                GameObject generationGroupObject = Instantiate(generationGroupPrefab.gameObject, contentPane);

                // Get the generation group component from the created object.
                generationGroup = generationGroupObject.GetComponent<SeedGenerationGroup>();

                // Initialise the group with the crop type of the seed.
                generationGroup.InitialiseFromCrop(modalWindowController, this, tooltip, cropTileset.GetTileFromName(seedGeneration.CropTileName) as CropTile);

                // Add the group to the collection.
                seedGroupsByCropName.Add(seedGeneration.CropTileName, generationGroup);
            }

            // Add the seed generation to the group. If the generation already exists under this group, the count will be updated instead.
            generationGroup.AddGeneration(seedGeneration);
        }

        /// <summary> Removes the UI element representing the given <paramref name="seedGeneration"/> from the UI list. </summary>
        /// <param name="seedGeneration"> The generation to remove. </param>
        public void SeedRemoved(SeedGeneration seedGeneration)
        {
            // If the generation has a group, refresh it.
            if (seedGroupsByCropName.TryGetValue(seedGeneration.CropTileName, out SeedGenerationGroup generationGroup))
                generationGroup.RefreshGeneration(seedGeneration);
        }

        /// <summary> This is called when a generation is clicked, and just passes it through to the event. </summary>
        /// <param name="seedGeneration"> The button that was clicked. </param>
        public void OnButtonSelected(SeedGeneration seedGeneration) => onSeedGenerationSelected.Invoke(seedGeneration);
        #endregion
    }
}