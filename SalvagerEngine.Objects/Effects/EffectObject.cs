using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SalvagerEngine.Objects;
using SalvagerEngine.Interfaces.Objects;
using SalvagerEngine.Interfaces.Objects.Effects;

namespace SalvagerEngine.Objects.Effects
{
    public abstract class EffectObject : GameObject, IEffectObject
    {
        /* Class Variables */

        IGameObject mParent;
        public IGameObject Parent
        {
            get { return mParent; }
        }

        /* Constructors */

        public EffectObject(IGameObject parent, float tick_max)
            : base(parent.ComponentOwner, tick_max)
        {
            mParent = parent;
        }
    }
}
