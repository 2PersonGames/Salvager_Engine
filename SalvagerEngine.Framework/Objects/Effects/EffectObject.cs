using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SalvagerEngine.Framework.Games;
using SalvagerEngine.Framework.Objects;
using SalvagerEngine.Framework.Components;

namespace SalvagerEngine.Framework.Objects.Effects
{
    public abstract class EffectObject : GameObject
    {
        /* Class Variables */

        GameObject mParent;
        public GameObject Parent
        {
            get { return mParent; }
        }

        /* Constructors */

        public EffectObject(GameObject parent, float tick_max)
            : base(parent.ComponentOwner, tick_max)
        {
            mParent = parent;
        }
    }
}
