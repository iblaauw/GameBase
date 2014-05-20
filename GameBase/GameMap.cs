using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using GameBase.Graphics;


namespace GameBase
{
    public class GameMap
    {
        /// <summary>
        /// This enum represents different status options:
        /// Running = normal, PAUSED = no update, still draws, DISABLED = no update or draw
        /// </summary>
        public enum STATUS { RUNNING, PAUSED, DISABLED };


        private List<GameObject> objects;
        private List<GameObject> removed;
        private List<GameObject> added;
        private Texture2D background;
        private ContentManager content;

        private bool loaded;

        private Dictionary<Type, List<GameObject>> sorted;

        /// <summary>
        /// The status of the map.
        /// </summary>
        public STATUS status = STATUS.RUNNING;

        /// <summary>
        /// List of MapView objects to be drawn. If this is null or empty, the game will attempt to draw the entire map.
        /// </summary>
        public List<MapView> Views { get; set; }
        private int _width;
        /// <summary>
        /// The width of the map in pixels.
        /// </summary>
        public int Width { get { return _width; } }
        private int _height;
        /// <summary>
        /// The height of the map in pixels.
        /// </summary>
        public int Height { get { return _height; } }


        /// <summary>
        /// Create a new Map. Only should be used from the main Game class.
        /// </summary>
        /// <param name="serviceProvider">Get this by doing: Content.ServiceProvider</param>
        /// <param name="backgroundName">The path to the content to be used for a background. Use null if there is none.</param>
        /// <param name="contentRootDirectory">The path to the root of where content will be found. Use null for default.</param>
        public GameMap(IServiceProvider serviceProvider, int width, int height, string contentRootDirectory)
        {
            objects = new List<GameObject>();
            removed = new List<GameObject>();
            added = new List<GameObject>();
            content = new ContentManager(serviceProvider);
            content.RootDirectory = contentRootDirectory ?? "Content";
            sorted = new Dictionary<Type, List<GameObject>>();

            Views = new List<MapView>();

            loaded = false;

            _width = width;
            _height = height;
        }

        /// <summary>
        /// Add a GameObject to the map. Unless a GameObject is added to a map, it will not execute.
        /// NOTE: the object will not actually show up until the next game cycle. This should not usually affect anything.
        /// </summary>
        public void AddObject(GameObject go)
        {
            if (loaded)
            {
                added.Add(go);
            }
            else
                AddObject_help(go);
        }

        /// <summary>
        /// Actually adds the objects
        /// </summary>
        private void AddObject_help(GameObject go)
        {
            objects.Add(go);

            if (!sorted.ContainsKey(go.GetType()))
            {
                List<GameObject> l = new List<GameObject>();
                l.Add(go);
                sorted.Add(go.GetType(), l);
            }
            else
                sorted[go.GetType()].Add(go);

            if (loaded)
                go.LoadContent(content);

            go.map = this;
        }

        /// <summary>
        /// Adds a collection of GameObjects to the map. For the moment this simply call AddObject on the whole collection.
        /// </summary>
        public void AddMultiple(IEnumerable<GameObject> objs)
        {
            foreach (GameObject go in objs)
                AddObject(go);
        }

        /// <summary>
        /// Deletes a GameObject from the map.
        /// </summary>
        public void RemoveObject(GameObject go)
        {
            go.OnDestroy();

            //We can't immediately remove an object because in the context that this would be called it would be modifying loops.
            //So defer the action, removal will take place in update
            go.status = GameObject.STATUS.DISABLED;
            removed.Add(go);
        }
        
        private void RemoveObject_help(GameObject go)
        {
            objects.Remove(go);

            sorted[go.GetType()].Remove(go);
        }

        /// <summary>
        /// Actually executes a single step of the map, causing things to move and function. 
        /// This should be called in the Update function of the main Game class.
        /// </summary>
        public void Update(GameTime gameTime)
        {
            if (status != STATUS.RUNNING)
                return;

            foreach (GameObject go in removed)
                RemoveObject_help(go);
            removed.Clear();

            foreach (GameObject go in added)
                AddObject_help(go);
            added.Clear();

            foreach (GameObject go in objects)
                go.Update(gameTime);
        }

        /// <summary>
        /// Actually draw the map and all the objects in it.
        /// </summary>
        public void Draw(GameTime gameTime)
        {
            if (status == STATUS.DISABLED)
                return; //Don't draw anything

            SpriteBatch spriteBatch = new SpriteBatch(GameState.GraphicsManager.GraphicsDevice);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            objects.Sort((g, h) => h.Depth - g.Depth);

            if (Views == null || Views.Count == 0)
            {
                foreach (GameObject go in objects)
                {
                    go.Draw(gameTime, spriteBatch);
                }
            }
            else
            {
                List<GameObject> stat, nonstat;
                SplitByStatic(out stat, out nonstat);

                foreach (var view in Views)
                {
                    view.Draw(nonstat, spriteBatch, gameTime);
                }

                foreach (var go in stat)
                {
                    go.Draw(gameTime, spriteBatch);
                }
            }
            
            spriteBatch.End();
        }
        
        /// <summary>
        /// If you are not using the map, and will not for a long time, then call this to unload all content.
        /// By default this sets the status to disabled. Be sure to re-enable it before you use it again.
        /// If this gets called, you MUST call LoadAll before using the map again.
        /// </summary>
        public void UnloadAll()
        {
            status = STATUS.DISABLED;
            content.Unload();

            loaded = false;
        }

        /// <summary>
        /// Loads all content. 
        /// </summary>
        public void LoadAll()
        {
            foreach (GameObject go in objects)
                go.LoadContent(content);

            loaded = true;
        }

        /// <summary>
        /// This will return a list of GameObjects in the map that are currently colliding with the given object, according to their masks.
        /// It will only check GameObjects with a type of T (or inherit T).
        /// NOTES: A GameObject with no CollisionMask will never be returned
        ///     If obj has no CollisionMask, an empty list will be returned
        ///     A GameObject that has a status of DISABLED will never be returned
        ///     A GameObject in a different Map will never be returned
        ///     The same object (or where obj.Equals() is true) will never be returned
        /// </summary>
        /// <typeparam name="T">The type of GameObject that will be checked. T must inherit from GameObject!</typeparam>
        /// <returns>A list of all the objects of type T considered colliding with obj.</returns>
        public IEnumerable<T> CollidingWith<T>(GameObject obj) where T : GameObject
        {
            return GetAllOfType<T>().Where(g => !g.Equals(obj) 
                && g.status != GameObject.STATUS.DISABLED
                && g.CollisionMask != null && obj.CollisionMask != null
                && obj.CollisionMask.Intersect(obj.position, g.CollisionMask, g.position)
                );
        }

        private IEnumerable<T> GetAllOfType<T>() where T : GameObject
        {
            foreach(var k in sorted)
            {
                if (typeof(T).IsAssignableFrom(k.Key))
                {
                    foreach (var g in k.Value)
                        yield return (T)g;
                }
            }
        }

        private void SplitByStatic(out List<GameObject> yes, out List<GameObject> no)
        {
            yes = new List<GameObject>();
            no = new List<GameObject>();

            foreach (GameObject go in objects)
            {
                if (go.StaticView)
                    yes.Add(go);
                else
                    no.Add(go);
            }
        }

    }
}
