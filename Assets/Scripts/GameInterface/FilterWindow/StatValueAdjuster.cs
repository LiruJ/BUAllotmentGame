using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameInterface.FilterWindow
{
    /// <summary> The controller class for the stats used in a filter, controlling the weight value with a slider. </summary>
    public class StatValueAdjuster : MonoBehaviour
    {
        #region Inspector Fields
        [Header("Elements")]
        [Tooltip("The text box that shows the name of the stat.")]
        [SerializeField]
        private Text statNameText = null;

        [Tooltip("The field which displays the current weight value, and allows for changes to be manually input.")]
        [SerializeField]
        private InputField statValueLabel = null;

        [Tooltip("The slider which allows changes to be made to the weight value.")]
        [SerializeField]
        private Slider weightSlider = null;
        #endregion

        #region Properties
        /// <summary> The name of the stat that this controller changes. </summary>
        public string StatName { get; private set; }
        #endregion

        #region Initialisation Functions
        /// <summary> Initialises this stat item adjuster, adjusting the stat with the given <paramref name="statName"/> and using the given <paramref name="filterWindow"/> to access the required seed data. </summary>
        /// <param name="statName"> The name of the stat to be adjusted. </param>
        /// <param name="filterWindow"> The containing filter window. </param>
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