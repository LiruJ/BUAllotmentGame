using Assets.Scripts.Crops;
using Assets.Scripts.Seeds;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.GameInterface.Seeds
{
    public class SeedList : MonoBehaviour, ISelectionBar<SeedDetails>
    {
        #region Inspector Fields
        [Header("Dependencies")]
        [SerializeField]
        private ModalWindowController modalWindowController = null;

        [SerializeField]
        private SeedManager seedManager = null;

        [SerializeField]
        private RectTransform contentPane = null;

        [SerializeField]
        private CropTileset cropTileset = null;

        [Header("Prefabs")]
        [SerializeField]
        private SeedDetails seedDetailsPrefab = null;
        #endregion

        #region Fields
        private readonly Dictionary<SeedGeneration, SeedDetails> seedDetailsBySeedGeneration = new Dictionary<SeedGeneration, SeedDetails>();
        #endregion

        #region Events
        [Serializable]
        private class seedGenerationEvent : UnityEvent<SeedGeneration> { }

        [SerializeField]
        private seedGenerationEvent onSeedGenerationSelected = new seedGenerationEvent();
        #endregion

        #region List Functions
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

        public void OnButtonSelected(SeedDetails button) => onSeedGenerationSelected.Invoke(button.SeedGeneration);
        #endregion
    }
}