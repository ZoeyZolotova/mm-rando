
namespace MMR.Randomizer.Models.Rom
{

    public class SequenceBinaryData
    {
        /// <summary>
        /// A bytearray copy of a sequence file's raw binary data.
        /// </summary>
        public byte[] SequenceData { get; set; } = null;
        public InstrumentSetInfo InstrumentSet { get; set; } = null;
        public byte[] Formmask { get; set; } // Why was this internal? It needs to be public for it to write to the cache
    }
}
