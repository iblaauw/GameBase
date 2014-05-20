using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameBase
{
    /// <summary>
    /// This is what lets only part of a map be drawn at a time. 
    /// Really unless you're making something really simple a MapView should be added to GameMap.Views.
    /// At the moment this is only meant for 2d games. Will update soon.
    /// </summary>
    public class MapView
    {
        /// <summary>
        /// The position that the MapView is in the Game. This position signifies the top-left corner.
        /// </summary>
        public GameVector GamePosition { get; set; }

        /// <summary>
        /// The position that the MapView is drawn on the screen. This position signifies the top-left corner.
        /// </summary>
        public GameVector ScreenPosition { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }

        /// <summary>
        /// If Active is false, the View is not drawn.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Create a new MapView at the given game position, screen position, width, and height.
        /// </summary>
        public MapView(GameVector gamePosition, GameVector screenPosition, int width, int height)
        {
            GamePosition = gamePosition;
            ScreenPosition = screenPosition;
            Height = height;
            Width = width;

            Active = true;
        }

        /// <summary>
        /// Draws the view. Only meant to be used by GameMap!
        /// </summary>
        internal void Draw(IEnumerable<GameObject> objects, SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (!Active)
                return;

            foreach (GameObject go in objects)
            {
                if (go.Sprite == null)
                    continue;

                if (go.position.X + go.Sprite.Width >= GamePosition.X
                    && go.position.X <= GamePosition.X + Width
                    && go.position.Y + go.Sprite.Height >= GamePosition.Y
                    && go.position.Y <= GamePosition.Y + Height
                    )
                {
                    go.DrawView(
                        -1*GamePosition + ScreenPosition, gameTime, spriteBatch, 
                        new Rectangle((int)(GamePosition.X - go.position.X), (int)(GamePosition.Y - go.position.Y), Width, Height)
                        );
                }

            }

        }

    }
}
