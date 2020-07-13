using Assets.Scripts.BUCore.TileMap;
using Assets.Scripts.Tiles;
using System;
using UnityEngine;

namespace Assets.Scripts.Objects
{
    [Serializable]
    public class ObjectTile : Tile<ObjectTileData>
    {
        #region Inspector Fields
        [SerializeField]
        private ObjectTileLogic objectTileLogic = null;
        #endregion

        #region Initialisation Functions
        public override void OnLoaded() => TileLogic = objectTileLogic;
        #endregion
    }
}