using Assets.Scripts.Crops;
using Assets.Scripts.Seeds;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.GameInterface.Seeds
{
    public class SeedList : MonoBehaviour
    {
        #region Inspector Fields
        [Header("Dependencies")]
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
        private readonly Dictionary<Seed, SeedDetails> seedDetailsBySeed = new Dictionary<Seed, SeedDetails>();
        #endregion

        #region Properties

        #endregion

        #region Events
        [Serializable]
        private class seedEvent : UnityEvent<Seed> { }

        [SerializeField]
        private seedEvent onSeedSelected = new seedEvent();
        #endregion

        #region Initialisation Functions
        private void Start()
        {
        }

        private void Awake()
        {

        }
        #endregion

        #region List Functions
        public void AddSeed(Seed seed)
        {
            GameObject seedDetailsPane = Instantiate(seedDetailsPrefab.gameObject, contentPane);

            SeedDetails seedDetails = seedDetailsPane.GetComponent<SeedDetails>();
            seedDetails.InitialiseFromSeed(cropTileset, seed);
            seedDetails.Button.onClick.AddListener(() => onSeedSelected.Invoke(seed));

            seedDetailsBySeed.Add(seed, seedDetails);
        }

        public void DeleteSeed(Seed seed)
        {
            if (seedDetailsBySeed.TryGetValue(seed, out SeedDetails seedDetails))
            {
                seedDetails.Button.onClick.RemoveAllListeners();
                Destroy(seedDetails.gameObject);
                seedDetailsBySeed.Remove(seed);
            }
        }
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