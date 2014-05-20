using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameBase.Graphics;

namespace GameBase
{
    /// <summary>
    /// This class is the meat of how GameBase works. Every object that will be drawn on the screen should inherit from this class.
    /// It provides a framework for updating, drawing, collision detection, etc.
    /// In order for a GameObject to be active it should be added to a GameMap.
    /// </summary>
    public abstract class GameObject
    {
        /// <summary>
        /// This will be automatically set to the owning map... do not modify!
        /// </summary>
        public GameMap map;

        /// <summary>
        /// Represents the status of a GameObject
        /// NORMAL = normal, INVISIBLE = not drawn but still updated, DISABLED = not being updated or drawn
        /// </summary>
        public enum STATUS { NORMAL, INVISIBLE, DISABLED };

        /// <summary>
        /// The status of this GameObject. A status of INVISIBLE will prevent drawing. A status of DISABLED will prevent updating or drawing.
        /// </summary>
        public STATUS status;
        
        /// <summary>
        /// The coordinates of the GameObject.
        /// </summary>
        public GameVector position;

        /// <summary>
        /// A mask can be viewed as a sort of hit-box for objects: when detecting collisions, two masks will be checked for overlap.
        /// At the moment only 2 masks are provided: RectangleMask and CircleMask. However, a custom mask can be made as long as it
        ///     implements the interface Mask.
        /// </summary>
        protected Mask mask;
        /// <summary>
        /// A mask can be viewed as a sort of hit-box for objects: when detecting collisions, two masks will be checked for overlap.
        /// At the moment only 2 masks are provided: RectangleMask and CircleMask. However, a custom mask can be made as long as it
        ///     implements the interface Mask.
        /// </summary>
        public Mask CollisionMask
        {
            get { return mask; }
        }

        /// <summary>
        /// Determines when the object is drawn. If this object has more depth than another object,
        ///     it will be drawn underneath the other object.
        /// </summary>
        public virtual int Depth { get; set; }

        /// <summary>
        /// When true, this.position refers exactly to where the object is drawn on the screen. Makes it unaffected by MapView.
        /// Use this to make objects act like a HUD.
        /// NOTE: An object with StaticView will always be drawn as on-top of a non-static object, regardless of Depth. Really, static
        ///     and non-static are not meant to interact: at the very least collision detection will go beserk... 
        ///     However, Depth works normally between two static objects.
        /// </summary>
        public bool StaticView { get; set; }

        /// <summary>
        /// A sprite is the image or animation that shows up on the screen, representing the object.
        /// At the moment there is only one sprite class: Sprite2d. However, custom ones that implement the Sprite class can be made.
        /// </summary>
        protected Sprite sprite;
        /// <summary>
        /// Get the sprite of this object.
        /// A sprite is the image or animation that shows up on the screen, representing the object.
        /// At the moment there is only one sprite class: Sprite2d. However, custom ones that implement the Sprite class can be made.
        /// </summary>
        public Sprite Sprite
        {
            get { return sprite; }
        }

        /// <summary>
        /// Required to be overriden. This gives the object functionality, use it to move the object, make it do actions, etc. It will be repeatedly called ~30 times per second.
        /// Do not call this function manually! Instead call Update.
        /// </summary>
        protected abstract void UpdateObject(GameTime gameTime);
        /// <summary>
        /// Required to be overriden. This should load any extra content that the object will use.
        /// Do not call this function manually! Instead call LoadContent.
        /// NOTE: Not necessary to load the sprite content here, that is done automatically.
        /// </summary>
        protected abstract void LoadObjectContent(ContentManager content);
        
        /// <summary>
        /// Draws the object simply at the given position. 
        /// By default this calls sprite.Draw(). Override this if you want to make custom simple drawing, or custom effects (think drawing text or numbers).
        /// Do not call this manually! Instead call Draw.
        /// NOTE: If you are using GameViews, you must override DrawObjectView as well.
        /// </summary>
        protected virtual void DrawObject(GameVector drawPosition, GameTime gameTime, SpriteBatch spriteBatch)
        {
            sprite.Draw(drawPosition, gameTime, spriteBatch);
        }

        /// <summary>
        /// Draws the object in the bounds of the given view at the given offset. 
        /// By default this calls sprite.DrawPart(). Override this to define custom drawing or effects when you are using GameViews.
        /// Do not call this manually! Instead call DrawView.
        /// </summary>
        /// <param name="bounds">
        /// Rectangle defining where the view is in game terms. It is expected that if the object is outside this rectangle, it will not be drawn,
        /// and if it overlaps, only the correct portion will be drawn.
        /// </param>
        protected virtual void DrawObjectView(GameVector offset, GameTime gameTime, SpriteBatch spriteBatch, Rectangle bounds)
        {
            sprite.DrawPart(position + offset, gameTime, spriteBatch, bounds);
        }

        /// <summary>
        /// This is called right before a GameObject is removed from a Map.
        /// By default this does nothing. Override this to do custom functionality (like clean up resources, handle sub-objects, etc.)
        /// </summary>
        protected internal virtual void OnDestroy()
        {

        }

        /// <summary>
        /// This updates the object further and gives it functionality. This calls UpdateObject.
        /// It is already called automatically by GameMap, no need to call it manually (though it can be done if desired).
        /// </summary>
        public void Update(GameTime gameTime)
        {
            if (status != STATUS.DISABLED)
                UpdateObject(gameTime);
        }

        /// <summary>
        /// This loads any of the objects content into memory. This calls LoadObjectContent.
        /// It is already called automatically by GameMap, no need to call it manually (though it can be done if desired).
        /// </summary>
        public void LoadContent(ContentManager content)
        {
            if (sprite != null) //Check is here for possible non-visible/logic only objects.
                sprite.LoadContent(content);
            LoadObjectContent(content);
        }

        /// <summary>
        /// This draws the object simply. This calls DrawObject.
        /// It is already called automatically by GameMap, no need to call it manually (though it can be done if desired).
        /// </summary>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (status == STATUS.NORMAL)
                DrawObject(position, gameTime, spriteBatch);
        }

        /// <summary>
        /// This draws the object with the given offset and view boundaries. This calls DrawObjectView.
        /// It is already called automatically by GameMap, no need to call it manually (though it can be done if desired).
        /// </summary>
        public void DrawView(GameVector offset, GameTime gameTime, SpriteBatch spriteBatch, Rectangle bounds)
        {
            if (status == STATUS.NORMAL)
                DrawObjectView(offset, gameTime, spriteBatch, bounds);
        }
    }
}
