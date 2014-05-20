using GameBase;
using GameBase.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameBaseDebug
{
    public class BasicHUD : GameObject
    {
        public static int count;
        private SpriteFont font;

        public BasicHUD(int x, int y)
        {
            count = 0;
            sprite = new Sprite2d("RedBlock");
            StaticView = true;
            position = new GameVector(x, y);
        }

        protected override void UpdateObject(Microsoft.Xna.Framework.GameTime gameTime)
        {
            
        }

        protected override void LoadObjectContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            font = content.Load<SpriteFont>("DefaultFont");
        }

        protected override void DrawObject(GameVector drawPosition, Microsoft.Xna.Framework.GameTime gameTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            base.DrawObject(drawPosition, gameTime, spriteBatch);
            spriteBatch.DrawString(font, String.Format("Count: {0}", count), position + new Vector2(10,10), Color.Black);
        }
    }
}
