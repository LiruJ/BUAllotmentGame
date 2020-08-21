using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameInterface.FilterWindow
{
    public class StatValuePane : MonoBehaviour
    {
        #region Inspector Fields
        [Header("Dependencies")]
        [SerializeField]
        private RectTransform contentObject = null;

        [SerializeField]
        private ToggleGroup toggleGroup = null;

        [Header("Prefabs")]
        [SerializeField]
        private StatValueAdjuster statValueListItem = null;
        #endregion

        #region Fields
        private Dictionary<string, float> scoreFilter = null;
        #endregion

        #region List Functions
        public void Populate(Dictionary<string, float> scoreFilter)
        {
            // Set the score filter.
            this.scoreFilter = scoreFilter;

            // Go over each stat in the filter and add it as a UI item to the list.
            foreach (string statName in scoreFilter.Keys)
                AddStatValueItem(statName);
        }

        public void AddStatValueItem(string statName)
        {
            // Create the item.
            GameObject itemObject = Instantiate(statValueListItem.gameObject, contentObject);

            // Get the stat value list item component from the item.
            itemObject.GetComponent<StatValueAdjuster>().CreateFrom(toggleGroup, statName, scoreFilter);
        }
        #endregion
    }
}