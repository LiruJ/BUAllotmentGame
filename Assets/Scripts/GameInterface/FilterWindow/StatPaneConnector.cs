using UnityEngine;

namespace Assets.Scripts.GameInterface.FilterWindow
{
    /// <summary> The controller that allows stats to be added to or removed from the score filter through the use of toggles and buttons. </summary>
    public class StatPaneConnector : MonoBehaviour
    {
        #region Inspector Fields
        [Header("Dependencies")]
        [Tooltip("The pane that holds all of the unused stats that are available to use.")]
        [SerializeField]
        private AvailableStatsPane availableStatsPane = null;

        [Tooltip("The pane that holes all of the currently used stats and their values.")]
        [SerializeField]
        private StatValuePane statValuePane = null;

        [Tooltip("The main window.")]
        [SerializeField]
        private FilterWindowController filterWindow = null;
        #endregion

        #region Button Functions
        /// <summary> Takes all currently selected available stats and adds them to the score filter with a value of 0. </summary>
        public void AssignSelectedAvailableStats()
        {
            // Go over each selected item in the available stats pane.
            foreach (AvailableStatItem statItem in availableStatsPane.MultiSelectorGroup.GetAllSelected<AvailableStatItem>())
            {
                // Get the stat name from the item before it is removed and destroyed.
                string statName = statItem.StatName;

                // Remove the item from the available stats pane.
                availableStatsPane.RemoveStatItem(statItem);

                // Add the stat name to the filter dictionary.
                filterWindow.SeedGeneration.ScoreFilter.Add(statName, 0);

                // Add the item to the value pane.
                statValuePane.AddStatValueItem(statName);
            } 
        }

        /// <summary> Takes all currently selected score filter stats and removes them from the score filter. </summary>
        public void RemoveSelectedValueStats()
        {
            // Go over each selected item in the value pane.
            foreach (StatValueAdjuster statValue in statValuePane.MultiSelectorGroup.GetAllSelected<StatValueAdjuster>())
            {
                // Get the stat name from the item before it is removed and destroyed.
                string statName = statValue.StatName;

                // Remove the item from the value pane.
                statValuePane.RemoveStatValueItem(statValue);

                // Remove the stat from the filter dictionary.
                filterWindow.SeedGeneration.ScoreFilter.Remove(statName);

                // Add the item to the available stats pane.
                availableStatsPane.AddStatItem(statName);
            }
        }
        #endregion
    }
}