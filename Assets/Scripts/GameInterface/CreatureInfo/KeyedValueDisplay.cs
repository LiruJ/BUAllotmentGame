using TMPro;
using UnityEngine;

namespace Assets.Scripts.GameInterface.CreatureInfo
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class KeyedValueDisplay : MonoBehaviour, IKeyedValueDisplay
    {
        #region Inspector Fields
        [Header("Settings")]
        [Tooltip("The key of the value to display.")]
        [SerializeField]
        private string key = string.Empty;

        [Tooltip("The text added to the start of the value.")]
        [SerializeField]
        private string prefix = string.Empty;

        [Tooltip("The text added to the end of the value.")]
        [SerializeField]
        private string suffix = string.Empty;

        [Tooltip("The number of decimal places to use.")]
        [Range(0, 6)]
        [SerializeField]
        private byte decimalPlaces = 4;
        #endregion

        #region Fields
        private TextMeshProUGUI text = null;
        #endregion

        #region Properties
        public string Key => key;
        
        public float Value { set => text.text = $"{prefix}{string.Format($"{{0:N{decimalPlaces}}}", value)}{suffix}"; }
        #endregion

        #region Initialisation Functions
        private void Awake() => text = GetComponent<TextMeshProUGUI>();
        #endregion
    }
}