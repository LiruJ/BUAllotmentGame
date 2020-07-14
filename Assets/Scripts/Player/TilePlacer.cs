using Assets.Scripts.Map;
using Assets.Scripts.Objects;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Player
{
    public class TilePlacer : MonoBehaviour
    {
        #region Inspector Fields
        [Header("Dependencies")]
        [Tooltip("The player's camera.")]
        [SerializeField]
        private Camera playerCamera = null;

        [Tooltip("The main world map.")]
        [SerializeField]
        private WorldMap worldMap = null;

        [Tooltip("The object used to indicate the player's placement.")]
        [SerializeField]
        private PlacementIndicator tileIndicator = null;
        #endregion

        #region Properties
        /// <summary> The main world map. </summary>
        public WorldMap WorldMap => worldMap;

        /// <summary> The currently selected object tile. </summary>
        public ObjectTile CurrentObjectTile { get; private set; }
        #endregion

        #region Events
        [Tooltip("Is fired when the selected tile is changed.")]
        [SerializeField]
        private UnityEvent onCurrentObjectTileChanged = new UnityEvent();
        #endregion

        #region Update Functions
        private void Update()
        {
            // TODO: Replace this with selection menu.
            if (CurrentObjectTile == null)
            {
                CurrentObjectTile = worldMap.GetTilemap<ObjectTileData>().Tileset.GetTileFromName("Shed") as ObjectTile;
                onCurrentObjectTileChanged.Invoke();
            }

            // Calculate the current tile position of the player's mouse.
            Vector3Int currentTilePosition = ScreenPositionToCell(Input.mousePosition);
            
            // Update the placement ghost.
            tileIndicator.UpdateIndication(currentTilePosition);

            // If the player clicks, place the currently selected tile.
            if (Input.GetMouseButtonDown(0))
            {
                WorldMap.GetTilemap<ObjectTileData>().SetTile(currentTilePosition.x, currentTilePosition.z, CurrentObjectTile);
            }
        }
        #endregion

        #region Screen Functions
        public Vector3Int ScreenPositionToCell(Vector3 screenPosition)
        {
            // Create the ray from the position.
            Ray screenRay = playerCamera.ScreenPointToRay(screenPosition);

            // Simple rearranging of the parametric form of the screen ray, where we know the y position will be the same as the map's y position.
            return worldMap.Grid.WorldToCell(screenRay.origin + (worldMap.transform.position.y - screenRay.origin.y) / screenRay.direction.y * screenRay.direction);
        }
        #endregion
    }
}