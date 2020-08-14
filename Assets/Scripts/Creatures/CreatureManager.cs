using Assets.Scripts.Crops;
using Assets.Scripts.Map;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Creatures
{
    public class CreatureManager : MonoBehaviour
    {
        #region Inspector Fields
        [Header("Dependencies")]
        [SerializeField]
        private CropTilemap cropTilemap = null;
        #endregion

        #region Fields

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
        #endregion

        #region Spawn Functions
        public void SpawnCreature(Creature creaturePrefab, int plantX, int plantY, IReadOnlyDictionary<string, float> cropStats)
        {
            
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