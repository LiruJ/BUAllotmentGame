using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameInterface.Tiles
{
    /// <summary> Handles easy selection of a <see cref="Toggle"/> within a <see cref="ToggleGroup"/>. </summary>
    public class TileIcon : MonoBehaviour
    {
        #region Properties
        /// <summary> The name of the tile that this icon represents. </summary>
        public string TileName { get; set; }
        #endregion

        #region Initialisation Functions
        private void Start()
        {
            // Get the parent as an ISelectionBar.
            ISelectionBar<TileIcon> selectionBar = GetComponentInParent<ISelectionBar<TileIcon>>();
            
            // Get the toggle component of the GameObject.
            Toggle toggle = GetComponent<Toggle>();

            // When the value changes and this toggle becomes selected, call the function on the selection bar.
            toggle.onValueChanged.AddListener((selected) => { if (selected) selectionBar.OnButtonSelected(this); });

            // Set the toggle group of the toggle.
            toggle.group = GetComponentInParent<ToggleGroup>();
        }
        #endregion
    }
}