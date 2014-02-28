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
        enum Direction { clockwise, counterclockwise };
        float vel, angle, drawAngle = 0; //velocity of bullet
        bool homing;
        bool spinning;
        GameObject parent;
        public enum Action {speed, curve, velocityX, velocityY};
        List<Tuple<Action, float, int>> actionList = new List<Tuple<Action, float, int>>(); //list of actions for bullet to perform during its lifetime. first item in tuple is the action and second is the amount to adjust
                                                                                            //third item is the time into the bullet's life to perform this action
        int timer = 0; //used to see when to change actions

        public Bullet(Sprite sprite, Vector2 pos, float vel, float angle, Player parent, List<Tuple<Action, float, int>> actionList, bool homing = false, bool spinning = false)
        {
            this.sprite = sprite;
            this.pos = pos;
            this.vel = vel;
            this.angle = angle;
            this.parent = parent;
            this.boundingBox = new Rectangle((int)pos.X, (int)pos.Y, sprite.Width, sprite.Height);
            this.hitbox = new Rectangle((int)(pos.X - Math.Ceiling(sprite.Width / 2.0)), (int)(pos.Y - Math.Ceiling(sprite.Height / 2.0)), sprite.Width, sprite.Height);
            this.actionList.InsertRange(0, actionList);
            this.homing = homing;
            this.spinning = spinning;
            this.actionList.Sort(delegate(Tuple<Action, float, int> action1, Tuple<Action, float, int> action2) //sorts actionList by time the action will be performed, that way you only have to check the first item in the list
            {
                return action1.Item3.CompareTo(action2.Item3);
            });
        }

        public override void Update()
        {
            angle = angle % 360;

            if (spinning == true)
                drawAngle = (drawAngle + .1f) % 360;
            
            float a = AngleDifference(Game1.player2);
            if (homing == true)
            {
                if (ClosestDirection(Game1.player2) == Direction.clockwise)
                    angle += AngleDifference(Game1.player2) / DistanceToTarget(Game1.player2.Position) * 100;
                if (ClosestDirection(Game1.player2) == Direction.counterclockwise)
                    angle -= AngleDifference(Game1.player2) / DistanceToTarget(Game1.player2.Position) * 100;
            }

            pos.X += (int)(vel * Math.Cos((double)angle * Math.PI / 180));
            pos.Y += (int)(vel * Math.Sin((double)angle * Math.PI / 180));

            if (boundingBox.X + boundingBox.Height < 0 || boundingBox.Y + boundingBox.Height < 0
                || boundingBox.X > Game1.windowWidth || boundingBox.Y > Game1.windowHeight)
            {
                Game1.objectManager.Remove(this);
                /*collisionGrid[(int)topLeft.X, (int)topLeft.Y].Remove(this);
                collisionGrid[(int)topRight.X, (int)topRight.Y].Remove(this);
                collisionGrid[(int)bottomLeft.X, (int)bottomLeft.Y].Remove(this);
                collisionGrid[(int)bottomRight.X, (int)bottomRight.Y].Remove(this);*/
            }
            
            while (actionList.Count > 0 && actionList[0].Item3 == timer)
            {
                switch (actionList[0].Item1)
                {
                    case Action.speed:
                        //vel = new Vector2(vel.X * actionList[0].Item2, vel.Y * actionList[0].Item2);
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

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            sprite.Update();
            sprite.Draw(spriteBatch, boundingBox, Color.White, angle * (float)Math.PI / 180 + drawAngle, new Vector2((float)sprite.Width / 2, (float)sprite.Height / 2), 0, 1 - (pos.Y / Game1.windowHeight));

            spriteBatch.End();
        }

        public GameObject Parent { get { return parent; } }

        private Direction ClosestDirection(GameObject obj)
        {
            if (((AngleToTarget(obj.Position) + 180) % 360 > angle && AngleToTarget(obj.Position) > angle && AngleToTarget(obj.Position) > 180) || AngleToTarget(obj.Position) < angle)
                return Direction.counterclockwise;
            else
                return Direction.clockwise;
        }

        private float AngleDifference(GameObject obj)
        {
            if (Math.Abs(AngleToTarget(obj.Position) - angle) > 180)
                return (360 - Math.Abs(AngleToTarget(obj.Position) - angle));
            else
                return Math.Abs(AngleToTarget(obj.Position) - angle);
        }
    }
}
