using LowRezJam22.Engine;
using LowRezJam22.Engine.Graphics;
using LowRezJam22.Engine.Tiles;
using LowRezJam22.Helpers;
using OpenTK.Graphics.OpenGL;

namespace LowRezJam22.Scenes
{
    internal class GameScene : SceneBase
    {
        private Shader mainShader;
        private TileMap mainTileMap;

        RenderTexture gameRenderTexture = new(64, 64);
        RenderTexture uiRenderTexture = new(64, 64);
        RenderTexture mainRendertexture = new(64, 64);

        private string tempString =
            "----------\n" +
            "-x-----x--\n" +
            "----x--x--\n" +
            "----x--x--\n" +
            "---xxx----\n" +
            "-xxxxxxx--\n" +
            "---xxx----\n" +
            "-x--x-----\n" +
            "-xx-x-xxx-\n" +
            "----------\n";

        public GameScene()
        {
            mainShader = new Shader("Assets/Shaders/MainShader.vert", "Assets/Shaders/MainShader.frag");
            Renderer.SetShader(mainShader);

            List<Texture> desertTiles = new();
            for (int i = 0; i <= 16; i++)
            {
                desertTiles.Add(new Texture("Assets/Tiles/DesertTiles_" + i.ToString().PadLeft(2, '0') + ".png"));
            }

            SmartTiles.DesertTile tile = new(new() { desertTiles[0] }, new() { desertTiles[1] }, new() { desertTiles[2] },
                new() { desertTiles[3] }, new() { desertTiles[4], desertTiles[5] }, new() { desertTiles[6] },
                new() { desertTiles[7] }, new() { desertTiles[8] }, new() { desertTiles[9] },
                new() { desertTiles[10] }, new() { desertTiles[11] }, new() { desertTiles[12] },
                new() { desertTiles[13] }, new() { desertTiles[14] }, new() { desertTiles[15] },
                new() { desertTiles[16] });
            mainTileMap = new TileMap(0, 0, 4, 4, mainShader);
            string[] temp = tempString.Split('\n');
            for (int y = 0; y < temp.Length; y++)
            {
                for (int x = 0; x < temp[y].Length; x++)
                {
                    if (temp[y][x] == 'x')
                    {
                        mainTileMap.SetTileAt(x,y, tile);
                    }
                }
            }
        }

        public override void Init()
        {
        }

        public override void Destroy()
        {
        }

        public override void Update()
        {
        }

        public override RenderTexture Draw()
        {
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            DrawGame();
            DrawUI();

            RenderTexture.Begin(mainRendertexture);
            Renderer.Clear(new Color(255, 0, 0, 255));
            Renderer.DrawSprite(gameRenderTexture, new Rectangle(0, 0, 64, 64), new Color(255, 255, 255, 255));
            Renderer.DrawSprite(uiRenderTexture, new Rectangle(0, 0, 64, 64), new Color(255, 255, 255, 255));
            RenderTexture.End();

            return mainRendertexture;
        }

        private void DrawGame()
        {
            RenderTexture.Begin(gameRenderTexture);
            Renderer.Clear(new Color(0, 0, 0, 255));
            mainTileMap.Draw();
            RenderTexture.End();
        }

        private void DrawUI()
        {
            RenderTexture.Begin(uiRenderTexture);
            RenderTexture.End();
        }
    }
}
