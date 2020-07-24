using Assets.Scripts.BUCore.TileMap;
using Assets.Scripts.Tiles;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.GameInterface.Tiles
{
    /// <summary> The floor tile selection bar. </summary>
    public class FloorTileBar : TileBar, ISelectionBar<TileIcon>
    {
        #region Inspector Fields
        [Header("Dependencies")]
        [Tooltip("The tileset for the floor tiles.")]
        [SerializeField]
        private FloorTileset floorTileset = null;
        #endregion

        #region Events
        [Serializable]
        private class tileEvent : UnityEvent<FloorTile> { }

        [Header("Events")]
        [Tooltip("Is fired when a tile is selected.")]
        [SerializeField]
        private tileEvent onSelected = new tileEvent();
        #endregion

        #region Initialisation Functions
        private void Start() => createIcons(floorTileset);

        protected override void initialiseTileIcon<T>(Tile<T> tile, TileIcon tileIcon)
        {
            // If the tile is valid, set the texture of the icon to the texture from the tile, otherwise log an error.
            if (tile is FloorTile floorTile) tileIcon.GetComponent<RawImage>().texture = floorTile.Material.mainTexture;
            else Debug.LogError($"Invalid floor tile from tile: {tile}", this);
        }
        #endregion

        #region Button Functions
        void ISelectionBar<TileIcon>.OnButtonSelected(TileIcon button)
        {
            // Invoke the event, logging an error if the tile is invalid.
            if (floorTileset.GetTileFromName(button.TileName) is FloorTile floorTile) onSelected.Invoke(floorTile);
            else Debug.LogError($"Invalid floor tile from tile with name: {button.TileName}", this);
        }
        #endregion
    }
}