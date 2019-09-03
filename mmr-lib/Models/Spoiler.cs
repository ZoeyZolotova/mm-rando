using MMRando.GameObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization.Json;

namespace MMRando.Models
{
    public class Spoiler
    {
        public string Version { get; set; }

        public string SettingsString { get; set; }

        public bool RandomizeDungeonEntrances { get; set; }

        public string Seed { get; set; }

        public List<SpoilerItem> ItemList { get; set; }

        public List<ItemLogic> Logic { get; set; }

        public Dictionary<GossipQuote, string> GossipHints { get; set; }

        public string CustomItemListString { get; set; }

        public string LogicJson
        {
            get
            {
                var serializer = new DataContractJsonSerializer(typeof(List<ItemLogic>));
                using (var stream = new MemoryStream())
                {
                    serializer.WriteObject(stream, Logic);
                    stream.Seek(0, SeekOrigin.Begin);
                    using (var reader = new StreamReader(stream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }

        public int[] NewDestinationIndices { get; set; }

        public Item[] Entrances { get; set; } = new Item[]
        {
            Item.AreaWoodFallTempleAccess,
            Item.AreaSnowheadTempleAccess,
            Item.AreaInvertedStoneTowerTempleAccess,
            Item.AreaGreatBayTempleAccess,
        };
    }
}

