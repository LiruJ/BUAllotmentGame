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
        protected override void setTileIndex(int x, int y, Tile<ObjectTileData> tile)
        {
            // Get the object tile.
            if (!(tile is ObjectTile objectTile)) { Debug.LogError("Invalid object tile state.", this); return; }

            // Get the index of the tile.
            ushort index = Tileset.GetTileIndexFromName(objectTile.Name);

            // Go over each tile covered by the object and set its index.
            for (int placeX = x; placeX < x + objectTile.Width; placeX++)
                for (int placeY = y; placeY < y + objectTile.Height; placeY++)
                    this[placeX, placeY] = new ObjectTileData() { Index = index };

            base.setTileIndex(x, y, tile);
        }
        #endregion
    }
}