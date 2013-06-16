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
    /// Container for frame size and duration
    /// </summary>
    public class AnimationData
    {
        public AnimationData(Texture2D Sprites)
        {
            SpriteSheet = Sprites;
        }

        public List<int> FrameHeight = new List<int>();
        public List<int> FrameWidth = new List<int>();
        public List<int> FrameCount = new List<int>();
        public List<int> FrameStartY = new List<int>();
        public List<float> FrameLength = new List<float>();
        public Texture2D SpriteSheet = null;

// nie ma doca?
        public void AddAnimation(int Width, int Height, int Count, float Length)
        {
            int Index = FrameCount.Count;

            FrameHeight.Add(Height);
            FrameWidth.Add(Width);
            FrameCount.Add(Count-1);
            FrameLength.Add(Length);

            if (Index > 0)
            {
                FrameStartY.Add(FrameStartY[Index - 1] + FrameHeight[Index - 1]);
            }
            else
            {
                FrameStartY.Add(0);
            }
        }
    }
}
