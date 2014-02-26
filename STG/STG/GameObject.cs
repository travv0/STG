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
        protected Texture2D sprite;
        protected Rectangle hitbox; //used for collision detection

        const int gridWidth = 5, gridHeight = 3; //height and width of collision grid

        static protected HashSet<GameObject>[,] collisionGrid = new HashSet<GameObject>[gridWidth, gridHeight]; //grid for collisions; only detect collisions from objects close to you
        protected Vector2 topLeft = new Vector2(), topRight = new Vector2(), 
            bottomLeft = new Vector2(), bottomRight = new Vector2(),
            prevTopLeft = new Vector2(), prevTopRight = new Vector2(),
            prevBottomLeft = new Vector2(), prevBottomRight = new Vector2(); //all of these are for the collision grid and all x and y values range from 0-9. names should be self-explanitory
        
        public GameObject()
        {
            if (collisionGrid[0, 0] == null) //if collisionGrid hasn't been initialized
                for (int i = 0; i < gridWidth; i++)
                    for (int j = 0; j < gridHeight; j++)
                        collisionGrid[i, j] = new HashSet<GameObject>();

            collisionGrid[(int)topLeft.X, (int)topLeft.Y].Add(this);
            collisionGrid[(int)topRight.X, (int)topRight.Y].Add(this);
            collisionGrid[(int)bottomLeft.X, (int)bottomLeft.Y].Add(this);
            collisionGrid[(int)bottomRight.X, (int)bottomRight.Y].Add(this);
        }

        virtual public void Update()
        {
            //update the boundingBox's coords to move with the object
            boundingBox.X = (int)pos.X;
            boundingBox.Y = (int)pos.Y;
            //hitbox too
            hitbox.X = (int)(pos.X - Math.Ceiling(hitbox.Width / 2.0));
            hitbox.Y = (int)(pos.Y - Math.Ceiling(hitbox.Height / 2.0));

            topLeft.X = (int)(hitbox.X / Game1.windowWidth * gridWidth);
            topLeft.Y = (int)(hitbox.Y / Game1.windowHeight * gridHeight);
            topRight.X = (int)((hitbox.X + Game1.windowWidth) / Game1.windowWidth * gridWidth);
            topRight.Y = (int)(hitbox.Y / Game1.windowHeight * gridHeight);
            bottomLeft.X = (int)(hitbox.X / Game1.windowWidth * gridWidth);
            bottomLeft.Y = (int)((hitbox.Y + Game1.windowHeight) / Game1.windowHeight * gridHeight);
            bottomRight.X = (int)((hitbox.X + Game1.windowWidth) / Game1.windowWidth * gridWidth);
            bottomRight.Y = (int)((hitbox.Y + Game1.windowHeight) / Game1.windowHeight * gridHeight);

            //check if object should be moved in collision grid, and if so, move it
            if ((topLeft.X != prevTopLeft.X || topLeft.Y != prevTopLeft.Y) && 0 <= topLeft.X && topLeft.X < gridWidth && 0 <= topLeft.Y && topLeft.Y < gridHeight)
            {
                collisionGrid[(int)prevTopLeft.X, (int)prevTopLeft.Y].Remove(this);
                collisionGrid[(int)topLeft.X, (int)topLeft.Y].Add(this);
            }

            prevTopLeft.X = topLeft.X;
            prevTopLeft.Y = topLeft.Y;

            topRight.X = (int)(pos.X / Game1.windowWidth * gridWidth);
            topRight.Y = (int)(pos.Y / Game1.windowHeight * gridHeight);

            //check if object should be moved in collision grid, and if so, move it
            if ((topRight.X != prevTopRight.X || topRight.Y != prevTopRight.Y) && 0 <= topRight.X && topRight.X < gridWidth && 0 <= topRight.Y && topRight.Y < gridHeight)
            {
                collisionGrid[(int)prevTopRight.X, (int)prevTopRight.Y].Remove(this);
                collisionGrid[(int)topRight.X, (int)topRight.Y].Add(this);
            }

            prevTopRight.X = topRight.X;
            prevTopRight.Y = topRight.Y;

            bottomLeft.X = (int)(pos.X / Game1.windowWidth * gridWidth);
            bottomLeft.Y = (int)(pos.Y / Game1.windowHeight * gridHeight);

            //check if object should be moved in collision grid, and if so, move it
            if ((bottomLeft.X != prevBottomLeft.X || bottomLeft.Y != prevBottomLeft.Y) && 0 <= bottomLeft.X && bottomLeft.X < gridWidth && 0 <= bottomLeft.Y && bottomLeft.Y < gridHeight)
            {
                collisionGrid[(int)prevBottomLeft.X, (int)prevBottomLeft.Y].Remove(this);
                collisionGrid[(int)bottomLeft.X, (int)bottomLeft.Y].Add(this);
            }

            prevBottomLeft.X = bottomLeft.X;
            prevBottomLeft.Y = bottomLeft.Y;

            bottomRight.X = (int)(pos.X / Game1.windowWidth * gridWidth);
            bottomRight.Y = (int)(pos.Y / Game1.windowHeight * gridHeight);

            //check if object should be moved in collision grid, and if so, move it
            if ((bottomRight.X != prevBottomRight.X || bottomRight.Y != prevBottomRight.Y) && 0 <= bottomRight.X && bottomRight.X < gridWidth && 0 <= bottomRight.Y && bottomRight.Y < gridHeight)
            {
                collisionGrid[(int)prevBottomRight.X, (int)prevBottomRight.Y].Remove(this);
                collisionGrid[(int)bottomRight.X, (int)bottomRight.Y].Add(this);
            }

            prevBottomRight.X = bottomRight.X;
            prevBottomRight.Y = bottomRight.Y;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(sprite, boundingBox, null, Color.White, 0, new Vector2((float)sprite.Width / 2, (float)sprite.Height / 2), 0, 1 - (pos.Y / Game1.windowHeight));
            float test = pos.Y / Game1.windowHeight;

            spriteBatch.Draw(Game1.textureDict["hitbox"], hitbox, Color.White);

            spriteBatch.End();
        }

        public Vector2 Position { get { return pos; } }

        public Rectangle BoundingBox { get { return boundingBox; } }

        //returns a list of objects that this object is colliding with
        protected List<GameObject> CollidingWith()
        {
            List<GameObject> collidingObjs = new List<GameObject>();

            foreach (GameObject o in collisionGrid[(int)topLeft.X, (int)topLeft.Y])
            {
                if (hitbox.Intersects(o.hitbox))
                    collidingObjs.Add(o);
            }
            foreach (GameObject o in collisionGrid[(int)topRight.X, (int)topRight.Y])
            {
                if (hitbox.Intersects(o.hitbox))
                    collidingObjs.Add(o);
            }
            foreach (GameObject o in collisionGrid[(int)bottomRight.X, (int)bottomRight.Y])
            {
                if (hitbox.Intersects(o.hitbox))
                    collidingObjs.Add(o);
            }
            foreach (GameObject o in collisionGrid[(int)bottomLeft.X, (int)bottomLeft.Y])
            {
                if (hitbox.Intersects(o.hitbox))
                    collidingObjs.Add(o);
            }
            return collidingObjs;
        }
    }
}
