using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.BUCore.TileMap
{
    public abstract class Tileset<T> : ScriptableObject where T : ITileData
    {
        #region Inspector Fields
        [Header("Settings")]
        [SerializeField]
        private string emptyTileName = "Empty";
        #endregion

        #region Fields
        private readonly Dictionary<string, ushort> tileIndicesByName = new Dictionary<string, ushort>();

        private readonly Dictionary<ushort, Tile<T>> tilesByIndex = new Dictionary<ushort, Tile<T>>();

        private readonly Dictionary<string, Tile<T>> tilesByName = new Dictionary<string, Tile<T>>();
        #endregion

        #region Properties
        public string EmptyTileName => emptyTileName;
        #endregion

        #region Initialisation Functions
        protected void fromTileList<TileType>(List<TileType> tiles) where TileType : Tile<T>
        {
            // This will eventually be tracked outside of the start loop, so that tiles from different sets can be loaded together.
            ushort currentIndex = 1;

            // Go over each tile in the set and add them to the dictionaries.
            foreach (Tile<T> tile in tiles)
            {
                // If this tile's name is the same as the empty tile, log a warning and skip it.
                if (tile.Name == emptyTileName) { Debug.LogWarning("Cannot define tile with same name as empty tile.", this); continue; }

                tileIndicesByName.Add(tile.Name, currentIndex);
                tilesByIndex.Add(currentIndex, tile);
                tilesByName.Add(tile.Name, tile);
                tile.OnLoaded();
                currentIndex++;
            }
        }
        #endregion

        #region Get Functions
        public ushort GetTileIndexFromName(string name) => name == emptyTileName ? (ushort)0 : tileIndicesByName[name];

        public string GetTileNameFromIndex(ushort index) => index == 0 ? emptyTileName : tilesByIndex[index].Name;

        public Tile<T> GetTileFromName(string name) => tilesByName[name];

        public Tile<T> GetTileFromIndex(ushort index) => tilesByIndex[index];
        #endregion
    }
}