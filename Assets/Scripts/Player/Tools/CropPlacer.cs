﻿using Assets.Scripts.BUCore.TileMap;
using Assets.Scripts.Crops;
using Assets.Scripts.Seeds;
using UnityEngine;

namespace Assets.Scripts.Player.Tools
{
    /// <summary> The tool that allows the player to place crops. </summary>
    public class CropPlacer : Tool
    {
        #region Inspector Fields
        [Header("Maps")]
        [Tooltip("The crop tilemap of the world.")]
        [SerializeField]
        private CropTilemap cropTilemap = null;
        #endregion

        #region Fields
        /// <summary> The currently selected seed. </summary>
        private SeedGeneration currentSeedGeneration;
        #endregion

        #region Properties
        /// <summary> The currently selected seed. </summary>
        public SeedGeneration CurrentSeedGeneration
        {
            get => currentSeedGeneration;
            set
            {
                // Set the current tile.
                currentSeedGeneration = value;

                // If the current tile exists, show the placement ghost, otherwise; hide it.
                TileIndicator.ShowObjectGhost = value != null;

                // If the ghost is to be shown, switch out the model.
                // Note: This will break if the crop has no tile object, which shouldn't happen.
                if (TileIndicator.ShowObjectGhost)
                {
                    Tile<CropTileData> cropTile = cropTilemap.Tileset.GetTileFromName(value.CropTileName);
                    TileIndicator.ObjectGhost = cropTile.TileObject;
                }
            }
        }
        #endregion

        #region Tool Functions
        public override void OnSelected()
        {
            // Initialise the tile placement ghost.
            TileIndicator.ShowGridGhost = false;
            TileIndicator.ShowObjectGhost = true;

            // If a seed is selected, change the object ghost and grid indicators to match the newly selected object.
            if (currentSeedGeneration != null)
            {
                Tile<CropTileData> cropTile = cropTilemap.Tileset.GetTileFromName(currentSeedGeneration.CropTileName);
                if (cropTile.HasTileObject) TileIndicator.ObjectGhost = cropTile.TileObject;
            }
        }
        #endregion

        #region Update Functions
        public override void HandleInput()
        {
            // Calculate the current tile position of the player's mouse.
            Vector3Int tilePosition = toolBelt.ScreenPositionToCell(Input.mousePosition);

            // Update the position of the indicator.
            TileIndicator.UpdatePosition(tilePosition);

            // Only handle input and indicate placement validity if a tile is currently being placed.
            if (CurrentSeedGeneration != null)
            {
                // Get the tile of the seed's crop.
                Tile<CropTileData> cropTile = cropTilemap.Tileset.GetTileFromName(currentSeedGeneration.CropTileName);

                // Update the colour of the object ghost based on the validity of the current placement.
                TileIndicator.UpdateObjectGhost(currentSeedGeneration.Count > 0 && cropTile.CanPlace(cropTilemap, tilePosition.x, tilePosition.z));

                // If the player clicks and their mouse is not over the UI, place the currently selected tile.
                if (CurrentSeedGeneration.Count > 0 && Input.GetMouseButtonDown(0) && !eventSystem.IsPointerOverGameObject() && CurrentSeedGeneration.GetBestSeed() is Seed seed)
                    cropTilemap.PlantSeed(tilePosition.x, tilePosition.z, seed);
            }
        }
        #endregion
    }
}