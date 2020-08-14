using Assets.Scripts.BUCore.TileMap;
using Assets.Scripts.Creatures;
using Assets.Scripts.Seeds;
using System;
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
        #endregion

        #region Fields
        private Dictionary<ushort, Dictionary<string, float>> cropStatsByStatIndex = new Dictionary<ushort, Dictionary<string, float>>();

        private Queue<Tuple<ushort, Dictionary<string, float>>> unusedStats = new Queue<Tuple<ushort, Dictionary<string, float>>>();

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
        public void PlantSeed(int x, int y, Seed seed)
        {
            // The tile and seed stats cannot be null when planting a seed, so log an error if this happens.
            if (seed == null || string.IsNullOrWhiteSpace(seed.CropTileName )|| seed.CropTileName == Tileset.EmptyTileName || seed.GeneticStats == null)
            {
                Debug.LogError($"Invalid seed {seed}.", this);
                return;
            }

            // Get the tile from the seed.
            Tile<CropTileData> cropTile = Tileset.GetTileFromName(seed.CropTileName);

            // Go through the regular procedure of placing the tile.
            SetTile(x, y, cropTile);

            // As the tile cannot be empty or null, the tile should have stats after planting.
            Dictionary<string, float> cropStats = getCropStats(x, y);
            if (cropStats == null)
            {
                Debug.LogError("Seeds were planted, but crop did not get stats.", this);
                return;
            }

            // Copy each seed stat over to the plant.
            foreach (KeyValuePair<string, float> nameValuePair in seed.GeneticStats)
                cropStats.Add(nameValuePair.Key, nameValuePair.Value);
        }

        public IReadOnlyDictionary<string, float> GetCropStats(int x, int y) => getCropStats(x, y);

        private Dictionary<string, float> getCropStats(int x, int y) => cropStatsByStatIndex.TryGetValue(this[x, y].StatsIndex, out Dictionary<string, float> cropStats) ? cropStats : null;

        protected override void setTileIndex(int x, int y, Tile<CropTileData> tile)
        {
            // If the given tile is empty, reset the tile on the map and remove its stats.
            if (tile == null || tile.Name == Tileset.EmptyTileName)
            {
                // If the index of the stats is 0, do nothing.
                if (this[x, y].StatsIndex != 0)
                {
                    // Get the stats dictionary from the main dictionary, then remove it.
                    Dictionary<string, float> cropStats = cropStatsByStatIndex[this[x, y].StatsIndex];
                    cropStatsByStatIndex.Remove(this[x, y].StatsIndex);

                    // Clear the crop stats dictionary, then add it to the queue along with the ID.
                    cropStats.Clear();
                    unusedStats.Enqueue(new Tuple<ushort, Dictionary<string, float>>(this[x, y].StatsIndex, cropStats));

                    // Reset the tile on the map.
                    this[x, y] = new CropTileData() { Index = 0, Age = 0, StatsIndex = 0 };
                }
            }
            // Otherwise; set the stats of the crop.
            // TODO: Get stats from seed, set the stats of the plant to the stats of the seed.
            else
            {
                // The index and stats of the stat data.
                ushort statsIndex;
                Dictionary<string, float> cropStats;

                // If the unused stats queue is empty, create a new dictionary.
                if (unusedStats.Count == 0)
                {
                    // Create the new dictionary.
                    cropStats = new Dictionary<string, float>();

                    // Set the index of this new stat, then increment the current index.
                    statsIndex = currentStatIndex;
                    currentStatIndex++;
                }
                // Otherwise; reuse the old stat index and dictionary.
                else (statsIndex, cropStats) = unusedStats.Dequeue();

                // Add the data to the main dictionary.
                cropStatsByStatIndex.Add(statsIndex, cropStats);

                // Set the tile on the map.
                this[x, y] = new CropTileData() { Index = Tileset.GetTileIndexFromName(tile.Name), Age = 0, StatsIndex = statsIndex };
            }
        }
        #endregion
    }
}