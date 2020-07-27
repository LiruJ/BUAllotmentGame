using UnityEngine;

namespace Assets.Scripts.BUCore.TileMap
{
    /// <summary> The base tilemap that handles tile data and gameobjects, belonging to a <see cref="BaseWorldMap"/>. </summary>
    /// <typeparam name="T"> The type of <see cref="ITileData"/> to store. </typeparam>
    public abstract class BaseTilemap<T> : MonoBehaviour where T : struct, ITileData
    {
        #region Inspector Fields
        [Header("Dependencies")]
        [Tooltip("The world map that this tilemap is part of.")]
        [SerializeField]
        private BaseWorldMap worldMap = null;

        [Header("Settings")]
        [Tooltip("The name of the tile that will be used as the base when the map is created.")]
        [SerializeField]
        protected string startingTileName = string.Empty;

        [Tooltip("How many seconds to wait between update ticks. Divison can be used within the Unity input field, e.g. 1/60 for 60 ticks per second.")]
        [SerializeField]
        private float timeBetweenTicks = 1;
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
        /// <summary> Gets or sets the tile at the given <paramref name="x"/> and <paramref name="y"/> position. If this position is out of range, getting will return a tile with an index of 0, and setting will do nothing. </summary>
        /// <param name="x"> The x axis of the position. </param>
        /// <param name="y"> The y axis of the position. </param>
        /// <returns> The tile at the given position if it is in range, otherwise; a tile with the index of 0. </returns>
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

            // Initialise the map with the starting tile, if a starting tile was given.
            Tile<T> tile = Tileset.GetTileFromName(startingTileName);

            if (tile != null)
                for (int x = 0; x < Width; x++)
                    for (int y = 0; y < Height; y++)
                        SetTile(x, y, tile);
        }
        #endregion

        #region Range Functions
        /// <summary> Calculates if the given <paramref name="x"/> and <paramref name="y"/> positions are in range of the map's bounds. </summary>
        /// <param name="x"> The x co-ordinate of the position. </param>
        /// <param name="y"> The y co-ordinate of the position. </param>
        /// <returns> True if the given <paramref name="x"/> and <paramref name="y"/> positions are in range; otherwise, false. </returns>
        public bool IsInRange(int x, int y) => x >= 0 && x < Width && y >= 0 && y < Height;

        /// <summary> Calculates if the given <paramref name="tilePosition"/> is in range of the map's bounds. </summary>
        /// <param name="tilePosition"> The tile position to check, where the x correlates to the x axis, and the z correlates to the y axis. </param>
        /// <returns> True if the given <paramref name="tilePosition"/> is in range; otherwise, false. </returns>
        public bool IsInRange(Vector3Int tilePosition) => IsInRange(tilePosition.x, tilePosition.z);
        #endregion

        #region Tile Functions
        /// <summary> Gets the <see cref="Tile{T}"/> from the <see cref="Tileset"/> using the index of the <see cref="ITileData"/> at the given <paramref name="x"/> and <paramref name="y"/> positions. </summary>
        /// <param name="x"> The x co-ordinate of the position. </param>
        /// <param name="y"> The y co-ordinate of the position. </param>
        /// <returns> The <see cref="Tile{T}"/> at the given <paramref name="x"/> and <paramref name="y"/> positions. </returns>
        public Tile<T> GetTile(int x, int y) => Tileset == null ? null : IsInRange(x, y) ? Tileset.GetTileFromIndex(this[x, y].Index) : null;

        /// <summary> Get the name of the <see cref="Tile{T}"/> from the <see cref="Tileset"/> using the index of the <see cref="ITileData"/> at the given <paramref name="x"/> and <paramref name="y"/> positions, or <see cref="Tileset.EmptyTileName"/> if no tile exists. </summary>
        /// <param name="x"> The x co-ordinate of the position. </param>
        /// <param name="y"> The y co-ordinate of the position. </param>
        /// <returns> The name of the <see cref="Tile{T}"/> from the <see cref="Tileset"/> using the index of the <see cref="ITileData"/> at the given <paramref name="x"/> and <paramref name="y"/> positions, or <see cref="Tileset.EmptyTileName"/> if no tile exists.  </returns>
        public string GetTileName(int x, int y) => Tileset == null ? null : IsInRange(x, y) ? Tileset.GetTileNameFromIndex(this[x, y].Index) : null;

        /// <summary> Gets the index of the <see cref="ITileData"/> at the given <paramref name="x"/> and <paramref name="y"/> positions. </summary>
        /// <param name="x"> The x co-ordinate of the position. </param>
        /// <param name="y"> The y co-ordinate of the position. </param>
        /// <returns> The index of the <see cref="ITileData"/> at the given <paramref name="x"/> and <paramref name="y"/> positions. </returns>
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

            // If the tile cannot be placed here, do nothing.
            else if (tile != null && !tile.CanPlace(this, x, y)) return;

            // If the old tile has logic, fire the tile destroyed function.
            if (!IsTileEmpty(x, y) && GetTile(x, y).HasTileLogic) GetTile(x, y).TileLogic.OnTileDestroyed(this, x, y);

            // Set the index of the tile data at the given position to that of the given tile.
            setTileIndex(x, y, tile);

            // Place the tile object and destroy the old one.
            placeTileObject(x, y, tile);

            // If the tile has logic, fire the tile placed function.
            if (tile != null && tile.HasTileLogic) tile.TileLogic.OnTilePlaced(this, x, y);
        }

        /// <summary> Is called by <see cref="SetTile(int, int, Tile{T})"/> in order to set the index of the tile at the given <paramref name="x"/> and <paramref name="y"/> position. </summary>
        /// <param name="x"> The x co-ordinate of the position. </param>
        /// <param name="y"> The y co-ordinate of the position. </param>
        /// <param name="tile"> The tile that the position is being set to. </param>
        protected virtual void setTileIndex(int x, int y, Tile<T> tile) => tileData[x, y].Index = tile == null ? (ushort)0 : Tileset.GetTileIndexFromName(tile.Name);

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
            if (tile == null || !tile.HasTileObject) return;

            // Instantiate the tile's GameObject, positioning it within the grid and setting its parent to this map's GameObject.
            GameObject tileObject = Instantiate(tile.TileObject, transform);
            tileObject.transform.localPosition = Grid.CellToLocal(new Vector3Int(x, 0, y)) + tile.TileObject.transform.localPosition;
            tileObject.transform.localRotation = Quaternion.identity;

            // Name the object based on its co-ords for easy debugging.
            tileObject.name = $"{x}, {y}";

            // Set the object within the array.
            tileObjects[x, y] = tileObject;
        }

        /// <summary> Compares the name of the <see cref="Tile{T}"/> at the given <paramref name="x"/> and <paramref name="y"/> positions to the given <paramref name="tileName"/>. </summary>
        /// <param name="x"> The x co-ordinate of the position. </param>
        /// <param name="y"> The y co-ordinate of the position. </param>
        /// <param name="tileName"> The name of the tile to compare against. If the names match, this function will return true. </param>
        /// <returns> True if the given <paramref name="tileName"/> and the name of the tile on the map match, false otherwise. </returns>
        public bool IsTile(int x, int y, string tileName) => IsTile(x, y, Tileset.GetTileIndexFromName(tileName));

        /// <summary> Compares the name of the <see cref="Tile{T}"/> at the given <paramref name="x"/> and <paramref name="y"/> positions to the name of the given <paramref name="tile"/>. </summary>
        /// <param name="x"> The x co-ordinate of the position. </param>
        /// <param name="y"> The y co-ordinate of the position. </param>
        /// <param name="tile"> The the tile to compare against. If the names of the tiles match, this function will return true. </param>
        /// <returns> True if the name of the given <paramref name="tile"/> and the name of the tile on the map match, false otherwise. </returns>
        public bool IsTile(int x, int y, Tile<T> tile) => IsTile(x, y, Tileset.GetTileIndexFromName(tile.Name));

        /// <summary> Compares the index of the <see cref="Tile{T}"/> at the given <paramref name="x"/> and <paramref name="y"/> positions to the given <paramref name="index"/>. </summary>
        /// <param name="x"> The x co-ordinate of the position. </param>
        /// <param name="y"> The y co-ordinate of the position. </param>
        /// <param name="index"> The index of the tile to compare against. If the indices match, this function will return true. </param>
        /// <returns> True if the given <paramref name="index"/> and the index of the tile on the map match, false otherwise. </returns>
        public bool IsTile(int x, int y, ushort index) => IsInRange(x, y) ? tileData[x, y].Index == index : false;

        /// <summary> Returns true if the <see cref="Tile{T}"/> at the given <paramref name="x"/> and <paramref name="y"/> positions is empty, false otherwise. </summary>
        /// <param name="x"> The x co-ordinate of the position. </param>
        /// <param name="y"> The y co-ordinate of the position. </param>
        /// <returns> True if the <see cref="Tile{T}"/> at the given <paramref name="x"/> and <paramref name="y"/> positions is empty, false otherwise. </returns>
        public bool IsTileEmpty(int x, int y) => IsTile(x, y, Tileset.EmptyTileName);

        /// <summary> Gets the <see cref="GameObject"/> associated to a tile at the given <paramref name="x"/> and <paramref name="y"/> positions. </summary>
        /// <param name="x"> The x co-ordinate of the position. </param>
        /// <param name="y"> The y co-ordinate of the position. </param>
        /// <returns> The <see cref="GameObject"/> at the given position, or null if the position is out of range or no object exists. </returns>
        public GameObject GetTileObject(int x, int y) => IsInRange(x, y) ? tileObjects[x, y] : null;
        #endregion

        #region Update Functions
        protected virtual void Update() => tryTick();

        /// <summary> Attempt to perform a tick depending on the current time and the <see cref="timeOfLastTick"/>. </summary>
        protected void tryTick()
        {
            // If there should be no ticks, return immediately.
            if (timeBetweenTicks == 0) return;

            // Calculate how many seconds it has been since the last tick.
            float timeSinceLastTick = Time.time - timeOfLastTick;

            // Keep ticking as many times as needed.
            int ticksThisFrame = 0;
            while (timeSinceLastTick >= timeBetweenTicks)
            {
                // Do the tick.
                doTick();

                // Track the number of ticks made this frame.
                ticksThisFrame++;

                // Remove the tick time from the time since last tick.
                timeSinceLastTick -= timeBetweenTicks;
            }

            // If ticks were made, track the time of the last tick.
            if (ticksThisFrame > 0) timeOfLastTick = Time.time;
        }

        /// <summary> Perform a tick on each tile within the map. </summary>
        private void doTick()
        {
            if (Tileset == null)
            {
                Debug.LogError("Tileset is null, cannot tick.", this);
                return;
            }

            if (tileData == null)
            {
                Debug.LogError("Tile data is null, cannot tick.", this);
                return;
            }

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

            // Ensure the tick time never goes negative.
            timeBetweenTicks = Mathf.Max(0, timeBetweenTicks);
        }
        #endregion
    }
}