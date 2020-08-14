using Assets.Scripts.BUCore.TileMap;

namespace Assets.Scripts.Crops
{
    public struct CropTileData : ITileData
    {
        public ushort Index { get; set; }

        public byte Age { get; set; }

        public ushort StatsIndex { get; set; }
    }
}
