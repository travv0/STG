using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// A collectible item.
    /// </summary>
    public class CollectibleItem:GameObject
    {
        public float vel=0f, maxVel = 3f, powerLevel;
        /// <summary>
        /// Accessor for power level of item.
        /// </summary>
        public float PowerLevel { get { return powerLevel; } }
        /// <summary>
        /// Creates a new collectible item object.
        /// </summary>
        /// <param name="sprite">This is the item's sprite.</param>
        /// <param name="pos">This is the item's position.</param>
        /// <param name="vel">This is the item's velocity.</param>
        /// <param name="powerLevel">This is the item's power value.</param>
        public CollectibleItem(Sprite sprite, Vector2 pos, float vel, float powerLevel)
        {
            this.sprite = sprite;
            this.pos = pos;
            this.vel = vel;
            this.powerLevel = powerLevel;
            objType = 'C';
        }
        public CollectibleItem()
        {
            objType = 'C';            
        }
        /// <summary>
        /// Updates the Collectible Item.
        /// </summary>
        public override void Update()
        {
            if (vel == 0)
                vel += .3f;
            else if( vel < maxVel)
                vel += vel * .02f;

            this.pos.Y += vel;
            this.boundingBox = new Rectangle((int)pos.X, (int)pos.Y, sprite.Width, sprite.Height);

            if(boundingBox.Y > MainGame.WindowHeight + 100)
                MainGame.ObjectManager.Remove(this);

            base.Update();
        }

    }
}
