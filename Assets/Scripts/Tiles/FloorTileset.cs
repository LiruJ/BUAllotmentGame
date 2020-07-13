using Assets.Scripts.BUCore.TileMap;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Tiles
{
    [CreateAssetMenu(fileName = "New Floor Tileset", menuName = "Tilemap/Tilesets/Floor")]
    public class FloorTileset : Tileset<FloorTileData>
    {
        #region Inspector Fields
        [SerializeField]
        private List<FloorTile> tiles = new List<FloorTile>();
        #endregion

        #region Initialisation Functions
        private void OnEnable() => fromTileList(tiles);
        #endregion
    }
}