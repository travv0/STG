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
    public class GameObjectManager
    {
        List<GameObject> objectList = new List<GameObject>(); //list of all objects in the game
        List<GameObject> addList = new List<GameObject>(); //list to store objects until they can be added to objectList
        List<GameObject> removeList = new List<GameObject>(); //list to store objects until they can be removed from objectList

        public GameObjectManager() { }

        public void Add(GameObject o)
        {
            addList.Add(o);
        }

        public void Remove(GameObject o)
        {
            removeList.Add(o);
        }

        public void Update()
        {
            foreach (GameObject o in objectList)
                o.Update();
            foreach (GameObject o in addList) //now that we're done looping through objectList, we can add new objects to it
                objectList.Add(o);
            addList.Clear(); //clears the temp list
            foreach (GameObject o in removeList) //now that we're done looping through objectList, we can remove objects from it
                objectList.Remove(o);
            removeList.Clear();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            objectList.Sort(delegate(GameObject obj1, GameObject obj2) //sorts objectList by Y value
            {
                return obj1.Position.Y.CompareTo(obj2.Position.Y);
            });
            foreach (GameObject o in objectList)
                o.Draw(spriteBatch);
        }

        public GameObject Find(Type type)
        {
            return objectList.Find(delegate(GameObject obj)
            {
                return (type == obj.GetType());
            });
        }
    }
}
