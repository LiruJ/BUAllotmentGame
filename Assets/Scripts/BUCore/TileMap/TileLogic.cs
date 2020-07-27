using UnityEngine;

namespace Assets.Scripts.BUCore.TileMap
{
    /// <summary> Represents the logic that runs on data from a tile and can modify the map. </summary>
    public abstract class TileLogic<T> : ScriptableObject where T : struct, ITileData
    {
        #region Tile Functions
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