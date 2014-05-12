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
    class ScrollingBackground
    {
        // class ScrollingBackground
        private Vector2 screenpos, origin, texturesize;
        private Texture2D mytexture;
        private int screenwidth;
        private int screenheight;
        public void Load(GraphicsDevice device, Texture2D backgroundTexture)
        {
            mytexture = backgroundTexture;
            screenheight = MainGame.PlayingArea.Height;
            screenwidth = MainGame.PlayingArea.Width;
            // Set the origin so that we're drawing from the 
            // center of the top edge.
            origin = new Vector2(mytexture.Width, 0);
            // Set the screen position to the center of the screen.
            screenpos = new Vector2(screenwidth / 2, screenheight / 2);
            // Offset to draw the second texture, when necessary.
            texturesize = new Vector2(0, mytexture.Height);
        }
        // ScrollingBackground.Update
        public void Update(float deltaY)
        {
            screenpos.Y += deltaY;
            screenpos.Y = screenpos.Y % (mytexture.Height * 4);
        }
        // ScrollingBackground.Draw
        public void Draw(SpriteBatch batch)
        {
            for (int i = 0; i < 2; i++)
            {
                // Draw the texture, if it is still onscreen.
                if (screenpos.Y < screenheight + mytexture.Height)
                {
                    batch.Draw(mytexture, new Vector2(screenpos.X + i * mytexture.Width, screenpos.Y), null,
                         Color.White, 0, origin, 1, SpriteEffects.None, 0f);
                }
                // Draw the texture a second time, behind the first,
                // to create the scrolling illusion.
                batch.Draw(mytexture, new Vector2(screenpos.X + i * mytexture.Width, screenpos.Y) - texturesize, null,
                     Color.White, 0, origin, 1, SpriteEffects.None, 0f);

                batch.Draw(mytexture, new Vector2(screenpos.X + i * mytexture.Width, screenpos.Y) + texturesize, null,
                     Color.White, 0, origin, 1, SpriteEffects.None, 0f);

                batch.Draw(mytexture, new Vector2(screenpos.X + i * mytexture.Width, screenpos.Y) + texturesize * 2, null,
                     Color.White, 0, origin, 1, SpriteEffects.None, 0f);

                batch.Draw(mytexture, new Vector2(screenpos.X + i * mytexture.Width, screenpos.Y) - texturesize * 2, null,
                     Color.White, 0, origin, 1, SpriteEffects.None, 0f);

                batch.Draw(mytexture, new Vector2(screenpos.X + i * mytexture.Width, screenpos.Y) - texturesize * 3, null,
                     Color.White, 0, origin, 1, SpriteEffects.None, 0f);

                batch.Draw(mytexture, new Vector2(screenpos.X + i * mytexture.Width, screenpos.Y) - texturesize * 4, null,
                     Color.White, 0, origin, 1, SpriteEffects.None, 0f);
            }
        }
    }
}
