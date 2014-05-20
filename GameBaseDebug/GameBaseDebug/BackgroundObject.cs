using GameBase.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameBaseDebug
{
    public class BackgroundObject : GameBase.GameObject
    {
        public BackgroundObject()
        {
            sprite = new Sprite2d("GreenBack");
            Depth = 100;
        }

        protected override void UpdateObject(Microsoft.Xna.Framework.GameTime gameTime)
        {
            
        }

        protected override void LoadObjectContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            
        }
    }
}
