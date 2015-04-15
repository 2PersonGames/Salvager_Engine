using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using SalvagerEngine.Framework.Objects;
using SalvagerEngine.Framework.Components;

namespace SalvagerEngine.Framework.Objects.Graphics
{
    public class Animation : Sprite
    {
        // Animation Variables

        float mFrameAlarmMax;
        public float FrameAlarmMax
        {
            get { return mFrameAlarmMax; }
            set { mFrameAlarmMax = value; }
        }

        float mFrameAlarmCurrent;
        Rectangle mAnimationBounds;
        public Rectangle AnimationBounds
        {
            get { return mAnimationBounds; }
        }

        // Constructors

        public Animation(Level component_owner, PhysicalObject parent, string texture_name, float animation_timer)
            : base(component_owner, parent, texture_name)
        {
            // Retrieve the animation bounds
            Rectangle frame = Rectangle.Empty;
            component_owner.Game.Content.GetTexture(texture_name, out mAnimationBounds, out frame);
            Source = frame;

            // Setup the animation alarm
            mFrameAlarmMax = animation_timer;
        }

        public Animation(Level component_owner, PhysicalObject parent, string texture_name, float animation_timer, Rectangle frame)
            : base(component_owner, parent, texture_name)
        {
            mFrameAlarmMax = animation_timer;
            mAnimationBounds = frame;
        }

        // Overrides

        protected override void Tick(float delta)
        {
            // Call the base method
            base.Tick(delta);
            
            // Check the timer
            mFrameAlarmCurrent += delta;
            if (mFrameAlarmCurrent >= mFrameAlarmMax)
            {
                // Reset the alarm
                mFrameAlarmCurrent = 0.0f;

                // Store the frame
                Rectangle frame = Source;

                // Increment the x axis
                frame.X += frame.Width;
                if (frame.X >= mAnimationBounds.Right)
                {
                    // Reset the x axis
                    frame.X = mAnimationBounds.Left;

                    // Increment the y axis
                    frame.Y += frame.Height;
                    if (frame.Y >= mAnimationBounds.Bottom)
                    {
                        // Reset the y axis
                        frame.Y = mAnimationBounds.Top;
                    }
                }

                // Copy the frame back
                Source = frame;
            }
        }
    }
}
