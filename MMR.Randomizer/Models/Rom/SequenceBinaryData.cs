namespace MMR.Randomizer.Models.Rom
{
    /// <summary>
    /// A bytearray copy of a sequence file's raw binary data.
    /// </summary>
    public class SequenceBinaryData
    {
        public byte[] SequenceData { get; set; } = null;
        public InstrumentSetInfo InstrumentSet { get; set; } = null;
        public byte[] Formmask { get; set; }
    }
}

