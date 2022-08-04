using LowRezJam22.Engine.Graphics;
using OpenTK.Windowing.Common;

namespace LowRezJam22.Engine
{
    internal abstract class SceneBase
    {
        public abstract void Init();

        public abstract void Destroy();

        public abstract void Update(FrameEventArgs args);

        public abstract RenderTexture Draw(FrameEventArgs args);
    }
}
