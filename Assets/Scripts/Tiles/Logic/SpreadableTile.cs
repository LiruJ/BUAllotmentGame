﻿using Assets.Scripts.BUCore.TileMap;
using UnityEngine;

namespace Assets.Scripts.Tiles.Logic
{
    /// <summary> A floor tile that can spread to specific other floor tiles, e.g. grass. </summary>
    [CreateAssetMenu(fileName = "New Spreadable Tile", menuName = "Tilemap/Tiles/Floor/Grass")]
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
        public override void OnTick(BaseTilemap<FloorTileData> tilemap, int x, int y)
        {
            // Try to spread to the adjacent tiles.
            trySpread(tilemap, tilemap.GetTile(x, y), x, y);
        }
        #endregion

        #region Spread Functions
        /// <summary> Attempts to spread to the surrounding 4 adjacent tiles, or all 8 if <see cref="spreadDiagonally"/> is true. </summary>
        /// <param name="tilemap"> The tilemap. </param>
        /// <param name="tile"> The tile that is being spread. </param>
        /// <param name="x"> The central x position. </param>
        /// <param name="y"> The central y position. </param>
        private void trySpread(BaseTilemap<FloorTileData> tilemap, Tile<FloorTileData> tile, int x, int y)
        {
            // If the given tile is null, do nothing.
            if (tile == null) return;

            // Try to spread to the directly adjacent tiles, if any of the attempts succeed, don't do the others.
            if (trySpreadToTile(tilemap, tile, x, y + 1)) return;
            if (trySpreadToTile(tilemap, tile, x + 1, y)) return;
            if (trySpreadToTile(tilemap, tile, x, y - 1)) return;
            if (trySpreadToTile(tilemap, tile, x - 1, y)) return;

            // If this tile cannot spread diagonally, return.
            if (!spreadDiagonally) return;

            // Try to spread to the diagonally adjacent tiles.
            if (trySpreadToTile(tilemap, tile, x + 1, y + 1)) return;
            if (trySpreadToTile(tilemap, tile, x + 1, y - 1)) return;
            if (trySpreadToTile(tilemap, tile, x - 1, y - 1)) return;
            if (trySpreadToTile(tilemap, tile, x - 1, y + 1)) return;
        }

        /// <summary> Tries to spread the given <paramref name="tile"/> to the given <paramref name="x"/> and <paramref name="y"/> positions. </summary>
        /// <param name="tilemap"> The tilemap. </param>
        /// <param name="tile"> The tile that is being spread. </param>
        /// <param name="x"> The x position that is being spread to. </param>
        /// <param name="y"> The y position that is being spread to. </param>
        /// <returns> True if the tile spread, otherwise; false. </returns>
        private bool trySpreadToTile(BaseTilemap<FloorTileData> tilemap, Tile<FloorTileData> tile, int x, int y)
        {
            // If the tile is the spreadable material, can be placed, and the random roll was successful, spread the grass and return true.
            if (tilemap.IsTile(x, y, spreadableTile) && tile.CanPlace(tilemap, x, y) && Random.value <= spreadChance)
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