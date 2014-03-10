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
    /// <summary>
    /// A object in the game.  Everything that can interact with other objects should inherit this class.
    /// </summary>
    public class GameObject
    {
        /// <summary>
        /// The position of the object.
        /// </summary>
        protected Vector2 pos;

        /// <summary>
        /// The bounding box of the sprite.  This is the rectangle that the sprite will be drawn in.
        /// </summary>
        protected Rectangle boundingBox;

        /// <summary>
        /// The sprite for this game object.
        /// </summary>
        protected Sprite sprite;

        /// <summary>
        /// The angle that the sprite will be drawn at.
        /// </summary>
        protected float rotation;
        
        /// <summary>
        /// Initializes a new GameObject.  Don't use this, make a class that inherits GameObject and use that.
        /// </summary>
        public GameObject()
        {
            Initialize();
        }

        /// <summary>
        /// A function that is called whenever any GameObject is created.  Make an override function in a class that inherits GameObject.
        /// </summary>
        virtual protected void Initialize()
        {

        }

        /// <summary>
        /// Updates the object's member variables.
        /// </summary>
        virtual public void Update()
        {
            //update the boundingBox's coords to move with the object
            boundingBox.X = (int)pos.X;
            boundingBox.Y = (int)pos.Y;
        }

        /// <summary>
        /// Draws the object.
        /// </summary>
        /// <param name="spriteBatch">The spriteBatch that should be used for drawing.</param>
        virtual public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            if (sprite != null)
            {
                sprite.Update();
                sprite.Draw(spriteBatch, boundingBox, Color.White, rotation, new Vector2((float)sprite.Width / 2, (float)sprite.Height / 2), 0, 1 - (pos.Y / MainGame.windowHeight));
            }

            spriteBatch.End();
        }

        /// <summary>
        /// Returns the position of the object.
        /// </summary>
        public Vector2 Position { get { return pos; } }

        /// <summary>
        /// Returns the object's bounding box.
        /// </summary>
        public Rectangle BoundingBox { get { return boundingBox; } }

        /// <summary>
        /// Calculates the angle from one position to another.
        /// </summary>
        /// <param name="pos1">The starting position.</param>
        /// <param name="pos2">The ending position.</param>
        /// <returns></returns>
        public float AngleToTarget(Vector2 pos1, Vector2 pos2)
        {
            double deltaX = (pos2.X - pos1.X);
            double deltaY = (pos2.Y - pos1.Y);
            return ((float)Math.Atan2(deltaY, deltaX) / ((float)Math.PI / 180) + 360) % 360;
        }

        /// <summary>
        /// Calculates the angle from current position to another position.
        /// </summary>
        /// <param name="targetPos">The target position.</param>
        public float AngleToTarget(Vector2 targetPos)
        {
            double deltaX = (targetPos.X - pos.X);
            double deltaY = (targetPos.Y - pos.Y);
            return ((float)Math.Atan2(deltaY, deltaX) / ((float)Math.PI / 180) + 360) % 360;
        }

        /// <summary>
        /// Calculates the distance between two points.
        /// </summary>
        /// <param name="pos1">The first point.</param>
        /// <param name="pos2">The second point.</param>
        /// <returns></returns>
        public float DistanceToTarget(Vector2 pos1, Vector2 pos2)
        {
            return (float)Math.Sqrt(Math.Pow((float)pos1.X - pos2.X, 2) + Math.Pow((float)pos1.Y - pos2.Y, 2));

        }

        /// <summary>
        /// Calculates the distance between the current position and a given position.
        /// </summary>
        /// <param name="targetPos">The position to find the distance to.</param>
        /// <returns></returns>
        public float DistanceToTarget(Vector2 targetPos)
        {
            return (float)Math.Sqrt(Math.Pow((float)pos.X - targetPos.X, 2) + Math.Pow((float)pos.Y - targetPos.Y, 2));

        }
    }
}
