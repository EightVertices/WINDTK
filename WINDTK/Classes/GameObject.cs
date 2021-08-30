using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WINDXN.Classes
{
    public abstract class GameObject
    {
        public Engine Engine;
        public Scene CurrentScene;

        // Transform
        public Point Position = new Point(0, 0);
        public float Rotation = 0f;
        public float Scale = 1f;

        //public Matrix Transform = new Matrix();

        public virtual void Initialize()
        {
            LoadContent();
        }

        public virtual void Update(ref GameTime gameTime)
        {
            /*Transform = (
                Matrix.Identity *
                Matrix.CreateTranslation(new Vector3(Position.X, Position.Y, 0f)) *
                Matrix.CreateRotationZ(Rotation * (180f / MathF.PI)) *
                Matrix.CreateScale(Scale)
            );*/
        }

        public virtual void Render(SpriteBatch spriteBatch)
        {

        }

        protected virtual void LoadContent()
        {
            
        }

        // Utility functions
    }
}