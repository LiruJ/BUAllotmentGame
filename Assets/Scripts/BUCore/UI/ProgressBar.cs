using UnityEngine;

namespace Assets.Scripts.BUCore.UI
{
    public class ProgressBar : MonoBehaviour
    {
        #region Inspector Fields
        [Header("Dependencies")]
        [Tooltip("The bar that is resized to show progress.")]
        [SerializeField]
        private RectTransform bar = null;

        [Header("Settings")]
        [Tooltip("The maximum value of the progress bar.")]
        [SerializeField]
        private float max = 1;

        [Tooltip("The current progress.")]
        [SerializeField]
        private float progress = 0;
        #endregion

        #region Properties
        public float Max { get => max; set => max = value; }

        public float Progress
        {
            get => progress;
            set
            {
                progress = Mathf.Clamp(value, 0, Max);

                // Resize the bar to represent the value. This has an annoying warning message when used in the editor, but according to the unity forums this bug will be fixed in 2016, so we just have to wait -4 years.
                if (bar != null) bar.anchorMax = new Vector2(Progress / Max, bar.anchorMax.y);
            }
        }
        #endregion

        #region Initialisation Functions
        private void Start()
        {
            if (bar == null) Debug.LogError("Progress bar is missing bar reference.", this);
            else Progress = progress;
        }
        #endregion

        #region Validation Functions
        private void OnValidate()
        {
            max = Mathf.Max(max, 0.001f);
            Progress = progress;
        }
        #endregion
    }
}