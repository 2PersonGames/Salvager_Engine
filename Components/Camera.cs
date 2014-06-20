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

        /* Class Variables */

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

        Matrix mView;
        public Matrix View
        {
            get { return mView; }
        }

        public GraphicsDevice GraphicsDevice
        {
            get { return mRenderer.GraphicsDevice; }
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
            mView = Matrix.Identity;
        }

        /* Accessors */

        public Vector2 GetPosition()
        {
            return new Vector2(-mView.Translation.X, -mView.Translation.Y) +
                (new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height) * 0.5f);
        }

        /* Mutators */

        public void SetPosition(Vector2 position)
        {
            /* Modify the position to take the viewport into account and centre the position */

            /* Set the translation matrix */
            mView = Matrix.CreateTranslation(new Vector3(-position, 0.0f));
        }

        /* Events */

        public void Begin()
        {
            mRenderer.Begin(mSpriteSortMode, mBlendState, mSamplerState, mDepthStencilState, mRasterizerState, mEffect, mView);
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
