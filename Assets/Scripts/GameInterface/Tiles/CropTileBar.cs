using Assets.Scripts.Crops;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.GameInterface.Tiles
{
    /// <summary> The crop tile selection bar. </summary>
    public class CropTileBar : TileBar, ISelectionBar<TileIcon>
    {
        #region Inspector Fields
        [Header("Dependencies")]
        [Tooltip("The tileset for the crop tiles.")]
        [SerializeField]
        private CropTileset cropTileset = null;
        #endregion

        #region Events
        [Serializable]
        private class tileEvent : UnityEvent<CropTile> { }

        [Header("Events")]
        [Tooltip("Is fired when a tile is selected.")]
        [SerializeField]
        private tileEvent onSelected = new tileEvent();
        #endregion

        #region Initialisation Functions
        private void Start() => createIcons(cropTileset);
        #endregion

        #region Button Functions
        void ISelectionBar<TileIcon>.OnButtonSelected(TileIcon button)
        {
            // Invoke the event, logging an error if the tile is invalid.
            if (cropTileset.GetTileFromName(button.TileName) is CropTile cropTile) onSelected.Invoke(cropTile);
            else Debug.LogError($"Invalid crop tile from tile with name: {button.TileName}", this);
        }
        #endregion
    }
}