using Assets.Scripts.BUCore.TileMap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Tiles
{
    [CreateAssetMenu(fileName = "New Floor Tileset", menuName = "Tilemap/Tilesets/Floor")]
    public class FloorTileset : Tileset<FloorTileData>, IEnumerable<FloorTile>
    {
        #region Inspector Fields
        [SerializeField]
        private List<FloorTile> tiles = new List<FloorTile>();
        #endregion

        #region Initialisation Functions
        private void OnEnable() => fromTileList(tiles);
        #endregion

        #region Get Functions
        public IEnumerator<FloorTile> GetEnumerator() => tiles.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<FloorTile>)tiles).GetEnumerator();
        #endregion
    }
}