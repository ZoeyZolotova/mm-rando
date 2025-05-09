using System.Collections.Generic;
using System.IO;
using MMR.Randomizer.Constants;

namespace MMR.Randomizer.Models.Rom
{
    //[System.Diagnostics.DebuggerDisplay("[{Name}][{Replaces.ToString(\"X2\")}]")] // slower but more debug
    [System.Diagnostics.DebuggerDisplay("{Name}")]
    public class SequenceInfo
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Game { get; set; }
        public string Directory { get; set; } = Values.MusicDirectory;
        public string Filename => Path.Combine(Directory, Name);
        public string Filepath { get; set; } // For music cache
        public int Replaces { get; set; } = -1;
        /// <summary>
        /// Majora's Mask sequence ID that the sequence represents.
        /// </summary>
        public int SeqId { get; set; } = -1; // MM_seq -> SeqId
        public List<int> Categories { get; set; } = [];
        public int Instrument { get; set; }
        public SequenceBinaryData SequenceBinary { get; set; }
        public int PreviousSlot { get; set; } = -1;
        public List<SequenceSoundSampleBinaryData> InstrumentSamples { get; set; } = null;

    }
}
