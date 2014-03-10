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
    /// A animated or non-animated sprite.
    /// </summary>
    public class Sprite
    {
        Texture2D sprite;
        int frameWidth;
        int numberOfFrames;
        int timePerFrame;
        int currentFrame;
        int count = 0;

        /// <summary>
        /// Initializes a new non-animated sprite using a Texture2D.
        /// </summary>
        /// <param name="sprite">The Texture2D to be used by the sprite.</param>
        public Sprite(Texture2D sprite)
        {
            this.sprite = sprite;
            frameWidth = sprite.Width;
            numberOfFrames = 1;
            timePerFrame = 1;
            currentFrame = 0;
        }

        /// <summary>
        /// Initializes a new animated sprite.
        /// </summary>
        /// <param name="sprite">The spritesheet to be used for the sprite.</param>
        /// <param name="numberOfFrames">Number of frames in the animation.</param>
        /// <param name="timePerFrame">How long each frame should be on the screen.</param>
        public Sprite(Texture2D sprite, int numberOfFrames, int timePerFrame)
        {
            this.sprite = sprite;
            frameWidth = sprite.Width / numberOfFrames;
            this.numberOfFrames = numberOfFrames;
            this.timePerFrame = timePerFrame;
            currentFrame = 0;
        }

        /// <summary>
        /// Initializes a new animated sprite, starting at a specific frame.
        /// </summary>
        /// <param name="sprite">The spritesheet to be used for the sprite.</param>
        /// <param name="numberOfFrames">Number of frames in the animation.</param>
        /// <param name="timePerFrame">How long each frame should be on the screen.</param>
        /// <param name="startingFrame">Which frame it should start on, with 0 being the first frame.</param>
        public Sprite(Texture2D sprite, int numberOfFrames, int timePerFrame, int startingFrame)
        {
            this.sprite = sprite;
            frameWidth = sprite.Width / numberOfFrames;
            this.numberOfFrames = numberOfFrames;
            this.timePerFrame = timePerFrame;
            currentFrame = startingFrame;
        }

        /// <summary>
        /// Updates the sprite's animation.
        /// </summary>
        public void Update()
        {
            if (count >= timePerFrame)
            {
                currentFrame = (currentFrame + 1) % numberOfFrames;
                count = 0;
            }

            count++;
        }

        /// <summary>
        /// Draws the sprite at the given position.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to be used for drawing.</param>
        /// <param name="position">The position to draw the sprite at.</param>
        /// <param name="color">The color to tint the sprite.</param>
        public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color)
        {
            spriteBatch.Draw(sprite, position, new Rectangle(frameWidth * currentFrame, 0, Width, Height), color, 0, new Vector2(), 1, 0, 0);
        }

        /// <summary>
        /// Draws the sprite in the given rectangle.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to be used for drawing.</param>
        /// <param name="destinationRectangle">The rectangle to draw the sprite in.</param>
        /// <param name="color">The color to tint the sprite.</param>
        public void Draw(SpriteBatch spriteBatch, Rectangle destinationRectangle, Color color)
        {
            spriteBatch.Draw(sprite, destinationRectangle, new Rectangle(frameWidth * currentFrame, 0, Width, Height), color, 0, new Vector2(), 0, 0);
        }

        /// <summary>
        /// Draw the sprite in the given rectangle and at the given rotation.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to be used for drawing.</param>
        /// <param name="destinationRectangle">The rectangle to draw the sprite in.</param>
        /// <param name="color">The color to tint the sprite.</param>
        /// <param name="rotation">The angle to draw the sprite at in radians.</param>
        /// <param name="origin">The sprite's origin.</param>
        /// <param name="effects">The SpriteEffects to be applied to the sprite.</param>
        /// <param name="layerDepth">The layer it should be drawn on, between 0 and 1.</param>
        public void Draw(SpriteBatch spriteBatch, Rectangle destinationRectangle, Color color, float rotation, Vector2 origin, SpriteEffects effects, float layerDepth)
        {
            spriteBatch.Draw(sprite, destinationRectangle, new Rectangle(frameWidth * currentFrame, 0, Width, Height), color, rotation, origin, effects, layerDepth);
        }

        /// <summary>
        /// Returns the width of the sprite.
        /// </summary>
        public int Width { get { return sprite.Width / numberOfFrames; } }

        /// <summary>
        /// Returns the height of the sprite.
        /// </summary>
        public int Height { get { return sprite.Height; } }
    }
}
