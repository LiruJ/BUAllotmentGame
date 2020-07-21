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
        /// <summary> The <see cref="IndicatorGhost"/> component of the <see cref="ObjectGhost"/>, or null if it does not exist. </summary>
        private IndicatorGhost objectGhost = null;

        /// <summary> A 2D list of the grid indicator objects. </summary>
        private readonly List<List<IndicatorGhost>> gridIndicators = new List<List<IndicatorGhost>>();

        /// <summary> Is true if the grid ghosts should be shown; otherwise, false. </summary>
        private bool showGridGhost = false;

        /// <summary> Is true if the object ghost should be shown; otherwise, false. </summary>
        private bool showObjectGhost = false;
        #endregion

        #region Properties
        /// <summary> Gets or sets the object shown as the object ghost. Note that setting this value creates a copy of the given object. </summary>
        public GameObject ObjectGhost
        {
            get => objectGhost == null ? null : objectGhost.gameObject;
            set
            {
                // Delete the last ghost object if one existed.
                if (ObjectGhost != null) Destroy(ObjectGhost);

                // Disable the object ghost if the given item is null.
                if (value == null) { ShowObjectGhost = false; return; }

                // Create a new ghost object.
                GameObject ghostObject = Instantiate(value.gameObject, transform);

                // Create the ghost behaviour for the ghost object and save the reference.
                objectGhost = ghostObject.AddComponent<IndicatorGhost>();
                objectGhost.ValidMaterial = validMaterial;
                objectGhost.InvalidMaterial = invalidMaterial;
            }
        }

        /// <summary> Is true if the grid ghosts should be shown; otherwise, false. </summary>
        public bool ShowGridGhost { get => showGridGhost; set { showGridGhost = value; gridGhostContainer.gameObject.SetActive(value); } }

        /// <summary> Is true if the object ghost should be shown; otherwise, false. </summary>
        public bool ShowObjectGhost { get => showObjectGhost; set { showObjectGhost = value; if (ObjectGhost != null) ObjectGhost.gameObject.SetActive(value); } }
        #endregion

        #region Indicator Functions
        /// <summary> Adds or removes grid indicators based on the given <paramref name="width"/> and <paramref name="height"/>. </summary>
        /// <param name="width"> The number of grid cells to indicate on the x axis. </param>
        /// <param name="height"> The number of grid cells to indicate on the z axis. </param>
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
        #endregion

        #region Update Functions
        /// <summary> Updates the material of the grid ghosts based on the returned value from the given <paramref name="validityFunction"/>. </summary>
        /// <param name="tilePosition"> The tile position on the map from which to begin the check. </param>
        /// <param name="width"> The number of grid cells to check on the x axis. </param>
        /// <param name="height"> The number of grid cells to check on the z axis. </param>
        /// <param name="validityFunction"> A function that takes the x and y position of the checked tiles and returns true if the tile is valid, and false otherwise. </param>
        public void UpdateGridGhost(Vector3Int tilePosition, int width, int height, Func<int, int, bool> validityFunction)
        {
            // If grid ghosts are not to be shown, do nothing.
            if (!ShowGridGhost) return;

            // Go over each grid tile indicator and call the given function.
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    gridIndicators[y][x].Change(validityFunction(tilePosition.x + x, tilePosition.z + y));
        }

        /// <summary> Updates the material of the <see cref="ObjectGhost"/> to signify the placement validity based on the value of <paramref name="canPlace"/>. </summary>
        /// <param name="canPlace"> The validity of the tile placement, where true is valid and false is invalid. </param>
        public void UpdateObjectGhost(bool canPlace)
        {
            // If the object ghost doesn't exist or is hidden, do nothing.
            if (!ShowObjectGhost || ObjectGhost == null) return;

            // Change the ghost material of the main object based on if it can be placed.
            objectGhost.Change(canPlace);
        }

        /// <summary> Positions the indicator in the world from the given <paramref name="tilePosition"/>. </summary>
        /// <param name="tilePosition"> The tile position. </param>
        public void UpdatePosition(Vector3Int tilePosition) => transform.SetPositionAndRotation(worldMap.Grid.CellToWorld(tilePosition), worldMap.transform.rotation);
        #endregion
    }
}