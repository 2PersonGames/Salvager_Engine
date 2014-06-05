using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SalvagerEngine.Games;

namespace SalvagerEngine.Components
{
    public class Camera : IDisposable
    {
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
        }

        BlendState mBlendState;
        public BlendState BlendState
        {
            get { return mBlendState; }
        }

        SamplerState mSamplerState;
        public SamplerState SamplerState
        {
            get { return mSamplerState; }
        }

        DepthStencilState mDepthStencilState;
        public DepthStencilState DepthStencilState
        {
            get { return mDepthStencilState; }
        }

        RasterizerState mRasterizerState;
        public RasterizerState RasterizerState
        {
            get { return mRasterizerState; }
        }

        Effect mEffect;
        public Effect Effect
        {
            get { return mEffect; }
        }

        Matrix mView;
        public Matrix View
        {
            get { return mView; }
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

        /* Events */

        public void Begin()
        {
            mRenderer.Begin(mSpriteSortMode, mBlendState, mSamplerState, mDepthStencilState, mRasterizerState, mEffect, mView);
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
