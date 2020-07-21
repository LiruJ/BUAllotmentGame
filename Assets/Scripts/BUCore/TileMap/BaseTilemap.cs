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
        private T[,] tileData = null;

        /// <summary> The physical <see cref="GameObject"/>s for each tile in the Unity scene. </summary>
        protected GameObject[,] tileObjects = null;

        /// <summary> The gametime at which the last tick took place. </summary>
        private float timeOfLastTick = 0;
        #endregion

        #region Indexers
        public T this[int x, int y]
        {
            get => (tileData != null && IsInRange(x, y)) ? tileData[x, y] : default;
            set { if (tileData != null && IsInRange(x, y)) tileData[x, y] = value; }
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
        protected virtual void Start()
        {
            // Throw an error if the world map object is missing.
            if (worldMap == null) { Debug.LogError("World map object is missing, tilemap cannot initialise.", this); return; }

            // Create the arrays.
            tileData = new T[Width, Height];
            tileObjects = new GameObject[Width, Height];
        }
        #endregion

        #region Range Functions
        /// <summary> Calculates if the given position is in range of the map's bounds. </summary>
        /// <param name="x"> The x co-ordinate of the position. </param>
        /// <param name="y"> The y co-ordinate of the position. </param>
        /// <returns> True if the position is in range; otherwise, false. </returns>
        public bool IsInRange(int x, int y) => x >= 0 && x < Width && y >= 0 && y < Height;

        public bool IsInRange(Vector3Int tilePosition) => IsInRange(tilePosition.x, tilePosition.z);
        #endregion

        #region Tile Functions
        public Tile<T> GetTile(int x, int y) => Tileset == null ? null : IsInRange(x, y) ? Tileset.GetTileFromIndex(this[x, y].Index) : null;

        public string GetTileName(int x, int y) => Tileset == null ? null : IsInRange(x, y) ? Tileset.GetTileNameFromIndex(this[x, y].Index) : null;

        public ushort GetTileIndex(int x, int y) => IsInRange(x, y) ? this[x, y].Index : (ushort)0;

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
        /// <remarks> If this function is overridden, be sure to call the OnTileDestroyed and OnTilePlaced functions. </remarks>
        public virtual void SetTile(int x, int y, Tile<T> tile)
        {
            // If the given position is out of range, do nothing.
            if (!IsInRange(x, y)) return;

            // If the tile cannot be placed here, return.
            if (!tile.CanPlace(this, x, y)) return;

            // Set the index of the tile data at the given position to that of the given tile.
            setTileIndex(x, y, tile);

            // If the tile has logic, fire the tile destroyed function.
            if (tile.HasTileLogic) tile.TileLogic.OnTileDestroyed(this, x, y);

            // Place the tile object and destroy the old one.
            placeTileObject(x, y, tile);

            // If the tile has logic, fire the tile placed function.
            if (tile.HasTileLogic) tile.TileLogic.OnTilePlaced(this, x, y);
        }

        /// <summary> Is called by <see cref="SetTile(int, int, Tile{T})"/> in order to set the index of the tile at the given <paramref name="x"/> and <paramref name="y"/> position. </summary>
        /// <param name="x"> The x co-ordinate of the position. </param>
        /// <param name="y"> The y co-ordinate of the position. </param>
        /// <param name="tile"> The tile that the position is being set to. </param>
        protected virtual void setTileIndex(int x, int y, Tile<T> tile) => tileData[x, y].Index = Tileset.GetTileIndexFromName(tile.Name);

        /// <summary> Is called by <see cref="SetTile(int, int, Tile{T})"/> in order to create the tile object at the given <paramref name="x"/> and <paramref name="y"/> position. </summary>
        /// <param name="x"> The x co-ordinate of the position. </param>
        /// <param name="y"> The y co-ordinate of the position. </param>
        /// <param name="tile"> The tile whose object is being placed. </param>
        /// <remarks> Handles destroying the old <see cref="GameObject"/> at the given position as well as creating a new one. This function is called even if the <paramref name="tile"/> has no tile object. </remarks>
        protected virtual void placeTileObject(int x, int y, Tile<T> tile)
        {
            // If a GameObject already exists at the tile position, destroy it.
            if (tileObjects[x, y] != null) Destroy(tileObjects[x, y]);

            // Do nothing if no tile object exists.
            if (!tile.HasTileObject) return;

            // Instantiate the tile's GameObject, positioning it within the grid and setting its parent to this map's GameObject.
            GameObject tileObject = Instantiate(tile.TileObject, transform);
            tileObject.transform.localPosition = Grid.CellToLocal(new Vector3Int(x, 0, y)) + tile.TileObject.transform.localPosition;
            tileObject.transform.localRotation = Quaternion.identity;

            // Name the object based on its co-ords for easy debugging.
            tileObject.name = $"{x}, {y}";

            // Set the object within the array.
            tileObjects[x, y] = tileObject;
        }

        public bool IsTile(int x, int y, string tileName) => IsTile(x, y, Tileset.GetTileIndexFromName(tileName));

        public bool IsTile(int x, int y, Tile<T> tile) => IsTile(x, y, Tileset.GetTileIndexFromName(tile.Name));

        public bool IsTile(int x, int y, ushort index) => IsInRange(x, y) ? tileData[x, y].Index == index : false;

        public bool IsTileEmpty(int x, int y) => IsTile(x, y, Tileset.EmptyTileName);
        #endregion

        #region Update Functions
        protected virtual void Update() => tryTick();

        protected void tryTick()
        {
            // If there should be no ticks, return immediately.
            if (ticksPerSecond == 0) return;

            // Calculate how many seconds it has been since the last tick.
            float timeSinceLastTick = Time.time - timeOfLastTick;

            // Keep ticking as many times as needed.
            int ticksThisFrame = 0;
            while (timeSinceLastTick >= 1.0f / ticksPerSecond)
            {
                // Do the tick.
                doTick();

                // Track the number of ticks made this frame.
                ticksThisFrame++;

                // Remove the tick time from the time since last tick.
                timeSinceLastTick -= 1.0f / ticksPerSecond;
            }

            // If ticks were made, track the time of the last tick.
            if (ticksThisFrame > 0) timeOfLastTick = Time.time;
        }

        private void doTick()
        {
            // Go over each tile in the map.
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                {
                    // Get the tile from the tileset.
                    Tile<T> tile = Tileset.GetTileFromIndex(tileData[x, y].Index);
                    
                    // If the tile has logic, tick it.
                    if (tile != null && tile.HasTileLogic) tile.TileLogic.OnTick(this, x, y);
                }
        }
        #endregion

        #region Validation Functions
        protected virtual void OnValidate()
        {
            // If the world map hasn't been set, try to resolve it.
            if (worldMap == null) worldMap = GetComponentInParent<BaseWorldMap>();
        }
        #endregion
    }
}