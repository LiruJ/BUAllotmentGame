using Assets.Scripts.BUCore.TileMap;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlacementIndicator : MonoBehaviour
    {
        #region Inspector Fields
        [Header("Dependencies")]
        [Tooltip("The object that holds the main object ghost.")]
        [SerializeField]
        private Transform objectGhostContainer = null;

        [Tooltip("The object that holds the grid indicator objects.")]
        [SerializeField]
        private Transform gridGhostContainer = null;

        [Tooltip("The world map, used to orientate the indicator.")]
        [SerializeField]
        private BaseWorldMap worldMap = null;

        [Header("Prefabs")]
        [Tooltip("The prefab for the grid tile indicator.")]
        [SerializeField]
        private GameObject tileValidityIndicator = null;

        [Header("Materials")]
        [Tooltip("The material to use to signify an invalid action.")]
        [SerializeField]
        private Material invalidMaterial = null;

        [Tooltip("The material to use to signify a valid action.")]
        [SerializeField]
        private Material validMaterial = null;
        #endregion

        #region Fields
        /// <summary> A 2D list of the grid indicator objects. </summary>
        private readonly List<List<IndicatorGhost>> gridIndicators = new List<List<IndicatorGhost>>();

        /// <summary> The current ghost object. </summary>
        private IndicatorGhost currentGhost = null;
        private bool showGridGhost = false;
        private bool showObjectGhost = false;
        #endregion

        #region Properties
        public bool ShowGridGhost { get => showGridGhost; set { showGridGhost = value; gridGhostContainer.gameObject.SetActive(value); } }

        public bool ShowObjectGhost { get => showObjectGhost; set { showObjectGhost = value; if (currentGhost != null) currentGhost.gameObject.SetActive(value); } }
        #endregion

        #region Indicator Functions
        public void ChangeGridGhosts(int width, int height)
        {
            // If either of the dimensions is 0, disable the grid.
            if (width == 0 || height == 0) { ShowGridGhost = false; return; }

            // Resize the grid indicator main list if it is needed.
            if (gridIndicators.Count < height)
            {
                // Set the capacity first to save on memory allocations.
                gridIndicators.Capacity = height;

                // Add the empty lists.
                for (int y = gridIndicators.Count; y < height; y++)
                    gridIndicators.Add(new List<IndicatorGhost>(width));
            }

            // Go over each row of the grid indicators.
            for (int y = 0; y < height; y++)
            {
                // If the row needs to be resized, do so.
                List<IndicatorGhost> row = gridIndicators[y];
                if (row.Count < width) row.Capacity = width;

                // Go over the new grid cells and instantiate the grid indicators.
                for (int x = row.Count; x < width; x++)
                {
                    // Ensure nothing exists at this location.
                    Debug.Assert(x >= row.Count || row[x] == null);

                    // Create the cell indicator. 
                    // You may think that it's strange that I didn't use the Instantiate function with the position and rotation parameters.
                    // I spent an entire hour trying to work out if it is local or world space, the only answer I could come to was "yes". Just set the transform data after, instead of relying on a black-box function.
                    GameObject cellIndicator = Instantiate(tileValidityIndicator, gridGhostContainer);
                    cellIndicator.transform.localPosition = Vector3.Scale(new Vector3(x, 0, y), worldMap.Grid.cellSize);
                    cellIndicator.transform.localRotation = Quaternion.identity;

                    // Get and save the indicator ghost component from the indicator, if none exists, throw an error.
                    if (!cellIndicator.TryGetComponent(out IndicatorGhost cellGhost)) { Debug.LogError("Cell indicator prefab is missing IndicatorGhost component.", this); return; }
                    row.Add(cellGhost);
                }
            }
        }

        public void ChangeObjectGhost(GameObject newObject)
        {
            // Delete the last ghost object if one existed.
            if (objectGhostContainer.childCount > 0) Destroy(objectGhostContainer.GetChild(0).gameObject);

            // Disable the object ghost if the given item is null.
            if (newObject == null) { ShowObjectGhost = false; return; }

            // Create a new ghost object.
            GameObject ghostObject = Instantiate(newObject, objectGhostContainer);

            // Create the ghost behaviour for the ghost object and save the reference.
            currentGhost = ghostObject.AddComponent<IndicatorGhost>();
            currentGhost.ValidMaterial = validMaterial;
            currentGhost.InvalidMaterial = invalidMaterial;
        }
        #endregion

        #region Update Functions
        public void UpdateGridGhost(Vector3Int tilePosition, int width, int height, Func<int, int, bool> validityFunction)
        {
            if (!ShowGridGhost) return;

            // Go over each grid tile indicator.
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    gridIndicators[y][x].Change(validityFunction(tilePosition.x + x, tilePosition.z + y));
        }

        public void UpdateObjectGhost(bool canPlace)
        {
            if (!ShowObjectGhost) return;

            // Change the ghost material of the main object based on if it can be placed.
            currentGhost.Change(canPlace);
        }

        public void UpdatePosition(Vector3Int tilePosition)
        {
            // Set the position of the indicator.
            transform.SetPositionAndRotation(worldMap.Grid.CellToWorld(tilePosition), worldMap.transform.rotation);
        }
        #endregion
    }
}