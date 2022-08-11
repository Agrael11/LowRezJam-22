using LowRezJam22.Engine;
using LowRezJam22.Engine.Graphics;
using LowRezJam22.Engine.Tiles;
using LowRezJam22.Helpers;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace LowRezJam22.Scenes
{
    enum Gravity { DOWN, UP, LEFT, RIGHT};

    internal class GameScene : SceneBase
    {
        public Shader MainShader { get; private set; }
        public TileMap MainTileMap { get; private set; }
        public TileMap EnemyBlocksTileMap { get; private set; }

        private RenderTexture _gameRenderTexture = new(64, 64);
        private RenderTexture _uiRenderTexture = new(64, 64);
        private RenderTexture _mainRendertexture = new(64, 64);
        private Background _orangeBackground = new();
        private Background _blueBackground = new();
        private Background _sandBackground = new();
        public float CameraX { get; private set; } = 0;
        public float CameraY { get; private set; } = 0;
        public Player Player { get; private set; }
        public int CheckPointX = 18;
        public int CheckPointY = 40;
        public float SandX = 0;
        public static Gravity GravityDirection = Gravity.DOWN;
        public List<Enemy> Enemies { get; private set; } = new();
        float rotation = 0;

        private string tempString =
            "xxxxxxxxxxxxxxxxxxxxxx----xxxxxxxxxxxxxxxxxxxxxxxx\n" +
            "xxxxxxxxxxxxxxxxxxxxxx----xxxxxxxxxxxxxxxxxxxxxxxx\n" +
            "xxxxxxxxxxxxxxxxxxxxxx----xxxxxxxxxxxxxxxxxxxxxxxx\n" +
            "xxxx---------xxxxx----------------------------xxxx\n" +
            "xxxx--------y-----y---------------------------xxxx\n" +
            "xxxx------------------------------------------xxxx\n" +
            "xxxx------------------------------------------xxxx\n" +
            "xxx--------------------------------------------xxx\n" +
            "--------------------------------------------------\n" +
            "--------------------------------------------------\n" +
            "xxx--------------------------------------------xxx\n" + 
            "xxx-------y-------y----------------------------xxx\n" +
            "xxx--------xxxxxxx-----------------------------xxx\n" +
            "xxxxxxxxxxxxxxxxxxxxxxxxxxx----xxxxxxxxxxxxxxxxxxx\n" +
            "xxxxxxxxxxxxxxxxxxxxxxxxxxx----xxxxxxxxxxxxxxxxxxx\n" +
            "xxxxxxxxxxxxxxxxxxxxxxxxxxx----xxxxxxxxxxxxxxxxxxx\n";

        public GameScene()
        {
            MainShader = new NullShader();
            MainTileMap = new TileMap(0, 0, 0, 0, MainShader);
            EnemyBlocksTileMap = new TileMap(0, 0, 0, 0, MainShader);
        }

        public void Respawn()
        {
            Player.X = CheckPointX;
            Player.Y = CheckPointY;
            Player.Reset();
        }
        
        public void Death()
        {
            if (Game.Instance is null)
                return;

            DeathScene deathScene = new();
            deathScene.Init();
            deathScene.Gamescene = this;
            Game.Instance.SwitchScene(deathScene);
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
            _orangeBackground.ParallaxStrength = 1;
            _blueBackground.Layers.Add((Colors.White, new("Assets/Backgrounds/BlueSky.png"), false));
            _blueBackground.Layers.Add((new(234, 244, 255, 255), new("Assets/Backgrounds/Clouds.png"), true));
            _blueBackground.ParallaxStrength = 1;
            _sandBackground.Layers.Add((Colors.White, new("Assets/Backgrounds/Sand.png"), true));
            _sandBackground.ParallaxStrength = 3;
            Texture cactus0 = new("Assets/Enemies/Cactus_0.png");
            Texture cactus1 = new("Assets/Enemies/Cactus_1.png");
            Enemies.Add(new(cactus0, this, 30, 48, Gravity.DOWN, false));
            Enemies.Add(new(cactus1, this, 46, 44, Gravity.DOWN, true));
            Enemies.Add(new(cactus1, this, 40, 12, Gravity.UP, true));
            TileDefinition tempTile = new(new("Assets/Tiles/DesertTiles_00.png"));

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
            EnemyBlocksTileMap = new TileMap(0, 0, 4, 4, MainShader);
            MainTileMap = new TileMap(0, 0, 4, 4, MainShader);
            string[] temp = tempString.Split('\n');
            for (int y = 0; y < temp.Length; y++)
            {
                for (int x = 0; x < temp[y].Length; x++)
                {
                    if (temp[y][x] == 'x')
                    {
                        MainTileMap.SetTileAt(x, y, tile);
                    }
                    if (temp[y][x] == 'y')
                    {
                        EnemyBlocksTileMap.SetTileAt(x, y, tempTile);
                    }
                }
            }

            List<Texture> playerMoveTextures = new();
            for (int i = 0; i < 4; i++)
            {
                playerMoveTextures.Add(new Texture("Assets/Player/CharacterWalk_" + i + ".png"));
            }
            Texture playerJumpTexture = new Texture("Assets/Player/CharacterJump.png");
            Player = new(18, 20, playerMoveTextures, playerJumpTexture);
            Respawn();
        }

        public override void Destroy()
        {
        }

        public override void Update(FrameEventArgs args)
        {
            if (Game.Instance is null)
                return;

            foreach (Enemy enemy in Enemies)
            {
                enemy.Update(args);
            }

            Player.Update(args);
        }

        public override RenderTexture Draw(FrameEventArgs args)
        {
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            Renderer.Clear(Colors.Black);
            DrawGame(args);
            DrawUI(args);

            rotation %= 6.28f;
            while (rotation < 0) rotation += 6.28f;
            if (GravityDirection == Gravity.DOWN)
            {
                if (rotation != 0)
                {
                    rotation-=0.1f;
                    if (rotation < 0.1f) rotation = 0;
                }
            }
            else if (GravityDirection == Gravity.UP)
            {
                if (rotation != 3.14f)
                {
                    rotation -= 0.1f;
                    if (rotation > 3f && rotation < 3.2f) rotation = 3.14f;
                }
            }

            RenderTexture.Begin(_mainRendertexture);
            Renderer.Clear(Colors.Black);
            _orangeBackground.Draw((int)CameraX, (int)CameraY);
            Renderer.DrawSprite(_gameRenderTexture, new Rectangle(0, 0, 64, 64), Colors.White, rotation);
            _sandBackground.Draw((int)(CameraX + SandX), (int)CameraY);
            Renderer.DrawSprite(_uiRenderTexture, new Rectangle(0, 0, 64, 64), Colors.White);
            RenderTexture.End();

            SandX += (float)args.Time*20;

            return _mainRendertexture;
        }

        private void DrawGame(FrameEventArgs args)
        {
            if (Game.Instance is null)
            {
                return;
            }

            _gameRenderTexture.Begin();
            Renderer.Clear(new Color(0, 0, 0, 0));
            MainTileMap.X = -(int)CameraX;
            MainTileMap.Y = (int)CameraY;
            MainTileMap.Draw();

            //DEBUG DRAW ENEMY BLOCKERS
            /*EnemyBlocksTileMap.X = -(int)CameraX;
            EnemyBlocksTileMap.Y = (int)CameraY;
            EnemyBlocksTileMap.Draw();*/

            CameraX = (int)Player.X - 28;

            foreach (Enemy enemy in Enemies)
            {
                enemy.Draw(args);
            }

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
