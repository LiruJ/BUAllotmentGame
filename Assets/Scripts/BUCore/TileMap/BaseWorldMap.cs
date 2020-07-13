using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.BUCore.TileMap
{
    public class BaseWorldMap : MonoBehaviour
    {
        #region Inspector Fields
        [Header("Dependencies")]
        [Tooltip("The grid object used to orientate the tiles.")]
        [SerializeField]
        protected GridLayout grid = null;

        [Header("Tile Data")]
        [Tooltip("The width of the maps in tiles.")]
        [Range(1, 256)]
        [SerializeField]
        private int width = 16;

        [Tooltip("The height (z axis) of the maps in tiles.")]
        [Range(1, 256)]
        [SerializeField]
        private int height = 16;
        #endregion

        #region Fields
        private readonly Dictionary<Type, object> mapsByType = new Dictionary<Type, object>();
        #endregion

        #region Properties
        /// <summary> The <see cref="GridLayout"/> object used to orientate the tiles </summary>
        public GridLayout Grid => grid;

        /// <summary> The width of the maps in tiles. </summary>
        public int Width => width;

        /// <summary> The height (z axis) of the maps in tiles. </summary>
        public int Height => height;
        #endregion

        #region Tilemap Functions
        public BaseTilemap<T> GetTilemap<T>() where T : ITileData
            => mapsByType.TryGetValue(typeof(T), out object tilemap) ? tilemap as BaseTilemap<T> : null;

        public void RegisterTilemap<T>(BaseTilemap<T> tilemap) where T : ITileData => mapsByType.Add(typeof(T), tilemap);
        #endregion
    }
}