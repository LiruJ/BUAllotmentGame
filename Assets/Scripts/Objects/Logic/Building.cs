using Assets.Scripts.BUCore.TileMap;
using Assets.Scripts.Tiles;
using UnityEngine;

namespace Assets.Scripts.Objects.Logic
{
    [CreateAssetMenu(fileName = "New Building", menuName = "Tilemap/Objects/Building")]
    public class Building : ObjectTileLogic
    {
        #region Inspector Fields

        #endregion

        #region Tile Functions


        public override void OnTilePlaced(BaseTilemap<ObjectTileData> tilemap, int x, int y) { }

        public override void OnTileDestroyed(BaseTilemap<ObjectTileData> tilemap, int x, int y) { }

        public override void OnTick(BaseTilemap<ObjectTileData> tilemap, int x, int y) { }
        #endregion
    }
}