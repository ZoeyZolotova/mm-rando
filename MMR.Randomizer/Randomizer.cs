using MMR.Common.Extensions;
using MMR.Randomizer.Constants;
using MMR.Randomizer.Extensions;
using MMR.Randomizer.GameObjects;
using MMR.Randomizer.LogicMigrator;
using MMR.Randomizer.Models;
using MMR.Randomizer.Models.Settings;
using MMR.Randomizer.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MMR.Randomizer
{
    public class Randomizer
    {
        public static readonly string AssemblyVersion = typeof(Randomizer).Assembly.GetName().Version.ToString();

        private Random Random { get; set; }

        public ItemList ItemList { get; set; }

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
                Item.MaskKeaton, ItemUtils.OverwritableItems().ToList()
            },
        };

        private readonly Dictionary<Item, List<Item>> ForbiddenPlacedAt = new Dictionary<Item, List<Item>>
        {
        };

        #endregion

        private GameplaySettings _settings;
        private int _seed;
        private RandomizedResult _randomized;

        public Randomizer(GameplaySettings settings, int seed)
        {
            _settings = settings;
            _seed = seed;
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
            var dungeonEntrances = new List<Item>
            {
                Item.AreaWoodFallTempleAccess,
                Item.AreaSnowheadTempleAccess,
                Item.AreaGreatBayTempleAccess,
                Item.AreaInvertedStoneTowerTempleAccess,
            };

            var dungeonExits = new List<Item>
            {
                Item.AreaWoodFallTempleClear,
                Item.AreaSnowheadTempleClear,
                Item.AreaGreatBayTempleClear,
                Item.AreaStoneTowerClear,
            };

            var randomized = Enumerable.Range(0, 4).ToList().OrderBy(_ => Random.Next()).ToList();

            for (var i = 0; i < randomized.Count; i++)
            {
                var fromIndex = i;
                var toIndex = randomized[i];

                var entrance = dungeonEntrances[fromIndex];
                var targetEntrance = dungeonEntrances[toIndex];

                var exit = dungeonExits[toIndex];
                var targetExit = dungeonExits[fromIndex];

                ItemList[entrance].NewLocation = targetEntrance;
                ItemList[entrance].IsRandomized = true;
                ItemList[exit].NewLocation = targetExit;
                ItemList[exit].IsRandomized = true;
            }
        }

        private void UpdateLogicForSettings()
        {
            foreach (var itemObject in ItemList)
            {
                if (_settings.CustomStartingItemList != null)
                {
                    itemObject.DependsOnItems?.RemoveAll(item => _settings.CustomStartingItemList.Contains(item));
                    itemObject.Conditionals?.ForEach(c => c.RemoveAll(item => _settings.CustomStartingItemList.Contains(item)));
                }

                if (itemObject.Conditionals != null)
                {
                    itemObject.Conditionals.RemoveAll(c => c.Any(item => ItemList[item].IsTrick && !_settings.EnabledTricks.Contains((int)item)));
                }
            }

            if (_settings.AddShopItems)
            {
                ItemList[Item.ShopItemWitchBluePotion].DependsOnItems?.Remove(Item.BottleCatchMushroom);
            }

            if (_settings.RandomizeBottleCatchContents && _settings.LogicMode == LogicMode.Casual)
            {
                var anyBottleIndex = ItemList.FindIndex(io => io.Name == "Any Bottle");
                var twoBottlesIndex = ItemList.FindIndex(io => io.Name == "2 Bottles");
                if (anyBottleIndex >= 0 && twoBottlesIndex >= 0)
                {
                    ItemList[Item.BottleCatchPrincess].DependsOnItems.Remove((Item)anyBottleIndex);
                    ItemList[Item.BottleCatchPrincess].DependsOnItems.Add((Item)twoBottlesIndex);
                }
            }

            var arrows40 = ItemList
                .FirstOrDefault(io =>
                    io.DependsOnItems.Count == 0
                    && io.Conditionals.Count == 2
                    && io.Conditionals.Any(c => c.SequenceEqual(new List<Item> { Item.UpgradeBigQuiver }))
                    && io.Conditionals.Any(c => c.SequenceEqual(new List<Item> { Item.UpgradeBiggestQuiver })));
            if (arrows40 == null)
            {
                arrows40 = new ItemObject
                {
                    ID = ItemList.Count,
                    TimeAvailable = 63,
                    Conditionals = new List<List<Item>>
                    {
                        new List<Item>
                        {
                            Item.UpgradeBigQuiver,
                        },
                        new List<Item>
                        {
                            Item.UpgradeBiggestQuiver,
                        },
                    },
                };
                ItemList.Add(arrows40);
            }

            if (_settings.ByoAmmo && _settings.LogicMode != LogicMode.NoLogic)
            {
                ItemList[Item.ChestInvertedStoneTowerBombchu10].TimeNeeded = 1;
                ItemList[Item.ChestLinkTrialBombchu10].TimeNeeded = 1;
                ItemList[Item.ShopItemBombsBombchu10].TimeNeeded = 1;
                var bombchu10 = new ItemObject
                {
                    ID = ItemList.Count,
                    TimeAvailable = 63,
                    Conditionals = new List<List<Item>>
                    {
                        new List<Item>
                        {
                            Item.ChestInvertedStoneTowerBombchu10,
                        },
                        new List<Item>
                        {
                            Item.ChestLinkTrialBombchu10,
                        },
                        new List<Item>
                        {
                            Item.ShopItemBombsBombchu10,
                        },
                    },
                };
                ItemList.Add(bombchu10);

                ItemList[Item.UpgradeBigQuiver].DependsOnItems.Add(arrows40.Item);
                ItemList[Item.UpgradeBiggestQuiver].DependsOnItems.Add(arrows40.Item);
                ItemList[Item.HeartPieceSwampArchery].DependsOnItems.Add(arrows40.Item);
                ItemList[Item.HeartPieceTownArchery].DependsOnItems.Add(Item.UpgradeBiggestQuiver);
                ItemList[Item.HeartPieceHoneyAndDarling].DependsOnItems.Add(bombchu10.Item);
                
                var escortCremia = new ItemObject
                {
                    ID = ItemList.Count,
                    TimeAvailable = 63,
                    Conditionals = new List<List<Item>>
                    {
                        new List<Item>
                        {
                            Item.OtherArrow,
                        },
                        new List<Item>
                        {
                            Item.MaskCircusLeader,
                        },
                    },
                };
                ItemList.Add(escortCremia);
                ItemList[Item.MaskRomani].DependsOnItems.Add(escortCremia.Item);
            }

            if (_settings.ProgressiveUpgrades && _settings.LogicMode != LogicMode.NoLogic)
            {
                arrows40.Conditionals.Clear();
                arrows40.Conditionals.AddRange(new List<Item>
                {
                    Item.ItemBow,
                    Item.UpgradeBigQuiver,
                    Item.UpgradeBiggestQuiver,
                }.Combinations(2).Select(a => a.ToList()));

                var arrows50 = new ItemObject
                {
                    ID = ItemList.Count,
                    TimeAvailable = 63,
                    DependsOnItems = new List<Item>
                    {
                        Item.ItemBow,
                        Item.UpgradeBigQuiver,
                        Item.UpgradeBiggestQuiver,
                    },
                };
                ItemList.Add(arrows50);

                var bombs20 = ItemList
                    .FirstOrDefault(io =>
                        io.DependsOnItems.Count == 0
                        && io.Conditionals.Count == 3
                        && io.Conditionals.Any(c => c.SequenceEqual(new List<Item> { Item.ItemBombBag }))
                        && io.Conditionals.Any(c => c.SequenceEqual(new List<Item> { Item.UpgradeBigBombBag }))
                        && io.Conditionals.Any(c => c.SequenceEqual(new List<Item> { Item.UpgradeBiggestBombBag })));

                var bombs30 = ItemList
                    .FirstOrDefault(io =>
                        io.DependsOnItems.Count == 0
                        && io.Conditionals.Count == 2
                        && io.Conditionals.Any(c => c.SequenceEqual(new List<Item> { Item.UpgradeBigBombBag }))
                        && io.Conditionals.Any(c => c.SequenceEqual(new List<Item> { Item.UpgradeBiggestBombBag})));
                if (bombs30 == null)
                {
                    bombs30 = new ItemObject
                    {
                        ID = ItemList.Count,
                        TimeAvailable = 63,
                        Conditionals = new List<Item>
                        {
                            Item.ItemBombBag,
                            Item.UpgradeBigBombBag,
                            Item.UpgradeBiggestBombBag,
                        }.Combinations(2).Select(a => a.ToList()).ToList(),
                    };
                    ItemList.Add(bombs30);
                }
                else
                {
                    bombs30.Conditionals.Clear();
                    bombs30.Conditionals.AddRange(new List<Item>
                    {
                        Item.ItemBombBag,
                        Item.UpgradeBigBombBag,
                        Item.UpgradeBiggestBombBag,
                    }.Combinations(2).Select(a => a.ToList()));
                }

                var bombs40 = new ItemObject
                {
                    ID = ItemList.Count,
                    TimeAvailable = 63,
                    DependsOnItems = new List<Item>
                    {
                        Item.ItemBombBag,
                        Item.UpgradeBigBombBag,
                        Item.UpgradeBiggestBombBag,
                    },
                };
                ItemList.Add(bombs40);

                var sword1 = ItemList
                    .FirstOrDefault(io =>
                        io.DependsOnItems.Count == 0
                        && io.Conditionals.Count == 3
                        && io.Conditionals.Any(c => c.SequenceEqual(new List<Item> { Item.StartingSword }))
                        && io.Conditionals.Any(c => c.SequenceEqual(new List<Item> { Item.UpgradeRazorSword }))
                        && io.Conditionals.Any(c => c.SequenceEqual(new List<Item> { Item.UpgradeGildedSword })));

                var sword2 = ItemList
                    .FirstOrDefault(io =>
                        io.DependsOnItems.Count == 0
                        && io.Conditionals.Count == 2
                        && io.Conditionals.Any(c => c.SequenceEqual(new List<Item> { Item.UpgradeRazorSword }))
                        && io.Conditionals.Any(c => c.SequenceEqual(new List<Item> { Item.UpgradeGildedSword })));
                if (sword2 == null)
                {
                    sword2 = new ItemObject
                    {
                        ID = ItemList.Count,
                        TimeAvailable = 63,
                        Conditionals = new List<Item>
                        {
                            Item.StartingSword,
                            Item.UpgradeRazorSword,
                            Item.UpgradeGildedSword,
                        }.Combinations(2).Select(a => a.ToList()).ToList(),
                    };
                    ItemList.Add(sword2);
                }
                else
                {
                    sword2.Conditionals.Clear();
                    sword2.Conditionals.AddRange(new List<Item>
                    {
                        Item.StartingSword,
                        Item.UpgradeRazorSword,
                        Item.UpgradeGildedSword,
                    }.Combinations(2).Select(a => a.ToList()));
                }

                var sword3 = new ItemObject
                {
                    ID = ItemList.Count,
                    TimeAvailable = 63,
                    DependsOnItems = new List<Item>
                    {
                        Item.StartingSword,
                        Item.UpgradeRazorSword,
                        Item.UpgradeGildedSword,
                    },
                };
                ItemList.Add(sword3);

                var wallets200 = ItemList
                    .FirstOrDefault(io =>
                        io.DependsOnItems.Count == 0
                        && io.Conditionals.Count == 2
                        && io.Conditionals.Any(c => c.SequenceEqual(new List<Item> { Item.UpgradeAdultWallet }))
                        && io.Conditionals.Any(c => c.SequenceEqual(new List<Item> { Item.UpgradeGiantWallet })));

                var wallets500 = new ItemObject
                {
                    ID = ItemList.Count,
                    TimeAvailable = 63,
                    DependsOnItems = new List<Item>
                    {
                        Item.UpgradeAdultWallet,
                        Item.UpgradeGiantWallet,
                    },
                };
                ItemList.Add(wallets500);

                var magicAny = ItemList
                    .FirstOrDefault(io =>
                        io.DependsOnItems.Count == 0
                        && io.Conditionals.Count == 2
                        && io.Conditionals.Any(c => c.SequenceEqual(new List<Item> { Item.FairyMagic }))
                        && io.Conditionals.Any(c => c.SequenceEqual(new List<Item> { Item.FairyDoubleMagic })));

                var magicLarge = new ItemObject
                {
                    ID = ItemList.Count,
                    TimeAvailable = 63,
                    DependsOnItems = new List<Item>
                    {
                        Item.FairyMagic,
                        Item.FairyDoubleMagic,
                    },
                };
                ItemList.Add(magicLarge);

                foreach (var itemObject in ItemList)
                {
                    if (itemObject != magicLarge && itemObject.DependsOnItems.Contains(Item.FairyDoubleMagic))
                    {
                        itemObject.DependsOnItems.Remove(Item.FairyDoubleMagic);
                        itemObject.DependsOnItems.Add(magicLarge.Item);
                    }

                    if (itemObject != magicAny)
                    {
                        foreach (var conditions in itemObject.Conditionals)
                        {
                            if (conditions.Contains(Item.FairyDoubleMagic))
                            {
                                conditions.Remove(Item.FairyDoubleMagic);
                                conditions.Add(magicLarge.Item);
                            }
                        }
                    }

                    if (itemObject != wallets500 && itemObject.DependsOnItems.Contains(Item.UpgradeGiantWallet))
                    {
                        itemObject.DependsOnItems.Remove(Item.UpgradeGiantWallet);
                        itemObject.DependsOnItems.Add(wallets500.Item);
                    }

                    if (itemObject != wallets200)
                    {
                        foreach (var conditions in itemObject.Conditionals)
                        {
                            if (conditions.Contains(Item.UpgradeGiantWallet))
                            {
                                conditions.Remove(Item.UpgradeGiantWallet);
                                conditions.Add(wallets500.Item);
                            }
                        }
                    }

                    if (itemObject != sword3 && itemObject.DependsOnItems.Contains(Item.UpgradeGildedSword))
                    {
                        itemObject.DependsOnItems.Remove(Item.UpgradeGildedSword);
                        itemObject.DependsOnItems.Add(sword3.Item);
                    }

                    if (itemObject != sword1 && itemObject != sword2)
                    {
                        foreach (var conditions in itemObject.Conditionals)
                        {
                            if (conditions.Contains(Item.UpgradeGildedSword))
                            {
                                conditions.Remove(Item.UpgradeGildedSword);
                                conditions.Add(sword3.Item);
                            }

                            if (conditions.Contains(Item.UpgradeRazorSword))
                            {
                                conditions.Remove(Item.UpgradeRazorSword);
                                conditions.Add(sword2.Item);
                            }
                        }
                    }

                    if (itemObject != bombs40 && itemObject.DependsOnItems.Contains(Item.UpgradeBiggestBombBag))
                    {
                        itemObject.DependsOnItems.Remove(Item.UpgradeBiggestBombBag);
                        itemObject.DependsOnItems.Add(bombs40.Item);
                    }

                    if (itemObject != bombs20 && itemObject != bombs30 && itemObject.Item != Item.OtherExplosive)
                    {
                        foreach (var conditions in itemObject.Conditionals)
                        {
                            if (conditions.Contains(Item.UpgradeBiggestBombBag))
                            {
                                conditions.Remove(Item.UpgradeBiggestBombBag);
                                conditions.Add(bombs40.Item);
                            }

                            if (conditions.Contains(Item.UpgradeBigBombBag))
                            {
                                conditions.Remove(Item.UpgradeBigBombBag);
                                conditions.Add(bombs30.Item);
                            }
                        }
                    }

                    if (itemObject != arrows50 && itemObject.DependsOnItems.Contains(Item.UpgradeBiggestQuiver))
                    {
                        itemObject.DependsOnItems.Remove(Item.UpgradeBiggestQuiver);
                        itemObject.DependsOnItems.Add(arrows50.Item);
                    }

                    if (itemObject != arrows40 && itemObject.Item != Item.OtherArrow)
                    {
                        foreach (var conditions in itemObject.Conditionals)
                        {
                            if (conditions.Contains(Item.UpgradeBiggestQuiver))
                            {
                                conditions.Remove(Item.UpgradeBiggestQuiver);
                                conditions.Add(arrows50.Item);
                            }

                            if (conditions.Contains(Item.UpgradeBigQuiver))
                            {
                                conditions.Remove(Item.UpgradeBigQuiver);
                                conditions.Add(arrows40.Item);
                            }
                        }
                    }
                }
            }
        }

        private void PrepareRulesetItemData()
        {
            if (_settings.LogicMode == LogicMode.Casual
                || _settings.LogicMode == LogicMode.Glitched
                || _settings.LogicMode == LogicMode.UserLogic)
            {
                string[] data = LogicUtils.ReadRulesetFromResources(_settings.LogicMode, _settings.UserLogicFileName);
                ItemList = LogicUtils.PopulateItemListFromLogicData(data);
            }
            else
            {
                ItemList = LogicUtils.PopulateItemListWithoutLogic();
            }

            if (_settings.UseCustomItemList)
            {
                UpdateCustomItemListSettings();
            }

            UpdateLogicForSettings();

            ItemUtils.PrepareJunkItems(ItemList);
            if (_settings.CustomJunkLocations.Count > ItemUtils.JunkItems.Count)
            {
                throw new Exception($"Too many Enforced Junk Locations. Select up to {ItemUtils.JunkItems.Count}.");
            }
        }

        private void UpdateCustomItemListSettings()
        {
            if (_settings.CustomItemList.Contains(-1))
            {
                throw new Exception("Invalid custom item string.");
            }

            // Keep shop items vanilla, unless custom item list contains a shop item
            _settings.AddShopItems = false;

            // Keep cows vanilla, unless custom item list contains a cow
            _settings.AddCowMilk = false;

            // Keep skulltula tokens vanilla, unless custom item list contains a token
            _settings.AddSkulltulaTokens = false;

            // Keep stray fairies vanilla, unless custom item list contains a fairy
            _settings.AddStrayFairies = false;

            // Keep scoops vanilla, unless custom item list contains a scoop
            _settings.RandomizeBottleCatchContents = false;

            foreach (var item in _settings.CustomItemList.Select(ItemUtils.AddItemOffset).Cast<Item>())
            {
                if (ItemUtils.IsShopItem(item))
                {
                    _settings.AddShopItems = true;
                }

                if (ItemUtils.IsCowItem(item))
                {
                    _settings.AddCowMilk = true;
                }

                if (ItemUtils.IsSkulltulaToken(item))
                {
                    _settings.AddSkulltulaTokens = true;
                }

                if (ItemUtils.IsStrayFairy(item))
                {
                    _settings.AddStrayFairies = true;
                }

                if (ItemUtils.IsBottleCatchContent(item))
                {
                    _settings.RandomizeBottleCatchContents = true;
                }
            }
        }

        private void SeedRNG()
        {
            Random = new Random(_seed);
        }

        private Dependence CheckDependence(Item currentItem, Item target, List<Item> dependencyPath)
        {
            Debug.WriteLine($"CheckDependence({currentItem}, {target})");
            var currentItemObject = ItemList[currentItem];
            var currentTargetObject = ItemList[target];

            if (currentTargetObject.IsTrick && !_settings.EnabledTricks.Contains(currentTargetObject.ID))
            {
                return Dependence.Dependent;
            }

            if (currentItemObject.TimeNeeded == 0 && ItemUtils.IsJunk(currentItem))
            {
                return Dependence.NotDependent;
            }

            //check timing
            if (currentItemObject.TimeNeeded != 0 && dependencyPath.Skip(1).All(p => p.IsFake() || ItemList.Single(i => i.NewLocation == p).Item.IsTemporary()))
            {
                if ((currentItemObject.TimeNeeded & currentTargetObject.TimeAvailable) == 0)
                {
                    Debug.WriteLine($"{currentItem} is needed at {currentItemObject.TimeNeeded} but {target} is only available at {currentTargetObject.TimeAvailable}");
                    return Dependence.Dependent;
                }
            }

            if (currentTargetObject.Conditionals.Any())
            {
                if (currentTargetObject.Conditionals.All(u => u.Contains(currentItem)))
                {
                    Debug.WriteLine($"All conditionals of {target} contains {currentItem}");
                    return Dependence.Dependent;
                }

                foreach (var cannotRequireItem in currentItemObject.CannotRequireItems)
                {
                    if (currentTargetObject.Conditionals.All(u => u.Contains(cannotRequireItem) || u.Contains(currentItem)))
                    {
                        Debug.WriteLine($"All conditionals of {target} cannot be required by {currentItem}");
                        return Dependence.Dependent;
                    }
                }

                int k = 0;
                var circularDependencies = new List<Item>();
                for (int i = 0; i < currentTargetObject.Conditionals.Count; i++)
                {
                    bool match = false;
                    for (int j = 0; j < currentTargetObject.Conditionals[i].Count; j++)
                    {
                        var d = currentTargetObject.Conditionals[i][j];
                        if (!d.IsFake() && !ItemList[d].NewLocation.HasValue && d != currentItem)
                        {
                            continue;
                        }

                        int[] check = new int[] { (int)target, i, j };

                        if (ItemList[d].NewLocation.HasValue)
                        {
                            d = ItemList[d].NewLocation.Value;
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

                if (k == currentTargetObject.Conditionals.Count)
                {
                    if (circularDependencies.Any())
                    {
                        return Dependence.Circular(circularDependencies.ToArray());
                    }
                    Debug.WriteLine($"All conditionals of {target} failed dependency check for {currentItem}.");
                    return Dependence.Dependent;
                }
            }

            if (currentTargetObject.DependsOnItems == null)
            {
                return Dependence.NotDependent;
            }

            foreach (var cannotRequireItem in currentItemObject.CannotRequireItems)
            {
                if (currentTargetObject.DependsOnItems.Contains(cannotRequireItem))
                {
                    Debug.WriteLine($"Dependence {cannotRequireItem} of {target} cannot be required by {currentItem}");
                    return Dependence.Dependent;
                }
            }

            //cycle through all things
            foreach (var dependency in currentTargetObject.DependsOnItems)
            {
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

                if (dependency.IsFake()
                    || ItemList[dependency].NewLocation.HasValue)
                {
                    var location = ItemList[dependency].NewLocation ?? dependency;

                    if (dependencyPath.Contains(location))
                    {
                        DependenceChecked[location] = Dependence.Circular(location);
                        return DependenceChecked[location];
                    }
                    if (!DependenceChecked.ContainsKey(location) || (DependenceChecked[location].Type == DependenceType.Circular && !DependenceChecked[location].Items.All(id => dependencyPath.Contains(id))))
                    {
                        var childPath = dependencyPath.ToList();
                        childPath.Add(location);
                        DependenceChecked[location] = CheckDependence(currentItem, location, childPath);
                    }
                    if (DependenceChecked[location].Type != DependenceType.NotDependent)
                    {
                        if (DependenceChecked[location].Type == DependenceType.Circular && DependenceChecked[location].Items.All(id => id == location))
                        {
                            DependenceChecked[location] = Dependence.Dependent;
                        }
                        Debug.WriteLine($"{currentItem} is dependent on {location}");
                        return DependenceChecked[location];
                    }
                }
            }

            return Dependence.NotDependent;
        }

        private void RemoveConditionals(Item currentItem)
        {
            foreach (var conditionRemove in ConditionRemoves)
            {
                int x = conditionRemove[0];
                int y = conditionRemove[1];
                int z = conditionRemove[2];
                ItemList[x].Conditionals[y] = null;
            }
            
            foreach (var targetRemovals in ConditionRemoves.Select(cr => ItemList[cr[0]]))
            {
                foreach (var conditionals in targetRemovals.Conditionals)
                {
                    if (conditionals != null)
                    {
                        foreach (var d in conditionals)
                        {
                            if (!ItemList[d].CannotRequireItems.Contains(currentItem))
                            {
                                ItemList[d].CannotRequireItems.Add(currentItem);
                            }
                        }
                    }
                }
            }

            foreach (var itemObject in ItemList)
            {
                itemObject.Conditionals.RemoveAll(u => u == null);
            }
        }

        private void UpdateConditionals(Item currentItem, Item target)
        {
            var targetItemObject = ItemList[target];
            if (!targetItemObject.Conditionals.Any())
            {
                return;
            }

            if (targetItemObject.Conditionals.Count == 1)
            {
                foreach (var conditionalItem in targetItemObject.Conditionals[0])
                {
                    if (!targetItemObject.DependsOnItems.Contains(conditionalItem))
                    {
                        targetItemObject.DependsOnItems.Add(conditionalItem);
                    }
                    if (!ItemList[conditionalItem].CannotRequireItems.Contains(currentItem))
                    {
                        ItemList[conditionalItem].CannotRequireItems.Add(currentItem);
                    }
                }
                targetItemObject.Conditionals.RemoveAt(0);
            }
            else
            {
                //check if all conditions have a common item
                var commonConditionals = targetItemObject.Conditionals[0].Where(c => targetItemObject.Conditionals.All(cs => cs.Contains(c))).ToList();
                foreach (var commonConditional in commonConditionals)
                {
                    // require this item and remove from conditions
                    if (!targetItemObject.DependsOnItems.Contains(commonConditional))
                    {
                        targetItemObject.DependsOnItems.Add(commonConditional);
                    }
                    foreach (var conditional in targetItemObject.Conditionals)
                    {
                        conditional.Remove(commonConditional);
                    }
                    if (targetItemObject.Conditionals.Any(cs => !cs.Any()))
                    {
                        targetItemObject.Conditionals.Clear();
                    }
                }
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
            var targetItemObject = ItemList[target];
            if (target == Item.MaskBlast)
            {
                if (!currentItem.IsTemporary())
                {
                    targetItemObject.DependsOnItems?.Remove(Item.TradeItemKafeiLetter);
                    targetItemObject.DependsOnItems?.Remove(Item.TradeItemPendant);
                }
            }

            ConditionsChecked.Add(target);
            UpdateConditionals(currentItem, target);

            foreach (var dependency in targetItemObject.DependsOnItems)
            {
                var dependencyObject = ItemList[dependency];
                if (!dependencyObject.CannotRequireItems.Contains(currentItem))
                {
                    dependencyObject.CannotRequireItems.Add(currentItem);
                }
                if (dependency.IsFake() || dependencyObject.NewLocation.HasValue)
                {
                    var location = dependencyObject.NewLocation ?? dependency;

                    if (!ConditionsChecked.Contains(location))
                    {
                        var childPath = dependencyPath.ToList();
                        childPath.Add(location);
                        CheckConditionals(currentItem, location, childPath);
                    }
                }
                else if (ItemList[currentItem].TimeNeeded != 0 && dependency.IsTemporary() && dependencyPath.Skip(1).All(p => p.IsFake() || ItemList.Single(j => j.NewLocation == p).Item.IsTemporary()))
                {
                    if (dependencyObject.TimeNeeded == 0)
                    {
                        dependencyObject.TimeNeeded = ItemList[currentItem].TimeNeeded;
                    }
                    else
                    {
                        dependencyObject.TimeNeeded &= ItemList[currentItem].TimeNeeded;
                    }
                }
            }

            // todo double check this
            //ItemList[target].DependsOnItems.RemoveAll(u => u == -1);
        }

        private bool CheckMatch(Item currentItem, Item target)
        {
            if (_settings.CustomStartingItemList.Contains(currentItem))
            {
                return true;
            }

            if (ItemUtils.IsStartingLocation(target) && ForbiddenStartingItems.Contains(currentItem))
            {
                Debug.WriteLine($"{currentItem} cannot be a starting item.");
                return false;
            }

            if (_settings.LogicMode == LogicMode.NoLogic)
            {
                return true;
            }

            if (_settings.CustomJunkLocations.Contains(target) && !ItemUtils.IsJunk(currentItem))
            {
                return false;
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

            if (currentItem.IsTemporary())
            {
                if (ItemUtils.IsMoonLocation(target))
                {
                    Debug.WriteLine($"{currentItem} is temporary and cannot be placed on the moon.");
                    return false;
                }

                // This is to prevent business scrub relocation logic from potentially causing unbeatable seeds.
                // TODO fix this in a nicer way.
                if (target == Item.HeartPieceNotebookHand && !ItemUtils.IsJunk(currentItem))
                {
                    Debug.WriteLine($"{currentItem} is temporary and cannot be placed on {target}.");
                    return false;
                }
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
            var currentItemObject = ItemList[currentItem];
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
                    throw new RandomizationException($"Unable to place {currentItem.Name()} anywhere.");
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
            PlaceSkulltulaTokens(itemPool);
            PlaceStrayFairies(itemPool);
            PlaceMundaneRewards(itemPool);
            PlaceShopItems(itemPool);
            PlaceCowMilk(itemPool);
            PlaceMoonItems(itemPool);
            PlaceHeartpieces(itemPool);
            PlaceOther(itemPool);
            PlaceTingleMaps(itemPool);

            _randomized.ItemList = ItemList;
        }

        /// <summary>
        /// Places plando items in the randomization pool
        ///   re-implements PlaceItem for plando
        ///   reasons: we need pass a different list of checks, but itemPool still needs to be decremented
        ///            we want a different error, stating plando failed not just any randomization
        ///            for plando, we want the ability to bypass logic
        ///            for skipped logic, we still need to make sure certain items are not placed in starting slots
        /// </summary>
        /// <param name="itemPool"></param>
        private void PlacePlandoItems(List<Item> itemPool = null)
        {
            // remember, this is a check pool, not an item pool
            if (itemPool == null)
            {
                itemPool = new List<Item>();
                AddAllItems(itemPool);
            }

            List<PlandoItemCombo> plandoItemCombos = PlandoUtils.ReadAllItemPlandoFiles(itemPool);
            if (plandoItemCombos != null)
            {
                foreach (PlandoItemCombo pic in plandoItemCombos)
                {
                    var itemCombo = PlandoUtils.CleanItemCombo(pic, Random, itemPool, ItemList);
                    if (itemCombo == null) // not possible to fullfill
                    {
                        // let's backtrack and find the items that are already assigned
                        //   and the checks that are already taken and print them
                        var allItems = ItemList.FindAll(u => pic.ItemList.Contains(u.Item));
                        var previouslyPlacedItems = allItems.FindAll(u => u.IsRandomized);
                        string picDebug = "Items that were already assigned:\n";
                        foreach(var item in previouslyPlacedItems)
                        {
                            picDebug += "- [" + item.Item.Name() + "] was placed in check: [" + item.NewLocation.Value.Location() + "]\n";
                        }
                        var previouslyPlacedChecks = ItemList.FindAll(u => u.IsRandomized && pic.CheckList.Contains(u.NewLocation.Value));
                        picDebug += "\nChecks that were already assigned:\n";
                        foreach (var item in previouslyPlacedChecks)
                        {
                            picDebug += "- [" + item.NewLocation.Value.Location() + "] was filled with item: [" + item.Item.Name() + "]\n";
                        }
                        throw new Exception("Error: Plando failed to build with this seed\n combo name: [" + pic.Name + "]\n\n" + picDebug);
                    }

                    int drawCount = 0;
                    for(int itemCount = 0; drawCount < itemCombo.ItemDrawCount && itemCount < itemCombo.ItemList.Count; itemCount++)
                    {
                        /// for all items, attempt to add; count successes
                        Item item = itemCombo.ItemList[itemCount];
                        foreach (Item check in itemCombo.CheckList)
                        {
                            if (itemCombo.SkipLogic && ItemUtils.IsStartingLocation(check) && ForbiddenStartingItems.Contains(item))
                            {
                                Debug.WriteLine("Cannot place forbidden item in starting location: " + item.Name());
                                continue;
                            }

                            if (itemCombo.SkipLogic || CheckMatch(item, check))
                            {
                                ItemList[item].NewLocation = check;
                                ItemList[item].IsRandomized = true;

                                Debug.WriteLine($"----Plando Placed {item.Name()} at {check.Location()}----");

                                itemPool.Remove(check);
                                itemCombo.CheckList.Remove(check);
                                drawCount++;
                                break;
                            }
                        }
                    }
                    if (drawCount < itemCombo.ItemDrawCount)
                    {
                        throw new Exception("Error: Plando could not find enough checks to match this plandos items with this seed: " + itemCombo.Name);
                    }
                }
            }
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
        /// Places skulltula tokens in the randomization pool.
        /// </summary>
        private void PlaceSkulltulaTokens(List<Item> itemPool)
        {
            for (var i = Item.CollectibleSwampSpiderToken1; i <= Item.CollectibleOceanSpiderToken30; i++)
            {
                PlaceItem(i, itemPool);
            }
        }

        /// <summary>
        /// Places stray fairies in the randomization pool.
        /// </summary>
        private void PlaceStrayFairies(List<Item> itemPool)
        {
            for (var i = Item.CollectibleStrayFairyClockTown; i <= Item.CollectibleStrayFairyStoneTower15; i++)
            {
                PlaceItem(i, itemPool);
            }
        }

        /// <summary>
        /// Places mundane rewards in the randomization pool.
        /// </summary>
        private void PlaceMundaneRewards(List<Item> itemPool)
        {
            for (var i = Item.MundaneItemLotteryPurpleRupee; i <= Item.MundaneItemSeahorse; i++)
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
        /// Places cow milk in the randomization pool
        /// </summary>
        private void PlaceCowMilk(List<Item> itemPool)
        {
            for (var i = Item.ItemRanchBarnMainCowMilk; i <= Item.ItemCoastGrottoCowMilk2; i++)
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
                .Where(item => !ItemList[item].NewLocation.HasValue && !ForbiddenStartingItems.Contains(item) && !_settings.CustomStartingItemList.Contains(item))
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
                        throw new Exception("Failed to replace a starting item. Not enough items that can be started with are randomized or too many Extra Starting Items are selected.");
                    }
                    ItemList[placedItem.Value].NewLocation = location;
                    ItemList[placedItem.Value].IsRandomized = true;
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
                ItemList[Item.SongSoaring].NewLocation = Item.SongSoaring;
            }

            PlacePlandoItems();

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

            if (!_settings.AddOther)
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

            if (!_settings.AddCowMilk)
            {
                PreserveCowMilk();
            }

            if (!_settings.AddSkulltulaTokens)
            {
                PreserveSkulltulaTokens();
            }

            if (!_settings.AddStrayFairies)
            {
                PreserveStrayFairies();
            }

            if (!_settings.AddMundaneRewards)
            {
                PreserveMundaneRewards();
            }

            if (_settings.LogicMode == LogicMode.Casual && ItemList[Item.ItemRanchBarnOtherCowMilk2].Conditionals.Count == 1)
            {
                PreserveGlitchedCowMilk();
            }
        }

        /// <summary>
        /// Keeps bottle catch contents vanilla
        /// </summary>
        private void PreserveBottleCatchContents()
        {
            for (var i = Item.BottleCatchFairy; i <= Item.BottleCatchMushroom; i++)
            {
                ItemList[i].NewLocation = i;
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
                if (ItemList[i].NewLocation.HasValue)
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
                ItemList[i].NewLocation = i;
            }
        }

        /// <summary>
        /// Keeps shop items vanilla
        /// </summary>
        private void PreserveShopItems()
        {
            for (var i = Item.ShopItemTradingPostRedPotion; i <= Item.ShopItemZoraRedPotion; i++)
            {
                ItemList[i].NewLocation = i;
            }

            ItemList[Item.ItemBombBag].NewLocation = Item.ItemBombBag;
            ItemList[Item.UpgradeBigBombBag].NewLocation = Item.UpgradeBigBombBag;
            ItemList[Item.MaskAllNight].NewLocation = Item.MaskAllNight;

            ItemList[Item.ShopItemMilkBarChateau].NewLocation = Item.ShopItemMilkBarChateau;
            ItemList[Item.ShopItemMilkBarMilk].NewLocation = Item.ShopItemMilkBarMilk;
            ItemList[Item.ShopItemBusinessScrubMagicBean].NewLocation = Item.ShopItemBusinessScrubMagicBean;
            ItemList[Item.ShopItemBusinessScrubGreenPotion].NewLocation = Item.ShopItemBusinessScrubGreenPotion;
            ItemList[Item.ShopItemBusinessScrubBluePotion].NewLocation = Item.ShopItemBusinessScrubBluePotion;
            ItemList[Item.ShopItemGormanBrosMilk].NewLocation = Item.ShopItemGormanBrosMilk;
        }

        /// <summary>
        /// Keeps dungeon items vanilla
        /// </summary>
        private void PreserveDungeonItems()
        {
            for (var i = Item.ItemWoodfallMap; i <= Item.ItemStoneTowerKey4; i++)
            {
                ItemList[i].NewLocation = i;
            };
        }

        /// <summary>
        /// Keeps moon items vanilla
        /// </summary>
        private void PreserveMoonItems()
        {
            for (var i = Item.HeartPieceDekuTrial; i <= Item.ChestLinkTrialBombchu10; i++)
            {
                ItemList[i].NewLocation = i;
            }
        }

        /// <summary>
        /// Keeps great fairy rewards vanilla
        /// </summary>
        private void PreserveFairyRewards()
        {
            for (var i = Item.FairyMagic; i <= Item.ItemFairySword; i++)
            {
                ItemList[i].NewLocation = i;
            }
            ItemList[Item.MaskGreatFairy].NewLocation = Item.MaskGreatFairy;
        }

        /// <summary>
        /// Keeps nut chest vanilla
        /// </summary>
        private void PreserveNutChest()
        {
            ItemList[Item.ChestPreClocktownDekuNut].NewLocation = Item.ChestPreClocktownDekuNut;
        }

        /// <summary>
        /// Keeps regular starting items vanilla
        /// </summary>
        private void PreserveStartingItems()
        {
            for (var i = Item.StartingSword; i <= Item.StartingHeartContainer2; i++)
            {
                ItemList[i].NewLocation = i;
            }
        }

        /// <summary>
        /// Keeps cow milk vanilla
        /// </summary>
        private void PreserveCowMilk()
        {
            for (var i = Item.ItemRanchBarnMainCowMilk; i <= Item.ItemCoastGrottoCowMilk2; i++)
            {
                ItemList[i].NewLocation = i;
            }
        }

        /// <summary>
        /// Keeps skulltula tokens vanilla
        /// </summary>
        private void PreserveSkulltulaTokens()
        {
            for (var i = Item.CollectibleSwampSpiderToken1; i <= Item.CollectibleOceanSpiderToken30; i++)
            {
                ItemList[i].NewLocation = i;
            }
        }

        /// <summary>
        /// Keeps stray fairies vanilla
        /// </summary>
        private void PreserveStrayFairies()
        {
            for (var i = Item.CollectibleStrayFairyClockTown; i <= Item.CollectibleStrayFairyStoneTower15; i++)
            {
                ItemList[i].NewLocation = i;
            }
        }

        private void PreserveMundaneRewards()
        {
            for (var i = Item.MundaneItemLotteryPurpleRupee; i <= Item.MundaneItemSeahorse; i++)
            {
                if (!ItemUtils.IsShopItem(i))
                {
                    ItemList[i].NewLocation = i;
                }
            }
        }

        /// <summary>
        /// Keeps glitched cow milk vanilla
        /// </summary>
        private void PreserveGlitchedCowMilk()
        {
            ItemList[Item.ItemRanchBarnOtherCowMilk2].NewLocation = Item.ItemRanchBarnOtherCowMilk2;
        }

        /// <summary>
        /// Randomizes songs with other songs
        /// </summary>
        private void ShuffleSongs()
        {
            var itemPool = new List<Item>();
            // build check list
            for (var i = Item.SongHealing; i <= Item.SongOath; i++)
            {
                /*if (ItemList[i].NewLocation.HasValue) // this seems to do the oposite of what we want
                {
                    continue;
                }*/
                itemPool.Add(i);
            }

            // plando: if a song location was already selected
            foreach (var item in itemPool.ToList())
            {
                // if check already has an item, remove from list
                if (ItemList[item].NewLocation.HasValue)
                {
                    itemPool.Remove(ItemList[item].NewLocation.Value);
                }
            }

            // add songs to list
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
            // Make all items vanilla, and override using custom item list
            MakeAllItemsVanilla();

            // Should these be vanilla by default? Why not check settings.
            ApplyCustomItemList();

            // the above functions would cancel plando, must be at least here
            PlacePlandoItems();

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
                ItemList[location].NewLocation = location;
            }
        }

        /// <summary>
        /// Adds items specified from the Custom Item List to the randomizer pool, while keeping the rest vanilla
        /// </summary>
        private void ApplyCustomItemList()
        {
            if (_settings.CustomItemList.Contains(-1))
            {
                throw new Exception("Invalid custom item string.");
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
            }
        }

        /// <summary>
        /// Overwrite junk items with ice traps.
        /// </summary>
        /// <param name="iceTraps">Ice traps amount setting</param>
        /// <param name="appearance">Ice traps appearance setting</param>
        public void AddIceTraps(IceTraps iceTraps, IceTrapAppearance appearance)
        {
            var random = this.Random;

            // Select replaceable junk items of specified amount.
            var items = IceTrapUtils.SelectJunkItems(_randomized.ItemList, iceTraps, random);

            // Dynamically generate appearance set for ice traps.
            // Only mimic song items if they are included in the main randomization pool (not in their own pool).
            var mimics = IceTrapUtils.BuildIceTrapMimicSet(_randomized.ItemList, appearance, _randomized.Settings.AddSongs)
                .ToArray();

            var list = new List<ItemObject>();
            foreach (var item in items)
            {
                // If check is visible (can be seen via world model), add "graphic override" for imitating other item.
                var mimic = mimics[random.Next(mimics.Length)];
                item.ItemOverride = Item.IceTrap;
                item.Mimic = mimic;

                var newLocation = item.NewLocation.Value;
                if (newLocation.IsVisible() || newLocation.IsShop() || newLocation.IsPurchaseable())
                {
                    // Store name override for logging in HTML tracker.
                    item.NameOverride = $"{Item.IceTrap.Name()} ({mimic.Item.Name()})";

                    // If ice trap quirks enabled and placed as a shop item, use a fake shop item name.
                    if (_settings.IceTrapQuirks && (newLocation.IsShop() || newLocation.IsPurchaseable()))
                    {
                        item.Mimic.FakeName = FakeNameUtils.CreateFakeName(item.Mimic.Item.Name(), random);
                    }
                }

                if (_randomized.Settings.UpdateChests)
                {
                    // Choose chest type for ice trap appearance.
                    item.Mimic.ChestType = IceTrapUtils.GetIceTrapChestTypeOverride(appearance, random);
                }

                list.Add(item);
            }

            _randomized.IceTraps = list.AsReadOnly();
        }

        /// <summary>
        /// Randomizes the ROM with respect to the configured ruleset.
        /// </summary>
        public RandomizedResult Randomize(IProgressReporter progressReporter)
        {
            SeedRNG();

            _randomized = new RandomizedResult(_settings, _seed);

            if (_settings.LogicMode != LogicMode.Vanilla)
            {
                progressReporter.ReportProgress(5, "Preparing ruleset...");
                PrepareRulesetItemData();

                if (_settings.RandomizeDungeonEntrances)
                {
                    progressReporter.ReportProgress(10, "Shuffling entrances...");
                    EntranceShuffle();
                }

                _randomized.Logic = ItemList.Select(io => new ItemLogic(io)).ToList();

                progressReporter.ReportProgress(30, "Shuffling items...");
                RandomizeItems();

                foreach (var item in _settings.CustomStartingItemList)
                {
                    ItemList[item].ItemOverride = Item.RecoveryHeart;
                }

                // Replace junk items with ice traps according to settings.
                AddIceTraps(_randomized.Settings.IceTraps, _randomized.Settings.IceTrapAppearance);
                
                var freeItemIds = _settings.CustomStartingItemList
                    .Cast<int>()
                    .Union(ItemList.Where(io => io.NewLocation.HasValue && ItemUtils.IsStartingLocation(io.NewLocation.Value)).Select(io => io.ID))
                    .ToList();

                bool updated;
                do
                {
                    updated = false;
                    foreach (var itemLogic in _randomized.Logic.Where(il => ((Item)il.ItemId).IsFake() && !freeItemIds.Contains(il.ItemId)))
                    {
                        if ((itemLogic.RequiredItemIds?.All(id => freeItemIds.Contains(id)) != false)
                            && (itemLogic.ConditionalItemIds?.Any(c => c.All(id => freeItemIds.Contains(id))) != false)
                            && (itemLogic.RequiredItemIds != null || itemLogic.ConditionalItemIds != null))
                        {
                            freeItemIds.Add(itemLogic.ItemId);
                            updated = true;
                        }
                    }
                } while (updated);

                foreach (var itemLogic in _randomized.Logic)
                {
                    if (_settings.CustomStartingItemList.Contains((Item)itemLogic.ItemId) && !ItemList[itemLogic.ItemId].IsRandomized)
                    {
                        itemLogic.Acquired = true;
                    }

                    var keep = new List<int>();
                    for (var i = 0; itemLogic.ConditionalItemIds != null && i < itemLogic.ConditionalItemIds.Count; i++)
                    {
                        if (itemLogic.ConditionalItemIds[i].All(freeItemIds.Contains))
                        {
                            keep.Add(i);
                        }
                    }
                    if (keep.Count > 0)
                    {
                        for (var i = itemLogic.ConditionalItemIds.Count - 1; i >= 0; i--)
                        {
                            if (!keep.Contains(i))
                            {
                                itemLogic.ConditionalItemIds.RemoveAt(i);
                            }
                        }
                    }
                }

                var logicForRequiredItems = _settings.LogicMode == LogicMode.Casual && _settings.GossipHintStyle == GossipHintStyle.Competitive
                    ? _randomized.Logic.Select(il =>
                    {
                        var itemLogic = new ItemLogic(il);

                        // prevent Giant's Mask from being Way of the Hero.
                        itemLogic.RequiredItemIds.Remove((int)Item.MaskGiant);

                        return itemLogic;
                    }).ToList()
                    : _randomized.Logic;

                _randomized.ImportantItems = LogicUtils.GetImportantItems(ItemList, _settings, Item.AreaMoonAccess, _randomized.Logic)?.Important.Where(item => !item.IsFake()).ToList().AsReadOnly();
                if (_randomized.ImportantItems == null)
                {
                    throw new RandomizationException("Moon Access is unobtainable.");
                }
                var itemsRequiredForMoonAccess = new List<Item>();
                foreach (var item in _randomized.ImportantItems)
                {
                    var checkPaths = LogicUtils.GetImportantItems(ItemList, _settings, Item.AreaMoonAccess, logicForRequiredItems, exclude: item);
                    if (checkPaths == null)
                    {
                        itemsRequiredForMoonAccess.Add(item);
                    }
                }
                _randomized.ItemsRequiredForMoonAccess = itemsRequiredForMoonAccess.AsReadOnly();

                if (_settings.GossipHintStyle != GossipHintStyle.Default)
                {
                    progressReporter.ReportProgress(35, "Making gossip quotes...");

                    //gossip
                    SeedRNG();
                    MakeGossipQuotes();
                }

                SeedRNG();
                _randomized.FileSelectSkybox = Random.Next(360);
                _randomized.FileSelectColor = Random.Next(360);
                _randomized.TitleLogoColor = Random.Next(360);
            }

            return _randomized;
        }
    }

}