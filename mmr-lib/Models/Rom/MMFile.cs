using System.Linq;

namespace MMRando.Models.Rom
{
    public class MMFile
    {
        public int Addr;
        public int End;
        public int Cmp_Addr;
        public int Cmp_End;
        public byte[] Data;
        public bool IsCompressed;
        public bool WasEdited;

        public MMFile Clone()
        {
            return new MMFile
            {
                Addr = Addr,
                End = End,
                Cmp_Addr = Cmp_Addr,
                Cmp_End = Cmp_End,
                IsCompressed = IsCompressed,
                WasEdited = WasEdited,
                Data = Data?.ToArray()
            };
        }
    }
}
