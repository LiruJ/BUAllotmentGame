using Assets.Scripts.Objects;
using UnityEngine;

namespace Assets.Scripts.Player.Tools
{
    /// <summary> The tool that allows the player to place object tiles. </summary>
    public class ObjectPlacer : Tool
    {
        #region Fields
        /// <summary> The object tilemap of the world. </summary>
        private ObjectTilemap objectTilemap;

        /// <summary> The currently selected object tile. </summary>
        private ObjectTile currentTile;
        #endregion

        #region Properties
        /// <summary> The currently selected object tile. </summary>
        public ObjectTile CurrentTile
        {
            get => currentTile;
            set
            {
                // Set the current tile.
                currentTile = value;

                // If the current tile exists, show the placement ghosts, otherwise; hide them.
                TileIndicator.ShowGridGhost = value != null;
                TileIndicator.ShowObjectGhost = value != null;

                // Change the object ghost and grid indicators to match the newly selected object.
                if (TileIndicator.ShowObjectGhost) TileIndicator.ObjectGhost = value.TileObject;
                if (TileIndicator.ShowGridGhost) TileIndicator.ChangeGridGhosts(value.Width, value.Height);
            }
        }
        #endregion

        #region Initialisation Functions
        protected override void initialiseTool() => objectTilemap = worldMap.GetTilemap<ObjectTileData>() as ObjectTilemap;
        #endregion

        #region Tool Functions
        public override void OnSelected()
        {
            // Initialise the tile placement ghost.
            TileIndicator.ShowGridGhost = true;
            TileIndicator.ShowObjectGhost = true;

            // If a tile is selected, change the object ghost and grid indicators to match the newly selected object.
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

            // Update the position of the indicator.
            TileIndicator.UpdatePosition(tilePosition);

            // Only handle input and indicate placement validity if an object is currently being placed.
            if (CurrentTile != null)
            {
                // Update the colour of the object ghosts based on the validity of the current placement.
                TileIndicator.UpdateGridGhost(tilePosition, CurrentTile.Width, CurrentTile.Height, (x, y) => CurrentTile != null && CurrentTile.TileIsValid(objectTilemap, x, y));
                TileIndicator.UpdateObjectGhost(CurrentTile.CanPlace(objectTilemap, tilePosition.x, tilePosition.z));

                // If the player clicks and their mouse is not over the UI, place the currently selected tile.
                if (Input.GetMouseButtonDown(0) && !eventSystem.IsPointerOverGameObject()) objectTilemap.SetTile(tilePosition.x, tilePosition.z, CurrentTile);
            }
        }
        #endregion
    }
}