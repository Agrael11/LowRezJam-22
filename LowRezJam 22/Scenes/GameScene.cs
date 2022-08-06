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
        public Shader MainShader { get; private set; }
        public TileMap MainTileMap { get; private set; }

        private RenderTexture _gameRenderTexture = new(64, 64);
        private RenderTexture _uiRenderTexture = new(64, 64);
        private RenderTexture _mainRendertexture = new(64, 64);
        private Background _orangeBackground = new();
        private Background _blueBackground = new();
        public float CameraX { get; private set; } = 0;
        public float CameraY { get; private set; } = 0;
        public Player Player { get; private set; }

        private string tempString =
            "...........xxxxxxx--------------------------------\n" +
            "xxxxxxxxxxxxxxxxxxxxxxxxxxx----xxxxxxxxxxxxxxxxxxx\n" +
            "xxxxxxxxxxxxxxxxxxxxxxxxxxx----xxxxxxxxxxxxxxxxxxx\n" +
            "xxxxxxxxxxxxxxxxxxxxxxxxxxx----xxxxxxxxxxxxxxxxxxx\n";

        public GameScene()
        {
        }

        public override void Init()
        {
            if (Game.Instance is null)
                return;
            Game.Instance.SetViewport(0, 0, (int)Game.Instance.WindowWidth, (int)Game.Instance.WindowHeight);


            MainShader = new Shader("Assets/Shaders/MainShader.vert", "Assets/Shaders/MainShader.frag");
            Renderer.SetShader(MainShader);

            _orangeBackground.Layers.Add((Colors.White, new("Assets/Backgrounds/OrangeSky.png"), false));
            _orangeBackground.Layers.Add((new(255, 236, 204, 255), new("Assets/Backgrounds/Clouds.png"), true));
            _blueBackground.Layers.Add((Colors.White, new("Assets/Backgrounds/BlueSky.png"), false));
            _blueBackground.Layers.Add((new(234, 244, 255, 255), new("Assets/Backgrounds/Clouds.png"), true));

            List<Texture> desertTiles = new();
            for (int i = 0; i <= 20; i++)
            {
                desertTiles.Add(new Texture("Assets/Tiles/DesertTiles_" + i.ToString().PadLeft(2, '0') + ".png"));
            }

            SmartTiles.DesertTile tile = new(new() { desertTiles[0] }, new() { desertTiles[1], desertTiles[2] }, new() { desertTiles[3] },
                new() { desertTiles[4], desertTiles[5] }, new() { desertTiles[6], desertTiles[7] }, new() { desertTiles[8], desertTiles[9] },
                new() { desertTiles[10] }, new() { desertTiles[11], desertTiles[12] }, new() { desertTiles[13] },
                new() { desertTiles[14] }, new() { desertTiles[15] }, new() { desertTiles[16] },
                new() { desertTiles[17] }, new() { desertTiles[18] }, new() { desertTiles[19] },
                new() { desertTiles[20] });
            MainTileMap = new TileMap(0, 0, 4, 4, MainShader);
            string[] temp = tempString.Split('\n');
            for (int y = 0; y < temp.Length; y++)
            {
                for (int x = 0; x < temp[y].Length; x++)
                {
                    if (temp[y][x] == 'x')
                    {
                        MainTileMap.SetTileAt(x, y + 13, tile);
                    }
                }
            }

            List<Texture> playerMoveTextures = new();
            for (int i = 0; i < 4; i++)
            {
                playerMoveTextures.Add(new Texture("Assets/Player/CharacterWalk_" + i + ".png"));
            }
            Texture playerJumpTexture = new Texture("Assets/Player/CharacterJump.png");
            Player = new(0, 0, playerMoveTextures, playerJumpTexture);
        }

        public override void Destroy()
        {
        }

        public override void Update(FrameEventArgs args)
        {
            if (Game.Instance is null)
                return;

            CameraX = (int)(Player.X - 28);
            Game.Instance.Title = CameraX.ToString() + "_" + CameraY.ToString();

            Player.Update(args);
        }

        public override RenderTexture Draw(FrameEventArgs args)
        {
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            Renderer.Clear(Colors.Red);
            DrawGame(args);
            DrawUI(args);
            
            RenderTexture.Begin(_mainRendertexture);
            Renderer.Clear(new Color(255, 0, 0, 255));
            Renderer.DrawSprite(_gameRenderTexture, new Rectangle(0, 0, 64, 64), new Color(255, 255, 255, 255));
            Renderer.DrawSprite(_uiRenderTexture, new Rectangle(0, 0, 64, 64), new Color(255, 255, 255, 255));
            RenderTexture.End();

            return _mainRendertexture;
        }

        private void DrawGame(FrameEventArgs args)
        {
            if (Game.Instance is null)
            {
                return;
            }

            _gameRenderTexture.Begin();

            _blueBackground.Draw((int)CameraX, (int)CameraY);

            MainTileMap.X = -(int)CameraX;
            MainTileMap.Y = (int)CameraY;
            MainTileMap.Draw();

            Player.Draw(args);
            
            RenderTexture.End();
        }

        private void DrawUI(FrameEventArgs args)
        {
            RenderTexture.Begin(_uiRenderTexture);
            RenderTexture.End();
        }
    }
}
