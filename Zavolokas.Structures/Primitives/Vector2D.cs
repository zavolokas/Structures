using System;

namespace Zavolokas.Structures
{
    public struct Vector2D : IComparable<Vector2D>, IEquatable<Vector2D>
    {
        public double X;
        public double Y;

        private static readonly Vector2D _zero = new Vector2D(0, 0);

        public Vector2D(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static bool operator >=(Vector2D v1, Vector2D v2)
        {
            if (v1.CompareTo(v2) == -1)
                return false;
            return true;
        }

        public static bool operator <=(Vector2D v1, Vector2D v2)
        {
            if (v1.CompareTo(v2) == 1)
                return false;
            return true;
        }

        public static bool operator ==(Vector2D v1, Vector2D v2)
        {
            if (v1.CompareTo(v2) != 0)
                return false;
            return true;
        }

        public static bool operator !=(Vector2D v1, Vector2D v2)
        {
            if (v1.CompareTo(v2) == 0)
                return false;
            return true;
        }

        public static bool operator >(Vector2D v1, Vector2D v2)
        {
            if (v1.CompareTo(v2) != 1)
                return false;
            return true;
        }

        public static bool operator <(Vector2D v1, Vector2D v2)
        {
            if (v1.CompareTo(v2) != -1)
                return false;
            return true;
        }

        public static Vector2D operator +(Point v1, Vector2D v2)
        {
            return new Vector2D(v1.X + v2.X, v1.Y + v2.Y);
        }

        public static double operator *(Vector2D v1, Vector2D v2)
        {
            return (v1.X * v2.X) + (v1.Y * v2.Y);
        }

        public static Vector2D operator *(double a, Vector2D v)
        {
            return new Vector2D(v.X * a, v.Y * a);
        }

        public static Vector2D operator *(Vector2D v, double a)
        {
            return new Vector2D(v.X * a, v.Y * a);
        }

        public static Vector2D operator +(Vector2D v1, Point v2)
        {
            return new Vector2D(v1.X + v2.X, v1.Y + v2.Y);
        }

        public static Vector2D operator +(Vector2D v1, Vector2D v2)
        {
            return new Vector2D(v1.X + v2.X, v1.Y + v2.Y);
        }

        public static Vector2D operator -(Point v1, Vector2D v2)
        {
            return new Vector2D(v1.X - v2.X, v1.Y - v2.Y);
        }

        public static Vector2D operator -(Vector2D v1, Point v2)
        {
            return new Vector2D(v1.X - v2.X, v1.Y - v2.Y);
        }

        public static Vector2D operator -(Vector2D v1, Vector2D v2)
        {
            return new Vector2D(v1.X - v2.X, v1.Y - v2.Y);
        }

        public static Vector2D Zero
        {
            get { return _zero; }
        }

        public Vector2D Ortogonal1
        {
            get
            {
                return new Vector2D(Y, -X);
            }
        }

        public Vector2D Ortogonal2
        {
            get
            {
                return new Vector2D(-Y, X);
            }
        }

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: 
        ///                     Value 
        ///                     Meaning 
        ///                     Less than zero 
        ///                     This object is less than the <paramref name="other"/> parameter.
        ///                     Zero 
        ///                     This object is equal to <paramref name="other"/>. 
        ///                     Greater than zero 
        ///                     This object is greater than <paramref name="other"/>. 
        /// </returns>
        /// <param name="other">An object to compare with this object.
        ///                 </param>
        public int CompareTo(Vector2D other)
        {
            double diff = SquareLength - other.SquareLength;
            if (diff < 0)
                return -1;
            if (diff == 0)
                return 0;
            return 1;
        }

        public double SquareLength
        {
            get { return (X * X) + (Y * Y); }
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.
        ///                 </param>
        public bool Equals(Vector2D other)
        {
            return other.X.Equals(X) && other.Y.Equals(Y);
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <returns>
        /// true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.
        /// </returns>
        /// <param name="obj">Another object to compare to. 
        ///                 </param><filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof(Vector2D)) return false;
            return Equals((Vector2D)obj);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            unchecked
            {
                return (X.GetHashCode() * 397) ^ Y.GetHashCode();
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("({0},{1})", X, Y);
        }
    }
}