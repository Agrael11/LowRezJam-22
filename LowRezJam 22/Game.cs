using LowRezJam22.Engine.Graphics;
using LowRezJam22.Helpers;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;


namespace LowRezJam22
{
    internal class Game : GameWindow
    {
        public static Game? Instance { get; private set; } = null;
        public Engine.SceneBase? Scene { get; set; } = null;
        public Rectangle Viewport = new(0, 0, 64, 64);
        public float WindowWidth { get; private set; } = 512;
        public float WindowHeight { get; private set; } = 512;

        public Game(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
            if (Instance is not null)
                Helpers.Logger.Log(Helpers.Logger.Levels.Fatal, "Only one game instance is allowed");
            Instance = this;
        }

        public void SetViewport(Rectangle viewport)
        {
            Viewport = viewport;
        }

        public void Init()
        {
            Renderer.Init(null);
            Scenes.GameScene nextscene = new();
            nextscene.Init();
            SwitchScene(nextscene);
        }

        public void SwitchScene(Engine.SceneBase newScene)
        {
            Scene = newScene;
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            if (Scene is not null)
            {
                Scene.Update();
            }
            base.OnUpdateFrame(args);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            if (Scene is null)
                return;
            
            RenderTexture rt = Scene.Draw();
            Renderer.DrawSprite(rt, new Rectangle(0, 0, 512, 512), Colors.White);

            Context.SwapBuffers();
            base.OnRenderFrame(args);
        }
    }
}
