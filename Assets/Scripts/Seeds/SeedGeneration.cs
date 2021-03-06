﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Seeds
{
    /// <summary> Represents an entire generation of seeds for a specific plant type, as well as a set of weights used to sort them. </summary>
    public class SeedGeneration
    {
        #region Fields
        /// <summary> The collection of seeds. </summary>
        private readonly List<Seed> seeds = new List<Seed>();

        /// <summary> Stores the best and worst scores of every seed in this generation, keyed by stat name. </summary>
        private readonly Dictionary<string, BestWorst> bestWorstScoresByStatName = new Dictionary<string, BestWorst>();

        /// <summary> Is true if the seeds collection has been changed since the last time it was accessed, false otherwise. </summary>
        private bool needsSorting = true;
        #endregion

        #region Properties
        /// <summary> The name of the crop whose seeds are held by this generation. </summary>
        public string CropTileName { get; }

        /// <summary> The numerical generation count. </summary>
        public uint Generation { get; }

        /// <summary> The collection of stat names and associated weights used to sort the seeds. </summary>
        public Dictionary<string, float> ScoreFilter { get; } = new Dictionary<string, float>();

        /// <summary> The amount of seeds contained within this generation. </summary>
        public int Count => seeds.Count;

        /// <summary> The seeds sorted by score. </summary>
        /// <remarks> This will call the <see cref="Sort"/> function if the seeds are unsorted when this property is used. </remarks>
        public IReadOnlyList<Seed> SortedSeeds { get { if (needsSorting) Sort(); return seeds; } }

        /// <summary> The unsorted seed collection. </summary>
        public IReadOnlyList<Seed> UnsortedSeeds => seeds;
        #endregion

        #region Constructors
        /// <summary> Creates a new <see cref="SeedGeneration"/> with the given <paramref name="cropTileName"/> and <paramref name="generation"/>. </summary>
        /// <param name="cropTileName"> The name of the crop whose seeds should be stored. </param>
        /// <param name="generation"> The number of this generation. </param>
        public SeedGeneration(string cropTileName, uint generation)
        {
            CropTileName = cropTileName;
            Generation = generation;
        }
        #endregion

        #region Seed Functions
        /// <summary> Gets the seed with the best score. If the seeds are unsorted, they will first be sored using the <see cref="Sort"/> function. </summary>
        /// <returns> The <see cref="Seed"/> with the highest score according to the <see cref="ScoreFilter"/>. </returns>
        /// <remarks> The returned seed is not removed from the list. </remarks>
        public Seed GetBestSeed()
        {
            // If the seeds need to be sorted, sort them.
            if (needsSorting) Sort();

            if (Count == 0) return null;

            // Generate a random number between 0.5 and 1 (technically 0.9985), weighted more towards 1.
            // This uses a modified sigmoid curve and essentially gives some sense of genetic diversity by still giving the not-quite-best seeds a chance to be selected. 
            float random = 1 / (1 + Mathf.Pow((float)Math.E, -4f * UnityEngine.Random.value));

            // Convert the random number to an index.
            int index = Mathf.FloorToInt(random * Count);

            // Return the seed at the calculated index.
            return seeds[index];
        }

        /// <summary> Adds the given <paramref name="seed"/> to the generation. </summary>
        /// <param name="seed"> The <see cref="Seed"/> to add. </param>
        public void Add(Seed seed)
        {
            // Ensure the seed is valid.
            if (seed == null) Debug.LogException(new ArgumentNullException($"Attempted to add a null seed to {CropTileName} generation {Generation}."));
            if (seed.Generation != Generation || seed.CropTileName != CropTileName) 
                Debug.LogException(new ArgumentException($"Attempted to add a seed with generation {seed.Generation} and {seed.CropTileName} to {CropTileName} generation {Generation}."));

            // Ensure the seed does not already exist within the collection.
            if (seeds.Contains(seed)) Debug.LogException(new Exception($"{CropTileName} generation {Generation} already has seed {seed}."));

            // Set the needs sorting flag to true.
            needsSorting = true;

            // Add the seed to the list.
            seeds.Add(seed);

            // Go over each lifetime stat of the seed.
            foreach (KeyValuePair<string, float> statNameValue in seed.LifetimeStats)
            {
                // If the best/worst collection does not contain this stat, add the stat's value as the best and worst.
                if (!bestWorstScoresByStatName.TryGetValue(statNameValue.Key, out BestWorst bestWorst))
                {
                    bestWorst = new BestWorst(statNameValue.Value);
                    bestWorstScoresByStatName.Add(statNameValue.Key, bestWorst);
                }
                // Otherwise; only change the best/worst if the value of this stat is the best or worst.
                else
                {
                    if (statNameValue.Value > bestWorst.Best) bestWorst.Best = statNameValue.Value;
                    if (statNameValue.Value < bestWorst.Worst) bestWorst.Worst = statNameValue.Value;
                }
            }
        }

        /// <summary> Removes the given <paramref name="seed"/> from this generation. </summary>
        /// <param name="seed"> The <see cref="Seed"/> to remove. </param>
        /// <returns> True if the seed was removed; otherwise, false. </returns>
        public bool Remove(Seed seed) => seeds.Remove(seed);
        #endregion

        #region Filter Functions
        /// <summary> Sorts the seeds using the current <see cref="ScoreFilter"/>. </summary>
        public void Sort()
        {
            // If the list does not need to be sorted, do nothing.
            if (!needsSorting) return;

            // Sort the seeds using the filter.
            seeds.Sort((first, second) => first.CalculateScore(ScoreFilter, bestWorstScoresByStatName).CompareTo(second.CalculateScore(ScoreFilter, bestWorstScoresByStatName)));

            // Now that sorting is done, set the flag to false.
            needsSorting = false;
        }

        /// <summary> Copies the score filter from the given <paramref name="priorGeneration"/> into this generation's <see cref="ScoreFilter"/>, completely clearing the old values. </summary>
        /// <param name="priorGeneration"> The generation from which to copy. </param>
        public void SetFilterFrom(SeedGeneration priorGeneration)
        {
            // Clear the score filter.
            ScoreFilter.Clear();

            // Copy each stat filter from the given generation's dictionary.
            foreach (string statName in priorGeneration.ScoreFilter.Keys)
                ScoreFilter.Add(statName, priorGeneration.ScoreFilter[statName]);
        }
        #endregion
    }
}
