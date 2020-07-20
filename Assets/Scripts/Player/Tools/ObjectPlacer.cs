﻿using Assets.Scripts.Objects;
using Assets.Scripts.Tiles;
using UnityEngine;

namespace Assets.Scripts.Player.Tools
{
    public class ObjectPlacer : Tool
    {
        #region Inspector Fields
        [SerializeField]
        private ObjectTilemap objectTilemap = null;

        [SerializeField]
        private FloorTilemap floorTilemap = null;
        #endregion

        #region Fields
        private ObjectTile currentTile;
        #endregion

        #region Properties
        /// <summary> The currently selected object tile. </summary>
        public ObjectTile CurrentTile
        {
            get => currentTile;
            set
            {
                currentTile = value; 

                TileIndicator.ShowGridGhost = value != null;
                TileIndicator.ShowObjectGhost = value != null;

                if (TileIndicator.ShowObjectGhost) TileIndicator.ChangeObjectGhost(value.TileObject);
                if (TileIndicator.ShowGridGhost) TileIndicator.ChangeGridGhosts(value.Width, value.Height);
            }
        }
        #endregion

        #region Tool Functions
        public override void OnSelected()
        {
            TileIndicator.ShowGridGhost = true;
            TileIndicator.ShowObjectGhost = true;

            if (currentTile != null)
            {
                if (currentTile.HasTileObject) TileIndicator.ChangeObjectGhost(currentTile.TileObject);
                TileIndicator.ChangeGridGhosts(currentTile.Width, currentTile.Height);
            }

        }
        #endregion

        #region Update Functions
        public override void HandleInput()
        {
            // Calculate the current tile position of the player's mouse.
            Vector3Int currentTilePosition = toolBelt.ScreenPositionToCell(Input.mousePosition);

            TileIndicator.UpdatePosition(currentTilePosition);

            // Update the placement ghost.
            if (CurrentTile != null)
            {
                TileIndicator.UpdateGridGhost(currentTilePosition, CurrentTile.Width, CurrentTile.Height,
                    (x, y) => CurrentTile.TileIsValid(objectTilemap, floorTilemap, x, y, !string.IsNullOrWhiteSpace(CurrentTile.RequiredFloor), CurrentTile.RequiredFloor));

                TileIndicator.UpdateObjectGhost(!CurrentTile.HasTileLogic || CurrentTile.TileLogic.CanPlaceTile(objectTilemap, CurrentTile, currentTilePosition.x, currentTilePosition.z));

                // If the player clicks, place the currently selected tile.
                if (Input.GetMouseButtonDown(0))
                {
                    if (objectTilemap.IsInRange(currentTilePosition)) objectTilemap.SetTile(currentTilePosition.x, currentTilePosition.z, CurrentTile);
                }
            }
        }
        #endregion
    }
}