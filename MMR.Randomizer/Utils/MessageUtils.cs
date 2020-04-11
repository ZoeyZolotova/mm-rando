﻿using MMR.Common.Extensions;
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
            var competitiveHints = new List<string>();
            var itemsInRegions = new Dictionary<Region, List<ItemObject>>();
            foreach (var item in randomizedResult.ItemList)
            {
                if (item.NewLocation == null)
                {
                    continue;
                }

                if (randomizedResult.Settings.ClearHints)
                {
                    // skip free items
                    if (ItemUtils.IsStartingLocation(item.NewLocation.Value))
                    {
                        continue;
                    }
                }

                if (!item.IsRandomized)
                {
                    continue;
                }

                var itemName = item.Item.Name();
                if (randomizedResult.Settings.GossipHintStyle != GossipHintStyle.Competitive 
                    && (itemName.Contains("Heart") || itemName.Contains("Rupee"))
                    && (randomizedResult.Settings.ClearHints || random.Next(8) != 0))
                {
                    continue;
                }

                if (randomizedResult.Settings.GossipHintStyle == GossipHintStyle.Competitive)
                {
                    var preventRegions = new List<Region> { Region.TheMoon, Region.BottleCatch, Region.Misc };
                    var itemRegion = item.NewLocation.Value.Region();
                    if (itemRegion.HasValue
                        && !preventRegions.Contains(itemRegion.Value)
                        && !randomizedResult.Settings.CustomJunkLocations.Contains(item.NewLocation.Value))
                    {
                        if (!itemsInRegions.ContainsKey(itemRegion.Value))
                        {
                            itemsInRegions[itemRegion.Value] = new List<ItemObject>();
                        }
                        itemsInRegions[itemRegion.Value].Add(item);
                    }

                    var competitiveHintInfo = item.NewLocation.Value.GetAttribute<GossipCompetitiveHintAttribute>();
                    if (competitiveHintInfo == null)
                    {
                        continue;
                    }

                    if (randomizedResult.Settings.CustomJunkLocations.Contains(item.NewLocation.Value))
                    {
                        continue;
                    }

                    if (competitiveHintInfo.Condition != null && competitiveHintInfo.Condition(randomizedResult.Settings))
                    {
                        continue;
                    }
                }

                randomizedItems.Add(item);
            }

            var unusedItems = randomizedItems.ToList();

            if (randomizedResult.Settings.GossipHintStyle == GossipHintStyle.Competitive)
            {
                var totalUniqueGossipHints = Enum.GetValues(typeof(GossipQuote)).Cast<GossipQuote>().Count(gq => !gq.IsMoonGossipStone()) / 2;

                var numberOfRequiredHints = randomizedResult.Settings.AddSongs ? 4 : 3;
                var numberOfNonRequiredHints = 3;
                var maxNumberOfSongOnlyHints = 3;
                var maxNumberOfClockTownHints = 2;

                var numberOfLocationHints = totalUniqueGossipHints - numberOfRequiredHints - numberOfNonRequiredHints;
                unusedItems = randomizedItems.GroupBy(io => io.NewLocation.Value.GetAttribute<GossipCompetitiveHintAttribute>().Priority)
                                        .OrderByDescending(g => g.Key)
                                        .Select(g => g.OrderBy(_ => random.Next()).AsEnumerable())
                                        .Aggregate((g1, g2) => g1.Concat(g2))
                                        .Take(numberOfLocationHints)
                                        .ToList();

                unusedItems.AddRange(unusedItems);
                var importantRegionCounts = new Dictionary<Region, int>();
                var nonImportantRegionCounts = new Dictionary<Region, int>();
                var songOnlyRegionCounts = new Dictionary<Region, int>();
                var clockTownRegionCounts = new Dictionary<Region, int>();
                foreach (var kvp in itemsInRegions)
                {
                    var numberOfRequiredItems = kvp.Value.Count(io => ItemUtils.IsRequired(io.Item, randomizedResult) && !unusedItems.Contains(io));
                    var numberOfImportantItems = kvp.Value.Count(io => ItemUtils.IsImportant(io.Item, randomizedResult));

                    if (numberOfRequiredItems == 0 && numberOfImportantItems > 0)
                    {
                        continue;
                    }

                    Dictionary<Region, int> dict;
                    if (numberOfRequiredItems == 0)
                    {
                        dict = nonImportantRegionCounts;
                    }
                    else if (Gossip.ClockTownRegions.Contains(kvp.Key))
                    {
                        dict = clockTownRegionCounts;
                    }
                    else if (!randomizedResult.Settings.AddSongs && kvp.Value.Count(io => ItemUtils.IsRequired(io.Item, randomizedResult) && !ItemUtils.IsSong(io.Item) && !unusedItems.Contains(io)) == 0)
                    {
                        dict = songOnlyRegionCounts;
                    }
                    else
                    {
                        dict = importantRegionCounts;
                    }
                    
                    dict[kvp.Key] = numberOfRequiredItems;
                }

                var chosenSongOnlyRegions = 0;
                var chosenClockTownRegions = 0;
                for (var i = 0; i < numberOfRequiredHints; i++)
                {
                    var regionCounts = importantRegionCounts.AsEnumerable();
                    if (chosenClockTownRegions < maxNumberOfClockTownHints)
                    {
                        regionCounts = regionCounts.Concat(clockTownRegionCounts);
                    }
                    if (chosenSongOnlyRegions < maxNumberOfSongOnlyHints)
                    {
                        regionCounts = regionCounts.Concat(songOnlyRegionCounts);
                    }
                    if (!regionCounts.Any())
                    {
                        regionCounts = regionCounts.Concat(clockTownRegionCounts);
                    //}
                    //if (!regionCounts.Any())
                    //{
                        regionCounts = regionCounts.Concat(songOnlyRegionCounts);
                    }
                    if (regionCounts.Any())
                    {
                        var chosen = regionCounts.ToList().Random(random);
                        competitiveHints.Add(BuildRegionHint(chosen, random));
                        competitiveHints.Add(BuildRegionHint(chosen, random));
                        if (songOnlyRegionCounts.Remove(chosen.Key))
                        {
                            chosenSongOnlyRegions++;
                        }
                        else if (clockTownRegionCounts.Remove(chosen.Key))
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
                    if (nonImportantRegionCounts.Any())
                    {
                        var chosen = nonImportantRegionCounts.ToList().Random(random);
                        competitiveHints.Add(BuildRegionHint(chosen, random));
                        competitiveHints.Add(BuildRegionHint(chosen, random));
                        nonImportantRegionCounts.Remove(chosen.Key);
                    }
                }
            }

            List<MessageEntry> finalHints = new List<MessageEntry>();

            foreach (var gossipQuote in Enum.GetValues(typeof(GossipQuote)).Cast<GossipQuote>().OrderBy(gq => random.Next()))
            {
                string messageText = null;
                var isMoonGossipStone = gossipQuote.IsMoonGossipStone();
                if (!isMoonGossipStone && competitiveHints.Any())
                {
                    messageText = competitiveHints.Random(random);
                    competitiveHints.Remove(messageText);
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
                                ? randomizedResult.ItemList.Single(io => io.Item == chosen.Item)
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
                                item = unusedItems.FirstOrDefault(io => unusedItems.Count(x => x.Item == io.Item) == 1);
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
                        ushort soundEffectId = 0x690C; // grandma curious
                        string itemName = null;
                        string locationName = null;
                        if (forceClear || randomizedResult.Settings.ClearHints)
                        {
                            itemName = item.Item.Name();
                            locationName = item.NewLocation.Value.Location();
                        }
                        else
                        {
                            if (isMoonGossipStone || randomizedResult.Settings.GossipHintStyle == GossipHintStyle.Competitive || random.Next(100) >= 5) // 5% chance of fake/junk hint if it's not a moon gossip stone or competitive style
                            {
                                itemName = item.Item.ItemHints().Random(random);
                                locationName = item.NewLocation.Value.LocationHints().Random(random);
                            }
                            else
                            {
                                if (random.Next(2) == 0) // 50% chance for fake hint. otherwise default to junk hint.
                                {
                                    soundEffectId = 0x690A; // grandma laugh
                                    itemName = item.Item.ItemHints().Random(random);
                                    locationName = randomizedItems.Random(random).Item.LocationHints().Random(random);
                                }
                            }
                        }
                        if (itemName != null && locationName != null)
                        {
                            messageText = BuildGossipQuote(soundEffectId, locationName, itemName, random);
                        }
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

        private static string BuildRegionHint(KeyValuePair<Region, int> regionInfo, Random random)
        {
            var region = regionInfo.Key;
            var numberOfRequiredItems = regionInfo.Value;

            ushort soundEffectId = 0x690C; // grandma curious
            string start = Gossip.MessageStartSentences.Random(random);

            string sfx = $"{(char)((soundEffectId >> 8) & 0xFF)}{(char)(soundEffectId & 0xFF)}";
            var locationMessage = region.Name();
            var mid = "is";
            var itemMessage = numberOfRequiredItems > 0
                ? "on the Way of the Hero"
                : "a foolish choice";
            char color;
            if (numberOfRequiredItems > 0)
            {
                color = TextCommands.ColorYellow;
            }
            else
            {
                color = TextCommands.ColorSilver;
            }

            return $"\x1E{sfx}{start} {color}{locationMessage}{TextCommands.ColorWhite} {mid} {itemMessage}...\xBF".Wrap(35, "\x11");

            //var mid = "has";
            //return $"\x1E{sfx}{start} {TextCommands.ColorRed}{locationMessage}{TextCommands.ColorWhite} {mid} {color}{NumberToWords(numberOfImportantItems)} important item{(numberOfImportantItems == 1 ? "" : "s")}{TextCommands.ColorWhite}...\xBF".Wrap(35, "\x11");
        }

        private static string BuildGossipQuote(ushort soundEffectId, string locationMessage, string itemMessage, Random random)
        {
            int startIndex = random.Next(Gossip.MessageStartSentences.Count);
            int midIndex = random.Next(Gossip.MessageMidSentences.Count);
            string start = Gossip.MessageStartSentences[startIndex];
            string mid = Gossip.MessageMidSentences[midIndex];

            string sfx = $"{(char)((soundEffectId >> 8) & 0xFF)}{(char)(soundEffectId & 0xFF)}";

            return $"\x1E{sfx}{start} \x01{locationMessage}\x00 {mid} \x06{itemMessage}\x00...\xBF".Wrap(35, "\x11");
        }

        public static string BuildShopDescriptionMessage(string title, int cost, string description)
        {
            return $"\x01{title}: {cost} Rupees\x11\x00{description.Wrap(35, "\x11")}\x1A\xBF";
        }

        public static string BuildShopPurchaseMessage(string title, int cost, Item item)
        {
            return $"{title}: {cost} Rupees\x11 \x11\x02\xC2I'll buy {GetPronoun(item)}\x11No thanks\xBF";
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

        public static string GetPronounOrAmount(Item item, string it = " It")
        {
            var shopTexts = item.ShopTexts();
            var itemAmount = Regex.Replace(item.Name(), "[^0-9]", "");
            return shopTexts.IsMultiple
                ? string.IsNullOrWhiteSpace(itemAmount)
                    ? it
                    : " " + itemAmount
                : shopTexts.IsDefinite
                    ? it
                    : " One";
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

        public static string GetAlternateName(Item item)
        {
            return Regex.Replace(item.Name(), "[0-9]+ ", "");
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