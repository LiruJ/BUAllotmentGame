using Assets.Scripts.Tiles;
using UnityEngine;

namespace Assets.Scripts.Player.Tools
{
    /// <summary> The tool that allows the player to place floor tiles. </summary>
    public class FloorPlacer : Tool
    {
        #region Inspector Fields
        [Header("Maps")]
        [Tooltip("The floor tilemap of the world.")]
        [SerializeField]
        private FloorTilemap floorTilemap = null;

        [Header("Prefabs")]
        [Tooltip("The prefab used for the placement ghost, as floor tiles do not always have tile objects.")]
        [SerializeField]
        private GameObject tileBasePrefab = null;
        #endregion

        #region Fields
        /// <summary> The currently selected floor tile. </summary>
        private FloorTile currentTile;
        #endregion

        #region Properties
        /// <summary> The currently selected floor tile. </summary>
        public FloorTile CurrentTile
        {
            get => currentTile;
            set
            {
                // Set the current tile.
                currentTile = value;

                // If the current tile exists, show the placement ghost, otherwise; hide it.
                TileIndicator.ShowObjectGhost = value != null;
            }
        }
        #endregion

        #region Tool Functions
        public override void OnSelected()
        {
            // Initialise the tile placement ghost.
            TileIndicator.ShowGridGhost = false;
            TileIndicator.ShowObjectGhost = currentTile != null;
            TileIndicator.ObjectGhost = tileBasePrefab;
        }
        #endregion

        #region Update Functions
        public override void HandleInput()
        {
            // Calculate the current tile position of the player's mouse.
            Vector3Int tilePosition = toolBelt.ScreenPositionToCell(Input.mousePosition);

            // Update the position of the indicator.
            TileIndicator.UpdatePosition(tilePosition);

            // Only handle input and indicate placement validity if a tile is currently being placed.
            if (CurrentTile != null)
            {
                // Update the colour of the object ghost based on the validity of the current placement.
                TileIndicator.UpdateObjectGhost(CurrentTile.CanPlace(floorTilemap, tilePosition.x, tilePosition.z));

                // If the player clicks and their mouse is not over the UI, place the currently selected tile.
                if (Input.GetMouseButtonDown(0) && !eventSystem.IsPointerOverGameObject()) floorTilemap.SetTile(tilePosition.x, tilePosition.z, CurrentTile);
            }
        }
        #endregion
    }
}