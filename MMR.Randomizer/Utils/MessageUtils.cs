using MMR.Common.Extensions;
using MMR.Randomizer.Attributes;
using MMR.Randomizer.Extensions;
using MMR.Randomizer.GameObjects;
using MMR.Randomizer.Models;
using MMR.Randomizer.Models.Rom;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace MMR.Randomizer.Utils
{
    public static class MessageUtils
    {
        static ReadOnlyCollection<byte> MessageHeader
            = new ReadOnlyCollection<byte>(new byte[] {
                2, 0, 0xFE, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF
        });

        public static List<MessageEntry> MakeGossipQuotes(RandomizedResult randomizedResult)
        {
            if (randomizedResult.Settings.GossipHintStyle == GossipHintStyle.Default)
                return new List<MessageEntry>();

            var random = new Random(randomizedResult.Seed);

            var randomizedItems = new List<ItemObject>();
            var hintableItems = new List<ItemObject>();
            var itemsInRegions = new Dictionary<Region, List<(ItemObject io, Item locationForImportance)>>();
            foreach (var io in randomizedResult.ItemList)
            {
                if ((!io.IsRandomized || !io.NewLocation.Value.Region().HasValue) && (!io.Item.MainLocation().HasValue || !randomizedResult.ItemList[io.Item.MainLocation().Value].IsRandomized))
                {
                    continue;
                }

                // TODO make this less hard-coded
                if (io.NewLocation == Item.UpgradeRoyalWallet)
                {
                    continue;
                }

                var item = io.Item.MainLocation().HasValue ? randomizedResult.ItemList.Find(x => x.NewLocation == io.Item.MainLocation().Value) : io;

                if (randomizedResult.Settings.ClearHints && !io.Item.MainLocation().HasValue)
                {
                    // skip free items
                    if (ItemUtils.IsStartingLocation(io.NewLocation.Value))
                    {
                        continue;
                    }
                }

                if (ItemUtils.IsRegionRestricted(randomizedResult.Settings, item.Item))
                {
                    continue;
                }

                randomizedItems.Add(item);

                if (randomizedResult.Settings.GossipHintStyle == GossipHintStyle.Random)
                {
                    if (ItemUtils.IsJunk(item.Item) && (randomizedResult.Settings.ClearHints || random.Next(8) != 0))
                    {
                        continue;
                    }
                }

                if (randomizedResult.Settings.GossipHintStyle == GossipHintStyle.Competitive)
                {
                    var preventRegions = new List<Region> { Region.TheMoon, Region.BottleCatch, Region.Misc };
                    var locationForImportance = io.Item.MainLocation().HasValue ? io.Item : io.NewLocation.Value;
                    var itemRegion = locationForImportance.Region();
                    if (itemRegion.HasValue
                        && !preventRegions.Contains(itemRegion.Value)
                        && !randomizedResult.Settings.CustomJunkLocations.Contains(item.NewLocation.Value))
                    {
                        if (!itemsInRegions.ContainsKey(itemRegion.Value))
                        {
                            itemsInRegions[itemRegion.Value] = new List<(ItemObject, Item)>();
                        }
                        itemsInRegions[itemRegion.Value].Add((item, locationForImportance));
                    }

                    var competitiveHintInfo = item.NewLocation.Value.GetAttribute<GossipCompetitiveHintAttribute>();
                    if (competitiveHintInfo == null)
                    {
                        continue;
                    }

                    if (randomizedResult.Settings.CustomJunkLocations.Contains(io.NewLocation.Value))
                    {
                        randomizedItems.Remove(item);
                        continue;
                    }

                    if (competitiveHintInfo.Condition != null && !competitiveHintInfo.Condition(randomizedResult.Settings))
                    {
                        randomizedItems.Remove(item);
                        continue;
                    }
                }

                hintableItems.Add(item);
            }

            var unusedItems = hintableItems.ToList();
            var itemsToCombineWith = new List<ItemObject>();
            var competitiveHints = new List<(string message, List<GossipQuote> allowedGossipQuotes)>();

            if (randomizedResult.Settings.GossipHintStyle == GossipHintStyle.Competitive)
            {
                var gossipStoneRequirements = LogicUtils.GetGossipStoneRequirements(randomizedResult.ItemList, randomizedResult.Logic, randomizedResult.Settings);

                var totalUniqueGossipHints = Enum.GetValues(typeof(GossipQuote)).Cast<GossipQuote>().Count(gq => !gq.IsMoonGossipStone()) / 2;

                var numberOfRequiredHints = randomizedResult.Settings.AddSongs ? 4 : 3;
                var numberOfNonRequiredHints = 3;
                var maxNumberOfClockTownHints = 2;

                var numberOfLocationHints = totalUniqueGossipHints - numberOfRequiredHints - numberOfNonRequiredHints;
                unusedItems = hintableItems.GroupBy(io => io.NewLocation.Value.GetAttribute<GossipCompetitiveHintAttribute>().Priority)
                                        .OrderByDescending(g => g.Key)
                                        .Select(g => g.OrderBy(_ => random.Next()).AsEnumerable())
                                        .Aggregate(Enumerable.Empty<ItemObject>(), (g1, g2) => g1.Concat(g2))
                                        .Take(numberOfLocationHints)
                                        .ToList();
                var combinedItems = unusedItems
                    .SelectMany(io => io.NewLocation.Value.GetAttributes<GossipCombineAttribute>().Select(gca => gca.OtherItem))
                    .Where(item => randomizedItems.Any(io => io.NewLocation == item))
                    .Select(item => randomizedItems.Single(io => io.NewLocation == item))
                    .Where(io => !unusedItems.Contains(io))
                    ;
                itemsToCombineWith.AddRange(combinedItems);

                unusedItems.AddRange(unusedItems);

                foreach (var unusedItem in unusedItems)
                {
                    (var messageText, var combined) = BuildItemHint(
                        unusedItem,
                        randomizedResult.Settings.GossipHintStyle,
                        false,
                        randomizedResult.Settings.ClearHints,
                        false,
                        randomizedResult.Settings.ProgressiveUpgrades,
                        itemsToCombineWith,
                        hintableItems,
                        random
                        );

                    var allowedGossipQuotes = combined
                        .Select(io => gossipStoneRequirements.Where(kvp => !kvp.Value.Contains(io.NewLocation.Value)).Select(kvp => kvp.Key))
                        .Aggregate((list1, list2) => list1.Intersect(list2))
                        .ToList();
                    competitiveHints.Add((messageText, allowedGossipQuotes));
                }

                var importantRegionCounts = new Dictionary<Region, List<(ItemObject io, Item locationForImportance)>>();
                var nonImportantRegionCounts = new Dictionary<Region, List<(ItemObject, Item)>>();
                var songOnlyRegionCounts = new Dictionary<Region, List<(ItemObject, Item)>>();
                var clockTownRegionCounts = new Dictionary<Region, List<(ItemObject, Item)>>();
                foreach (var kvp in itemsInRegions)
                {
                    var requiredItems = kvp.Value.Where(io => ItemUtils.IsRequired(io.io.Item, io.locationForImportance, randomizedResult) && !unusedItems.Contains(io.io) && !itemsToCombineWith.Contains(io.io)).ToList();
                    var importantItems = kvp.Value.Where(io => ItemUtils.IsImportant(io.io.Item, io.locationForImportance, randomizedResult)).ToList();

                    Dictionary<Region, List<(ItemObject, Item)>> dict;
                    if (requiredItems.Count == 0 && importantItems.Count > 0)
                    {
                        if (!randomizedResult.Settings.AddSongs && importantItems.All(io => ItemUtils.IsSong(io.io.Item) && !randomizedResult.ImportantSongLocations.Contains(io.locationForImportance)))
                        {
                            dict = songOnlyRegionCounts;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else if (requiredItems.Count == 0)
                    {
                        dict = nonImportantRegionCounts;
                    }
                    else if (Gossip.ClockTownRegions.Contains(kvp.Key))
                    {
                        dict = clockTownRegionCounts;
                    }
                    else
                    {
                        dict = importantRegionCounts;
                    }
                    
                    dict[kvp.Key] = requiredItems;
                }

                var chosenClockTownRegions = 0;
                for (var i = 0; i < numberOfRequiredHints; i++)
                {
                    var regionCounts = importantRegionCounts.AsEnumerable();
                    if (chosenClockTownRegions < maxNumberOfClockTownHints)
                    {
                        regionCounts = regionCounts.Concat(clockTownRegionCounts);
                    }
                    if (!regionCounts.Any())
                    {
                        regionCounts = regionCounts.Concat(clockTownRegionCounts);
                    }
                    if (regionCounts.Any())
                    {
                        var chosen = regionCounts.ToList().Random(random);
                        var allowedGossipQuotes = chosen.Value
                            .Select(io => gossipStoneRequirements.Where(kvp => !kvp.Value.Contains(io.locationForImportance)).Select(kvp => kvp.Key))
                            .Aggregate((list1, list2) => list1.Intersect(list2))
                            .ToList();
                        competitiveHints.Add((BuildRegionHint(chosen, RegionHintType.SomeRequired, random), allowedGossipQuotes));
                        competitiveHints.Add((BuildRegionHint(chosen, RegionHintType.SomeRequired, random), allowedGossipQuotes));
                        if (clockTownRegionCounts.Remove(chosen.Key))
                        {
                            chosenClockTownRegions++;
                        }
                        else
                        {
                            importantRegionCounts.Remove(chosen.Key);
                        }
                    }
                }

                for (var i = 0; i < numberOfNonRequiredHints; i++)
                {
                    var regionCounts = nonImportantRegionCounts.AsEnumerable();
                    regionCounts = regionCounts.Concat(songOnlyRegionCounts);
                    if (regionCounts.Any())
                    {
                        var chosen = regionCounts.ToList().Random(random);
                        RegionHintType regionHintType;
                        if (songOnlyRegionCounts.Remove(chosen.Key))
                        {
                            regionHintType = RegionHintType.OnlyImportantSong;
                        }
                        else
                        {
                            nonImportantRegionCounts.Remove(chosen.Key);
                            regionHintType = RegionHintType.NoneRequired;
                        }

                        competitiveHints.Add((BuildRegionHint(chosen, regionHintType, random), new List<GossipQuote>()));
                        competitiveHints.Add((BuildRegionHint(chosen, regionHintType, random), new List<GossipQuote>()));
                    }
                }

                unusedItems.Clear();
            }

            List<MessageEntry> finalHints = new List<MessageEntry>();

            while (competitiveHints.Any(ch => ch.allowedGossipQuotes.Count > 0))
            {
                var competitiveHint = competitiveHints
                    .Where(ch => ch.allowedGossipQuotes.Count > 0)
                    .OrderBy(ch => ch.allowedGossipQuotes.Count)
                    .ThenBy(ch => random.Next())
                    .First();

                var gossipQuote = competitiveHint.allowedGossipQuotes.Random(random);
                finalHints.Add(new MessageEntry()
                {
                    Id = (ushort)gossipQuote,
                    Message = competitiveHint.message,
                    Header = MessageHeader.ToArray()
                });
                competitiveHints.Remove(competitiveHint);
                foreach (var ch in competitiveHints)
                {
                    ch.allowedGossipQuotes.Remove(gossipQuote);
                }
            }

            foreach (var gossipQuote in Enum.GetValues<GossipQuote>().OrderBy(gq => random.Next()))
            {
                if (finalHints.Any(me => me.Id == (ushort)gossipQuote))
                {
                    continue;
                }

                string messageText = null;
                var isMoonGossipStone = gossipQuote.IsMoonGossipStone();
                if (!isMoonGossipStone && competitiveHints.Any())
                {
                    var competitiveHint = competitiveHints.Random(random);
                    messageText = competitiveHint.message;
                    competitiveHints.Remove(competitiveHint);
                }

                if (messageText == null)
                {
                    var restrictionAttributes = gossipQuote.GetAttributes<GossipRestrictAttribute>().ToList();
                    ItemObject item = null;
                    var forceClear = false;
                    while (item == null)
                    {
                        if (restrictionAttributes.Any() && (isMoonGossipStone || randomizedResult.Settings.GossipHintStyle == GossipHintStyle.Relevant))
                        {
                            var chosen = restrictionAttributes.Random(random);
                            var candidateItem = chosen.Type == GossipRestrictAttribute.RestrictionType.Item
                                ? randomizedResult.ItemList.Single(io => io.ID == (int)chosen.Item)
                                : randomizedResult.ItemList.Single(io => io.NewLocation == chosen.Item);
                            if (isMoonGossipStone || unusedItems.Contains(candidateItem))
                            {
                                item = candidateItem;
                                forceClear = chosen.ForceClear;
                            }
                            else
                            {
                                restrictionAttributes.Remove(chosen);
                            }
                        }
                        else if (unusedItems.Any())
                        {
                            if (randomizedResult.Settings.GossipHintStyle == GossipHintStyle.Competitive)
                            {
                                item = unusedItems.FirstOrDefault(io => unusedItems.Count(x => x.ID == io.ID) == 1);
                                if (item == null)
                                {
                                    item = unusedItems.Random(random);
                                }
                            }
                            else
                            {
                                item = unusedItems.Random(random);
                            }
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (!isMoonGossipStone)
                    {
                        unusedItems.Remove(item);
                    }

                    if (item != null)
                    {
                        (var hint, var combined) = BuildItemHint(
                            item,
                            randomizedResult.Settings.GossipHintStyle,
                            forceClear,
                            randomizedResult.Settings.ClearHints,
                            isMoonGossipStone,
                            randomizedResult.Settings.ProgressiveUpgrades,
                            itemsToCombineWith,
                            hintableItems,
                            random
                            );
                        messageText = hint;
                    }
                }

                if (messageText == null)
                {
                    messageText = Gossip.JunkMessages.Random(random);
                }

                finalHints.Add(new MessageEntry()
                {
                    Id = (ushort)gossipQuote,
                    Message = messageText,
                    Header = MessageHeader.ToArray()
                });
            }

            return finalHints;
        }

        private static (string, List<ItemObject>) BuildItemHint(ItemObject item, GossipHintStyle gossipHintStyle, bool forceClear, bool clearHints, bool isMoonGossipStone, bool progressiveUpgradesEnabled, List<ItemObject> itemsToCombineWith, List<ItemObject> hintableItems, Random random)
        {
            ushort soundEffectId = 0x690C; // grandma curious
            var itemNames = new List<string>();
            var locationNames = new List<string>();
            bool hasOrder = item.NewLocation.Value.HasAttribute<GossipCombineOrderAttribute>();
            var combined = new List<ItemObject>();
            combined.Add(item);
            if (forceClear || clearHints)
            {
                itemNames.Add(item.Item.ProgressiveUpgradeName(progressiveUpgradesEnabled));
                locationNames.Add(item.NewLocation.Value.Location());
                if (!isMoonGossipStone)
                {
                    var gossipCombineAttributes = item.NewLocation.Value.GetAttributes<GossipCombineAttribute>();
                    combined = itemsToCombineWith.Where(io => gossipCombineAttributes.Any(gca => gca.OtherItem == io.NewLocation)).ToList();
                    if (combined.Any())
                    {
                        combined.Add(item);
                        combined = combined.OrderBy(io => io.NewLocation.Value.GetAttribute<GossipCombineOrderAttribute>()?.Order ?? random.Next()).ToList();
                        locationNames.Clear();
                        itemNames.Clear();
                        var combinedName = gossipCombineAttributes.First().CombinedName;
                        if (!string.IsNullOrWhiteSpace(combinedName))
                        {
                            locationNames.Add(combinedName);
                        }
                        else
                        {
                            locationNames.AddRange(combined.Select(io => io.NewLocation.Value.Location()));
                        }
                        itemNames.AddRange(combined.Select(io => io.Item.ProgressiveUpgradeName(progressiveUpgradesEnabled)));
                    }
                    else
                    {
                        combined.Add(item);
                    }
                }
            }
            else
            {
                if (item.Mimic != null)
                {
                    // If item has a mimic and not using clear hints, always use a fake hint.
                    soundEffectId = 0x690A; // grandma laugh
                    itemNames.Add(item.Mimic.Item.ItemHints().Random(random));
                    locationNames.Add(item.NewLocation.Value.LocationHints().Random(random));
                }
                else if (isMoonGossipStone || gossipHintStyle == GossipHintStyle.Competitive || random.Next(100) >= 5) // 5% chance of fake/junk hint if it's not a moon gossip stone or competitive style
                {
                    itemNames.Add(item.Item.ItemHints().Random(random));
                    locationNames.Add(item.NewLocation.Value.LocationHints().Random(random));
                }
                else
                {
                    if (random.Next(2) == 0) // 50% chance for fake hint. otherwise default to junk hint.
                    {
                        soundEffectId = 0x690A; // grandma laugh
                        itemNames.Add(item.Item.ItemHints().Random(random));
                        locationNames.Add(hintableItems.Random(random).NewLocation.Value.LocationHints().Random(random));
                    }
                }
            }
            if (itemNames.Any() && locationNames.Any())
            {
                return (BuildGossipQuote(soundEffectId, locationNames, itemNames, hasOrder, random), combined);
            }
            return (null, null);
        }

        private enum RegionHintType
        {
            NoneRequired,
            SomeRequired,
            OnlyImportantSong,
        }

        private static string BuildRegionHint(KeyValuePair<Region, List<(ItemObject, Item)>> regionInfo, RegionHintType regionHintType, Random random)
        {
            var region = regionInfo.Key;
            //var numberOfRequiredItems = regionInfo.Value.Count;

            ushort soundEffectId = 0x690C; // grandma curious
            string start = Gossip.MessageStartSentences.Random(random);

            string sfx = $"{(char)((soundEffectId >> 8) & 0xFF)}{(char)(soundEffectId & 0xFF)}";
            var locationMessage = region.Name();
            var mid = "is";
            var (itemMessage, color) = regionHintType switch
            {
                RegionHintType.NoneRequired => ("a foolish choice", TextCommands.ColorSilver),
                RegionHintType.SomeRequired => ("on the Way of the Hero", TextCommands.ColorYellow),
                RegionHintType.OnlyImportantSong => ("foolish except for its song", TextCommands.ColorOrange),
                _ => throw new ArgumentException("Invalid argument.", nameof(regionHintType))
            };

            return $"\x1E{sfx}{start} {color}{locationMessage}{TextCommands.ColorWhite} {mid} {itemMessage}...\xBF".Wrap(35, "\x11");

            //var mid = "has";
            //return $"\x1E{sfx}{start} {TextCommands.ColorRed}{locationMessage}{TextCommands.ColorWhite} {mid} {color}{NumberToWords(numberOfImportantItems)} important item{(numberOfImportantItems == 1 ? "" : "s")}{TextCommands.ColorWhite}...\xBF".Wrap(35, "\x11");
        }

        private static string BuildGossipQuote(ushort soundEffectId, IEnumerable<string> locationMessages, IEnumerable<string> itemMessages, bool hasOrder, Random random)
        {
            int startIndex = random.Next(Gossip.MessageStartSentences.Count);
            int midIndex = random.Next(Gossip.MessageMidSentences.Count);
            string start = Gossip.MessageStartSentences[startIndex];
            string mid = Gossip.MessageMidSentences[midIndex];

            string sfx = $"{(char)((soundEffectId >> 8) & 0xFF)}{(char)(soundEffectId & 0xFF)}";

            return $"\x1E{sfx}{start} {string.Join(" and ", locationMessages.Select(locationName => $"\x01{locationName}\x00"))} {mid} {string.Join(hasOrder ? " then " : " and ", itemMessages.Select(itemName => $"\x06{itemName}\x00"))}...\xBF".Wrap(35, "\x11");
        }

        public static string GetArticle(Item item, string indefiniteArticle = null)
        {
            var shopTexts = item.ShopTexts();
            return shopTexts.IsMultiple
                ? ""
                : shopTexts.IsDefinite
                    ? "the "
                    : indefiniteArticle ?? (Regex.IsMatch(item.Name(), "^[aeiou]", RegexOptions.IgnoreCase)
                        ? "an "
                        : "a ");
        }

        public static string GetPronoun(Item item)
        {
            var shopTexts = item.ShopTexts();
            var itemAmount = Regex.Replace(item.Name(), "[^0-9]", "");
            return shopTexts.IsMultiple && !string.IsNullOrWhiteSpace(itemAmount)
                ? "them"
                : "it";
        }

        public static string GetPronounOrAmount(Item item)
        {
            var shopTexts = item.ShopTexts();
            var itemAmount = Regex.Replace(item.Name(), "[^0-9]", "");
            return shopTexts.IsMultiple
                ? string.IsNullOrWhiteSpace(itemAmount)
                    ? " it"
                    : " " + itemAmount
                : shopTexts.IsDefinite
                    ? " it"
                    : " one";
        }

        public static string GetVerb(Item item)
        {
            var shopTexts = item.ShopTexts();
            var itemAmount = Regex.Replace(item.Name(), "[^0-9]", "");
            return shopTexts.IsMultiple && !string.IsNullOrWhiteSpace(itemAmount)
                ? "are"
                : "is";
        }

        public static string GetFor(Item item)
        {
            var shopTexts = item.ShopTexts();
            return shopTexts.IsDefinite
                ? "is"
                : "for";
        }

        public static string GetAlternateName(string name)
        {
            return Regex.Replace(name, "[0-9]+ ", "");
        }

        static string GetRawPlural(string name)
        {
            var useEs = "ch,i,ns,o,sh,ss,x".Split(',').Any(x => name.EndsWith(x));
            if (useEs)
            {
                // Use "es" ending instead of "s".
                return $"{name}es";
            }
            else if ("by,ry".Split(',').Any(x => name.EndsWith(x)))
            {
                // Replace "y" => "ies" in certain situations.
                var withoutY = name.Substring(0, name.Length - 1);
                return $"{withoutY}ies";
            }
            else if (name.EndsWith("s"))
            {
                // Assume name is already plural.
                return name;
            }
            else
            {
                return $"{name}s";
            }
        }

        public static string GetPlural(string name)
        {
            var alt = GetAlternateName(name);
            var altSplit = alt.Split(' ');

            // Check if the plural is identical to the singular.
            var samePlural = "Bombchu".Split(',').Any(x => alt.Equals(x));
            if (samePlural)
            {
                return alt;
            }

            // Check if there is a starting noun which should be pluralized instead.
            var startingNoun = "Bottle,Elegy,Lens,Letter,Map,Mask,Oath,Pendant,Piece,Sonata,Song".Split(',').Any(x => altSplit[0].Equals(x));
            if (startingNoun)
            {
                altSplit[0] = GetRawPlural(altSplit[0]);
                return string.Join(" ", altSplit);
            }

            return GetRawPlural(alt);
        }

        private static string[] numberWordUnitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
        private static string[] numberWordTensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };
        public static string NumberToWords(int number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                if (number < 20)
                    words += numberWordUnitsMap[number];
                else
                {
                    words += numberWordTensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + numberWordUnitsMap[number % 10];
                }
            }

            return words;
        }
    }
}
