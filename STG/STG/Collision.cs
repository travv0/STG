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
    class Collision
    {
        const int ROWS = 5, COLUMNS = 5;
        static int CELLWIDTH = (MainGame.PlayingArea.Width + 100) / COLUMNS, CELLHEIGHT = (MainGame.PlayingArea.Height + 100) / ROWS;//constants for the size of the collision grid as well as the cell dimensions
        public List<GameObject>[,] collisionGrid = new List<GameObject>[ROWS, COLUMNS];//list of the gameobjects in each square of the collision grid
        List<GameObject> addList = new List<GameObject>(); //list to store objects until they can be added to the collision grid
        List<GameObject> removeList = new List<GameObject>(); //list to store objects until they can be removed from the collision grid

        /// <summary>
        /// Getter for collision grid cell width
        /// </summary>
        public static int getCellWidth()
        {
            return CELLWIDTH;
        }
        /// <summary>
        /// Getter for collision grid cell height
        /// </summary>
        public static int getCellHeight()
        {
            return CELLHEIGHT;
        }

        /// <summary>
        /// Constructor for collision object
        /// 
        /// </summary>
        public Collision()
        {
            for (int i = 0; i < ROWS; i++)
            {
                for (int k = 0; k < COLUMNS; k++)
                {
                    collisionGrid[i, k] = new List<GameObject>();
                }
            }
        }

        /// <summary>
        /// If any of the object is in the playing area it adds the object to all of the collision cells it could be in by using its vertices
        /// </summary>
        /// <param name="o"></param>
        public void addToCollisionGrid(GameObject o){
            if (o.insidePlayingArea(0))
            {
                if(MainGame.PlayingArea.Contains(new Point((int)o.getVertices()[0].X, (int)o.getVertices()[0].Y)))
                    this.addToGrid(o.getCollisionColumn()[0], o.getCollisionRow()[0], o);
                if (MainGame.PlayingArea.Contains(new Point((int)o.getVertices()[1].X, (int)o.getVertices()[1].Y)))
                    this.addToGrid(o.getCollisionColumn()[1], o.getCollisionRow()[1], o);
                if (MainGame.PlayingArea.Contains(new Point((int)o.getVertices()[2].X, (int)o.getVertices()[2].Y)))
                    this.addToGrid(o.getCollisionColumn()[2], o.getCollisionRow()[2], o);
                if (MainGame.PlayingArea.Contains(new Point((int)o.getVertices()[3].X, (int)o.getVertices()[3].Y)))
                    this.addToGrid(o.getCollisionColumn()[3], o.getCollisionRow()[3], o);
            }
        }
        public void removeFromCollisionGrid(GameObject o){
            if (!this.collides(o.getVertices(), MainGame.PlayingArea))
            {
                for (int i = 0; i < ROWS; i++)
                {
                    for (int j = 0; j < COLUMNS; j++)
                    {
                        for (int k = 0; k < 4; k++)
                        {
                            if (collisionGrid[i, j].Contains(o) && (o.getCollisionColumn()[k] != j || o.getCollisionRow()[k] != i))
                                collisionGrid[i, j].Remove(o);
                        }
                    }
                }
            }
        }

        /// gets the list of all objects and checks if they are in the same collision grid square as each player then 
        /// returns a list of all the objects near it. These will then be looped through and checked for collision
        /// pass it the object you are checking if things are near to
        public HashSet<GameObject> getObjectsNearPlayer(GameObject o)
        {
            HashSet<GameObject> objectsThatCouldCollide = new HashSet<GameObject>();
            int[] column = o.getCollisionColumn();
            int[] row = o.getCollisionRow();
            for (int i = 0; i < 4; i++)
            {
                if(row[i] != -1 && column[i] != -1)//in case some of her verticies aren't on the collision grid
                    foreach (GameObject obj in collisionGrid[row[i], column[i]])
                        objectsThatCouldCollide.Add(obj);
            }
            return objectsThatCouldCollide;
        }

        /// <summary>
        /// Adds a game object to the collision grid(double array of lists of game objects)
        /// </summary>
        /// <param name="column"></param>
        /// <param name="row"></param>
        /// <param name="o"></param>
        public void addToGrid(int column, int row, GameObject o)
        {
            collisionGrid[row, column].Add(o);
        }

        /// <summary>
        /// Uses separating axis theorem to determine if there is a collision
        /// </summary>
        /// <param name="o1">verticies of an object</param>
        /// <param name="o2">main playing area</param>
        /// <returns></returns>
        public Boolean collides(List<Vector2> o1, Rectangle o2)
        {
            //finding axes
            Vector2 axis1, axis2, axis3, axis4;
            axis1.X = o1[1].X - o1[0].X;
            axis1.Y = o1[1].Y - o1[0].Y;
            axis2.X = o1[1].X - o1[3].X;
            axis2.Y = o1[1].Y - o1[3].Y;
            axis3.X = o2.X - o2.X;
            axis3.Y = o2.Y - (o2.Y + o2.Height);
            axis4.X = o2.X - (o2.X + o2.Width);
            axis4.Y = o2.Y - o2.Y;

            //checking if overlap on axis1
            float scalarUR = (axis1.X * 2) * ((o1[1].X * axis1.X + o1[1].Y * axis1.Y) / (o1[1].X * o1[1].X + o1[1].Y * o1[1].Y)) + (axis1.Y * 2) * ((o1[1].X * axis1.X + o1[1].Y * axis1.Y) / (o1[1].X * o1[1].X + o1[1].Y * o1[1].Y));
            float scalarUL = (axis1.X * 2) * ((o1[0].X * axis1.X + o1[0].Y * axis1.Y) / (o1[0].X * o1[0].X + o1[0].Y * o1[0].Y)) +(axis1.Y * 2) * ((o1[0].X * axis1.X + o1[0].Y * axis1.Y) / (o1[0].X * o1[0].X + o1[0].Y * o1[0].Y));
            float scalarBR = (axis1.X * 2) * ((o1[3].X * axis1.X + o1[3].Y * axis1.Y) / (o1[3].X * o1[3].X + o1[3].Y * o1[3].Y)) + (axis1.Y * 2) * ((o1[3].X * axis1.X + o1[3].Y * axis1.Y) / (o1[3].X * o1[3].X + o1[3].Y * o1[3].Y));
            float scalarBL = (axis1.X * 2) * ((o1[2].X * axis1.X + o1[2].Y * axis1.Y) / (o1[2].X * o1[2].X + o1[2].Y * o1[2].Y)) + (axis1.Y * 2) * ((o1[2].X * axis1.X + o1[2].Y * axis1.Y) / (o1[2].X * o1[2].X + o1[2].Y * o1[2].Y));
            float maxA, minA, maxB, minB;
            float[] scalars = {scalarUR, scalarUL, scalarBR, scalarBL};
            Array.Sort(scalars);
            minA = scalars[0];
            maxA = scalars[3];

            scalarUR = (axis1.X * 2) * (((o2.X + o2.Width) * axis1.X + o2.Y * axis1.Y) / ((o2.X + o2.Width) * (o2.X + o2.Width) + o2.Y * o2.Y)) + (axis1.Y * 2) * (((o2.X + o2.Width) * axis1.X + o2.Y * axis1.Y) / ((o2.X + o2.Width) * (o2.X + o2.Width) + o2.Y * o2.Y));
            scalarUL = (axis1.X * 2) * ((o2.X * axis1.X + o2.Y * axis1.Y) / (o2.X * o2.X + o2.Y * o2.Y)) + (axis1.Y * 2) * ((o2.X * axis1.X + o2.Y * axis1.Y) / (o2.X * o2.X + o2.Y * o2.Y));
            scalarBR = (axis1.X * 2) * (((o2.X + o2.Width) * axis1.X + o1[3].Y * axis1.Y) / ((o2.X + o2.Width) * (o2.X + o2.Width) + (o2.Y + o2.Height) * (o2.Y + o2.Height))) + (axis1.Y * 2) * (((o2.X + o2.Width) * axis1.X + (o2.Y + o2.Height) * axis1.Y) / ((o2.X + o2.Width) * (o2.X + o2.Width) + (o2.Y + o2.Height) * (o2.Y + o2.Height)));
            scalarBL = (axis1.X * 2) * ((o2.X * axis1.X + (o2.Y + o2.Height) * axis1.Y) / (o2.X * o2.X + (o2.Y + o2.Height) * (o2.Y + o2.Height))) + (axis1.Y * 2) * ((o2.X * axis1.X + (o2.Y + o2.Height) * axis1.Y) / (o2.X * o2.X + (o2.Y + o2.Height) * (o2.Y + o2.Height)));
            scalars = new float[]{scalarUR, scalarUL, scalarBR, scalarBL};
            Array.Sort(scalars);
            minB = scalars[0];
            maxB = scalars[3];
            if (!(minB <= minA && maxB >= minA))
                return false;
            //end checking for overlap on axis1;

            //checking if overlap on axis2
            scalarUR = (axis2.X * 2) * ((o1[1].X * axis2.X + o1[1].Y * axis2.Y) / (o1[1].X * o1[1].X + o1[1].Y * o1[1].Y)) + (axis2.Y * 2) * ((o1[1].X * axis2.X + o1[1].Y * axis2.Y) / (o1[1].X * o1[1].X + o1[1].Y * o1[1].Y));
            scalarUL = (axis2.X * 2) * ((o1[0].X * axis2.X + o1[0].Y * axis2.Y) / (o1[0].X * o1[0].X + o1[0].Y * o1[0].Y)) + (axis2.Y * 2) * ((o1[0].X * axis2.X + o1[0].Y * axis2.Y) / (o1[0].X * o1[0].X + o1[0].Y * o1[0].Y));
            scalarBR = (axis2.X * 2) * ((o1[3].X * axis2.X + o1[3].Y * axis2.Y) / (o1[3].X * o1[3].X + o1[3].Y * o1[3].Y)) + (axis2.Y * 2) * ((o1[3].X * axis2.X + o1[3].Y * axis2.Y) / (o1[3].X * o1[3].X + o1[3].Y * o1[3].Y));
            scalarBL = (axis2.X * 2) * ((o1[2].X * axis2.X + o1[2].Y * axis2.Y) / (o1[2].X * o1[2].X + o1[2].Y * o1[2].Y)) + (axis2.Y * 2) * ((o1[2].X * axis2.X + o1[2].Y * axis2.Y) / (o1[2].X * o1[2].X + o1[2].Y * o1[2].Y));
            scalars = new float[] { scalarUR, scalarUL, scalarBR, scalarBL };
            Array.Sort(scalars);
            minA = scalars[0];
            maxA = scalars[3];

            scalarUR = (axis2.X * 2) * (((o2.X + o2.Width) * axis2.X + o2.Y * axis2.Y) / ((o2.X + o2.Width) * (o2.X + o2.Width) + o2.Y * o2.Y)) + (axis2.Y * 2) * (((o2.X + o2.Width) * axis2.X + o2.Y * axis2.Y) / ((o2.X + o2.Width) * (o2.X + o2.Width) + o2.Y * o2.Y));
            scalarUL = (axis2.X * 2) * ((o2.X * axis2.X + o2.Y * axis2.Y) / (o2.X * o2.X + o2.Y * o2.Y)) + (axis2.Y * 2) * ((o2.X * axis2.X + o2.Y * axis2.Y) / (o2.X * o2.X + o2.Y * o2.Y));
            scalarBR = (axis2.X * 2) * (((o2.X + o2.Width) * axis2.X + o1[3].Y * axis2.Y) / ((o2.X + o2.Width) * (o2.X + o2.Width) + (o2.Y + o2.Height) * (o2.Y + o2.Height))) + (axis2.Y * 2) * (((o2.X + o2.Width) * axis2.X + (o2.Y + o2.Height) * axis2.Y) / ((o2.X + o2.Width) * (o2.X + o2.Width) + (o2.Y + o2.Height) * (o2.Y + o2.Height)));
            scalarBL = (axis2.X * 2) * ((o2.X * axis2.X + (o2.Y + o2.Height) * axis2.Y) / (o2.X * o2.X + (o2.Y + o2.Height) * (o2.Y + o2.Height))) + (axis2.Y * 2) * ((o2.X * axis2.X + (o2.Y + o2.Height) * axis2.Y) / (o2.X * o2.X + (o2.Y + o2.Height) * (o2.Y + o2.Height)));
            scalars = new float[] { scalarUR, scalarUL, scalarBR, scalarBL };
            Array.Sort(scalars);
            minB = scalars[0];
            maxB = scalars[3];
            if (!(minB <= minA && maxB >= minA))
                return false;
            //end checking for overlap on axis2;

            //checking if overlap on axis3
            scalarUR = (axis3.X * 2) * ((o1[1].X * axis3.X + o1[1].Y * axis3.Y) / (o1[1].X * o1[1].X + o1[1].Y * o1[1].Y)) + (axis3.Y * 2) * ((o1[1].X * axis3.X + o1[1].Y * axis3.Y) / (o1[1].X * o1[1].X + o1[1].Y * o1[1].Y));
            scalarUL = (axis3.X * 2) * ((o1[0].X * axis3.X + o1[0].Y * axis3.Y) / (o1[0].X * o1[0].X + o1[0].Y * o1[0].Y)) + (axis3.Y * 2) * ((o1[0].X * axis3.X + o1[0].Y * axis3.Y) / (o1[0].X * o1[0].X + o1[0].Y * o1[0].Y));
            scalarBR = (axis3.X * 2) * ((o1[3].X * axis3.X + o1[3].Y * axis3.Y) / (o1[3].X * o1[3].X + o1[3].Y * o1[3].Y)) + (axis3.Y * 2) * ((o1[3].X * axis3.X + o1[3].Y * axis3.Y) / (o1[3].X * o1[3].X + o1[3].Y * o1[3].Y));
            scalarBL = (axis3.X * 2) * ((o1[2].X * axis3.X + o1[2].Y * axis3.Y) / (o1[2].X * o1[2].X + o1[2].Y * o1[2].Y)) + (axis3.Y * 2) * ((o1[2].X * axis3.X + o1[2].Y * axis3.Y) / (o1[2].X * o1[2].X + o1[2].Y * o1[2].Y));
            scalars = new float[] { scalarUR, scalarUL, scalarBR, scalarBL };
            Array.Sort(scalars);
            minA = scalars[0];
            maxA = scalars[3];

            scalarUR = (axis3.X * 2) * (((o2.X + o2.Width) * axis3.X + o2.Y * axis3.Y) / ((o2.X + o2.Width) * (o2.X + o2.Width) + o2.Y * o2.Y)) + (axis3.Y * 2) * (((o2.X + o2.Width) * axis3.X + o2.Y * axis3.Y) / ((o2.X + o2.Width) * (o2.X + o2.Width) + o2.Y * o2.Y));
            scalarUL = (axis3.X * 2) * ((o2.X * axis3.X + o2.Y * axis3.Y) / (o2.X * o2.X + o2.Y * o2.Y)) + (axis3.Y * 2) * ((o2.X * axis3.X + o2.Y * axis3.Y) / (o2.X * o2.X + o2.Y * o2.Y));
            scalarBR = (axis3.X * 2) * (((o2.X + o2.Width) * axis3.X + o1[3].Y * axis3.Y) / ((o2.X + o2.Width) * (o2.X + o2.Width) + (o2.Y + o2.Height) * (o2.Y + o2.Height))) + (axis3.Y * 2) * (((o2.X + o2.Width) * axis3.X + (o2.Y + o2.Height) * axis3.Y) / ((o2.X + o2.Width) * (o2.X + o2.Width) + (o2.Y + o2.Height) * (o2.Y + o2.Height)));
            scalarBL = (axis3.X * 2) * ((o2.X * axis3.X + (o2.Y + o2.Height) * axis3.Y) / (o2.X * o2.X + (o2.Y + o2.Height) * (o2.Y + o2.Height))) + (axis3.Y * 2) * ((o2.X * axis3.X + (o2.Y + o2.Height) * axis3.Y) / (o2.X * o2.X + (o2.Y + o2.Height) * (o2.Y + o2.Height)));
            scalars = new float[] { scalarUR, scalarUL, scalarBR, scalarBL };
            Array.Sort(scalars);
            minB = scalars[0];
            maxB = scalars[3];
            if (!(minB <= minA && maxB >= minA))
                return false;
            //end checking for overlap on axis3;

            //checking if overlap on axis4
            scalarUR = (axis4.X * 2) * ((o1[1].X * axis4.X + o1[1].Y * axis4.Y) / (o1[1].X * o1[1].X + o1[1].Y * o1[1].Y)) + (axis4.Y * 2) * ((o1[1].X * axis4.X + o1[1].Y * axis4.Y) / (o1[1].X * o1[1].X + o1[1].Y * o1[1].Y));
            scalarUL = (axis4.X * 2) * ((o1[0].X * axis4.X + o1[0].Y * axis4.Y) / (o1[0].X * o1[0].X + o1[0].Y * o1[0].Y)) + (axis4.Y * 2) * ((o1[0].X * axis4.X + o1[0].Y * axis4.Y) / (o1[0].X * o1[0].X + o1[0].Y * o1[0].Y));
            scalarBR = (axis4.X * 2) * ((o1[3].X * axis4.X + o1[3].Y * axis4.Y) / (o1[3].X * o1[3].X + o1[3].Y * o1[3].Y)) + (axis4.Y * 2) * ((o1[3].X * axis4.X + o1[3].Y * axis4.Y) / (o1[3].X * o1[3].X + o1[3].Y * o1[3].Y));
            scalarBL = (axis4.X * 2) * ((o1[2].X * axis4.X + o1[2].Y * axis4.Y) / (o1[2].X * o1[2].X + o1[2].Y * o1[2].Y)) + (axis4.Y * 2) * ((o1[2].X * axis4.X + o1[2].Y * axis4.Y) / (o1[2].X * o1[2].X + o1[2].Y * o1[2].Y));
            scalars = new float[] { scalarUR, scalarUL, scalarBR, scalarBL };
            Array.Sort(scalars);
            minA = scalars[0];
            maxA = scalars[3];

            scalarUR = (axis4.X * 2) * (((o2.X + o2.Width) * axis4.X + o2.Y * axis4.Y) / ((o2.X + o2.Width) * (o2.X + o2.Width) + o2.Y * o2.Y)) + (axis4.Y * 2) * (((o2.X + o2.Width) * axis4.X + o2.Y * axis4.Y) / ((o2.X + o2.Width) * (o2.X + o2.Width) + o2.Y * o2.Y));
            scalarUL = (axis4.X * 2) * ((o2.X * axis4.X + o2.Y * axis4.Y) / (o2.X * o2.X + o2.Y * o2.Y)) + (axis4.Y * 2) * ((o2.X * axis4.X + o2.Y * axis4.Y) / (o2.X * o2.X + o2.Y * o2.Y));
            scalarBR = (axis4.X * 2) * (((o2.X + o2.Width) * axis4.X + o1[3].Y * axis4.Y) / ((o2.X + o2.Width) * (o2.X + o2.Width) + (o2.Y + o2.Height) * (o2.Y + o2.Height))) + (axis4.Y * 2) * (((o2.X + o2.Width) * axis4.X + (o2.Y + o2.Height) * axis4.Y) / ((o2.X + o2.Width) * (o2.X + o2.Width) + (o2.Y + o2.Height) * (o2.Y + o2.Height)));
            scalarBL = (axis4.X * 2) * ((o2.X * axis4.X + (o2.Y + o2.Height) * axis4.Y) / (o2.X * o2.X + (o2.Y + o2.Height) * (o2.Y + o2.Height))) + (axis4.Y * 2) * ((o2.X * axis4.X + (o2.Y + o2.Height) * axis4.Y) / (o2.X * o2.X + (o2.Y + o2.Height) * (o2.Y + o2.Height)));
            scalars = new float[] { scalarUR, scalarUL, scalarBR, scalarBL };
            Array.Sort(scalars);
            minB = scalars[0];
            maxB = scalars[3];
            if (!(minB <= minA && maxB >= minA))
                return false;
            //end checking for overlap on axis4;
            else
                return true;//if only one of the projected values do not overlap they do not collide
        }
        /// <summary>
        /// Uses separating axis theorem to determine if there is a collision
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public Boolean collides(List<Vector2> o1, List<Vector2> o2)
        {
            //finding axes
            Vector2 axis1, axis2, axis3, axis4;
            axis1.X = o1[1].X - o1[0].X;
            axis1.Y = o1[1].Y - o1[0].Y;
            axis2.X = o1[1].X - o1[3].X;
            axis2.Y = o1[1].Y - o1[3].Y;
            axis3.X = o2[0].X - o2[2].X;
            axis3.Y = o2[0].Y - o2[2].Y;
            axis4.X = o2[0].X - o2[1].X;
            axis4.Y = o2[0].Y - o2[1].Y;

            //checking if overlap on axis1
            //changed division to axis points squared instead of vertice points squared hopefully it will fix it
            float scalarUR = (axis1.X * 2) * ((o1[1].X * axis1.X + o1[1].Y * axis1.Y) / (axis1.X * axis1.X + axis1.Y * axis1.Y)) + (axis1.Y * 2) * ((o1[1].X * axis1.X + o1[1].Y * axis1.Y) / (axis1.X * axis1.X + axis1.Y * axis1.Y));
            float scalarUL = (axis1.X * 2) * ((o1[0].X * axis1.X + o1[0].Y * axis1.Y) / (axis1.X * axis1.X + axis1.Y * axis1.Y)) + (axis1.Y * 2) * ((o1[0].X * axis1.X + o1[0].Y * axis1.Y) / (axis1.X * axis1.X + axis1.Y * axis1.Y));
            float scalarBR = (axis1.X * 2) * ((o1[3].X * axis1.X + o1[3].Y * axis1.Y) / (axis1.X * axis1.X + axis1.Y * axis1.Y)) + (axis1.Y * 2) * ((o1[3].X * axis1.X + o1[3].Y * axis1.Y) / (axis1.X * axis1.X + axis1.Y * axis1.Y));
            float scalarBL = (axis1.X * 2) * ((o1[2].X * axis1.X + o1[2].Y * axis1.Y) / (axis1.X * axis1.X + axis1.Y * axis1.Y)) + (axis1.Y * 2) * ((o1[2].X * axis1.X + o1[2].Y * axis1.Y) / (axis1.X * axis1.X + axis1.Y * axis1.Y));
            float maxA, minA, maxB, minB;
            float[] scalars = { scalarUR, scalarUL, scalarBR, scalarBL };
            Array.Sort(scalars);
            minA = scalars[0];
            maxA = scalars[3];

            scalarUR = (axis1.X * 2) * ((o2[1].X * axis1.X + o2[1].Y * axis1.Y) / (axis1.X * axis1.X + axis1.Y * axis1.Y)) + (axis1.Y * 2) * ((o2[1].X * axis1.X + o2[1].Y * axis1.Y) / (axis1.X * axis1.X + axis1.Y * axis1.Y));
            scalarUL = (axis1.X * 2) * ((o2[0].X * axis1.X + o2[0].Y * axis1.Y) / (axis1.X * axis1.X + axis1.Y * axis1.Y)) + (axis1.Y * 2) * ((o2[0].X * axis1.X + o2[0].Y * axis1.Y) / (axis1.X * axis1.X + axis1.Y * axis1.Y));
            scalarBR = (axis1.X * 2) * ((o2[3].X * axis1.X + o2[3].Y * axis1.Y) / (axis1.X * axis1.X + axis1.Y * axis1.Y)) + (axis1.Y * 2) * ((o2[3].X * axis1.X + o2[3].Y * axis1.Y) / (axis1.X * axis1.X + axis1.Y * axis1.Y));
            scalarBL = (axis1.X * 2) * ((o2[2].X * axis1.X + o2[2].Y * axis1.Y) / (axis1.X * axis1.X + axis1.Y * axis1.Y)) + (axis1.Y * 2) * ((o2[2].X * axis1.X + o2[2].Y * axis1.Y) / (axis1.X * axis1.X + axis1.Y * axis1.Y));
            scalars = new float[] { scalarUR, scalarUL, scalarBR, scalarBL };
            Array.Sort(scalars);
            minB = scalars[0];
            maxB = scalars[3];
            if (!(minB <= maxA && maxB >= minA))
                return false;
            //end checking for overlap on axis1;

            //checking if overlap on axis2
            scalarUR = (axis2.X * 2) * ((o1[1].X * axis2.X + o1[1].Y * axis2.Y) / (axis2.X * axis2.X + axis2.Y * axis2.Y)) + (axis2.Y * 2) * ((o1[1].X * axis2.X + o1[1].Y * axis2.Y) / (axis2.X * axis2.X + axis2.Y * axis2.Y));
            scalarUL = (axis2.X * 2) * ((o1[0].X * axis2.X + o1[0].Y * axis2.Y) / (axis2.X * axis2.X + axis2.Y * axis2.Y)) + (axis2.Y * 2) * ((o1[0].X * axis2.X + o1[0].Y * axis2.Y) / (axis2.X * axis2.X + axis2.Y * axis2.Y));
            scalarBR = (axis2.X * 2) * ((o1[3].X * axis2.X + o1[3].Y * axis2.Y) / (axis2.X * axis2.X + axis2.Y * axis2.Y)) + (axis2.Y * 2) * ((o1[3].X * axis2.X + o1[3].Y * axis2.Y) / (axis2.X * axis2.X + axis2.Y * axis2.Y));
            scalarBL = (axis2.X * 2) * ((o1[2].X * axis2.X + o1[2].Y * axis2.Y) / (axis2.X * axis2.X + axis2.Y * axis2.Y)) + (axis2.Y * 2) * ((o1[2].X * axis2.X + o1[2].Y * axis2.Y) / (axis2.X * axis2.X + axis2.Y * axis2.Y));
            scalars = new float[] { scalarUR, scalarUL, scalarBR, scalarBL };
            Array.Sort(scalars);
            minA = scalars[0];
            maxA = scalars[3];

            scalarUR = (axis2.X * 2) * ((o2[1].X * axis2.X + o2[1].Y * axis2.Y) / (axis2.X * axis2.X + axis2.Y * axis2.Y)) + (axis2.Y * 2) * ((o2[1].X * axis2.X + o2[1].Y * axis2.Y) / (axis2.X * axis2.X + axis2.Y * axis2.Y));
            scalarUL = (axis2.X * 2) * ((o2[0].X * axis2.X + o2[0].Y * axis2.Y) / (axis2.X * axis2.X + axis2.Y * axis2.Y)) + (axis2.Y * 2) * ((o2[0].X * axis2.X + o2[0].Y * axis2.Y) / (axis2.X * axis2.X + axis2.Y * axis2.Y));
            scalarBR = (axis2.X * 2) * ((o2[3].X * axis2.X + o2[3].Y * axis2.Y) / (axis2.X * axis2.X + axis2.Y * axis2.Y)) + (axis2.Y * 2) * ((o2[3].X * axis2.X + o2[3].Y * axis2.Y) / (axis2.X * axis2.X + axis2.Y * axis2.Y));
            scalarBL = (axis2.X * 2) * ((o2[2].X * axis2.X + o2[2].Y * axis2.Y) / (axis2.X * axis2.X + axis2.Y * axis2.Y)) + (axis2.Y * 2) * ((o2[2].X * axis2.X + o2[2].Y * axis2.Y) / (axis2.X * axis2.X + axis2.Y * axis2.Y));
            scalars = new float[] { scalarUR, scalarUL, scalarBR, scalarBL };
            Array.Sort(scalars);
            minB = scalars[0];
            maxB = scalars[3];
            if (!(minB <= maxA && maxB >= minA))
                return false;
            //end checking for overlap on axis2;

            //checking if overlap on axis3
            scalarUR = (axis3.X * 2) * ((o1[1].X * axis3.X + o1[1].Y * axis3.Y) / (axis3.X * axis3.X + axis3.Y * axis3.Y)) + (axis3.Y * 2) * ((o1[1].X * axis3.X + o1[1].Y * axis3.Y) / (axis3.X * axis3.X + axis3.Y * axis3.Y));
            scalarUL = (axis3.X * 2) * ((o1[0].X * axis3.X + o1[0].Y * axis3.Y) / (axis3.X * axis3.X + axis3.Y * axis3.Y)) + (axis3.Y * 2) * ((o1[0].X * axis3.X + o1[0].Y * axis3.Y) / (axis3.X * axis3.X + axis3.Y * axis3.Y));
            scalarBR = (axis3.X * 2) * ((o1[3].X * axis3.X + o1[3].Y * axis3.Y) / (axis3.X * axis3.X + axis3.Y * axis3.Y)) + (axis3.Y * 2) * ((o1[3].X * axis3.X + o1[3].Y * axis3.Y) / (axis3.X * axis3.X + axis3.Y * axis3.Y));
            scalarBL = (axis3.X * 2) * ((o1[2].X * axis3.X + o1[2].Y * axis3.Y) / (axis3.X * axis3.X + axis3.Y * axis3.Y)) + (axis3.Y * 2) * ((o1[2].X * axis3.X + o1[2].Y * axis3.Y) / (axis3.X * axis3.X + axis3.Y * axis3.Y));
            scalars = new float[] { scalarUR, scalarUL, scalarBR, scalarBL };
            Array.Sort(scalars);
            minA = scalars[0];
            maxA = scalars[3];

            scalarUR = (axis3.X * 2) * ((o2[1].X * axis3.X + o2[1].Y * axis3.Y) / (axis3.X * axis3.X + axis3.Y * axis3.Y)) + (axis3.Y * 2) * ((o2[1].X * axis3.X + o2[1].Y * axis3.Y) / (axis3.X * axis3.X + axis3.Y * axis3.Y));
            scalarUL = (axis3.X * 2) * ((o2[0].X * axis3.X + o2[0].Y * axis3.Y) / (axis3.X * axis3.X + axis3.Y * axis3.Y)) + (axis3.Y * 2) * ((o2[0].X * axis3.X + o2[0].Y * axis3.Y) / (axis3.X * axis3.X + axis3.Y * axis3.Y));
            scalarBR = (axis3.X * 2) * ((o2[3].X * axis3.X + o2[3].Y * axis3.Y) / (axis3.X * axis3.X + axis3.Y * axis3.Y)) + (axis3.Y * 2) * ((o2[3].X * axis3.X + o2[3].Y * axis3.Y) / (axis3.X * axis3.X + axis3.Y * axis3.Y));
            scalarBL = (axis3.X * 2) * ((o2[2].X * axis3.X + o2[2].Y * axis3.Y) / (axis3.X * axis3.X + axis3.Y * axis3.Y)) + (axis3.Y * 2) * ((o2[2].X * axis3.X + o2[2].Y * axis3.Y) / (axis3.X * axis3.X + axis3.Y * axis3.Y));
            scalars = new float[] { scalarUR, scalarUL, scalarBR, scalarBL };
            Array.Sort(scalars);
            minB = scalars[0];
            maxB = scalars[3];
            if (!(minB <= maxA && maxB >= minA))
                return false;
            //end checking for overlap on axis3;

            //checking if overlap on axis4
            scalarUR = (axis4.X * 2) * ((o1[1].X * axis4.X + o1[1].Y * axis4.Y) / (axis4.X * axis4.X + axis4.Y * axis4.Y)) + (axis4.Y * 2) * ((o1[1].X * axis4.X + o1[1].Y * axis4.Y) / (axis4.X * axis4.X + axis4.Y * axis4.Y));
            scalarUL = (axis4.X * 2) * ((o1[0].X * axis4.X + o1[0].Y * axis4.Y) / (axis4.X * axis4.X + axis4.Y * axis4.Y)) + (axis4.Y * 2) * ((o1[0].X * axis4.X + o1[0].Y * axis4.Y) / (axis4.X * axis4.X + axis4.Y * axis4.Y));
            scalarBR = (axis4.X * 2) * ((o1[3].X * axis4.X + o1[3].Y * axis4.Y) / (axis4.X * axis4.X + axis4.Y * axis4.Y)) + (axis4.Y * 2) * ((o1[3].X * axis4.X + o1[3].Y * axis4.Y) / (axis4.X * axis4.X + axis4.Y * axis4.Y));
            scalarBL = (axis4.X * 2) * ((o1[2].X * axis4.X + o1[2].Y * axis4.Y) / (axis4.X * axis4.X + axis4.Y * axis4.Y)) + (axis4.Y * 2) * ((o1[2].X * axis4.X + o1[2].Y * axis4.Y) / (axis4.X * axis4.X + axis4.Y * axis4.Y));
            scalars = new float[] { scalarUR, scalarUL, scalarBR, scalarBL };
            Array.Sort(scalars);
            minA = scalars[0];
            maxA = scalars[3];

            scalarUR = (axis4.X * 2) * ((o2[1].X * axis4.X + o2[1].Y * axis4.Y) / (axis4.X * axis4.X + axis4.Y * axis4.Y)) + (axis4.Y * 2) * ((o2[1].X * axis4.X + o2[1].Y * axis4.Y) / (axis4.X * axis4.X + axis4.Y * axis4.Y));
            scalarUL = (axis4.X * 2) * ((o2[0].X * axis4.X + o2[0].Y * axis4.Y) / (axis4.X * axis4.X + axis4.Y * axis4.Y)) + (axis4.Y * 2) * ((o2[0].X * axis4.X + o2[0].Y * axis4.Y) / (axis4.X * axis4.X + axis4.Y * axis4.Y));
            scalarBR = (axis4.X * 2) * ((o2[3].X * axis4.X + o2[3].Y * axis4.Y) / (axis4.X * axis4.X + axis4.Y * axis4.Y)) + (axis4.Y * 2) * ((o2[3].X * axis4.X + o2[3].Y * axis4.Y) / (axis4.X * axis4.X + axis4.Y * axis4.Y));
            scalarBL = (axis4.X * 2) * ((o2[2].X * axis4.X + o2[2].Y * axis4.Y) / (axis4.X * axis4.X + axis4.Y * axis4.Y)) + (axis4.Y * 2) * ((o2[2].X * axis4.X + o2[2].Y * axis4.Y) / (axis4.X * axis4.X + axis4.Y * axis4.Y));
            scalars = new float[] { scalarUR, scalarUL, scalarBR, scalarBL };
            Array.Sort(scalars);
            minB = scalars[0];
            maxB = scalars[3];
            if (!(minB <= maxA && maxB >= minA))
                return false;
            //end checking for overlap on axis4;
            else
                return true;//it only takes one axis to not have an overlap to say an object does not collide
        }
    }
}