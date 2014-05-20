using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameBase.Graphics
{
    /// <summary>
    /// This interface is used to define where in a Sprite a collision area is defined. (Think hitboxes)
    /// A GameObject is required to have a mask if it is testing for collisions.
    /// A RectangleMask and CircleMask are built in. In order to make custom shapes, implement this.
    /// </summary>
    public interface Mask
    {
        /// <summary>
        /// The maximum height of the mask. Doesn't need to be exact, but it may break if this is smaller than actual.
        /// Just used to help optimize runtime.
        /// </summary>
        int Height { get; }
        /// <summary>
        /// The maximum width of the mask. Doesn't need to be exact, but it may break if this is smaller than actual.
        /// Just used to help optimize runtime.
        /// </summary>
        int Width { get; }

        /// <summary>
        /// X and Y are offsets to the center from the top right corner of the sprite.
        /// Ex: Rectangle mask, if X,Y are 0,0 then the rectangle will be centered around 0,0 + whatever position the object is at.
        /// </summary>
        int X { get; set; } //TODO: switch this to a GameVector in order to support 3d masks as well.
        /// <summary>
        /// X and Y are offsets to the center from the top right corner of the sprite.
        /// </summary>
        int Y { get; set; }
        
        

        /// <summary>
        /// Whether this mask intersects with another mask.
        /// NOTE: there may be a corner case in these methods where one mask is completely inside the other. 
        ///     To help prevent that, make the smaller mask of the two call this.
        /// </summary>
        bool Intersect(GameVector position, Mask mask, GameVector otherPosition);
        /// <summary>
        /// Does the given point lie within the mask?
        /// Used for implementing Intersect
        /// </summary>
        bool ContainsPoint(GameVector position, GameVector point);
    }
}
