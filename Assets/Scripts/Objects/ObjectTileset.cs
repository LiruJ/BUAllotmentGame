using Assets.Scripts.BUCore.TileMap;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Objects
{
    [CreateAssetMenu(fileName = "New Object Tileset", menuName = "Tilemap/Tilesets/Object")]
    public class ObjectTileset : Tileset<ObjectTileData>
    {
        #region Inspector Fields
        [SerializeField]
        private List<ObjectTile> tiles = new List<ObjectTile>();
        #endregion

        #region Initialisation Functions
        private void OnEnable() => fromTileList(tiles);
        #endregion
    }
}