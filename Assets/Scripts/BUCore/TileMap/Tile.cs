using UnityEngine;

namespace Assets.Scripts.BUCore.TileMap
{
    /// <summary> Represents a collection of data that will represent a tile within a <see cref="TileMap"/>. </summary>
    /// <typeparam name="T"> The type of <see cref="ITileData"/> that this tile works on. </typeparam>
    public abstract class Tile<T> where T : ITileData
    {
        #region Inspector Fields
        [Tooltip("The name of the tile, used to identify and place them.")]
        [SerializeField]
        private string name = string.Empty;

        [Tooltip("The object that gets placed when this tile is placed down.")]
        [SerializeField]
        private GameObject tileObject = null;
        #endregion

        #region Properties
        /// <summary> The name of the tile, used to identify and place them. </summary>
        public string Name => name;

        /// <summary> The logic associated to this tile. </summary>
        public TileLogic<T> TileLogic { get; protected set; }

        /// <summary> Is true if this tile has an associated <see cref="TileLogic{T}"/>; otherwise, false. </summary>
        public bool HasTileLogic => TileLogic != null;

        /// <summary> The <see cref="GameObject"/> that gets placed when this tile is placed down. </summary>
        public GameObject TileObject => tileObject;
        #endregion

        #region Initialisation Functions
        /// <summary> Is fired when the tile is loaded by a <see cref="TileMap.Tileset"/>. </summary>
        public virtual void OnLoaded() { }
        #endregion
    }
}