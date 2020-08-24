using Assets.Scripts.BUCore.UI;
using UnityEngine;

namespace Assets.Scripts.GameInterface.FilterWindow
{
    public class StatValuePane : MonoBehaviour
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
        private StatValueAdjuster statValueListItem = null;
        #endregion

        #region Properties
        public MultiSelectorGroup MultiSelectorGroup => multiSelectorGroup;
        #endregion

        #region List Functions
        public void Populate()
        {
            // Go over each stat in the filter and add it as a UI item to the list.
            foreach (string statName in filterWindow.SeedGeneration.ScoreFilter.Keys)
                AddStatValueItem(statName);
        }

        public void AddStatValueItem(string statName)
        {
            // Create the item.
            GameObject itemObject = Instantiate(statValueListItem.gameObject, contentObject);

            // Get the stat value list item component from the item.
            itemObject.GetComponent<StatValueAdjuster>().CreateFrom(statName, filterWindow);
        }

        public void RemoveStatValueItem(StatValueAdjuster statValue) => Destroy(statValue.gameObject);
        #endregion
    }
}