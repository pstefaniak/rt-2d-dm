using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WormsDeathmatch
{
    public class TextTag
    {
        string Text;

        SpriteFont Font;
        public Vector2 Position;
        Vector2 Origin;
        Color Color;
        Level Level;
        public float Duration;

        public static void CreateOnHitTextTag(int DamageAmount, Level Level, Vector2 Position)
        {
            Position.Y -= 15;
            Level.TextTags.Add(new TextTag(Level, DamageAmount.ToString(), ResourceManager.Arial, Position, Color.Maroon, 1));
        }

        public TextTag(Level Owner, string TagText, SpriteFont TagFont, Vector2 TagPosition, Color TagColor, float TagDuration)
        {
            Level = Owner;
            Text = TagText;
            Font = TagFont;
            Position = TagPosition;
            Color = TagColor;
            Duration = TagDuration;

            Origin = Font.MeasureString(TagText)/2;
        }

        public void Draw(SpriteBatch spriteBatch, float DeltaTime)
        {
            if (Duration <= -1)
            {
                Level.TextTagsToRemove.Add(this);
            }
            else
            {
                if (Duration <= 0)// Shrink string
                {
                    spriteBatch.DrawString(Font, Text, Position - Level.CameraPosition, Color, 0, Origin, 1 + Duration, SpriteEffects.None, 0);
                }
                else
                {
                    spriteBatch.DrawString(Font, Text, Position - Level.CameraPosition, Color, 0, Origin, 1, SpriteEffects.None, 0);
                }

                Duration -= DeltaTime;
            }
        }
    }
}
