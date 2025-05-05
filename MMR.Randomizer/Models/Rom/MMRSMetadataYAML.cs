using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace MMR.Randomizer.Models.Rom
{
    public class MMRSMetadataYAML
    {
        [YamlMember(Alias = "game")]
        public string Game { get; set; }

        [YamlMember(Alias = "metadata")]
        public Meta Metadata { get; set; }

        [YamlMember(Alias = "formmask")]
        public SequencePlayState[] Formmask { get; set; }

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
    }
}
