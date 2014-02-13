using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SalvagerEngine.Games;
using SalvagerEngine.Objects;

namespace SalvagerEngine.Components
{
    public class Component : DrawableGameComponent
    {
        /* Class Variables */

        public new SalvagerGame Game
        {
            get { return base.Game as SalvagerGame; }
        }

        EventHandler mOnDispose;
        public EventHandler OnDispose
        {
            get { return mOnDispose; }
            set { mOnDispose = value; }
        }

        /* Constructors */

        public Component(SalvagerGame game)
            : base(game)
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
