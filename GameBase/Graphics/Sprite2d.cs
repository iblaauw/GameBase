using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace GameBase.Graphics
{
    /// <summary>
    /// A basic 2-dimensional sprite. Implements interface Sprite.
    /// This sprite is used for basic 2d images, as well as 2d strip animations.
    /// </summary>
    public class Sprite2d : Sprite
    {
        private Texture2D texture;
        /// <summary>
        /// Gets the underlying XNA texture object. This may provide some more details not exposed in this class.
        /// </summary>
        public Texture2D Texture
        {
            get { return texture; }
        }

        private string fileName;

        /// <summary>
        /// If UseStrip is true, then the underlying image is treated as a strip animation (a larger image that is a 
        /// sequence of images that should be played). If not, the entire image is drawn.
        /// </summary>
        public bool UseStrip { get; set; }

        private int stripHeight;    //Height of one square
        private int stripWidth;     //Length of one square
        private int stripNumb;      //Number of squares total
        private int stripLong;      //Number of squares widthwise

        private int stripPosition;
        /// <summary>
        /// If this sprite is strip animated, this will get/set what frame the animation is at.
        /// </summary>
        public int StripPosition {
            get { return stripPosition; }
            set { stripPosition = value % stripNumb; }
        }

        /// <summary>
        /// Create a basic 2d sprite with no animation.
        /// </summary>
        /// <param name="contentName">File name of image (without extension prefered)</param>
        public Sprite2d(string contentName)
        {
            fileName = contentName;
            UseStrip = false;
            DrawColor = Color.White;
        }

        /// <summary>
        /// Create a basic 2d sprite using strip image for animation.
        /// </summary>
        /// <param name="contentName">File name of strip image (without extension preferred)</param>
        /// <param name="stripHeight">The height of one "square" in the strip</param>
        /// <param name="stripLength">The length of one "square" in the strip</param>
        /// <param name="stripNumb">The total number of "squares" in the strip</param>
        public Sprite2d(string contentName, int stripHeight, int stripLength, int stripNumb)
        {
            fileName = contentName;
            UseStrip = true;
            DrawColor = Color.White;

            this.stripHeight = stripHeight;
            this.stripWidth = stripLength;
            this.stripNumb = stripNumb;
        }


        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>(fileName);
            if (UseStrip)
                this.stripLong = Texture.Width / stripWidth;
        }

        public void Draw(GameVector position, 
            Microsoft.Xna.Framework.GameTime gameTime, 
            Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            if (UseStrip)
            {
                int x = stripPosition % stripLong;
                int y = stripPosition / stripLong;
                Rectangle source = new Rectangle(x*stripWidth, y*stripHeight, stripWidth, stripHeight);
                
                spriteBatch.Draw(texture, position, source, DrawColor);
            }
            else
            {
                spriteBatch.Draw(texture, position, DrawColor);
            }
        }

        public void DrawPart(GameVector position, GameTime gameTime, SpriteBatch spriteBatch, Rectangle bounds)
        {
            int w = UseStrip ? stripWidth : Texture.Width;
            int h = UseStrip ? stripHeight : Texture.Height;

            if (bounds.Left > w || bounds.Right < 0 || bounds.Top > h || bounds.Bottom < 0)
                return;

            int left = Math.Max(bounds.Left, 0);
            int top = Math.Max(bounds.Top, 0);
            int right = Math.Min(bounds.Right, w);
            int bot = Math.Min(bounds.Bottom, h);

            Vector2 finalPos = new Vector2(position.X + left, position.Y + top);

            Rectangle source = new Rectangle(left, top, right - left, bot - top);


            if (UseStrip)
            {
                int x = stripPosition % stripLong;
                int y = stripPosition / stripLong;
                source.X += x * w;
                source.Y += y * h;
            }

            spriteBatch.Draw(texture, finalPos, source, DrawColor);
        }

        public void StepAnimation()
        {
            stripPosition++;
            if (stripPosition >= stripNumb)
                stripPosition = 0;
        }


        public Color DrawColor { get; set; }
        
        /// <summary>
        /// If UseStrip is true, this is the Width of each frame, not the entire image.
        /// </summary>
        public int Width {
            get { return UseStrip ? stripWidth : Texture.Width; }
        }

        /// <summary>
        /// If UseStrip is true, this is the Width of each frame, not the entire image.
        /// </summary>
        public int Height {
            get { return UseStrip ? stripHeight : Texture.Height; }
        }

    }
}
