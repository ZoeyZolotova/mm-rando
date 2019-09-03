using MMRando.Models.Vectors;
using System;
using System.Collections.Generic;

namespace MMRando.Utils
{

    public class VectorUtils
    {

        public static vecf32 CrossProduct(vecf32 A, vecf32 B)
        {
            return new vecf32
            {
                x = (A.y * B.z) - (A.z * B.y),
                y = (A.z * B.x) - (A.x * B.z),
                z = (A.x * B.y) - (A.y * B.x)
            };
        }

        public static vecf32 Subtract(vecf32 A, vecf32 B)
        {
            return new vecf32
            {
                x = A.x - B.x,
                y = A.y - B.y,
                z = A.z - B.z
            };
        }

        public static vecf32 Subtract(vec16 A, vec16 B)
        {
            return new vecf32
            {
                x = A.x - B.x,
                y = A.y - B.y,
                z = A.z - B.z
            };
        }

        public static float Length(vecf32 A)
        {
            return (float)Math.Sqrt((A.x * A.x) + (A.y * A.y) + (A.z * A.z));
        }

        public static vecf32 Normalise(vecf32 A)
        {
            float a = Length(A);
            return new vecf32
            {
                x = A.x / a,
                y = A.y / a,
                z = A.z / a
            };
        }

        public static float PlaneDist(vecf32 n, vec16 p)
        {
            return - ((n.x * p.x) + (n.y * p.y) + (n.z * p.z));
        }

    }

}