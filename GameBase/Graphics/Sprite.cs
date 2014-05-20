using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameBase.Graphics
{
    /// <summary>
    /// This interface is used to provide methods for actually drawing an image/animation. 
    /// A GameObject must have a Sprite in order for it to be drawn.
    /// </summary>
    public interface Sprite
    {
        /// <summary>
        /// Only for 2d use. This is the XNA Texture object that will actually be drawn.
        /// It will contain more details than what is exposed here.
        /// </summary>
        Texture2D Texture { get; }
        /// <summary>
        /// The color that the image is blended with when drawn. Default is White (which is no change).
        /// </summary>
        Color DrawColor { get; set; }
        int Width { get; }
        int Height { get; }

        /// <summary>
        /// Actually load the image into memory. Automatically called for the member "sprite" of GameObject.
        /// </summary>
        /// <param name="content">Get this from the LoadObjectContent in GameObject.</param>
        void LoadContent(ContentManager content);
        /// <summary>
        /// Use the given spriteBatch to do a basic drawing of the sprite.
        /// </summary>
        void Draw(GameVector position, GameTime gameTime, SpriteBatch spriteBatch);
        /// <summary>
        /// Use the given spriteBatch to draw the Sprite with the given source bounds.
        /// Main use is in the MapView object.
        /// </summary>
        /// <param name="bounds">A rectangle specifying the source. 
        ///     Note that this may be much bigger than or go outside of the area of the image. 
        ///     Some expectations:
        ///         If the sprite is fully contained in bounds, it will be drawn normally
        ///         If bounds is completely outside of the sprite, nothing will be drawn.
        ///         When there is overlap, no part of the image that doesn't exist will be drawn.
        ///     </param>
        void DrawPart(GameVector position, GameTime gameTime, SpriteBatch spriteBatch, Rectangle bounds);
        /// <summary>
        /// If the sprite has an animation, this will progress it. Should be called in Update (not Draw, otherwise pause effects will be broken).
        /// </summary>
        void StepAnimation();
    }
}
