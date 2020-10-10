using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.BUCore.UI
{
    public class TooltipActivator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        #region Inspector Fields
        [Header("Dependencies")]
        [Tooltip("The single tooltip object in the canvas.")]
        [SerializeField]
        private Tooltip tooltip = null;

        [Header("Settings")]
        [Tooltip("What the tooltip should say.")]
        [SerializeField]
        private string tooltipText = string.Empty;
        #endregion

        #region Properties
        /// <summary> The single tooltip object in the canvas. </summary>
        public Tooltip Tooltip { get => tooltip; set => tooltip = value; }
        #endregion

        #region Pointer Functions
        public void OnPointerEnter(PointerEventData eventData) => tooltip.Show(tooltipText);

        public void OnPointerExit(PointerEventData eventData) => tooltip.Hide();
        #endregion
    }
}