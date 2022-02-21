using System.Collections.Generic;
using MMR.Randomizer.Extensions;
using MMR.Randomizer.GameObjects;
using MMR.Randomizer.Utils;

namespace MMR.Randomizer.Models
{
    public class ItemObject
    {
        public int ID { get; set; }
        public Item Item => ItemOverride ?? (Item)ID; // todo?
        public string Name { get; set; }
        public List<Item> DependsOnItems { get; set; } = new List<Item>();
        public List<List<Item>> Conditionals { get; set; } = new List<List<Item>>();
        public List<Item> CannotRequireItems { get; set; } = new List<Item>();
        public int TimeNeeded { get; set; }
        public int TimeAvailable { get; set; }
        public int TimeSetup { get; set; }
        public Item? NewLocation { get; set; }

        public bool IsTrick { get; set; }
        public string TrickTooltip { get; set; }
        public string TrickCategory { get; set; }

        public bool IsRandomized { get; set; }

        /// <summary>
        /// Name override used in spoiler log.
        /// </summary>
        public string NameOverride { get; set; }

        /// <summary>
        /// Used for Ice Traps and Recovery Hearts.
        /// </summary>
        public Item? ItemOverride { get; set; }

        /// <summary>
        /// Item which is being mimiced, used by ice traps.
        /// </summary>
        public MimicItem Mimic { get; set; }

        public Item DisplayItem => Mimic?.Item ?? Item;
    }
}
