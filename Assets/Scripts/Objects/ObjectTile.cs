using Assets.Scripts.BUCore.TileMap;
using Assets.Scripts.Tiles;
using System;
using UnityEngine;

namespace Assets.Scripts.Objects
{
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
        public string RequiredFloor => requiredFloor;

        public int Width => width;

        public int Height => height;
        #endregion

        #region Initialisation Functions
        public override void OnLoaded() => TileLogic = objectTileLogic;
        #endregion

        #region Tile Functions
        /// <summary> Calculates if the given tile position has no other objects and a valid floor. </summary>
        /// <param name="tilemap"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
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

        public bool TileIsValid(BaseTilemap<ObjectTileData> objectMap, int x, int y)
        {
            // If this tile has an object already, return false.
            if (!objectMap.IsTileEmpty(x, y)) return false;

            // If the floor is not valid, return false.
            if (!FloorIsValid(objectMap.WorldMap.GetTilemap<FloorTileData>(), x, y)) return false;

            // If the other checks have passed, return true.
            return true;
        }

        public bool FloorIsValid(BaseTilemap<FloorTileData> floorMap, int x, int y) => FloorIsValid(floorMap.GetTile(x, y));

        public bool FloorIsValid(Tile<FloorTileData> floorTile) => string.IsNullOrWhiteSpace(requiredFloor) || floorTile.Name == requiredFloor;
        #endregion
    }
}