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
using GameBase;

namespace GameBaseDebug
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        GameMap map1;

        List<GameObject> objects;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            //graphics.PreferredBackBufferHeight = 1000;
            //graphics.PreferredBackBufferWidth = 1000;

            Content.RootDirectory = "Content";

            map1 = new GameMap(Content.ServiceProvider, 1000, 1000, null);
            map1.AddObject(new SpriteGameObj());
            var xx = new SpriteGameObj(150, 75);
            xx.Depth = 3;
            map1.AddObject(xx);
            //map1.AddObject(new BackgroundObject());
            map1.AddObject(new GameBase.Graphics.Background("RandomBack"));
            map1.AddObject(new BasicHUD(1100, 100));
            map1.AddObject(new ExplosionObject(300, 475));

            MapView view1 = new MapView(new GameVector(0, 0), new GameVector(505, 0), 500, 500);
            MapView view2 = new MapView(new GameVector(500, 0), new GameVector(505, 505), 500, 500);
            MapView view3 = new MapView(new GameVector(500, 500), new GameVector(0, 505), 500, 500);
            MapView view4 = new MapView(new GameVector(0, 500), new GameVector(0, 0), 500, 500);
            map1.Views.Add(view1);
            map1.Views.Add(view2);
            map1.Views.Add(view3);
            map1.Views.Add(view4);

            objects = new List<GameObject>();
            //objects.Add(new SpriteGameObj());
            //objects.Add(new SpriteGameObj(150, 75));
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            GameState.Initialize(graphics, false);
            base.Initialize();
        }

        // This is a texture we can render.
        Texture2D myTexture;

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.

            myTexture = Content.Load<Texture2D>("GreenBack");
            
            spriteBatch = new SpriteBatch(GraphicsDevice);
            map1.LoadAll();
            //foreach (GameObject go in objects)
                //go.LoadContent(Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {

            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GameState.Keyboard.IsKeyDown(Keys.Escape))
                this.Exit();

            if (GameState.Keyboard.IsKeyDown(Keys.F))
                graphics.ToggleFullScreen();

            if (GameState.Keyboard.IsKeyDown(Keys.P))
                map1.status = GameMap.STATUS.PAUSED;

            if (GameState.Keyboard.IsKeyDown(Keys.R))
                map1.status = GameMap.STATUS.RUNNING;

            

            // Move the sprite around.

            map1.Update(gameTime);
            //foreach (GameObject go in objects)
                //go.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            // Draw the sprite.
            //spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            spriteBatch.Begin();

            //spriteBatch.Draw(myTexture, new Vector2(600, 0), new Rectangle(100, 100, 400, 400), Color.White);

            map1.Draw(gameTime);

            //foreach (GameObject go in objects)
                //go.Draw(gameTime, spriteBatch);

            //spriteBatch.Draw(myTexture, new Vector2(75, 75), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
