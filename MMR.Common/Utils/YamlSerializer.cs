using YamlDotNet.Core.Events;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.EventEmitters;
using YamlDotNet.Serialization.NamingConventions;

namespace MMR.Common.Utils
{
    /// <summary>
    /// A YAML serializer and deserializer.
    /// </summary>
    public static class YamlSerializer
    {
        // Make lists emit in flow style instead of block style
        public class FlowStyleListEmitter : ChainedEventEmitter
        {
            public FlowStyleListEmitter(IEventEmitter nextEmitter) : base(nextEmitter) { }

            public override void Emit(SequenceStartEventInfo eventInfo, IEmitter emitter)
            {
                eventInfo.Style = SequenceStyle.Flow;
                base.Emit(eventInfo, emitter);
            }
        }

        private static readonly IDeserializer _deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .IgnoreUnmatchedProperties()
            .Build();

        private static readonly ISerializer _serializer = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitNull)
            .WithIndentedSequences()
            .Build();

        /// <summary>
        /// ISerializer that uses flow style lists instead of block style lists.
        /// </summary>
        private static readonly ISerializer _flowListserializer = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitNull)
            .WithEventEmitter(next => new FlowStyleListEmitter(next))
            .WithIndentedSequences()
            .Build();

        public static T? Deserialize<T>(string yaml)
        {
            return _deserializer.Deserialize<T>(yaml);
        }

        public static string Serialize<T>(T value)
        {
            return _serializer.Serialize(value);
        }

        /// <summary>
        /// Serializes a YAML file with flow style lists instead of block style lists.
        /// </summary>
        public static string FlowListSerialize<T>(T value)
        {
            return _flowListserializer.Serialize(value);
        }
    }
}
