using Assets.Scripts.BUCore.UI;
using UnityEngine;

namespace Assets.Scripts.GameInterface.FilterWindow
{
    /// <summary> The controller for the pane that manages the list of stats used by a <see cref="Assets.Scripts.Seeds.SeedGeneration"/>'s score filter. </summary>
    public class StatValuePane : MonoBehaviour
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
        private StatValueAdjuster statValueListItem = null;
        #endregion

        #region Properties
        /// <summary> The component used for selecting the items. </summary>
        public MultiSelectorGroup MultiSelectorGroup => multiSelectorGroup;
        #endregion

        #region List Functions
        /// <summary> Add every stat found within the score filter of the main window's current seed generation. </summary>
        public void Populate()
        {
            // Go over each stat in the filter and add it as a UI item to the list.
            foreach (string statName in filterWindow.SeedGeneration.ScoreFilter.Keys)
                AddStatValueItem(statName);
        }

        /// <summary> Add the given <paramref name="statName"/> as a list item. </summary>
        /// <param name="statName"> The name of the stat. </param>
        public void AddStatValueItem(string statName)
        {
            // Create the item.
            GameObject itemObject = Instantiate(statValueListItem.gameObject, contentObject);

            // Get the stat value list item component from the item.
            itemObject.GetComponent<StatValueAdjuster>().CreateFrom(statName, filterWindow);
        }

        /// <summary> Remove the given <paramref name="statValue"/> from the list. </summary>
        /// <param name="statValue"> The stat value item to remove. </param>
        public void RemoveStatValueItem(StatValueAdjuster statValue) => Destroy(statValue.gameObject);
        #endregion
    }
}