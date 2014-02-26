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
    public class Bullet:GameObject
    {
        Vector2 vel; //velocity of bullet
        GameObject parent;
        public enum Action {speed, curve, velocityX, velocityY};
        List<Tuple<Action, float, int>> actionList = new List<Tuple<Action, float, int>>(); //list of actions for bullet to perform during its lifetime. first item in tuple is the action and second is the amount to adjust
                                                                                            //third item is the time into the bullet's life to perform this action
        int timer = 0; //used to see when to change actions
        float curveX = 0;
        float curveXmom = 0; //momentum for the curve on the X axis
        float curveY = 0;
        float curveYmom = 0; //momentum for the curve on the Y axis

        public Bullet(Texture2D sprite, Vector2 pos, Vector2 vel, Player parent, List<Tuple<Action, float, int>> actionList)
        {
            this.sprite = sprite;
            this.pos = pos;
            this.vel = vel;
            this.parent = parent;
            this.boundingBox = new Rectangle((int)pos.X, (int)pos.Y, sprite.Width, sprite.Height);
            if (vel.Y < 0)
                this.hitbox = new Rectangle((int)(pos.X - Math.Ceiling(sprite.Width / 2.0)) + (int)vel.X / 2, (int)(pos.Y - Math.Ceiling(sprite.Height / 2.0)) + (int)vel.Y / 2, sprite.Width - (int)vel.X / 2, sprite.Height - (int)vel.Y / 2 - parent.Speed);
            if (vel.Y > 0)
                this.hitbox = new Rectangle((int)(pos.X - Math.Ceiling(sprite.Width / 2.0)) - (int)vel.X / 2, (int)(pos.Y - Math.Ceiling(sprite.Height / 2.0)) - (int)vel.Y / 2, sprite.Width + (int)vel.X / 2, sprite.Height + (int)vel.Y / 2 + parent.Speed);
            this.actionList.InsertRange(0, actionList);
            this.actionList.Sort(delegate(Tuple<Action, float, int> action1, Tuple<Action, float, int> action2) //sorts actionList by time the action will be performed, that way you only have to check the first item in the list
            {
                return action1.Item3.CompareTo(action2.Item3);
            });
        }

        public override void Update()
        {
            pos += vel; //moves the bullet, duh

            if (boundingBox.X + boundingBox.Height < 0 || boundingBox.Y + boundingBox.Height < 0
                || boundingBox.X > Game1.windowWidth || boundingBox.Y > Game1.windowHeight)
                Game1.objectManager.Remove(this);

            //ignore this
            curveX += curveXmom;
            pos.X += curveX;
            curveY += curveYmom;
            pos.Y += curveY;
            
            while (actionList.Count > 0 && actionList[0].Item3 == timer)
            {
                switch (actionList[0].Item1)
                {
                    case Action.speed:
                        vel = new Vector2(vel.X * actionList[0].Item2, vel.Y * actionList[0].Item2);
                        break;
                    case Action.curve:
                        break;
                    case Action.velocityX:
                        break;
                    case Action.velocityY:
                        break;
                }
                actionList.RemoveAt(0);
            }

            timer++;

            base.Update();
        }

        public GameObject Parent { get { return parent; } }
    }
}
