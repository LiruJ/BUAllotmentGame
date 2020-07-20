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
        public bool TileIsValid(BaseTilemap<ObjectTileData> objectMap, BaseTilemap<FloorTileData> floorMap, int x, int y, bool checkFloor, string requiredFloor)
            => objectMap.IsTileEmpty(x, y) && (!checkFloor || FloorIsValid(floorMap, x, y));

        public bool FloorIsValid(BaseTilemap<FloorTileData> floorMap, int x, int y) => FloorIsValid(floorMap.Tileset.GetTileFromIndex(floorMap[x, y].Index));

        public bool FloorIsValid(Tile<FloorTileData> floorTile) => string.IsNullOrWhiteSpace(requiredFloor) || floorTile.Name == requiredFloor;
        #endregion
    }
}