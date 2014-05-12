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
    /// Manages all GameObjects to simplify updating and drawing them.
    /// </summary>
    public class GameObjectManager
    {
        public int dead = 0;
        int z = 0;
        public bool canDraw = true;
        public List<GameObject> objectList = new List<GameObject>(); //list of all objects in the game
        List<GameObject> addList = new List<GameObject>(); //list to store objects until they can be added to objectList
        List<GameObject> removeList = new List<GameObject>(); //list to store objects until they can be removed from objectList
        static Collision collisionGrid = new Collision(); //makes the collision instance
        HashSet<GameObject> objNearPlayerOne, objNearPlayerTwo;
        GameObject playerOne, playerTwo;
        /// <summary>
        /// Initializes a new GameObjectManager with an empty list of GameObjects.
        /// </summary>
        public GameObjectManager() { }

        /// <summary>
        /// Adds a new object to the object manager.
        /// </summary>
        /// <param name="o">The object to add to the manager.</param>
        public void Add(GameObject o)
        {
            addList.Add(o);
        }

        /// <summary>
        /// Removes an object from the object manager.
        /// </summary>
        /// <param name="o">The object to remove from the manager.</param>
        public void Remove(GameObject o)
        {
            removeList.Add(o);
        }

        /// <summary>
        /// Updates all objects in the object manager.
        /// </summary>
        public void Update()
        {
            #region hitboxtesting
            ///used for testing if hitboxes are sufficient
            if (dead != 0)
                dead++;
            if (dead == 10)
                canDraw = true;
            ///
            #endregion

            foreach (GameObject o in objectList)
            {
                z++;
                if(z % 10 == 0)
                    o.color = Color.White;
                o.Update();
                if (o.getSprite != null)//Updates for collision grid
                {   
                    
                    collisionGrid.removeFromCollisionGrid(o);
                    if (o.insidePlayingArea(0))
                    {
                        collisionGrid.addToCollisionGrid(o);
                    }
                }
            }
            foreach (GameObject o in addList) //now that we're done looping through objectList, we can add new objects to it
                objectList.Add(o);
            /*playerOne = objectList[0];
            playerTwo = objectList[1];*/
            addList.Clear(); //clears the temp list


            /*if (objNearPlayerOne != null && objNearPlayerTwo != null)
            {
                foreach (GameObject o in objNearPlayerOne)
                {
                    if (o.objectType == 'B')
                    {
                        if (collisionGrid.collides(o.getVertices(), playerOne.getVertices()))
                        {
                            //collisionGrid.removeFromCollisionGrid(objectList[0]);
                            //removeList.Add(objectList[0]);
                            dead = 1;
                            canDraw = false;
                        }
                    }
                }
                foreach (GameObject o in objNearPlayerOne)
                {
                    if (o.objectType == 'C')
                    {
                        if (collisionGrid.collides(o.getVertices(), playerOne.getVertices()))
                        {
                            ////

                            ///Add power to player

                            ////
                            collisionGrid.removeFromCollisionGrid(o);
                            removeList.Add(o);
                        }
                    }
                }
            }*/
            foreach (GameObject o in removeList) //now that we're done looping through objectList, we can remove objects from it
                objectList.Remove(o);
            removeList.Clear();
        }

        /// <summary>
        /// Draws all objects in the object manager.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to use for drawing.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            /*objectList.Sort(delegate(GameObject obj1, GameObject obj2) //sorts objectList by Y value
            {
                return obj1.Position.Y.CompareTo(obj2.Position.Y);
            });*/
            if (canDraw == true)
                foreach (GameObject o in objectList)
                    o.Draw(spriteBatch);
            else
                for (int i = 1; i < objectList.Count(); i++)
                {
                    objectList[i].Draw(spriteBatch);
                }
        }

        /// <summary>
        /// Find and return an object of a given type in the object manager.
        /// </summary>
        /// <param name="type">Type of object to find.</param>
        /// <returns>An object of a given type that is in the object manager.</returns>
        public GameObject Find(GameObject type)
        {
            return objectList.Find(delegate(GameObject obj)
            {
                return (type == obj);
            });
        }

        public void DeleteAll(char objType)
        {
            foreach (GameObject o in objectList)
            {
                if (o.objectType == objType)
                    this.Remove(o);
            }
        }

        public void moveAllBoxes(char objType)
        {
            foreach (GameObject o in objectList)
            {
                if(o.objectType == objType)
                    o.boundingBox = new Rectangle(0, 0, 0, 0);
            }
        }

        /// <summary>
        /// Returns the number of objects in the object manager.
        /// </summary>
        public int Count { get { return objectList.Count; } }

        public static Collision CollisionGrid { get { return collisionGrid; } }
    }
}
