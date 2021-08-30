using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using WINDXN.Classes;

namespace WINDXN
{
    public class Engine : Game
    {
        private GraphicsDeviceManager Graphics;
        private SpriteBatch SpriteBatch;

        public Scene CurrentScene;

        public Engine()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Assets";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();

            // Engine config
            Graphics.PreferredBackBufferWidth = 1024;
            Graphics.PreferredBackBufferHeight = 768;

            Graphics.ApplyChanges();

            // Static classes
            Input.Initialize(this);
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Static classes
            Input.Update();

            // Scene
            CurrentScene.Update(ref gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // Setting up
            GraphicsDevice.Clear(Color.Black);

            SpriteBatch.Begin();

            // Scene
            CurrentScene.Render(SpriteBatch);

            SpriteBatch.End();

            base.Draw(gameTime);
        }

        // Utility functions
        public void ChangeScene(string scene)
        {
            CurrentScene = null;
            CurrentScene = new Scene(scene);
            CurrentScene.Engine = this;
        }
    }
}
