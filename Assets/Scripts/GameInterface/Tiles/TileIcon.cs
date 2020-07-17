using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameInterface.Tiles
{
    public class TileIcon : MonoBehaviour
    {
        #region Properties
        public string TileName { get; set; }
        #endregion

        #region Initialisation Functions
        private void Start()
        {
            ISelectionBar<TileIcon> selectionBar = GetComponentInParent<ISelectionBar<TileIcon>>();
            Toggle toggle = GetComponent<Toggle>();
            
            toggle.onValueChanged.AddListener((selected) => { if (selected) selectionBar.OnButtonSelected(this); });
            toggle.group = GetComponentInParent<ToggleGroup>();
        }
        #endregion
    }
}