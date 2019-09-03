using System;

namespace MMRando.Utils.Mzxrules
{
    public class Vector2<T1>
    {
        public T1 x;
        public T1 y;
        public Vector2()
        {
        }
        public Vector2(T1 inV)
        {
            x = inV;
            y = inV;
        }
        public Vector2(T1 inX, T1 inY)
        {
            x = inX;
            y = inY;
        }
    }
    public class Vector2<T1, T2>
    {
        public T1 x;
        public T2 y;
    }
    public class Vector3<T1> : IFormattable, IEquatable<Vector3<T1>> where T1 : IEquatable<T1>
    {
        public T1 x { get; }
        public T1 y { get; }
        public T1 z { get; }
        public Vector3()
        {
        }
        public Vector3(T1 inV)
        {
            x = inV;
            y = inV;
            z = inV;
        }
        public Vector3(T1 inX, T1 inY, T1 inZ)
        {
            x = inX;
            y = inY;
            z = inZ;
        }
        public T1 Index(int i)
        {
            switch (i)
            {
                case 0: return x;
                case 1: return y;
                case 2: return z;
                default:
                    throw new ArgumentOutOfRangeException("Expected range between 0 and 2");
            }
        }

        public static bool operator ==(Vector3<T1> a, Vector3<T1> b)
        {
            return a.x.Equals(b.x)
                && a.y.Equals(b.y)
                && a.z.Equals(b.z);
        }

        public static bool operator !=(Vector3<T1> a, Vector3<T1> b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() + y.GetHashCode() + z.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Vector3<T1>))
            {
                return false;
            }
            var v = (Vector3<T1>)obj;
            return Equals(v);
        }

        public bool Equals(Vector3<T1> other)
        {
            return x.Equals(other.x) && y.Equals(other.y) && z.Equals(other.z);
        }

        public string ToString(string format, IFormatProvider provider)
        {
            if (x is IFormattable)
            {
                IFormattable a = (IFormattable)x;
                IFormattable b = (IFormattable)y;
                IFormattable c = (IFormattable)z;

                return $"({a.ToString(format, provider)}, {b.ToString(format, provider)}, {c.ToString(format, provider)})";
            }
            return ToString();
        }

        public override string ToString()
        {
            return $"({x}, {y}, {z})";
        }
    }
    public class Vector3<T1, T2, T3>
    {
        public T1 x;
        public T2 y;
        public T3 z;
    }
}