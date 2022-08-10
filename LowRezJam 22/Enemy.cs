using LowRezJam22.Engine.Graphics;
using LowRezJam22.Helpers;
using LowRezJam22.Scenes;
using OpenTK.Windowing.Common;

namespace LowRezJam22
{
    internal class Enemy
    {
        public bool Moving { get; set; } = false;
        public Texture _texture;
        public int X { get; set; }
        public int Y { get; set; }
        private GameScene? _parentScene;
        private Gravity _heading = Gravity.DOWN;

        public Enemy(Texture texture, GameScene parentScene, int x, int y, Gravity heading, bool moving = false)
        {
            _texture = texture;
            _parentScene = parentScene;
            X = x;
            Y = y;
            _heading = heading;
            Moving = moving;
        }

        public void Update(FrameEventArgs args)
        {

        }

        public void Draw(FrameEventArgs args)
        {
            if (_parentScene is null)
                return;

            float x = (int)(X) - _parentScene.CameraX;
            float y = (int)(Y) - _parentScene.CameraY;
            float rotation = 0;
            switch (GameScene.GravityDirection)
            {
                case Gravity.DOWN:
                    rotation = 0f;
                    break;
                case Gravity.UP:
                    rotation = 3.14f;
                    break;
            }

            Renderer.DrawSprite(_texture, new Rectangle(x, y, 4, 4), Colors.White, rotation);
        }
    }
}
