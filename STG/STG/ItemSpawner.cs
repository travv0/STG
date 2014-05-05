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
    /// Class that spawns items at random intervals on top of the screen
    /// </summary>
    class ItemSpawner:GameObject
    {
        protected int time;
        protected int smallIntervalTime;
        protected int largeIntervalTime;
        protected int maxTime = Int32.MaxValue;
        //protected int position;
        static private Random smallRand = new Random();
        static private Random largeRand = new Random();
        static private Random position = new Random();
        protected int smallInterval = smallRand.Next(1200, 3600);
        protected int largeInterval = largeRand.Next(3600, 8000);
        private bool smallSpawn = false;
        private bool largeSpawn = false;

        public ItemSpawner() { }



        public override void Update()
        {
            time++;
            smallIntervalTime++;
            largeIntervalTime++;


            if (smallIntervalTime == smallInterval)
            {
                MainGame.ObjectManager.Add(new SmallPowerItem(new Vector2(position.Next(MainGame.PlayingArea.Width - (MainGame.SpriteDict["SmallItem"].Width / 2)) + (MainGame.SpriteDict["SmallItem"].Width / 2), 50), 0));
                smallSpawn = true;
            }
            if (largeIntervalTime == largeInterval)
            {
                MainGame.ObjectManager.Add(new LargePowerItem(new Vector2(position.Next(MainGame.PlayingArea.Width - (MainGame.SpriteDict["LargeItem"].Width / 2)) + (MainGame.SpriteDict["LargeItem"].Width / 2), 50), 0));
                largeSpawn = true;
            }

            if (time == maxTime)
            {
                time = 0;
            }
            if (smallSpawn == true)
            {
                smallSpawn = false;
                smallInterval = smallRand.Next(1200, 3600);
                smallIntervalTime = 0;
            }
            if (largeSpawn == true)
            {
                largeSpawn = false;
                largeInterval = largeRand.Next(3600, 8000);
                largeIntervalTime = 0;
            }

            base.Update();
        }
    }
}
