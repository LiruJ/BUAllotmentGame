using Assets.Scripts.BUCore.TileMap;
using Assets.Scripts.Objects;
using Assets.Scripts.Tiles;
using UnityEngine;

namespace Assets.Scripts.Map
{
    /// <summary> The main world map of the allotment. </summary>
    public class WorldMap : BaseWorldMap
    {
        #region Inspector Fields
        [Header("Dependencies")]
        [Tooltip("The floor layer.")]
        [SerializeField]
        private FloorTilemap floorTilemap = null;

        [Tooltip("The object layer.")]
        [SerializeField]
        private ObjectTilemap objectTilemap = null;
        #endregion

        #region Validation Functions
        private void OnValidate()
        {
            if (floorTilemap == null) floorTilemap = GetComponentInChildren<FloorTilemap>();
            if (objectTilemap == null) objectTilemap = GetComponentInChildren<ObjectTilemap>();
            if (grid == null) grid = GetComponent<GridLayout>();
        }
        #endregion
    }
}