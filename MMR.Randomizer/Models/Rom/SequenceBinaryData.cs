
namespace MMR.Randomizer.Models.Rom
{

    public class SequenceBinaryData
    {
        /// <summary>
        /// A bytearray copy of a sequence file's raw binary data.
        /// </summary>
        public byte[] SequenceData { get; set; } = null;
        public InstrumentSetInfo InstrumentSet { get; set; } = null;
        public byte[] Formmask { get; internal set; }
    }
}
