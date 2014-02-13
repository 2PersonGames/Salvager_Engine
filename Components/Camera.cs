using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SalvagerEngine.Components
{
    public struct Camera
    {
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
    }
}
