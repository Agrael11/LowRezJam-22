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
    internal class TutorialScene : SceneBase
    {
        public GameScene Gamescene;
        private Texture _tutorialInfoTexture;

        public TutorialScene()
        {
        }

        public override void Init()
        {
            texture = new RenderTexture(64, 64);
            _tutorialInfoTexture = new Texture("Assets/UI/MoveWith.png");
        }

        public override void Destroy()
        {
        }

        public override void Update(FrameEventArgs args)
        {
            if (Game.Instance is null)
                return;

            timer -= (float)args.Time*2;
            if (timer <= 0)
            {
                Game.Instance.SwitchScene(Gamescene);
            }

            Gamescene.ShowingInfo = false;
            Gamescene.Update(args);
        }

        RenderTexture texture;
        float timer = 10;

        public override RenderTexture Draw(FrameEventArgs args)
        {
            RenderTexture game = Gamescene.Draw(args);

            RenderTexture.Begin(texture);
            Renderer.Clear(new Color(0, 0, 0, 255));
            Renderer.DrawSprite(game, new Rectangle(0, 0, 64, 64), Colors.White);
            Renderer.DrawSprite(_tutorialInfoTexture, new Rectangle(0, 0, 64, 64), new Color(255, 255, 255, (int)(timer * 255)));
            RenderTexture.End();

            return texture;
        }
    }
}
