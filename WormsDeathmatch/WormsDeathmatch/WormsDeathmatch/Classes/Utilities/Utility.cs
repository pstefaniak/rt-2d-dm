using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WormsDeathmatch
{
    public class Utility
    {
        public static bool CircleIntersectsRectangle(Vector2 Position, float Radius, Worm Worm)
        {
            Rectangle rect = new Rectangle((int)(Worm.Position.X - Worm.GroundCollisionSize.X / 2), (int)(Worm.Position.Y - Worm.GroundCollisionSize.Y / 2), (int)Worm.GroundCollisionSize.X, (int)Worm.GroundCollisionSize.Y);

            int circleDistanceX = (int)Math.Abs(Position.X - rect.X);
            int circleDistanceY = (int)Math.Abs(Position.Y - rect.Y);

            if (circleDistanceX > (rect.Width / 2 + Radius)) { return false; }
            if (circleDistanceY > (rect.Height / 2 + Radius)) { return false; }

            if (circleDistanceX <= (rect.Width / 2)) { return true; }
            if (circleDistanceY <= (rect.Height / 2)) { return true; }

            int cornerDistance_sq = (circleDistanceX - rect.Width / 2) ^ 2 +
                                 (circleDistanceY - rect.Height / 2) ^ 2;

            return (cornerDistance_sq <= (Radius * Radius));
        }

        public static float Distance(Vector2 Vector1, Vector2 Vector2)
        {
            return (Vector1 - Vector2).Length();
        }
    }
}
