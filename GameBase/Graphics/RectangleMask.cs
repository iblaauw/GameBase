using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameBase.Graphics
{
    /// <summary>
    /// A very basic rectangle mask. Implements the Mask interface.
    /// Note that this is a 2d only mask. Use the (to come soon) CubeMask class for 3d.
    /// </summary>
    public struct RectangleMask : Mask
    {
        /// <summary>
        /// Note that X and Y mark the center of the rectangle
        /// </summary>
        private int _x;
        public int X
        {
            get { return _x; }
            set { _x = value; }
        }

        /// <summary>
        /// Note that X and Y mark the center of the rectangle
        /// </summary>
        private int _y;
        public int Y
        {
            get { return _y; }
            set { _y = value; }
        }

        private int _height;
        public int Height
        {
            get { return _height; }
        }

        private int _width;
        public int Width
        {
            get { return _width; }
        }

        /// <summary>
        /// Creates a basic rectangle mask. Note that all offsets are from the top right corner.
        /// </summary>
        public RectangleMask(int leftOffset, int rightOffset, int topOffset, int botOffset)
        {
            if (leftOffset >= rightOffset || topOffset >= botOffset)
                throw new Exception("Invalid bounds on rectangle mask.");

            _x = (leftOffset + rightOffset) / 2;
            _y = (topOffset + botOffset) / 2;
            _width = rightOffset - leftOffset;
            _height = botOffset - topOffset;
        }

        public bool Intersect(GameVector position, Mask mask, GameVector otherPosition)
        {
            position = new GameVector(position.X + X, position.Y + Y);
            GameVector otherP = new GameVector(otherPosition.X + mask.X, otherPosition.Y + mask.Y);
            //first check if they are too far away by height/width
            if (Math.Abs(position.X - otherP.X) > Width/2 + mask.Width/2)
                return false;
            if (Math.Abs(position.Y - otherP.Y) > Height/2 + mask.Height/2)
                return false;

            //then check corner points
            if (mask.ContainsPoint(otherPosition, new GameVector(position.X+Width/2, position.Y-Height/2)) //^>
                || mask.ContainsPoint(otherPosition, new GameVector(position.X + Width/2, position.Y + Height/2)) //v>
                || mask.ContainsPoint(otherPosition, new GameVector(position.X - Width/2, position.Y + Height/2)) //<v
                || mask.ContainsPoint(otherPosition, new GameVector(position.X - Width/2, position.Y - Height/2))) //<^
                return true;

            //then check mid points
            if (mask.ContainsPoint(otherPosition, new GameVector(position.X + Width/2, position.Y)) //>
                || mask.ContainsPoint(otherPosition, new GameVector(position.X, position.Y + Height/2)) //v
                || mask.ContainsPoint(otherPosition, new GameVector(position.X - Width/2, position.Y)) //<
                || mask.ContainsPoint(otherPosition, new GameVector(position.X, position.Y - Height/2))) //^
                return true;

            //then check full length :(
            for (int i = 0; i < Width; i++)
            {
                if (mask.ContainsPoint(otherPosition, new GameVector(position.X - Width/2 + i, position.Y-Height/2))
                    || mask.ContainsPoint(otherPosition, new GameVector(position.X - Width/2 + i, position.Y + Height/2)))
                    return true;
            }

            for (int i = 0; i < Height; i++)
            {
                if (mask.ContainsPoint(otherPosition, new GameVector(position.X-Width/2, position.Y - Height/2 + i))
                    || mask.ContainsPoint(otherPosition, new GameVector(position.X + Width/2, position.Y - Height/2 + i)))
                    return true;
            }

            return false;
        }

        public bool ContainsPoint(GameVector position, GameVector point)
        {
            position = new GameVector(position.X + X, position.Y + Y);

            return point.X >= position.X && point.X <= position.X + Width
                && point.Y >= position.Y && point.Y <= position.Y + Height;
        }
    }
}
