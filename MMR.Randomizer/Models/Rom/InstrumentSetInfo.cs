
namespace MMR.Randomizer.Models.Rom
{
    public class InstrumentSetInfo
    {
        public byte[] BankBinary { get; set; } = null;
        public int BankSlot { get; set; }
        public byte[] BankMetaData { get; set; } = null;
        public int Modified = 0;
        public long Hash { get; set; } = 0;

    }
}
