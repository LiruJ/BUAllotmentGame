using Assets.Scripts.Crops;
using Assets.Scripts.Seeds;
using UnityEngine;

namespace Assets.Scripts.Player
{
    /// <summary> The player controlled by AI. </summary>
    public class AIPlayer : BasePlayer
    {
        #region Inspector Fields
        [SerializeField]
        private int tomatoZ = 0;

        [SerializeField]
        private int asparagusZ = 0;
        #endregion

        #region Fields
        /// <summary> The crop tilemap of the AI's map. </summary>
        private CropTilemap cropTilemap;

        private bool hasPlantedFirst = false;
        #endregion

        #region Properties

        #endregion

        #region Initialisation Functions
        private void Start()
        {
            cropTilemap = WorldMap.GetTilemap<CropTileData>() as CropTilemap;
        }
        #endregion

        #region Seed Functions
        public void SetupSeedFilters()
        {
            // Set up the score filter.
            // TODO: Better way of doing this.
            SeedGeneration tomatoSeeds = SeedManager.GetLatestGenerationOfCropType("Tomato");
            tomatoSeeds.ScoreFilter.Add("DistanceFromGoal", -2);
            tomatoSeeds.ScoreFilter.Add("AliveTime", 0.5f);
            tomatoSeeds.ScoreFilter.Add("MaxConcurrentSeenCreatures", 1);
            tomatoSeeds.ScoreFilter.Add("DamageDealt", 10);
            tomatoSeeds.ScoreFilter.Add("Kills", 10);

            SeedGeneration asparagusSeeds = SeedManager.GetLatestGenerationOfCropType("Asparagus");
            asparagusSeeds.ScoreFilter.Add("DistanceFromGoal", -2);
            asparagusSeeds.ScoreFilter.Add("AliveTime", 0.5f);
            asparagusSeeds.ScoreFilter.Add("MaxConcurrentSeenCreatures", 1);
            asparagusSeeds.ScoreFilter.Add("DamageDealt", 10);
            asparagusSeeds.ScoreFilter.Add("Kills", 10);
        }

        private void plantCrops()
        {
            for (int x = 0; x < WorldMap.Width; x++)
            {
                if (cropTilemap.IsTileEmpty(x, tomatoZ))
                    cropTilemap.PlantSeed(x, tomatoZ, SeedManager.GetLatestGenerationOfCropType("Tomato").GetBestSeed());
                if (cropTilemap.IsTileEmpty(x, asparagusZ))
                    cropTilemap.PlantSeed(x, asparagusZ, SeedManager.GetLatestGenerationOfCropType("Asparagus").GetBestSeed());
            }
        }

        public void PlantNextGeneration(uint generation) => plantCrops();
        #endregion

        #region Tick Functions
        public void OnCropTick()
        {
            if (!hasPlantedFirst) { plantCrops(); hasPlantedFirst = true; }
        }
        #endregion
    }
}