using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WormsDeathmatch
{
    public class Entity
    {
        public Vector2 Position;
        public Vector2 Velocity;

        private float _BoundigSphereRadiusSquared;
        public float BoundigSphereRadiusSquared 
        { 
            get 
            { 
                return _BoundigSphereRadiusSquared; 
            } 
        }
        private Vector2 _GroundCollisionSize;
        public Vector2 GroundCollisionSize 
        { 
            get 
            { 
                return _GroundCollisionSize; 
            } 
            set 
            { 
                _GroundCollisionSize = value; 
            } 
        }

        private Vector2 _ProjectileCollisionSize;
        public Vector2 ProjectileCollisionSize 
        { 
            get 
            { 
                return _ProjectileCollisionSize; 
            } 
            set 
            { 
                _ProjectileCollisionSize = value; 
                _BoundigSphereRadiusSquared = Math.Max(_ProjectileCollisionSize.X * _ProjectileCollisionSize.X, _ProjectileCollisionSize.Y * _ProjectileCollisionSize.Y); 
                Bounds.Width = (int)_ProjectileCollisionSize.X; 
                Bounds.Height = (int)_ProjectileCollisionSize.Y; 
            } 
        }

        public Rectangle Bounds = new Rectangle();

        /// <summary>
        /// Gravity * mass
        /// </summary>
        public Vector2 LocalGravity;
        public float Mass;

        public bool CollideWithWormsCheck = false;
        /// <summary>
        /// Entity can climb up small hills, used for with bullets to prevent rebounds
        /// </summary>
        public bool HardCollisionWithTerrain = false;
        List<Worm> CollisionCandidates = new List<Worm>();
        public List<Worm> RecentlyCollided = new List<Worm>();

        public float Rotation;
        public Level Level;
        public bool IsOnGround = false;
        /// <summary>
        /// True if entity collided in this frame with terrain
        /// </summary>
        protected bool CollisionX = false, CollisionY = false;
        public void Initialize(Level Owner, float Mass)
        {
            LocalGravity = Owner.cGravity * Mass;
            Level = Owner;
        }
        /// <summary>
        /// If true entity will be removed from game after update
        /// </summary>
        public bool Kill = false;
        public void Update(float DeltaTime)
        {
            Velocity += LocalGravity * DeltaTime;

            float AccumulatorX = Math.Abs(Velocity.X * DeltaTime), AccumulatorY = Math.Abs(Velocity.Y * DeltaTime);
            bool Unclimbable = false;
           
            Color[] PixelData = Level.GroundData;

            CollisionCandidates.Clear();
            RecentlyCollided.Clear();

            int Max = (int)Math.Max(AccumulatorX, AccumulatorY) + 1;
            Max *= Max;

            if (CollideWithWormsCheck)
            {
                foreach (Worm CurrentWorm in Level.Worms)
                {
                    // TODO - check if worm is in bouding sphere radius
                    CollisionCandidates.Add(CurrentWorm);
                    /*
                     if ((Position.X - CurrentWorm.Position.X) * (Position.X - CurrentWorm.Position.X) +
                        (Position.Y - CurrentWorm.Position.Y) * (Position.Y - CurrentWorm.Position.Y) < BoundigSphereRadiusSquared + CurrentWorm.BoundigSphereRadiusSquared + Max)
                    {

                    }
                    */
                }
            }
            
            while (AccumulatorX > 0 || AccumulatorY > 0)
            {
                #region Kill on touching map edge
                if (Position.X - GroundCollisionSize.X / 2 -1 < 0) 
                { 
                    Kill = true; 
                    AccumulatorX = 0; 
                }
                if (Position.Y - GroundCollisionSize.Y / 2 -1< 0) 
                { 
                    Kill = true;  
                    AccumulatorY = 0; 
                }

                if (Position.X + GroundCollisionSize.X / 2 +1 > Level.Ground.Width) 
                { 
                    Kill = true;  
                    AccumulatorX = 0; 
                }
                if (Position.Y + GroundCollisionSize.Y / 2 + 1 > Level.Ground.Height) 
                {
                    Kill = true; 
                    AccumulatorY = 0; 
                }
                #endregion

                if (AccumulatorX > 0)
                {
                    
                    // Calc lower bound pixel
                    int Sign = Math.Sign(Velocity.X);
                    int Bound = (int)(Position.X + Sign * (GroundCollisionSize.X / 2));
                    // Calc target pixel line to collision
                    Bound += Sign;

                    CollisionX = false;
                    Unclimbable = HardCollisionWithTerrain;
                    int StartingPos = (Level.Ground.Width * (int)(Position.Y - GroundCollisionSize.Y / 2) + Bound);
                    if (StartingPos < 0) StartingPos = 0;
                    for (int i = 0; i < (int)(GroundCollisionSize.Y); i += 1)
                    {
                        if (Level.GroundData[StartingPos + Level.Ground.Width * i].A != 0)
                        {
                            CollisionX = true;
                        }

                        if(CollisionX)
                        {
                            // Check if we can climb up
                            if (i >= 0.9f * GroundCollisionSize.Y && !Unclimbable && IsOnGround)
                            {
                                // Check if we can climb up (terrain is no tall, so we can just walk on it)
                                // Calculate new position
                                Vector2 NewPos = new Vector2(Position.X + Sign, Position.Y - (GroundCollisionSize.Y - i));
                                // Check if it'll collide with terrain
                                int StartingPosX = (int)(NewPos.X - (GroundCollisionSize.X / 2));
                                int StartingPosY = (int)(NewPos.Y - (GroundCollisionSize.Y / 2));
                                bool Collision = false;
                                for (int x = 0; x < (int)GroundCollisionSize.X && !Collision; x++)
                                {
                                    for(int y = 0; y < (int)GroundCollisionSize.Y; y++)
                                    {
                                        if(PixelData[Level.Ground.Width * (StartingPosY + y) + (StartingPosX + x)].A != 0 )
                                        {
                                            Collision = true;
                                            break;
                                        }
                                    }
                                }

                                if (Collision == false)
                                {
                                    Position.Y = NewPos.Y;
                                    Unclimbable = HardCollisionWithTerrain;
                                    CollisionX = false;
                                }
                            }
                            else
                            {
                                Unclimbable = true; // We have collision on top of the worm or something
                            }
                        }
                    }

                    if (CollisionX)
                    {
                        AccumulatorX = 0;
                        Velocity.X = 0;
                        if (HardCollisionWithTerrain)
                        {
                            AccumulatorY = 0;
                            Velocity.Y = 0;
                        }
                    }
                    else
                    {
                        if (AccumulatorX > 1)
                        {
                            AccumulatorX--;
                            Position.X += Sign;
                        }
                        else
                        {
                            Position.X += Sign * AccumulatorX;
                            AccumulatorX = 0;
                        }
                    }
                }

                if (AccumulatorY > 0)
                {
                    // Calc lower bound pixel
                    int Sign = Math.Sign(Velocity.Y);
                    int Bound = (int)(Position.Y + Sign * (GroundCollisionSize.Y / 2));
                    // Calc target pixel line to collision
                    Bound += Sign;
                    
                    CollisionY = false;
                    int StartingPos = (int)(Level.Ground.Width * Bound + Position.X - GroundCollisionSize.X / 2);
                    if (StartingPos < 0) StartingPos = 0;
                    for (int i = StartingPos; i < (int)(StartingPos + GroundCollisionSize.X); i++)
                    {
                        if (Level.GroundData[i].A != 0)
                        {
                            CollisionY = true;
                        }
                    }

                    if (CollisionY)
                    {
                        if(Velocity.Y > 0) IsOnGround = true;
                        AccumulatorY = 0;
                        Velocity.Y = 0;

                        if (HardCollisionWithTerrain)
                        {
                            AccumulatorX = 0;
                            Velocity.X = 0;
                        }
                        
                    }
                    else
                    {
                        IsOnGround = false;
                        if (AccumulatorY > 1)
                        {
                            AccumulatorY--;
                            Position.Y += Sign;
                        }
                        else
                        {
                            Position.Y += AccumulatorY * Sign;
                            AccumulatorY = 0;
                        }
                    }
                }

                // Update bounds
                Bounds.X = (int)(Position.X - GroundCollisionSize.X/2);
                Bounds.Y = (int)(Position.Y - GroundCollisionSize.Y/2);

                // Calc collision with other worms
                foreach (Worm CollisionCandidate in CollisionCandidates)
                {
                    if (CollisionCandidate.Bounds.Intersects(Bounds))
                    {
                        if (!RecentlyCollided.Contains(CollisionCandidate))
                        {
                            RecentlyCollided.Add(CollisionCandidate);
                        }
                    }
                }
            }
        }
    }
}
