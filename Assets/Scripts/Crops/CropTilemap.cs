using Assets.Scripts.BUCore.TileMap;
using Assets.Scripts.Creatures;
using Assets.Scripts.Seeds;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Crops
{
    /// <summary> The type of tilemap that stores crops. </summary>
    public class CropTilemap : BaseTilemap<CropTileData>
    {
        #region Inspector Fields
        [Tooltip("The tileset that holds the crop tiles.")]
        [SerializeField]
        private CropTileset cropTileset = null;

        [SerializeField]
        private CreatureManager creatureManager = null;

        [SerializeField]
        private SeedManager seedManager = null;
        #endregion

        #region Fields
        private readonly Dictionary<ushort, Seed> seedsByIndex = new Dictionary<ushort, Seed>();

        private readonly Queue<ushort> unusedIndices = new Queue<ushort>();

        private ushort currentStatIndex = 1;
        #endregion

        #region Properties
        public CreatureManager CreatureManager => creatureManager;
        #endregion

        #region Initialisation Functions
        protected override void Start()
        {
            // Set the tileset.
            Tileset = cropTileset;

            // Initialise the internal map data.
            base.Start();
        }
        #endregion

        #region Tile Functions
        public bool PlantSeed(int x, int y, Seed seed)
        {
            // The tile and seed stats cannot be null when planting a seed, so log an error if this happens.
            if (seed == null || string.IsNullOrWhiteSpace(seed.CropTileName )|| seed.CropTileName == Tileset.EmptyTileName || seed.GeneticStats == null)
            {
                Debug.LogError($"Invalid seed {seed}.", this);
                return false;
            }

            // Get the tile from the seed.
            Tile<CropTileData> cropTile = Tileset.GetTileFromName(seed.CropTileName);

            if (cropTile.CanPlace(this, x, y))
            {
                // Go through the regular procedure of placing the tile.
                SetTile(x, y, cropTile);

                // As the tile cannot be empty or null, the tile should have a seed after planting.
                if (!seedsByIndex.TryGetValue(this[x, y].StatsIndex, out Seed cropSeed) || cropSeed != null)
                {
                    Debug.LogError($"Seed value was invalid: {cropSeed}", this);
                    return false;
                }

                // Set the seed of the crop to the given seed.
                seedsByIndex[this[x, y].StatsIndex] = seed;

                // Remove the seed from the seed manager, as it's now in the ground.
                seedManager.RemoveSeed(seed);

                return true;
            }
            else return false;
        }

        public Seed GetCropSeed(int x, int y) => seedsByIndex.TryGetValue(this[x, y].StatsIndex, out Seed seed) ? seed : null;

        protected override void setTileIndex(int x, int y, Tile<CropTileData> tile)
        {
            // If the given tile is empty, reset the tile on the map and remove its seed.
            if (tile == null || tile.Name == Tileset.EmptyTileName)
            {
                // If the index of the stats is 0, do nothing.
                if (this[x, y].StatsIndex != 0)
                {
                    // Remove the seed.
                    seedsByIndex.Remove(this[x, y].StatsIndex);

                    // Add the index to the queue, as it's now unused.
                    unusedIndices.Enqueue(this[x, y].StatsIndex);

                    // Reset the tile on the map.
                    this[x, y] = new CropTileData() { Index = 0, Age = 0, StatsIndex = 0 };
                }
            }
            // Otherwise; reserve the seed slot for this plant.
            else
            {
                // If the unused indices queue is empty, use a bigger index, otherwise; use an index from the queue.
                ushort statsIndex = unusedIndices.Count == 0 ? currentStatIndex++ : unusedIndices.Dequeue();

                // Reserve the seed slot with a null value.
                seedsByIndex.Add(statsIndex, null);

                // Set the tile on the map.
                this[x, y] = new CropTileData() { Index = Tileset.GetTileIndexFromName(tile.Name), Age = 0, StatsIndex = statsIndex };
            }
        }
        #endregion
    }
}