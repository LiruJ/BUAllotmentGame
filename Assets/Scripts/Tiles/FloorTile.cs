using Assets.Scripts.BUCore.TileMap;
using Assets.Scripts.Objects;
using System;
using UnityEngine;

namespace Assets.Scripts.Tiles
{
    /// <summary> A tile on the floor layer. </summary>
    [Serializable]
    public class FloorTile : Tile<FloorTileData>
    {
        #region Inspector Fields
        [Tooltip("The floor-specific tile logic.")]
        [SerializeField]
        private FloorTileLogic floorTileLogic = null;

        [Tooltip("The material applied to the base floor tile.")]
        [SerializeField]
        private Material material = null;
        #endregion

        #region Properties
        /// <summary> The material applied to the base floor tile. </summary>
        public Material Material => material;
        #endregion

        #region Initialisation Functions
        /// <summary> Is fired when the tileset loads. </summary>
        public override void OnLoaded() => TileLogic = floorTileLogic;
        #endregion

        #region Tile Functions
        /// <summary> Calculates if the tile at the given <paramref name="x"/> and <paramref name="y"/> positions can be replaced with this <see cref="Tile{T}"/>. </summary>
        /// <param name="tilemap"> The tilemap. </param>
        /// <param name="x"> The x axis of the position. </param>
        /// <param name="y"> The y axis of the position. </param>
        /// <returns> True if this tile can be placed, otherwise; false. </returns>
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