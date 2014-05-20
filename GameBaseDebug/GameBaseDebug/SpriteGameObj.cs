using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using GameBase;
using GameBase.Graphics;

namespace GameBaseDebug
{
    public class SpriteGameObj : GameObject
    {

        SoundEffect bounceSound;

        Vector2 speed;

        public SpriteGameObj()
        {
            position = Vector2.Zero;
            speed = new Vector2(5, 3);
            sprite = new Sprite2d("profile_pic"); 
        }

        public SpriteGameObj(float x, float y)
        {
            position = new Vector2(x, y);
            speed = new Vector2(5, 5);
            sprite = new Sprite2d("profile_pic");
        }

        protected override void LoadObjectContent(ContentManager content)
        {
            bounceSound = content.Load<SoundEffect>("sound/bounceSound");
            mask = new RectangleMask(95, 180, 0, sprite.Texture.Width);
        }

        protected override void DrawObject(GameVector drawPosition, GameTime gameTime, SpriteBatch spriteBatch)
        {
            sprite.Draw(drawPosition, gameTime, spriteBatch);
        }

        protected override void UpdateObject(GameTime gameTime)
        {
            sprite.StepAnimation();


            //var otherBottles = map.CollidingWith<SpriteGameObj>(this);
            //foreach(SpriteGameObj sgo in otherBottles)
            //{
                //map.RemoveObject(sgo);
            //}

            if (map.CollidingWith<SpriteGameObj>(this).SingleOrDefault(f => true) != null)
            {
                //bounceSound.Play();
                speed.X += 1;
                speed.Y += -1;
                BasicHUD.count++;
            }


            int MaxX =
                //GameState.ViewPortWidth - sprite.Texture.Width;
                map.Width - sprite.Texture.Width;
            int MinX = 0;
            int MaxY =
                //GameState.ViewPortHeight - sprite.Texture.Height;
                map.Height - sprite.Texture.Height;
            int MinY = 0;

            // Check for bounce.
            if (position.X > MaxX)
            {
                //bounceSound.Play();
                speed.X *= -1;
                position.X = MaxX;
            }

            else if (position.X < MinX)
            {
                //bounceSound.Play();
                speed.X *= -1;
                position.X = MinX;
            }

            if (position.Y > MaxY)
            {
                //bounceSound.Play();
                speed.Y *= -1;
                position.Y = MaxY;
            }

            else if (position.Y < MinY)
            {
                //bounceSound.Play();
                speed.Y *= -1;
                position.Y = MinY;
            }

            // Get the current gamepad state.
            /*GamePadState currentState = GamePad.GetState(PlayerIndex.One);

            // Process input only if connected and button A is pressed.
            if (currentState.IsConnected && currentState.Buttons.A ==
                ButtonState.Pressed)
            {
                // Button A is currently being pressed;
                if (Math.Abs(speed.Y) == 50) speed.Y *= -2;
            }
            else
            {
                // Button A is not being pressed;
                if (speed.Y > 0) speed.Y = 50;
                else speed.Y *= -50;
            }*/

            position = position + speed;
        }
    }
}
