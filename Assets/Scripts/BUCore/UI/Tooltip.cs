using TMPro;
using UnityEngine;

namespace Assets.Scripts.BUCore.UI
{
    public class Tooltip : MonoBehaviour
    {
        #region Inspector Fields
        [Header("Dependencies")]
        [SerializeField]
        private TextMeshProUGUI text = null;
        #endregion

        #region Tooltip Function
        public void Show(string tooltipText)
        {
            // Set the tooltip text.
            text.text = tooltipText;

            // Show the tooltip.
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            // Clear the tooltip text.
            text.text = string.Empty;

            // Hide the tooltip.
            gameObject.SetActive(false);
        }
        #endregion

        #region Update Functions
        private void LateUpdate()
        {
            // Known broken if the canvas is not on Screen Space - Overlay. Other ways of doing this were way more complex, though.
            if (gameObject.activeSelf)
                transform.position = Input.mousePosition;
        }
        #endregion
    }
}