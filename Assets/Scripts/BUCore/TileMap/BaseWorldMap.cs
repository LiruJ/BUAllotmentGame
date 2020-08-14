using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.BUCore.TileMap
{
    /// <summary> The base world map, allowing for <see cref="BaseTilemap{T}"/>s to be registered. </summary>
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
        /// <summary> The <see cref="BaseTilemap{T}"/>s keyed by <see cref="Type"/>. </summary>
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
        /// <summary> Get the registered <see cref="BaseTilemap{T}"/> with the given <typeparamref name="T"/>. </summary>
        /// <typeparam name="T"> The <see cref="Type"/> of the <see cref="BaseTilemap{T}"/>. </typeparam>
        /// <returns> The <see cref="BaseTilemap{T}"/> with the matching <see cref="Type"/>. </returns>
        public BaseTilemap<T> GetTilemap<T>() where T : struct, ITileData
            => mapsByType.TryGetValue(typeof(T), out object tilemap) ? tilemap as BaseTilemap<T> : findAndRegisterTilemap<T>();

        /// <summary> Finds the <see cref="BaseTilemap{T}"/> with the given <typeparamref name="T"/> from the <see cref="GameObject"/>'s children of this behaviour, and registers it. </summary>
        /// <typeparam name="T"> The <see cref="Type"/> of the <see cref="BaseTilemap{T}"/>. </typeparam>
        /// <returns> The <see cref="BaseTilemap{T}"/> with the matching <see cref="Type"/>, or null if none was found. </returns>
        private BaseTilemap<T> findAndRegisterTilemap<T>() where T : struct, ITileData
        {
            // Get the tilemap within the children of this GameObject.
            BaseTilemap<T> tilemap = GetComponentInChildren<BaseTilemap<T>>();

            // If a tilemap was found, register it.
            if (tilemap != null) RegisterTilemap(tilemap);

            // Return the tilemap, which could be null if none was found.
            return tilemap;
        }

        /// <summary> Registers the given <paramref name="tilemap"/> keyed by the given <typeparamref name="T"/>. </summary>
        /// <typeparam name="T"> The <see cref="Type"/> of the <see cref="BaseTilemap{T}"/>. </typeparam>
        public void RegisterTilemap<T>(BaseTilemap<T> tilemap) where T : struct, ITileData
        {
            // If the given tilemap is null, do nothing.
            if (tilemap == null) return;

            // Add the tilemap keyed by the given type.
            mapsByType.Add(typeof(T), tilemap);
        }
        #endregion
    }
}