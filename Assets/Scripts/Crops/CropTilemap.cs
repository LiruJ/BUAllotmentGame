using Assets.Scripts.BUCore.TileMap;
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
    }
}