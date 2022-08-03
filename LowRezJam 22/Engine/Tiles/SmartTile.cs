using LowRezJam22.Engine.Graphics;

namespace LowRezJam22.Engine.Tiles
{
    internal abstract class SmartTile : TileDefinition
    {
        public SmartTile() : base(null)
        {
        }

        public abstract override Texture? GetTexture(int X, int Y, TileMap map);
    }
}
