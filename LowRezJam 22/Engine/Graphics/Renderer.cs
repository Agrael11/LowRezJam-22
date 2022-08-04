using LowRezJam22.Helpers;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace LowRezJam22.Engine.Graphics
{
    internal static class Renderer
    {
        private static bool _initialized = false;
        private static Color _lastColor = new(1, 1, 1, 1);
        private static int _vertexBufferObject;
        private static int _elementBufferObject;
        private static int _vertexArrayObject;
        private static int _indiciesLength;
        private static Shader _shader = new();

        public static void Init(Shader? shader)
        {
            if (shader is not null)
            {
                SetShader(shader);
            }
        }

        public static void SetShader(Shader shader)
        {
            if (_shader.Handle == shader.Handle)
                return;

            _shader = shader;
            if (_initialized)
            {
                DeleteBuffers();
            }
            _initialized = true;

            BeginGenerateVertexArrayObject();

            GenerateBufferObjects();

            EndGenerateVertexArrayObject();

        }

        public static void SetShaderSimple(Shader shader)
        {
            _shader = shader;
        }

        private static void BeginGenerateVertexArrayObject()
        {
            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);
        }

        private static void EndGenerateVertexArrayObject()
        {
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            int TexCoordLocation = _shader.GetAttributeLocation("aTexCoord");
            GL.EnableVertexAttribArray(TexCoordLocation);
            GL.VertexAttribPointer(TexCoordLocation, 2, VertexAttribPointerType.Float,
                false, 5 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(0);
        }

        private static void GenerateBufferObjects()
        {
            float[] vertices =
           {
                +1f, +1f, 0f, 1f, 0f, //top right
                +1f, -1f, 0f, 1f, 1f, //bottom right
                -1f, -1f, 0f, 0f, 1f, //bottom left
                -1f, +1f, 0f, 0f, 0f //top left
            };

            uint[] indicies =
            {
                0, 1, 3,
                1, 2, 3
            };
            _indiciesLength = indicies.Length;

            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float),
                vertices, BufferUsageHint.StaticDraw);

            _elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indicies.Length * sizeof(uint),
                indicies, BufferUsageHint.StaticDraw);
        }

        private static void DeleteBuffers()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.DeleteBuffer(_vertexBufferObject);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.DeleteBuffer(_elementBufferObject);
            GL.BindVertexArray(_vertexArrayObject);
            GL.DeleteVertexArray(_vertexArrayObject);
        }

        public static void Clear()
        {
            if (!_initialized)
            {
                Logger.Log(Logger.Levels.Error, "Renderer not initialized");
                return;
            }
            GL.ClearColor(_lastColor);
            GL.Clear(ClearBufferMask.ColorBufferBit);
        }

        public static void Clear(Color clearColor)
        {
            if (!_initialized)
            {
                Logger.Log(Logger.Levels.Error, "Renderer not initialized");
                return;
            }
            _lastColor = clearColor;
            GL.ClearColor(clearColor);
            GL.Clear(ClearBufferMask.ColorBufferBit);
        }

        public static void DrawSprite(List<Texture> textures, Rectangle destination, Color color, float rotation = 0, bool horizontalFlip = false, bool verticalFlip = false)
        {
            if (!_initialized)
            {
                Logger.Log(Logger.Levels.Error, "Renderer not initialized");
                return;
            }

            if (Game.Instance is null)
            {
                Logger.Log(Logger.Levels.Error, "Game is not initialized");
                return;
            }
            
            if (textures is not null && textures.Count > 0 && textures[0].GetType() == typeof(RenderTexture))
            {
                verticalFlip = !verticalFlip;
            }

            float x = destination.X / (Game.Instance.Viewport.Width / 2);
            float y = -destination.Y / (Game.Instance.Viewport.Height / 2);
            float width = destination.Width / (Game.Instance.Viewport.Width);
            float height = destination.Height / (Game.Instance.Viewport.Height);

            Matrix4 translationMatrix = Matrix4.CreateTranslation(x, y, 0);
            Matrix4 rotationMatrix = Matrix4.CreateRotationZ((float)Math.Tau - rotation);
            Matrix4 scaleMatrix = Matrix4.CreateTranslation(1f, -1f, 0) *
                Matrix4.CreateScale(width, height, 1f) *
                Matrix4.CreateTranslation(-1f, 1f, 0);
            Matrix4 flipMatrix = Matrix4.Identity;
            if (horizontalFlip)
            {
                flipMatrix *= Matrix4.CreateScale(-1f, 1f, 1f);
            }
            if (verticalFlip)
            {
                flipMatrix *= Matrix4.CreateScale(1f, -1f, 1f);
            }

            _shader.UseShader();

            Matrix4 transformationMatrix = rotationMatrix * flipMatrix * scaleMatrix * translationMatrix;
            _shader.SetMatrix4("transform", ref transformationMatrix);
            Vector4 vecColor = color;
            _shader.SetVector4("color", ref vecColor);

            if (textures is not null && textures.Count > 0)
            {
                for (int i = 0; i < textures.Count; i++)
                {
                    _shader.SetInt("texture" + (i + 1).ToString(), i);

                    textures[i].UseTexture(TextureUnit.Texture0 + i);
                }
            }
            GL.BindVertexArray(_vertexArrayObject);
            GL.DrawElements(PrimitiveType.Triangles, _indiciesLength, DrawElementsType.UnsignedInt, 0);
        }


        public static void DrawSprite(Texture texture, Rectangle destination, Color color, float rotation = 0, bool horizontalFlip = false, bool verticalFlip = false)
        {
            if (!_initialized)
            {
                Logger.Log(Logger.Levels.Error, "Renderer not initialized");
                return;
            }

            if (Game.Instance is null)
            {
                Logger.Log(Logger.Levels.Error, "Game is not initialized");
                return;
            }
            if (texture.GetType() == typeof(RenderTexture))
            {
                verticalFlip = !verticalFlip;
            }

            float x = destination.X / (Game.Instance.Viewport.Width / 2);
            float y = -destination.Y / (Game.Instance.Viewport.Height / 2);
            float width = destination.Width / (Game.Instance.Viewport.Width);
            float height = destination.Height / (Game.Instance.Viewport.Height);

            Matrix4 translationMatrix = Matrix4.CreateTranslation(x, y, 0);
            Matrix4 rotationMatrix = Matrix4.CreateRotationZ((float)Math.Tau - rotation);
            Matrix4 scaleMatrix = Matrix4.CreateTranslation(1f, -1f, 0) *
                Matrix4.CreateScale(width, height, 1f) *
                Matrix4.CreateTranslation(-1f, 1f, 0);
            Matrix4 flipMatrix = Matrix4.Identity;
            if (horizontalFlip)
            {
                flipMatrix *= Matrix4.CreateScale(-1f, 1f, 1f);
            }
            if (verticalFlip)
            {
                flipMatrix *= Matrix4.CreateScale(1f, -1f, 1f);
            }

            _shader.UseShader();

            Matrix4 transformationMatrix = rotationMatrix * flipMatrix * scaleMatrix * translationMatrix;
            _shader.SetMatrix4("transform", ref transformationMatrix);
            Vector4 vecColor = color;
            _shader.SetVector4("color", ref vecColor);

            _shader.SetInt("texture1", 0);

            texture.UseTexture(TextureUnit.Texture0);

            GL.BindVertexArray(_vertexArrayObject);
            GL.DrawElements(PrimitiveType.Triangles, _indiciesLength, DrawElementsType.UnsignedInt, 0);
        }


        public static void DrawSprite(ComplexSpriteDefintion spriteDefinition, Rectangle destination, Color color, float rotation = 0, bool horizontalFlip = false, bool verticalFlip = false)
        {
            if (!_initialized)
            {
                Logger.Log(Logger.Levels.Error, "Renderer not initialized");
                return;
            }

            if (Game.Instance is null)
            {
                Logger.Log(Logger.Levels.Error, "Game is not initialized");
                return;
            }

            if (spriteDefinition.Texture.GetType() == typeof(RenderTexture))
            {
                verticalFlip = !verticalFlip;
            }

            float x = destination.X / (Game.Instance.Viewport.Width / 2);
            float y = -destination.Y / (Game.Instance.Viewport.Height / 2);
            float width = destination.Width / (Game.Instance.Viewport.Width);
            float height = destination.Height / (Game.Instance.Viewport.Height);

            Matrix4 translationMatrix = Matrix4.CreateTranslation(x, y, 0);
            Matrix4 rotationMatrix = Matrix4.CreateRotationZ((float)Math.Tau - rotation);
            Matrix4 scaleMatrix = Matrix4.CreateTranslation(1f, -1f, 0) *
                Matrix4.CreateScale(width, height, 1f) *
                Matrix4.CreateTranslation(-1f, 1f, 0);
            Matrix4 flipMatrix = Matrix4.Identity;
            if (horizontalFlip)
            {
                flipMatrix *= Matrix4.CreateScale(-1f, 1f, 1f);
            }
            if (verticalFlip)
            {
                flipMatrix *= Matrix4.CreateScale(1f, -1f, 1f);
            }

            _shader.UseShader();

            Matrix4 transformationMatrix = rotationMatrix * flipMatrix * scaleMatrix * translationMatrix;
            _shader.SetMatrix4("transform", ref transformationMatrix);
            Vector4 vecColor = color;
            _shader.SetVector4("color", ref vecColor);

            spriteDefinition.Shader.SetInt("texture1", 0);

            spriteDefinition.Texture.UseTexture(TextureUnit.Texture0);

            GL.BindVertexArray(spriteDefinition.VertexArrayObject);
            GL.DrawElements(PrimitiveType.Triangles, spriteDefinition.IndiciesLength, DrawElementsType.UnsignedInt, 0);
        }
    }
}
