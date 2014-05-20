using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameBase.Graphics
{
    /// <summary>
    /// This is a game object that functions as a basic background. Inherits from GameObject.
    /// It can also do a tiling background: takes an image and repeats it across the whole map.
    /// NOTE: The default Depth is 100. This can be changed like normal, through the Depth property.
    /// </summary>
    public class Background : GameObject
    {
        private bool _tiled;
        /// <summary>
        /// If Tiled is true, then background's image will be repeated to fill up the whole map.
        ///     Otherwise the image is just drawn once, in the upper left corner.
        /// </summary>
        public bool Tiled { get { return _tiled; } }

        private bool added;
        private string fileName;

        private List<BackgroundHelper> helpers;

        /// <summary>
        /// Create a new background. This will become the background of the map which it is added to.
        /// </summary>
        /// <param name="imageName">The filename of the image, from the content section. Preferrably without extensions.</param>
        /// <param name="tiled">If tiled is true, then the image given will be repeated to fill up the whole map.
        ///     Otherwise the image is just drawn once, in the upper left corner.</param>
        public Background(string imageName, bool tiled = true)
        {
            Depth = 100;
            fileName = imageName;
            _tiled = tiled;

            added = false;

            if (!_tiled)
                sprite = new Sprite2d(imageName);
        }

        protected override void UpdateObject(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (!added)
            {
                map.AddMultiple(helpers);
                added = true;
            }
        }

        protected override void LoadObjectContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            if (_tiled)
            {
                Texture2D text = content.Load<Texture2D>(fileName);
                helpers = new List<BackgroundHelper>();
                for (int i = 0; i < map.Width; i += text.Width)
                {
                    for (int j = 0; j < map.Height; j += text.Height)
                    {
                        BackgroundHelper help = new BackgroundHelper(i, j, fileName);
                        help.Depth = Depth;
                        helpers.Add(help);
                    }
                }
            }
            
        }

        protected internal override void OnDestroy()
        {
            if (_tiled)
            {
                foreach (BackgroundHelper h in helpers)
                    map.RemoveObject(h);
            }
            base.OnDestroy();
        }

        public override int Depth
        {
            get
            {
                return base.Depth;
            }
            set
            {
                base.Depth = value;
                if (helpers != null)
                {
                    foreach (var h in helpers)
                    {
                        h.Depth = value;
                    }
                }
            }
        }


        /*******************Helper****************/
        private class BackgroundHelper : GameObject
        {
            public BackgroundHelper(int x, int y, string fileName)
            {
                sprite = new Sprite2d(fileName);
                position = new GameVector(x, y);
            }

            protected override void UpdateObject(Microsoft.Xna.Framework.GameTime gameTime)
            {
            }

            protected override void LoadObjectContent(Microsoft.Xna.Framework.Content.ContentManager content)
            {
            }
        }

    }
}
