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
        List<GameObject> objectList = new List<GameObject>(); //list of all objects in the game
        List<GameObject> addList = new List<GameObject>(); //list to store objects until they can be added to objectList
        List<GameObject> removeList = new List<GameObject>(); //list to store objects until they can be removed from objectList
        Collision collisionGrid = new Collision(); //makes the collision instance
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
            foreach (GameObject o in objectList)
                o.Update();
            foreach (GameObject o in addList) //now that we're done looping through objectList, we can add new objects to it
                objectList.Add(o);
            addList.Clear(); //clears the temp list

            //Updates for collision grid
            foreach (GameObject o in objectList)
            {
                if (o.getSprite != null)
                {
                    if (!(o.Position.X + o.getSprite.Width / 2 > (MainGame.PlayingArea.X + MainGame.PlayingArea.Width) ||
                    o.Position.X - o.getSprite.Width / 2 < MainGame.PlayingArea.X || o.Position.Y + o.getSprite.Height / 2 > MainGame.PlayingArea.Y + MainGame.PlayingArea.Height ||
                    o.Position.Y - o.getSprite.Height / 2 < MainGame.PlayingArea.Y))
                    {
                        collisionGrid.addToGrid(o.getCollisionColumn(), o.getCollisionRow(), o);
                    }
                    if (o.objectType == 'C' && (o.Position.X + o.getSprite.Width / 2 > (MainGame.PlayingArea.X + MainGame.PlayingArea.Width) ||
                    o.Position.X - o.getSprite.Width / 2 < MainGame.PlayingArea.X || o.Position.Y + o.getSprite.Height / 2 > MainGame.PlayingArea.Y + MainGame.PlayingArea.Height ||
                    o.Position.Y - o.getSprite.Height / 2 < MainGame.PlayingArea.Y))
                    {
                        collisionGrid.removeFromGrid(o.getCollisionColumn(), o.getCollisionRow(), o);
                        Remove(o);
                    }
                }
            }
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
            foreach (GameObject o in objectList)
                o.Draw(spriteBatch);
        }

        /// <summary>
        /// Find and return an object of a given type in the object manager.
        /// </summary>
        /// <param name="type">Type of object to find.</param>
        /// <returns>An object of a given type that is in the object manager.</returns>
        public GameObject Find(Type type)
        {
            return objectList.Find(delegate(GameObject obj)
            {
                return (type == obj.GetType());
            });
        }

        /// <summary>
        /// Returns the number of objects in the object manager.
        /// </summary>
        public int Count { get { return objectList.Count; } }
    }
}
