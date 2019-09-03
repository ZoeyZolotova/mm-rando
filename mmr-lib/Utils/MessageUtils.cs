using MMRando.Attributes;
using MMRando.Extensions;
using MMRando.GameObjects;
using MMRando.Models;
using MMRando.Models.Rom;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MMRando.Utils
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

            var randomizedItems = new List<ItemObject>();
            var competitiveHints = new List<string>();
            var itemsInRegions = new Dictionary<string, List<ItemObject>>();
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
                    && (randomizedResult.Settings.ClearHints || randomizedResult.Random.Next(8) != 0))
                {
                    continue;
                }

                if (randomizedResult.Settings.GossipHintStyle == GossipHintStyle.Competitive)
                {
                    var preventRegions = new List<string> { "The Moon", "Bottle Catch", "Misc" };
                    var itemRegion = item.NewLocation.Value.Region();
                    if (!string.IsNullOrWhiteSpace(itemRegion) && !preventRegions.Contains(itemRegion) && (randomizedResult.Settings.AddSongs || !ItemUtils.IsSong(item.Item)))
                    {
                        if (!itemsInRegions.ContainsKey(itemRegion))
                        {
                            itemsInRegions[itemRegion] = new List<ItemObject>();
                        }
                        itemsInRegions[itemRegion].Add(item);
                    }

                    if (!Gossip.GuaranteedLocationHints.Contains(item.NewLocation.Value))
                    {
                        continue;
                    }
                }

                randomizedItems.Add(item);
            }

            var unusedItems = randomizedItems.ToList();

            if (randomizedResult.Settings.GossipHintStyle == GossipHintStyle.Competitive)
            {
                unusedItems.AddRange(randomizedItems);
                var requiredHints = new List<string>();
                var nonRequiredHints = new List<string>();
                foreach (var kvp in itemsInRegions)
                {
                    bool regionHasRequiredItem;
                    if (kvp.Value.Any(io => randomizedResult.ItemsRequiredForMoonAccess.Contains(io.Item)))
                    {
                        regionHasRequiredItem = true;
                    }
                    else if (!kvp.Value.Any(io => randomizedResult.AllItemsOnPathToMoon.Contains(io.Item)))
                    {
                        regionHasRequiredItem = false;
                    }
                    else
                    {
                        continue;
                    }

                    ushort soundEffectId = 0x690C; // grandma curious
                    string start = Gossip.MessageStartSentences.Random(randomizedResult.Random);

                    string sfx = $"{(char)((soundEffectId >> 8) & 0xFF)}{(char)(soundEffectId & 0xFF)}";
                    var locationMessage = kvp.Key;
                    var mid = "is";
                    var itemMessage = regionHasRequiredItem
                        ? "on the Way of the Hero"
                        : "a foolish choice";
                    var list = regionHasRequiredItem
                        ? requiredHints
                        : nonRequiredHints;

                    list.Add($"\x1E{sfx}{start} \x01{locationMessage}\x00 {mid} \x06{itemMessage}\x00...\xBF".Wrap(35, "\x11"));
                }

                var numberOfRequiredHints = 3;
                var numberOfNonRequiredHints = 2;

                for (var i = 0; i < numberOfRequiredHints; i++)
                {
                    var chosen = requiredHints.RandomOrDefault(randomizedResult.Random);
                    if (chosen != null)
                    {
                        requiredHints.Remove(chosen);
                        competitiveHints.Add(chosen);
                        competitiveHints.Add(chosen);
                    }
                }

                for (var i = 0; i < numberOfNonRequiredHints; i++)
                {
                    var chosen = nonRequiredHints.RandomOrDefault(randomizedResult.Random);
                    if (chosen != null)
                    {
                        nonRequiredHints.Remove(chosen);
                        competitiveHints.Add(chosen);
                        competitiveHints.Add(chosen);
                    }
                }
            }

            List<MessageEntry> finalHints = new List<MessageEntry>();

            foreach (var gossipQuote in Enum.GetValues(typeof(GossipQuote)).Cast<GossipQuote>().OrderBy(gq => randomizedResult.Random.Next()))
            {
                var isMoonGossipStone = gossipQuote.ToString().StartsWith("Moon");
                var restrictionAttributes = gossipQuote.GetAttributes<GossipRestrictAttribute>().ToList();
                ItemObject item = null;
                var forceClear = false;
                while (item == null)
                {
                    if (restrictionAttributes.Any() && (isMoonGossipStone || randomizedResult.Settings.GossipHintStyle == GossipHintStyle.Relevant))
                    {
                        var chosen = restrictionAttributes.Random(randomizedResult.Random);
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
                        item = unusedItems.Random(randomizedResult.Random);
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

                string messageText = null;
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
                        if (isMoonGossipStone || randomizedResult.Settings.GossipHintStyle == GossipHintStyle.Competitive || randomizedResult.Random.Next(100) >= 5) // 5% chance of fake/junk hint if it's not a moon gossip stone or competitive style
                        {
                            itemName = item.Item.ItemHints().Random(randomizedResult.Random);
                            locationName = item.NewLocation.Value.LocationHints().Random(randomizedResult.Random);
                        }
                        else
                        {
                            if (randomizedResult.Random.Next(2) == 0) // 50% chance for fake hint. otherwise default to junk hint.
                            {
                                soundEffectId = 0x690A; // grandma laugh
                                itemName = item.Item.ItemHints().Random(randomizedResult.Random);
                                locationName = randomizedItems.Random(randomizedResult.Random).Item.LocationHints().Random(randomizedResult.Random);
                            }
                        }
                    }
                    if (itemName != null && locationName != null)
                    {
                        messageText = BuildGossipQuote(soundEffectId, locationName, itemName, randomizedResult.Random);
                    }
                }
                if (messageText == null)
                {
                    if (competitiveHints.Any())
                    {
                        messageText = competitiveHints.Random(randomizedResult.Random);
                        competitiveHints.Remove(messageText);
                    }
                    else
                    {
                        messageText = Gossip.JunkMessages.Random(randomizedResult.Random);
                    }
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

        public static string BuildShopPurchaseMessage(string title, int cost, bool isMultiple)
        {
            return $"{title}: {cost} Rupees\x11 \x11\x02\xC2I'll buy {(isMultiple ? "them" : "it")}\x11No thanks\xBF";
        }
    }
}