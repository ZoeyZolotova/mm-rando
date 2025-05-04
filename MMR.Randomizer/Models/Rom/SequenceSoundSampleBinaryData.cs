
namespace MMR.Randomizer.Models.Rom
{
    public class SequenceSoundSampleBinaryData
    {
        public byte[] BinaryData { get; set; }
        public uint Addr { get; set; }
        public uint Marker { get; set; }
        public long Hash { get; set; } = 0;

        // Store the instrument type, list index, and key region (if any)
        public string ParentFile { get; set; }
        public string InstrumentType { get; set; }
        public int ListIndex { get; set; } = -1;
        public string KeyRegion { get; set; }
    }
}
