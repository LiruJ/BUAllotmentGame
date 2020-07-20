using Assets.Scripts.Tiles;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.GameInterface.Tiles
{
    public class FloorTileBar : TileBar, ISelectionBar<TileIcon>
    {
        #region Inspector Fields
        [Header("Dependencies")]
        [SerializeField]
        private FloorTileset floorTileset = null;
        #endregion

        #region Events
        [Serializable]
        private class tileEvent : UnityEvent<FloorTile> { }

        [SerializeField]
        private tileEvent onSelected = new tileEvent();
        #endregion

        #region Initialisation Functions
        private void Start()
        {
            createIcons(floorTileset);
        }
        #endregion

        #region Button Functions
        void ISelectionBar<TileIcon>.OnButtonSelected(TileIcon button)
        {
            // Get the object tile with the tile name of the clicked button.
            FloorTile floorTile = floorTileset.GetTileFromName(button.TileName) as FloorTile;

            // Invoke the event.
            onSelected.Invoke(floorTile);
        }
        #endregion
    }
}