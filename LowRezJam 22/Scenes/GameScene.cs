using LowRezJam22.Engine;
using LowRezJam22.Engine.Graphics;
using LowRezJam22.Engine.Tiles;
using LowRezJam22.Helpers;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace LowRezJam22.Scenes
{
    internal class GameScene : SceneBase
    {
        private Shader _mainShader;
        private TileMap _mainTileMap;

        private RenderTexture _gameRenderTexture = new(64, 64);
        private RenderTexture _uiRenderTexture = new(64, 64);
        private RenderTexture _mainRendertexture = new(64, 64);
        private Background orangeBackground = new();
        private Background blueBackground = new();
        private float CameraX = 0;
        private float CameraY = 0;

        private string tempString =
            "...........xxxxxxx--------------------------------\n" +
            "xxxxxxxxxxxxxxxxxxxxxxxxxxx----xxxxxxxxxxxxxxxxxxx\n" +
            "xxxxxxxxxxxxxxxxxxxxxxxxxxx----xxxxxxxxxxxxxxxxxxx\n" +
            "xxxxxxxxxxxxxxxxxxxxxxxxxxx----xxxxxxxxxxxxxxxxxxx\n";

        public GameScene()
        {
            _mainShader = new Shader("Assets/Shaders/MainShader.vert", "Assets/Shaders/MainShader.frag");
            Renderer.SetShader(_mainShader);

            orangeBackground.Layers.Add((Colors.White, new("Assets/Backgrounds/OrangeSky.png"), false));
            orangeBackground.Layers.Add((new(255, 236, 204, 255), new("Assets/Backgrounds/Clouds.png"), true));
            blueBackground.Layers.Add((Colors.White, new("Assets/Backgrounds/BlueSky.png"), false));
            blueBackground.Layers.Add((new(234, 244, 255, 255), new("Assets/Backgrounds/Clouds.png"), true));

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
            _mainTileMap = new TileMap(0, 0, 4, 4, _mainShader);
            string[] temp = tempString.Split('\n');
            for (int y = 0; y < temp.Length; y++)
            {
                for (int x = 0; x < temp[y].Length; x++)
                {
                    if (temp[y][x] == 'x')
                    {
                        _mainTileMap.SetTileAt(x,y+13, tile);
                    }
                }
            }
        }

        public override void Init()
        {
            if (Game.Instance is null)
                return;
            Game.Instance.SetViewport(0, 0, (int)Game.Instance.WindowWidth, (int)Game.Instance.WindowHeight);
        }

        public override void Destroy()
        {
        }

        public override void Update(FrameEventArgs args)
        {
            if (Game.Instance is null)
                return;

            if (Game.Instance.KeyboardState.IsKeyDown(Keys.D))
            {
                CameraX += 0.5f;
            }
            if (Game.Instance.KeyboardState.IsKeyDown(Keys.A))
            {
                CameraX -= 0.5f;
            }
            if (Game.Instance.KeyboardState.IsKeyDown(Keys.W))
            {
                CameraY += 0.5f;
            }
            if (Game.Instance.KeyboardState.IsKeyDown(Keys.S))
            {
                CameraY -= 0.5f;
            }
            Game.Instance.Title = CameraX.ToString() + "_" + CameraY.ToString();
        }

        public override RenderTexture Draw(FrameEventArgs args)
        {
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            Renderer.Clear(Colors.Red);
            DrawGame();
            DrawUI();
            
            RenderTexture.Begin(_mainRendertexture);
            Renderer.Clear(new Color(255, 0, 0, 255));
            Renderer.DrawSprite(_gameRenderTexture, new Rectangle(0, 0, 64, 64), new Color(255, 255, 255, 255));
            Renderer.DrawSprite(_uiRenderTexture, new Rectangle(0, 0, 64, 64), new Color(255, 255, 255, 255));
            RenderTexture.End();

            return _mainRendertexture;
        }

        private void DrawGame()
        {
            if (Game.Instance is null)
            {
                return;
            }

            _gameRenderTexture.Begin();

            blueBackground.Draw((int)CameraX, (int)CameraY);

            _mainTileMap.X = -(int)CameraX;
            _mainTileMap.Y = (int)CameraY;
            _mainTileMap.Draw();
            
            RenderTexture.End();
        }

        private void DrawUI()
        {
            RenderTexture.Begin(_uiRenderTexture);
            RenderTexture.End();
        }
    }
}
