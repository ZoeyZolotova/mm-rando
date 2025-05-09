
namespace MMR.Randomizer.Models.Rom
{
    /// <summary>
    /// Represents a binary Zelda64 ADPCM audio sample file.
    /// </summary>
    public class SequenceSoundSampleBinaryData
    {
        public byte[] BinaryData { get; set; }
        public uint Addr { get; set; }
        public uint Marker { get; set; }
        public long Hash { get; set; } = 0;

        // Store the instrument type, list index, and key region (if any)
        public string ParentFile { get; set; }
        /// <summary>
        /// The type of instrument the sample is assigned to.
        /// </summary>
        public string InstrumentType { get; set; }
        /// <summary>
        /// The index of the parent structure in the instrument, drum, or effect list.
        /// </summary>
        public int ListIndex { get; set; } = -1;
        /// <summary>
        /// The key region of the sample struct in the instrument struct.
        /// </summary>
        public string KeyRegion { get; set; }
    }
}
