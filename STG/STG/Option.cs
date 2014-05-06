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
    class Option:GameObject
    {
        Player parent;
        public Vector2 relativePosition;
        public Vector2 focusPosition;

        public Option(Player parent, Sprite sprite, Vector2 relativePosition)
        {
            this.parent = parent;
            this.relativePosition = relativePosition;
            this.sprite = sprite;
            pos.X = parent.Position.X + relativePosition.X;
            pos.Y = parent.Position.Y + relativePosition.Y;
            this.boundingBox = new Rectangle((int)pos.X, (int)pos.Y, sprite.Width, sprite.Height);
            objType = 'O';
            focusPosition.X = parent.Position.X + (relativePosition.X * 3 / 4);
            focusPosition.Y = parent.Position.Y + (relativePosition.Y - boundingBox.Height / 2);
        }

        public override void Update()
        {
            pos.X = parent.Position.X + relativePosition.X;
            pos.Y = parent.Position.Y + relativePosition.Y;

            base.Update();
        }

        public Vector2 RelativePosition { get { return relativePosition; } }
    }
}
