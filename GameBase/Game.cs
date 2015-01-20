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

namespace GameBase
{
    /// <summary>
    /// This is the highest level object for the GameBase framework. It inherits from the XNA Game class, allowing it to be used like a normal XNA game.
    /// <para>To use: replace lines from main in the normal Program.cs file so it looks something like this - </para>
    /// <para>    IGameStateManager manager = new ... </para>
    /// <para>     using (GameBase.Game game = new GameBase.Game(manager, ...)) </para>
    /// <para>    { </para>
    /// <para>        game.Run(); </para>
    /// <para>    } </para>
    /// </summary>
    public sealed class Game : Microsoft.Xna.Framework.Game
    {

        #region Fields

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        IGameStateManager stateManager;


        #endregion

        #region Public Properties

        /// <summary>
        /// Get and set the default background color that is displayed.
        /// </summary>
        public Color BackgroundColor { get; set; }

        /// <summary>
        /// Get the current GameMap that is being used
        /// </summary>
        public GameMap CurrentMap { get; private set; }

        private int windowWidth;
        /// <summary>
        /// Gets or sets the width of the main window. 
        /// This ideally shouldn't be used to set unless you are resizing. Use the Game constructor instead.
        /// </summary>
        public int WindowWidth
        {
            get { return windowWidth; }
            set
            {
                windowWidth = value;
                graphics.PreferredBackBufferWidth = value;
                graphics.ApplyChanges();
            }
        }

        private int windowHeight;
        /// <summary>
        /// Gets or sets the height of the main window. 
        /// This ideally shouldn't be used to set unless you are resizing. Use the Game constructor instead.
        /// </summary>
        public int WindowHeight
        {
            get { return windowHeight; }
            set 
            { 
                windowHeight = value;
                graphics.PreferredBackBufferHeight = value;
                graphics.ApplyChanges();
            }
        }

        /// <summary>
        /// Get or Set the directory that all the XNA Content lives in.
        /// By default this is "Content"
        /// </summary>
        public string ContentDirectory
        {
            get { return Content.RootDirectory; }
            set { Content.RootDirectory = value; }
        }

        #endregion

        #region Public Methods

        public void ToggleFullscreen()
        {
            graphics.ToggleFullScreen();
        }

        /// <summary>
        /// Create a GameMap that can then be used by the map.
        /// </summary>
        public GameMap CreateMap(int width, int height)
        {
            return new GameMap(Content.ServiceProvider, width, height, ContentDirectory);
        }

        /// <summary>
        /// Start displaying the given map.
        /// </summary>
        /// <param name="unloadLastMap"> If true (default), the current map will unload all of its XNA content.</param>
        public void SwitchToMap(GameMap map, bool unloadLastMap = true)
        {
            if (unloadLastMap)
                CurrentMap.UnloadAll();

            CurrentMap = map;
            map.LoadAll();
            map.status = GameMap.STATUS.RUNNING;
        }

        #endregion

        /// <summary>
        /// Create a Game object that notifies the given IGameStateManager
        /// </summary>
        public Game(IGameStateManager stateManager, int windowWidth = 800, int windowHeight = 600, bool fullScreen = false)
        {
            this.stateManager = stateManager;
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;

            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = windowWidth;
            graphics.PreferredBackBufferHeight = windowHeight;
            graphics.IsFullScreen = fullScreen;


            //Set defaults
            BackgroundColor = Color.CornflowerBlue;
            Content.RootDirectory = "Content";

            CurrentMap = stateManager.InitializeFirstMap(this);
        }

        #region Game Implementing Methods

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
            stateManager.OnInitialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            CurrentMap.LoadAll();
            stateManager.OnLoad();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            CurrentMap.UnloadAll();
            stateManager.OnUnload();
        }

        protected override void Update(GameTime gameTime)
        {
            // Move the sprite around.
            CurrentMap.Update(gameTime);
            stateManager.OnUpdate();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            // Draw the sprite.
            spriteBatch.Begin();

            CurrentMap.Draw(gameTime);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        #endregion
    }
}
