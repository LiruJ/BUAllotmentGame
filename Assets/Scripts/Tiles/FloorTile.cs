using Assets.Scripts.BUCore.TileMap;
using Assets.Scripts.Objects;
using System;
using UnityEngine;

namespace Assets.Scripts.Tiles
{
    [Serializable]
    public class FloorTile : Tile<FloorTileData>
    {
        #region Inspector Fields
        [Tooltip("The floor-specific tile logic.")]
        [SerializeField]
        private FloorTileLogic floorTileLogic = null;

        [SerializeField]
        private Material material = null;
        #endregion

        #region Properties
        public Material Material => material;
        #endregion

        #region Initialisation Functions
        /// <summary> Is fired when the tileset loads. </summary>
        public override void OnLoaded() => TileLogic = floorTileLogic;
        #endregion

        #region Tile Functions
        public override bool CanPlace(BaseTilemap<FloorTileData> tilemap, int x, int y)
        {
            // Get the object tilemap.
            BaseTilemap<ObjectTileData> objectMap = tilemap.WorldMap.GetTilemap<ObjectTileData>();

            // If there is an object at this position and it cannot support this type of flooring, return false.
            if (objectMap.GetTile(x, y) is ObjectTile objectTile && !objectTile.FloorIsValid(this)) return false;

            // If the previous checks have passed, return true as the placement is valid.
            return true;
        }
        #endregion
    }
}