using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.BUCore.TileMap
{
    public abstract class Tileset<T> : ScriptableObject where T : ITileData
    {
        #region Fields
        private readonly Dictionary<string, ushort> tileIndicesByName = new Dictionary<string, ushort>();

        private readonly Dictionary<ushort, Tile<T>> tilesByIndex = new Dictionary<ushort, Tile<T>>();

        private readonly Dictionary<string, Tile<T>> tilesByName = new Dictionary<string, Tile<T>>();
        #endregion

        #region Initialisation Functions
        protected void fromTileList<TileType>(List<TileType> tiles) where TileType : Tile<T>
        {
            // This will eventually be tracked outside of the start loop, so that tiles from different sets can be loaded together.
            ushort currentIndex = 0;

            // Go over each tile in the set and add them to the dictionaries.
            foreach (Tile<T> tile in tiles)
            {
                tileIndicesByName.Add(tile.Name, currentIndex);
                tilesByIndex.Add(currentIndex, tile);
                tilesByName.Add(tile.Name, tile);
                tile.OnLoaded();
                currentIndex++;
            }
        }
        #endregion

        #region Get Functions
        public ushort GetTileIndexFromName(string name) => tileIndicesByName[name];

        public string GetTileNameFromIndex(ushort index) => tilesByIndex[index].Name;

        public Tile<T> GetTileFromName(string name) => tilesByName[name];

        public Tile<T> GetTileFromIndex(ushort index) => tilesByIndex[index];
        #endregion
    }
}