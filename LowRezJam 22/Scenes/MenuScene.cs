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
    internal class MenuScene : SceneBase
    {
        private Dictionary<string, Texture> _textures = new();
        private int _page = 0;
        private int _selection = 0;
        RenderTexture texture;

        public MenuScene()
        {
            texture = new RenderTexture(64, 64);
        }

        public override void Init()
        {
            Shader MainShader = new("Assets/Shaders/MainShader.vert", "Assets/Shaders/MainShader.frag");
            Renderer.Init(MainShader);

            _textures.Add("BG", new Texture("Assets/Backgrounds/BlueSky.png"));
            _textures.Add("StartGame", new Texture("Assets/MenuItems/StartGame.png"));
            _textures.Add("Scale", new Texture("Assets/MenuItems/Scale.png"));
            _textures.Add("Exit", new Texture("Assets/MenuItems/Exit.png"));
            _textures.Add("Back", new Texture("Assets/MenuItems/Back.png"));
            _textures.Add("2", new Texture("Assets/MenuItems/2.png"));
            _textures.Add("4", new Texture("Assets/MenuItems/4.png"));
            _textures.Add("6", new Texture("Assets/MenuItems/6.png"));
            _textures.Add("8", new Texture("Assets/MenuItems/8.png"));
            _textures.Add("10", new Texture("Assets/MenuItems/10.png"));
            _textures.Add("12", new Texture("Assets/MenuItems/12.png"));
        }

        public override void Destroy()
        {
        }

        public override void Update(FrameEventArgs args)
        {
            if (Game.Instance is null)
                return;

            KeyboardState state = Game.Instance.KeyboardState.GetSnapshot();
            if (state.IsKeyPressed(Keys.S) || state.IsKeyPressed(Keys.Down))
            {
                _selection++;
                if (_page == 0)
                {
                    _selection %= 3;
                }
                else if (_page == 1)
                {
                    _selection %= 7;
                }
            }
            if (state.IsKeyPressed(Keys.W) || state.IsKeyPressed(Keys.Up))
            {
                _selection--;
                if (_page == 0)
                {
                    if (_selection < 0)
                    _selection += 3;
                }
                else if (_page == 1)
                {
                    if (_selection < 0)
                        _selection += 7;
                }
            }
            if (state.IsKeyPressed(Keys.Escape))
            {
                if (_page == 0)
                {
                    _selection = 2;
                }
                else if (_page == 1)
                {
                    _selection = 6;
                }
            }
            if (state.IsKeyPressed(Keys.Enter) || state.IsKeyPressed(Keys.Space))
            {
                Game.PlaySFX("Click");
                if (_page == 0)
                {
                    switch (_selection)
                    {
                        case 0:
                            Game.Instance.StartGame();
                            break;
                        case 1:
                            _page = 1;
                            _selection = 0;
                            break;
                        case 2:
                            Game.Instance.Close();
                            break;
                    }
                }
                else if (_page == 1)
                {
                    switch (_selection)
                    {
                        case 0:
                            Game.Instance.ResizeGame(2);
                            break;
                        case 1:
                            Game.Instance.ResizeGame(4);
                            break;
                        case 2:
                            Game.Instance.ResizeGame(6);
                            break;
                        case 3:
                            Game.Instance.ResizeGame(8);
                            break;
                        case 4:
                            Game.Instance.ResizeGame(10);
                            break;
                        case 5:
                            Game.Instance.ResizeGame(12);
                            break;
                        case 6:
                            _page = 0;
                            _selection = 0;
                            break;
                    }
                }
            }
        }

        public override RenderTexture Draw(FrameEventArgs args)
        {
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            RenderTexture.Begin(texture);
            Renderer.Clear(new Color(0, 0, 0, 255));
            Renderer.DrawSprite(_textures["BG"], new Rectangle(0, 0, 64, 64), Colors.White);
            if (_page == 0)
            {
                DrawMenuItem(_textures["StartGame"], 64, 0, 37, _selection == 0);
                DrawMenuItem(_textures["Scale"], 64, 0, 46, _selection == 1);
                DrawMenuItem(_textures["Exit"], 64, 0, 55, _selection == 2);
            }
            else
            {
                DrawMenuItem(_textures["Scale"], 64, 0, 10, false);
                DrawMenuItem(_textures["2"], 32, 0, 28, _selection == 0);
                DrawMenuItem(_textures["6"], 32, 0, 37, _selection == 2);
                DrawMenuItem(_textures["10"], 32, 0, 46, _selection == 4);
                DrawMenuItem(_textures["4"], 32, 32, 28, _selection == 1);
                DrawMenuItem(_textures["8"], 32, 32, 37, _selection == 3);
                DrawMenuItem(_textures["12"], 32, 32, 46, _selection == 5);
                DrawMenuItem(_textures["Back"], 64, 0, 55, _selection == 6);
            }
            RenderTexture.End();

            return texture;
        }

        public void DrawMenuItem(Texture texture, int X, int XOffset, int Y, bool active)
        {
            Renderer.DrawSprite(texture, new Rectangle(XOffset+(X-texture.Width)/2, Y, texture.Width, texture.Height), active ? Colors.Red : Colors.White);
        }
    }
}
