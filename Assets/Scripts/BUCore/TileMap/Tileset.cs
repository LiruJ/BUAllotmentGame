using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.BUCore.TileMap
{
    /// <summary> Holds a list of tiles correlating to names and indices, used by a <see cref="BaseTilemap{T}"/>. </summary>
    /// <typeparam name="T"> The type of <see cref="ITileData"/> to store. </typeparam>
    public abstract class Tileset<T> : ScriptableObject where T : struct, ITileData
    {
        #region Inspector Fields
        [Header("Settings")]
        [Tooltip("The name of the tile to use when a tile has an index of 0.")]
        [SerializeField]
        private string emptyTileName = "Empty";
        #endregion

        #region Fields
        /// <summary> Stores the indices of each tile keyed by name. </summary>
        private readonly Dictionary<string, ushort> tileIndicesByName = new Dictionary<string, ushort>();

        /// <summary> Stores the tiles keyed by index. </summary>
        private readonly Dictionary<ushort, Tile<T>> tilesByIndex = new Dictionary<ushort, Tile<T>>();

        /// <summary> Stores the tiles keyed by name. </summary>
        private readonly Dictionary<string, Tile<T>> tilesByName = new Dictionary<string, Tile<T>>();
        #endregion

        #region Properties
        /// <summary> The name of the tile to use when a tile has an index of 0. </summary>
        public string EmptyTileName => emptyTileName;
        #endregion

        #region Initialisation Functions
        /// <summary> Intialises the tileset using the given <paramref name="tiles"/>. </summary>
        /// <param name="tiles"> The list of tiles to initialise from. </param>
        protected void fromTileList(IReadOnlyList<Tile<T>> tiles)
        {
            // This will eventually be tracked outside of the start loop, so that tiles from different sets can be loaded together.
            ushort currentIndex = 1;

            // Go over each tile in the set and add them to the dictionaries.
            foreach (Tile<T> tile in tiles)
            {
                // If this tile's name is the same as the empty tile, log a warning and skip it.
                if (tile.Name == emptyTileName) { Debug.LogWarning("Cannot define tile with same name as empty tile.", this); continue; }

                // Add the tile to each collection.
                tileIndicesByName.Add(tile.Name, currentIndex);
                tilesByIndex.Add(currentIndex, tile);
                tilesByName.Add(tile.Name, tile);

                // Call the loaded function on the tile so that it can initialise.
                tile.OnLoaded();

                // Increment the index.
                currentIndex++;
            }
        }
        #endregion

        #region Get Functions
        /// <summary> Gets the index of the <see cref="Tile{T}"/> with the given <paramref name="name"/>. </summary>
        /// <param name="name"> The name of the <see cref="Tile{T}"/>. </param>
        /// <returns> The index of the <see cref="Tile{T}"/> with the given <paramref name="name"/>. </returns>
        public ushort GetTileIndexFromName(string name) => name == emptyTileName ? (ushort)0 : tileIndicesByName[name];

        /// <summary> Gets the name of the <see cref="Tile{T}"/> with the given <paramref name="index"/>. </summary>
        /// <param name="index"> The index of the <see cref="Tile{T}"/>. </param>
        /// <returns> The name of the <see cref="Tile{T}"/> with the given <paramref name="index"/>. </returns>
        public string GetTileNameFromIndex(ushort index) => index == 0 ? emptyTileName : tilesByIndex[index].Name;

        /// <summary> Gets the <see cref="Tile{T}"/> with the given <paramref name="name"/>. </summary>
        /// <param name="name"> The name of the <see cref="Tile{T}"/>. </param>
        /// <returns> The <see cref="Tile{T}"/> with the given <paramref name="name"/>. </returns>
        public Tile<T> GetTileFromName(string name) => tilesByName.TryGetValue(name, out Tile<T> tile) ? tile : null;

        /// <summary> Gets the <see cref="Tile{T}"/> with the given <paramref name="index"/>. </summary>
        /// <param name="index"> The index of the <see cref="Tile{T}"/>. </param>
        /// <returns> The <see cref="Tile{T}"/> with the given <paramref name="index"/>. </returns>
        public Tile<T> GetTileFromIndex(ushort index) => tilesByIndex.TryGetValue(index, out Tile<T> tile) ? tile : null;
        #endregion
    }
}