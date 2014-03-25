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
    /// A bullet.
    /// </summary>
    public class Bullet:GameObject
    {
        enum Direction { clockwise, counterclockwise };
        float vel, angle, curve, drawAngle = 0; //velocity of bullet
        int angleTime, curveTime, velTime;
        float angleChange, curveChange, velChange;
        bool homing;
        bool spinning;
        GameObject parent;

        /// <summary>
        /// An enum used for modifying the bullet's properties during its lifetime.
        /// </summary>
        public enum Action
        {
            /// <summary>
            /// Used to modify the speed of the bullet.
            /// </summary>
            speed,
            /// <summary>
            /// Used to modify the angle of the bullet.
            /// </summary>
            angle,
            /// <summary>
            /// Used to modify the curve of the bullet.
            /// </summary>
            curve
        };
        List<Tuple<Action, float, int, int, bool>> actionList = new List<Tuple<Action, float, int, int, bool>>(); //list of actions for bullet to perform during its lifetime. first item in tuple is the action and second is the amount to adjust
                                                                                            //third item is the time into the bullet's life to perform this action, fourth is how long to finish this action, fifth is whether the change is relative
        int timer = 0; //used to see when to change actions

        /// <summary>
        /// Creates a new bullet object.
        /// </summary>
        /// <param name="sprite">The bullet's sprite.</param>
        /// <param name="pos">The bullet's position.</param>
        /// <param name="vel">The bullet's velocity.</param>
        /// <param name="angle">The bullet's angle.</param>
        /// <param name="curve">The curve of the bullet.</param>
        /// <param name="parent">The object that created the bullet.</param>
        /// <param name="actionList">The list of actions for the bullet to do during its lifetime.</param>
        /// <param name="homing">Does the bullet home in on an object?</param>
        /// <param name="spinning">Should the bullet's sprite spin around?</param>
        public Bullet(Sprite sprite, Vector2 pos, float vel, float angle, float curve, GameObject parent, List<Tuple<Action, float, int, int, bool>> actionList, bool homing = false, bool spinning = false)
        {
            this.sprite = sprite;
            this.pos = pos;
            this.vel = vel;
            this.angle = angle;
            this.parent = parent;
            this.curve = curve;
            this.boundingBox = new Rectangle((int)pos.X, (int)pos.Y, sprite.Width, sprite.Height);
            if (actionList != null)
                this.actionList.InsertRange(0, actionList);
            this.homing = homing;
            this.spinning = spinning;
            this.actionList.Sort(delegate(Tuple<Action, float, int, int, bool> action1, Tuple<Action, float, int, int, bool> action2) //sorts actionList by time the action will be performed, that way you only have to check the first item in the list
            {
                return action1.Item3.CompareTo(action2.Item3);
            });

            rotation = angle * (float)Math.PI / 180 + drawAngle;
            objType = 'B';
        }

        /// <summary>
        /// Updates the bullet.
        /// </summary>
        public override void Update()
        {
            angle += curve;

            if (angleTime > 0)
            {
                angle += angleChange;
                angleTime--;
            }
            if (velTime > 0)
            {
                vel += velChange;
                velTime--;
            }
            if (curveTime > 0)
            {
                curve += curveChange;
                curveTime--;
            }

            angle = angle % 360;

            if (spinning == true)
                drawAngle = (drawAngle + .1f) % 360;
            
            if (homing == true)
            {
                if (ClosestDirection(MainGame.player2) == Direction.clockwise)
                    angle += AngleDifference(MainGame.player2) / DistanceToTarget(MainGame.player2.Position) * Math.Abs(vel) * 2.5f;
                if (ClosestDirection(MainGame.player2) == Direction.counterclockwise)
                    angle -= AngleDifference(MainGame.player2) / DistanceToTarget(MainGame.player2.Position) * Math.Abs(vel) * 2.5f;
                vel += 0.1f;
            }

            pos.X += (float)(vel * Math.Cos((double)angle * Math.PI / 180));
            pos.Y += (float)(vel * Math.Sin((double)angle * Math.PI / 180));

            if (boundingBox.X + boundingBox.Height < -100 || boundingBox.Y + boundingBox.Height < -100
                || boundingBox.X > MainGame.WindowWidth + 100 || boundingBox.Y > MainGame.WindowHeight + 100)
            {
                MainGame.ObjectManager.Remove(this);
            }
            
            while (actionList.Count > 0 && actionList[0].Item3 == timer)
            {
                switch (actionList[0].Item1)
                {
                    case Action.speed:
                        if (actionList[0].Item5 == true)
                        {
                            if (actionList[0].Item4 != 0)
                            {
                                velChange = actionList[0].Item2 / actionList[0].Item4;
                                velTime = actionList[0].Item4;
                            }
                            else
                                vel += actionList[0].Item2;
                        }
                        else
                        {
                            if (actionList[0].Item4 != 0)
                            {
                                velChange = -(vel - actionList[0].Item2) / actionList[0].Item4;
                                velTime = actionList[0].Item4;
                            }
                            else
                                vel = actionList[0].Item2;
                        }
                        break;
                    case Action.angle:
                        if (actionList[0].Item5 == true)
                        {
                            if (actionList[0].Item4 != 0)
                            {
                                angleChange = actionList[0].Item2 / actionList[0].Item4;
                                angleTime = actionList[0].Item4;
                            }
                            else
                                angle += actionList[0].Item2;
                        }
                        else
                        {
                            if (actionList[0].Item4 != 0)
                            {
                                angleChange = -(angle - actionList[0].Item2) / actionList[0].Item4;
                                angleTime = actionList[0].Item4;
                            }
                            else
                                angle = actionList[0].Item2;
                        }
                        break;
                    case Action.curve:
                        if (actionList[0].Item5 == true)
                        {
                            if (actionList[0].Item4 != 0)
                            {
                                curveChange = actionList[0].Item2 / actionList[0].Item4;
                                curveTime = actionList[0].Item4;
                            }
                            else
                                curve += actionList[0].Item2;
                        }
                        else
                        {
                            if (actionList[0].Item4 != 0)
                            {
                                curveChange = -(curve - actionList[0].Item2) / actionList[0].Item4;
                                curveTime = actionList[0].Item4;
                            }
                            else
                                curve = actionList[0].Item2;
                        }
                        break;
                }
                actionList.RemoveAt(0);
            }

            timer++;

            rotation = angle * (float)Math.PI / 180 + drawAngle;

            base.Update();
        }

        /// <summary>
        /// Returns the parent of the bullet, usually the object that created it.
        /// </summary>
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
            if (AngleToTarget(obj.Position) - angle > 180)
                return (360 - AngleToTarget(obj.Position) - angle);
            else
                return Math.Abs(AngleToTarget(obj.Position) - angle);
        }
    }
}