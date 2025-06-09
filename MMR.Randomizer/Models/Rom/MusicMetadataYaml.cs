using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace MMR.Randomizer.Models.Rom
{
    /// <summary>
    /// Represents the '.meta' YAML formatted metadata file used in Zelda64 music files.
    /// </summary>
    public class MusicMetadataYaml
    {
        /// <summary>
        /// Represents the "game" field in the root YAML dictionary.
        /// </summary>
        [YamlMember(Alias = "game")]
        public string Game { get; set; }

        [YamlMember(Alias = "metadata")]
        public Meta Metadata { get; set; }

        /// <summary>
        /// Represents a formmask dictionary in the root YAML dictionary.
        /// </summary>
        [YamlMember(Alias = "formmask")]
        //public SequencePlayState[] Formmask { get; set; }
        public FormmaskLists Formmask { get; set; }

        /// <summary>
        /// Represents the "metadata" YAML dictionary.
        /// </summary>
        public class Meta
        {
            [YamlMember(Alias = "display name")]
            public string DisplayName { get; set; }

            [YamlMember(Alias = "instrument set")]
            public string InstrumentSet { get; set; }

            [YamlMember(Alias = "song type")]
            public string SongType { get; set; }

            [YamlMember(Alias = "music groups")]
            public List<object> MusicGroups { get; set; }

            [YamlMember(Alias = "audio samples")]
            public Dictionary<string, Sample> AudioSamples { get; set; }
        }

        /// <summary>
        /// Represents the "audio sample" YAML dictionary.
        /// </summary>
        public class Sample
        {
            [YamlMember(Alias = "instrument type")]
            public string Type { get; set; } = null;

            [YamlMember(Alias = "list index")]
            public int? Index { get; set; } = null;

            [YamlMember(Alias = "key region")]
            public string KeyRegion { get; set; } = null;

            [YamlMember(Alias = "temp address")]
            public uint? TempAddress { get; set; } = null;
        }

        /// <summary>
        /// Represents the "formmask" dictionary's lists.
        /// </summary>
        public class FormmaskLists
        {
            [YamlMember(Alias = "channel 0")]
            public List<string> Channel0 { get; set; } = [];

            [YamlMember(Alias = "channel 1")]
            public List<string> Channel1 { get; set; } = [];

            [YamlMember(Alias = "channel 2")]
            public List<string> Channel2 { get; set; } = [];

            [YamlMember(Alias = "channel 3")]
            public List<string> Channel3 { get; set; } = [];

            [YamlMember(Alias = "channel 4")]
            public List<string> Channel4 { get; set; } = [];

            [YamlMember(Alias = "channel 5")]
            public List<string> Channel5 { get; set; } = [];

            [YamlMember(Alias = "channel 6")]
            public List<string> Channel6 { get; set; } = [];

            [YamlMember(Alias = "channel 7")]
            public List<string> Channel7 { get; set; } = [];

            [YamlMember(Alias = "channel 8")]
            public List<string> Channel8 { get; set; } = [];

            [YamlMember(Alias = "channel 9")]
            public List<string> Channel9 { get; set; } = [];

            [YamlMember(Alias = "channel 10")]
            public List<string> Channel10 { get; set; } = [];

            [YamlMember(Alias = "channel 11")]
            public List<string> Channel11 { get; set; } = [];

            [YamlMember(Alias = "channel 12")]
            public List<string> Channel12 { get; set; } = [];

            [YamlMember(Alias = "channel 13")]
            public List<string> Channel13 { get; set; } = [];

            [YamlMember(Alias = "channel 14")]
            public List<string> Channel14 { get; set; } = [];

            [YamlMember(Alias = "channel 15")]
            public List<string> Channel15 { get; set; } = [];

            [YamlMember(Alias = "cumulative states")]
            public List<string> CumulativeStates { get; set; } = [];
        }
    }
}
