using Assets.Scripts.Objects;
using Assets.Scripts.Tiles;
using UnityEngine;

namespace Assets.Scripts.Player.Tools
{
    public class FloorPlacer : Tool
    {
        #region Inspector Fields
        [SerializeField]
        private ObjectTilemap objectTilemap = null;

        [SerializeField]
        private FloorTilemap floorTilemap = null;
        #endregion

        #region Fields
        private FloorTile currentTile;
        #endregion

        #region Properties
        /// <summary> The currently selected floor tile. </summary>
        public FloorTile CurrentTile
        {
            get => currentTile;
            set
            {
                currentTile = value;

                TileIndicator.ShowObjectGhost = value != null;

                if (TileIndicator.ShowObjectGhost) TileIndicator.ChangeObjectGhost(value.TileObject);
            }
        }
        #endregion

        #region Tool Functions
        public override void OnSelected()
        {
            TileIndicator.ShowGridGhost = false;
            TileIndicator.ShowObjectGhost = true;

            if (currentTile != null && currentTile.HasTileObject) TileIndicator.ChangeObjectGhost(currentTile.TileObject);
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
                TileIndicator.UpdateObjectGhost(!CurrentTile.HasTileLogic || CurrentTile.TileLogic.CanPlaceTile(floorTilemap, CurrentTile, currentTilePosition.x, currentTilePosition.z));

                // If the player clicks, place the currently selected tile.
                if (Input.GetMouseButtonDown(0))
                {
                    if (floorTilemap.IsInRange(currentTilePosition)) floorTilemap.SetTile(currentTilePosition.x, currentTilePosition.z, CurrentTile);
                }
            }
        }
        #endregion
    }
}