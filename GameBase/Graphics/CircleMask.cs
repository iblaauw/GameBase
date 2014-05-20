using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameBase.Graphics
{
    /// <summary>
    /// Basic circle (not oval) mask. Implements the Mask interface. Note that this is 2d. For the 3d see the (coming soon) SphereMask.
    /// </summary>
    public struct CircleMask : Mask
    {
        private int _X;
        /// <summary>
        /// X and Y are the offsets (from the upper right corner of the sprite) to the center point
        /// </summary>
        public int X { 
            get { return _X; } 
            set { _X = value; } 
        }

        private int _Y;
        /// <summary>
        /// X and Y are the offsets (from the upper right corner of the sprite) to the center point
        /// </summary>
        public int Y {
            get { return _Y; }
            set { _Y = value; }
        }

        private float _Radius;
        /// <summary>
        /// The radius of the mask, in pixels.
        /// </summary>
        public float Radius
        {
            get { return _Radius; }
            set { _Radius = value; }
        }

        private int _Height;
        /// <summary>
        /// How tall the CircleMask is. This is equivaluent to 2*Radius, or Diameter.
        /// </summary>
        public int Height
        {
            get { return _Height; }
        }

        private int _Width;
        /// <summary>
        /// How wide the CircleMask is. This is equivaluent to 2*Radius, or Diameter.
        /// </summary>
        public int Width
        {
            get { return _Width; }
        }

        /// <summary>
        /// Create a new circle mask, with center at offset x,y and radius r.
        /// </summary>
        public CircleMask(int x, int y, int r)
        {
            _X = x;
            _Y = y;
            _Radius = r;
            _Height = _Width = 2 * r;
        }

        public bool Intersect(GameVector position, Mask mask, GameVector otherPosition)
        {
            if (Math.Abs(X + position.X - otherPosition.X - mask.X) > Width/2 + mask.Width/2)
                return false;
            if (Math.Abs(Y + position.Y - otherPosition.Y - mask.Y) > Height / 2 + mask.Height / 2)
                return false;


            for (int i = 0; i < 90; i++) //If this is too slow, the i++ can probably be increased to +2/+3.
            {
                float sin = (float)Math.Sin(Math.PI * i / 180.0);
                float cos = (float)Math.Sin(Math.PI * i / 180.0);
                if (mask.ContainsPoint(otherPosition, new GameVector(position.X + X + Radius * cos, position.Y + Y + Radius * sin)) //>
                    || mask.ContainsPoint(otherPosition, new GameVector(position.X + X - Radius * sin, position.Y + Y + Radius * cos)) //v
                    || mask.ContainsPoint(otherPosition, new GameVector(position.X + X - Radius * cos, position.Y + Y - Radius * sin)) //<
                    || mask.ContainsPoint(otherPosition, new GameVector(position.X + X + Radius * sin, position.Y + Y - Radius * cos)) //^
                    )
                    return true;
            }

            return false;
        }

        public bool ContainsPoint(GameVector position, GameVector point)
        {
            Vector2 pos = new Vector2(position.X + X, position.Y + Y);

            return Vector2.Distance(pos, point) < Radius;

        }
    }
}
