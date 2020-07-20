using Assets.Scripts.BUCore.TileMap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Tiles
{
    public class FloorTilemap : BaseTilemap<FloorTileData>
    {
        #region Inspector Fields
        [SerializeField]
        private FloorTileset floorTileset = null;
        #endregion

        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Initialisation Functions
        private void Start()
        {
            // Set the tileset.
            Tileset = floorTileset;

            // Initialise the internal map data.
            initialiseData();

            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    SetTile(x, y, Tileset.GetTileFromName(startingTile));

            SetTile(Width / 2, Height / 2, Tileset.GetTileFromName("Grass"));

        }
        
        private void Awake()
        {

        }
        #endregion

        #region Update Functions
        private void Update()
        {
            tryTick();
        }

        private void FixedUpdate()
        {

        }
        #endregion
    }
}