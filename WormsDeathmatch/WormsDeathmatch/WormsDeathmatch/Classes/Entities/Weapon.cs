using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WormsDeathmatch
{
    /// <summary>
    /// Weapon meta data (single instance for weapon)
    /// </summary>
    public class WeaponData
    {
        // Projectile stats
        public int ProjectileDamage;
        public int ProjectileForce;
        public int ProjectileExplosionRadius;
        public float ProjectileMass;
        public int ProjectileSpeed;
        public float ProjectileLifeTime = -10;

        // Weapon stats
        public float FireInterval;
        public float Recoil;
        public int MagazineSize;
        public float ReloadTime;
        public float Inaccuracy = 0;
        public string Name;
        public bool SelfDamage;
        public bool FirendlyFire;

        public float ChargeTime = 0;
        public float ChargeSpeedBonusStart = 0;
        public float ChargeDamageBonusStart = 0;
        /// <summary>
        /// Decreases to acurracy
        /// </summary>
        public float ChargeInaccuracyBonusUncharged = 0;

        // Actor
        private Texture2D _WeaponTexture;
        public Texture2D WeaponTexture { get { return _WeaponTexture; } set { _WeaponTexture = value; _WeaponTextureOrigin = new Vector2(value.Width / 2.0f, value.Height / 2.0f); } }
        public Texture2D ProjectileTexture;
        private Vector2 _WeaponTextureOrigin;
        public Vector2 WeaponTextureOrigin { get { return _WeaponTextureOrigin; } }

    }

    /// <summary>
    /// Local weapon data, hold cooldowns etc.
    /// </summary>
    public class Weapon
    {
        public WeaponData Data;


        public float TimeToNextShoot;
        public float TimeToReloadEnd;
        public float TimeCharging;
        public bool Charging;
        public bool Reloading;
        public int Magazine;

        public Worm Carrier;
        public Weapon(Worm Owner, WeaponData WeaponData)
        {
            Data = WeaponData;
            Carrier = Owner;
            Magazine = WeaponData.MagazineSize;
        }

        public void Update(float DeltaTime)
        {
            // If weapon is charging
            if (Charging)
            {
                TimeCharging += DeltaTime;
                if (TimeCharging > Data.ChargeTime || Carrier.ControlsShoot == false)
                {
                    Charging = false;
                    float ChargedPrecent = TimeCharging / Data.ChargeTime;
                    if (ChargedPrecent > 1) ChargedPrecent = 1;
                    TimeCharging = 0;
                    Shoot(ChargedPrecent);
                }
            }

            // Time interval
            if (TimeToNextShoot > 0) TimeToNextShoot -= DeltaTime;
            {
                if (Carrier.AccuracyPenalty > 0)
                {
                    Carrier.AccuracyPenalty -= DeltaTime;
                    if (Carrier.AccuracyPenalty > 6) Carrier.AccuracyPenalty = 6;
                }
                else Carrier.AccuracyPenalty = 0;
            }

            // Accuracy
            if (Carrier.AccuracyPenalty > Data.Inaccuracy) Carrier.AccuracyPenalty -= DeltaTime;
            else Carrier.AccuracyPenalty = Data.Inaccuracy;

            // Handle reloading
            if (Reloading)
            {
                if (TimeToReloadEnd > 0) TimeToReloadEnd -= DeltaTime;
                else
                {
                    Reloading = false;
                    Magazine = Data.MagazineSize;
                }
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            float WeaponRotation = Carrier.WeaponAngle;
            SpriteEffects SE = SpriteEffects.None;
            if (!Carrier.FacingLeft)
            {
                SE = SpriteEffects.FlipHorizontally;
                WeaponRotation += (float)Math.PI;
            }
            
            spriteBatch.Draw(Data.WeaponTexture, Carrier.Position - Carrier.Level.CameraPosition, /// ->
                new Rectangle(0, 0, Data.WeaponTexture.Width, Data.WeaponTexture.Height), Color.White, WeaponRotation, Data.WeaponTextureOrigin, 1, SE, 0);
        }

        // Fire
        public void CreateProjectile(float ChargePrecent)
        {
            Projectile NewProjectile = new Projectile(this);
            NewProjectile.Initialize(Carrier.Level, Data.ProjectileMass);
            NewProjectile.Position = Carrier.Position;
            float WeaponRotation = Carrier.WeaponAngle + (float)Math.PI + ((float)new Random().NextDouble() * 2 - 1) *  /// ->
                (Carrier.AccuracyPenalty + (Data.ChargeInaccuracyBonusUncharged * (1 - ChargePrecent))) * (float)Math.PI / 180;

            NewProjectile.Rotation = WeaponRotation;

            float Speed = Data.ProjectileSpeed;
            if (Data.ChargeTime != 0)
            {
                Speed = Data.ChargeSpeedBonusStart + ((Data.ProjectileSpeed - Data.ChargeSpeedBonusStart) * ChargePrecent);
            }
            NewProjectile.Velocity = new Vector2(Speed * (float)Math.Cos(WeaponRotation), Speed * (float)Math.Sin(WeaponRotation));

            float Damage = Data.ProjectileDamage;
            if (Data.ChargeTime != 0)
            {
                Damage = Data.ChargeDamageBonusStart + ((Data.ProjectileDamage - Data.ChargeDamageBonusStart) * ChargePrecent);
            }


            NewProjectile.ExplosionRadius = Damage / Data.ProjectileDamage * Data.ProjectileExplosionRadius;
            NewProjectile.Damage = Damage;
            NewProjectile.LifeTime = Data.ProjectileLifeTime;
            NewProjectile.Texture = Data.ProjectileTexture;
            NewProjectile.GroundCollisionSize = new Vector2(1, 1);


            Carrier.Level.Projectiles.Add(NewProjectile); //

        }

        public void Shoot(float ChargePrecent = 1)
        {
            CreateProjectile(ChargePrecent);
            Carrier.RecoilBuffer += Data.Recoil;

            Magazine--;
            TimeToNextShoot = Data.FireInterval;
            Carrier.AccuracyPenalty += Data.FireInterval * 5;
            if (Magazine == 0)
            {
                Reloading = true;
                TimeToReloadEnd = Data.ReloadTime;
            }
        }

        /// <summary>
        /// Handles local shooting like recoil etc
        /// </summary>
        public void LocalShoot()
        {
            if (TimeToNextShoot <= 0 && !Reloading)
            {
                if (Data.ChargeTime == 0)
                {
                    Shoot();
                }
                else // Weapon is chargable
                {
                    if (!Charging)
                    {
                        Charging = true;
                        TimeCharging = 0.005f;
                    }
                }
            }
        }

    }
}
