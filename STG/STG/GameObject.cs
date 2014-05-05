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
        /// position shifted for collision grid to work with a play area moved away from the origin
        /// </summary>
        protected Vector2 colPos;

        /// <summary>
        /// The bounding box of the sprite.  This is the rectangle that the sprite will be drawn in.
        /// </summary>
        protected Rectangle boundingBox;
        protected Vector2 hitboxSize;
        protected int[] collisionColumns = new int[4], collisionRows = new int[4];
        protected char objType;
        /// <summary>
        /// The sprite for this game object.
        /// </summary>
        protected Sprite sprite;

        protected float sWidth, sHeight;
        /// <summary>
        /// The angle that the sprite will be drawn at.
        /// </summary>
        protected float rotation;

        /// <summary>
        /// The direction the object needs to turn
        /// </summary>
        protected enum Direction { clockwise, counterclockwise };

        public Color color = Color.White;

        protected float angle;

        /// Vertice positions of all corners based on collision position and rotation of the object as calculated in calculateVertices
        /// 
        /// </summary>
        protected Vector2 tlVertex, trVertex, blVertex, brVertex; 

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
            rotation = 0;
            colPos.X = Position.X - 20;
            colPos.Y = Position.Y - 20;
        }

        /// <summary>
        /// Updates the object's member variables.
        /// </summary>
        virtual public void Update()
        {
            //update the boundingBox's coords to move with the object
            boundingBox.X = (int)pos.X;
            boundingBox.Y = (int)pos.Y;
            colPos.X = Position.X - 20;
            colPos.Y = Position.Y - 20;
            calculateVertices();
            calculateCollisionGridCell();
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

                sprite.Draw(spriteBatch, boundingBox, color, rotation, new Vector2((float)sprite.Width / 2, (float)sprite.Height / 2), 0, 1 - (pos.Y / MainGame.WindowHeight));
                for (int i = 0; i < 4; i++)
                {
                    MainGame.SpriteDict["hitbox"].Draw(spriteBatch, new Rectangle((int)this.getVertices()[i].X + 20, (int)this.getVertices()[i].Y + 20, 2, 2), Color.White);
                }
            }

            spriteBatch.End();
        }

        /// <summary>
        /// Returns the position of the object.
        /// </summary>
        public Vector2 Position { get { return pos; } }

        /// <summary>
        /// Returns the collision position of the object.
        /// </summary>
        public Vector2 collisionPosition { get { return colPos; } }

        /// <summary>
        /// Returns object type
        /// used to detect if an object should be deleted or not drawn when it is outside of the playing area
        /// For example options should not be deleted only disabled and not drawn temporarily but bullets should be removed and cleaned up
        /// 'O' for options, 'C' for collectibles, 'B' for bullets, 'P' for players 
        /// </summary>
        public char objectType { get { return objType; } }

        /// <summary>
        /// Returns sprite of object
        /// </summary>
        public Sprite getSprite { get { return sprite; } }

        /// <summary>
        /// puts all vertices of an object into a list and returns it
        /// will be used for collision purposes
        /// </summary>
        /// <returns></returns>
        public List<Vector2> getVertices()
        {
            calculateVertices();
            List<Vector2> vertices = new List<Vector2>();
            vertices.Add(tlVertex);
            vertices.Add(trVertex);
            vertices.Add(blVertex);
            vertices.Add(brVertex);
            return vertices;
        }

        /// <summary>
        /// Returns the collision column for collision grid.
        /// </summary>
        public int[] getCollisionColumn()
        {
            return collisionColumns;
        }

        /// <summary>
        /// Returns the collision row for collision grid.
        /// </summary>
        public int[] getCollisionRow()
        {
            return collisionRows;
        }

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
        /// <summary>
        /// Calculates where in the collision grid the cell is currently
        /// collision positions are shifted 20 pixels from actual position to accomadate for the play area being moved away from the origin
        /// </summary>
        private void calculateCollisionGridCell()
        {
            collisionColumns[0] = (int)Math.Floor(tlVertex.X / Collision.getCellWidth());
            collisionRows[0] = (int)Math.Floor(tlVertex.Y / Collision.getCellHeight());
            collisionColumns[1] = (int)Math.Floor(trVertex.X / Collision.getCellWidth());
            collisionRows[1] = (int)Math.Floor(trVertex.Y / Collision.getCellHeight());
            collisionColumns[2] = (int)Math.Floor(blVertex.X / Collision.getCellWidth());
            collisionRows[2] = (int)Math.Floor(blVertex.Y / Collision.getCellHeight());
            collisionColumns[3] = (int)Math.Floor(brVertex.X / Collision.getCellWidth());
            collisionRows[3] = (int)Math.Floor(brVertex.Y / Collision.getCellHeight());
        }
        #region Gamelogic
        private void calculateVertices()
        {
            if (sprite != null)
            {
                if (objectType == 'P')
                {
                    hitboxSize.X = 2;
                    hitboxSize.Y = 2;
                }
                else
                {
                    hitboxSize.X = boundingBox.Width / 4;
                    hitboxSize.Y = boundingBox.Height / 4;
                }
                tlVertex.X = colPos.X + ((float)(-hitboxSize.X) * (float)Math.Cos(rotation) - (float)(-hitboxSize.Y) * (float)Math.Sin(rotation));
                tlVertex.Y = colPos.Y + ((float)(-hitboxSize.X) * (float)Math.Sin(rotation) + (float)(-hitboxSize.Y) * (float)Math.Cos(rotation));
                trVertex.X = colPos.X + ((float)(hitboxSize.X) * (float)Math.Cos(rotation) - (float)(-hitboxSize.Y) * (float)Math.Sin(rotation));
                trVertex.Y = colPos.Y + ((float)(hitboxSize.X) * (float)Math.Sin(rotation) + (float)(-hitboxSize.Y) * (float)Math.Cos(rotation));
                blVertex.X = colPos.X + ((float)(-hitboxSize.X) * (float)Math.Cos(rotation) - (float)(hitboxSize.Y) * (float)Math.Sin(rotation));
                blVertex.Y = colPos.Y + ((float)(-hitboxSize.X) * (float)Math.Sin(rotation) + (float)(hitboxSize.Y) * (float)Math.Cos(rotation));
                brVertex.X = colPos.X + ((float)(hitboxSize.X) * (float)Math.Cos(rotation) - (float)(hitboxSize.Y) * (float)Math.Sin(rotation));
                brVertex.Y = colPos.Y + ((float)(hitboxSize.X) * (float)Math.Sin(rotation) + (float)(hitboxSize.Y) * (float)Math.Cos(rotation));
            }
        }
        /// <summary>
        /// checks if the object is inside the playing area at all
        /// remove buffer is how far outside (int pixels) the playing area you want to check if the object is in
        /// </summary>
        /// <returns></returns>
        public bool insidePlayingArea(int removeBuffer)
        {
            return (this.tlVertex.X > MainGame.PlayingArea.X - removeBuffer && this.tlVertex.Y > MainGame.PlayingArea.Y - removeBuffer && this.brVertex.X < MainGame.PlayingArea.X + MainGame.PlayingArea.Width + removeBuffer && this.brVertex.Y < MainGame.PlayingArea.Y + MainGame.PlayingArea.Height + removeBuffer);
        }

        protected Direction ClosestDirection(GameObject obj)
        {
            float tempAtT = NormalizeAngle(NormalizeAngle(AngleToTarget(obj.Position)) - NormalizeAngle(angle));
            if (tempAtT > 180)
                return Direction.counterclockwise;
            else
                return Direction.clockwise;
        }

        protected float AngleDifference(GameObject obj)
        {
            if (AngleToTarget(obj.Position) - angle > 180)
                return (360 - (AngleToTarget(obj.Position) - angle));
            else
                return Math.Abs(AngleToTarget(obj.Position) - angle);
        }

        protected float NormalizeAngle(float angle)
        {
            while (angle < 0 || angle > 360)
            {
                if (angle < 0)
                    angle += 360;
                if (angle > 360)
                    angle -= 360;
            }

            return angle;
        }
        #endregion
    }
}
