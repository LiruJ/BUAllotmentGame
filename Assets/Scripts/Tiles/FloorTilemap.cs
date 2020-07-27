using Assets.Scripts.BUCore.TileMap;
using UnityEngine;

namespace Assets.Scripts.Tiles
{
    /// <summary> The <see cref="BaseTilemap{T}"/> for the floor layer. </summary>
    public class FloorTilemap : BaseTilemap<FloorTileData>
    {
        #region Inspector Fields
        [Tooltip("The tileset for the floor tiles.")]
        [SerializeField]
        private FloorTileset floorTileset = null;

        [Header("Prefabs")]
        [Tooltip("The prefab used for each floor tile's base.")]
        [SerializeField]
        private GameObject tilePrefab = null;
        #endregion

        #region Initialisation Functions
        protected override void Start()
        {
            // Set the tileset.
            Tileset = floorTileset;

            // Initialise the internal map data.
            base.Start();
        }
        #endregion

        #region Tile Functions
        protected override void placeTileObject(int x, int y, Tile<FloorTileData> tile)
        {
            // If the tile object doesn't exist, create it.
            GameObject tileObject = tileObjects[x, y];
            if (tileObject == null)
            {
                // Create the tile object.
                tileObject = Instantiate(tilePrefab, transform);
                tileObject.transform.localPosition = Grid.CellToLocal(new Vector3Int(x, 0, y));
                tileObject.transform.localRotation = Quaternion.identity;
                tileObject.name = $"{x}, {y}";

                // Set the tileobject at the position to the newly created one.
                tileObjects[x, y] = tileObject;
            }

            // Try get the mesh renderer from the tile base. If non exists, do nothing.
            if (!tileObject.TryGetComponent(out MeshRenderer tileRenderer)) return;

            // Get the tile from the tileset, if it is invalid, do nothing.
            if (!(tile is FloorTile floorTile)) return;

            // Set the material of the tile.
            tileRenderer.material = floorTile.Material;
        }
        #endregion
    }
}