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

namespace STG
{
    public class GameObject
    {
        protected Vector2 pos; //position
        protected Rectangle boundingBox; //boundingBox of the sprite, likely not the hitbox.  used mainly for drawing
        protected Sprite sprite;
        protected float rotation;
        
        public GameObject()
        {
            Initialize();
        }

        virtual protected void Initialize()
        {

        }

        virtual public void Update()
        {
            //update the boundingBox's coords to move with the object
            boundingBox.X = (int)pos.X;
            boundingBox.Y = (int)pos.Y;
        }

        virtual public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            if (sprite != null)
            {
                sprite.Update();
                sprite.Draw(spriteBatch, boundingBox, Color.White, rotation, new Vector2((float)sprite.Width / 2, (float)sprite.Height / 2), 0, 1 - (pos.Y / Game1.windowHeight));
            }

            spriteBatch.End();
        }

        public Vector2 Position { get { return pos; } }

        public Rectangle BoundingBox { get { return boundingBox; } }

        public float AngleToTarget(Vector2 pos1, Vector2 pos2)
        {
            double deltaX = (pos2.X - pos1.X);
            double deltaY = (pos2.Y - pos1.Y);
            return ((float)Math.Atan2(deltaY, deltaX) / ((float)Math.PI / 180) + 360) % 360;
        }

        public float AngleToTarget(Vector2 targetPos)
        {
            double deltaX = (targetPos.X - pos.X);
            double deltaY = (targetPos.Y - pos.Y);
            return ((float)Math.Atan2(deltaY, deltaX) / ((float)Math.PI / 180) + 360) % 360;
        }

        public float DistanceToTarget(Vector2 pos1, Vector2 pos2)
        {
            return (float)Math.Sqrt(Math.Pow((float)pos1.X - pos2.X, 2) + Math.Pow((float)pos1.Y - pos2.Y, 2));

        }

        public float DistanceToTarget(Vector2 targetPos)
        {
            return (float)Math.Sqrt(Math.Pow((float)pos.X - targetPos.X, 2) + Math.Pow((float)pos.Y - targetPos.Y, 2));

        }
    }
}
