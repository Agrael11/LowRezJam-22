using LowRezJam22.Engine.Graphics;
using LowRezJam22.Helpers;
using LowRezJam22.Scenes;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LowRezJam22
{
    internal class Player
    {
        public float X { get; set; } = 0;
        public float Y { get; set; } = 0;

        private int _movingStatus = 0;
        private float _velocity = 0;
        private float _maxVelocity = 2;
        private float _gravity = 3;
        private float _playerSpeed = 30;
        private List<Texture> _movingTexture;
        private Texture _jumpingTexture;
        private bool _grounded = false;
        private float _anim = 0;
        private bool _lastDirection = true;
        private GameScene? _parentScene;

        public Player(int x, int y, List<Texture> movingTexture, Texture jumpingTexture)
        {
            X = x;
            Y = y;
            _movingTexture = movingTexture;
            _jumpingTexture = jumpingTexture;
            if (Game.Instance is null)
                return;
            if (Game.Instance.Scene is null)
                return;
            GameScene? scene = (GameScene)Game.Instance.Scene;
            if (scene is null)
                return;
            _parentScene = scene;
        }

        public void SwitchGravity(Gravity newGravity)
        {
            if (newGravity == Gravity.UP)
            {
                GameScene.GravityDirection = Gravity.UP;
                _velocity = 0.1f;
                Y--;
            }
            else
            {
                GameScene.GravityDirection = Gravity.DOWN;
                _velocity = 0.1f;
                Y++;
            }
        }

        public void Update(FrameEventArgs args)
        {
            if (Game.Instance is null)
                return;
            if (_parentScene is null)
                return;

            //Settíng a velocity

            _velocity += _gravity * (float)args.Time;

           

            if (Game.Instance.KeyboardState.IsKeyDown(Keys.W) && _grounded)
            {
                _velocity = -_maxVelocity / 1.4f;
                Game.PlaySFX("jump");
            }

            if (_velocity > _maxVelocity) _velocity = _maxVelocity;
            if (_velocity < -_maxVelocity) _velocity = -_maxVelocity;

            //Setting a moving status

            if (Game.Instance.KeyboardState.IsKeyDown(Keys.D) && !Game.Instance.IsKeyDown(Keys.A))
            {
                _movingStatus = 1;
            }
            else if (Game.Instance.KeyboardState.IsKeyDown(Keys.A))
            {
                _movingStatus = -1;
            }
            else
            {
                _movingStatus = 0;
            }

            if (Game.Instance.KeyboardState.IsKeyPressed(Keys.Escape))
            {
                Scenes.MenuScene mainMenu = new MenuScene();
                mainMenu.Init();
                Game.Instance.SwitchScene(mainMenu);
            }

            switch (GameScene.GravityDirection)
            {
                case Gravity.DOWN:
                    UpdateDown(args);
                    break;
                case Gravity.UP:
                    UpdateUp(args);
                    break;
                case Gravity.LEFT:
                    break;
                case Gravity.RIGHT:
                    break;
            }

            Rectangle playerRectangle = new(X, Y, 4, 12);
            foreach (Enemy enemy in _parentScene.Enemies)
            {
                Rectangle enemyRectangle = new(enemy.X, enemy.Y, 4, 4);
                if (playerRectangle.Intersects(enemyRectangle))
                {
                    Game.PlaySFX("hit");
                    _parentScene.Death();
                }
            }

            foreach (var key in _parentScene.ObjectsTileMap.Tiles.Keys)
            {
                Rectangle objectKey = new(key.X * 4, key.Y * 4, 4, 4);
                if (playerRectangle.Intersects(objectKey))
                {
                    switch (_parentScene.ObjectsTileMap.Tiles[key].TileID)
                    {
                        case "Water":
                            //Collect water
                            _parentScene.Water--;
                            _parentScene.ObjectsTileMap.RemoveTileAt(key.X, key.Y);
                            Game.PlaySFX("pickup");
                            break;
                        case "Flag":
                            if (_parentScene.Water > 0)
                            {
                                _parentScene.ShowWaterInfo();
                            }
                            else
                            {
                                Game.PlaySFX("end");
                                _parentScene.level = LevelDefinitions.Definitions[_parentScene.level].NextLevel;
                                _parentScene.Death();
                            }
                            break;
                        case "GravityUp":
                            if (GameScene.GravityDirection != Gravity.UP)
                            {
                                Game.PlaySFX("gravityUp");
                                SwitchGravity(Gravity.UP);
                            }
                            break;
                        case "GravityDown":
                            if (GameScene.GravityDirection != Gravity.DOWN)
                            {
                                Game.PlaySFX("gravityDown");
                                SwitchGravity(Gravity.DOWN);
                            }
                            break;
                    }
                }
            }
        }

        public void Reset()
        {

            _movingStatus = 0;
            _velocity = 0;

            _grounded = false;
            _anim = 0;
            _lastDirection = true;
        }

        public void UpdateDown(FrameEventArgs args)
        {
            if (_parentScene is null)
                return;

            float velocityChange = _velocity * (float)args.Time * 50;
            Y += velocityChange;

            Rectangle playerRectangle = new(X, Y - velocityChange, 4, velocityChange + 12);
            _grounded = false;

            foreach ((int X, int Y) key in _parentScene.MainTileMap.Tiles.Keys)
            {
                if (new Rectangle(key.X * 4, key.Y * 4, 4, 4).Intersects(playerRectangle))
                {
                    if (_velocity > 0)
                    {
                        if (Y > key.Y * 4 - 12)
                        {
                            Y = key.Y * 4 - 12;
                        }
                        _velocity = 0;
                        _grounded = true;
                    }
                    else if (_velocity < 0)
                    {
                        _velocity = 0;
                        if (Y < key.Y * 4 + 4)
                        {
                            Y = key.Y * 4 + 4;
                        }
                    }
                }
            }

            bool moving = false;
            float oldX = X;

            if (_movingStatus == 1)
            {
                X += (float)args.Time * _playerSpeed;
                moving = true;
                _lastDirection = true;

                playerRectangle = new(X, Y, 4, 12);
                foreach ((int X, int Y) key in _parentScene.MainTileMap.Tiles.Keys)
                {
                    if (new Rectangle(key.X * 4, key.Y * 4, 4, 4).Intersects(playerRectangle))
                    {
                        X = oldX;
                        moving = false;
                    }
                }
            }

            if (_movingStatus == -1)
            {
                oldX = X;
                X -= (float)args.Time * _playerSpeed;
                moving = true;
                _lastDirection = false;

                playerRectangle = new(X, Y, 4, 12);
                foreach ((int X, int Y) key in _parentScene.MainTileMap.Tiles.Keys)
                {
                    if (new Rectangle(key.X * 4, key.Y * 4, 4, 4).Intersects(playerRectangle))
                    {
                        X = oldX;
                        moving = false;
                    }
                }
            }

            if (_grounded)
            {
                if (moving)
                {
                    _anim += (float)args.Time * 5;
                }
                else
                {
                    _anim = 0;
                }
            }
        }

        public void UpdateUp(FrameEventArgs args)
        {
            if (_parentScene is null)
                return;

            float velocityChange = _velocity * (float)args.Time * 50;
            Y -= velocityChange;

            Rectangle playerRectangle = new(X, Y - velocityChange, 4, velocityChange + 12);
            _grounded = false;

            foreach ((int X, int Y) key in _parentScene.MainTileMap.Tiles.Keys)
            {
                if (new Rectangle(key.X * 4, key.Y * 4, 4, 4).Intersects(playerRectangle))
                {
                    if (_velocity < 0)
                    {
                        if (Y > key.Y * 4 - 12)
                        {
                            Y = key.Y * 4 - 12;
                        }
                        _velocity = 0;
                    }
                    else if (_velocity > 0)
                    {
                        _velocity = 0;
                        if (Y < key.Y * 4 + 4)
                        {
                            Y = key.Y * 4 + 4;
                        }
                        _grounded = true;
                    }
                }
            }

            bool moving = false;
            float oldX = X;

            if (_movingStatus == 1)
            {
                X -= (float)args.Time * _playerSpeed;
                moving = true;
                _lastDirection = true;

                playerRectangle = new(X, Y, 4, 12);
                foreach ((int X, int Y) key in _parentScene.MainTileMap.Tiles.Keys)
                {
                    if (new Rectangle(key.X * 4, key.Y * 4, 4, 4).Intersects(playerRectangle))
                    {
                        X = oldX;
                        moving = false;
                    }
                }
            }
            if (_movingStatus == -1)
            {
                oldX = X;
                X += (float)args.Time * _playerSpeed;
                moving = true;
                _lastDirection = false;

                playerRectangle = new(X, Y, 4, 12);
                foreach ((int X, int Y) key in _parentScene.MainTileMap.Tiles.Keys)
                {
                    if (new Rectangle(key.X * 4, key.Y * 4, 4, 4).Intersects(playerRectangle))
                    {
                        X = oldX;
                        moving = false;
                    }
                }
            }

            if (_grounded)
            {
                if (moving)
                {
                    _anim += (float)args.Time * 5;
                }
                else
                {
                    _anim = 0;
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
            switch (GameScene.GravityDirection)
            {
                case Gravity.DOWN:
                    rotation = 0f;
                    break;
                case Gravity.UP:
                    rotation = 3.14f;
                    break;
            }

            if (_grounded)
            {
                int index = (int)(_anim) % 4;
                Renderer.DrawSprite(_movingTexture[index], new Rectangle(x, y, 4, 12), Colors.White, rotation, !_lastDirection);
            }
            else
            {
                Renderer.DrawSprite(_jumpingTexture, new Rectangle(x, y, 4, 12), Colors.White, rotation, !_lastDirection);
            }
        }
    }
}
