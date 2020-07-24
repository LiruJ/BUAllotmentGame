using Assets.Scripts.BUCore.TileMap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Objects
{
    /// <summary> The <see cref="Tileset{T}"/> for <see cref="ObjectTile"/>s. </summary>
    [CreateAssetMenu(fileName = "New Object Tileset", menuName = "Tilemap/Tilesets/Object")]
    public class ObjectTileset : Tileset<ObjectTileData>, IEnumerable<ObjectTile>
    {
        #region Inspector Fields
        [Tooltip("The list of object tiles.")]
        [SerializeField]
        private List<ObjectTile> tiles = new List<ObjectTile>();
        #endregion

        #region Initialisation Functions
        private void OnEnable() => fromTileList(tiles);
        #endregion

        #region IEnumerable Functions
        public IEnumerator<ObjectTile> GetEnumerator() => tiles.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<ObjectTile>)tiles).GetEnumerator();
        #endregion
    }
}