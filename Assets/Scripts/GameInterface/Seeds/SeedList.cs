using Assets.Scripts.Crops;
using Assets.Scripts.Seeds;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.GameInterface.Seeds
{
    /// <summary> The controller for the list of seed generations. </summary>
    public class SeedList : MonoBehaviour, ISelectionBar<SeedDetails>
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

        [Header("Prefabs")]
        [Tooltip("The details UI element prefab.")]
        [SerializeField]
        private SeedDetails seedDetailsPrefab = null;
        #endregion

        #region Fields
        /// <summary> The UI seed detail elements keyed by generation. </summary>
        private readonly Dictionary<SeedGeneration, SeedDetails> seedDetailsBySeedGeneration = new Dictionary<SeedGeneration, SeedDetails>();
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
            // If the generation has no list entry, create one.
            if (!seedDetailsBySeedGeneration.TryGetValue(seedGeneration, out SeedDetails seedDetails))
            {
                GameObject seedDetailsPane = Instantiate(seedDetailsPrefab.gameObject, contentPane);
                seedDetails = seedDetailsPane.GetComponent<SeedDetails>();
                seedDetails.InitialiseFromSeed(modalWindowController, this, cropTileset, seedGeneration);
                seedDetailsBySeedGeneration.Add(seedGeneration, seedDetails);
            }
            // Otherwise; refresh the existing one.
            else seedDetails.Refresh();
        }

        /// <summary> Removes the UI element representing the given <paramref name="seedGeneration"/> from the UI list. </summary>
        /// <param name="seedGeneration"> The generation to remove. </param>
        public void SeedRemoved(SeedGeneration seedGeneration)
        {
            // If the generation has a list entry, check to see if it needs to be destroyed.
            if (seedDetailsBySeedGeneration.TryGetValue(seedGeneration, out SeedDetails seedDetails))
            {
                // If the generation has no seeds, destroy the UI item.
                if (seedGeneration.Count == 0)
                {
                    Destroy(seedDetails.gameObject);
                    seedDetailsBySeedGeneration.Remove(seedGeneration);
                }
                // Otherwise, refresh it.
                else seedDetails.Refresh();
            }
        }

        /// <summary> This is called when a generation is clicked, and just passes it through to the event. </summary>
        /// <param name="button"> The button that was clicked. </param>
        public void OnButtonSelected(SeedDetails button) => onSeedGenerationSelected.Invoke(button.SeedGeneration);
        #endregion
    }
}