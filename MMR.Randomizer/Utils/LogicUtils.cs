using MMR.Common.Extensions;
using MMR.Randomizer.Attributes;
using MMR.Randomizer.Extensions;
using MMR.Randomizer.GameObjects;
using MMR.Randomizer.LogicMigrator;
using MMR.Randomizer.Models;
using MMR.Randomizer.Models.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace MMR.Randomizer.Utils
{
    public static class LogicUtils
    {
        public static LogicFile ReadRulesetFromResources(LogicMode mode, string userLogicFileName)
        {
            if (mode == LogicMode.Casual)
            {
                return LogicFile.FromJson(Properties.Resources.REQ_CASUAL);
            }
            else if (mode == LogicMode.Glitched)
            {
                return LogicFile.FromJson(Properties.Resources.REQ_GLITCH);
            }
            else if (mode == LogicMode.UserLogic)
            {
                using (StreamReader Req = new StreamReader(File.OpenRead(userLogicFileName)))
                {
                    var logic = Req.ReadToEnd();

                    return LogicFile.FromJson(logic);

                    // TODO handle logic within settings file
                }
            }

            return null;
        }

        /// <summary>
        /// Populates the item list using the lines from a logic file, processes them 4 lines per item. 
        /// </summary>
        /// <param name="data">The lines from a logic file</param>
        public static ItemList PopulateItemListFromLogicData(LogicFile logicFile)
        {
            var itemList = new ItemList();
            if (logicFile.Version != Migrator.CurrentVersion)
            {
                throw new Exception("Logic file is out of date or invalid. Open it in the Logic Editor to bring it up to date.");
            }

            var logic = logicFile.Logic;
            for (var i = 0; i < logic.Count; i++)
            {
                var logicItem = logic[i];
                itemList.Add(new ItemObject
                {
                    ID = i,
                    Name = logicItem.Id,
                    TimeNeeded = (int)logicItem.TimeNeeded,
                    TimeAvailable = logicItem.TimeAvailable > 0 ? (int)logicItem.TimeAvailable : 63,
                    TimeSetup = (int)logicItem.TimeSetup,
                    IsTrick = logicItem.IsTrick,
                    TrickTooltip = logicItem.TrickTooltip,
                    DependsOnItems = logicItem.RequiredItems.Select(item => (Item)logic.FindIndex(li => li.Id == item)).ToList(),
                    Conditionals = logicItem.ConditionalItems.Select(c => c.Select(item => (Item)logic.FindIndex(li => li.Id == item)).ToList()).ToList(),
                    TrickCategory = logicItem.TrickCategory
                });
            }

            foreach (var io in itemList)
            {
                if (io.DependsOnItems.Any(item => itemList[item].IsTrick))
                {
                    throw new Exception($"Dependencies of {io.Name} are not valid. Cannot have tricks as Dependencies.");
                }
                if (io.Conditionals.Any() && io.Conditionals.All(c => c.Any(item => itemList[item].IsTrick)))
                {
                    throw new Exception($"Conditionals of {io.Name} are not valid. Must have at least one conditional that isn't a trick.");
                }
            }

            return itemList;
        }

        /// <summary>
        /// Populates item list without logic. Default TimeAvailable = 63
        /// </summary>
        public static ItemList PopulateItemListWithoutLogic()
        {
            var itemList = new ItemList();
            foreach (var item in Enum.GetValues<Item>())
            {
                if (item < 0)
                {
                    continue;
                }

                var currentItem = new ItemObject
                {
                    ID = (int)item,
                    Name = item.Name() ?? item.ToString(),
                    TimeAvailable = 63
                };

                itemList.Add(currentItem);
            }
            return itemList;
        }

        public static Dictionary<GossipQuote, ReadOnlyCollection<Item>> GetGossipStoneRequirements(ItemList itemList, List<ItemLogic> logic, GameplaySettings settings)
        {
            return Enum.GetValues(typeof(GossipQuote))
                .Cast<GossipQuote>()
                .Where(gq => gq.HasAttribute<GossipStoneAttribute>())
                .ToDictionary(gq => gq, gq => GetGossipStoneRequirement(gq, itemList, logic, settings));
        }

        public static ReadOnlyCollection<Item> GetGossipStoneRequirement(GossipQuote gossipQuote, ItemList itemList, List<ItemLogic> logic, GameplaySettings settings)
        {
            var gossipStoneItem = gossipQuote.GetAttribute<GossipStoneAttribute>().Item;
            return GetImportantLocations(itemList, settings, gossipStoneItem, logic).Required;
        }

        public class LogicPaths
        {
            public ReadOnlyCollection<Item> Required { get; set; }
            public ReadOnlyCollection<Item> Important { get; set; }
            public ReadOnlyCollection<Item> ImportantSongLocations { get; set; }
        }

        public static LogicPaths GetImportantLocations(ItemList itemList, GameplaySettings settings, Item location, List<ItemLogic> itemLogic, List<Item> logicPath = null, Dictionary<Item, LogicPaths> checkedLocations = null, params Item[] exclude)
        {
            var itemObject = itemList.Find(io => io.NewLocation == location) ?? itemList[location];
            if (settings.CustomStartingItemList.Contains(itemObject.Item))
            {
                return new LogicPaths();
            }
            if (logicPath == null)
            {
                logicPath = new List<Item>();
            }
            if (logicPath.Contains(location))
            {
                return null;
            }
            if (exclude.Contains(location))
            {
                if (settings.AddSongs || !ItemUtils.IsSong(location) || logicPath.Any(i => !i.IsFake() && itemList[i].IsRandomized && !ItemUtils.IsRegionRestricted(settings, i) && !ItemUtils.IsSong(i)))
                {
                    return null;
                }
            }
            var importantSongLocations = new List<Item>();
            if (!settings.AddSongs && ItemUtils.IsSong(location) && logicPath.Any(i => !i.IsFake() && itemList[i].IsRandomized && !ItemUtils.IsRegionRestricted(settings, i)))
            {
                importantSongLocations.Add(location);
            }
            logicPath.Add(location);
            if (checkedLocations == null)
            {
                checkedLocations = new Dictionary<Item, LogicPaths>();
            }
            if (checkedLocations.ContainsKey(location))
            {
                if (logicPath.Intersect(checkedLocations[location].Required).Any())
                {
                    return null;
                }
                if (!exclude.Intersect(checkedLocations[location].Required).Any())
                {
                    return checkedLocations[location];
                }
            }
            var locationLogic = itemLogic[(int)location];
            var required = new List<Item>();
            var important = new List<Item>();
            if (locationLogic.RequiredItemIds != null && locationLogic.RequiredItemIds.Any())
            {
                foreach (var requiredItemId in locationLogic.RequiredItemIds.Cast<Item>())
                {
                    if (itemList[requiredItemId].Item != requiredItemId)
                    {
                        continue;
                    }

                    var requiredLocation = itemList[requiredItemId].NewLocation ?? requiredItemId;

                    var childPaths = GetImportantLocations(itemList, settings, requiredLocation, itemLogic, logicPath.ToList(), checkedLocations, exclude);
                    if (childPaths == null)
                    {
                        return null;
                    }

                    required.Add(requiredLocation);
                    important.Add(requiredLocation);
                    if (childPaths.Required != null)
                    {
                        required.AddRange(childPaths.Required);
                    }
                    if (childPaths.Important != null)
                    {
                        important.AddRange(childPaths.Important);
                    }
                    if (childPaths.ImportantSongLocations != null)
                    {
                        importantSongLocations.AddRange(childPaths.ImportantSongLocations);
                    }
                }
            }
            if (locationLogic.ConditionalItemIds != null && locationLogic.ConditionalItemIds.Any())
            {
                var logicPaths = new List<LogicPaths>();
                foreach (var conditions in locationLogic.ConditionalItemIds)
                {
                    var conditionalRequired = new List<Item>();
                    var conditionalImportant = new List<Item>();
                    var conditionalImportantSongLocations = new List<Item>();
                    foreach (var conditionalItemId in conditions.Cast<Item>())
                    {
                        if (itemList[conditionalItemId].Item != conditionalItemId)
                        {
                            continue;
                        }

                        var conditionalLocation = itemList[conditionalItemId].NewLocation ?? conditionalItemId;

                        var childPaths = GetImportantLocations(itemList, settings, conditionalLocation, itemLogic, logicPath.ToList(), checkedLocations, exclude);
                        if (childPaths == null)
                        {
                            conditionalRequired = null;
                            conditionalImportant = null;
                            break;
                        }

                        conditionalRequired.Add(conditionalLocation);
                        conditionalImportant.Add(conditionalLocation);
                        if (childPaths.Required != null)
                        {
                            conditionalRequired.AddRange(childPaths.Required);
                        }
                        if (childPaths.Important != null)
                        {
                            conditionalImportant.AddRange(childPaths.Important);
                        }
                        if (childPaths.ImportantSongLocations != null)
                        {
                            conditionalImportantSongLocations.AddRange(childPaths.ImportantSongLocations);
                        }
                    }

                    if (conditionalRequired != null && conditionalImportant != null)
                    {
                        logicPaths.Add(new LogicPaths
                        {
                            Required = conditionalRequired.AsReadOnly(),
                            Important = conditionalImportant.AsReadOnly(),
                            ImportantSongLocations = conditionalImportantSongLocations.AsReadOnly()
                        });
                    }
                }
                if (!logicPaths.Any())
                {
                    return null;
                }

                // Hopefully this makes item importance a little smarter.
                var shouldRemove = new List<int>();
                for (var i = 0; i < logicPaths.Count; i++)
                {
                    var currentLogicPath = logicPaths[i];
                    var currentLogicImportant = currentLogicPath.Important.Except(important);
                    for (var j = 0; j < logicPaths.Count; j++)
                    {
                        if (i != j && !shouldRemove.Contains(i) && !shouldRemove.Contains(j))
                        {
                            var otherLogicPath = logicPaths[j];
                            var otherLogicImportant = otherLogicPath.Important.Except(important);
                            if (!currentLogicImportant.Except(otherLogicImportant).Any() && otherLogicImportant.Except(currentLogicImportant).Any())
                            {
                                shouldRemove.Add(j);
                            }
                        }
                    }
                }
                foreach (var index in shouldRemove.OrderByDescending(x => x))
                {
                    logicPaths.RemoveAt(index);
                }

                required.AddRange(logicPaths.Select(lp => lp.Required.AsEnumerable()).Aggregate((a, b) => a.Intersect(b)));
                important.AddRange(logicPaths.SelectMany(lp => lp.Required.Union(lp.Important)).Distinct());
                importantSongLocations.AddRange(logicPaths.SelectMany(lp => lp.ImportantSongLocations).Distinct());
            }
            var result = new LogicPaths
            {
                Required = required.Distinct().ToList().AsReadOnly(),
                Important = important.Union(required).Distinct().ToList().AsReadOnly(),
                ImportantSongLocations = importantSongLocations.Distinct().ToList().AsReadOnly()
            };
            if (!location.IsFake())
            {
                checkedLocations[location] = result;
            }
            return result;
        }
    }
}
