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
        Vector2 mTransformedOffset;
        Vector2 mOriginalTransformedOffset;
        public Vector2 TransformedOffset
        {
            get { return mTransformedOffset; }
            set { mOriginalTransformedOffset = value; }
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

        BlendState mBlendState;
        public BlendState BlendState
        {
            get { return mBlendState; }
            set { mBlendState = value; }
        }
        SamplerState mSamplerState;
        public SamplerState SamplerState
        {
            get { return mSamplerState; }
            set { mSamplerState = value; }
        }

        /* Constructors */

        public GraphicsObject(Level component_owner, PhysicalObject parent)
            : base(component_owner, 0.0f)
        {
            mParent = parent;
            mColour = Color.White;
            mRenderColour = Color.White;
            mScale = Vector2.One;
            mBlendState = BlendState.AlphaBlend;
            mSamplerState = SamplerState.AnisotropicClamp;
        }

        /* Overrides */

        protected override void Tick(float delta)
        {
            /* Calculate the offset */
            mTransformedOffset = Vector2.Transform(mOriginalTransformedOffset, Matrix.CreateRotationZ(Parent.Rotation));
        }

        protected sealed override void Render(Camera camera)
        {
            camera.RenderObject(RenderObject, mBlendState, mSamplerState);
        }

        public override float GetDepth()
        {
            return mDepth;
        }

        /* Abstracts */

        protected abstract void RenderObject(Camera camera);

        /* Accessors */

        public abstract Point GetBounds();

        public Vector2 GetActualPosition()
        {
            return Parent.Position + mOffset + mTransformedOffset;
        }
    }
}
