using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameBase
{
    /// <summary>
    /// This class reduces down to Vector2 or Vector3 depending on if the game is 2d or 3d.
    /// </summary>
    public struct GameVector
    {
        public float X, Y, Z;
        public GameVector(float x, float y, float? z = null)
        {
            if (z.HasValue != GameState.Is3d)
            {
                throw new InvalidOperationException(
                    "A game vector must be 3 values long if the game is 3d, otherwise it must be 2 long.");
            }

            X = x;
            Y = y;
            if (z.HasValue)
                Z = z.Value;
            else
                Z = 0;
        }

        public static implicit operator Vector2(GameVector vector)
        {
            if (GameState.Is3d)
                throw new InvalidCastException("Cannot change a 3d GameVector into a 2d Vector");
            
            return new Vector2(vector.X, vector.Y);
        }

        public static implicit operator Vector3(GameVector vector)
        {
            if (!GameState.Is3d)
                throw new InvalidCastException("Cannot change a 2d GameVector into a 3d Vector");

            return new Vector3(vector.X, vector.Y, vector.Z);
        }

        public static implicit operator GameVector(Vector2 vector)
        {
            if (GameState.Is3d)
                throw new InvalidCastException("Cannot change a 2d Vector into a 3d GameVector");

            return new GameVector(vector.X, vector.Y);
        }

        public static implicit operator GameVector(Vector3 vector)
        {
            if (!GameState.Is3d)
                throw new InvalidCastException("Cannot change a 3d Vector into a 2d GameVector");

            return new GameVector(vector.X, vector.Y, vector.Z);
        }

        public static GameVector operator +(GameVector v1, GameVector v2)
        {
            if (GameState.Is3d)
                return new GameVector(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
            else
                return new GameVector(v1.X + v2.X, v1.Y + v2.Y);
        }

        public static GameVector operator +(GameVector v1, Vector2 v2)
        {
            if (GameState.Is3d)
                throw new InvalidCastException("Cannot change a 2d Vector into a 3d GameVector.");

            return new GameVector(v1.X + v2.X, v1.Y + v2.Y);
        }

        public static GameVector operator +(GameVector v1, Vector3 v2)
        {
            if (!GameState.Is3d)
                throw new InvalidCastException("Cannot change a 3d Vector into a 2d GameVector.");

            return new GameVector(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }

        public static GameVector operator *(float x, GameVector v)
        {
            if (GameState.Is3d)
                return new GameVector(x * v.X, x * v.Y, x * v.Z);
            return new GameVector(x * v.X, x * v.Y);
        }
    }
}
