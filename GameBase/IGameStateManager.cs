using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameBase
{
    /// <summary>
    /// This is the high level state manager for a Game object. Use this to manage when the game changes maps or other stuff.
    /// </summary>
    public interface IGameStateManager
    {
        /// <summary>
        /// Initialize the first map and give it to the game.
        /// </summary>
        GameMap InitializeFirstMap(Game game);

        /// <summary>
        /// Called on starting, after loading content and the map, but before starting to run
        /// </summary>
        void OnInitialize();

        /// <summary>
        /// Called when the game is done updating its current map
        /// </summary>
        void OnUpdate();

        /// <summary>
        /// Called when the game is done loading its current map
        /// </summary>
        void OnLoad();

        /// <summary>
        /// Called when the game is done unloading its current map
        /// </summary>
        void OnUnload();
    }
}
