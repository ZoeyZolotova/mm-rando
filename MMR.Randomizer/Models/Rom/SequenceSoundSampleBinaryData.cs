
namespace MMR.Randomizer.Models.Rom
{
    public class SequenceSoundSampleBinaryData
    {
        public byte[] BinaryData { get; set; } = null;
        public uint Addr { get; set; } = 0;
        public uint Marker { get; set; } = 0;
        public long Hash { get; set; } = 0;

        // Store the instrument type, list index, and key region (if any)
        public string InstrumentType { get; set; } = null;
        public int ListIndex { get; set; } = -1;
        public string KeyRegion { get; set; } = null;
    }
}
