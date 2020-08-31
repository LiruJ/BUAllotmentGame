using Assets.Scripts.Crops;
using Assets.Scripts.Seeds;

namespace Assets.Scripts.Player
{
    /// <summary> The player controlled by AI. </summary>
    public class AIPlayer : BasePlayer
    {
        #region Inspector Fields

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
            SeedGeneration firstGeneration = SeedManager.GetLatestGenerationOfCropType("Tomato");
            firstGeneration.ScoreFilter.Add("DistanceFromGoal", -4);
            firstGeneration.ScoreFilter.Add("AliveTime", 0.5f);
        }

        private void plantCrops()
        {
            for (int x = 0; x < WorldMap.Width; x++)
                if (cropTilemap.IsTileEmpty(x, 0))
                    cropTilemap.PlantSeed(x, 0, SeedManager.GetLatestGenerationOfCropType("Tomato").GetBestSeed());
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