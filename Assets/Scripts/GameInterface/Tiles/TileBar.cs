using Assets.Scripts.BUCore.TileMap;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameInterface.Tiles
{
    public abstract class TileBar : MonoBehaviour
    {
        #region Inspector Fields
        [Header("Prefabs")]
        [SerializeField]
        private GameObject tileIconPrefab = null;
        #endregion

        #region Initialisation Functions
        protected void createIcons<T>(IEnumerable<Tile<T>> collection) where T : ITileData
        {
            foreach (Tile<T> tile in collection)
            {
                GameObject newTileIcon = Instantiate(tileIconPrefab, transform);

                TileIcon tileIcon = newTileIcon.GetComponent<TileIcon>();
                tileIcon.TileName = tile.Name;
            }
        }
        #endregion
    }
}