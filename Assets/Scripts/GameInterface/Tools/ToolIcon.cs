using Assets.Scripts.GameInterface;
using Assets.Scripts.Player.Tools;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Tools
{
    /// <summary> Handles easy selection of a <see cref="Button"/> and holds a <see cref="ToolType"/>. </summary>
    public class ToolIcon : MonoBehaviour
    {
        #region Inspector Fields
        [Header("Dependencies")]
        [Tooltip("The button that must be clicked in order for this tool to be selected.")]
        [SerializeField]
        private Button button = null;
        #endregion

        #region Properties
        [field: Header("Tool Icon Settings")]
        [field: Tooltip("The type of tool to select when the button is selected.")]
        [field: SerializeField]
        public ToolType ToolType { get; private set; }
        #endregion

        #region Initialisation Functions
        private void Start()
        {
            // Get the parent as an ISelectionBar.
            ISelectionBar<ToolIcon> selectionBar = GetComponentInParent<ISelectionBar<ToolIcon>>();

            // When this button is clicked, invoke the event on the parent selection bar.
            button.onClick.AddListener(() => selectionBar.OnButtonSelected(this));
        }
        #endregion
    }
}