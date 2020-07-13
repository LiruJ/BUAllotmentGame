using Assets.Scripts.BUCore.TileMap;
using Assets.Scripts.Objects;
using UnityEngine;

namespace Assets.Scripts.Tiles.Logic
{
    [CreateAssetMenu(fileName = "New Grass Tile", menuName = "Tilemap/Tiles/Grass")]
    public class GrassTile : FloorTileLogic
    {
        #region Inspector Fields
        [Header("Settings")]
        [Tooltip("The tile onto which the grass can spread.")]
        [SerializeField]
        private string spreadableTile = string.Empty;

        [Tooltip("The grass tile.")]
        [SerializeField]
        private string grassTileName = string.Empty;

        [Tooltip("The chance that the grass will spread to an adjacent tile per tick.")]
        [Range(0, 1)]
        [SerializeField]
        private float spreadChance = 0.001f;
        #endregion

        #region Tile Functions
        public override void OnTilePlaced(BaseTilemap<FloorTileData> tilemap, int x, int y) { }

        public override void OnTileDestroyed(BaseTilemap<FloorTileData> tilemap, int x, int y) { }

        public override void OnTick(BaseTilemap<FloorTileData> tilemap, int x, int y)
        {
            // Get the objects map.
            BaseTilemap<ObjectTileData> objectMap = tilemap.WorldMap.GetTilemap<ObjectTileData>();

            trySpread(tilemap, objectMap, x, y);
        }
        #endregion

        #region Spread Functions
        private void trySpread(BaseTilemap<FloorTileData> tilemap, BaseTilemap<ObjectTileData> objectMap, int x, int y)
        {
            // Try to spread to the directly adjacent tiles, if any of the attempts succeed, don't do the others.
            if (trySpreadToTile(tilemap, objectMap, x, y + 1)) return;
            if (trySpreadToTile(tilemap, objectMap, x + 1, y)) return;
            if (trySpreadToTile(tilemap, objectMap, x, y - 1)) return;
            if (trySpreadToTile(tilemap, objectMap, x - 1, y)) return;
        }

        private bool trySpreadToTile(BaseTilemap<FloorTileData> tilemap, BaseTilemap<ObjectTileData> objectMap, int x, int y)
        {
            // If the tile is in range, is the spreadable material, there's no object at the position, and the random roll was successful, spread the grass and return true.
            if (tilemap.IsTile(x, y, spreadableTile) && objectMap.IsTile(x, y, "Air") && Random.value <= spreadChance)
            {
                tilemap.SetTile(x, y, tilemap.Tileset.GetTileFromName(grassTileName));
                return true;
            }
            // Otherwise; return false as no grass was spread.
            else return false;
        }
        #endregion
    }
}