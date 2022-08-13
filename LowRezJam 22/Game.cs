using LowRezJam22.Engine.Graphics;
using LowRezJam22.Helpers;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;


namespace LowRezJam22
{
    internal class Game : GameWindow
    {
        public static Game? Instance { get; private set; } = null;
        public Engine.SceneBase? Scene { get; set; } = null;
        public Rectangle Viewport { get; private set; } = new(0, 0, 64, 64);
        public float WindowWidth { get; private set; } = 64*10;
        public float WindowHeight { get; private set; } = 64 * 10;

        public void ResizeGame(int scale)
        {
            WindowWidth = 64 * scale;
            WindowHeight = 64 * scale;
            Size = new OpenTK.Mathematics.Vector2i((int)WindowWidth, (int)WindowHeight);
            SetViewport(new Rectangle(0, 0, WindowWidth, WindowHeight));
        }

        public static void PlaySFX(string effect)
        {
            Engine.Sounds.SoundManager.Add("Assets/Sounds/" + effect + ".wav", 0.1f, false);
        }

        public void SetViewport(int X, int Y, int Width, int Height)
        {
            Viewport = new Rectangle(X,Y, Width, Height);
            GL.Viewport(X, Y, Width, Height);
        }

        public void SetViewport(Rectangle rectangle)
        {
            Viewport = rectangle;
            GL.Viewport((int)rectangle.X, (int)rectangle.Y, (int)rectangle.Width, (int)rectangle.Height);
        }

        public Game(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
            if (Instance is not null)
                Helpers.Logger.Log(Helpers.Logger.Levels.Fatal, "Only one game instance is allowed");
            Instance = this;
        }

        public void Init()
        {
            Renderer.Init(null);
            Scenes.MenuScene mainMenuScene = new();
            SwitchScene(mainMenuScene);
            mainMenuScene.Init();
            LevelDefinitions.LoadDefs();
        }
        
        public void StartGame()
        {
            Scenes.GameScene gamescene = new();
            SwitchScene(gamescene);
            gamescene.level = "Level4";
            gamescene.Init();
            Scenes.TutorialScene tutorial = new();
            tutorial.Gamescene = gamescene;
            tutorial.Init();
            SwitchScene(tutorial);
        }

        public void SwitchScene(Engine.SceneBase newScene)
        {
            Scene = newScene;
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            if (Scene is not null)
            {
                Scene.Update(args);
            }
            Engine.Sounds.SoundManager.Update();
            base.OnUpdateFrame(args);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            if (Scene is null)
                return;
            
            RenderTexture rt = Scene.Draw(args);
            Renderer.Clear(Colors.Black);
            Renderer.DrawSprite(rt, new Rectangle(0, 0, WindowWidth, WindowHeight), Colors.White);

            Context.SwapBuffers();
            base.OnRenderFrame(args);
        }
    }
}
