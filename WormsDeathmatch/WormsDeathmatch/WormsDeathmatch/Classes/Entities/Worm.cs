using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WormsDeathmatch
{
    /// <summary>
    /// Control player charcters
    /// </summary>
    public class Worm : Entity
    {
        public enum WormAnimations
        {
            Idle = 0,
            Walk = 1,
            Airbone = 2,
            Jump = 3,
            Land = 4
        }

        public float WeaponAngle;

        public Animation Animation;

        bool Jumped = false;
        public bool FacingLeft = true;
        
        public int LifeMaximum;
        public float LifeCurrent;


        // Controls
        public bool ControlsIsMovingLeft = false;
        public bool ControlsIsMovingRight = false;
        public bool ControlsJump = false;
        public bool ControlsShoot = false;
        public bool ControlsMovement = false;

        public float RecoilBuffer = 0;

        // Temporary variables
        public float AccuracyPenalty = 0;
        float JumpDelay = 0;
        float FallDistance = 0;
        Vector2 OldVelocity = Vector2.Zero;

        public List<Weapon> CarriedWeapons = new List<Weapon>();
        public Weapon CurrentWeapon;
        public int WeaponIndex;
        public MovementDeviceData MovementDevice;

        public Worm()
        {
            MovementDevice = new MovementDeviceData(this, "ASD");
        }

        public void Draw(SpriteBatch spriteBatch, float DeltaTime)
        {
            SpriteEffects SF = SpriteEffects.None;
            if(!FacingLeft)SF = SpriteEffects.FlipHorizontally;

            Animation.Update(DeltaTime);
            Animation.Draw(spriteBatch, Position - Level.CameraPosition, SF, 0);

            if(CurrentWeapon != null)
                CurrentWeapon.Draw(spriteBatch);
        }

        public void Respawn(Vector2 pPosition)
        {
            CollisionX = true;
            CollisionY = true;

            foreach (var Weapon in CarriedWeapons)
            {
                Weapon.Reloading = false;
                Weapon.TimeCharging = 0;
                Weapon.Charging = false;
                Weapon.TimeToNextShoot = 0;
                Weapon.TimeToReloadEnd = 0;
                Weapon.Magazine = Weapon.Data.MagazineSize;
            }

            IsOnGround = false;
            LifeCurrent = Level.cLifeStart;
            Position = pPosition;
            Bounds.X = (int)(Position.X - GroundCollisionSize.X / 2);
            Bounds.Y = (int)(Position.Y - GroundCollisionSize.Y / 2);
            Velocity = Vector2.Zero;
            Kill = false;
        }

        public void DoDamage(float Amount)
        {
            Amount = (float)Math.Ceiling(Amount);
            TextTag.CreateOnHitTextTag((int)Amount, Level, Position);
            LifeCurrent -= Amount;

            if (LifeCurrent < 0)
            {
                Respawn(new Vector2(new Random().Next(300, Level.Ground.Width - 300), 50));
            }
        }

        public new void Update(float DeltaTime)
        {
            float VelXChunk = Level.cWormSpeed;

            // Keyboard input
            if (ControlsIsMovingLeft && Velocity.X > -VelXChunk)
            {
                if(Velocity.X <= 0)
                    Velocity.X -= VelXChunk;
                else
                    Velocity.X -= VelXChunk * DeltaTime * 10;
            }
            else if (ControlsIsMovingRight && Velocity.X < VelXChunk)
            {
                if(Velocity.X >= 0)
                    Velocity.X += VelXChunk;
                else
                    Velocity.X += VelXChunk * DeltaTime * 10;
            }


            // Increased acurracy if not moving
            if (Velocity.X == 0 && Velocity.Y == 0)
            {
                AccuracyPenalty -= DeltaTime / 3;
            }

            if (IsOnGround)
            {
                #region Damage for falling
                if (CollisionX || CollisionY)
                {
                    float FallVelocity = OldVelocity.Y;
                    if (FallVelocity > 400)
                    {
                        DoDamage((FallVelocity - 250) / 3.0f);
                        AccuracyPenalty += FallVelocity / 300;
                        JumpDelay += 1;
                    }
                    else if (FallVelocity > 300)
                    {
                        DoDamage((FallVelocity - 250) / 4.0f);
                        AccuracyPenalty += FallVelocity / 400;
                        JumpDelay += 0.25f;
                    }
                    else if (FallVelocity > 250)
                    {
                        DoDamage((FallVelocity - 250) / 5.0f);
                        AccuracyPenalty += FallVelocity / 500;
                    }
                }
                #endregion

                #region Walk/Idle animation
                if (Velocity.X == 0)
                {
                    Animation.SetAnimation((int)WormAnimations.Idle);
                }
                else
                {
                    Animation.SetAnimation((int)WormAnimations.Walk);
                }
                #endregion

                Jumped = false;
                FallDistance = 0;
            }
            else // falling or flying
            {
                // For handling long falls
                FallDistance += Velocity.Y * DeltaTime;

                // Falling or jumping animation
                if (FallDistance > 10 || Jumped)
                {
                    AccuracyPenalty += DeltaTime;
                    Animation.SetAnimation((int)WormAnimations.Airbone);

                    if (Velocity.Y < 0)
                        Animation.SetFrame(0);
                    else
                        Animation.SetFrame(1);
                }
            }

            if(JumpDelay > 0)
            {
                JumpDelay -= DeltaTime;
            }
            else
            {
                // Jump
                if (ControlsJump && !Jumped && IsOnGround) 
                {
                    ControlsJump = false;
                    Jumped = true;
                    Velocity.Y += Level.cWormJumpVelocityBonus.Y;
                    AccuracyPenalty += 0.75f;

                    float VelocityBonus = Velocity.X/Level.cWormSpeed * Level.cWormJumpVelocityBonus.X;
                    if (VelocityBonus > Level.cWormJumpVelocityBonus.X) VelocityBonus = Level.cWormJumpVelocityBonus.X;

                    if(FacingLeft)
                        Velocity.X -= VelocityBonus;
                    else
                        Velocity.X += VelocityBonus;
                    JumpDelay = 1;
                }
            }

            if (CurrentWeapon != null) CurrentWeapon.Update(DeltaTime);
            if (ControlsShoot)
            {
                if (CurrentWeapon != null)
                {
                    CurrentWeapon.LocalShoot();
                }
            }

            // Update jetpacks etc.
            MovementDevice.Active = ControlsMovement;
            MovementDevice.Update(DeltaTime);


            // Facing
            if(FacingLeft && Velocity.X > 0) FacingLeft = false;
            if(!FacingLeft && Velocity.X < 0) FacingLeft = true;

            // Update position
            OldVelocity = Velocity;
            base.Update(DeltaTime);

            if (IsOnGround)
            {
                if (VelXChunk < Math.Abs(Velocity.X))
                {
                    Velocity.X -= VelXChunk * Level.cWormGroundFrictionFactor * Math.Sign(Velocity.X) * DeltaTime;
                }
                else Velocity.X = 0;
            }


            if (Kill)
            {
                DoDamage(9999999);
            }
        }

    }
}
