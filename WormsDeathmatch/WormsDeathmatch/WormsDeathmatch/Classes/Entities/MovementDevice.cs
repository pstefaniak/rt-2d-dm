using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WormsDeathmatch
{
    /// <summary>
    /// Under right mouse button, used to move worm (jetpacks, etc.)
    /// </summary>
    public class MovementDeviceData
    {
        public float Fuel;
        public float FuelMaximum;
        public float UseInterval;
        public Vector2 Force;
        public bool Active; // Player is currently using it (ex. holding right mouse button)

        Worm Carrier;
        string Name;

        public MovementDeviceData(Worm Owner, string DeviceName)
        {
            Name = DeviceName;
            Carrier = Owner;
            Fuel = FuelMaximum = 2;
            Force = new Vector2(0, -300);
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {

        }


        public void Update(float DeltaTime)
        {
            if (Fuel > 0 && Active)
            {
                Carrier.Velocity += Force * DeltaTime;
                Fuel -= DeltaTime * 2;
            }
            else
            {
                Fuel += DeltaTime;
                if (Fuel > FuelMaximum)
                    Fuel = FuelMaximum;
            }

        }
    }
}
