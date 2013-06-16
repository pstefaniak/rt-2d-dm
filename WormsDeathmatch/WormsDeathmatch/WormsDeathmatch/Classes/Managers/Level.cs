using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using System.IO;
using INI;

namespace WormsDeathmatch
{
    /// <summary>
    /// Hold data
    /// </summary>
    public class Level
    {

        // State variables
        public KeyboardState CurrentKeyboard;
        public MouseState CurrentMouse;
        public Vector2 CameraPosition = new Vector2(0, 0);
        public Color[] GroundData;
        public ContentManager Content;

        public static Texture2D Ground;

        // Data
        public List<Worm> Worms = new List<Worm>();

        public List<Projectile> Projectiles = new List<Projectile>(); // public ?
        public List<Projectile> ProjectilesToRemove = new List<Projectile>();

        public List<TextTag> TextTags = new List<TextTag>();
        public List<TextTag> TextTagsToRemove = new List<TextTag>();

        public Dictionary<string, WeaponData> Weapons = new Dictionary<string, WeaponData>();
        public Dictionary<string, MovementDeviceData> MovementDevices = new Dictionary<string, MovementDeviceData>();


        public PlayerManager PlayerManager = new PlayerManager();

        public string LevelName;

        // Settings, config
        public Vector2 cGravity = new Vector2(0, 50);
        public int cLifeMaximum = 200;
        public int cLifeStart = 150;
        public float cWormMass = 5;
        public Vector2 cWormSize = new Vector2(8, 12);
        public float cWormSpeed = 50;
        public Vector2 cWormJumpVelocityBonus = new Vector2(75, -200);
        public float cWormGroundFrictionFactor = 100;
        public float cExplosionGroundDestructionPercent = 0.66f;
        public WeaponData cStartingWeapon;
        public bool EnabledWeapons;

        #region Initialization
        public void LoadConfiguration(string LevelDirectory)
        {
            const string GENERAL_CONFIG = "General";

            string INIPath = System.IO.Path.Combine(LevelDirectory, "Config.ini");
            if(System.IO.File.Exists(INIPath))
            {
                INIFile ConfigFile = new INIFile(INIPath);

                cGravity.X = float.Parse(ConfigFile.IniReadValue(GENERAL_CONFIG, "GravityX"));
                cGravity.Y = float.Parse(ConfigFile.IniReadValue(GENERAL_CONFIG, "GravityY"));

                cLifeMaximum = int.Parse(ConfigFile.IniReadValue(GENERAL_CONFIG, "LifeMaximum"));
                cLifeStart = int.Parse(ConfigFile.IniReadValue(GENERAL_CONFIG, "LifeStarting"));

                cWormMass = int.Parse(ConfigFile.IniReadValue(GENERAL_CONFIG, "WormMass"));
                cWormSize.X = float.Parse(ConfigFile.IniReadValue(GENERAL_CONFIG, "WormSizeX"));
                cWormSize.Y = float.Parse(ConfigFile.IniReadValue(GENERAL_CONFIG, "WormSizeY"));
                cWormSpeed = float.Parse(ConfigFile.IniReadValue(GENERAL_CONFIG, "WormSpeed"));
                cWormJumpVelocityBonus.X = float.Parse(ConfigFile.IniReadValue(GENERAL_CONFIG, "WormJumpVelocityX"));
                cWormJumpVelocityBonus.Y = float.Parse(ConfigFile.IniReadValue(GENERAL_CONFIG, "WormJumpVelocityY"));
                
                cExplosionGroundDestructionPercent = float.Parse(ConfigFile.IniReadValue(GENERAL_CONFIG, "ExplosionGroundDestructionPercent"));
                
                LoadWeaponData(System.IO.Path.Combine(LevelDirectory, "Weapons"));

                string DefaultWeapon = ConfigFile.IniReadValue(GENERAL_CONFIG, "DefaultWeapon");
                if(!string.IsNullOrEmpty(DefaultWeapon))
                {
                    if (Weapons.ContainsKey(DefaultWeapon))
                    {
                        cStartingWeapon = Weapons[DefaultWeapon];
                    }
                    else Console.WriteLine("Error loading data - default weapon not found."); //
                }
                else Console.WriteLine("Error loading data - default weapon string not found."); //
               
                
            }
            else
            {
                Console.WriteLine("Can't find file " + Path.GetFileName("Config.ini") + " from " + LevelName  + ". Loading default configuration."); //
            }
        }

        public void LoadWeaponData(string DirectoryPath)
        {
            DirectoryInfo WeaponDirectory = new DirectoryInfo(DirectoryPath);

            foreach (var File in WeaponDirectory.EnumerateFiles("*.ini"))
            {
                const string WeaponSection = "Weapon";
                INIFile WeaponFile = new INIFile(File.FullName);

                WeaponData NewWeapon = new WeaponData();

                if(WeaponFile.TryLoading(WeaponSection, "ProjectileDamage"))
                    NewWeapon.ProjectileDamage = int.Parse(WeaponFile.LastLoadedData());
                if (WeaponFile.TryLoading(WeaponSection, "ProjectileForce"))
                    NewWeapon.ProjectileForce = int.Parse(WeaponFile.LastLoadedData());
                if (WeaponFile.TryLoading(WeaponSection, "ProjectileExplosionRadius"))
                    NewWeapon.ProjectileExplosionRadius = int.Parse(WeaponFile.LastLoadedData());
                if (WeaponFile.TryLoading(WeaponSection, "ProjectileMass"))
                    NewWeapon.ProjectileMass = float.Parse(WeaponFile.LastLoadedData());
                if (WeaponFile.TryLoading(WeaponSection, "ProjectileSpeed"))
                    NewWeapon.ProjectileSpeed = int.Parse(WeaponFile.LastLoadedData());
                if (WeaponFile.TryLoading(WeaponSection, "ProjectileLifeTime"))
                    NewWeapon.ProjectileLifeTime = float.Parse(WeaponFile.LastLoadedData());
                
                if (WeaponFile.TryLoading(WeaponSection, "ChargeTime"))
                    NewWeapon.ChargeTime = float.Parse(WeaponFile.LastLoadedData());
                if (WeaponFile.TryLoading(WeaponSection, "ChargeSpeedBonusStart"))
                    NewWeapon.ChargeSpeedBonusStart = float.Parse(WeaponFile.LastLoadedData());
                if (WeaponFile.TryLoading(WeaponSection, "ChargeDamageBonusStart"))
                    NewWeapon.ChargeDamageBonusStart = int.Parse(WeaponFile.LastLoadedData());
                if (WeaponFile.TryLoading(WeaponSection, "ChargeInaccuracyBonusUncharged"))
                    NewWeapon.ChargeInaccuracyBonusUncharged = float.Parse(WeaponFile.LastLoadedData());
               
                if (WeaponFile.TryLoading(WeaponSection, "SelfDamage"))
                    NewWeapon.SelfDamage = bool.Parse(WeaponFile.LastLoadedData());
                if (WeaponFile.TryLoading(WeaponSection, "FirendlyFire"))
                    NewWeapon.FirendlyFire = bool.Parse(WeaponFile.LastLoadedData());


                if (WeaponFile.TryLoading(WeaponSection, "WeaponTexture"))
                    NewWeapon.WeaponTexture = ResourceManager.GetTexture(WeaponFile.LastLoadedData());
                if (WeaponFile.TryLoading(WeaponSection, "ProjectileTexture"))
                    NewWeapon.ProjectileTexture = ResourceManager.GetTexture(WeaponFile.LastLoadedData());

                if (WeaponFile.TryLoading(WeaponSection, "FireInterval"))
                    NewWeapon.FireInterval = float.Parse(WeaponFile.LastLoadedData());
                if (WeaponFile.TryLoading(WeaponSection, "Recoil"))
                    NewWeapon.Recoil = float.Parse(WeaponFile.LastLoadedData());
                if (WeaponFile.TryLoading(WeaponSection, "MagazineSize"))
                    NewWeapon.MagazineSize = int.Parse(WeaponFile.LastLoadedData());
                if (WeaponFile.TryLoading(WeaponSection, "ReloadTime"))
                    NewWeapon.ReloadTime = float.Parse(WeaponFile.LastLoadedData());
                if (WeaponFile.TryLoading(WeaponSection, "Inaccuracy"))
                    NewWeapon.Inaccuracy = float.Parse(WeaponFile.LastLoadedData());

                NewWeapon.Name = File.Name.Substring(0, File.Name.LastIndexOf('.'));

                if (NewWeapon.WeaponTexture != null && NewWeapon.ProjectileTexture != null)
                {
                    Weapons.Add(NewWeapon.Name, NewWeapon);
                }

            }
        }

        public Level(ContentManager GameContent, Game game, string LevelNameString, string LevelDirectory, string[] PlayerStrings, bool AllWeaponEnabled)
        {
            LevelName = LevelNameString;
            EnabledWeapons = AllWeaponEnabled;

            Content = GameContent;
            Console.WriteLine("Initializing level");

            game.MainWindow.MouseEventButtonDownLeft += new GameWindow.MouseEventHandler(PlayerManager.MouseClickDownLeft);
            game.MainWindow.MouseEventButtonUpLeft += new GameWindow.MouseEventHandler(PlayerManager.MouseClickUpLeft);
            game.MainWindow.MouseEventButtonDownRight += new GameWindow.MouseEventHandler(PlayerManager.MouseClickDownRight);
            game.MainWindow.MouseEventButtonUpRight += new GameWindow.MouseEventHandler(PlayerManager.MouseClickUpRight);
            game.MainWindow.MouseEventWheelUp += new GameWindow.MouseEventHandler(PlayerManager.MouseWheelUp);
            game.MainWindow.MouseEventWheelDown += new GameWindow.MouseEventHandler(PlayerManager.MouseWheelDown);

            ResourceManager.LoadResources(Content);
            LoadConfiguration(Path.Combine(LevelDirectory, LevelNameString));
            
            using (FileStream fileStream = new FileStream(Path.Combine(new string[] { LevelDirectory, LevelNameString, "Level" } ) + ".png", FileMode.Open))
            {
                    Ground = Texture2D.FromStream(game.GraphicsDevice, fileStream);
            }
            GroundData = new Color[Ground.Width * Ground.Height];
            Ground.GetData<Color>(GroundData);


            PlayerManager.Level = this;

            // Parse players
            foreach (var PlayerString in PlayerStrings)
            {
                if (string.IsNullOrEmpty(PlayerString)) continue;

                Player NewPlayer = new Player(PlayerManager);
                NewPlayer.Type = Player.PlayerType.Local;
                string[] Params = PlayerString.Substring(PlayerString.IndexOf('(') + 1, PlayerString.IndexOf(')') - PlayerString.IndexOf('(') - 1).Split(';');
                foreach (var Param in Params)
                {
                    if (Param.StartsWith("name="))
                    {
                        NewPlayer.Name = Param.Substring(Param.IndexOf('=') + 1);
                    }
                    else if (Param.StartsWith("color="))
                    {
                        string[] ColorString = Param.Substring(Param.IndexOf('=') + 1).Split(',');
                        NewPlayer.Color = new Microsoft.Xna.Framework.Color(int.Parse(ColorString[0]), int.Parse(ColorString[1]), int.Parse(ColorString[2]));
                    }
                    else if (Param.StartsWith("mouse="))
                    {
                        IntPtr MouseHandle = (IntPtr)int.Parse(Param.Substring(Param.IndexOf('=') + 1));

                        NewPlayer.LocalMouse = GameWindow.Instance.Mouses[MouseHandle];                      
                    }
                    else if (Param.StartsWith("keyboard="))
                    {
                        string KeyString = Param.Substring(Param.IndexOf('=') + 1);
                        if (KeyString.StartsWith("W"))
                        {
                            NewPlayer.LocalMoveLeft = Keys.A;
                            NewPlayer.LocalMoveRight = Keys.D;
                            NewPlayer.LocalJump = Keys.W;
                            NewPlayer.LocalShoot = Keys.R;
                        }
                        else if (KeyString.StartsWith("Up"))
                        {
                            NewPlayer.LocalMoveLeft = Keys.Left;
                            NewPlayer.LocalMoveRight = Keys.Right;
                            NewPlayer.LocalJump = Keys.Up;
                            NewPlayer.LocalShoot = Keys.Down;
                        }
                    }
                    else if (Param.StartsWith("weapons="))
                    {
                        // NYI
                    }
                }
                
                PlayerManager.AddPlayer(NewPlayer);
            }
        }

        public void InitWorm(Worm NewWorm)
        {
            NewWorm.Initialize(this, cWormMass);

            if(EnabledWeapons)
            {
                foreach (var WeaponData in Weapons)
                    NewWorm.CarriedWeapons.Add(new Weapon(NewWorm, WeaponData.Value));


                NewWorm.CurrentWeapon = NewWorm.CarriedWeapons[0];
                NewWorm.WeaponIndex = 0;
            }

            NewWorm.Respawn(new Vector2(new Random().Next(300, Level.Ground.Width - 300), 50));
            NewWorm.Position = new Vector2(400, 15);
            NewWorm.Animation = new Animation(ResourceManager.WormAnimationData);
            NewWorm.Animation.SetAnimation(0);
            NewWorm.GroundCollisionSize = cWormSize * 0.8f;
            NewWorm.ProjectileCollisionSize = cWormSize;

            NewWorm.LifeMaximum = cLifeMaximum;
            NewWorm.LifeCurrent = cLifeStart;

            Worms.Add(NewWorm);
        }
        #endregion


        public void Explosion(Vector2 Position, int Radius)
        {
            int PosX = (int)Position.X - Radius;
            int PosY = (int)Position.Y - Radius;

            int RadiusSquared = Radius * Radius;

            for( int x = PosX; x < PosX + 2*Radius; x++)
            {
                for(int y = Math.Max(PosY, 0); y < PosY + 2*Radius; y++)
                {
                    if( ((x - Position.X) * (x - Position.X)) + ((y - Position.Y) * (y - Position.Y)) <= RadiusSquared)
                    {
                        GroundData[y * Ground.Width + x] = Color.FromNonPremultiplied(0, 0, 0, 0);
                    }
                }
            }

            Ground.SetData(GroundData);
        }

        public void Draw(SpriteBatch spriteBatch, float DeltaTime)
        {
            #region Camera
            CameraPosition.X = 0;
            CameraPosition.Y = 0;
            int Sum = 0;
            foreach (var Player in PlayerManager.Players)
            {
                if (Player.Value.Type == WormsDeathmatch.Player.PlayerType.Local)
                {
                    CameraPosition += Player.Value.ControlledWorm.Position;
                    Sum++;
                }
            }
            CameraPosition /= Sum;
            CameraPosition.X -= spriteBatch.GraphicsDevice.Viewport.Width / 2;
            CameraPosition.Y -= spriteBatch.GraphicsDevice.Viewport.Height / 2;
            if (CameraPosition.X < 0) CameraPosition.X = 0;
            if (CameraPosition.Y < 0) CameraPosition.Y = 0;
            if (CameraPosition.X + spriteBatch.GraphicsDevice.Viewport.Width > Ground.Width) CameraPosition.X = Ground.Width - spriteBatch.GraphicsDevice.Viewport.Width;
            if (CameraPosition.Y + spriteBatch.GraphicsDevice.Viewport.Height > Ground.Height) CameraPosition.Y = Ground.Height - spriteBatch.GraphicsDevice.Viewport.Height;
            #endregion

            spriteBatch.Draw(ResourceManager.Skybox, new Rectangle(0, 0, ResourceManager.Skybox.Width, ResourceManager.Skybox.Height), Color.White);
            spriteBatch.Draw(Ground, new Rectangle(-(int)CameraPosition.X, -(int)CameraPosition.Y, Ground.Width, Ground.Height), Color.White);
            
            foreach (var CurrentProjectile in Projectiles)
            {
                CurrentProjectile.Draw(spriteBatch, DeltaTime);
            }

            foreach (var CurrentWorm in Worms)
            {
            }

            PlayerManager.Draw(spriteBatch, DeltaTime);
            
            foreach (var CurrentTextTag in TextTags)
            {
                CurrentTextTag.Draw(spriteBatch, DeltaTime);
            }
            foreach (var TextTagToRemove in TextTagsToRemove)
            {
                TextTags.Remove(TextTagToRemove);
            }
            TextTagsToRemove.Clear();
        }

        public void Update(float DeltaTime)
        {
            CurrentKeyboard = Keyboard.GetState();

            PlayerManager.Update(DeltaTime);

            foreach (var CurrentWorm in Worms)
            {
                CurrentWorm.Update(DeltaTime);
            }

            foreach (var CurrentProjectile in Projectiles)
            {
                CurrentProjectile.Update(DeltaTime);
            }
            foreach (var ProjectileToRemove in ProjectilesToRemove)
            {
                Projectiles.Remove(ProjectileToRemove);
            }
            ProjectilesToRemove.Clear();
        }
    }
}
