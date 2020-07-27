using Assets.Scripts.BUCore.TileMap;
using System;
using UnityEngine;

namespace Assets.Scripts.Crops.Logic
{
    /// <summary> A crop tile that grows over time. </summary>
    [CreateAssetMenu(fileName = "New Crop Growth Tile", menuName = "Tilemap/Tiles/Crop/Growth")]
    public class CropGrowthLogic : CropTileLogic
    {
        #region Inspector Fields
        [Header("Crop Settings")]
        [Tooltip("How many ticks are required for this crop to be considered mature.")]
        [Range(byte.MinValue, byte.MaxValue)]
        [SerializeField]
        private byte matureAge = byte.MaxValue;

        [Tooltip("How many ticks are required for this crop to die.")]
        [Range(byte.MinValue, byte.MaxValue)]
        [SerializeField]
        private byte deathAge = byte.MaxValue;
        #endregion

        #region Tile Functions
        public override void OnTick(BaseTilemap<CropTileData> tilemap, int x, int y)
        {
            // Increment the age of the crop.
            CropTileData crop = tilemap[x, y];
            crop.Age++;
            tilemap[x, y] = crop;

            // If the age is greater than or equal to the death age, unset the tile.
            if (crop.Age >= deathAge)
            {
                tilemap.SetTile(x, y, tilemap.Tileset.EmptyTileName);
            }
            else
            {
                tilemap.GetTileObject(x, y).transform.localScale = new Vector3(1, Mathf.Min((float)crop.Age / matureAge, 1), 1);
            }
        }

        public override void OnTileDestroyed(BaseTilemap<CropTileData> tilemap, int x, int y)
        {
            tilemap[x, y] = new CropTileData() { Index = tilemap[x, y].Index, Age = 0 };
        }
        #endregion
    }
}