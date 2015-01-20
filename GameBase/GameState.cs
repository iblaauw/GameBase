using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameBase
{
    /// <summary>
    /// GameState contains some global objects that are referrenced by many things, such as game dimensions, input, etc.
    /// </summary>
    public static class GameState
    {
        private static GraphicsDeviceManager manager;
        public static GraphicsDeviceManager GraphicsManager
        {
            get {
                if (manager == null)
                    throw new Exception("GameState.Initialize must be called before its variables can be accessed.");
                return manager;
            }
        }

        /// <summary>
        /// This must be called from the Initialize function in your main Game class, or GameState may not function.
        /// </summary>
        /// <param name="deviceManager">In the main Game class, this will just be "graphics".</param>
        public static void Initialize(GraphicsDeviceManager deviceManager, bool is3d)
        {
            manager = deviceManager;
            Is3d = is3d;
        }

        /// <summary>
        /// Gets all input data about the given player GamePad.
        /// </summary>
        public static GamePadState GetGamePad(PlayerIndex player)
        {
            return GamePad.GetState(player);
        }

        /// <summary>
        /// Gets the input data about the Keyboard
        /// </summary>
        public static KeyboardState Keyboard
        {
            get { return Microsoft.Xna.Framework.Input.Keyboard.GetState(); }
        }

        /// <summary>
        /// Gets the input data about the Mouse
        /// </summary>
        public static MouseState Mouse
        {
            get { return Microsoft.Xna.Framework.Input.Mouse.GetState(); }
        }

        /// <summary>
        /// Gets how wide the drawing space is in pixels. This is the screen size if fullscreen, otherwise the window size.
        /// </summary>
        public static int ViewPortWidth
        {
            get { return GraphicsManager.GraphicsDevice.Viewport.Width; }
        }

        /// <summary>
        /// Gets how high the drawing space is in pixels. This is the screen size if fullscreen, otherwise the window size.
        /// </summary>
        public static int ViewPortHeight
        {
            get { return GraphicsManager.GraphicsDevice.Viewport.Height; }
        }

        /// <summary>
        /// Gets the aspect ration the drawing space is using. This is for the screen if fullscreen, otherwise for the window.
        /// </summary>
        public static float AspectRatio
        {
            get { return GraphicsManager.GraphicsDevice.Viewport.AspectRatio; }
        }

        /// <summary>
        /// Is this a 3d game? If true, coordinates have 3 components: X,Y,Z. Otherwise they have 2: X,Y.
        /// </summary>
        public static bool Is3d
        {
            get;
            set;
        }

    }
}
