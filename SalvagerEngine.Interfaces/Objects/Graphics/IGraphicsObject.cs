using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using SalvagerEngine.Interfaces.Objects.Effects;

namespace SalvagerEngine.Interfaces.Objects.Graphics
{
    public interface IGraphicsObject : IEffectObject
    {
        Point GetBounds();
        Vector2 GetActualPosition();
    }
}
