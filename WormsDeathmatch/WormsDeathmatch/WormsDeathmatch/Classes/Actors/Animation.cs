// ShadowDancer

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WormsDeathmatch
{
    /// <summary>
    /// Actor for animated objects
    /// </summary>
    public class Animation
    {
        int Frame;
        int Index;
        Vector2 Origin;

        float AnimationTime;
        AnimationData Data;

        public Animation(AnimationData SpriteSheet)
        {
            Data = SpriteSheet;
            SetAnimation(0);
        }

        public void AnimationReset()
        {
            Frame = 0;
            AnimationTime = 0;
        }

        public void SetAnimation(int AnimationIndex)
        {
            if (AnimationIndex != Index && AnimationIndex < Data.FrameCount.Count)
            {
                Index = AnimationIndex;
                Origin = new Vector2(Data.FrameWidth[Index] / 2, Data.FrameHeight[Index] / 2);
                AnimationReset();
            }
        }

        public void Update(float DeltaTime)
        {
            if (Data.FrameLength[Index] != 0)
            {
                AnimationTime += DeltaTime;
                if(AnimationTime > Data.FrameLength[Index])
                {
                    NextFrame();
                }
            }
        }

        public void NextFrame()
        {
            AnimationTime = 0;
            Frame++;
            if (Frame > Data.FrameCount[Index])
            {
                Frame = 0;
            }
        }

        public void SetFrame(int Index)
        {
            AnimationTime = 0;
            Frame = Index;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 Position, SpriteEffects Se, float Rotation)
        {
            spriteBatch.Draw(Data.SpriteSheet, Position, new Rectangle(Data.FrameWidth[Index] * Frame, Data.FrameStartY[Index], Data.FrameWidth[Index], Data.FrameHeight[Index]), Color.White, Rotation, Origin, 1, Se, 0);
        }

    }
}
