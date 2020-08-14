using Assets.Scripts.BUCore.TileMap;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameInterface.Tiles
{
    /// <summary> The base class for the tile selection bar. </summary>
    public abstract class TileBar : MonoBehaviour
    {
        #region Inspector Fields
        [Header("Prefabs")]
        [Tooltip("The prefab used for each tile icon.")]
        [SerializeField]
        private GameObject tileIconPrefab = null;
        #endregion

        #region Initialisation Functions
        /// <summary> Populates the tile bar, creating a new instance of the <see cref="tileIconPrefab"/> for each element within the given <paramref name="collection"/>. </summary>
        /// <typeparam name="T"> The type of <see cref="ITileData"/> within the given <paramref name="collection"/>. </typeparam>
        /// <param name="collection"> The collection of <see cref="ITileData"/>. </param>
        protected void createIcons<T>(IEnumerable<Tile<T>> collection) where T : struct, ITileData
        {
            foreach (Tile<T> tile in collection)
            {
                GameObject newTileIcon = Instantiate(tileIconPrefab, transform);

                TileIcon tileIcon = newTileIcon.GetComponent<TileIcon>();
                tileIcon.TileName = tile.Name;

                initialiseTileIcon(tile, tileIcon);
            }
        }

        /// <summary> Is called for every element within the collection given to the <see cref="createIcons{T}(IEnumerable{Tile{T}})"/> function, allowing the <paramref name="tileIcon"/>'s properties to be set from the given <paramref name="tile"/>. </summary>
        /// <typeparam name="T"> The type of <see cref="ITileData"/> of the given <paramref name="tile"/>. </typeparam>
        /// <param name="tile"> The <see cref="Tile{T}"/> within the collection. </param>
        /// <param name="tileIcon"> The UI icon representing the <paramref name="tile"/>. </param>
        protected virtual void initialiseTileIcon<T>(Tile<T> tile, TileIcon tileIcon) where T : struct, ITileData { }
        #endregion
    }
}