using LowRezJam22.Engine.Graphics;

namespace LowRezJam22.Engine.Tiles
{
    internal class TileDefinition
    {
        private Texture? _textureReference;
        public string TileID = "Unneeded";

        public TileDefinition(Texture? textureReference)
        {
            _textureReference = textureReference;
        }

        public virtual Texture? GetTexture(int X, int Y, TileMap map)
        {
            return _textureReference;
        }
    }
}
