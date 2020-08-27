using Assets.Scripts.BUCore.TileMap;
using Assets.Scripts.Creatures;
using UnityEngine;

namespace Assets.Scripts.Crops.Logic
{
    /// <summary> A crop tile that grows over time. </summary>
    [CreateAssetMenu(fileName = "New Crop Growth Tile", menuName = "Tilemap/Tiles/Crop/Growth")]
    public class CropGrowthLogic : CropTileLogic
    {
        #region Inspector Fields
        [Header("Prefabs")]
        [Tooltip("The prefab of the creature to spawn.")]
        [SerializeField]
        private Creature creaturePrefab = null;

        [Header("Crop Settings")]
        [Tooltip("How many ticks are required for this crop to be considered mature.")]
        [Range(byte.MinValue, byte.MaxValue)]
        [SerializeField]
        private byte matureAge = byte.MaxValue;

        [Tooltip("How many ticks are required for this crop to die.")]
        [Range(byte.MinValue, byte.MaxValue)]
        [SerializeField]
        private byte deathAge = byte.MaxValue;

        [Tooltip("How many creatures are spawned every tick when this plant is mature.")]
        [Range(1, byte.MaxValue)]
        [SerializeField]
        private byte creaturesPerTick = 1;
        #endregion

        #region Tile Functions
        public override void OnTick(BaseTilemap<CropTileData> tilemap, int x, int y)
        {
            // Increment the age of the crop.
            CropTileData crop = tilemap[x, y];
            crop.Age++;
            tilemap[x, y] = crop;

            // If the age is greater than or equal to the death age, unset the tile.
            if (crop.Age >= deathAge) tilemap.SetTile(x, y, tilemap.Tileset.EmptyTileName);
            // Otherwise; handle ageing the crop and spawning creatures.
            else
            {
                // Scale the plant.
                scalePlant(tilemap, x, y, crop);

                // If the age is greater than or equal to the mature age, spawn creatures.
                if (crop.Age >= matureAge)
                {
                    CropTilemap cropTilemap = tilemap as CropTilemap;

                    // Spawn the amount of creatures for this plant.
                    for (int i = 0; i < creaturesPerTick; i++)
                        cropTilemap.CreatureManager.SpawnCreature(cropTilemap.GetCropSeed(x, y), creaturePrefab, x, y);
                }
            }
        }

        private void scalePlant(BaseTilemap<CropTileData> tilemap, int x, int y, CropTileData crop)
        {
            // Set the y scale of the plant so that it starts at 0 and is at maximum 1 when the plant is fully mature.
            // TODO: Multiple plant models for growth stages.
            tilemap.GetTileObject(x, y).transform.localScale = new Vector3(1, Mathf.Min((float)crop.Age / matureAge, 1), 1);
        }

        public override void OnTilePlaced(BaseTilemap<CropTileData> tilemap, int x, int y) => scalePlant(tilemap, x, y, tilemap[x, y]);
        #endregion
    }
}