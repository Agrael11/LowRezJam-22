using LowRezJam22.Helpers;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LowRezJam22.Engine.Graphics
{
    internal class ComplexSpriteDefintion
    {
        private Rectangle _sourceRectangle;
        public Rectangle SourceRectangle { get { return _sourceRectangle; } set { SetSourceRectangle(value); } }
        private int _vertexBufferObject = -1;
        public int VertexBufferObject { get { return _vertexBufferObject; } }
        private int _elementBufferObject = -1;
        public int ElementBufferObject { get { return _elementBufferObject; } }
        private int _vertexArrayObject = -1;
        public int VertexArrayObject { get { return _vertexArrayObject; } }
        private int _indiciesLength;
        public int IndiciesLength { get { return _indiciesLength; } }
        private Shader _shader;
        public Shader Shader { get { return _shader; } }
        private Texture _texture;
        public Texture Texture { get { return _texture; } }

        public void SetSourceRectangle(Rectangle rectangle)
        {
            _sourceRectangle = rectangle;

            DeleteBuffers();

            BeginGenerateVertexArrayObject();

            GenerateBufferObjects();

            EndGenerateVertexArrayObject();
        }

        public ComplexSpriteDefintion(Rectangle sourceRectangle, Shader shader, Texture texture)
        {
            _sourceRectangle = sourceRectangle;
            _texture = texture;
            _shader = shader;
            SetShader(shader);
        }

        public void SetShader(Shader shader)
        {
            _shader = shader;

            DeleteBuffers();

            BeginGenerateVertexArrayObject();

            GenerateBufferObjects();

            EndGenerateVertexArrayObject();

        }

        public void SetShaderSimple(Shader shader)
        {
            _shader = shader;
        }

        private void BeginGenerateVertexArrayObject()
        {
            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);
        }

        private void EndGenerateVertexArrayObject()
        {
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            int TexCoordLocation = _shader.GetAttributeLocation("aTexCoord");
            GL.EnableVertexAttribArray(TexCoordLocation);
            GL.VertexAttribPointer(TexCoordLocation, 2, VertexAttribPointerType.Float,
                false, 5 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(0);
        }

        private void GenerateBufferObjects()
        {
            float textureX1 = _sourceRectangle.X / _texture.Width;
            float textureY1 = _sourceRectangle.Y / _texture.Height;
            float textureX2 = _sourceRectangle.X2 / _texture.Width;
            float textureY2 = _sourceRectangle.Y2 / _texture.Width;

            float[] vertices =
            {
                +1f, +1f, 0f, textureX2, textureY1, //top right
                +1f, -1f, 0f, textureX2, textureY2, //bottom right
                -1f, -1f, 0f, textureX1, textureY2, //bottom left
                -1f, +1f, 0f, textureX1, textureY1 //top left
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

        private void DeleteBuffers()
        {
            if (_vertexBufferObject != -1)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
                GL.DeleteBuffer(_vertexBufferObject);
                _vertexBufferObject = -1;
            }
            if (_elementBufferObject != -1)
            {
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
                GL.DeleteBuffer(_elementBufferObject);
                _elementBufferObject = -1;
            }
            if (_vertexArrayObject != -1)
            {
                GL.BindVertexArray(_vertexArrayObject);
                GL.DeleteVertexArray(_vertexArrayObject);
                _vertexArrayObject = -1;
            }
        }
    }
}
