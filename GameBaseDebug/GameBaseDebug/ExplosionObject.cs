using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameBase;
using GameBase.Graphics;

namespace GameBaseDebug
{
    public class ExplosionObject : GameObject
    {
        public ExplosionObject(int x, int y)
        {
            sprite = new Sprite2d("explosion", 64, 64, 25);
            Depth = -10;
            position = new GameVector(x, y);
        }

        protected override void UpdateObject(Microsoft.Xna.Framework.GameTime gameTime)
        {
            sprite.StepAnimation();
        }

        protected override void LoadObjectContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            
        }
    }
}
