using Assets.Scripts.BUCore.TileMap;
using Assets.Scripts.Objects;
using UnityEngine;

namespace Assets.Scripts.Tiles.Logic
{
    [CreateAssetMenu(fileName = "New Spreadable Tile", menuName = "Tilemap/Tiles/Grass")]
    public class SpreadableTile : FloorTileLogic
    {
        #region Inspector Fields
        [Header("Settings")]
        [Tooltip("The tile onto which the tile can spread.")]
        [SerializeField]
        private string spreadableTile = string.Empty;

        [Tooltip("The chance that the tile will spread to an adjacent tile per tick.")]
        [Range(0, 1)]
        [SerializeField]
        private float spreadChance = 0.001f;

        [Tooltip("Is true if the tile can spread diagonally; otherwise, false.")]
        [SerializeField]
        private bool spreadDiagonally = false;
        #endregion

        #region Tile Functions
        public override bool CanPlaceTile(BaseTilemap<FloorTileData> tilemap, Tile<FloorTileData> tile, int x, int y)
        {
            // Get the objects map.
            BaseTilemap<ObjectTileData> objectMap = tilemap.WorldMap.GetTilemap<ObjectTileData>();

            return canPlaceTile(tilemap, objectMap, tile, x, y);
        }

        private bool canPlaceTile(BaseTilemap<FloorTileData> tilemap, BaseTilemap<ObjectTileData> objectMap, Tile<FloorTileData> tile, int x, int y)
        {
            // If the given position is out of range, return false.
            if (!tilemap.IsInRange(x, y)) return false;

            // If the object map hasn't been loaded yet, nothing is on it, so return true.
            if (objectMap == null || objectMap.Tileset == null) return true;

            // Get the object at the given position.

            // Return whether or not the floor is valid
            return !(objectMap.Tileset.GetTileFromIndex(objectMap[x, y].Index) is ObjectTile objectTile) || objectTile.FloorIsValid(tile);
        }

        public override void OnTick(BaseTilemap<FloorTileData> tilemap, int x, int y)
        {
            // Get the objects map.
            BaseTilemap<ObjectTileData> objectMap = tilemap.WorldMap.GetTilemap<ObjectTileData>();

            // Get this tile's name.
            string tileName = tilemap.Tileset.GetTileNameFromIndex(tilemap[x, y].Index);

            // Try to spread to the adjacent tiles.
            trySpread(tilemap, objectMap, tileName, x, y);
        }
        #endregion

        #region Spread Functions
        private void trySpread(BaseTilemap<FloorTileData> tilemap, BaseTilemap<ObjectTileData> objectMap, string tileName, int x, int y)
        {
            // Get the floor tile.
            Tile<FloorTileData> floorTile = tilemap.Tileset.GetTileFromName(tileName);

            // Try to spread to the directly adjacent tiles, if any of the attempts succeed, don't do the others.
            if (trySpreadToTile(tilemap, objectMap, floorTile, x, y + 1)) return;
            if (trySpreadToTile(tilemap, objectMap, floorTile, x + 1, y)) return;
            if (trySpreadToTile(tilemap, objectMap, floorTile, x, y - 1)) return;
            if (trySpreadToTile(tilemap, objectMap, floorTile, x - 1, y)) return;

            // If this tile cannot spread diagonally, return.
            if (!spreadDiagonally) return;

            // Try to spread to the diagonally adjacent tiles.
            if (trySpreadToTile(tilemap, objectMap, floorTile, x + 1, y + 1)) return;
            if (trySpreadToTile(tilemap, objectMap, floorTile, x + 1, y - 1)) return;
            if (trySpreadToTile(tilemap, objectMap, floorTile, x - 1, y - 1)) return;
            if (trySpreadToTile(tilemap, objectMap, floorTile, x - 1, y + 1)) return;
        }

        private bool trySpreadToTile(BaseTilemap<FloorTileData> tilemap, BaseTilemap<ObjectTileData> objectMap, Tile<FloorTileData> tile, int x, int y)
        {
            // If the tile is in range, is the spreadable material, there's no object at the position, and the random roll was successful, spread the grass and return true.
            if (tilemap.IsTile(x, y, spreadableTile) && canPlaceTile(tilemap, objectMap, tile, x, y) && Random.value <= spreadChance)
            {
                tilemap.SetTile(x, y, tile);
                return true;
            }
            // Otherwise; return false as no grass was spread.
            else return false;
        }
        #endregion
    }
}