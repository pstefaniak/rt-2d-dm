using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace WormsDeathmatch
{
    /// <summary>
    /// Stores and loads textures on demand
    /// </summary>
    public static class ResourceManager
    {
        private static ContentManager Content;
        public static Dictionary<string, Texture2D> Textures = new Dictionary<string, Texture2D>();

        public static SpriteFont Arial;
        public static SpriteFont NameFont;
        public static Texture2D Skybox;

        public static class GUI
        {
            public static Texture2D CursorTexture;
            public static Texture2D Bar;
            public static Texture2D WeaponChargeBar;
        }

        public static AnimationData WormAnimationData;

        public static void LoadResources(ContentManager ContentArg)
        {
            Content = ContentArg;

            NameFont = Arial = ContentArg.Load<SpriteFont>("Arial");
            Skybox = ContentArg.Load<Texture2D>("Sky");

            GUI.CursorTexture = ContentArg.Load<Texture2D>("GUI/Cursor");
            GUI.Bar = ContentArg.Load<Texture2D>("GUI/Bar");
            GUI.WeaponChargeBar = ContentArg.Load<Texture2D>("GUI/WeaponChargeBar");

            // Worm spritesheet
            WormAnimationData = new AnimationData(ContentArg.Load<Texture2D>("Worm"));
            WormAnimationData.AddAnimation(15, 15, 1, 0);// Walk
            WormAnimationData.AddAnimation(15, 15, 10, 0.05f); // Move
            WormAnimationData.AddAnimation(12, 27, 2, 0); // Airbone 0 - going up, 1 - falling
            WormAnimationData.AddAnimation(15, 27, 7, 0.03f); // Jump
            WormAnimationData.AddAnimation(15, 27, 4, 0.05f); // Land
        }


        public static Texture2D GetTexture(string Name)
        {
            if (Textures.ContainsKey(Name))
            {
                return Textures[Name];
            }
            else
            {
                Texture2D NewTexture = Content.Load<Texture2D>(Name);
                if (NewTexture == null)
                    return null;
                else
                {
                    Textures.Add(Name, NewTexture);
                    return NewTexture;
                }
            }

        }
    }
}
