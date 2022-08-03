using LowRezJam22.Engine.Graphics;

namespace LowRezJam22.Engine
{
    internal abstract class SceneBase
    {
        public abstract void Init();

        public abstract void Destroy();

        public abstract void Update();

        public abstract RenderTexture Draw();
    }
}
