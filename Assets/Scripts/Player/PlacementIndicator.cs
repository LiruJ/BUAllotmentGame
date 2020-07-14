using Assets.Scripts.Objects;
using Assets.Scripts.Tiles;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlacementIndicator : MonoBehaviour
    {
        #region Inspector Fields
        [Header("Dependencies")]
        [Tooltip("The tile placer component used by the player to place tiles.")]
        [SerializeField]
        private TilePlacer tilePlacer = null;

        [Tooltip("The object that holds the main object ghost.")]
        [SerializeField]
        private Transform objectGhostContainer = null;

        [Tooltip("The object that holds the grid indicator objects.")]
        [SerializeField]
        private Transform gridGhostContainer = null;

        [Tooltip("The floor tile map.")]
        [SerializeField]
        private FloorTilemap floorMap = null;

        [Tooltip("The object tile map.")]
        [SerializeField]
        private ObjectTilemap objectMap = null;

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
        #endregion

        #region Indicator Functions
        public void HandleTileChange()
        {
            // Change the main object ghost to the new selected object.
            changeObjectGhost();

            // Change the grid ghosts to match the dimensions of the new object.
            changeGridGhosts();
        }

        private void changeGridGhosts()
        {
            // Resize the grid indicator main list if it is needed.
            if (gridIndicators.Count < tilePlacer.CurrentObjectTile.Height)
            {
                // Set the capacity first to save on memory allocations.
                gridIndicators.Capacity = tilePlacer.CurrentObjectTile.Height;

                // Add the empty lists.
                for (int y = gridIndicators.Count; y < tilePlacer.CurrentObjectTile.Height; y++)
                    gridIndicators.Add(new List<IndicatorGhost>(tilePlacer.CurrentObjectTile.Width));
            }

            // Go over each row of the grid indicators.
            for (int y = 0; y < tilePlacer.CurrentObjectTile.Height; y++)
            {
                // If the row needs to be resized, do so.
                List<IndicatorGhost> row = gridIndicators[y];
                if (row.Count < tilePlacer.CurrentObjectTile.Width) row.Capacity = tilePlacer.CurrentObjectTile.Width;

                // Go over the new grid cells and instantiate the grid indicators.
                for (int x = row.Count; x < tilePlacer.CurrentObjectTile.Width; x++)
                {
                    // Ensure nothing exists at this location.
                    Debug.Assert(x >= row.Count || row[x] == null);

                    // Create the cell indicator.
                    GameObject cellIndicator = Instantiate(tileValidityIndicator, tilePlacer.WorldMap.Grid.CellToLocal(new Vector3Int(x, 0, y)), tilePlacer.WorldMap.transform.rotation, gridGhostContainer);

                    // Get and save the indicator ghost component from the indicator, if none exists, throw an error.
                    if (!cellIndicator.TryGetComponent(out IndicatorGhost cellGhost)) { Debug.LogError("Cell indicator prefab is missing IndicatorGhost component.", this); return; }
                    row.Add(cellGhost);
                }
            }
        }

        private void changeObjectGhost()
        {
            // Delete the last ghost object if one existed.
            if (objectGhostContainer.childCount > 0) Destroy(objectGhostContainer.GetChild(0).gameObject);

            // Only create a new ghost if the new tile has an associated object.
            if (tilePlacer.CurrentObjectTile.HasTileObject)
            {
                // Create a new ghost object.
                GameObject ghostObject = Instantiate(tilePlacer.CurrentObjectTile.TileObject, objectGhostContainer);

                // Create the ghost behaviour for the ghost object and save the reference.
                currentGhost = ghostObject.AddComponent<IndicatorGhost>();
                currentGhost.ValidMaterial = validMaterial;
                currentGhost.InvalidMaterial = invalidMaterial;
            }
        }
        #endregion

        #region Update Functions
        public void UpdateIndication(Vector3Int tilePosition)
        {
            // Set the position of the indicator.
            transform.SetPositionAndRotation(tilePlacer.WorldMap.Grid.CellToWorld(tilePosition), tilePlacer.WorldMap.transform.rotation);

            // If there's a ghost for the object, change the materials of the indicators.
            if (currentGhost != null)
            {
                // Whether or not to check the floor for the current object.
                bool checkFloor = !string.IsNullOrWhiteSpace(tilePlacer.CurrentObjectTile.RequiredFloor);

                // Go over each grid tile indicator.
                for (int x = 0; x < tilePlacer.CurrentObjectTile.Width; x++)
                    for (int y = 0; y < tilePlacer.CurrentObjectTile.Height; y++)
                        gridIndicators[y][x].Change(tilePlacer.CurrentObjectTile.TileIsValid(objectMap, floorMap, tilePosition.x + x, tilePosition.z + y, checkFloor, tilePlacer.CurrentObjectTile.RequiredFloor));

                // Change the ghost material of the main object based on if it can be placed.
                currentGhost.Change(!tilePlacer.CurrentObjectTile.HasTileLogic || tilePlacer.CurrentObjectTile.TileLogic.CanPlaceTile(tilePlacer.WorldMap.GetTilemap<ObjectTileData>(), tilePlacer.CurrentObjectTile, tilePosition.x, tilePosition.z));
            }
        }
        #endregion
    }
}