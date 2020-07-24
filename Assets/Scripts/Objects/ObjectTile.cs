using Assets.Scripts.BUCore.TileMap;
using Assets.Scripts.Tiles;
using System;
using UnityEngine;

namespace Assets.Scripts.Objects
{
    /// <summary> A tile from an object. </summary>
    [Serializable]
    public class ObjectTile : Tile<ObjectTileData>
    {
        #region Inspector Fields
        [Tooltip("The object-specific logic.")]
        [SerializeField]
        private ObjectTileLogic objectTileLogic = null;
        
        [Tooltip("The type of flooring required for this object's foundations, or empty if no specific floor type is required.")]
        [SerializeField]
        protected string requiredFloor = string.Empty;

        [Tooltip("The width (x axis) of the object in tiles.")]
        [Range(1, 16)]
        [SerializeField]
        protected int width = 1;

        [Tooltip("The height (z axis) of the object in tiles.")]
        [Range(1, 16)]
        [SerializeField]
        protected int height = 1;
        #endregion

        #region Properties
        /// <summary> The type of flooring required for this object's foundations, or empty if no specific floor type is required. </summary>
        public string RequiredFloor => requiredFloor;

        /// <summary> The width (x axis) of the object in tiles. </summary>
        public int Width => width;

        /// <summary> The height (z axis) of the object in tiles. </summary>
        public int Height => height;
        #endregion

        #region Initialisation Functions
        public override void OnLoaded() => TileLogic = objectTileLogic;
        #endregion

        #region Tile Functions
        /// <summary> Calculates if the given tile position has no other objects and a valid floor. </summary>
        /// <param name="tilemap"> The tilemap. </param>
        /// <param name="x"> The x axis of the position. </param>
        /// <param name="y"> The y axis of the position. </param>
        /// <returns> True if this tile can be placed, otherwise; false. </returns>
        public override bool CanPlace(BaseTilemap<ObjectTileData> tilemap, int x, int y)
        {
            // Check each tile in the area of the object.
            for (int checkX = x; checkX < x + Width; checkX++)
                for (int checkY = y; checkY < y + Height; checkY++)
                    // If the area is not empty on the object map, or the floor is not valid on the floor map, return false.
                    if (!TileIsValid(tilemap, checkX, checkY)) return false;

            // If the other checks have passed, return true.
            return true;
        }

        /// <summary> Calculates if the given specific tile at the <paramref name="x"/> and <paramref name="y"/> position is valid for this object's placement. Note that this does not necessarily mean that the entire object can be placed. </summary>
        /// <param name="objectMap"> The tilemap. </param>
        /// <param name="x"> The x axis of the position. </param>
        /// <param name="y"> The y axis of the position. </param>
        /// <returns> True if the tile is valid, otherwise; false. </returns>
        public bool TileIsValid(BaseTilemap<ObjectTileData> objectMap, int x, int y)
        {
            // If this tile has an object already, return false.
            if (!objectMap.IsTileEmpty(x, y)) return false;

            // If the floor is not valid, return false.
            if (!FloorIsValid(objectMap.WorldMap.GetTilemap<FloorTileData>(), x, y)) return false;

            // If the other checks have passed, return true.
            return true;
        }

        /// <summary> Calculates if the <see cref="Tile{T}"/> at the given <paramref name="x"/> and <paramref name="y"/> positions is a valid floor compared to this <see cref="Tile{T}"/>'s <see cref="RequiredFloor"/>. </summary>
        /// <param name="floorMap"> The tilemap of the floor. </param>
        /// <param name="x"> The x axis of the position. </param>
        /// <param name="y"> The y axis of the position. </param>
        /// <returns> True if the tile is a valid floor, otherwise; false. </returns>
        public bool FloorIsValid(BaseTilemap<FloorTileData> floorMap, int x, int y) => FloorIsValid(floorMap.GetTile(x, y));

        /// <summary> Calculates if the given <paramref name="floorTile"/> is a valid floor compared to this <see cref="Tile{T}"/>'s <see cref="RequiredFloor"/>. </summary>
        /// <param name="floorTile"> The floor tile against which to check. </param>
        /// <returns> True if the tile is a valid floor, otherwise; false. </returns>
        public bool FloorIsValid(Tile<FloorTileData> floorTile) => string.IsNullOrWhiteSpace(requiredFloor) || floorTile.Name == requiredFloor;
        #endregion
    }
}