using Assets.Scripts.BUCore.UI;
using Assets.Scripts.Seeds;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameInterface.FilterWindow
{
    /// <summary> Manages a list of stat item toggles to represent the stats that the player can choose to add to a filter. </summary>
    public class AvailableStatsPane : MonoBehaviour
    {
        #region Inspector Fields
        [Header("Dependencies")]
        [Tooltip("The element into which the items are placed.")]
        [SerializeField]
        private RectTransform contentObject = null;

        [Tooltip("The component used for selecting the items.")]
        [SerializeField]
        private MultiSelectorGroup multiSelectorGroup = null;

        [Tooltip("The main window.")]
        [SerializeField]
        private FilterWindowController filterWindow = null;

        [Header("Prefabs")]
        [Tooltip("The item created for each stat.")]
        [SerializeField]
        private AvailableStatItem statListItem = null;
        #endregion

        #region Properties
        /// <summary> The component used for selecting the items. </summary>
        public MultiSelectorGroup MultiSelectorGroup => multiSelectorGroup;
        #endregion

        #region List Functions
        /// <summary> Add every stat found within the seed generation of the main filter window. </summary>
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

        /// <summary> Add the given <paramref name="statName"/> as a list item. </summary>
        /// <param name="statName"> The name of the stat. </param>
        public void AddStatItem(string statName)
        {
            // Create the button.
            GameObject statButton = Instantiate(statListItem.gameObject, contentObject);

            // Get the stat item component of the button and initialise it.
            statButton.GetComponent<AvailableStatItem>().CreateFrom(statName);
        }

        /// <summary> Remove the given <paramref name="statItem"/> from the list. </summary>
        /// <param name="statItem"> The stat item to remove. </param>
        public void RemoveStatItem(AvailableStatItem statItem) => Destroy(statItem.gameObject);
        #endregion
    }
}