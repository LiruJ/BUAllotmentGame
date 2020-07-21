﻿using Assets.Scripts.Objects;
using UnityEngine;

namespace Assets.Scripts.Player.Tools
{
    public class ObjectPlacer : Tool
    {
        #region Inspector Fields
        [Header("Maps")]
        [Tooltip("The object tilemap of the world.")]
        [SerializeField]
        private ObjectTilemap objectTilemap = null;
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

                if (TileIndicator.ShowObjectGhost) TileIndicator.ObjectGhost = value.TileObject;
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
                if (currentTile.HasTileObject) TileIndicator.ObjectGhost = currentTile.TileObject;
                TileIndicator.ChangeGridGhosts(currentTile.Width, currentTile.Height);
            }

        }
        #endregion

        #region Update Functions
        public override void HandleInput()
        {
            // Calculate the current tile position of the player's mouse.
            Vector3Int tilePosition = toolBelt.ScreenPositionToCell(Input.mousePosition);

            TileIndicator.UpdatePosition(tilePosition);

            // Update the placement ghost.
            if (CurrentTile != null)
            {

                TileIndicator.UpdateGridGhost(tilePosition, CurrentTile.Width, CurrentTile.Height, (x, y) => CurrentTile != null && CurrentTile.TileIsValid(objectTilemap, x, y));
                TileIndicator.UpdateObjectGhost(CurrentTile != null && CurrentTile.CanPlace(objectTilemap, tilePosition.x, tilePosition.z));

                // If the player clicks and their mouse is not over the UI, place the currently selected tile.
                if (Input.GetMouseButtonDown(0) && !eventSystem.IsPointerOverGameObject())
                {
                    objectTilemap.SetTile(tilePosition.x, tilePosition.z, CurrentTile);
                }
            }
        }
        #endregion
    }
}