using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SalvagerEngine.Interfaces.Objects.Graphics
{
    public delegate void RenderEvent(ICamera camera);

    public interface ICamera : IDisposable
    {
        SpriteBatch Renderer { get; }
        Viewport Viewport { get; }
        Vector2 Position { get; }

        void Begin();
        void RenderObject(RenderEvent render, BlendState blend, SamplerState sampler);
        void End();
    }
}
