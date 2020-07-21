using Assets.Scripts.Tiles;
using UnityEngine;

namespace Assets.Scripts.Player.Tools
{
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
            }
        }
        #endregion

        #region Tool Functions
        public override void OnSelected()
        {
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

            TileIndicator.UpdatePosition(tilePosition);

            // Update the placement ghost.
            if (CurrentTile != null)
            {
                TileIndicator.UpdateObjectGhost(CurrentTile != null && CurrentTile.CanPlace(floorTilemap, tilePosition.x, tilePosition.z));

                // If the player clicks and their mouse is not over the UI, place the currently selected tile.
                if (Input.GetMouseButtonDown(0) && !eventSystem.IsPointerOverGameObject())
                {
                    floorTilemap.SetTile(tilePosition.x, tilePosition.z, CurrentTile);
                }
            }
        }
        #endregion
    }
}