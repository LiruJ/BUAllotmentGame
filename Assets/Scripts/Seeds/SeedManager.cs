using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Seeds
{
    /// <summary> Stores and manages the player's seeds. </summary>
    public class SeedManager : MonoBehaviour
    {
        #region Inspector Fields
        
        #endregion

        #region Fields
        private readonly List<Seed> seeds = new List<Seed>();
        #endregion

        #region Properties
        public IReadOnlyList<Seed> Seeds => seeds;
        #endregion

        #region Events
        [Serializable]
        private class seedEvent : UnityEvent<Seed> { }

        [SerializeField]
        private seedEvent onSeedAdded = new seedEvent();
        #endregion

        #region Initialisation Functions
        private void Start()
        {
            AddSeed(new Seed("Tomato", new Dictionary<string, float>()));
        }

        private void Awake()
        {

        }
        #endregion

        #region Seed Functions
        public void AddSeed(Seed seed)
        {
            seeds.Add(seed);
            onSeedAdded.Invoke(seed);
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