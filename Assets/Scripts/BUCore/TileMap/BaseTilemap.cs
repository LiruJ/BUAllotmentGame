using Assets.Scripts.Tiles;
using UnityEngine;

namespace Assets.Scripts.BUCore.TileMap
{
    public abstract class BaseTilemap<T> : MonoBehaviour where T : ITileData 
    {
        #region Inspector Fields
        [Header("Dependencies")]
        [Tooltip("The world map that this tilemap is part of.")]
        [SerializeField]
        private BaseWorldMap worldMap = null;

        [Header("Settings")]
        [Tooltip("The name of the tile that will be used as the base when the map is created.")]
        [SerializeField]
        protected string startingTile = string.Empty;

        [Tooltip("How many update ticks are applied across every tile per second, or 0 if no update ticks should be applied.")]
        [Range(0, 30)]
        [SerializeField]
        private int ticksPerSecond = 1;
        #endregion

        #region Fields
        /// <summary> The array of tile data. </summary>
        private FloorTileData[,] tileData = null;

        /// <summary> The physical <see cref="GameObject"/>s for each tile in the Unity scene. </summary>
        protected GameObject[,] tileObjects = null;

        /// <summary> The gametime at which the last tick took place. </summary>
        private float timeOfLastTick = 0;
        #endregion

        #region Indexers
        public FloorTileData this[int x, int y]
        {
            get => IsInRange(x, y) ? tileData[x, y] : new FloorTileData();
            set { if (IsInRange(x, y)) tileData[x, y] = value; }
        }
        #endregion

        #region Properties
        /// <summary> The <see cref="GridLayout"/> object used to orientate the tiles </summary>
        public GridLayout Grid => worldMap.Grid;

        /// <summary> The <see cref="Tileset{T}"/> used by this map to associate IDs with names and vice versa. </summary>
        public Tileset<T> Tileset { get; protected set; }

        /// <summary> The <see cref="BaseWorldMap"/> that this <see cref="BaseTilemap{T}"/> is part of. </summary>
        public BaseWorldMap WorldMap => worldMap;

        /// <summary> The width of the map in tiles. </summary>
        public int Width => worldMap.Width;

        /// <summary> The height (z axis) of the map in tiles. </summary>
        public int Height => worldMap.Height;
        #endregion

        #region Initialisation Functions
        /// <summary> Initialises this <see cref="BaseTilemap{T}"/> within the world. </summary>
        protected void initialiseData()
        {
            // Throw an error if the world map object is missing.
            if (worldMap == null) { Debug.LogError("World map object is missing, tilemap cannot initialise.", this); return; }

            // Register this map with the world map.
            worldMap.RegisterTilemap(this);

            tileData = new FloorTileData[Width, Height];
            tileObjects = new GameObject[Width, Height];
        }
        #endregion

        #region Range Functions
        /// <summary> Calculates if the given position is in range of the map's bounds. </summary>
        /// <param name="x"> The x co-ordinate of the position. </param>
        /// <param name="y"> The y co-ordinate of the position. </param>
        /// <returns> True if the position is in range; otherwise, false. </returns>
        public bool IsInRange(int x, int y) => x >= 0 && x < Width && y >= 0 && y < Height;
        #endregion

        #region Tile Functions
        /// <summary> Sets the tile at the given <paramref name="x"/> and <paramref name="y"/> to the tile with the given <paramref name="name"/>. </summary>
        /// <param name="x"> The x co-ordinate of the position. </param>
        /// <param name="y"> The y co-ordinate of the position. </param>
        /// <param name="name"> The name of the tile. </param>
        public void SetTile(int x, int y, string name) => SetTile(x, y, Tileset.GetTileFromName(name));

        /// <summary> Sets the tile at the given <paramref name="x"/> and <paramref name="y"/> to the tile with the given <paramref name="index"/>. </summary>
        /// <param name="x"> The x co-ordinate of the position. </param>
        /// <param name="y"> The y co-ordinate of the position. </param>
        /// <param name="index"> The index of the tile. </param>
        public void SetTile(int x, int y, ushort index) => SetTile(x, y, Tileset.GetTileFromIndex(index));

        /// <summary> Sets the tile at the given <paramref name="x"/> and <paramref name="y"/> to the given <paramref name="tile"/>. </summary>
        /// <param name="x"> The x co-ordinate of the position. </param>
        /// <param name="y"> The y co-ordinate of the position. </param>
        /// <param name="tile"> The tile. </param>
        public virtual void SetTile(int x, int y, Tile<T> tile)
        {
            // If the tile has logic associated with it, query it first to ensure that the tile can be placed.
            if (tile.HasTileLogic && !tile.TileLogic.CanPlaceTile(this, tile, x, y)) return;

            // Set the index of the tile data at the given position to that of the given tile.
            tileData[x, y].Index = Tileset.GetTileIndexFromName(tile.Name);

            // If the tile has logic, fire the tile destroyed function.
            if (tile.HasTileLogic) tile.TileLogic.OnTileDestroyed(this, x, y);

            // If a GameObject already exists at the tile position, destroy it.
            if (tileObjects[x, y] != null) Destroy(tileObjects[x, y]);

            // If the tile has a GameObject associated with it, instantiate it.
            if (tile.TileObject != null)
            {
                // Instantiate the tile's GameObject, positioning it within the grid and setting its parent to this map's GameObject.
                GameObject tileObject = Instantiate(tile.TileObject, Grid.CellToLocal(new Vector3Int(x, 0, y)) + (tile.TileObject.transform.localScale / 2.0f), Quaternion.identity, transform);

                // Name the object based on its co-ords for easy debugging.
                tileObject.name = $"{x}, {y}";
                
                // Set the object within the array.
                tileObjects[x, y] = tileObject;
            }

            // If the tile has logic, fire the tile placed function.
            if (tile.HasTileLogic) tile.TileLogic.OnTilePlaced(this, x, y);
        }

        public bool IsTile(int x, int y, string tileName) => IsTile(x, y, Tileset.GetTileIndexFromName(tileName));

        public bool IsTile(int x, int y, Tile<T> tile) => IsTile(x, y, Tileset.GetTileIndexFromName(tile.Name));

        public bool IsTile(int x, int y, ushort index) => IsInRange(x, y) ? tileData[x, y].Index == index : false;
        #endregion

        #region Update Functions
        protected void tryTick()
        {
            if (ticksPerSecond == 0) return;

            float timeSinceLastTick = Time.time - timeOfLastTick;

            int ticksThisFrame = 0;
            while (timeSinceLastTick >= 1.0f / ticksPerSecond)
            {
                doTick();
                ticksThisFrame++;
                timeSinceLastTick -= 1.0f / ticksPerSecond;
            }

            if (ticksThisFrame > 0) timeOfLastTick = Time.time;
            if (ticksThisFrame > 1) Debug.LogWarning($"{ticksThisFrame} ticks in one frame.");
        }

        private void doTick()
        {
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                {
                    Tile<T> tile = Tileset.GetTileFromIndex(tileData[x, y].Index);

                    if (tile.HasTileLogic) tile.TileLogic.OnTick(this, x, y);
                }
        }
        #endregion

        #region Validation Functions
        private void OnValidate()
        {
            // If the world map hasn't been set, try to resolve it.
            if (worldMap == null) worldMap = GetComponentInParent<BaseWorldMap>();
        }
        #endregion
    }
}