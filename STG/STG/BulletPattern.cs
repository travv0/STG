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
    /// The parent class for bullet patterns, meant to be inherited by different pattern classes.
    /// </summary>
    public class BulletPattern:GameObject
    {
        /// <summary>
        /// The object that created the bullet pattern.
        /// </summary>
        protected GameObject parent;

        /// <summary>
        /// The time that the bullet pattern has been in existance.
        /// </summary>
        protected int time;

        /// <summary>
        /// The maximum time the bullet pattern should exist.
        /// </summary>
        protected int maxTime = Int32.MaxValue;

        /// <summary>
        /// The number of times the bullet pattern has cycled through.
        /// </summary>
        protected int cycle = 0;

        /// <summary>
        /// Initializes a new bullet pattern.
        /// </summary>
        public BulletPattern() { }

        /// <summary>
        /// Updates the bullet pattern.
        /// </summary>
        public override void Update()
        {
            this.pos = parent.Position;

            time++;
            if (time == maxTime)
            {
                time = 0;
                cycle++;
            }
            

            base.Update();
        }
    }
}
