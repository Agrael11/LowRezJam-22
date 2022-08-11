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
        public float X { get; set; }
        public float Y { get; set; }
        private GameScene? _parentScene;
        private Gravity _heading = Gravity.DOWN;
        private int _movementHeading = -1;
        private float _velocity = 0;
        private float _maxVelocity = 2;
        private float _gravity = 3;
        private float _movementSpeed = 5;
        private bool _grounded = false;

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
            if (Moving)
            {
                _velocity += _gravity * (float)args.Time;
                if (_velocity > _maxVelocity) _velocity = _maxVelocity;
                if (_velocity < -_maxVelocity) _velocity = -_maxVelocity;
                switch (_heading)
                {
                    case Gravity.UP:
                        UpdateUp(args);
                        break;
                    case Gravity.DOWN:
                    default:
                        UpdateDown(args);
                        break;
                }
            }
        }

        public void UpdateDown(FrameEventArgs args)
        {
            if (Game.Instance is null || Game.Instance.Scene is null)
                return;
            GameScene? scene = Game.Instance.Scene as GameScene;
            if (scene is null)
                return;

            float velocityChange = _velocity * (float)args.Time * 50;
            _grounded = false;
            Rectangle enemyRectangle = new(X, Y, 4, 4 + velocityChange);
            foreach (var tile in scene.MainTileMap.Tiles.Keys)
            {
                if (new Rectangle(tile.X * 4, tile.Y * 4, 4, 4).Intersects(enemyRectangle))
                {
                    if (Y > tile.Y * 4 - 4)
                    {
                        Y = tile.Y * 4 - 4;
                    }
                    _velocity = 0;
                    velocityChange = 0;
                    _grounded = true;
                }
            }
            Y += velocityChange;
            if (_grounded)
            {
                X += _movementHeading * (float)args.Time * _movementSpeed;
                enemyRectangle = new(X, Y, 4, 4);
                foreach (var tile in scene.MainTileMap.Tiles.Keys)
                {
                    if (new Rectangle(tile.X * 4, tile.Y * 4, 4, 4).Intersects(enemyRectangle))
                    {
                        _movementHeading *= -1;
                        X += _movementHeading * (float)args.Time * _movementSpeed;
                        return;
                    }
                }
                foreach (var tile in scene.EnemyBlocksTileMap.Tiles.Keys)
                {
                    if (new Rectangle(tile.X * 4, tile.Y * 4, 4, 4).Intersects(enemyRectangle))
                    {
                        _movementHeading *= -1;
                        X += _movementHeading * (float)args.Time * _movementSpeed;
                        return;
                    }
                }
            }
        }

        public void UpdateUp(FrameEventArgs args)
        {
            if (Game.Instance is null || Game.Instance.Scene is null)
                return;
            GameScene? scene = Game.Instance.Scene as GameScene;
            if (scene is null)
                return;

            float velocityChange = _velocity * (float)args.Time * 50;
            _grounded = false;
            Rectangle enemyRectangle = new(X, Y - velocityChange, 4, 4 + velocityChange);
            foreach (var tile in scene.MainTileMap.Tiles.Keys)
            {
                if (new Rectangle(tile.X * 4, tile.Y * 4, 4, 4).Intersects(enemyRectangle))
                {
                    if (Y < tile.Y * 4 + 4)
                    {
                        Y = tile.Y * 4 + 4;
                    }
                    _velocity = 0;
                    velocityChange = 0;
                    _grounded = true;
                }
            }
            Y -= velocityChange;
            if (_grounded)
            {
                X += _movementHeading * (float)args.Time * _movementSpeed;
                enemyRectangle = new(X, Y, 4, 4);
                foreach (var tile in scene.MainTileMap.Tiles.Keys)
                {
                    if (new Rectangle(tile.X * 4, tile.Y * 4, 4, 4).Intersects(enemyRectangle))
                    {
                        _movementHeading *= -1;
                        X += _movementHeading * (float)args.Time * _movementSpeed;
                        return;
                    }
                }
                foreach (var tile in scene.EnemyBlocksTileMap.Tiles.Keys)
                {
                    if (new Rectangle(tile.X * 4, tile.Y * 4, 4, 4).Intersects(enemyRectangle))
                    {
                        _movementHeading *= -1;
                        X += _movementHeading * (float)args.Time * _movementSpeed;
                        return;
                    }
                }
            }
        }

        public void Draw(FrameEventArgs args)
        {
            if (_parentScene is null)
                return;

            float x = (int)(X) - _parentScene.CameraX;
            float y = (int)(Y) - _parentScene.CameraY;
            float rotation = 0;
            switch (_heading)
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