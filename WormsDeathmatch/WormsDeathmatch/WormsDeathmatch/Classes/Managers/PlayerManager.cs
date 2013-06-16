using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace WormsDeathmatch
{
    /// <summary>
    /// Handle input, and draw gui for local players
    /// Store player data
    /// </summary>
    public class PlayerManager
    {
        public Level Level;
        public Dictionary<int, Player> Players = new Dictionary<int, Player>(); //
        public Color[] PlayerColors = { Color.Green, Color.Blue, Color.Red, Color.Yellow };

        private int MaxID = 0; //
        private int LocalPlayers = 0;
        private Vector2[] GUIPos = new Vector2[] { new Vector2(5, 5), new Vector2(695, 5) };

        public void MouseClickDownLeft(object Sender, GameWindow.MouseData Mouse)
        {
            foreach (var Player in Players)
            {
                if (Player.Value.LocalMouse == Mouse)
                {
                    Player.Value.WannaShoot = true;
                    break;
                }
            }
        }
        public void MouseClickUpLeft(object Sender, GameWindow.MouseData Mouse)
        {
            foreach (var Player in Players)
            {
                if (Player.Value.LocalMouse == Mouse)
                {
                    Player.Value.WannaShoot = false;
                    break;
                }
            }
        }

        public void MouseClickDownRight(object Sender, GameWindow.MouseData Mouse)
        {
            foreach (var Player in Players)
            {
                if (Player.Value.LocalMouse == Mouse)
                {
                    Player.Value.WannaMovement = true;
                    break;
                }
            }
        }
        public void MouseClickUpRight(object Sender, GameWindow.MouseData Mouse)
        {
            foreach (var Player in Players)
            {
                if (Player.Value.LocalMouse == Mouse)
                {
                    Player.Value.WannaMovement = false;
                    break;
                }
            }
        }

        public void MouseWheelUp(object Sender, GameWindow.MouseData Mouse)
        {
            foreach (var Player in Players)
            {
                if (Player.Value.LocalMouse == Mouse)
                {
                    Worm PlayerWorm = Player.Value.ControlledWorm;
                    PlayerWorm.WeaponIndex++;
                    if (PlayerWorm.WeaponIndex >= PlayerWorm.CarriedWeapons.Count)
                    {
                        PlayerWorm.WeaponIndex = 0;
                    }
                    PlayerWorm.CurrentWeapon = PlayerWorm.CarriedWeapons[PlayerWorm.WeaponIndex];
                    Player.Value.CreateWeaponSwapTag(PlayerWorm.CurrentWeapon);
                }
            }          
        }
        public void MouseWheelDown(object Sender, GameWindow.MouseData Mouse)
        {
            foreach (var Player in Players)
            {
                if (Player.Value.LocalMouse == Mouse)
                {
                    Worm PlayerWorm = Player.Value.ControlledWorm;
                    PlayerWorm.WeaponIndex--;
                    if (PlayerWorm.WeaponIndex < 0)
                    {
                        PlayerWorm.WeaponIndex = PlayerWorm.CarriedWeapons.Count - 1;
                    }
                    PlayerWorm.CurrentWeapon = PlayerWorm.CarriedWeapons[PlayerWorm.WeaponIndex];
                    Player.Value.CreateWeaponSwapTag(PlayerWorm.CurrentWeapon);
                }
            }
        }

        public void AddPlayer(Player NewPlayer)
        {
            NewPlayer.ID = MaxID++;
            Players.Add(NewPlayer.ID, NewPlayer);
            NewPlayer.Color = PlayerColors[NewPlayer.ID];
            NewPlayer.ControlledWorm = new Worm();
            NewPlayer.NameOrigin = ResourceManager.NameFont.MeasureString(NewPlayer.Name) / 2;
            NewPlayer.NameOrigin.Y += 20;
            Level.InitWorm(NewPlayer.ControlledWorm);
            Console.WriteLine("{0} joined the game as {1}!", NewPlayer.Name, NewPlayer.Type.ToString());
            if (NewPlayer.Type == Player.PlayerType.Local)
            {
                if (LocalPlayers < GUIPos.Length)
                {
                    NewPlayer.LocalGUIPosition = GUIPos[LocalPlayers];
                }
                LocalPlayers++;
            }
        }

        public void DrawGUIBehindWorm(SpriteBatch spriteBatch, float DeltaTime, Player CurrentPlayer)
        {
            // Charging weapon
            if (CurrentPlayer.ControlledWorm.CurrentWeapon != null && CurrentPlayer.ControlledWorm.CurrentWeapon.Charging)
            {
                Vector2 WormScreenPosition = (CurrentPlayer.ControlledWorm.Position - CurrentPlayer.ControlledWorm.Level.CameraPosition);
                float Distance = (WormScreenPosition - new Vector2(CurrentPlayer.LocalMouse.PosX, CurrentPlayer.LocalMouse.PosY)).Length();
                float Angle = CurrentPlayer.ControlledWorm.WeaponAngle + (float)Math.PI / 2;

                float ChargePercent = CurrentPlayer.ControlledWorm.CurrentWeapon.TimeCharging / CurrentPlayer.ControlledWorm.CurrentWeapon.Data.ChargeTime;

                spriteBatch.Draw(ResourceManager.GUI.WeaponChargeBar, WormScreenPosition, new Rectangle(0, 0, (int)(ResourceManager.GUI.WeaponChargeBar.Width), (int)(ChargePercent * ResourceManager.GUI.WeaponChargeBar.Height)), Color.White, Angle, new Vector2(ResourceManager.GUI.WeaponChargeBar.Width / 2.0f, 0), new Vector2(1, 1), SpriteEffects.None, 0);
            }

        }

        public void DrawGUI(SpriteBatch spriteBatch, float DeltaTime, Player CurrentPlayer)
        {
            // Aim
            if (CurrentPlayer.LocalMouse != null)
            {
                float AimSize;
                if (CurrentPlayer.ControlledWorm.CurrentWeapon != null && CurrentPlayer.ControlledWorm.CurrentWeapon.Data.ChargeTime != 0)
                {
                    AimSize = (CurrentPlayer.ControlledWorm.AccuracyPenalty + 
                        (1 - (CurrentPlayer.ControlledWorm.CurrentWeapon.TimeCharging / CurrentPlayer.ControlledWorm.CurrentWeapon.Data.ChargeTime)) 
                        * (CurrentPlayer.ControlledWorm.CurrentWeapon.Data.ChargeInaccuracyBonusUncharged) ) 
                        / 2.0f;
                }
                else
                {
                   AimSize = (CurrentPlayer.ControlledWorm.AccuracyPenalty) / 2.0f;
                }
                AimSize = Math.Min(AimSize, 5);
                spriteBatch.Draw(ResourceManager.GUI.CursorTexture, new Vector2(CurrentPlayer.LocalMouse.PosX, CurrentPlayer.LocalMouse.PosY), new Rectangle(0, 0, ResourceManager.GUI.CursorTexture.Width, ResourceManager.GUI.CursorTexture.Height), CurrentPlayer.Color, 0, new Vector2(ResourceManager.GUI.CursorTexture.Width / 2, ResourceManager.GUI.CursorTexture.Height / 2), Math.Max(AimSize, 0.1f), SpriteEffects.None, 0);

                // Draw word coord in cursor position
                //spriteBatch.DrawString(ResourceManager.Arial, string.Format("{0}, {1}", CurrentPlayer.LocalMouse.PosX + Level.CameraPosition.X, CurrentPlayer.LocalMouse.PosY + Level.CameraPosition.Y), new Vector2(CurrentPlayer.LocalMouse.PosX, CurrentPlayer.LocalMouse.PosY), Color.Orange); 
            }

            // Hp Bar
            int HpFillBarPercent = (int)(CurrentPlayer.ControlledWorm.LifeCurrent / CurrentPlayer.ControlledWorm.LifeMaximum * 100);
            spriteBatch.Draw(ResourceManager.GUI.Bar, new Rectangle((int)CurrentPlayer.LocalGUIPosition.X, (int)CurrentPlayer.LocalGUIPosition.Y, HpFillBarPercent, 8), CurrentPlayer.ColorWithAlpha);
            spriteBatch.Draw(ResourceManager.GUI.Bar, new Rectangle((int)CurrentPlayer.LocalGUIPosition.X + HpFillBarPercent, (int)CurrentPlayer.LocalGUIPosition.Y, 100 - HpFillBarPercent, 8), new Color(150, 150, 150, 150));

            // Magazine capility
            if (CurrentPlayer.ControlledWorm.CurrentWeapon != null)
            {
                float MagazinePrecent;
                if (!CurrentPlayer.ControlledWorm.CurrentWeapon.Reloading)
                {
                    MagazinePrecent = (CurrentPlayer.ControlledWorm.CurrentWeapon.Magazine * 1.0f / CurrentPlayer.ControlledWorm.CurrentWeapon.Data.MagazineSize * 100.0f);
                }
                else
                {
                    MagazinePrecent = (CurrentPlayer.ControlledWorm.CurrentWeapon.TimeToReloadEnd / CurrentPlayer.ControlledWorm.CurrentWeapon.Data.ReloadTime * 100);
                    MagazinePrecent = 100 - MagazinePrecent;
                }
                spriteBatch.Draw(ResourceManager.GUI.Bar, new Rectangle((int)(CurrentPlayer.LocalGUIPosition.X), (int)(CurrentPlayer.LocalGUIPosition.Y + 10), (int)MagazinePrecent, ResourceManager.GUI.Bar.Height), new Rectangle(0, 0, ResourceManager.GUI.Bar.Width, ResourceManager.GUI.Bar.Height), Color.Gray);
                spriteBatch.DrawString(ResourceManager.Arial, string.Format("{0} / {1}", CurrentPlayer.ControlledWorm.CurrentWeapon.Magazine, CurrentPlayer.ControlledWorm.CurrentWeapon.Data.MagazineSize),
                    new Vector2(CurrentPlayer.LocalGUIPosition.X, CurrentPlayer.LocalGUIPosition.Y + 10), Color.Black);
            }

            // Movement device energy
            if (CurrentPlayer.ControlledWorm.MovementDevice != null)
            {
                int JetpackLength = (int)(CurrentPlayer.ControlledWorm.MovementDevice.Fuel / CurrentPlayer.ControlledWorm.MovementDevice.FuelMaximum * 100.0f);
                spriteBatch.Draw(ResourceManager.GUI.Bar, new Rectangle((int)CurrentPlayer.LocalGUIPosition.X, (int)CurrentPlayer.LocalGUIPosition.Y + 20, JetpackLength, 8), Color.Cyan);
                spriteBatch.Draw(ResourceManager.GUI.Bar, new Rectangle((int)CurrentPlayer.LocalGUIPosition.X + JetpackLength, (int)CurrentPlayer.LocalGUIPosition.Y + 20, 100 - JetpackLength, 8), new Color(150, 150, 150, 150));
            }
        }

        public void Draw(SpriteBatch spriteBatch, float DeltaTime)
        {

            foreach (var KvP in Players)
            {
                if (KvP.Value.Type == Player.PlayerType.Local)
                {
                    DrawGUIBehindWorm(spriteBatch, DeltaTime, KvP.Value);
                }

                KvP.Value.ControlledWorm.Draw(spriteBatch, DeltaTime);

                if (KvP.Value.Type == Player.PlayerType.Local)
                {
                    DrawGUI(spriteBatch, DeltaTime, KvP.Value);
                }

                // Player name
                spriteBatch.DrawString(ResourceManager.Arial, KvP.Value.Name, KvP.Value.ControlledWorm.Position - Level.CameraPosition, KvP.Value.Color, 0, KvP.Value.NameOrigin, 1, SpriteEffects.None, 0);
            }
        }

        public void Update(float DeltaTime)
        {
            foreach (var KvP in Players)
            {
                KvP.Value.Update(DeltaTime);
            }
        }
    }
}
