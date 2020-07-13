using Assets.Scripts.BUCore.TileMap;
using Assets.Scripts.Tiles;
using UnityEngine;

namespace Assets.Scripts.Objects
{
    public class ObjectTilemap : BaseTilemap<ObjectTileData>
    {
        #region Inspector Fields
        [SerializeField]
        private ObjectTileset objectTileset = null;

        #endregion

        #region Fields
        private BaseTilemap<FloorTileData> floorTilemap = null;
        #endregion

        #region Properties

        #endregion

        #region Initialisation Functions
        private void Start()
        {
            // Set the tileset.
            Tileset = objectTileset;

            // Set the floor tilemap.
            floorTilemap = WorldMap.GetTilemap<FloorTileData>();

            // Initialise the internal map data.
            initialiseData();
        }
        #endregion

        #region Tile Functions
        //public override void SetTile(int x, int y, Tile<ObjectTileData> tile)
        //{
        //    // Ensure the floor is valid and the space is valid.
        //    if (tile.HasTileLogic && !tile.TileLogic.CanPlaceTile(this, tile, x, y)) return;

        //    // Get the index of the tile type.
        //    ushort index = Tileset.GetTileIndexFromName(tile.Name);

        //    GameObject tileObject = null;

        //    // Place the tile object at the origin position.
        //    if (tile.TileObject != null)
        //    {
        //        tileObject = Instantiate(tile.TileObject, Grid.CellToLocal(new Vector3Int(x, 0, y)) + tile.TileObject.transform.localScale / 2.0f, Quaternion.identity, transform);
        //        tileObject.name = $"{tile.Name} {x}, {y}";
        //    }

        //    // Set the index of of the entire area to that of the given tile.
        //    for (int placeX = x; placeX < x + objectTile.Width; placeX++)
        //        for (int placeY = y; placeY < y + objectTile.Height; placeY++)
        //        {
        //            this[placeX, placeY] = new FloorTileData() { Index = index };
        //            tileObjects[placeX, placeY] = tileObject;
        //            if (objectTile.HasTileLogic) objectTile.ObjectTileLogic.OnTilePlaced(this, placeX, placeY);
        //        }
        //}
        #endregion
    }
}