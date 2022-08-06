using LowRezJam22.Engine.Graphics;
using LowRezJam22.Helpers;

namespace LowRezJam22.Engine.Tiles
{
    internal class TileMap
    {
        public Dictionary<(int X, int Y), TileDefinition> Tiles { get; private set; } = new();

        public int X { get; set; } = 0;
        public int Y { get; set; } = 0;
        public int Width { get; set; } = 0;
        public int Height { get; set; } = 0;

        public Shader ShaderReference { get; set; }
        public Color ColorFilter { get; set; } = Colors.White;

        public TileMap(int x, int y, int width, int height, Shader shaderReference)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            ShaderReference = shaderReference;
        }

        public TileDefinition? GetTileAt(int X, int Y)
        {
            if (Tiles.ContainsKey((X, Y))) 
            {
                return Tiles[(X, Y)];
            }
            return null;
        }

        public void SetTileAt(int X, int Y, TileDefinition tile)
        {
            if (Tiles.ContainsKey((X,Y)))
            {
                Tiles[(X, Y)] = tile;
            }
            else
            {
                Tiles.Add((X, Y), tile);
            }
        }

        public void RemoveTileAt(int X, int Y)
        {
            if (Tiles.ContainsKey((X,Y)))
            {
                Tiles.Remove((X, Y));
            }
        }

        public void Serialize(string file)
        {
            System.Text.Json.JsonSerializerOptions options = new()
            {
                WriteIndented = true
            };
            string serializedData = System.Text.Json.JsonSerializer.Serialize(Tiles, Tiles.GetType(), options);
            System.IO.File.WriteAllText(file, serializedData);
        }

        public void Deserialize(string file)
        {
            if (!System.IO.File.Exists(file))
                return;
            string data = System.IO.File.ReadAllText(file);
            object? returnData = System.Text.Json.JsonSerializer.Deserialize(data, Tiles.GetType());
            if (returnData is null)
                return;
            Tiles = (Dictionary<(int X, int Y), TileDefinition>)returnData;
        }

        public void Draw()
        {
            foreach (var key in Tiles.Keys)
            {
                Texture? texture = Tiles[key].GetTexture(key.X, key.Y, this);
                if (texture is null)
                    continue;
                Renderer.SetShader(ShaderReference);
                Renderer.DrawSprite(texture, new Rectangle(X + (key.X * Width), Y + (key.Y * Height), Width, Height), Colors.White);
            }
        }
    }
}
