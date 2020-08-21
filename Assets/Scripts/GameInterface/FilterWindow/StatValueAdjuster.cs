using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameInterface.FilterWindow
{
    public class StatValueAdjuster : MonoBehaviour
    {
        #region Inspector Fields
        [Header("Elements")]
        [SerializeField]
        private Toggle toggle = null;

        [SerializeField]
        private Text statNameText = null;

        [SerializeField]
        private Text statValueLabel = null;

        [SerializeField]
        private Slider weightSlider = null;
        #endregion

        #region Properties
        public string StatName { get; private set; }
        #endregion

        #region Initialisation Functions
        public void CreateFrom(ToggleGroup toggleGroup, string statName, Dictionary<string, float> scoreFilter)
        {
            // Set the toggle group of the toggle button.
            toggle.group = toggleGroup;

            // Set the stat name.
            StatName = statName;

            // If the score filter does not have this stat, initialise it to 0.
            if (!scoreFilter.TryGetValue(statName, out float weightValue))
            {
                weightValue = 0;
                scoreFilter.Add(statName, weightValue);
            }

            // Set the value of the slider to the value of the weight.
            weightSlider.value = weightValue;

            // Bind the slider changing to change the weight value.
            weightSlider.onValueChanged.AddListener((newValue) => scoreFilter[statName] = newValue);
        }
        #endregion
    }
}