using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using SalvagerEngine.Interfaces.Games;

using Microsoft.Xna.Framework.Graphics;

namespace SalvagerEngine.Framework.Components
{
    public class Component : DrawableGameComponent
    {
        /* Class Variables */

        public new ISalvagerGame Game
        {
            get { return base.Game as ISalvagerGame; }
        }

        EventHandler mOnDispose;
        public EventHandler OnDispose
        {
            get { return mOnDispose; }
            set { mOnDispose = value; }
        }

        /* Constructors */

        public Component(ISalvagerGame game)
            : base(game as Game)
        {
        }

        /* Overrides */

        protected override void Dispose(bool disposing)
        {
            /* Call the base disposing */
            base.Dispose(disposing);

            /* Call the dispose event */
            if (disposing && mOnDispose != null)
            {
                mOnDispose(this, null);
            }
        }
    }
}
