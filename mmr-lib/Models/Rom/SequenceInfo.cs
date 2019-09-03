using System.Collections.Generic;

namespace MMRando.Models.Rom
{

    public class SequenceInfo
    {
        public string Name { get; set; }
        public int SequenceId { get; set; } = -1;
        public List<int> Type { get; set; } = new List<int>();
        public int Instrument { get; set; }
        public bool Replaceable { get; set; }
        public int Replaces { get; set; } = -1;
    }
}
