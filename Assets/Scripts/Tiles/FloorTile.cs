﻿using Assets.Scripts.BUCore.TileMap;
using System;
using UnityEngine;

namespace Assets.Scripts.Tiles
{
    [Serializable]
    public class FloorTile : Tile<FloorTileData>
    {
        #region Inspector Fields
        [Tooltip("The floor-specific tile logic.")]
        [SerializeField]
        private FloorTileLogic floorTileLogic = null;
        #endregion

        #region Initialisation Functions
        /// <summary> Is fired when the tileset loads. </summary>
        public override void OnLoaded() => TileLogic = floorTileLogic;
        #endregion
    }
}