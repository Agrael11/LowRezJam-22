using LowRezJam22.Helpers;
using OpenTK.Graphics.OpenGL;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace LowRezJam22.Engine.Graphics
{
    internal class Texture : IDisposable
    {
        internal int _handle;
        internal int _width;
        public int Width { get { return _width; } }
        internal int _height;
        public int Height { get { return _height; } }
        internal byte[] _pixels = Array.Empty<byte>();
        public TextureWrapMode TextureWrapS { get; set; } = TextureWrapMode.Repeat;
        public TextureWrapMode TextureWrapT { get; set; } = TextureWrapMode.Repeat;
        public TextureMinFilter TextureMinFilter { get; set; } = TextureMinFilter.Nearest;
        public TextureMagFilter TextureMagFilter { get; set; } = TextureMagFilter.Nearest;

        public Texture(string path)
        {
            Image<Rgba32> image;
            try
            {
                image = Image.Load<Rgba32>(path);
            }
            catch (Exception ex)
            {
                Logger.Log(Logger.Levels.Warning, ex.Message);
                return;
            }

            _width = image.Width;
            _height = image.Height;
            _pixels = new byte[4 * image.Width * image.Height];
            image.CopyPixelDataTo(_pixels);

            _handle = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, _handle);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS,
                (int)TextureWrapS);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT,
                (int)TextureWrapT);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                (int)TextureMinFilter);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                (int)TextureMagFilter);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, _width, _height, 0,
                PixelFormat.Rgba, PixelType.UnsignedByte, _pixels);

            image.Dispose();
        }

        internal Texture()
        {
        }

        public void UseTexture(TextureUnit unit = TextureUnit.Texture0)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, _handle);
        }

        public virtual void Dispose()
        {
            GL.DeleteTexture(_handle);
        }
    }
}
