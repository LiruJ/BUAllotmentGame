using Assets.Scripts.BUCore.TileMap;
using Assets.Scripts.Objects;
using Assets.Scripts.Tiles;
using System;
using UnityEngine;

namespace Assets.Scripts.Crops
{
    /// <summary> A tile from a crop. </summary>
    [Serializable]
    public class CropTile : Tile<CropTileData>
    {
        #region Inspector Fields
        [Tooltip("The crop-specific logic.")]
        [SerializeField]
        private CropTileLogic cropTileLogic = null;

        [Tooltip("The type of flooring required for this crop's roots, or empty if no specific floor type is required.")]
        [SerializeField]
        protected string requiredFloor = string.Empty;

        [Tooltip("The icon used to represent this crop.")]
        [SerializeField]
        private Sprite icon = null;
        #endregion

        #region Properties
        public Sprite Icon => icon;
        #endregion

        #region Initialisation Functions
        public override void OnLoaded() => TileLogic = cropTileLogic;
        #endregion

        #region Tile Functions
        /// <summary> Calculates if the given tile position has no objects and a valid floor. </summary>
        /// <param name="tilemap"> The tilemap. </param>
        /// <param name="x"> The x axis of the position. </param>
        /// <param name="y"> The y axis of the position. </param>
        /// <returns> True if this tile can be placed, otherwise; false. </returns>
        public override bool CanPlace(BaseTilemap<CropTileData> tilemap, int x, int y)
        {
            // If this tile has a crop on it already, return false.
            if (!tilemap.IsTileEmpty(x, y)) return false;
            
            // If the floor is not valid, return false.
            if (!string.IsNullOrWhiteSpace(requiredFloor) && !tilemap.WorldMap.GetTilemap<FloorTileData>().IsTile(x, y, requiredFloor)) return false;

            // If this tile has an object, return false.
            if (!tilemap.WorldMap.GetTilemap<ObjectTileData>().IsTileEmpty(x, y)) return false;

            // If the other checks have passed, return true.
            return true;
        }
        #endregion
    }
}