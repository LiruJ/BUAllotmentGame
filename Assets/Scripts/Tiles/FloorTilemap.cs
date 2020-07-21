using Assets.Scripts.BUCore.TileMap;
using UnityEngine;

namespace Assets.Scripts.Tiles
{
    public class FloorTilemap : BaseTilemap<FloorTileData>
    {
        #region Inspector Fields
        [SerializeField]
        private FloorTileset floorTileset = null;

        [Header("Prefabs")]
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

            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                {
                    // Create the tile object first.
                    GameObject tileBase = Instantiate(tilePrefab, transform);
                    tileBase.transform.localPosition = Grid.CellToLocal(new Vector3Int(x, 0, y));
                    tileBase.transform.localRotation = Quaternion.identity;
                    tileBase.name = $"{x}, {y}";

                    tileObjects[x, y] = tileBase;

                    SetTile(x, y, Tileset.GetTileFromName(startingTile));
                }

            SetTile(Width / 2, Height / 2, Tileset.GetTileFromName("Grass"));
        }
        #endregion

        #region Tile Functions
        protected override void placeTileObject(int x, int y, Tile<FloorTileData> tile)
        {
            // Get the tile base object, if it is null or has no renderer, do nothing.
            GameObject tileObject = tileObjects[x, y];
            if (tileObject == null || !tileObject.TryGetComponent(out MeshRenderer tileRenderer)) return;

            // Get the tile from the tileset, if it is invalid, do nothing.
            if (!(tile is FloorTile floorTile)) return;

            tileRenderer.material = floorTile.Material;
        }
        #endregion
    }
}