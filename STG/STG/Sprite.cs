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
    public class Sprite
    {
        Texture2D sprite;
        int frameWidth;
        public int numberOfFrames;
        int timePerFrame;
        int currentFrame;
        int count = 0;

        public Sprite(Texture2D sprite)
        {
            this.sprite = sprite;
            frameWidth = sprite.Width;
            numberOfFrames = 1;
            timePerFrame = 1;
            currentFrame = 0;
        }

        public Sprite(Texture2D sprite, int numberOfFrames, int timePerFrame)
        {
            this.sprite = sprite;
            frameWidth = sprite.Width / numberOfFrames;
            this.numberOfFrames = numberOfFrames;
            this.timePerFrame = timePerFrame;
            currentFrame = 0;
        }

        public Sprite(Texture2D sprite, int numberOfFrames, int timePerFrame, int startingFrame)
        {
            this.sprite = sprite;
            frameWidth = sprite.Width / numberOfFrames;
            this.numberOfFrames = numberOfFrames;
            this.timePerFrame = timePerFrame;
            currentFrame = startingFrame;
        }

        public void Update()
        {
            if (count >= timePerFrame)
            {
                currentFrame = (currentFrame + 1) % numberOfFrames;
                count = 0;
            }

            count++;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color)
        {
            spriteBatch.Draw(sprite, position, new Rectangle(frameWidth * currentFrame, 0, Width, Height), color, 0, new Vector2(), 1, 0, 0);
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle destinationRectangle, Color color)
        {
            spriteBatch.Draw(sprite, destinationRectangle, new Rectangle(frameWidth * currentFrame, 0, Width, Height), color, 0, new Vector2(), 0, 0);
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle destinationRectangle, Color color, float rotation, Vector2 origin, SpriteEffects effects, float layerDepth)
        {
            spriteBatch.Draw(sprite, destinationRectangle, new Rectangle(frameWidth * currentFrame, 0, Width, Height), color, rotation, origin, effects, layerDepth);
        }

        public int Width { get { return sprite.Width / numberOfFrames; } }

        public int Height { get { return sprite.Height; } }
    }
}
