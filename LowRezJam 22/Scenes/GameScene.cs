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
    enum Gravity { DOWN, UP, LEFT, RIGHT };

    internal class GameScene : SceneBase
    {
        public Shader MainShader { get; private set; }
        public TileMap MainTileMap { get; private set; }
        public TileMap EnemyBlocksTileMap { get; private set; }

        private RenderTexture _gameRenderTexture = new(64, 64);
        private RenderTexture _uiRenderTexture = new(64, 64);
        private RenderTexture _mainRendertexture = new(64, 64);
        private Background _background = new();
        private Background _foreground = new();
        private bool _foregroundOn = true;
        public float CameraX { get; private set; } = 0;
        public float CameraY { get; private set; } = 0;
        public Player Player { get; private set; }
        public int CheckPointX = 18;
        public int CheckPointY = 40;
        public float SandX = 0;
        public static Gravity GravityDirection { get; set; } = Gravity.DOWN;
        public static Gravity OriginalGravityDirection { get; set; } = Gravity.DOWN;
        public List<Enemy> Enemies { get; private set; } = new();
        public TileMap ObjectsTileMap { get; private set; }
        float rotation = 0;
        public string level = "Level0";
        public int Water = 0;
        public bool ShowingInfo = false;
        private int _mapWidth = 64;
        private int _mapHeight = 64;

        public GameScene()
        {
            MainShader = new NullShader();
            MainTileMap = new TileMap(0, 0, 0, 0, MainShader);
            ObjectsTileMap = new TileMap(0, 0, 0, 0, MainShader);
            EnemyBlocksTileMap = new TileMap(0, 0, 0, 0, MainShader);
        }

        public void Respawn()
        {
            Player.X = CheckPointX;
            Player.Y = CheckPointY;
            GravityDirection = OriginalGravityDirection;
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

        public void ShowWaterInfo()
        {
            if (Game.Instance is null)
                return;

            if (!ShowingInfo)
            {
                WaterScene waterScene = new();
                waterScene.Init();
                waterScene.Gamescene = this;
                ShowingInfo = true;
                Game.Instance.SwitchScene(waterScene);
            }
        }

        public override void Init()
        {
            if (Game.Instance is null)
                return;
            Game.Instance.SetViewport(0, 0, (int)Game.Instance.WindowWidth, (int)Game.Instance.WindowHeight);


            MainShader = new Shader("Assets/Shaders/MainShader.vert", "Assets/Shaders/MainShader.frag");
            Renderer.SetShader(MainShader);

            EnemyBlocksTileMap = new TileMap(0, 0, 4, 4, MainShader);
            ObjectsTileMap = new TileMap(0, 0, 4, 4, MainShader);
            MainTileMap = new TileMap(0, 0, 4, 4, MainShader);


            _foreground.Layers.Add((Colors.White, new("Assets/Backgrounds/Sand.png"), true));
            _foreground.ParallaxStrength = 3;



            List<Texture> playerMoveTextures = new();
            for (int i = 0; i < 4; i++)
            {
                playerMoveTextures.Add(new Texture("Assets/Player/CharacterWalk_" + i + ".png"));
            }
            Texture playerJumpTexture = new Texture("Assets/Player/CharacterJump.png");
            Player = new(18, 20, playerMoveTextures, playerJumpTexture);


            LoadLevel(level);
        }

        public void LoadLevel(string level)
        {
            LevelDefinition definition = LevelDefinitions.Defintions[level];
            _background.Layers.Clear();
            Water = 0;
            switch (definition.Background)
            {
                case 0:
                    _background.Layers.Add((Colors.White, new("Assets/Backgrounds/OrangeSky.png"), false));
                    _background.Layers.Add((new(255, 236, 204, 255), new("Assets/Backgrounds/Clouds.png"), true));
                    _background.ParallaxStrength = 1;
                    _foregroundOn = true;
                    break;
                case 1:
                    _background.Layers.Add((Colors.White, new("Assets/Backgrounds/OrangeSky.png"), false));
                    _background.Layers.Add((new(255, 236, 204, 255), new("Assets/Backgrounds/Clouds.png"), true));
                    _background.ParallaxStrength = 1;
                    _foregroundOn = false;
                    break;
                case 2:
                    _background.Layers.Add((Colors.White, new("Assets/Backgrounds/BlueSky.png"), false));
                    _background.Layers.Add((new(234, 244, 255, 255), new("Assets/Backgrounds/Clouds.png"), true));
                    _background.ParallaxStrength = 1;
                    _foregroundOn = true;
                    break;
                case 3:
                    _background.Layers.Add((Colors.White, new("Assets/Backgrounds/BlueSky.png"), false));
                    _background.Layers.Add((new(234, 244, 255, 255), new("Assets/Backgrounds/Clouds.png"), true));
                    _background.ParallaxStrength = 1;
                    _foregroundOn = false;
                    break;
                case 4:
                    _background.Layers.Add((Colors.White, new("Assets/Backgrounds/OrangeSky.png"), false));
                    _background.Layers.Add((new(255, 236, 204, 255), new("Assets/Backgrounds/Clouds.png"), true));
                    _background.Layers.Add((Colors.White, new("Assets/Backgrounds/Temple.png"), true));
                    _background.ParallaxStrength = 1;
                    _foregroundOn = false;
                    break;
                case 5:
                    _background.Layers.Add((Colors.White, new("Assets/Backgrounds/BlueSky.png"), false));
                    _background.Layers.Add((new(234, 244, 255, 255), new("Assets/Backgrounds/Clouds.png"), true));
                    _background.Layers.Add((Colors.White, new("Assets/Backgrounds/Temple.png"), true));
                    _background.ParallaxStrength = 1;
                    _foregroundOn = false;
                    break;
                case 6:
                    _background.Layers.Add((Colors.White, new("Assets/Backgrounds/CaveSolid.png"), true));
                    _background.ParallaxStrength = 1;
                    _foregroundOn = false;
                    break;
                case 7:
                    _background.Layers.Add((Colors.White, new("Assets/Backgrounds/Cave.png"), true));
                    _background.ParallaxStrength = 1;
                    _foregroundOn = false;
                    break;
                case 8:
                    _background.Layers.Add((Colors.White, new("Assets/Backgrounds/Stars.png"), true));
                    _background.Layers.Add((Colors.White, new("Assets/Backgrounds/Planets.png"), false));
                    _background.ParallaxStrength = 1;
                    _foregroundOn = false;
                    break;
            }

            Texture gravityUp = new("Assets/SpecialObjects/GravitySwitchUp.png");
            Texture gravityDown = new("Assets/SpecialObjects/GravitySwitchDown.png");
            Texture water = new("Assets/SpecialObjects/WaterDropplet.png");
            Texture flag = new("Assets/SpecialObjects/Flag.png");
            TileDefinition gravityUpTD = new(gravityUp) { TileID = "GravityUp" };
            TileDefinition gravityDownTD = new(gravityDown) { TileID = "GravityDown" };
            TileDefinition waterTD = new(water) { TileID = "Water" };
            TileDefinition flagTD = new(flag) { TileID = "Flag" };


            string[] levelSplit = definition.Map.Split('\n');
            MainTileMap.Clear();
            EnemyBlocksTileMap.Clear();
            Enemies.Clear();
            ObjectsTileMap.Clear();
            _mapHeight = levelSplit.Length * 4;

            for (int y = 0; y < levelSplit.Length; y++)
            {
                for (int x = 0; x < levelSplit[y].Length; x++)
                {
                    if (x*4 > _mapWidth)
                    {
                        _mapWidth = x * 4;
                    }
                    if (levelSplit[y][x] == 'x')
                    {
                        MainTileMap.SetTileAt(x, y, definition.mainTile);
                    }
                    else if (levelSplit[y][x] == 'y')
                    {
                        EnemyBlocksTileMap.SetTileAt(x, y, definition.mainTile);
                    }
                    else if (levelSplit[y][x] == 'd')
                    {
                        ObjectsTileMap.SetTileAt(x, y, gravityDownTD);
                    }
                    else if (levelSplit[y][x] == 'u')
                    {
                        ObjectsTileMap.SetTileAt(x, y, gravityUpTD);
                    }
                    else if (levelSplit[y][x] == 'w')
                    {
                        Water++;
                        ObjectsTileMap.SetTileAt(x, y, waterTD);
                    }
                    else if (levelSplit[y][x] == 'f')
                    {
                        ObjectsTileMap.SetTileAt(x, y, flagTD);
                    }
                    else if (levelSplit[y][x] == 'a')
                    {
                        Enemies.Add(new Enemy(definition.enemyTexture1, this, x * 4, y * 4, Gravity.DOWN, false));
                    }
                    else if (levelSplit[y][x] == 'b')
                    {
                        Enemies.Add(new Enemy(definition.enemyTexture1, this, x * 4, y * 4, Gravity.UP, false));
                    }
                    else if (levelSplit[y][x] == 'A')
                    {
                        Enemies.Add(new Enemy(definition.enemyTexture2, this, x * 4, y * 4, Gravity.DOWN, false));
                    }
                    else if (levelSplit[y][x] == 'B')
                    {
                        Enemies.Add(new Enemy(definition.enemyTexture2, this, x * 4, y * 4, Gravity.UP, false));
                    }
                    else if (levelSplit[y][x] == 'c')
                    {
                        Enemies.Add(new Enemy(definition.enemyTexture1, this, x * 4, y * 4, Gravity.DOWN, true));
                    }
                    else if (levelSplit[y][x] == 'e')
                    {
                        Enemies.Add(new Enemy(definition.enemyTexture1, this, x * 4, y * 4, Gravity.UP, true));
                    }
                    else if (levelSplit[y][x] == 'C')
                    {
                        Enemies.Add(new Enemy(definition.enemyTexture2, this, x * 4, y * 4, Gravity.DOWN, true));
                    }
                    else if (levelSplit[y][x] == 'E')
                    {
                        Enemies.Add(new Enemy(definition.enemyTexture2, this, x * 4, y * 4, Gravity.UP, true));
                    }
                    else if (levelSplit[y][x] == 'p')
                    {
                        CheckPointX = x * 4;
                        CheckPointY = y * 4;
                        OriginalGravityDirection = Gravity.DOWN;
                    }
                    else if (levelSplit[y][x] == 'P')
                    {
                        CheckPointX = x * 4;
                        CheckPointY = y * 4;
                        OriginalGravityDirection = Gravity.UP;
                    }
                }
            }

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
                    rotation -= 0.1f;
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
            _background.Draw((int)CameraX, (int)CameraY);
            Renderer.DrawSprite(_gameRenderTexture, new Rectangle(0, 0, 64, 64), Colors.White, rotation);
            if (_foregroundOn)
            {
                _foreground.Draw((int)(CameraX + SandX), (int)CameraY);
            }
            Renderer.DrawSprite(_uiRenderTexture, new Rectangle(0, 0, 64, 64), Colors.White);
            RenderTexture.End();

            SandX += (float)args.Time * 20;

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
            MainTileMap.Y = -(int)CameraY;
            MainTileMap.Draw();

            //DEBUG DRAW ENEMY BLOCKERS
            /*EnemyBlocksTileMap.X = -(int)CameraX;
            EnemyBlocksTileMap.Y = (int)CameraY;
            EnemyBlocksTileMap.Draw();*/

            ObjectsTileMap.X = -(int)CameraX;
            ObjectsTileMap.Y = -(int)CameraY;
            ObjectsTileMap.Draw();

            CameraX = (int)Player.X - 28;
            CameraY = (int)Player.Y - 26;

            if (CameraX < 0)
            {
                CameraX = 0;
            }
            if (CameraX > _mapWidth-60)
            {
                CameraX = _mapWidth-60;
            }
            if (CameraY < 0)
            {
                CameraY = 0;
            }
            if (CameraY > _mapHeight - 68)
            {
                CameraY = _mapHeight - 68;
            }
            Game.Instance.Title = CameraY.ToString();

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
