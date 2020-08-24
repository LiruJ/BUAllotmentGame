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
        private Text statNameText = null;

        [SerializeField]
        private InputField statValueLabel = null;

        [SerializeField]
        private Slider weightSlider = null;
        #endregion

        #region Properties
        public string StatName { get; private set; }
        #endregion

        #region Initialisation Functions
        public void CreateFrom(string statName, FilterWindowController filterWindow)
        {
            // Set the stat name.
            StatName = statName;

            // Set the stat name label.
            statNameText.text = statName;

            // If the score filter does not have this stat, log an error.
            if (!filterWindow.SeedGeneration.ScoreFilter.TryGetValue(statName, out float weightValue))
            {
                Debug.LogError($"Score filter dictionary was missing {statName} stat.");
                return;
            }

            // Set the value of the slider to the value of the weight.
            weightSlider.value = weightValue;

            // Bind the slider changing to change the weight value.
            weightSlider.onValueChanged.AddListener((newValue) =>
            {
                filterWindow.SeedGeneration.ScoreFilter[statName] = newValue;
                statValueLabel.text = newValue.ToString();
            });

            // Bind the text input field's end edit event to change the slider, which then changes the filter value.
            statValueLabel.text = weightValue.ToString();
            statValueLabel.onEndEdit.AddListener((stringValue) =>
            {
                if (float.TryParse(stringValue, out float newWeight)) weightSlider.value = newWeight;
            });

        }
        #endregion
    }
}