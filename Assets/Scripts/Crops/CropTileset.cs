using Assets.Scripts.BUCore.TileMap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Crops
{
    /// <summary> The <see cref="Tileset{T}"/> for <see cref="CropTile"/>s. </summary>
    [CreateAssetMenu(fileName = "New Crop Tileset", menuName = "Tilemap/Tilesets/Crop")]
    public class CropTileset : Tileset<CropTileData>, IEnumerable<CropTile>
    {
        #region Inspector Fields
        [Tooltip("The list of object tiles.")]
        [SerializeField]
        private List<CropTile> tiles = new List<CropTile>();
        #endregion

        #region Initialisation Functions
        private void OnEnable() => fromTileList(tiles);
        #endregion

        #region IEnumerable Functions
        public IEnumerator<CropTile> GetEnumerator() => tiles.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<CropTile>)tiles).GetEnumerator();
        #endregion
    }
}