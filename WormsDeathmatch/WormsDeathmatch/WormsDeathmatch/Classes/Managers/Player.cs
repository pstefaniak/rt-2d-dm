using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
 
namespace WormsDeathmatch
{
    /// <summary>
    /// Player data, contain controls, ingame-stats, current key states and player metadata
    /// </summary>
    public class Player
    {
        public enum PlayerType
        {
            Local = 0,
            Network = 1,
        }

        PlayerManager Manager;

        public Player(PlayerManager PManager)
        {
            Manager = PManager;
        }

        public PlayerType Type;
        public Keys LocalJump;
        public Keys LocalShoot;
        public Keys LocalMoveLeft;
        public Keys LocalMoveRight;
        public bool WannaShoot = false;
        public bool WannaMovement = false;
        public Vector2 LocalGUIPosition;
        public GameWindow.MouseData LocalMouse;

        public String Name;
        public Vector2 NameOrigin;
        public int ID;

        private Color _ColorWithAlpha;
        public Color ColorWithAlpha { get { return _ColorWithAlpha; } }
        private Color _Color;
        public Color Color { get { return _Color; } set { _Color = value; _ColorWithAlpha = value; _ColorWithAlpha.A = 150; } }


        public Worm ControlledWorm;
        public TextTag WeaponChangeTag = null;


        public void CreateWeaponSwapTag(Weapon CurrentWeapon)
        {
            // Cleanup
            if(WeaponChangeTag != null)
            {
                WeaponChangeTag.Duration = -1;
            }
            
            WeaponChangeTag = new TextTag(ControlledWorm.Level, CurrentWeapon.Data.Name, ResourceManager.Arial, ControlledWorm.Position + new Vector2(0, -30), Color.Orange, 2);

            ControlledWorm.Level.TextTags.Add(WeaponChangeTag);
        }

        public void Update(float DeltaTime)
        {
            if (Type == PlayerType.Local)
            {
                if (WeaponChangeTag != null)
                {
                    WeaponChangeTag.Position = ControlledWorm.Position + new Vector2(0, -30);
                    if (WeaponChangeTag.Duration <= 0)
                    {
                        WeaponChangeTag = null;
                    }
                }

                if (LocalMouse != null)
                {
                    Vector2 PosOnScreen = (ControlledWorm.Position - Manager.Level.CameraPosition) - new Vector2(LocalMouse.PosX, LocalMouse.PosY);
                    ControlledWorm.WeaponAngle = (float)Math.Atan2(PosOnScreen.Y, PosOnScreen.X);
                }


                ControlledWorm.ControlsIsMovingLeft = Manager.Level.CurrentKeyboard.IsKeyDown(LocalMoveLeft);
                ControlledWorm.ControlsIsMovingRight = Manager.Level.CurrentKeyboard.IsKeyDown(LocalMoveRight);
                ControlledWorm.ControlsJump = Manager.Level.CurrentKeyboard.IsKeyDown(LocalJump);
                ControlledWorm.ControlsShoot = WannaShoot;
                ControlledWorm.ControlsMovement = WannaMovement;

                if (LocalMouse != null && ControlledWorm.RecoilBuffer > 0) 
                {
                    Vector2 WormScreenPosition = (ControlledWorm.Position - ControlledWorm.Level.CameraPosition);
                    float Distance = (WormScreenPosition - new Vector2(LocalMouse.PosX, LocalMouse.PosY)).Length();
                    float Angle = ControlledWorm.WeaponAngle+ (float)Math.PI;
                    if (Angle < Math.PI / 2 || Angle > Math.PI * 3/2.0f)
                        Angle -= (float)(ControlledWorm.RecoilBuffer * 3 * Math.PI / 180);
                    else
                        Angle += (float)(ControlledWorm.RecoilBuffer * 3 * Math.PI / 180);

                    LocalMouse.PosX = (int)(WormScreenPosition.X + (Math.Cos(Angle) * Distance));
                    LocalMouse.PosY = (int)(WormScreenPosition.Y + (Math.Sin(Angle) * Distance));
                    ControlledWorm.RecoilBuffer = 0;


                }
            }
        }


    }
}
