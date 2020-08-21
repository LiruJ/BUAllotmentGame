using System.Collections.Generic;

namespace Assets.Scripts.Seeds
{
    public class SeedGeneration
    {
        #region Fields
        private readonly List<Seed> seeds = new List<Seed>();

        private bool needsSorting = true;
        #endregion

        #region Properties
        public string CropTileName { get; }

        public uint Generation { get; }

        public string UniqueIdentifier => CropTileName + Generation;

        public Dictionary<string, float> ScoreFilter { get; } = new Dictionary<string, float>();

        public int Count => seeds.Count;

        public IReadOnlyList<Seed> SortedSeeds { get { if (needsSorting) Sort(); return seeds; } }

        public IReadOnlyList<Seed> UnsortedSeeds => seeds;
        #endregion

        #region Constructors
        public SeedGeneration(string cropTileName, uint generation)
        {
            CropTileName = cropTileName;
            Generation = generation;
        }
        #endregion

        #region Seed Functions
        public Seed GetBestSeed()
        {
            // If the seeds need to be sorted, sort them.
            if (needsSorting) Sort();

            // Return the best seed, or null if none exist.
            return Count > 0 ? seeds[0] : null;
        }

        public void Add(Seed seed)
        {
            // Set the needs sorting flag to true.
            needsSorting = true;

            // Add the seed to the list.
            seeds.Add(seed);
        }

        public void Remove(Seed seed) => seeds.Remove(seed);

        public void Sort()
        {
            // If the list does not need to be sorted, do nothing.
            if (!needsSorting) return;

            // Sort the seeds using the filter.
            seeds.Sort((first, second) => first.CalculateScore(ScoreFilter).CompareTo(second.CalculateScore(ScoreFilter)));

            // Now that sorting is done, set the flag to false.
            needsSorting = false;
        }
        #endregion

        #region Calculation Functions
        public static string CalculateUniqueIdentifier(Seed seed) => seed.CropTileName + seed.Generation;
        #endregion
    }
}
