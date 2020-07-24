using Assets.Scripts.BUCore.TileMap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Tiles
{
    /// <summary> The <see cref="Tileset{T}"/> for <see cref="FloorTile"/>s. </summary>
    [CreateAssetMenu(fileName = "New Floor Tileset", menuName = "Tilemap/Tilesets/Floor")]
    public class FloorTileset : Tileset<FloorTileData>, IEnumerable<FloorTile>
    {
        #region Inspector Fields
        [Tooltip("The list of floor tiles.")]
        [SerializeField]
        private List<FloorTile> tiles = new List<FloorTile>();
        #endregion

        #region Initialisation Functions
        private void OnEnable() => fromTileList(tiles);
        #endregion

        #region IEnumerable Functions
        public IEnumerator<FloorTile> GetEnumerator() => tiles.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<FloorTile>)tiles).GetEnumerator();
        #endregion
    }
}