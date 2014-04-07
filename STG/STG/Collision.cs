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
    //BE SURE TO FIX SO THAT IT WORKS IN MULTIPLE CELLS
    class Collision
    {
        const int ROWS = 5, COLUMNS = 5;
        static int CELLWIDTH = MainGame.PlayingArea.Width / COLUMNS, CELLHEIGHT = MainGame.PlayingArea.Height / ROWS;//constants for the size of the collision grid as well as the cell dimensions
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
        /// Removes game object from collision grid
        /// </summary>
        /// <param name="column"></param>
        /// <param name="row"></param>
        /// <param name="o"></param>
        public void removeFromGrid(int column, int row, GameObject o)
        {
            collisionGrid[row, column].Remove(o);
        }

    }
}