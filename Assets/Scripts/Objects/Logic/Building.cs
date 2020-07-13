using Assets.Scripts.BUCore.TileMap;
using Assets.Scripts.Tiles;
using UnityEngine;

namespace Assets.Scripts.Objects.Logic
{
    [CreateAssetMenu(fileName = "New Building", menuName = "Tilemap/Objects/Building")]
    public class Building : ObjectTileLogic
    {
        #region Inspector Fields
        [Header("Settings")]
        [SerializeField]
        private string objectName = string.Empty;

        [SerializeField]
        protected string requiredFloor = string.Empty;

        [Range(1, 16)]
        [SerializeField]
        protected int width = 1;

        [Range(1, 16)]
        [SerializeField]
        protected int height = 1;
        #endregion

        #region Tile Functions
        public override bool CanPlaceTile(BaseTilemap<ObjectTileData> tilemap, Tile<ObjectTileData> tile, int x, int y)
        {
            // Get the floor map to check for placeable floor tiles.
            BaseTilemap<FloorTileData> floorMap = tilemap.WorldMap.GetTilemap<FloorTileData>();

            // Is true if a certain floor type is required.
            bool checkFloor = !string.IsNullOrWhiteSpace(requiredFloor);

            // Check each tile in the area of the object.
            for (int checkX = x; checkX < x + width; checkX++)
                for (int checkY = y; checkY < y + height; checkY++)
                    // If the area is not empty on the object map, or the floor is not valid on the floor map, return false.
                    if (!tilemap.IsTile(checkX, checkY, "Air") || (checkFloor && !floorMap.IsTile(checkX, checkY, requiredFloor))) return false;

            // Since every checked tile was valid, the area is valid, so return true.
            return true;
        }

        public override void OnTilePlaced(BaseTilemap<ObjectTileData> tilemap, int x, int y)
        {
            ushort index = tilemap.Tileset.GetTileIndexFromName(objectName);

            for (int placeX = x; placeX < x + width; placeX++)
                for (int placeY = y; placeY < y + height; placeY++)
                    tilemap[placeX, placeY] = new FloorTileData() { Index = index };
        }

        public override void OnTileDestroyed(BaseTilemap<ObjectTileData> tilemap, int x, int y) { }

        public override void OnTick(BaseTilemap<ObjectTileData> tilemap, int x, int y) { }
        #endregion
    }
}