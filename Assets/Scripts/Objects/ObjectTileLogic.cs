using Assets.Scripts.BUCore.TileMap;
using Assets.Scripts.Tiles;
using UnityEngine;

namespace Assets.Scripts.Objects
{
    public abstract class ObjectTileLogic : TileLogic<ObjectTileData> 
    {
        #region Tile Functions
        public override bool CanPlaceTile(BaseTilemap<ObjectTileData> tilemap, Tile<ObjectTileData> tile, int x, int y)
        {
            // Get the floor map to check for placeable floor tiles.
            BaseTilemap<FloorTileData> floorMap = tilemap.WorldMap.GetTilemap<FloorTileData>();

            if (floorMap == null || !(tile is ObjectTile objectTile)) { Debug.LogError("Invalid object tile state.", this); return false; }

            // Is true if a certain floor type is required.
            bool checkFloor = !string.IsNullOrWhiteSpace(objectTile.RequiredFloor);

            // Check each tile in the area of the object.
            for (int checkX = x; checkX < x + objectTile.Width; checkX++)
                for (int checkY = y; checkY < y + objectTile.Height; checkY++)
                    // If the area is not empty on the object map, or the floor is not valid on the floor map, return false.
                    if (!objectTile.TileIsValid(tilemap, floorMap, checkX, checkY, checkFloor, objectTile.RequiredFloor)) return false;

            // Since every checked tile was valid, the area is valid, so return true.
            return true;
        }
        #endregion
    }
}
