using LowRezJam22.Helpers;
using OpenTK.Graphics.OpenGL;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace LowRezJam22.Engine.Graphics
{
    internal class RenderTexture : Texture
    {
        private readonly int _frameBuffer;

        public RenderTexture(int width, int height) : base()
        {
            _width = width;
            _height = height;

            _frameBuffer = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, _frameBuffer);
            _handle = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, _handle);
            byte[] tmpPixels = Array.Empty<byte>();
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, _width, _height,
                0, PixelFormat.Rgba, PixelType.UnsignedByte, tmpPixels);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapS);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapT);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter);


            int depthRenderBuffer = GL.GenRenderbuffer();
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, depthRenderBuffer);
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.DepthComponent, _width, _height);
            GL.FramebufferRenderbuffer(FramebufferTarget.ReadFramebuffer, FramebufferAttachment.DepthAttachment,
                RenderbufferTarget.Renderbuffer, depthRenderBuffer);
            GL.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, _handle, 0);
            DrawBuffersEnum[] DrawBuffers = { DrawBuffersEnum.ColorAttachment0 };
            GL.DrawBuffers(1, DrawBuffers);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        public static void Begin(RenderTexture texture)
        {
            texture.Begin();
        }

        public void Begin()
        {
            if (Game.Instance is null)
            {
                Logger.Log(Logger.Levels.Fatal, "Game is not initialized!");
                return;
            }

            Game.Instance.SetViewport(new Helpers.Rectangle(0, 0, _width, _height));
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, _frameBuffer);
        }

        public static void End()
        {
            if (Game.Instance is null)
            {
                Logger.Log(Logger.Levels.Fatal, "Game is not initialized!");
                return;
            }

            Game.Instance.SetViewport(new Helpers.Rectangle(0, 0, 512, 512));
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }
    }
}
