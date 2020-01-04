using System.Collections.Generic;

namespace MMRando.Models.Rom
{

    public class SequenceInfo
    {
        public string Name { get; set; }
        public int Replaces { get; set; } = -1;
        public SequenceId MM_seq { get; set; } = SequenceId.invalid;
        public SequenceType Type { get; set; }
        public int Instrument { get; set; }
    }
}
