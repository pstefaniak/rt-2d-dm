using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WormsDeathmatch
{
    public class Projectile : Entity
    {
        public Texture2D Texture;
        public Worm SourceWorm;
        public WeaponData SourceWeapon;
        public List<Worm> RecentlyDamagedTargets = new List<Worm>();
        public List<Worm> RecentlyDamagedTargetsNew = new List<Worm>();

        public float LifeTime;
        public float Damage;
        public float ExplosionRadius;

        public Projectile(Weapon Source)
        {
            HardCollisionWithTerrain = true;
            CollideWithWormsCheck = true;
            SourceWeapon = Source.Data;
            SourceWorm = Source.Carrier;

            RecentlyDamagedTargets.Add(Source.Carrier);
            RecentlyCollided.Add(Source.Carrier);
        }

        public void Explode(bool Direct, Worm DirectTarget)
        {
            if (Direct)
            {
                DirectTarget.DoDamage(Damage);
                if (Velocity != Vector2.Zero)
                {
                    Velocity.Normalize();
                    Velocity *= SourceWeapon.ProjectileForce;
                    DirectTarget.Velocity += Velocity;
                }
            }

            foreach (var PlayerWorm in Level.Worms)
            {
                if (PlayerWorm == DirectTarget) continue; // Don't damage target again

                if ((Position - PlayerWorm.Position).LengthSquared() <= ExplosionRadius * ExplosionRadius + PlayerWorm.BoundigSphereRadiusSquared)
                {
                    Vector2 ExplosionVelocity = Position - PlayerWorm.Position;
                    float DistanceModifier = 1 - (ExplosionVelocity.Length() - Math.Max(PlayerWorm.ProjectileCollisionSize.X, PlayerWorm.ProjectileCollisionSize.Y)) / ExplosionRadius;
                    
                    if(DistanceModifier < 0) DistanceModifier = 0;
                    if (DistanceModifier > 1) DistanceModifier = 1;

                    if (ExplosionVelocity != Vector2.Zero)
                    {
                        ExplosionVelocity.Normalize();
                        ExplosionVelocity *= SourceWeapon.ProjectileForce * DistanceModifier;



                        PlayerWorm.Velocity -= ExplosionVelocity;
                    }
                    if (!(!SourceWeapon.SelfDamage && PlayerWorm == SourceWorm))
                        PlayerWorm.DoDamage(Damage * DistanceModifier);
                }
            }

            Level.Explosion(Position, (int)(ExplosionRadius * Level.cExplosionGroundDestructionPercent));
            Level.ProjectilesToRemove.Add(this);
        }

        public new void Update(float DeltaTime)
        {

            base.Update(DeltaTime);
            
            // Ground collision
            if (CollisionX || CollisionY)
            {
                Explode(false, null);
            }

            // Expire
            if (LifeTime > -10)
            {
                LifeTime -= DeltaTime;
                if (LifeTime <= 0) Explode(false, null);
            }

            // Collision with players
            foreach (var CurrentWorm in RecentlyCollided)
            {
                if (!SourceWeapon.SelfDamage && CurrentWorm == SourceWorm) continue;

                if (!RecentlyDamagedTargets.Contains(CurrentWorm))
                {
                    Explode(true, CurrentWorm);
                    break;
                }

                RecentlyDamagedTargetsNew.Add(CurrentWorm);
            }


            RecentlyDamagedTargets.Clear();
            List<Worm> Temp = RecentlyDamagedTargets;
            RecentlyDamagedTargets = RecentlyDamagedTargetsNew;
            RecentlyDamagedTargetsNew = Temp;

        }

        public void Draw(SpriteBatch spriteBatch, float DeltaTime)
        {
            SpriteEffects SF = SpriteEffects.None;
            if (Velocity.X > 0) SF = SpriteEffects.FlipHorizontally;

            
            spriteBatch.Draw(Texture, Position - Level.CameraPosition, new Rectangle(0, 0, Texture.Width, Texture.Height), Color.White, Rotation, new Vector2(Texture.Width / 2, Texture.Height / 2), 1, SF, 0);
        }

    }
}
