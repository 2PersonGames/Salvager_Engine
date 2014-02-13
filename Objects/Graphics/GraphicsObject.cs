using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SalvagerEngine.Games;
using SalvagerEngine.Objects;
using SalvagerEngine.Components;

namespace SalvagerEngine.Objects.Graphics
{
    public abstract class GraphicsObject : GameObject
    {
        /* Class Variables */



        /* Constructors */

        public GraphicsObject(Level component_owner)
            : base(component_owner)
        {
        }
    }
}
