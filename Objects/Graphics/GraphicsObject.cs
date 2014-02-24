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

        PhysicalObject mParent;
        public PhysicalObject Parent
        {
            get { return mParent; }
            set { mParent = value; }
        }

        Vector2 mOffset;
        public Vector2 Offset
        {
            get { return mOffset; }
            set { mOffset = value; }
        }
        Vector2 mCentre;
        public Vector2 Centre
        {
            get { return mCentre; }
            protected set { mCentre = value; }
        }
        Vector2 mScale;
        public Vector2 Scale
        {
            get { return mScale; }
            set { mScale = value; }
        }
        SpriteEffects mSpriteEffects;
        public SpriteEffects SpriteEffects
        {
            get { return mSpriteEffects; }
            set { mSpriteEffects = value; }
        }
        float mRotation;
        public float Rotation
        {
            get { return mRotation; }
            set { mRotation = value; }
        }
        Color mColour;
        public Color Colour
        {
            get { return mColour; }
            set 
            { 
                mColour = value;
                mRenderColour = Tools.DrawManager.PremultiplyAlpha(mColour, mColour.A / 255.0f); 
            }
        }
        Color mRenderColour;
        public Color RenderColour
        {
            get { return mRenderColour; }
        }
        float mDepth;
        public float Depth
        {
            get { return mDepth; }
            set { mDepth = value; }
        }

        /* Constructors */

        public GraphicsObject(Level component_owner, PhysicalObject parent)
            : base(component_owner)
        {
            mParent = parent;
        }

        /* Overrides */

        protected override void Initialise(out float tick_max)
        {
            tick_max = 1.0f;
        }
    }
}
