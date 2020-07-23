using Assets.Scripts.BUCore.TileMap;
using UnityEngine;

namespace Assets.Scripts.Objects
{
    /// <summary> The type of tilemap that stores objects. </summary>
    public class ObjectTilemap : BaseTilemap<ObjectTileData>
    {
        #region Inspector Fields
        [Tooltip("The tileset that holds the object tiles.")]
        [SerializeField]
        private ObjectTileset objectTileset = null;
        #endregion

        #region Initialisation Functions
        protected override void Start()
        {
            // Set the tileset.
            Tileset = objectTileset;

            // Initialise the internal map data.
            base.Start();
        }
        #endregion

        #region Tile Functions
        /// <summary> Is fired when a tile is changed on the map, allows the entire area of the object to have its index set. </summary>
        /// <param name="x"> The x co-ordinate of the position. </param>
        /// <param name="y"> The y co-ordinate of the position. </param>
        /// <param name="tile"> The <see cref="Tile{T}"/> that is being set. </param>
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
        }
        #endregion
    }
}