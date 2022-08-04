using LowRezJam22.Helpers;

namespace LowRezJam22.Engine.Graphics
{
    internal class Background
    {
        public List<(Color tint, Texture texture, bool scroll)> Layers = new();
        public int ParallaxStrength = 3;

        public void Draw(int CameraX, int CameraY)
        {
            for (int i = 0; i < Layers.Count; i++)
            {
                if (Layers[i].scroll)
                {
                    DrawScrollLayer(Layers[i].texture, Layers[i].tint, CameraX, CameraY, Layers.Count - i);
                }
                else
                {
                    Renderer.DrawSprite(Layers[i].texture, new Rectangle(0, 0, Layers[i].texture.Width, Layers[i].texture.Height), Layers[i].tint);
                }
            }
        }

        private void DrawScrollLayer(Texture layer, Color tint, int CameraX, int CameraY, int parallax)
        {
            float parallaxX = CameraX / 4;
            float parallaxY = (CameraY + 10) / 4;

            while (parallaxX < 0) parallaxX += layer.Width;
            while (parallaxX > layer.Width) parallaxX -= layer.Width;

            while (parallaxY < 0) parallaxY += layer.Height;
            while (parallaxY > layer.Height) parallaxY -= layer.Height;

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    Renderer.DrawSprite(layer, new Rectangle(-parallaxX + x * layer.Width, parallaxY + y * layer.Height, layer.Width, layer.Height), tint);
                }
            }
        }
    }
}
