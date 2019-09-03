using MMRando.Models.Vectors;

namespace MMRando.Models.Rom
{
    public class CollisionTri
    {
        public vec16 v1 = new vec16();
        public vec16 v2 = new vec16();
        public vec16 v3 = new vec16();
        public vecf32 u_n = new vecf32();
        public float d;
        public float a_v1;
        public float a_v2;
        public float a_v3;
    }
}
