using Assets.Scripts.Objects;
using Assets.Scripts.Tiles;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class TilePlacer : MonoBehaviour
    {
        #region Inspector Fields
        [SerializeField]
        private Camera playerCamera = null;

        [SerializeField]
        private FloorTilemap floorTilemap = null;

        [SerializeField]
        private ObjectTilemap objectTilemap = null;

        [SerializeField]
        private Transform tileIndicator = null;
        #endregion

        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Initialisation Functions
        private void Start()
        {

        }

        private void Awake()
        {

        }
        #endregion

        #region Update Functions
        private void Update()
        {
            Vector3Int currentTilePosition = ScreenPositionToCell(Input.mousePosition);
            tileIndicator.position = floorTilemap.Grid.CellToWorld(currentTilePosition) + (floorTilemap.Grid.cellSize / 2.0f);

            if (Input.GetMouseButtonDown(0))
            {
                objectTilemap.SetTile(currentTilePosition.x, currentTilePosition.z, "Shed");

            }
        }

        private void FixedUpdate()
        {

        }
        #endregion

        #region Screen Functions
        public Vector3Int ScreenPositionToCell(Vector3 screenPosition)
        {
            // Create the ray from the position.
            Ray screenRay = playerCamera.ScreenPointToRay(screenPosition);

            // Simple rearranging of the parametric form of the screen ray, where we know the y position will be the same as the map's y position.
            return floorTilemap.Grid.WorldToCell(screenRay.origin + (floorTilemap.transform.parent.position.y - screenRay.origin.y) / screenRay.direction.y * screenRay.direction);
        }
        #endregion
    }
}