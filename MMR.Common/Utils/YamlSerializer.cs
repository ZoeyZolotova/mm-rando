using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace MMR.Common.Utils
{
    public static class YamlSerializer
    {
        private static readonly IDeserializer _deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .IgnoreUnmatchedProperties()
            .Build();

        private static readonly ISerializer _serializer = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitNull)
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
    }
}
