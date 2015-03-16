using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SalvagerEngine.Games;
using SalvagerEngine.Objects.Graphics;

namespace SalvagerEngine.Components
{
    public class Camera : IDisposable
    {
        /* Typedefs and Constants */

        public delegate void RenderEvent (Camera camera);

        /* Render Variables */

        SpriteBatch mRenderer;
        public SpriteBatch Renderer
        {
            get { return mRenderer; }
        }

        SpriteSortMode mSpriteSortMode;
        public SpriteSortMode SpriteSortMode
        {
            get { return mSpriteSortMode; }
            set { mSpriteSortMode = value; }
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

        DepthStencilState mDepthStencilState;
        public DepthStencilState DepthStencilState
        {
            get { return mDepthStencilState; }
            set { mDepthStencilState = value; }
        }

        RasterizerState mRasterizerState;
        public RasterizerState RasterizerState
        {
            get { return mRasterizerState; }
            set { mRasterizerState = value; }
        }

        Effect mEffect;
        public Effect Effect
        {
            get { return mEffect; }
            set { mEffect = value; }
        }

        /* Camera Variables */

        public float MaxZoom { get; set; }
        public float MinZoom { get; set; }

        public Vector2 Position { get; set; }

        float mZoom;
        public float Zoom
        {
            get { return mZoom; }
            set { mZoom = MathHelper.Clamp(value, MinZoom, MaxZoom); }
        }

        float mRotation;
        public float Rotation
        {
            get { return mRotation; }
            set { mRotation = MathHelper.WrapAngle(value); }
        }

        /* Constructors */

        public Camera(SalvagerGame game)
        {
            mRenderer = new SpriteBatch(game.GraphicsDevice);
            mSpriteSortMode = Microsoft.Xna.Framework.Graphics.SpriteSortMode.Immediate;
            mBlendState = BlendState.AlphaBlend;
            mSamplerState = SamplerState.AnisotropicClamp;
            mDepthStencilState = DepthStencilState.Default;
            mRasterizerState = RasterizerState.CullNone;
            mEffect = null;

            MaxZoom = float.MaxValue;
            MinZoom = -float.MaxValue;
            Position = Vector2.Zero;
            mZoom = 1.0f;
            mRotation = 0.0f;
        }

        /* Accessors */

        public GraphicsDevice GraphicsDevice
        {
            get { return mRenderer.GraphicsDevice; }
        }

        public Viewport Viewport
        {
            get { return GraphicsDevice.Viewport; }
        }

        public Matrix CalculateViewMatrix()
        {
            return CalculteCamera(Position, new Vector2(mZoom), mRotation, new Vector2(Viewport.Width, Viewport.Height) * 0.5f);
        }

        /* Events */

        public void Begin()
        {
            mRenderer.Begin(mSpriteSortMode, mBlendState, mSamplerState, mDepthStencilState, mRasterizerState, mEffect, CalculateViewMatrix());
        }

        public void RenderObject(RenderEvent render, BlendState blend, SamplerState sampler)
        {
            /* Apply the render settings */
            GraphicsDevice.BlendState = blend;
            GraphicsDevice.SamplerStates[0] = sampler;

            try
            {
                /* Render the object */
                render(this);
            }
            finally
            {
                /* Reset the render settings */
                GraphicsDevice.BlendState = mBlendState;
                GraphicsDevice.SamplerStates[0] = mSamplerState;
            }
        }

        public void End()
        {
            mRenderer.End();
        }

        /* Utilities */

        public static Matrix CalculteCamera(Vector2 position, Vector2 scale, float rotation, Vector2 centre)
        {
            return Matrix.CreateTranslation(-new Vector3(position, 0.0f)) *
                    Matrix.CreateRotationZ(rotation) *
                    Matrix.CreateScale(new Vector3(scale, 1.0f)) *
                    Matrix.CreateTranslation(new Vector3(centre, 0.0f));
        }

        /* Interfaces */

        public void Dispose()
        {
            /* Dispose of the renderer */
            if (mRenderer != null)
            {
                mRenderer.Dispose(); 
                mRenderer = null;
            }
        }
    }
}
