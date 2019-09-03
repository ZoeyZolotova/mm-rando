using MMRando.Constants;
using MMRando.Extensions;
using MMRando.LogicMigrator;
using MMRando.Models;
using MMRando.Models.Rom;
using MMRando.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using MMRando.GameObjects;

namespace MMRando
{

    public class Randomizer
    {
        private Random _random { get; set; }
        public Random Random
        {
            get => _random;
            set => _random = value;
        }

        public List<ItemObject> ItemList { get; set; }

        #region Dependence and Conditions
        List<Item> ConditionsChecked { get; set; }
        Dictionary<Item, Dependence> DependenceChecked { get; set; }
        List<int[]> ConditionRemoves { get; set; }

        private class Dependence
        {
            public Item[] Items { get; set; }
            public DependenceType Type { get; set; }

            public static Dependence Dependent => new Dependence { Type = DependenceType.Dependent };
            public static Dependence NotDependent => new Dependence { Type = DependenceType.NotDependent };
            public static Dependence Circular(params Item[] items) => new Dependence { Items = items, Type = DependenceType.Circular };
        }

        private enum DependenceType
        {
            Dependent,
            NotDependent,
            Circular
        }

        // Starting items should not be replaced by trade items, or items that can be downgraded.
        private readonly List<Item> ForbiddenStartingItems = new List<Item>
            {
                Item.ChestMountainVillageGrottoBottle,

                // Starting with Magic Bean or Powder Keg doesn't actually give you one,
                // nor do you get one when you play Song of Time.
                Item.ItemMagicBean,
                Item.ItemPowderKeg,
            }
            .Concat(Enumerable.Range((int)Item.TradeItemMoonTear, Item.TradeItemMamaLetter - Item.TradeItemMoonTear + 1).Cast<Item>())
            .Concat(Enumerable.Range((int)Item.ItemBottleWitch, Item.ItemBottleMadameAroma - Item.ItemBottleWitch + 1).Cast<Item>())
            .ToList();

        private readonly Dictionary<Item, List<Item>> ForbiddenReplacedBy = new Dictionary<Item, List<Item>>
        {
            // Keaton_Mask and Mama_Letter are obtained one directly after another
            // Keaton_Mask cannot be replaced by items that may be overwritten by item obtained at Mama_Letter
            {
                Item.MaskKeaton,
                new List<Item> {
                    Item.TradeItemMoonTear,
                    Item.TradeItemLandDeed,
                    Item.TradeItemSwampDeed,
                    Item.TradeItemMountainDeed,
                    Item.TradeItemOceanDeed,
                    Item.TradeItemRoomKey,
                    Item.TradeItemMamaLetter,
                    Item.TradeItemKafeiLetter,
                    Item.TradeItemPendant
                }
            },
        };

        private readonly Dictionary<Item, List<Item>> ForbiddenPlacedAt = new Dictionary<Item, List<Item>>
        {
        };

        #endregion

        private Settings _settings;
        private RandomizedResult _randomized;

        public Randomizer(Settings settings)
        {
            _settings = settings;
            if (!_settings.PreventDowngrades)
            {
                ForbiddenReplacedBy[Item.MaskKeaton].AddRange(ItemUtils.DowngradableItems());
                ForbiddenStartingItems.AddRange(ItemUtils.DowngradableItems());
            }
        }

        //rando functions

        #region Gossip quotes

        private void MakeGossipQuotes()
        {
            _randomized.GossipQuotes = MessageUtils.MakeGossipQuotes
                (_randomized);
        }

        #endregion

        private void EntranceShuffle()
        {
            var newDCFlags = new int[] { -1, -1, -1, -1 };
            var newDCMasks = new int[] { -1, -1, -1, -1 };
            var newEntranceIndices = new int[] { -1, -1, -1, -1 };
            var newExitIndices = new int[] { -1, -1, -1, -1 };

            for (int i = 0; i < 4; i++)
            {
                int n;
                do
                {
                    n = Random.Next(4);
                } while (newEntranceIndices.Contains(n));

                newEntranceIndices[i] = n;
                newExitIndices[n] = i;
            }

            var areaAccessObjects = new ItemObject[] {
                ItemList[(int)Item.AreaWoodFallTempleAccess],
                ItemList[(int)Item.AreaSnowheadTempleAccess],
                ItemList[(int)Item.AreaInvertedStoneTowerTempleAccess],
                ItemList[(int)Item.AreaGreatBayTempleAccess]
            };

            var areaAccessObjectIndexes = new int[] {
                (int)Item.AreaWoodFallTempleAccess,
                (int)Item.AreaSnowheadTempleAccess,
                (int)Item.AreaInvertedStoneTowerTempleAccess,
                (int)Item.AreaGreatBayTempleAccess
            };

            for (int i = 0; i < 4; i++)
            {
                //Debug.WriteLine($"Entrance {Item.ITEM_NAMES[areaAccessObjectIndexes[newEntranceIndices[i]]]} placed at {Item.ITEM_NAMES[areaAccessObjects[i].ID]}.");
                areaAccessObjects[i].IsRandomized = true;
                ItemList[areaAccessObjectIndexes[newEntranceIndices[i]]] = areaAccessObjects[i];
            }

            var areaClearObjects = new ItemObject[] {
                ItemList[(int)Item.AreaWoodFallTempleClear],
                ItemList[(int)Item.AreaSnowheadTempleClear],
                ItemList[(int)Item.AreaStoneTowerClear],
                ItemList[(int)Item.AreaGreatBayTempleClear]
            };

            var areaClearObjectIndexes = new int[] {
                (int)Item.AreaWoodFallTempleClear,
                (int)Item.AreaSnowheadTempleClear,
                (int)Item.AreaStoneTowerClear,
                (int)Item.AreaGreatBayTempleClear
            };

            for (int i = 0; i < 4; i++)
            {
                ItemList[areaClearObjectIndexes[i]] = areaClearObjects[newEntranceIndices[i]];
            }

            var newEntrances = new int[] { -1, -1, -1, -1 };
            var newExits = new int[] { -1, -1, -1, -1 };

            for (int i = 0; i < 4; i++)
            {
                newEntrances[i] = Values.OldEntrances[newEntranceIndices[i]];
                newExits[i] = Values.OldExits[newExitIndices[i]];
                newDCFlags[i] = Values.OldDCFlags[newExitIndices[i]];
                newDCMasks[i] = Values.OldMaskFlags[newExitIndices[i]];
            }

            _randomized.NewEntrances = newEntrances;
            _randomized.NewDestinationIndices = newEntranceIndices;
            _randomized.NewExits = newExits;
            _randomized.NewExitIndices = newExitIndices;
            _randomized.NewDCFlags = newDCFlags;
            _randomized.NewDCMasks = newDCMasks;
        }

        #region Sequences and BGM

        private void ShuffleBGM()
        {
            var sequences = SequenceUtils.ReadSequenceInfo();

            var targetSequences = new List<SequenceInfo>();

            foreach (var item in sequences.Where(x => x.Replaceable))
            {
                SequenceInfo targetSequence = new SequenceInfo
                {
                    Name = item.Name,
                    Type = item.Type,
                    Instrument = item.Instrument,
                    Replaces = item.SequenceId
                };

                targetSequences.Add(targetSequence);
            }

            sequences.RemoveAll(x => x.SequenceId == 0x18); //File Select, which is a duplicated entry

            while (targetSequences.Count > 0)
            {
                List<SequenceInfo> Unassigned = sequences.FindAll(u => u.Replaces == -1);

                int targetIndex = Random.Next(targetSequences.Count);
                var targetSequence = targetSequences[targetIndex];

                while (true)
                {
                    int unassignedIndex = Random.Next(Unassigned.Count);
                    var nextSequence = Unassigned[unassignedIndex];

                    if (nextSequence.Name.StartsWith("mm")
                        & (Random.Next(100) < 50))
                    {
                        continue;
                    }

                    for (int i = 0; i < nextSequence.Type.Count; i++)
                    {
                        if (targetSequence.Type.Contains(nextSequence.Type[i]))
                        {
                            nextSequence.Replaces = targetSequence.Replaces;
                            Debug.WriteLine(nextSequence.Name + " -> " + targetSequence.Name);
                            targetSequences.RemoveAt(targetIndex);
                            break;
                        }
                        else if (i + 1 == nextSequence.Type.Count)
                        {
                            if ((Random.Next(30) == 0)
                                && ((nextSequence.Type[0] & 8) == (targetSequence.Type[0] & 8))
                                && (nextSequence.Type.Contains(10) == targetSequence.Type.Contains(10))
                                && (!nextSequence.Type.Contains(16)))
                            {
                                nextSequence.Replaces = targetSequence.Replaces;
                                Debug.WriteLine(nextSequence.Name + " -> " + targetSequence.Name);
                                targetSequences.RemoveAt(targetIndex);
                                break;
                            }
                        }
                    }

                    if (nextSequence.Replaces != -1)
                    {
                        break;
                    }
                }
            }

            sequences.RemoveAll(u => u.Replaces == -1);
            _randomized.Sequences = sequences;
        }

        #endregion

        private void SetTatlColour()
        {
            for (int i = 0; i < 10; i++)
            {
                byte[] c = new byte[4];
                Random.NextBytes(c);

                if ((i % 2) == 0)
                {
                    c[0] = 0xFF;
                }
                else
                {
                    c[0] = 0;
                }

                Values.TatlColours[4, i] = BitConverter.ToUInt32(c, 0);
            }
        }

        private void PrepareRulesetItemData()
        {
            ItemList = new List<ItemObject>();

            if (_settings.LogicMode == LogicMode.Casual
                || _settings.LogicMode == LogicMode.GlitchedNoSetups
                || _settings.LogicMode == LogicMode.GlitchedCommonTricks
                || _settings.LogicMode == LogicMode.Glitched
                || _settings.LogicMode == LogicMode.User)
            {
                string[] data = ReadRulesetFromResources();
                PopulateItemListFromLogicData(data);
            }
            else
            {
                PopulateItemListWithoutLogic();
            }
        }

        /// <summary>
        /// Populates item list without logic. Default TimeAvailable = 63
        /// </summary>
        private void PopulateItemListWithoutLogic()
        {
            foreach (var item in Enum.GetValues(typeof(Item)).Cast<Item>())
            {
                var currentItem = new ItemObject
                {
                    ID = (int)item,
                    Name = item.Name() ?? item.ToString(),
                    TimeAvailable = 63
                };

                ItemList.Add(currentItem);
            }
        }

        /// <summary>
        /// Populates the item list using the lines from a logic file, processes them 4 lines per item. 
        /// </summary>
        /// <param name="data">The lines from a logic file</param>
        private void PopulateItemListFromLogicData(string[] data)
        {
            if (Migrator.GetVersion(data.ToList()) != Migrator.CurrentVersion)
            {
                throw new InvalidDataException("Logic file is out of date. Open it in the Logic Editor to bring it up to date.");
            }

            int itemId = 0;
            int lineNumber = 0;

            var currentItem = new ItemObject();

            // Process lines in groups of 4
            foreach (string line in data)
            {
                if (line.Contains("-"))
                {
                    currentItem.Name = line.Substring(2);
                    continue;
                }

                switch (lineNumber)
                {
                    case 0:
                        //dependence
                        ProcessDependenciesForItem(currentItem, line);
                        break;
                    case 1:
                        //conditionals
                        ProcessConditionalsForItem(currentItem, line);
                        break;
                    case 2:
                        //time needed
                        currentItem.TimeNeeded = Convert.ToInt32(line);
                        break;
                    case 3:
                        //time available
                        currentItem.TimeAvailable = Convert.ToInt32(line);
                        if (currentItem.TimeAvailable == 0)
                        {
                            currentItem.TimeAvailable = 63;
                        }
                        break;
                }

                lineNumber++;

                if (lineNumber == 4)
                {
                    currentItem.ID = itemId;
                    ItemList.Add(currentItem);

                    currentItem = new ItemObject();

                    itemId++;
                    lineNumber = 0;
                }
            }
        }

        private void ProcessConditionalsForItem(ItemObject currentItem, string line)
        {
            List<List<Item>> conditional = new List<List<Item>>();

            if (line == "")
            {
                currentItem.Conditionals = null;
            }
            else
            {
                foreach (string conditions in line.Split(';'))
                {
                    currentItem.Conditionals.Add(Array.ConvertAll(conditions.Split(','), int.Parse).Select(i => (Item)i).ToList());
                }
            }
        }

        private void ProcessDependenciesForItem(ItemObject currentItem, string line)
        {
            List<Item> dependencies = new List<Item>();

            if (line == "")
            {
                currentItem.DependsOnItems = null;
            }
            else
            {
                foreach (string dependency in line.Split(','))
                {
                    currentItem.DependsOnItems.Add((Item)Convert.ToInt32(dependency));
                }
            }
        }

        public void SeedRNG()
        {
            //generate the "public" seed
            string seed = _settings.Seed.Trim();
            if (string.IsNullOrWhiteSpace(seed))
            {
                seed = Environment.TickCount.ToString();
            }
            _settings.Seed = seed;

            Random = new Random(BitConverter.ToInt32(_settings.GetGenerationSettingsHash(), 8));
        }

        private string[] ReadRulesetFromResources()
        {
            string[] lines = null;
            var mode = _settings.LogicMode;

            if (_settings.LogicMode == LogicMode.User)
            {
                return LogicUtil.GetLogic(_settings.UserLogicFileName);
            }
            else
                return LogicUtil.GetLogic(mode);
        }


        private Dependence CheckDependence(Item currentItem, Item target, List<Item> dependencyPath)
        {
            Debug.WriteLine($"CheckDependence({currentItem}, {target})");
            if (ItemList[(int)currentItem].TimeNeeded == 0
                && !ItemList.Any(io => (io.Conditionals?.Any(c => c.Contains(currentItem)) ?? false) || (io.DependsOnItems?.Contains(currentItem) ?? false)))
            {
                return Dependence.NotDependent;
            }

            //check timing
            if (ItemList[(int)currentItem].TimeNeeded != 0 && dependencyPath.Skip(1).All(p => p.IsFake() || ItemList.Single(i => i.NewLocation == p).Item.IsTemporary()))
            {
                if ((ItemList[(int)currentItem].TimeNeeded & ItemList[(int)target].TimeAvailable) == 0)
                {
                    Debug.WriteLine($"{currentItem} is needed at {ItemList[(int)currentItem].TimeNeeded} but {target} is only available at {ItemList[(int)target].TimeAvailable}");
                    return Dependence.Dependent;
                }
            }

            if (ItemList[(int)target].HasConditionals)
            {
                if (ItemList[(int)target].Conditionals
                    .FindAll(u => u.Contains(currentItem)).Count == ItemList[(int)target].Conditionals.Count)
                {
                    Debug.WriteLine($"All conditionals of {target} contains {currentItem}");
                    return Dependence.Dependent;
                }

                if (ItemList[(int)currentItem].HasCannotRequireItems)
                {
                    for (int i = 0; i < ItemList[(int)currentItem].CannotRequireItems.Count; i++)
                    {
                        if (ItemList[(int)target].Conditionals
                            .FindAll(u => u.Contains(ItemList[(int)currentItem].CannotRequireItems[i])
                            || u.Contains(currentItem)).Count == ItemList[(int)target].Conditionals.Count)
                        {
                            Debug.WriteLine($"All conditionals of {target} cannot be required by {currentItem}");
                            return Dependence.Dependent;
                        }
                    }
                }

                int k = 0;
                var circularDependencies = new List<Item>();
                for (int i = 0; i < ItemList[(int)target].Conditionals.Count; i++)
                {
                    bool match = false;
                    for (int j = 0; j < ItemList[(int)target].Conditionals[i].Count; j++)
                    {
                        var d = ItemList[(int)target].Conditionals[i][j];
                        if (!d.IsFake() && !ItemList[(int)d].NewLocation.HasValue && d != currentItem)
                        {
                            continue;
                        }

                        int[] check = new int[] { (int)target, i, j };

                        if (ItemList[(int)d].NewLocation.HasValue)
                        {
                            d = ItemList[(int)d].NewLocation.Value;
                        }
                        if (d == currentItem)
                        {
                            DependenceChecked[d] = Dependence.Dependent;
                        }
                        else
                        {
                            if (dependencyPath.Contains(d))
                            {
                                DependenceChecked[d] = Dependence.Circular(d);
                            }
                            if (!DependenceChecked.ContainsKey(d) || (DependenceChecked[d].Type == DependenceType.Circular && !DependenceChecked[d].Items.All(id => dependencyPath.Contains(id))))
                            {
                                var childPath = dependencyPath.ToList();
                                childPath.Add(d);
                                DependenceChecked[d] = CheckDependence(currentItem, d, childPath);
                            }
                        }

                        if (DependenceChecked[d].Type != DependenceType.NotDependent)
                        {
                            if (!dependencyPath.Contains(d) && DependenceChecked[d].Type == DependenceType.Circular && DependenceChecked[d].Items.All(id => id == d))
                            {
                                DependenceChecked[d] = Dependence.Dependent;
                            }
                            if (DependenceChecked[d].Type == DependenceType.Dependent)
                            {
                                if (!ConditionRemoves.Any(c => c.SequenceEqual(check)))
                                {
                                    ConditionRemoves.Add(check);
                                }
                            }
                            else
                            {
                                circularDependencies = circularDependencies.Union(DependenceChecked[d].Items).ToList();
                            }
                            if (!match)
                            {
                                k++;
                                match = true;
                            }
                        }
                    }
                }

                if (k == ItemList[(int)target].Conditionals.Count)
                {
                    if (circularDependencies.Any())
                    {
                        return Dependence.Circular(circularDependencies.ToArray());
                    }
                    Debug.WriteLine($"All conditionals of {target} failed dependency check for {currentItem}.");
                    return Dependence.Dependent;
                }
            }

            if (ItemList[(int)target].DependsOnItems == null)
            {
                return Dependence.NotDependent;
            }

            //cycle through all things
            for (int i = 0; i < ItemList[(int)target].DependsOnItems.Count; i++)
            {
                var dependency = ItemList[(int)target].DependsOnItems[i];
                if (!currentItem.IsTemporary() && target == Item.MaskBlast && (dependency == Item.TradeItemKafeiLetter || dependency == Item.TradeItemPendant))
                {
                    // Permanent items ignore Kafei Letter and Pendant on Blast Mask check.
                    continue;
                }
                if (dependency == currentItem)
                {
                    Debug.WriteLine($"{target} has direct dependence on {currentItem}");
                    return Dependence.Dependent;
                }

                if (ItemList[(int)currentItem].HasCannotRequireItems)
                {
                    for (int j = 0; j < ItemList[(int)currentItem].CannotRequireItems.Count; j++)
                    {
                        if (ItemList[(int)target].DependsOnItems.Contains(ItemList[(int)currentItem].CannotRequireItems[j]))
                        {
                            Debug.WriteLine($"Dependence {ItemList[(int)currentItem].CannotRequireItems[j]} of {target} cannot be required by {currentItem}");
                            return Dependence.Dependent;
                        }
                    }
                }

                if (dependency.IsFake()
                    || ItemList[(int)dependency].NewLocation.HasValue)
                {
                    if (ItemList[(int)dependency].NewLocation.HasValue)
                    {
                        dependency = ItemList[(int)dependency].NewLocation.Value;
                    }

                    if (dependencyPath.Contains(dependency))
                    {
                        DependenceChecked[dependency] = Dependence.Circular(dependency);
                        return DependenceChecked[dependency];
                    }
                    if (!DependenceChecked.ContainsKey(dependency) || (DependenceChecked[dependency].Type == DependenceType.Circular && !DependenceChecked[dependency].Items.All(id => dependencyPath.Contains(id))))
                    {
                        var childPath = dependencyPath.ToList();
                        childPath.Add(dependency);
                        DependenceChecked[dependency] = CheckDependence(currentItem, dependency, childPath);
                    }
                    if (DependenceChecked[dependency].Type != DependenceType.NotDependent)
                    {
                        if (DependenceChecked[dependency].Type == DependenceType.Circular && DependenceChecked[dependency].Items.All(id => id == dependency))
                        {
                            DependenceChecked[dependency] = Dependence.Dependent;
                        }
                        Debug.WriteLine($"{currentItem} is dependent on {dependency}");
                        return DependenceChecked[dependency];
                    }
                }
            }

            return Dependence.NotDependent;
        }

        private void RemoveConditionals(Item currentItem)
        {
            for (int i = 0; i < ConditionRemoves.Count; i++)
            {
                int x = ConditionRemoves[i][0];
                int y = ConditionRemoves[i][1];
                int z = ConditionRemoves[i][2];
                ItemList[x].Conditionals[y] = null;
            }

            for (int i = 0; i < ConditionRemoves.Count; i++)
            {
                int x = ConditionRemoves[i][0];
                int y = ConditionRemoves[i][1];
                int z = ConditionRemoves[i][2];

                for (int j = 0; j < ItemList[x].Conditionals.Count; j++)
                {
                    if (ItemList[x].Conditionals[j] != null)
                    {
                        for (int k = 0; k < ItemList[x].Conditionals[j].Count; k++)
                        {
                            var d = ItemList[x].Conditionals[j][k];

                            if (!ItemList[x].HasCannotRequireItems)
                            {
                                ItemList[x].CannotRequireItems = new List<Item>();
                            }
                            if (!ItemList[(int)d].CannotRequireItems.Contains(currentItem))
                            {
                                ItemList[(int)d].CannotRequireItems.Add(currentItem);
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < ItemList.Count; i++)
            {
                if (ItemList[i].Conditionals != null)
                {
                    ItemList[i].Conditionals.RemoveAll(u => u == null);
                }
            }

            /*
            for (int i = 0; i < ConditionRemoves.Count; i++)
            {
                for (int j = 0; j < ItemList[ConditionRemoves[i][0]].Conditional[ConditionRemoves[i][1]].Count; j++)
                {
                    int d = ItemList[ConditionRemoves[i][0]].Conditional[ConditionRemoves[i][1]][j];
                    if (ItemList[d].Cannot_Require == null)
                    {
                        ItemList[d].Cannot_Require = new List<int>();
                    };
                    ItemList[d].Cannot_Require.Add(CurrentItem);
                    if (ItemList[ConditionRemoves[i][0]].Dependence == null)
                    {
                        ItemList[ConditionRemoves[i][0]].Dependence = new List<int>();
                    };
                    ItemList[ConditionRemoves[i][0]].Dependence.Add(d);
                };
                ItemList[ConditionRemoves[i][0]].Conditional[ConditionRemoves[i][1]] = null;
            };
            for (int i = 0; i < ItemList.Count; i++)
            {
                if (ItemList[i].Conditional != null)
                {
                    if (ItemList[i].Conditional.Contains(null))
                    {
                        ItemList[i].Conditional = null;
                    };
                };
            };
            */
        }

        private void UpdateConditionals(Item currentItem, Item target)
        {
            var targetId = (int)target;
            if (!ItemList[targetId].HasConditionals)
            {
                return;
            }

            //if ((Target == 114) || (Target == 115))
            //{
            //    return;
            //};
            /*
            if (ItemList[Target].Cannot_Require != null)
            {
                for (int i = 0; i < ItemList[CurrentItem].Cannot_Require.Count; i++)
                {
                    ItemList[Target].Conditional.RemoveAll(u => u.Contains(ItemList[CurrentItem].Cannot_Require[i]));
                };
            };
            ItemList[Target].Conditional.RemoveAll(u => u.Contains(CurrentItem));
            if (ItemList[Target].Conditional.Count == 0)
            {
                return;
            };
            */
            if (ItemList[targetId].Conditionals.Count == 1)
            {
                for (int i = 0; i < ItemList[targetId].Conditionals[0].Count; i++)
                {
                    if (!ItemList[targetId].HasDependencies)
                    {
                        ItemList[targetId].DependsOnItems = new List<Item>();
                    }

                    var j = ItemList[targetId].Conditionals[0][i];
                    var jId = (int)j;
                    if (!ItemList[targetId].DependsOnItems.Contains(j))
                    {
                        ItemList[targetId].DependsOnItems.Add(j);
                    }
                    if (!ItemList[jId].HasCannotRequireItems)
                    {
                        ItemList[jId].CannotRequireItems = new List<Item>();
                    }
                    if (!ItemList[jId].CannotRequireItems.Contains(currentItem))
                    {
                        ItemList[jId].CannotRequireItems.Add(currentItem);
                    }
                }
                ItemList[targetId].Conditionals.RemoveAt(0);
            }
            else
            {
                //check if all conditions have a common item
                for (int i = 0; i < ItemList[targetId].Conditionals[0].Count; i++)
                {
                    var testitem = ItemList[targetId].Conditionals[0][i];
                    if (ItemList[targetId].Conditionals.FindAll(u => u.Contains(testitem)).Count == ItemList[targetId].Conditionals.Count)
                    {
                        // require this item and remove from conditions
                        if (!ItemList[targetId].HasDependencies)
                        {
                            ItemList[targetId].DependsOnItems = new List<Item>();
                        }
                        if (!ItemList[targetId].DependsOnItems.Contains(testitem))
                        {
                            ItemList[targetId].DependsOnItems.Add(testitem);
                        }
                        for (int j = 0; j < ItemList[targetId].Conditionals.Count; j++)
                        {
                            ItemList[targetId].Conditionals[j].Remove(testitem);
                        }

                        break;
                    }
                }
                //for (int i = 0; i < ItemList[Target].Conditional.Count; i++)
                //{
                //    for (int j = 0; j < ItemList[Target].Conditional[i].Count; j++)
                //    {
                //        int k = ItemList[Target].Conditional[i][j];
                //        if (ItemList[k].Cannot_Require == null)
                //        {
                //            ItemList[k].Cannot_Require = new List<int>();
                //        };
                //        ItemList[k].Cannot_Require.Add(CurrentItem);
                //    };
                //};
            };
        }

        private void AddConditionals(Item target, Item currentItem, int d)
        {
            var targetId = (int)target;
            var baseConditionals = ItemList[targetId].Conditionals;

            if (baseConditionals == null)
            {
                baseConditionals = new List<List<Item>>();
            }

            ItemList[targetId].Conditionals = new List<List<Item>>();
            foreach (var conditions in ItemList[d].Conditionals)
            {
                if (!conditions.Contains(currentItem))
                {
                    var newConditional = new List<List<Item>>();
                    if (baseConditionals.Count == 0)
                    {
                        newConditional.Add(conditions);
                    }
                    else
                    {
                        foreach (var baseConditions in baseConditionals)
                        {
                            newConditional.Add(baseConditions.Concat(conditions).ToList());
                        }
                    }

                    ItemList[targetId].Conditionals.AddRange(newConditional);
                }
            }
        }

        private void CheckConditionals(Item currentItem, Item target, List<Item> dependencyPath)
        {
            if (target == Item.MaskBlast)
            {
                if (!currentItem.IsTemporary())
                {
                    ItemList[(int)target].DependsOnItems?.Remove(Item.TradeItemKafeiLetter);
                    ItemList[(int)target].DependsOnItems?.Remove(Item.TradeItemPendant);
                }
            }

            ConditionsChecked.Add(target);
            UpdateConditionals(currentItem, target);

            if (!ItemList[(int)target].HasDependencies)
            {
                return;
            }

            for (int i = 0; i < ItemList[(int)target].DependsOnItems.Count; i++)
            {
                var dependency = ItemList[(int)target].DependsOnItems[i];
                if (!ItemList[(int)dependency].HasCannotRequireItems)
                {
                    ItemList[(int)dependency].CannotRequireItems = new List<Item>();
                }
                if (!ItemList[(int)dependency].CannotRequireItems.Contains(currentItem))
                {
                    ItemList[(int)dependency].CannotRequireItems.Add(currentItem);
                }
                if (dependency.IsFake() || ItemList[(int)dependency].NewLocation.HasValue)
                {
                    if (ItemList[(int)dependency].NewLocation.HasValue)
                    {
                        dependency = ItemList[(int)dependency].NewLocation.Value;
                    }

                    if (!ConditionsChecked.Contains(dependency))
                    {
                        var childPath = dependencyPath.ToList();
                        childPath.Add(dependency);
                        CheckConditionals(currentItem, dependency, childPath);
                    }
                }
                else if (ItemList[(int)currentItem].TimeNeeded != 0 && dependency.IsTemporary() && dependencyPath.Skip(1).All(p => p.IsFake() || ItemList.Single(j => j.NewLocation == p).Item.IsTemporary()))
                {
                    if (ItemList[(int)dependency].TimeNeeded == 0)
                    {
                        ItemList[(int)dependency].TimeNeeded = ItemList[(int)currentItem].TimeNeeded;
                    }
                    else
                    {
                        ItemList[(int)dependency].TimeNeeded &= ItemList[(int)currentItem].TimeNeeded;
                    }
                }
            }

            // todo double check this
            //ItemList[(int)target].DependsOnItems.RemoveAll(u => u == -1);
        }

        private bool CheckMatch(Item currentItem, Item target)
        {
            if (ItemUtils.IsStartingLocation(target) && ForbiddenStartingItems.Contains(currentItem))
            {
                Debug.WriteLine($"{currentItem} cannot be a starting item.");
                return false;
            }

            if (_settings.LogicMode == LogicMode.None)
            {
                return true;
            }

            if (ForbiddenPlacedAt.ContainsKey(currentItem)
                && ForbiddenPlacedAt[currentItem].Contains(target))
            {
                Debug.WriteLine($"{currentItem} forbidden from being placed at {target}");
                return false;
            }

            if (ForbiddenReplacedBy.ContainsKey(target) && ForbiddenReplacedBy[target].Contains(currentItem))
            {
                Debug.WriteLine($"{target} forbids being replaced by {currentItem}");
                return false;
            }

            if (currentItem.IsTemporary() && ItemUtils.IsMoonLocation(target))
            {
                Debug.WriteLine($"{currentItem} cannot be placed on the moon.");
                return false;
            }

            //check direct dependence
            ConditionRemoves = new List<int[]>();
            DependenceChecked = new Dictionary<Item, Dependence> { { target, new Dependence { Type = DependenceType.Dependent } } };
            var dependencyPath = new List<Item> { target };

            if (CheckDependence(currentItem, target, dependencyPath).Type != DependenceType.NotDependent)
            {
                return false;
            }

            //check conditional dependence
            RemoveConditionals(currentItem);
            ConditionsChecked = new List<Item>();
            CheckConditionals(currentItem, target, dependencyPath);
            return true;
        }

        private void PlaceItem(Item currentItem, List<Item> targets)
        {
            var currentItemObject = ItemList[(int)currentItem];
            if (currentItemObject.NewLocation.HasValue)
            {
                return;
            }

            var availableItems = targets.ToList();
            if (currentItem > Item.SongOath)
            {
                availableItems.Remove(Item.MaskDeku);
                availableItems.Remove(Item.SongHealing);
            }

            while (true)
            {
                if (availableItems.Count == 0)
                {
                    throw new Exception($"Unable to place {currentItem.Name()} anywhere.");
                }

                var targetLocation = availableItems.Random(Random);// Random.Next(availableItems.Count);

                Debug.WriteLine($"----Attempting to place {currentItem.Name()} at {targetLocation.Location()}.---");

                if (CheckMatch(currentItem, targetLocation))
                {
                    currentItemObject.NewLocation = targetLocation;
                    currentItemObject.IsRandomized = true;

                    Debug.WriteLine($"----Placed {currentItem.Name()} at {targetLocation.Location()}----");

                    targets.Remove(targetLocation);
                    return;
                }
                else
                {
                    Debug.WriteLine($"----Failed to place {currentItem.Name()} at {targetLocation.Location()}----");
                    availableItems.Remove(targetLocation);
                }
            }
        }

        private void RandomizeItems()
        {
            if (_settings.UseCustomItemList)
            {
                SetupCustomItems();
            }
            else
            {
                Setup();
            }

            var itemPool = new List<Item>();

            AddAllItems(itemPool);

            PlaceFreeItems(itemPool);
            PlaceQuestItems(itemPool);
            PlaceTradeItems(itemPool);
            PlaceDungeonItems(itemPool);
            PlaceStartingItems(itemPool);
            PlaceUpgrades(itemPool);
            PlaceSongs(itemPool);
            PlaceMasks(itemPool);
            PlaceRegularItems(itemPool);
            PlaceShopItems(itemPool);
            PlaceMoonItems(itemPool);
            PlaceHeartpieces(itemPool);
            PlaceOther(itemPool);
            PlaceTingleMaps(itemPool);

            _randomized.ItemList = ItemList;
        }

        /// <summary>
        /// Places starting items in the randomization pool.
        /// </summary>
        private void PlaceStartingItems(List<Item> itemPool)
        {
            for (var i = Item.StartingSword; i <= Item.StartingHeartContainer2; i++)
            {
                PlaceItem(i, itemPool);
            }
        }

        /// <summary>
        /// Places moon items in the randomization pool.
        /// </summary>
        private void PlaceMoonItems(List<Item> itemPool)
        {
            for (var i = Item.HeartPieceDekuTrial; i <= Item.ChestLinkTrialBombchu10; i++)
            {
                PlaceItem(i, itemPool);
            }
        }

        /// <summary>
        /// Places tingle maps in the randomization pool.
        /// </summary>
        private void PlaceTingleMaps(List<Item> itemPool)
        {
            for (var i = Item.ItemTingleMapTown; i <= Item.ItemTingleMapStoneTower; i++)
            {
                PlaceItem(i, itemPool);
            }
        }

        /// <summary>
        /// Places other chests and grottos in the randomization pool.
        /// </summary>
        /// <param name="itemPool"></param>
        private void PlaceOther(List<Item> itemPool)
        {
            for (var i = Item.ChestLensCaveRedRupee; i <= Item.ChestSouthClockTownPurpleRupee; i++)
            {
                PlaceItem(i, itemPool);
            }

            PlaceItem(Item.ChestToGoronRaceGrotto, itemPool);
            PlaceItem(Item.IkanaScrubGoldRupee, itemPool);
            PlaceItem(Item.ChestPreClocktownDekuNut, itemPool);
        }

        /// <summary>
        /// Places heart pieces in the randomization pool. Includes rewards/chests, as well as standing heart pieces.
        /// </summary>
        private void PlaceHeartpieces(List<Item> itemPool)
        {
            // Rewards/chests
            for (var i = Item.HeartPieceNotebookMayor; i <= Item.HeartPieceKnuckle; i++)
            {
                PlaceItem(i, itemPool);
            }

            // Bank reward
            PlaceItem(Item.HeartPieceBank, itemPool);

            // Standing heart pieces
            for (var i = Item.HeartPieceSouthClockTown; i <= Item.HeartContainerStoneTower; i++)
            {
                PlaceItem(i, itemPool);
            }
        }

        /// <summary>
        /// Places shop items in the randomization pool
        /// </summary>
        private void PlaceShopItems(List<Item> itemPool)
        {
            for (var i = Item.ShopItemTradingPostRedPotion; i <= Item.ShopItemZoraRedPotion; i++)
            {
                PlaceItem(i, itemPool);
            }
        }

        /// <summary>
        /// Places dungeon items in the randomization pool
        /// </summary>
        private void PlaceDungeonItems(List<Item> itemPool)
        {
            for (var i = Item.ItemWoodfallMap; i <= Item.ItemStoneTowerKey4; i++)
            {
                PlaceItem(i, itemPool);
            }
        }

        /// <summary>
        /// Places songs in the randomization pool
        /// </summary>
        private void PlaceSongs(List<Item> itemPool)
        {
            for (var i = Item.SongHealing; i <= Item.SongOath; i++)
            {
                PlaceItem(i, itemPool);
            }
        }

        /// <summary>
        /// Places masks in the randomization pool
        /// </summary>
        private void PlaceMasks(List<Item> itemPool)
        {
            for (var i = Item.MaskPostmanHat; i <= Item.MaskZora; i++)
            {
                PlaceItem(i, itemPool);
            }
        }

        /// <summary>
        /// Places upgrade items in the randomization pool
        /// </summary>
        private void PlaceUpgrades(List<Item> itemPool)
        {
            for (var i = Item.UpgradeRazorSword; i <= Item.UpgradeGiantWallet; i++)
            {
                PlaceItem(i, itemPool);
            }
        }

        /// <summary>
        /// Places regular items in the randomization pool
        /// </summary>
        private void PlaceRegularItems(List<Item> itemPool)
        {
            for (var i = Item.MaskDeku; i <= Item.ItemNotebook; i++)
            {
                PlaceItem(i, itemPool);
            }
        }

        /// <summary>
        /// Replace starting deku mask and song of healing with free items if not already replaced.
        /// </summary>
        private void PlaceFreeItems(List<Item> itemPool)
        {
            var freeItemLocations = new List<Item>
            {
                Item.MaskDeku,
                Item.SongHealing,
                Item.StartingShield,
                Item.StartingSword,
                Item.StartingHeartContainer1,
                Item.StartingHeartContainer2,
            };
            var availableStartingItems = (_settings.NoStartingItems
                ? ItemUtils.AllRupees()
                : ItemUtils.StartingItems())
                .Where(item => !ItemList[(int)item].NewLocation.HasValue && !ForbiddenStartingItems.Contains(item))
                .Cast<Item?>()
                .ToList();
            foreach (var location in freeItemLocations)
            {
                var placedItem = ItemList.FirstOrDefault(item => item.NewLocation == location)?.Item;
                if (placedItem == null)
                {
                    placedItem = availableStartingItems.RandomOrDefault(Random);
                    if (placedItem == null)
                    {
                        throw new Exception("Failed to replace a starting item.");
                    }
                    ItemList[(int)placedItem].NewLocation = location;
                    ItemList[(int)placedItem].IsRandomized = true;
                    itemPool.Remove(location);
                    availableStartingItems.Remove(placedItem.Value);
                }


                var forbiddenStartTogether = ItemUtils.ForbiddenStartTogether.FirstOrDefault(list => list.Contains(placedItem.Value));
                if (forbiddenStartTogether != null)
                {
                    availableStartingItems.RemoveAll(item => forbiddenStartTogether.Contains(item.Value));
                }
            }
        }

        /// <summary>
        /// Adds all items into the randomization pool (excludes area/other and items that already have placement)
        /// </summary>
        private void AddAllItems(List<Item> itemPool)
        {
            itemPool.AddRange(ItemUtils.AllLocations().Where(location => !ItemList.Any(io => io.NewLocation == location)));
        }

        /// <summary>
        /// Places quest items in the randomization pool
        /// </summary>
        private void PlaceQuestItems(List<Item> itemPool)
        {
            for (var i = Item.TradeItemRoomKey; i <= Item.TradeItemMamaLetter; i++)
            {
                PlaceItem(i, itemPool);
            }
        }

        /// <summary>
        /// Places trade items in the randomization pool
        /// </summary>
        private void PlaceTradeItems(List<Item> itemPool)
        {
            for (var i = Item.TradeItemMoonTear; i <= Item.TradeItemOceanDeed; i++)
            {
                PlaceItem(i, itemPool);
            }
        }

        /// <summary>
        /// Adds items to randomization pool based on settings.
        /// </summary>
        private void Setup()
        {
            if (_settings.ExcludeSongOfSoaring)
            {
                ItemList[(int)Item.SongSoaring].NewLocation = Item.SongSoaring;
            }

            if (!_settings.AddSongs)
            {
                ShuffleSongs();
            }

            if (!_settings.AddDungeonItems)
            {
                PreserveDungeonItems();
            }

            if (!_settings.AddShopItems)
            {
                PreserveShopItems();
            }

            if (!_settings.AddOtherItems)
            {
                PreserveOther();
            }

            if (_settings.RandomizeBottleCatchContents)
            {
                AddBottleCatchContents();
            }
            else
            {
                PreserveBottleCatchContents();
            }

            if (!_settings.AddMoonItems)
            {
                PreserveMoonItems();
            }

            if (!_settings.AddFairyRewards)
            {
                PreserveFairyRewards();
            }

            if (!_settings.AddNutChest || _settings.LogicMode == LogicMode.Casual)
            {
                PreserveNutChest();
            }

            if (!_settings.CrazyStartingItems)
            {
                PreserveStartingItems();
            }
        }

        /// <summary>
        /// Keeps bottle catch contents vanilla
        /// </summary>
        private void PreserveBottleCatchContents()
        {
            for (var i = Item.BottleCatchFairy; i <= Item.BottleCatchMushroom; i++)
            {
                ItemList[(int)i].NewLocation = i;
            }
        }

        /// <summary>
        /// Randomizes bottle catch contents
        /// </summary>
        private void AddBottleCatchContents()
        {
            var itemPool = new List<Item>();
            for (var i = Item.BottleCatchFairy; i <= Item.BottleCatchMushroom; i++)
            {
                if (ItemList[(int)i].NewLocation.HasValue)
                {
                    continue;
                }
                itemPool.Add(i);
            }

            for (var i = Item.BottleCatchFairy; i <= Item.BottleCatchMushroom; i++)
            {
                PlaceItem(i, itemPool);
            }
        }

        /// <summary>
        /// Keeps other vanilla
        /// </summary>
        private void PreserveOther()
        {
            for (var i = Item.ChestLensCaveRedRupee; i <= Item.IkanaScrubGoldRupee; i++)
            {
                ItemList[(int)i].NewLocation = i;
            }
        }

        /// <summary>
        /// Keeps shop items vanilla
        /// </summary>
        private void PreserveShopItems()
        {
            for (var i = Item.ShopItemTradingPostRedPotion; i <= Item.ShopItemZoraRedPotion; i++)
            {
                ItemList[(int)i].NewLocation = i;
            }

            ItemList[(int)Item.ItemBombBag].NewLocation = Item.ItemBombBag;
            ItemList[(int)Item.UpgradeBigBombBag].NewLocation = Item.UpgradeBigBombBag;
            ItemList[(int)Item.MaskAllNight].NewLocation = Item.MaskAllNight;
        }

        /// <summary>
        /// Keeps dungeon items vanilla
        /// </summary>
        private void PreserveDungeonItems()
        {
            for (var i = Item.ItemWoodfallMap; i <= Item.ItemStoneTowerKey4; i++)
            {
                ItemList[(int)i].NewLocation = i;
            };
        }

        /// <summary>
        /// Keeps moon items vanilla
        /// </summary>
        private void PreserveMoonItems()
        {
            for (var i = Item.HeartPieceDekuTrial; i <= Item.ChestLinkTrialBombchu10; i++)
            {
                ItemList[(int)i].NewLocation = i;
            }
        }

        /// <summary>
        /// Keeps great fairy rewards vanilla
        /// </summary>
        private void PreserveFairyRewards()
        {
            for (var i = Item.FairyMagic; i <= Item.ItemFairySword; i++)
            {
                ItemList[(int)i].NewLocation = i;
            }
            ItemList[(int)Item.MaskGreatFairy].NewLocation = Item.MaskGreatFairy;
        }

        /// <summary>
        /// Keeps nut chest vanilla
        /// </summary>
        private void PreserveNutChest()
        {
            ItemList[(int)Item.ChestPreClocktownDekuNut].NewLocation = Item.ChestPreClocktownDekuNut;
        }

        /// <summary>
        /// Keeps regular starting items vanilla
        /// </summary>
        private void PreserveStartingItems()
        {
            for (var i = Item.StartingSword; i <= Item.StartingHeartContainer2; i++)
            {
                ItemList[(int)i].NewLocation = i;
            }
        }

        /// <summary>
        /// Randomizes songs with other songs
        /// </summary>
        private void ShuffleSongs()
        {
            var itemPool = new List<Item>();
            for (var i = Item.SongHealing; i <= Item.SongOath; i++)
            {
                if (ItemList[(int)i].NewLocation.HasValue)
                {
                    continue;
                }
                itemPool.Add(i);
            }

            for (var i = Item.SongHealing; i <= Item.SongOath; i++)
            {
                PlaceItem(i, itemPool);
            }
        }

        /// <summary>
        /// Adds custom item list to randomization. NOTE: keeps area and other vanilla, randomizes bottle catch contents
        /// </summary>
        private void SetupCustomItems()
        {
            // Keep shop items vanilla, unless custom item list contains a shop item
            _settings.AddShopItems = false;

            // Make all items vanilla, and override using custom item list
            MakeAllItemsVanilla();

            // Should these be vanilla by default? Why not check settings.
            ApplyCustomItemList();

            // Should these be randomized by default? Why not check settings.
            AddBottleCatchContents();

            if (!_settings.AddSongs)
            {
                ShuffleSongs();
            }
        }

        /// <summary>
        /// Mark all items as replacing themselves (i.e. vanilla)
        /// </summary>
        private void MakeAllItemsVanilla()
        {
            foreach (var location in ItemUtils.AllLocations())
            {
                ItemList[(int)location].NewLocation = location;
            }
        }

        /// <summary>
        /// Adds items specified from the Custom Item List to the randomizer pool, while keeping the rest vanilla
        /// </summary>
        private void ApplyCustomItemList()
        {
            if (_settings.CustomItemList.Contains(-1))
            {
                throw new InvalidDataException("Invalid custom item string.");
            }
            for (int i = 0; i < _settings.CustomItemList.Count; i++)
            {
                int selectedItem = _settings.CustomItemList[i];

                selectedItem = ItemUtils.AddItemOffset(selectedItem);

                int selectedItemIndex = ItemList.FindIndex(u => u.ID == selectedItem);

                if (selectedItemIndex != -1)
                {
                    ItemList[selectedItemIndex].NewLocation = null;
                }

                if (ItemUtils.IsShopItem((Item)selectedItem))
                {
                    _settings.AddShopItems = true;
                }
            }
        }

        private ReadOnlyCollection<Item> GetRequiredItems(Item item, List<ItemLogic> itemLogic, List<Item> logicPath = null, Dictionary<Item, ReadOnlyCollection<Item>> checkedItems = null, Item? exclude = null)
        {
            if (item == exclude)
            {
                return null;
            }
            if (logicPath == null)
            {
                logicPath = new List<Item>();
            }
            if (logicPath.Contains(item))
            {
                return null;
            }
            logicPath.Add(item);
            if (checkedItems == null)
            {
                checkedItems = new Dictionary<Item, ReadOnlyCollection<Item>>();
            }
            if (checkedItems.ContainsKey(item))
            {
                return checkedItems[item];
            }
            var itemObject = ItemList[(int)item];
            var locationId = itemObject.NewLocation.HasValue ? itemObject.NewLocation : item;
            var locationLogic = itemLogic[(int)locationId];
            var result = new List<Item>();
            if (locationLogic.RequiredItemIds != null && locationLogic.RequiredItemIds.Any())
            {
                foreach (var requiredItemId in locationLogic.RequiredItemIds)
                {
                    var requiredChildren = GetRequiredItems((Item)requiredItemId, itemLogic, logicPath.ToList(), checkedItems, exclude);
                    if (requiredChildren == null)
                    {
                        return null;
                    }
                    result.Add((Item)requiredItemId);
                    result.AddRange(requiredChildren);
                }
            }
            if (locationLogic.ConditionalItemIds != null && locationLogic.ConditionalItemIds.Any())
            {
                var found = false;
                foreach (var conditions in locationLogic.ConditionalItemIds)
                {
                    var conditionalRequirements = new List<Item>();
                    foreach (var conditionalItemId in conditions)
                    {
                        var requiredChildren = GetRequiredItems((Item)conditionalItemId, itemLogic, logicPath.ToList(), checkedItems, exclude);
                        if (requiredChildren == null)
                        {
                            conditionalRequirements = null;
                            break;
                        }

                        conditionalRequirements.Add((Item)conditionalItemId);
                        conditionalRequirements.AddRange(requiredChildren);
                    }

                    if (conditionalRequirements != null)
                    {
                        found = true;
                        result.AddRange(conditionalRequirements);
                    }
                }
                if (!found)
                {
                    return null;
                }
            }
            var readOnlyResult = result.Distinct().ToList().AsReadOnly();
            checkedItems[item] = readOnlyResult;
            return readOnlyResult;
        }

        /// <summary>
        /// Randomizes the ROM with respect to the configured ruleset.
        /// </summary>
        public RandomizedResult Randomize(BackgroundWorker worker)
        {
            SeedRNG();

            _randomized = new RandomizedResult(_settings, Random);

            if (_settings.LogicMode != LogicMode.Vanilla)
            {
                worker.ReportProgress(5, "Preparing ruleset...");
                PrepareRulesetItemData();

                if (_settings.RandomizeDungeonEntrances)
                {
                    worker.ReportProgress(10, "Shuffling entrances...");
                    EntranceShuffle();
                }

                _randomized.Logic = ItemList.Select(io => new ItemLogic(io)).ToList();

                worker.ReportProgress(30, "Shuffling items...");
                RandomizeItems();

                _randomized.AllItemsOnPathToMoon = GetRequiredItems(Item.AreaMoonAccess, _randomized.Logic)?.Where(item => !item.IsFake()).ToList().AsReadOnly();
                if (_randomized.AllItemsOnPathToMoon == null)
                {
                    throw new Exception("Moon Access is unobtainable.");
                }
                var itemsRequiredForMoonAccess = new List<Item>();
                foreach (var item in _randomized.AllItemsOnPathToMoon)
                {
                    var checkPaths = GetRequiredItems(Item.AreaMoonAccess, _randomized.Logic, exclude: item);
                    if (checkPaths == null)
                    {
                        itemsRequiredForMoonAccess.Add(item);
                    }
                }
                _randomized.ItemsRequiredForMoonAccess = itemsRequiredForMoonAccess.AsReadOnly();

                if (_settings.GossipHintStyle != GossipHintStyle.Default)
                {
                    worker.ReportProgress(35, "Making gossip quotes...");

                    //gossip
                    MakeGossipQuotes();
                }
            }

            worker.ReportProgress(40, "Randomizing cosmetics...");

            //Randomize tatl colour

            if (_settings.TatlColorSchema == TatlColorSchema.Rainbow)
            {
                SeedRNG();
                SetTatlColour();
            }

            //Shuffle BGM
            if (_settings.RandomizeBGM)
            {
                SeedRNG();
                ShuffleBGM();
            }

            return _randomized;
        }
    }

}