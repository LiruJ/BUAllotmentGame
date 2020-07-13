using UnityEngine;

namespace Assets.Scripts.BUCore.TileMap
{
    /// <summary> Represents the logic that runs on data from a tile and can modify the map. </summary>
    public abstract class TileLogic<T> : ScriptableObject where T : ITileData
    {
        #region Tile Functions
        /// <summary> Queries the given <paramref name="tile"/> against the given <paramref name="tilemap"/> using the given position, and checks that the tile can be placed. </summary>
        /// <param name="tilemap"> The <see cref="BaseTilemap{T}"/> against which the <paramref name="tile"/> is being queried. </param>
        /// <param name="tile"></param>
        /// <param name="x"> The x co-ordinate of the position. </param>
        /// <param name="y"> The y co-ordinate of the position. </param>
        /// <returns> True if the tile can be placed; otherwise, false. </returns>
        public virtual bool CanPlaceTile(BaseTilemap<T> tilemap, Tile<T> tile, int x, int y) => true;

        /// <summary> Is fired immediately after a tile is placed. </summary>
        /// <param name="tilemap"> The <see cref="BaseTilemap{T}"/> on which this tile now resides. </param>
        /// <param name="x"> The x co-ordinate of the position. </param>
        /// <param name="y"> The y co-ordinate of the position. </param>
        public virtual void OnTilePlaced(BaseTilemap<T> tilemap, int x, int y) { }

        /// <summary> Is fired immediately after a tile is destroyed. </summary>
        /// <param name="tilemap"> The <see cref="BaseTilemap{T}"/> on which this tile resided. </param>
        /// <param name="x"> The x co-ordinate of the position. </param>
        /// <param name="y"> The y co-ordinate of the position. </param>
        public virtual void OnTileDestroyed(BaseTilemap<T> tilemap, int x, int y) { }

        /// <summary> Is fired every tick of the given <paramref name="tilemap"/>. </summary>
        /// <param name="tilemap"> The <see cref="BaseTilemap{T}"/> on which this tile resides. </param>
        /// <param name="x"> The x co-ordinate of the position. </param>
        /// <param name="y"> The y co-ordinate of the position. </param>
        public virtual void OnTick(BaseTilemap<T> tilemap, int x, int y) { }
        #endregion
    }
}