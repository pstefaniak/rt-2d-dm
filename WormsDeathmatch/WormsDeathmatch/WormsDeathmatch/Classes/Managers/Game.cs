using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Windows.Forms;

namespace WormsDeathmatch
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public GameWindow MainWindow;
        string CurrentLevelName;
        string[] PlayerStrings;
        bool AllWeaponsEnabled = true;

        public Game(GameWindow OwnerWindow, string LevelToLoad, string[] pPlayerStrings, bool EnableAllWeapons)
        {
            AllWeaponsEnabled = EnableAllWeapons;
            CurrentLevelName = LevelToLoad;
            PlayerStrings = pPlayerStrings;

            MainWindow = OwnerWindow; 
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = MainWindow.ViewportSize.Width,
                PreferredBackBufferHeight = MainWindow.ViewportSize.Height
            };

            graphics.PreparingDeviceSettings += graphics_PreparingDeviceSettings;
            System.Windows.Forms.Control.FromHandle(Window.Handle).VisibleChanged += MainGame_VisibleChanged;
           
            Content.RootDirectory = "Content";

        }
        void MainGame_VisibleChanged(object sender, System.EventArgs e)
        {
            if (System.Windows.Forms.Control.FromHandle(Window.Handle).Visible)
                System.Windows.Forms.Control.FromHandle(Window.Handle).Visible = false;
        }

        void graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            e.GraphicsDeviceInformation.PresentationParameters.DeviceWindowHandle = MainWindow.CanvasHandle;
        }
 
        protected override void Initialize()
        {

            Window.Title = "Game";
            base.Initialize();

            
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            if (CurrentLevelName != "")
                CurrentLevel = new Level(Content, this, CurrentLevelName, System.IO.Path.Combine(Application.StartupPath, "Data"), PlayerStrings, AllWeaponsEnabled);
            
        }

        protected override void UnloadContent()
        {

        }

        Level CurrentLevel = null;

        protected override void Update(GameTime gameTime)
        {

                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                    this.Exit();

                CurrentLevel.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

                base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            spriteBatch.Begin();
            CurrentLevel.Draw(spriteBatch, (float)gameTime.ElapsedGameTime.TotalSeconds);

            //Mouse
            MouseState CurrentMouseState = Mouse.GetState();
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
