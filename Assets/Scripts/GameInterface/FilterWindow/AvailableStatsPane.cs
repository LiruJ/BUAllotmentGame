using Assets.Scripts.BUCore.UI;
using Assets.Scripts.Seeds;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameInterface.FilterWindow
{
    public class AvailableStatsPane : MonoBehaviour
    {
        #region Inspector Fields
        [Header("Dependencies")]
        [SerializeField]
        private RectTransform contentObject = null;

        [SerializeField]
        private MultiSelectorGroup multiSelectorGroup = null;

        [SerializeField]
        private FilterWindowController filterWindow = null;

        [Header("Prefabs")]
        [SerializeField]
        private AvailableStatItem statListItem = null;
        #endregion

        #region Properties
        public MultiSelectorGroup MultiSelectorGroup => multiSelectorGroup;
        #endregion

        #region List Functions
        public void Populate()
        {
            // Create a hashset to hold each unique stat of the seed generation.
            HashSet<string> uniqueStats = new HashSet<string>();

            // Go over each seed in the generation, adding each stat to the hash set.
            foreach (Seed seed in filterWindow.SeedGeneration.UnsortedSeeds)
                foreach (string statKey in seed.LifetimeStats.Keys)
                    if (!uniqueStats.Contains(statKey)) uniqueStats.Add(statKey);

            // By now, the hashset contains every single stat of every seed, so turn each one into a button.
            foreach (string statName in uniqueStats)
                AddStatItem(statName);
        }

        public void AddStatItem(string statName)
        {
            // Create the button.
            GameObject statButton = Instantiate(statListItem.gameObject, contentObject);

            // Get the stat item component of the button and initialise it.
            statButton.GetComponent<AvailableStatItem>().CreateFrom(statName);
        }

        public void RemoveStatItem(AvailableStatItem statItem) => Destroy(statItem.gameObject);
        #endregion
    }
}