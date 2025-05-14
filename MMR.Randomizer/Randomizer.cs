using MMR.Common.Extensions;
using MMR.Randomizer.Attributes;
using MMR.Randomizer.Constants;
using MMR.Randomizer.Extensions;
using MMR.Randomizer.GameObjects;
using MMR.Randomizer.LogicMigrator;
using MMR.Randomizer.Models;
using MMR.Randomizer.Models.Rom;
using MMR.Randomizer.Models.Settings;
using MMR.Randomizer.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace MMR.Randomizer
{
    public class Randomizer
    {
        public static readonly string AssemblyVersion = typeof(Randomizer).Assembly.GetName().Version.ToString();

        private Random Random { get; set; }

        private ItemList ItemList { get; set; }

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
        private readonly List<Item> ForbiddenStartingItems = new List<Item>();

        private readonly List<List<Item>> ForcedCheckGroups = new List<List<Item>>
        {
            new List<Item>
            {
                Item.MaskKeaton,
                Item.TradeItemMamaLetter,
            },
            new List<Item>
            {
                Item.ItemBottleAliens,
                Item.NotebookSaveTheCows,
            },
            new List<Item>
            {
                Item.MaskRomani,
                Item.NotebookProtectMilkDelivery,
            },
            // TODO only if double archery rewards are enabled
            new List<Item>
            {
                Item.UpgradeBigQuiver,
                Item.HeartPieceTownArchery,
            },
            // TODO only if double archery rewards are enabled
            new List<Item>
            {
                Item.UpgradeBiggestQuiver,
                Item.HeartPieceSwampArchery,
            },
            new List<Item>
            {
                Item.TradeItemRoomKey,
                Item.NotebookInnReservation,
            },
            new List<Item>
            {
                Item.TradeItemKafeiLetter,
                Item.NotebookPromiseAnjuDelivery,
            },
            new List<Item>
            {
                Item.TradeItemPendant,
                Item.NotebookMeetKafei,
                Item.NotebookPromiseKafei,
            },
            new List<Item>
            {
                Item.MaskKeaton,
                Item.TradeItemMamaLetter,
                Item.NotebookCuriosityShopManSGift,
                Item.NotebookPromiseCuriosityShopMan,
            },
            new List<Item>
            {
                Item.SongEpona,
                Item.NotebookPromiseRomani,
            },
            new List<Item>
            {
                Item.ItemBottleMadameAroma,
                Item.NotebookDeliverLetterToMama,
            },
            new List<Item>
            {
                Item.ItemNotebook,
                Item.NotebookMeetBombers,
                Item.NotebookLearnBombersCode,
            },
            new List<Item>
            {
                Item.HeartPieceNotebookMayor,
                Item.NotebookDotoursThanks,
            },
            new List<Item>
            {
                Item.HeartPieceNotebookRosa,
                Item.NotebookRosaSistersThanks,
            },
            new List<Item>
            {
                Item.HeartPieceNotebookHand,
                Item.NotebookToiletHandSThanks,
            },
            new List<Item>
            {
                Item.HeartPieceNotebookGran1,
                Item.NotebookGrandmaShortStory,
            },
            new List<Item>
            {
                Item.HeartPieceNotebookGran2,
                Item.NotebookGrandmaLongStory,
            },
            new List<Item>
            {
                Item.HeartPieceNotebookPostman,
                Item.NotebookPostmansGame,
            },
            new List<Item>
            {
                Item.MaskKafei,
                Item.NotebookPromiseMadameAroma,
            },
            new List<Item>
            {
                Item.MaskAllNight,
                Item.NotebookPurchaseCuriosityShopItem,
            },
            new List<Item>
            {
                Item.MaskBunnyHood,
                Item.NotebookGrogsThanks,
            },
            new List<Item>
            {
                Item.MaskGaro,
                Item.NotebookDefeatGormanBrothers,
            },
            new List<Item>
            {
                Item.MaskCircusLeader,
                Item.NotebookMovingGorman,
            },
            new List<Item>
            {
                Item.MaskPostmanHat,
                Item.NotebookPostmansFreedom,
            },
            new List<Item>
            {
                Item.MaskCouple,
                Item.NotebookUniteAnjuAndKafei,
            },
            new List<Item>
            {
                Item.MaskBlast,
                Item.NotebookSaveOldLady,
            },
            new List<Item>
            {
                Item.MaskKamaro,
                Item.NotebookPromiseKamaro,
            },
            new List<Item>
            {
                Item.MaskStone,
                Item.NotebookSaveInvisibleSoldier,
            },
            new List<Item>
            {
                Item.MaskBremen,
                Item.NotebookGuruGuru,
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
                ForbiddenStartingItems.AddRange(ItemUtils.DowngradableItems());
            }

            ItemExtensions.PrepareSettings(settings);
        }

        //rando functions

        #region Gossip quotes

        private void MakeGossipQuotes()
        {
            _randomized.GossipQuotes = new List<MessageEntry>();
            var hintedRegions = new List<Region>();
            var hintedItems = new List<ItemObject>();
            if (_settings.GossipHintStyle != GossipHintStyle.Default)
            {
                var moonHints = Enum.GetValues<GossipQuote>().Where(gq => gq.IsMoonGossipStone());
                _randomized.GossipQuotes.AddRange(MessageUtils.MakeGossipQuotes(moonHints, GossipHintStyle.Relevant, _randomized, 0, 0, 0, hintedRegions, hintedItems));
            }

            var numberOfRequiredGaroHints = _settings.OverrideNumberOfRequiredGaroHints ?? 2;
            var numberOfNonRequiredGaroHints = _settings.OverrideNumberOfNonRequiredGaroHints ?? 2;
            var maxNumberOfClockTownGaroHints = _settings.OverrideMaxNumberOfClockTownGaroHints ?? 2;

            var numberOfRequiredGossipHints = _settings.OverrideNumberOfRequiredGossipHints ?? (_settings.AddSongs ? 4 : 3);
            var numberOfNonRequiredGossipHints = _settings.OverrideNumberOfNonRequiredGossipHints ?? 3;
            var maxNumberOfClockTownGossipHints = _settings.OverrideMaxNumberOfClockTownGossipHints ?? 2;

            if (_settings.GaroHintStyle != _settings.GossipHintStyle || !_settings.MixGossipAndGaroHints)
            {
                if (_settings.GossipHintStyle != GossipHintStyle.Default)
                {
                    var gossipHints = Enum.GetValues<GossipQuote>().Where(gq => !gq.IsMoonGossipStone() && !gq.IsGaroHint());
                    _randomized.GossipQuotes.AddRange(MessageUtils.MakeGossipQuotes(
                        gossipHints,
                        _settings.GossipHintStyle,
                        _randomized,
                        numberOfRequiredGossipHints,
                        numberOfNonRequiredGossipHints,
                        maxNumberOfClockTownGossipHints,
                        hintedRegions,
                        hintedItems
                    ));
                }

                if (_settings.GaroHintStyle != GossipHintStyle.Default)
                {
                    var garoHints = Enum.GetValues<GossipQuote>().Where(gq => gq.IsGaroHint());
                    _randomized.GossipQuotes.AddRange(MessageUtils.MakeGossipQuotes(
                        garoHints,
                        _settings.GaroHintStyle,
                        _randomized,
                        numberOfRequiredGaroHints,
                        numberOfNonRequiredGaroHints,
                        maxNumberOfClockTownGaroHints,
                        hintedRegions,
                        hintedItems
                    ));
                }
            }
            else if (_settings.GossipHintStyle != GossipHintStyle.Default)
            {
                var hints = Enum.GetValues<GossipQuote>().Where(gq => !gq.IsMoonGossipStone());
                _randomized.GossipQuotes.AddRange(MessageUtils.MakeGossipQuotes(
                    hints,
                    _settings.GossipHintStyle,
                    _randomized,
                    numberOfRequiredGaroHints + numberOfRequiredGossipHints,
                    numberOfNonRequiredGaroHints + numberOfNonRequiredGossipHints,
                    maxNumberOfClockTownGaroHints + maxNumberOfClockTownGossipHints,
                    hintedRegions,
                    hintedItems
                ));
            }
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

            var changesToMake = new Dictionary<Item, Item>();

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
                changesToMake[exit] = ItemList[targetExit].NewLocation ?? targetExit;
            }

            foreach (var kvp in changesToMake)
            {
                ItemList[kvp.Key].NewLocation = kvp.Value;
                ItemList[kvp.Key].IsRandomized = true;
            }
        }

        private void BossShuffle()
        {
            var bossEntrances = new List<Item>
            {
                Item.AreaOdolwasLair,
                Item.AreaGohtsLair,
                Item.AreaGyorgsLair,
                Item.AreaTwinmoldsLair,
            };

            var bossExits = new List<Item>
            {
                Item.AreaWoodFallTempleClear,
                Item.AreaSnowheadTempleClear,
                Item.AreaGreatBayTempleClear,
                Item.AreaStoneTowerClear,
            };

            var bossKills = new List<Item>
            {
                Item.OtherKillOdolwa,
                Item.OtherKillGoht,
                Item.OtherKillGyorg,
                Item.OtherKillTwinmold,
            };

            var bossRemains = ItemUtils.BossRemains().ToList();

            var randomized = Enumerable.Range(0, 4).ToList().OrderBy(_ => Random.Next()).ToList();

            //var changes = new List<Action>();
            var dependenciesToChange = new List<(int, int, Item)>();
            var conditionalsToChange = new List<(int, int, int, Item)>();

            for (var i = 0; i < randomized.Count; i++)
            {
                var fromIndex = i;
                var toIndex = randomized[i];

                var entrance = bossEntrances[fromIndex];
                var targetEntrance = bossEntrances[toIndex];

                var exit = bossExits[toIndex];
                var targetExit = bossExits[fromIndex];

                var kill = bossKills[toIndex];
                var targetKill = bossKills[fromIndex];

                ItemList[entrance].NewLocation = targetEntrance;
                ItemList[entrance].IsRandomized = true;
                ItemList[exit].NewLocation = targetExit;
                ItemList[exit].IsRandomized = true;
                ItemList[kill].NewLocation = targetKill;

                var remain = bossRemains[toIndex];
                var targetRemain = bossRemains[fromIndex];

                foreach (var io in ItemList)
                {
                    for (var ci = 0; ci < io.Conditionals.Count; ci++)
                    {
                        for (var cj = 0; cj < io.Conditionals[ci].Count; cj++)
                        {
                            if (io.Conditionals[ci][cj] == remain)
                            {
                                conditionalsToChange.Add((io.ID, ci, cj, targetRemain));
                            }
                        }
                    }
                    for (var di = 0; di < io.DependsOnItems.Count; di++)
                    {
                        if (io.DependsOnItems[di] == remain)
                        {
                            dependenciesToChange.Add((io.ID, di, targetRemain));
                        }
                    }
                }
            }

            foreach (var (item, i, j, newRemain) in conditionalsToChange)
            {
                ItemList[item].Conditionals[i][j] = newRemain;
            }

            foreach (var (item, i, newRemain) in dependenciesToChange)
            {
                ItemList[item].DependsOnItems[i] = newRemain;
            }
        }

        private void UpdateLogicForSettings()
        {
            var settingJunkedLocations = new List<Item>();

            foreach (var itemObject in ItemList)
            {
                itemObject.DependsOnItems?.RemoveAll(item => _settings.CustomStartingItemList.Contains(item));
                itemObject.Conditionals?.ForEach(c => c.RemoveAll(item => _settings.CustomStartingItemList.Contains(item)));

                if (itemObject.Conditionals?.Any(c => !c.Any()) == true)
                {
                    itemObject.Conditionals.Clear();
                }

                if (itemObject.IsTrick && !_settings.EnabledTricks.Contains(itemObject.Name))
                {
                    settingJunkedLocations.Add(itemObject.Item);
                    itemObject.DependsOnItems?.Clear();
                    itemObject.Conditionals?.Clear();
                }

                if (!LogicUtils.IsSettingEnabled(_settings, itemObject.SettingExpression))
                {
                    settingJunkedLocations.Add(itemObject.Item);
                    itemObject.DependsOnItems?.Clear();
                    itemObject.Conditionals?.Clear();
                }
            }

            if (_settings.LogicMode != LogicMode.NoLogic)
            {
                settingJunkedLocations.Add(Item.OtherInaccessible);
            }

            ItemUtils.PrepareTricksAndSettings(settingJunkedLocations, ItemList);

            if (_settings.RequiredBossRemains < 4)
            {
                ItemList[Item.AreaMoonAccess].DependsOnItems.RemoveAll(ItemUtils.BossRemains().Contains);
                if (_settings.RequiredBossRemains > 0)
                {
                    var requiredBossRemains = new ItemObject
                    {
                        ID = ItemList.Count,
                        TimeAvailable = 63,
                        Conditionals = ItemUtils.BossRemains().Combinations(_settings.RequiredBossRemains).Select(a => a.ToList()).ToList(),
                    };
                    ItemList.Add(requiredBossRemains);
                    ItemList[Item.AreaMoonAccess].DependsOnItems.Add(requiredBossRemains.Item);
                }
            }

            if (_settings.VictoryMode != VictoryMode.Default)
            {
                if (_settings.VictoryMode.HasFlag(VictoryMode.DirectToCredits))
                {
                    ItemList[Item.OtherCredits].DependsOnItems.Remove(Item.AreaMoonAccess);
                    ItemList[Item.OtherCredits].DependsOnItems.Remove(Item.OtherKillMajora);
                }

                if (_settings.VictoryMode.HasFlag(VictoryMode.Fairies))
                {
                    var requiredFairies = new ItemObject
                    {
                        ID = ItemList.Count,
                        TimeAvailable = 63,
                        DependsOnItems = ItemUtils.DungeonStrayFairies().ToList(),
                    };
                    ItemList.Add(requiredFairies);
                    ItemList[Item.OtherCredits].DependsOnItems.Add(requiredFairies.Item);
                }

                if (_settings.VictoryMode.HasFlag(VictoryMode.SkullTokens))
                {
                    var requiredSkulltulas= new ItemObject
                    {
                        ID = ItemList.Count,
                        TimeAvailable = 63,
                        DependsOnItems = ItemUtils.OceanSkulltulaTokens().Union(ItemUtils.SwampSkulltulaTokens()).ToList(),
                    };
                    ItemList.Add(requiredSkulltulas);
                    ItemList[Item.OtherCredits].DependsOnItems.Add(requiredSkulltulas.Item);
                }

                if (_settings.VictoryMode.HasFlag(VictoryMode.NonTransformationMasks))
                {
                    var requiredMasks = new ItemObject
                    {
                        ID = ItemList.Count,
                        TimeAvailable = 63,
                        DependsOnItems = Enumerable.Range((int)Item.MaskPostmanHat, 20).Cast<Item>().ToList(),
                    };
                    ItemList.Add(requiredMasks);
                    ItemList[Item.OtherCredits].DependsOnItems.Add(requiredMasks.Item);
                }

                if (_settings.VictoryMode.HasFlag(VictoryMode.TransformationMasks))
                {
                    var requiredMasks = new ItemObject
                    {
                        ID = ItemList.Count,
                        TimeAvailable = 63,
                        DependsOnItems = new List<Item> { Item.MaskDeku, Item.MaskGoron, Item.MaskZora, Item.MaskFierceDeity },
                    };
                    ItemList.Add(requiredMasks);
                    ItemList[Item.OtherCredits].DependsOnItems.Add(requiredMasks.Item);
                }

                if (_settings.VictoryMode.HasFlag(VictoryMode.Notebook))
                {
                    var requiredNotebookEntries = new ItemObject
                    {
                        ID = ItemList.Count,
                        TimeAvailable = 63,
                        DependsOnItems = Enumerable.Range((int)Item.NotebookMeetBombers, 51).Cast<Item>().ToList(),
                    };
                    ItemList.Add(requiredNotebookEntries);
                    ItemList[Item.OtherCredits].DependsOnItems.Add(requiredNotebookEntries.Item);
                }

                if (_settings.VictoryMode.HasFlag(VictoryMode.Hearts))
                {
                    var requiredHearts = new ItemObject
                    {
                        ID = ItemList.Count,
                        TimeAvailable = 63,
                        DependsOnItems = Enum.GetValues<Item>().Where(item => item.ItemCategory() == ItemCategory.PiecesOfHeart || item.ItemCategory() == ItemCategory.HeartContainers).ToList(),
                    };
                    ItemList.Add(requiredHearts);
                    ItemList[Item.OtherCredits].DependsOnItems.Add(requiredHearts.Item);
                }

                var bossRemainsAmount = 0;
                if (_settings.VictoryMode.HasFlag(VictoryMode.FourBossRemains))
                {
                    bossRemainsAmount = 4;
                }
                else if (_settings.VictoryMode.HasFlag(VictoryMode.ThreeBossRemains))
                {
                    bossRemainsAmount = 3;
                }
                else if (_settings.VictoryMode.HasFlag(VictoryMode.TwoBossRemains))
                {
                    bossRemainsAmount = 2;
                }
                else if (_settings.VictoryMode.HasFlag(VictoryMode.OneBossRemains))
                {
                    bossRemainsAmount = 1;
                }
                if (bossRemainsAmount > 0)
                {
                    var requiredBossRemains = new ItemObject
                    {
                        ID = ItemList.Count,
                        TimeAvailable = 63,
                    };
                    if (bossRemainsAmount == 4)
                    {
                        requiredBossRemains.DependsOnItems = ItemUtils.BossRemains().ToList();
                    }
                    else
                    {
                        requiredBossRemains.Conditionals = ItemUtils.BossRemains().Combinations(bossRemainsAmount).Select(c => c.ToList()).ToList();
                    }
                    ItemList.Add(requiredBossRemains);
                    ItemList[Item.OtherCredits].DependsOnItems.Add(requiredBossRemains.Item);
                }
            }

            if (_settings.FreeHints)
            {
                for (var item = Item.GossipTerminaSouth; item <= Item.GossipTerminaGossipDrums; item++)
                {
                    ItemList[item].DependsOnItems.Remove(Item.MaskTruth);
                }
            }

            if (_settings.FreeGaroHints)
            {
                for (var item = Item.HintGaroCanyonLower1; item <= Item.HintGaroCastleUpper; item++)
                {
                    ItemList[item].DependsOnItems.Remove(Item.MaskGaro);
                }
            }

            if (_settings.CustomItemList.Contains(Item.ShopItemBusinessScrubMagicBean))
            {
                ItemList[Item.ShopItemBusinessScrubMagicBeanInSwamp].DependsOnItems.Remove(Item.OtherMagicBean);
                ItemList[Item.ShopItemBusinessScrubMagicBeanInTown].DependsOnItems.Remove(Item.OtherMagicBean);
            }

            if (_settings.CustomItemList.Any(item => item.ItemCategory() == ItemCategory.ScoopedItems) && _settings.LogicMode == LogicMode.Casual)
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
                    io.Item.IsFake()
                    && io.DependsOnItems.Count == 0
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

            var bombchu10 = ItemList
                .FirstOrDefault(io =>
                    io.Item.IsFake()
                    && io.DependsOnItems.Count == 0
                    && io.Conditionals.Count == 3
                    && io.Conditionals.Any(c => c.SequenceEqual(new List<Item> { Item.ChestInvertedStoneTowerBombchu10 }))
                    && io.Conditionals.Any(c => c.SequenceEqual(new List<Item> { Item.ChestLinkTrialBombchu10 })
                    && io.Conditionals.Any(c => c.SequenceEqual(new List<Item> { Item.ShopItemBombsBombchu10 }))));
            if (bombchu10 == null)
            {
                bombchu10 = new ItemObject
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
            }

            if (_settings.ByoAmmo)
            {
                ItemList[Item.ChestInvertedStoneTowerBombchu10].TimeNeeded = 1;
                ItemList[Item.ChestLinkTrialBombchu10].TimeNeeded = 1;
                ItemList[Item.ShopItemBombsBombchu10].TimeNeeded = 1;

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

            if (_settings.ProgressiveUpgrades)
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
                        io.Item.IsFake()
                        && io.DependsOnItems.Count == 0
                        && io.Conditionals.Count == 3
                        && io.Conditionals.Any(c => c.SequenceEqual(new List<Item> { Item.ItemBombBag }))
                        && io.Conditionals.Any(c => c.SequenceEqual(new List<Item> { Item.UpgradeBigBombBag }))
                        && io.Conditionals.Any(c => c.SequenceEqual(new List<Item> { Item.UpgradeBiggestBombBag })));

                var bombs30 = ItemList
                    .FirstOrDefault(io =>
                        io.Item.IsFake()
                        && io.DependsOnItems.Count == 0
                        && io.Conditionals.Count == 2
                        && io.Conditionals.Any(c => c.SequenceEqual(new List<Item> { Item.UpgradeBigBombBag }))
                        && io.Conditionals.Any(c => c.SequenceEqual(new List<Item> { Item.UpgradeBiggestBombBag })));
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
                        io.Item.IsFake()
                        && io.DependsOnItems.Count == 0
                        && io.Conditionals.Count == 3
                        && io.Conditionals.Any(c => c.SequenceEqual(new List<Item> { Item.StartingSword }))
                        && io.Conditionals.Any(c => c.SequenceEqual(new List<Item> { Item.UpgradeRazorSword }))
                        && io.Conditionals.Any(c => c.SequenceEqual(new List<Item> { Item.UpgradeGildedSword })));

                var sword2 = ItemList
                    .FirstOrDefault(io =>
                        io.Item.IsFake()
                        && io.DependsOnItems.Count == 0
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
                        io.Item.IsFake()
                        && io.DependsOnItems.Count == 0
                        && io.Conditionals.Count == 3
                        && io.Conditionals.Any(c => c.SequenceEqual(new List<Item> { Item.UpgradeAdultWallet }))
                        && io.Conditionals.Any(c => c.SequenceEqual(new List<Item> { Item.UpgradeGiantWallet }))
                        && io.Conditionals.Any(c => c.SequenceEqual(new List<Item> { Item.UpgradeRoyalWallet })));

                var wallets500 = ItemList
                    .FirstOrDefault(io =>
                        io.Item.IsFake()
                        && io.DependsOnItems.Count == 0
                        && io.Conditionals.Count == 2
                        && io.Conditionals.Any(c => c.SequenceEqual(new List<Item> { Item.UpgradeGiantWallet }))
                        && io.Conditionals.Any(c => c.SequenceEqual(new List<Item> { Item.UpgradeRoyalWallet })));
                if (wallets500 == null)
                {
                    wallets500 = new ItemObject
                    {
                        ID = ItemList.Count,
                        TimeAvailable = 63,
                        Conditionals = new List<Item>
                    {
                        Item.UpgradeAdultWallet,
                        Item.UpgradeGiantWallet,
                        Item.UpgradeRoyalWallet,
                    }.Combinations(2).Select(a => a.ToList()).ToList(),
                    };
                    ItemList.Add(wallets500);
                }
                else
                {
                    wallets500.Conditionals.Clear();
                    wallets500.Conditionals.AddRange(new List<Item>
                {
                    Item.UpgradeAdultWallet,
                    Item.UpgradeGiantWallet,
                    Item.UpgradeRoyalWallet,
                }.Combinations(2).Select(a => a.ToList()));
                }

                var wallets999 = new ItemObject
                {
                    ID = ItemList.Count,
                    TimeAvailable = 63,
                    DependsOnItems = new List<Item>
                {
                    Item.UpgradeAdultWallet,
                    Item.UpgradeGiantWallet,
                    Item.UpgradeRoyalWallet,
                },
                };
                ItemList.Add(wallets999);

                var magicAny = ItemList
                    .FirstOrDefault(io =>
                        io.Item.IsFake()
                        && io.DependsOnItems.Count == 0
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

                var lullabyAny = ItemList
                    .FirstOrDefault(io =>
                        io.Item.IsFake()
                        && io.DependsOnItems.Count == 0
                        && io.Conditionals.Count == 2
                        && io.Conditionals.Any(c => c.SequenceEqual(new List<Item> { Item.SongLullaby }))
                        && io.Conditionals.Any(c => c.SequenceEqual(new List<Item> { Item.SongLullabyIntro })));

                var lullabyFull = new ItemObject
                {
                    ID = ItemList.Count,
                    TimeAvailable = 63,
                    DependsOnItems = new List<Item>
                {
                    Item.SongLullaby,
                    Item.SongLullabyIntro,
                },
                };
                ItemList.Add(lullabyFull);

                foreach (var itemObject in ItemList)
                {
                    if (itemObject != lullabyFull && itemObject.DependsOnItems.Contains(Item.SongLullaby))
                    {
                        itemObject.DependsOnItems.Remove(Item.SongLullaby);
                        itemObject.DependsOnItems.Add(lullabyFull.Item);
                    }

                    if (itemObject != lullabyAny)
                    {
                        foreach (var conditions in itemObject.Conditionals)
                        {
                            if (conditions.Contains(Item.SongLullaby))
                            {
                                conditions.Remove(Item.SongLullaby);
                                conditions.Add(lullabyFull.Item);
                            }
                        }
                    }

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

                    if (itemObject != wallets999 && itemObject.DependsOnItems.Contains(Item.UpgradeRoyalWallet))
                    {
                        itemObject.DependsOnItems.Remove(Item.UpgradeRoyalWallet);
                        itemObject.DependsOnItems.Add(wallets999.Item);
                    }

                    if (itemObject != wallets200 && itemObject != wallets500)
                    {
                        foreach (var conditions in itemObject.Conditionals)
                        {
                            if (conditions.Contains(Item.UpgradeRoyalWallet))
                            {
                                conditions.Remove(Item.UpgradeRoyalWallet);
                                conditions.Add(wallets999.Item);
                            }

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

            if (_settings.BombchuDrops)
            {
                bombchu10.Conditionals.Add(new List<Item> { Item.ChestIkanaSecretShrineGrotto });
                bombchu10.Conditionals.Add(new List<Item> { Item.ChestTerminaGrottoBombchu });
                bombchu10.Conditionals.Add(new List<Item> { Item.ChestGreatBayCapeGrotto });
                bombchu10.Conditionals.Add(new List<Item> { Item.ChestGraveyardGrotto });
                bombchu10.Conditionals.Add(new List<Item> { Item.ChestToIkanaGrotto });
                bombchu10.Conditionals.Add(new List<Item> { Item.ChestToGoronRaceGrotto });
            }

            if (!_settings.CustomItemList.Contains(Item.ChestLinkTrialBombchu10))
            {
                ItemList[Item.HeartPieceLinkTrial].Conditionals.ForEach(c => c.Remove(bombchu10.Item));
                var completeLinkTrial = ItemList.FirstOrDefault(io => io.Name == "Complete Link Trial");
                if (completeLinkTrial != null)
                {
                    completeLinkTrial.Conditionals.ForEach(c => c.Remove(bombchu10.Item));
                }
            }

            Func<ItemObject, bool> filter = io => !io.ItemOverride.HasValue
                && !_settings.CustomItemList.Contains(io.Item)
                && (!io.Item.MainLocation().HasValue || !_settings.CustomItemList.Contains(io.Item.MainLocation().Value))
                && io.DependsOnItems.Count == 0
                && io.Conditionals.Count == 0;

            var unrandomizedSphereZeroItems = ItemList
                .Where(filter)
                .Select(io => io.Item)
                .ToList();
            unrandomizedSphereZeroItems.Remove(Item.OtherInaccessible);
            bool updated;
            do
            {
                updated = false;
                foreach (var itemObject in ItemList)
                {
                    var location = (Item)itemObject.ID;
                    if (unrandomizedSphereZeroItems.Contains(location) || location == Item.OtherInaccessible)
                    {
                        continue;
                    }

                    itemObject.DependsOnItems.RemoveAll(unrandomizedSphereZeroItems.Contains);
                    itemObject.Conditionals.ForEach(c => c.RemoveAll(unrandomizedSphereZeroItems.Contains));

                    if (itemObject.Conditionals.Any(c => !c.Any()))
                    {
                        itemObject.Conditionals.Clear();
                    }

                    if (filter(itemObject))
                    {
                        unrandomizedSphereZeroItems.Add(itemObject.Item);
                        updated = true;
                    }
                }
            } while (updated);

            foreach (var itemObject in ItemList)
            {
                LogicUtils.Simplify(itemObject.Conditionals);
            }
        }

        private void PrepareRulesetItemData()
        {
            if (_settings.LogicMode == LogicMode.Casual
                || _settings.LogicMode == LogicMode.Glitched
                || _settings.LogicMode == LogicMode.UserLogic)
            {
                var data = LogicUtils.ReadRulesetFromResources(_settings.LogicMode, _settings.UserLogicFileName);
                ItemList = LogicUtils.PopulateItemListFromLogicData(data);
            }
            else
            {
                // TODO if failed to load glitched logic, let user decide to continue without any logic
                //ItemList = LogicUtils.PopulateItemListWithoutLogic();
                var data = LogicUtils.ReadRulesetFromResources(LogicMode.Glitched, null);
                ItemList = LogicUtils.PopulateItemListFromLogicData(data);
                ItemList.ForEach(io => io.IsTrick = false);
            }
        }

        private void PrepareAdditionalItemData()
        {
            RandomizePrices();

            UpdateLogicForSettings();

            ItemUtils.PrepareJunkItems(_settings, ItemList);
            _randomized.BlitzExtraItems = new List<Item>();
            if (_settings.CustomJunkLocations.Count > ItemUtils.JunkItems.Count) // TODO also account for HintedJunkLocations and BlitzJunkLocations
            {
                throw new Exception($"Too many Enforced Junk Locations. Select up to {ItemUtils.JunkItems.Count}.");
            }
        }

        private void SeedRNG()
        {
            Random = new Random(_seed);
        }

        private Dependence CheckDependence(Item currentItem, Item target, List<Item> dependencyPath)
        {
            var targetName = target.Location() ?? ItemList[target].Name;
            Debug.WriteLine($"CheckDependence({currentItem.Name()}, {targetName}[{target}])");
            var currentItemObject = ItemList[currentItem];
            var currentTargetObject = ItemList[target];

            if (currentItemObject.TimeNeeded == 0 && ItemUtils.IsLogicallyJunk(currentItem))
            {
                return Dependence.NotDependent;
            }

            //check timing
            if (currentItemObject.TimeNeeded != 0 && (!_timeTravelPlaced || (currentItem.IsTemporary() && dependencyPath.Skip(1).All(p => p.IsFake() || ItemList.Single(i => i.NewLocation == p).Item.IsTemporary()))))
            {
                if ((currentItemObject.TimeNeeded & currentTargetObject.TimeAvailable) == 0)
                {
                    Debug.WriteLine($"{currentItem} is needed at {currentItemObject.TimeNeeded} but {targetName} is only available at {currentTargetObject.TimeAvailable}");
                    return Dependence.Dependent;
                }
            }

            if (currentTargetObject.Conditionals.Any())
            {
                if (currentTargetObject.Conditionals.All(u => u.Contains(currentItem)))
                {
                    Debug.WriteLine($"All conditionals of {targetName} contains {currentItem}");
                    return Dependence.Dependent;
                }

                foreach (var cannotRequireItem in currentItemObject.CannotRequireItems)
                {
                    if (currentTargetObject.Conditionals.All(u => u.Contains(cannotRequireItem) || u.Contains(currentItem)))
                    {
                        Debug.WriteLine($"All conditionals of {targetName} cannot be required by {currentItem}");
                        return Dependence.Dependent;
                    }
                }

                int k = 0;
                var circularDependencies = new List<Item>();
                var conditionRemoves = new List<int[]>();
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
                        if (ItemList[d].Item < 0)
                        {
                            continue;
                        }

                        if (d == currentItem)
                        {
                            DependenceChecked[d] = Dependence.Dependent;
                        }
                        else
                        {
                            if (!_timeTravelPlaced && _timeTravelPath.Contains(d))
                            {
                                DependenceChecked[ItemList[d].NewLocation ?? d] = Dependence.Dependent;
                            }
                            d = ItemList[d].NewLocation ?? d;
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
                                int[] check = new int[] { (int)target, i, j };

                                if (!conditionRemoves.Any(c => c.SequenceEqual(check)))
                                {
                                    conditionRemoves.Add(check);
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
                    Debug.WriteLine($"All conditionals of {targetName} failed dependency check for {currentItem}.");
                    return Dependence.Dependent;
                }
                else
                {
                    foreach (var cr in conditionRemoves)
                    {
                        if (!ConditionRemoves.Any(c => c.SequenceEqual(cr)))
                        {
                            ConditionRemoves.Add(cr);
                        }
                    }
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
                    Debug.WriteLine($"{currentItem} cannot require dependence {cannotRequireItem} of {targetName}.");
                    return Dependence.Dependent;
                }
            }

            var currentItemIsTemporary = !_timeTravelPlaced || currentItem.IsTemporary();

            //cycle through all things
            foreach (var dependency in currentTargetObject.DependsOnItems)
            {
                if (!currentItemIsTemporary
                    && (target == Item.MaskBlast || target == Item.NotebookSaveOldLady || target == Item.UpgradeBigBombBag)
                    && (dependency == Item.TradeItemKafeiLetter || dependency == Item.TradeItemPendant))
                {
                    // Permanent items ignore Kafei Letter and Pendant on Blast Mask check.
                    continue;
                }
                if (ItemList[dependency].Item < 0)
                {
                    continue;
                }
                if (dependency == currentItem)
                {
                    Debug.WriteLine($"{targetName} has direct dependence on {currentItem}");
                    return Dependence.Dependent;
                }

                if (dependency.IsFake()
                    || ItemList[dependency].NewLocation.HasValue)
                {
                    if (!_timeTravelPlaced && _timeTravelPath.Contains(dependency))
                    {
                        Debug.WriteLine($"{dependency} has already been placed and must be avoided as a requirement during time travel logic.");
                        return Dependence.Dependent;
                    }

                    var location = ItemList[dependency].NewLocation ?? dependency;

                    var locationName = location.Location() ?? ItemList[location].Name;

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
                        Debug.WriteLine($"{currentItem} is dependent on {locationName} ({location})");
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
            if (target == Item.MaskBlast || target == Item.NotebookSaveOldLady || target == Item.UpgradeBigBombBag)
            {
                if (_timeTravelPlaced && !currentItem.IsTemporary())
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
            if (currentItem < 0)
            {
                return true;
            }

            if (_settings.CustomStartingItemList.Contains(currentItem))
            {
                return true;
            }

            if (_randomized.BlitzExtraItems.Contains(currentItem))
            {
                return true;
            }

            if (ItemUtils.IsStartingLocation(target) && ForbiddenStartingItems.Contains(currentItem))
            {
                Debug.WriteLine($"{currentItem} cannot be a starting item.");
                return false;
            }

            if ((ItemUtils.IsLocationJunk(target, _settings) || target == Item.UpgradeRoyalWallet) && !ItemUtils.IsJunk(currentItem))
            {
                return false;
            }

            if (_settings.LogicMode == LogicMode.NoLogic)
            {
                return true;
            }

            if (ForbiddenPlacedAt.ContainsKey(currentItem)
                && ForbiddenPlacedAt[currentItem].Contains(target))
            {
                Debug.WriteLine($"{currentItem} forbidden from being placed at {target}");
                return false;
            }

            var overwritableSlot = currentItem.OverwriteableSlot();

            if (overwritableSlot != OverwritableAttribute.ItemSlot.None)
            {
                var forcedCheckGroup = ForcedCheckGroups.FirstOrDefault(locations => locations.Contains(target));
                if (forcedCheckGroup != default)
                {
                    var slotItems = ItemUtils.OverwriteableSlotItems()[overwritableSlot];

                    foreach (var slotItem in slotItems)
                    {
                        if (ItemList[slotItem].NewLocation.HasValue && forcedCheckGroup.Contains(ItemList[slotItem].NewLocation.Value))
                        {
                            return false;
                        }
                    }
                }
            }

            if (!_timeTravelPlaced || currentItem.IsTemporary())
            {
                if ((target.Region(ItemList) == Region.TheMoon || target.Region(ItemList) == Region.ClockTowerRoof) && currentItem.ItemCategory() != ItemCategory.TimeTravel)
                {
                    Debug.WriteLine($"{currentItem} is temporary and cannot be placed on the moon or clock tower roof.");
                    return false;
                }

                // This is to prevent business scrub relocation logic from potentially causing unbeatable seeds.
                // TODO fix this in a nicer way.
                if ((target == Item.HeartPieceNotebookHand || target == Item.NotebookToiletHandSThanks) && !ItemUtils.IsLogicallyJunk(currentItem))
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

            if (currentItem == Item.SongTime)
            {
                var targetRegion = target.Region(ItemList);
                if (targetRegion != Region.ClockTowerRoof && targetRegion != Region.TheMoon)
                {
                    var returnFromTheMoon = new ItemObject
                    {
                        ID = ItemList.Count,
                        TimeAvailable = 63,
                    };
                    if (!_settings.VictoryMode.HasFlag(VictoryMode.CantFightMajora))
                    {
                        returnFromTheMoon.Conditionals.Add(new List<Item> { Item.SongTime });
                        returnFromTheMoon.Conditionals.Add(new List<Item> { Item.OtherKillMajora });
                    }
                    ItemList.Add(returnFromTheMoon);

                    var returnFromTheRoof = new ItemObject
                    {
                        ID = ItemList.Count,
                        TimeAvailable = 63,
                        Conditionals = new List<List<Item>>
                        {
                            new List<Item>()
                            {
                                Item.SongTime,
                            },
                            ItemList[Item.AreaMoonAccess].DependsOnItems
                                .Where(ItemUtils.BossRemains().Contains)
                                .Append(Item.SongOath)
                                .Append(returnFromTheMoon.Item)
                                .ToList(),
                        }
                    };
                    ItemList.Add(returnFromTheRoof);

                    foreach (var itemObject in ItemList)
                    {
                        var region = itemObject.Item.Region(ItemList);
                        if (region == Region.ClockTowerRoof)
                        {
                            itemObject.DependsOnItems.Add(returnFromTheRoof.Item);
                        }
                        else if (region == Region.TheMoon)
                        {
                            itemObject.DependsOnItems.Add(returnFromTheMoon.Item);
                        }
                    }
                }
            }

            return true;
        }

        private void UpdateTimeNeeded(Item sourceItem, Item targetItem)
        {
            if (!_timeTravelPlaced)
            {
                var source = ItemList[sourceItem];
                var target = ItemList[targetItem];
                var sourceLocation = ItemList[source.NewLocation ?? sourceItem];
                if (source.TimeNeeded != 0)
                {
                    if (target.TimeNeeded == 0)
                    {
                        target.TimeNeeded = source.TimeNeeded;
                    }
                    else
                    {
                        target.TimeNeeded &= source.TimeNeeded;
                        if (target.TimeNeeded == 0)
                        {
                            target.TimeNeeded = source.TimeNeeded;
                        }
                    }
                }
                if (sourceLocation.TimeSetup != 0)
                {
                    if (target.TimeNeeded == 0)
                    {
                        target.TimeNeeded = sourceLocation.TimeSetup;
                    }
                    else
                    {
                        target.TimeNeeded &= sourceLocation.TimeSetup;
                        if (target.TimeNeeded == 0)
                        {
                            target.TimeNeeded = source.TimeSetup;
                        }
                    }
                }
            }
        }

        private void PlaceRequirements(Item currentItem, List<Item> targets)
        {
            if (!ItemUtils.IsLogicallyJunk(currentItem))
            {
                _timeTravelPath.Push(currentItem);

                var currentItemObject = ItemList[currentItem];
                var location = ItemList[currentItemObject.NewLocation.Value];
                var placed = new List<Item>();
                foreach (var requiredItem in location.DependsOnItems.AllowModification().Where(item => item.IsSameType(currentItem)))
                {
                    UpdateTimeNeeded(currentItem, requiredItem);

                    PlaceItem(requiredItem, targets);
                }
                var conditional = _timeTravelChosenConditionals.FirstOrDefault(location.Conditionals.Contains);
                if (conditional == null)
                {
                    conditional = location.Conditionals.RandomOrDefault(Random);
                    _timeTravelChosenConditionals.Add(conditional);
                }
                if (conditional != null)
                {
                    foreach (var item in conditional.AllowModification().Where(item => item.IsSameType(currentItem)))
                    {
                        UpdateTimeNeeded(currentItem, item);

                        PlaceItem(item, targets);
                    }
                }

                _timeTravelPath.Pop();
            }
        }

        private void PlaceItem(Item currentItem, List<Item> targets, Func<Item, Item, ItemList, bool> restriction = null, bool placeJunk = false)
        {
            var currentItemObject = ItemList[currentItem];
            if (!_timeTravelPlaced && currentItem.IsFake())
            {
                _timeTravelPath.Push(currentItem);

                // TODO need to handle items that are already placed, and scoops
                foreach (var requiredItem in currentItemObject.DependsOnItems.AllowModification().Where(item => item.IsSameType(currentItem)))
                {
                    UpdateTimeNeeded(currentItem, requiredItem);

                    PlaceItem(requiredItem, targets);
                }
                var conditional = _timeTravelChosenConditionals.FirstOrDefault(currentItemObject.Conditionals.Contains);
                if (conditional == null)
                {
                    conditional = currentItemObject.Conditionals.RandomOrDefault(Random);
                    _timeTravelChosenConditionals.Add(conditional);
                }
                if (conditional != null)
                {
                    foreach (var item in conditional.AllowModification().Where(item => item.IsSameType(currentItem)))
                    {
                        UpdateTimeNeeded(currentItem, item);

                        PlaceItem(item, targets);
                    }
                }

                _timeTravelPath.Pop();

                return;
            }

            if (!placeJunk && ItemUtils.IsJunk(currentItemObject.Item))
            {
                // junk items are only placed within PlaceRemainingItems
                return;
            }

            if (currentItemObject.NewLocation.HasValue)
            {
                // already placed
                return;
            }

            var availableItems = targets.ToList();
            if (currentItem > Item.SongOath)
            {
                availableItems.Remove(Item.MaskDeku);
                availableItems.Remove(Item.SongHealing);
                availableItems.Remove(Item.StartingSword);
                availableItems.Remove(Item.StartingShield);
                availableItems.Remove(Item.StartingHeartContainer1);
                availableItems.Remove(Item.StartingHeartContainer2);
            }

            if (restriction != null)
            {
                availableItems.RemoveAll(location => !restriction(currentItem, location, ItemList));
            }

            if (!_settings.AddSongs)
            {
                availableItems.RemoveAll(location => location.IsSong() != currentItem.IsSong());
            }

            if (_settings.BossRemainsMode.HasFlag(BossRemainsMode.ShuffleOnly))
            {
                availableItems.RemoveAll(location => (location.ItemCategory() == ItemCategory.BossRemains) != (currentItem.ItemCategory() == ItemCategory.BossRemains));
            }

            currentItem = currentItemObject.Item;
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

                    break;
                }
                else
                {
                    Debug.WriteLine($"----Failed to place {currentItem.Name()} at {targetLocation.Location()}----");
                    availableItems.Remove(targetLocation);
                }
            }

            if (!_timeTravelPlaced)
            {
                PlaceRequirements(currentItem, targets);
            }
        }

        private void SetupItems()
        {
            SetupCustomItems();

            if (_settings.ProgressiveUpgrades)
            {
                _settings.CustomStartingItemList = _settings.CustomStartingItemList
                    .GroupBy(item => ItemUtils.ForbiddenStartTogether.FirstOrDefault(fst => fst.Contains(item)))
                    .SelectMany(g => g.Key == null || g.Key.Contains(Item.StartingShield) ? g.ToList() : g.Key.Take(g.Count()))
                    .ToList();
            }

            foreach (var item in _settings.CustomStartingItemList)
            {
                ItemList[item].ItemOverride = Item.RecoveryHeart;
            }

            if (_settings.SmallKeyMode.HasFlag(SmallKeyMode.DoorsOpen))
            {
                foreach (var item in ItemUtils.SmallKeys())
                {
                    ItemList[item].ItemOverride = Item.RecoveryHeart;
                }
            }

            if (_settings.BossKeyMode.HasFlag(BossKeyMode.DoorsOpen))
            {
                foreach (var item in ItemUtils.BossKeys())
                {
                    ItemList[item].ItemOverride = Item.RecoveryHeart;
                }
            }

            if (_settings.StrayFairyMode.HasFlag(StrayFairyMode.ChestsOnly))
            {
                foreach (var item in ItemUtils.DungeonStrayFairies())
                {
                    ItemList[item].ItemOverride = Item.RecoveryHeart;
                    if (!item.HasAttribute<ChestAttribute>())
                    {
                        ItemList[item].NewLocation = item;
                    }
                }
            }
        }

        private void RandomizePrices()
        {
            var royalWalletEnabled = _settings.PriceMode.HasFlag(PriceMode.AccountForRoyalWallet) && _settings.CustomItemList.Contains(Item.UpgradeRoyalWallet);
            var priceShouldMultiply = royalWalletEnabled && (_settings.PriceMode.HasFlag(PriceMode.ShuffleOnly) || _settings.PriceMode == PriceMode.AccountForRoyalWallet);

            var costPool = MessageCost.MessageCosts
                .Where(mc => _settings.PriceMode.HasFlag(mc.Category))
                .Select(mc => mc.Cost)
                .ToList();

            ushort randomPrice()
            {
                if (_settings.PriceMode.HasFlag(PriceMode.ShuffleOnly))
                {
                    return costPool.Random(Random);
                }

                if (royalWalletEnabled)
                {
                    return (ushort)Math.Clamp(1 + Random.BetaVariate(1.5, 8.5) * 999, 1, 999);
                }

                return (ushort)Math.Clamp(1 + Random.BetaVariate(1.5, 4.0) * 500, 1, 500);
            }

            (ushort, ushort) randomPriceWithComparablePrice()
            {
                var cost = randomPrice();
                var comparableCost = cost;
                if (priceShouldMultiply)
                {
                    comparableCost <<= 1;
                    if (comparableCost > 999)
                    {
                        comparableCost = 999;
                    }
                }
                return (cost, comparableCost);
            }

            _randomized.MessageCosts = new List<ushort?>();
            
            for (var i = 0; i < MessageCost.MessageCosts.Length; i++)
            {
                var messageCost = MessageCost.MessageCosts[i];
                if (!_settings.PriceMode.HasFlag(messageCost.Category) && !royalWalletEnabled)
                {
                    _randomized.MessageCosts.Add(null);
                    continue;
                }

                ushort cost;
                ushort comparableCost;

                if (_settings.PriceMode.HasFlag(messageCost.Category))
                {
                    (cost, comparableCost) = randomPriceWithComparablePrice();

                    // this relies on puchase 2 appearing in the list directly after purchase 1
                    if (messageCost.Name == "Business Scrub Purchase 2")
                    {
                        var purchase1Cost = _randomized.MessageCosts[i - 1] ?? 150;

                        while (comparableCost == purchase1Cost)
                        {
                            (cost, comparableCost) = randomPriceWithComparablePrice();
                        }
                    }

                    costPool.Remove(cost);
                }
                else
                {
                    cost = messageCost.Cost;
                }

                if (priceShouldMultiply)
                {
                    cost <<= 1;
                    if (cost > 999)
                    {
                        cost = 999;
                    }
                }

                _randomized.MessageCosts.Add(cost);
            }

            if (_settings.LogicMode != LogicMode.NoLogic)
            {
                var wallets200 = ItemList
                    .FirstOrDefault(io =>
                        io.Item.IsFake()
                        && io.DependsOnItems.Count == 0
                        && io.Conditionals.Count == 3
                        && io.Conditionals.Any(c => c.SequenceEqual(new List<Item> { Item.UpgradeAdultWallet }))
                        && io.Conditionals.Any(c => c.SequenceEqual(new List<Item> { Item.UpgradeGiantWallet }))
                        && io.Conditionals.Any(c => c.SequenceEqual(new List<Item> { Item.UpgradeRoyalWallet })));

                if (wallets200 == null)
                {
                    wallets200 = new ItemObject
                    {
                        ID = ItemList.Count,
                        TimeAvailable = 63,
                        Conditionals = new List<List<Item>>
                        {
                            new List<Item> { Item.UpgradeAdultWallet },
                            new List<Item> { Item.UpgradeGiantWallet },
                            new List<Item> { Item.UpgradeRoyalWallet },
                        },
                    };
                    ItemList.Add(wallets200);
                }

                var wallets500 = ItemList
                    .FirstOrDefault(io =>
                        io.Item.IsFake()
                        && io.DependsOnItems.Count == 0
                        && io.Conditionals.Count == 2
                        && io.Conditionals.Any(c => c.SequenceEqual(new List<Item> { Item.UpgradeGiantWallet }))
                        && io.Conditionals.Any(c => c.SequenceEqual(new List<Item> { Item.UpgradeRoyalWallet })));

                if (wallets500 == null)
                {
                    wallets500 = new ItemObject
                    {
                        ID = ItemList.Count,
                        TimeAvailable = 63,
                        Conditionals = new List<List<Item>>
                        {
                            new List<Item> { Item.UpgradeGiantWallet },
                            new List<Item> { Item.UpgradeRoyalWallet },
                        },
                    };
                    ItemList.Add(wallets500);
                }

                var affectedLocations = new Dictionary<Item, ushort>();
                for (var i = 0; i < MessageCost.MessageCosts.Length; i++)
                {
                    var messageCost = MessageCost.MessageCosts[i];
                    var cost = _randomized.MessageCosts[i];
                    if (!cost.HasValue)
                    {
                        continue;
                    }

                    foreach (var location in messageCost.LocationsAffected)
                    {
                        var affectedCost = affectedLocations.GetValueOrDefault(location, ushort.MaxValue); 
                        if (cost < affectedCost)
                        {
                            affectedLocations[location] = cost.Value;
                            ItemList[location].DependsOnItems.Remove(wallets200.Item);
                            ItemList[location].DependsOnItems.Remove(wallets500.Item);
                            ItemList[location].DependsOnItems.Remove(Item.UpgradeRoyalWallet);
                            if (cost > 500)
                            {
                                ItemList[location].DependsOnItems.Add(Item.UpgradeRoyalWallet);
                            }
                            else if (cost > 200)
                            {
                                ItemList[location].DependsOnItems.Add(wallets500.Item);
                            }
                            else if (cost > 99)
                            {
                                ItemList[location].DependsOnItems.Add(wallets200.Item);
                            }
                        }
                    }
                }

                for (var i = 0; i < MessageCost.MessageCosts.Length; i++)
                {
                    var messageCost = MessageCost.MessageCosts[i];
                    var cost = _randomized.MessageCosts[i];
                    if (!cost.HasValue)
                    {
                        continue;
                    }

                    Item walletRequired;
                    if (cost > 200)
                    {
                        walletRequired = Item.UpgradeGiantWallet;
                    }
                    else if (cost > 99)
                    {
                        walletRequired = wallets200.Item;
                    }
                    else
                    {
                        continue;
                    }

                    foreach (var item in messageCost.ItemsAffected)
                    {
                        foreach (var io in ItemList)
                        {
                            if (io.DependsOnItems.Contains(item))
                            {
                                io.DependsOnItems.Add(walletRequired);
                            }

                            foreach (var conditionalItems in io.Conditionals)
                            {
                                if (conditionalItems.Contains(item))
                                {
                                    conditionalItems.Add(walletRequired);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void ReplaceRecoveryHeartsWithJunk()
        {
            var allUsableJunk = ItemUtils.JunkItems.ToList();
            var usableJunk = allUsableJunk.Where(item => ItemList[item].IsRandomized).ToList();
            if (!usableJunk.Any())
            {
                usableJunk = allUsableJunk;
            }
            foreach (var io in ItemList.Where(io => !io.Item.IsFake()))
            {
                if (!ItemUtils.IsStartingLocation(io.NewLocation.Value) && (!io.NewLocation.Value.IsSong() || _settings.AddSongs) && io.Item == Item.RecoveryHeart)
                {
                    io.ItemOverride = usableJunk.Random(Random);
                }
            }
        }

        private void RemoveFreeRequirements()
        {
            var freeItems = _settings.CustomStartingItemList
                .Union(_randomized.BlitzExtraItems)
                .Union(ItemList.Where(io => io.NewLocation.HasValue && ItemUtils.IsStartingLocation(io.NewLocation.Value)).Select(io => io.Item))
                .ToList();

            bool updated;
            do
            {
                updated = false;
                foreach (var itemObject in ItemList.Where(io => io.Item.IsFake() && !freeItems.Contains(io.Item)))
                {
                    if ((itemObject.DependsOnItems?.All(id => freeItems.Contains(id)) != false)
                        && (itemObject.Conditionals?.Any(c => c.All(id => freeItems.Contains(id))) != false)
                        && (itemObject.DependsOnItems != null || itemObject.Conditionals != null))
                    {
                        freeItems.Add(itemObject.Item);
                        updated = true;
                    }
                }
            } while (updated);

            foreach (var itemObject in ItemList)
            {
                itemObject.DependsOnItems.RemoveAll(freeItems.Contains);

                if (itemObject.Conditionals.Any(c => c.All(freeItems.Contains)))
                {
                    itemObject.Conditionals.Clear();
                }
            }
        }

        private bool _timeTravelPlaced = true;
        private Stack<Item> _timeTravelPath = new Stack<Item>();
        private List<List<Item>> _timeTravelChosenConditionals = new List<List<Item>>();
        private void RandomizeItems()
        {
            var itemPool = new List<Item>();

            AddAllItems(itemPool);

            PlaceRestrictedItems(itemPool);

            PlaceFreeItems(itemPool);

            RemoveFreeRequirements();

            _timeTravelPlaced = false;
            _timeTravelPath.Clear();
            _timeTravelChosenConditionals.Clear();

            PlaceItem(Item.SongTime, itemPool);
            PlaceItem(Item.OtherTimeTravel, itemPool);

            _timeTravelPlaced = true;
            _timeTravelPath.Clear();
            _timeTravelChosenConditionals.Clear();

            PlaceOcarinaAndSongOfTime(itemPool);
            PlaceBossRemains(itemPool);

            if (_settings.ItemPlacement == ItemPlacement.Random)
            {
                PlaceRandomItems(itemPool);
            }
            else if (_settings.ItemPlacement == ItemPlacement.Bespoke)
            {
                PlaceBespokeItems(itemPool);
            }

            PlaceQuestItems(itemPool);
            PlaceTradeItems(itemPool);
            PlaceFrogs(itemPool);
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
            PlaceRemainingItems(itemPool);

            _randomized.ItemList = ItemList;
        }

        /// <summary>
        /// Items are placed in the following order:
        /// 1. Songs if they are not placed with items. Epona's first, the the rest in a random order in order to minimize failures.
        /// 2. All items that can effect progression in a random order.
        /// 3. Everything else is placed by the old algorithm. The order shouldn't matter because they aren't tied to logic.
        /// </summary>
        private void PlaceRandomItems(List<Item> itemPool)
        {

            var itemList = new List<Item>();

            for (var i = Item.TradeItemRoomKey; i <= Item.TradeItemMamaLetter; i++)
            {
                itemList.Add(i);
            }

            for (var i = Item.TradeItemMoonTear; i <= Item.TradeItemOceanDeed; i++)
            {
                itemList.Add(i);
            }

            for (var i = Item.ItemWoodfallMap; i <= Item.ItemStoneTowerKey4; i++)
            {
                itemList.Add(i);
            }

            for (var i = Item.StartingSword; i <= Item.StartingHeartContainer2; i++)
            {
                itemList.Add(i);
            }

            for (var i = Item.UpgradeRazorSword; i <= Item.UpgradeRoyalWallet; i++)
            {
                itemList.Add(i);
            }

            for (var i = Item.SongHealing; i <= Item.SongOath; i++)
            {
                itemList.Add(i);
            }

            itemList.Add(Item.SongLullabyIntro);

            for (var i = Item.MaskPostmanHat; i <= Item.MaskZora; i++)
            {
                itemList.Add(i);
            }

            for (var i = Item.MaskDeku; i <= Item.ItemNotebook; i++)
            {
                itemList.Add(i);
            }

            for (var i = Item.CollectibleSwampSpiderToken1; i <= Item.CollectibleOceanSpiderToken30; i++)
            {
                itemList.Add(i);
            }

            for (var i = Item.CollectibleStrayFairyClockTown; i <= Item.CollectibleStrayFairyStoneTower15; i++)
            {
                itemList.Add(i);
            }

            // Could maybe just add the items that affect logic but unless
            // there's a bunch of seed rolling failures it's probably not a big deal.
            // Same goes for trading shop items.
            for (var i = Item.BottleCatchFairy; i <= Item.BottleCatchMushroom; i++)
            {
                itemList.Add(i);
            }

            for (var i = Item.ItemRanchBarnMainCowMilk; i <= Item.ItemCoastGrottoCowMilk2; i++)
            {
                itemList.Add(i);
            }

            for (var i = Item.ShopItemTradingPostRedPotion; i <= Item.ShopItemZoraRedPotion; i++)
            {
                itemList.Add(i);
            }

            itemList.RemoveAll(item => _settings.CustomStartingItemList.Contains(item));
            itemList.RemoveAll(item => _randomized.BlitzExtraItems.Contains(item));

            if (!_settings.AddSongs)
            {
                var songs = new List<Item>();
                foreach (var item in itemList)
                {
                    if (item.IsSong())
                    {
                        songs.Add(item);
                    }
                }

                foreach (var song in songs)
                {
                    itemList.Remove(song);
                }

                if (songs.Contains(Item.SongEpona))
                {
                    PlaceItem(Item.SongEpona, itemPool);
                    songs.Remove(Item.SongEpona);
                }

                while (songs.Count != 0)
                {
                    var song = songs.Random(Random);
                    PlaceItem(song, itemPool);
                    songs.Remove(song);
                }
            }

            while (itemList.Count != 0)
            {
                var item = itemList.Random(Random);
                PlaceItem(item, itemPool);
                itemList.Remove(item);
            }
        }


        private void PlaceBespokeItems(List<Item> itemPool)
        {
            var densityRating = (int)Math.Max(500 - Math.Floor((itemPool.Count - _settings.CustomJunkLocations.Count) * Random.NextDouble(0.7, 1.3)), 0);

            var alwaysOrdered = new List<List<Item?>>
            {
                new List<Item?> { Item.UpgradeRoyalWallet, Item.UpgradeGiantWallet, Item.UpgradeAdultWallet },
            };

            var canPlaceSongs = !_settings.AddSongs;
            void PlaceBespokeItem(Item item)
            {
                item = alwaysOrdered
                    .FirstOrDefault(list => list.Contains(item))
                    ?.FirstOrDefault(listItem => !ItemList[listItem.Value].NewLocation.HasValue) ?? item;
                if (!item.IsSong() || canPlaceSongs)
                {
                    PlaceItem(item, itemPool);
                }
            }

            void PlaceBespokeItemGroup(params Item[] items)
            {
                foreach (var item in items.OrderBy(_ => Random.Next()))
                {
                    PlaceBespokeItem(item);
                }
            }

            if (_settings.GiantMaskAnywhere)
            {
                PlaceBespokeItem(Item.MaskGiant);
            }

            if (Random.NextDouble() < 0.3)
            {
                PlaceBespokeItem(Item.ItemNotebook);
            }

            PlaceBespokeItem(Item.UpgradeBiggestBombBag);
            PlaceBespokeItem(Item.UpgradeBigBombBag);
            PlaceBespokeItem(Item.UpgradeBiggestQuiver);
            PlaceBespokeItem(Item.UpgradeBigQuiver);

            var tradeGroup = new List<Item>
            {
                Item.TradeItemRoomKey,
                Item.TradeItemKafeiLetter,
                Item.TradeItemMamaLetter,
                Item.MaskKafei,
            };

            if (_settings.PriceMode == PriceMode.None && _settings.CustomItemList.Contains(Item.UpgradeRoyalWallet))
            {
                PlaceBespokeItem(Item.UpgradeRoyalWallet);
                tradeGroup.Add(Item.UpgradeGiantWallet);
                tradeGroup.Add(Item.UpgradeAdultWallet);
            }
            else
            {
                tradeGroup.Add(Item.UpgradeRoyalWallet);
                tradeGroup.Add(Item.UpgradeGiantWallet);
            }

            PlaceBespokeItemGroup(tradeGroup.Random((int)Math.Round(densityRating / 70.0), Random));

            var tier1 = new Item[]
            {
                Item.MaskGreatFairy,
                Item.MaskScents,
                Item.TradeItemKafeiLetter,
                Item.TradeItemMamaLetter,
                Item.MaskKafei,
            };

            var tier2 = new Item[]
            {
                Item.MaskPostmanHat,
                Item.MaskAllNight,
                Item.MaskKeaton,
                Item.MaskBremen,
                Item.MaskTruth,
                Item.MaskBunnyHood,
                Item.MaskRomani,
                Item.MaskCircusLeader,
                Item.MaskCouple,
                Item.MaskKamaro,
            };

            var tier3 = new Item[]
            {
                Item.TradeItemMoonTear,
                Item.TradeItemLandDeed,
                Item.TradeItemSwampDeed,
                Item.TradeItemMountainDeed,
                Item.TradeItemOceanDeed,
            };

            var tier4 = new Item[]
            {
                Item.MaskStone,
                Item.MaskDonGero,
                Item.TradeItemRoomKey,
                Item.TradeItemPendant,
                Item.SongLullabyIntro,
            };

            var blastBomb = new Item[]
            {
                Item.ItemBombBag,
                Item.MaskBlast,
            };

            var captainKeg = new Item[]
            {
                Item.ItemPowderKeg,
                Item.MaskCaptainHat,
            };

            var newWaveElegy = new Item[]
            {
                Item.SongNewWaveBossaNova,
                Item.SongElegy,
            };

            PlaceBespokeItemGroup(tier2.Random((int)Math.Round(densityRating / 50.0), Random));
            PlaceBespokeItemGroup(tier3.Random(1, Random));

            var roll = Random.NextDouble();
            if (roll < 0.6)
            {
                PlaceBespokeItemGroup(Item.ItemHookshot, Item.ItemBow, Item.MaskZora, Item.SongSonata, Item.SongLullaby);
                PlaceBespokeItem(Item.MaskFierceDeity);
                PlaceBespokeItem(Item.ItemLightArrow);
                PlaceBespokeItem(Item.ItemIceArrow);
                PlaceBespokeItem(Item.SongEpona);
                PlaceBespokeItem(Item.SongHealing);
                PlaceBespokeItem(Item.SongLullabyIntro);
                PlaceBespokeItem(Item.SongSoaring);
                PlaceBespokeItemGroup(newWaveElegy);
                PlaceBespokeItem(Item.SongStorms);
                PlaceBespokeItem(Item.SongOath);
                PlaceBespokeItemGroup(Item.ItemFireArrow, Item.MaskDeku, Item.MaskGoron);
                PlaceBespokeItemGroup(blastBomb);
            }
            else if (roll < 0.85)
            {
                PlaceBespokeItem(Item.MaskGoron);
                PlaceBespokeItem(Item.ItemHookshot);
                PlaceBespokeItem(Item.SongSonata);
                if (ItemList[Item.MaskGoron].NewLocation != Item.MaskGibdo)
                {
                    PlaceBespokeItem(Item.SongLullaby);
                }
                PlaceBespokeItem(Item.SongEpona);
                PlaceBespokeItem(Item.SongHealing);
                PlaceBespokeItem(Item.SongSoaring);
                PlaceBespokeItemGroup(newWaveElegy);
                PlaceBespokeItem(Item.SongStorms);
                PlaceBespokeItem(Item.SongLullaby);
                PlaceBespokeItem(Item.SongLullabyIntro);
                PlaceBespokeItem(Item.SongOath);
                PlaceBespokeItem(Item.MaskFierceDeity);
                PlaceBespokeItem(Item.MaskZora);
                PlaceBespokeItem(Item.ItemBow);
                PlaceBespokeItem(Item.ItemFireArrow);
                PlaceBespokeItem(Item.ItemLightArrow);
                PlaceBespokeItem(Item.ItemIceArrow);
                PlaceBespokeItem(Item.MaskDeku);
                PlaceBespokeItemGroup(blastBomb);
            }
            else
            {
                PlaceBespokeItem(Item.MaskDeku);
                PlaceBespokeItem(Item.SongLullaby);
                PlaceBespokeItem(Item.ItemHookshot);
                PlaceBespokeItem(Item.SongSonata);
                PlaceBespokeItem(Item.SongEpona);
                PlaceBespokeItem(Item.SongHealing);
                PlaceBespokeItem(Item.SongLullabyIntro);
                PlaceBespokeItem(Item.SongSoaring);
                PlaceBespokeItemGroup(newWaveElegy);
                PlaceBespokeItem(Item.SongStorms);
                PlaceBespokeItem(Item.SongOath);
                PlaceBespokeItem(Item.MaskFierceDeity);
                PlaceBespokeItem(Item.MaskZora);
                PlaceBespokeItem(Item.MaskGoron);
                PlaceBespokeItem(Item.ItemBow);
                PlaceBespokeItem(Item.ItemFireArrow);
                PlaceBespokeItem(Item.ItemLightArrow);
                PlaceBespokeItem(Item.ItemIceArrow);
                PlaceBespokeItemGroup(blastBomb);
            }

            PlaceBespokeItem(Item.ItemNotebook);
            PlaceBespokeItem(Item.ItemPictobox);
            PlaceBespokeItem(Item.ItemLens);
            PlaceBespokeItemGroup(captainKeg);
            PlaceBespokeItem(Item.UpgradeMirrorShield);
            PlaceBespokeItem(Item.FairyDoubleMagic);
            PlaceBespokeItem(Item.FairyMagic);
            PlaceBespokeItem(Item.MaskFierceDeity);
            PlaceBespokeItem(Item.MaskGiant);
            PlaceBespokeItem(Item.MaskGibdo);
            PlaceBespokeItem(Item.MaskGaro);

            PlaceBespokeItem(Item.UpgradeRoyalWallet);
            PlaceBespokeItem(Item.UpgradeGiantWallet);
            PlaceBespokeItem(Item.UpgradeAdultWallet);

            canPlaceSongs = true;

            if (_settings.AddSongs)
            {
                PlaceBespokeItem(Item.SongEpona);
                PlaceBespokeItemGroup(new Item[]
                {
                    Item.SongSonata,
                    Item.SongLullaby,
                    Item.SongNewWaveBossaNova,
                    Item.SongElegy,
                });
                PlaceBespokeItem(Item.SongHealing);
                PlaceBespokeItem(Item.SongOath);
                PlaceBespokeItem(Item.SongSoaring);
            }

            PlaceBespokeItemGroup(tier1);
            PlaceBespokeItemGroup(tier2);
            PlaceBespokeItemGroup(tier3);
            PlaceBespokeItemGroup(tier4);

            for (var i = Item.MaskPostmanHat; i <= Item.MaskKamaro; i++)
            {
                PlaceBespokeItem(i);
            }
            for (var i = Item.TradeItemRoomKey; i <= Item.TradeItemMamaLetter; i++)
            {
                PlaceBespokeItem(i);
            }
            for (var i = Item.TradeItemMoonTear; i <= Item.TradeItemOceanDeed; i++)
            {
                PlaceBespokeItem(i);
            }

            PlaceBespokeItemGroup(new Item[]
            {
                Item.ItemFairySword,
                Item.FairySpinAttack,
                Item.UpgradeGildedSword,
            });

            PlaceBespokeItem(Item.FairyDoubleDefense);
            PlaceBespokeItem(Item.UpgradeRazorSword);
            PlaceBespokeItem(Item.StartingSword);

            PlaceBespokeItem(Item.ItemBottleAliens);
            PlaceBespokeItem(Item.ItemBottleWitch);
            PlaceBespokeItem(Item.ItemBottleGoronRace);
            PlaceBespokeItem(Item.ItemBottleMadameAroma);
            PlaceBespokeItem(Item.ItemBottleBeavers);
            PlaceBespokeItem(Item.ItemBottleDampe);

            if (_settings.AddSongs)
            {
                PlaceBespokeItem(Item.SongStorms);
            }

            PlaceBespokeItem(Item.ItemMagicBean);
            PlaceBespokeItem(Item.ChestInvertedStoneTowerBean);
            PlaceBespokeItem(Item.ShopItemBusinessScrubMagicBean);
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
            // remember, this is a check pool, not an item pool (I didn't name it, not changing to avoid conflicts with upstream)
            if (itemPool == null)
            {
                itemPool = new List<Item>();
                AddAllItems(itemPool);
            }

            List<PlandoItemCombo> plandoItemCombos = PlandoUtils.ReadAllItemPlandoFiles(itemPool);
            if (plandoItemCombos == null) return; // no plandos found

            foreach (PlandoItemCombo pic in plandoItemCombos)
            {
                var oldPic = PlandoItemCombo.Copy(pic); // debugging
                var itemCombo = PlandoUtils.CleanItemCombo(pic, Random, itemPool, ItemList);
                if (itemCombo == null) // not possible to fullfill
                {
                    if (pic.SkipIfError) continue;

                    // let's backtrack and find the items that are already assigned
                    //   and the checks that are already taken and print them
                    var allItems = ItemList.FindAll(u => pic.ItemList.Contains(u.Item));
                    var previouslyPlacedItems = allItems.FindAll(u => u.IsRandomized);
                    string picDebug = "Items that were already assigned:\n";
                    foreach (var item in previouslyPlacedItems)
                    {
                        picDebug += "- [" + item.Item.Name() + "] was placed in check: [" + item.NewLocation.Value.Location() + "]\n";
                    }

                    var previouslyPlacedChecks = ItemList.FindAll(u => u.IsRandomized && pic.CheckList.Contains(u.NewLocation.Value));
                    picDebug += "\nChecks that were already assigned:\n";
                    foreach (var item in previouslyPlacedChecks)
                    {
                        picDebug += "- [" + item.NewLocation.Value.Location() + "] was filled with item: [" + item.Item.Name() + "]\n";
                    }

                    var originalChecks = oldPic.CheckList;
                    picDebug += "\nChecks this ItemCombo was supposed to use:\n";
                    foreach (var item in originalChecks)
                    {
                        picDebug += "- [" + item.ToString() + "]\n";
                    }

                    itemPool = new List<Item>();
                    AddAllItems(itemPool);
                    var notRandomized = oldPic.ItemList.Except(itemPool);
                    if (notRandomized.Count() > 0)
                    {
                        picDebug += "\nthis PIC has items that were NOT RANDOMIZED\n";
                    }

                    notRandomized = oldPic.CheckList.Except(itemPool);
                    if (notRandomized.Count() > 0)
                    {
                        picDebug += "\nthis PIC has checks that were NOT RANDOMIZED\n";
                    }

                    throw new Exception("Error: Plando failed to build with this seed\n combo name: [" + pic.Name + "]\n\n" + picDebug);
                }

                int drawCount = 0;
                for (int itemCount = 0; drawCount < itemCombo.ItemDrawCount && itemCount < itemCombo.ItemList.Count; itemCount++)
                {
                    /// for all items, attempt to add; count successes
                    Item item = itemCombo.ItemList[itemCount];
                    foreach (Item check in itemCombo.CheckList)
                    {
                        if (itemCombo.SkipLogic == false && ItemUtils.IsStartingLocation(check) && ForbiddenStartingItems.Contains(item))
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
                    var junkChecks = string.Join(", ", itemCombo.CheckList.Where(u => _settings.CustomJunkLocations.Contains(u)));
                    var remainingItems = string.Join(", ", itemCombo.ItemList.Where(u => ItemList[u].IsRandomized == false));

                    throw new Exception($"Error: Plando could not find enough checks to match this plandos items with this seed:\n [{itemCombo.Name}]\n"
                        + $"Remaining Unplaced Items:\n [{remainingItems}]\n"
                        + $"Remaining Unfullfilled Checks:\n [{string.Join(", ", itemCombo.CheckList)}]\n"
                        + $"Checks in this plando item combo marked as junk:\n [{junkChecks}]");
                }
            }
        }

        /// <summary>
        /// Places remaining items in the randomization pool.
        /// </summary>
        private void PlaceRemainingItems(List<Item> itemPool)
        {
            foreach (var item in ItemUtils.AllLocations().OrderByDescending(item => !ItemUtils.IsJunk(ItemList[item].Item)).ThenByDescending(item => item.IsTemporary()))
            {
                if (ItemList[item].NewLocation == null)
                {
                    PlaceItem(item, itemPool, placeJunk: true);
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

        private void PlaceRestrictedItems(List<Item> itemPool)
        {
            Func<Item, Item, ItemList, bool> GetRestriction<TMode>(TMode mode) where TMode : struct, Enum
            {
                var restrictions = Enum.GetValues<TMode>()
                    .Where(m => mode.HasFlag(m) && m.HasAttribute<RestrictedPlacementAttribute>())
                    .Select(m => m.GetAttribute<RestrictedPlacementAttribute>().RestrictPlacement);

                if (restrictions.Any())
                {
                    return restrictions
                        .Aggregate((a, b) => (item, location, itemList) => a(item, location, itemList) && b(item, location, itemList));
                }

                return null;
            }

            void PlaceRestricted<TMode>(IEnumerable<Item> items, TMode mode, bool placeJunk = false) where TMode : struct, Enum
            {
                var restrictions = GetRestriction(mode);
                if (restrictions != null)
                {
                    foreach (var item in items)
                    {
                        PlaceItem(item, itemPool, restrictions, placeJunk);
                    }
                }
            }

            if (_settings.BossRemainsMode.HasFlag(BossRemainsMode.GreatFairyRewards))
            {
                PlaceRestricted(ItemUtils.BossRemains(), _settings.BossRemainsMode, true);
            }

            if (_settings.BossKeyMode.HasFlag(BossKeyMode.GreatFairyRewards))
            {
                PlaceRestricted(ItemUtils.BossKeys(), _settings.BossKeyMode);
            }

            PlaceRestricted(ItemUtils.DungeonStrayFairies(), _settings.StrayFairyMode);
            PlaceRestricted(ItemUtils.BossKeys(), _settings.BossKeyMode);
            PlaceRestricted(ItemUtils.SmallKeys(), _settings.SmallKeyMode);
            PlaceRestricted(ItemUtils.BossRemains(), _settings.BossRemainsMode);
            PlaceRestricted(ItemUtils.DungeonNavigation(), _settings.DungeonNavigationMode);
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
            var songs = Enumerable.Range((int)Item.SongHealing, Item.SongOath - Item.SongHealing + 1)
                .Cast<Item>()
                .Append(Item.SongLullabyIntro);

            foreach (var song in songs)
            {
                PlaceItem(song, itemPool);
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
            for (var i = Item.UpgradeRazorSword; i <= Item.UpgradeRoyalWallet; i++)
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
            if (!_settings.AddSongs)
            {
                freeItemLocations.Remove(Item.SongHealing);
            }
            var availableStartingItems = (_settings.StartingItemMode switch {
                    StartingItemMode.Random => ItemUtils.StartingItems().Where(item => !item.IsTemporary() && item != Item.ItemPowderKeg),
                    StartingItemMode.AllowTemporaryItems => ItemUtils.StartingItems(),
                    _ => Enumerable.Empty<Item>(),
                })
                .Where(item => !ItemList[item].NewLocation.HasValue && !ForbiddenStartingItems.Contains(item) && !_settings.CustomStartingItemList.Contains(item) && !_randomized.BlitzExtraItems.Contains(item))
                .Cast<Item?>()
                .ToList();
            var itemHearts = _settings.CustomStartingItemList
                .Union(_randomized.BlitzExtraItems)
                .Where(item => !ItemList[item].NewLocation.HasValue && (_settings.AddSongs || !item.IsSong()))
                .Cast<Item?>()
                .ToList();
            var itemJunk = ItemUtils.AllRupees()
                .Where(item => !ItemList[item].NewLocation.HasValue)
                .Cast<Item?>()
                .ToList();
            var availableSongs = ItemUtils.StartingItems()
                .Where(item => item.IsSong())
                .Where(item => !ItemList[item].NewLocation.HasValue && !ForbiddenStartingItems.Contains(item) && !_settings.CustomStartingItemList.Contains(item))
                .Cast<Item?>()
                .ToList();
            var songHearts = _settings.CustomStartingItemList
                .Where(item => !ItemList[item].NewLocation.HasValue && !_settings.AddSongs && item.IsSong())
                .Cast<Item?>()
                .ToList();
            foreach (var location in freeItemLocations)
            {
                var placedItem = ItemList.FirstOrDefault(item => item.NewLocation == location)?.Item;
                if (placedItem == null)
                {
                    List<Item?> availableItems = null;
                    if (location.IsSong() && !_settings.AddSongs)
                    {
                        if (!ItemUtils.IsLocationJunk(location, _settings))
                        {
                            availableItems = availableSongs;
                        }
                        else
                        {
                            availableItems = songHearts;
                        }
                    }
                    else
                    {
                        if (!ItemUtils.IsLocationJunk(location, _settings))
                        {
                            availableItems = availableStartingItems;
                        }
                        if (availableItems == null || availableItems.Count == 0)
                        {
                            availableItems = itemHearts;
                        }
                        if (availableItems == null || availableItems.Count == 0)
                        {
                            availableItems = itemJunk;
                        }
                    }
                    placedItem = availableItems?.RandomOrDefault(Random);
                    if (placedItem == null)
                    {
                        throw new Exception("Failed to replace a starting item. Not enough items that can be started with are randomized or too many Extra Starting Items are selected.");
                    }
                    ItemList[placedItem.Value].NewLocation = location;
                    ItemList[placedItem.Value].IsRandomized = true;
                    itemPool.Remove(location);
                    availableItems.Remove(placedItem.Value);
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

        private void PlaceOcarinaAndSongOfTime(List<Item> itemPool)
        {
            PlaceItem(Item.SongTime, itemPool);
            PlaceItem(Item.ItemOcarina, itemPool);
        }

        private void PlaceBossRemains(List<Item> itemPool)
        {
            for (var i = Item.RemainsOdolwa; i <= Item.RemainsTwinmold; i++)
            {
                PlaceItem(i, itemPool);
            }
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

        private void PlaceFrogs(List<Item> itemPool)
        {
            for (var i = Item.FrogWoodfallTemple; i <= Item.FrogLaundryPool; i++)
            {
                PlaceItem(i, itemPool);
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
            if (_settings.CustomItemList == null)
            {
                throw new Exception("Invalid custom item string.");
            }
            foreach (var selectedItem in _settings.CustomItemList)
            {
                ItemList[selectedItem].NewLocation = null;
            }
        }

        /// <summary>
        /// Overwrite junk items with traps.
        /// </summary>
        /// <param name="trapAmount">Traps amount setting</param>
        /// <param name="appearance">Traps appearance setting</param>
        private void AddTraps(TrapAmount trapAmount, Dictionary<TrapType, int> trapWeights, TrapAppearance appearance)
        {
            var random = this.Random;

            // Select replaceable junk items of specified amount.
            var items = TrapUtils.SelectJunkItems(ItemList, trapAmount, random);

            // Dynamically generate appearance set for traps.
            // Only mimic items if they are included in the main randomization pool (not in their own pool).
            var mimics = TrapUtils.BuildTrapMimicSet(ItemList, appearance, (item) => item.IsPlacementHighlyRestricted(_settings))
                .ToList();

            if (trapWeights.GetValueOrDefault(TrapType.Nothing) > 0)
            {
                mimics.Add(new MimicItem(Item.Nothing));
            }

            var trapTypeItems = new Dictionary<TrapType, Item>
            {
                { TrapType.Ice, Item.IceTrap },
                { TrapType.Bomb, Item.BombTrap },
                { TrapType.Rupoor, Item.Rupoor },
                { TrapType.Nothing, Item.Nothing },
            };

            var list = new List<ItemObject>();
            foreach (var item in items)
            {
                var newLocation = item.NewLocation.Value;

                var allowedTrapTypes = trapTypeItems.Keys.ToList();
                if (newLocation.IsBlockingBombTrapPlacement())
                {
                    allowedTrapTypes.Remove(TrapType.Bomb);
                }

                if (allowedTrapTypes.Count == 0)
                {
                    continue;
                }

                var trapType = allowedTrapTypes.Random(random, t => trapWeights.GetValueOrDefault(t));
                var trapItem = trapTypeItems[trapType];

                // If check is visible (can be seen via world model), add "graphic override" for imitating other item.
                var mimic = trapType switch
                {
                    TrapType.Nothing => new MimicItem(Item.Nothing),
                    _ => mimics.Random(random)
                };

                item.ItemOverride = trapItem;
                item.Mimic = mimic;

                if (trapItem != Item.Nothing && (newLocation.IsModelVisible(_settings) || newLocation.IsTextVisible(_settings)))
                {
                    // Store name override for logging in HTML tracker.
                    if (trapItem != Item.Rupoor || newLocation.IsShopModelVisible())
                    {
                        item.NameOverride = $"{trapItem.Name()} ({mimic.Item.Name()})";
                    }

                    // If trap quirks enabled and placed as a shop item, use a fake shop item name.
                    if (_settings.TrapQuirks && newLocation.IsTextVisible(_settings))
                    {
                        item.Mimic.FakeName = FakeNameUtils.CreateFakeName(item.Mimic.Item.Name(), random);
                    }
                }

                if (_settings.UpdateChests)
                {
                    // Choose chest type for trap appearance.
                    item.Mimic.ChestType = TrapUtils.GetTrapChestTypeOverride(appearance, random);
                }

                list.Add(item);
            }

            _randomized.Traps = list.AsReadOnly();
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

                if (_settings.RandomizeBossRooms)
                {
                    progressReporter.ReportProgress(10, "Shuffling bosses...");
                    BossShuffle();
                }

                if (_settings.RandomizeDungeonEntrances)
                {
                    progressReporter.ReportProgress(20, "Shuffling entrances...");
                    EntranceShuffle();
                }

                PrepareAdditionalItemData();

                _randomized.Logic = ItemList.Select(io => new ItemLogic(io)).ToList();

                progressReporter.ReportProgress(30, "Shuffling items...");
                SetupItems();

                _randomized.BlitzExtraItems.AddRange(ItemUtils.PrepareBlitz(_settings, ItemList, Random));

                foreach (var item in _randomized.BlitzExtraItems)
                {
                    ItemList[item].ItemOverride = Item.RecoveryHeart;
                }

                ItemUtils.PrepareHintedJunkLocations(_settings, Random);

                // TODO check junk location count against junk item count


                RandomizeItems();
                if (_settings.BossRemainsMode.HasFlag(BossRemainsMode.GreatFairyRewards))
                {
                    foreach (var location in ItemUtils.GreatFairyRewards())
                    {
                        var itemObject = ItemList.Single(io => io.NewLocation == location);
                        if (itemObject.Item == Item.RecoveryHeart)
                        {
                            foreach (var requiredFairy in ItemList[location].DependsOnItems.Where(item => !_settings.CustomStartingItemList.Contains(item) && ItemUtils.DungeonStrayFairies().Contains(item)))
                            {
                                _randomized.BlitzExtraItems.Add(requiredFairy);
                                ItemList[requiredFairy].ItemOverride = Item.RecoveryHeart;
                            }
                        }
                    }
                }
                ReplaceRecoveryHeartsWithJunk(); // TODO make this an option?

                // Replace junk items with traps according to settings.
                AddTraps(_settings.TrapAmount, _settings.TrapWeights, _settings.TrapAppearance);
                
                var freeItemIds = _settings.CustomStartingItemList
                    .Union(_randomized.BlitzExtraItems)
                    .Cast<int>()
                    .Union(ItemList.Where(io => io.NewLocation.HasValue && ItemUtils.IsStartingLocation(io.NewLocation.Value)).Select(io => io.ID))
                    .ToList();

                bool updated;
                do
                {
                    updated = false;
                    foreach (var itemLogic in _randomized.Logic.Where(il => ((Item)il.ItemId).IsFake() && !freeItemIds.Contains(il.ItemId)))
                    {
                        if ((itemLogic.RequiredItemIds?.All(freeItemIds.Contains) != false)
                            && (itemLogic.ConditionalItemIds?.Any(c => c.All(freeItemIds.Contains)) != false)
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

                if (_settings.LogicMode != LogicMode.NoLogic)
                {
                    progressReporter.ReportProgress(32, "Calculating item importance...");

                    var logicForImportance = _randomized.Logic.Select(il => new ItemLogic(il)).ToList();
                    freeItemIds.Clear();
                    do
                    {
                        updated = false;
                        foreach (var itemLogic in logicForImportance.Where(il => !freeItemIds.Contains(il.ItemId)))
                        {
                            var item = (Item)itemLogic.ItemId;
                            var isFake = item.IsFake() && (!item.Region(ItemList).HasValue || item.Entrance() != null);
                            if (isFake
                                && (!ItemList[itemLogic.ItemId].IsTrick || _settings.EnabledTricks.Contains(ItemList[itemLogic.ItemId].Name))
                                && LogicUtils.IsSettingEnabled(_settings, ItemList[itemLogic.ItemId].SettingExpression)
                                && !itemLogic.RequiredItemIds.Any()
                                && !itemLogic.ConditionalItemIds.Any())
                            {
                                freeItemIds.Add(itemLogic.ItemId);
                                updated = true;
                                continue;
                            }

                            if ((itemLogic.RequiredItemIds?.All(freeItemIds.Contains) != false)
                                && (itemLogic.ConditionalItemIds?.Any(c => c.All(freeItemIds.Contains)) != false))
                            {
                                itemLogic.RequiredItemIds.Clear();
                                itemLogic.ConditionalItemIds.Clear();
                                if (isFake)
                                {
                                    freeItemIds.Add(itemLogic.ItemId);
                                    updated = true;
                                }
                            }
                        }
                    }
                    while (updated);

                    var logicForRequiredItems = _settings.LogicMode == LogicMode.Casual && _settings.GossipHintStyle == GossipHintStyle.Competitive && !_settings.GiantMaskAnywhere
                        ? logicForImportance.Select(il =>
                        {
                            var itemLogic = new ItemLogic(il);

                            // prevent Giant's Mask from being Way of the Hero.
                            itemLogic.RequiredItemIds.Remove((int)Item.MaskGiant);

                            return itemLogic;
                        }).ToList()
                        : logicForImportance;

                    var checkedLocations = new Dictionary<Item, LogicUtils.LogicPaths>();
                    var logicPaths = LogicUtils.GetImportantLocations(ItemList, _settings, Item.OtherCredits, logicForImportance, checkedLocations: checkedLocations);
                    var importantLocations = logicPaths?.Important.Where(item => item.Region(ItemList).HasValue && item.Entrance() == null).Distinct().ToHashSet();
                    var requiredSongLocations = logicPaths?.RequiredSongLocations.ToList();
                    if (importantLocations == null)
                    {
                        throw new RandomizationException("Moon Access is unobtainable.");
                    }
                    _randomized.CheckedImportanceLocations = checkedLocations;
                    var locationsRequiredForMoonAccess = new ConcurrentDictionary<Item, bool>(logicPaths.Required.ToDictionary(item => item, item => true));

                    // dont see a way to convert hashset to ConcurrentDictionary and then back, so mutex it is
                    Mutex importantLocationsMutex = new Mutex();
                    Mutex importantSongLocationsMutex = new Mutex();
                    var cts = new CancellationTokenSource();
                    var po = new ParallelOptions();
                    po.CancellationToken = cts.Token;
                    progressReporter.ReportProgress(32, "Verifying item importance...", cts);
                    try
                    {
                        Parallel.ForEach(importantLocations.ToList(), po, (location, state) =>
                        {
                            var item = ItemList.First(io => io.NewLocation == (location.MainLocation() ?? location)).Item;
                            if (!ItemUtils.CanBeRequired(item))
                            {
                                return;
                            }
                            var checkPaths = LogicUtils.GetImportantLocations(ItemList, _settings, Item.OtherCredits, logicForRequiredItems, cts: cts, exclude: location);
                            if (checkPaths != null)
                            {
                                locationsRequiredForMoonAccess.Remove(location, out bool _);
                                importantLocationsMutex.WaitOne();
                                importantLocations.UnionWith(checkPaths.Important.Distinct().Where(item => item.Region(ItemList).HasValue && item.Entrance() == null));
                                importantLocationsMutex.ReleaseMutex();

                                importantSongLocationsMutex.WaitOne();
                                requiredSongLocations.AddRange(checkPaths.RequiredSongLocations);
                                importantSongLocationsMutex.ReleaseMutex();
                            }
                        }
                        );
                    }
                    catch (OperationCanceledException)
                    {

                    }
                    finally
                    {
                        cts.Dispose();
                    }

                    // TODO one day maybe check if song of time is actually required
                    var songOfTime = ItemList[Item.SongTime];
                    var songOfTimeImportantItems = Enumerable.Empty<Item>();
                    if (songOfTime.Item == Item.SongTime)
                    {
                        progressReporter.ReportProgress(32, "Calculating song of time importance...");

                        var songOfTimeLocation = ItemList[Item.SongTime].NewLocation.Value;
                        importantLocations.Add(songOfTimeLocation);
                        var songOfTimePaths = LogicUtils.GetImportantLocations(ItemList, _settings, songOfTimeLocation, logicForImportance);
                        songOfTimeImportantItems = songOfTimePaths.Important;
                    }

                    _randomized.ImportantLocations = importantLocations.Union(songOfTimeImportantItems).Distinct().ToList().AsReadOnly();
                    _randomized.RequiredSongLocations = requiredSongLocations.Distinct().ToList().AsReadOnly();
                    _randomized.LocationsRequiredForMoonAccess = locationsRequiredForMoonAccess.Keys.ToList().AsReadOnly();

                    var spheres = new List<List<ItemLocationPair>>();
                    var acquired = new List<Item>();
                    acquired.AddRange(_settings.CustomStartingItemList);
                    acquired.AddRange(_randomized.BlitzExtraItems);
                    var ioAcquired = new List<ItemObject>();
                    bool spheresUpdated;
                    bool hasAcquired(ItemObject io)
                    {
                        return io.DependsOnItems.Where(item => ItemList[item].Item == item).All(acquired.Contains)
                            && (
                                !io.Conditionals.Any()
                                || io.Conditionals.Any(c => c.Where(item => ItemList[item].Item == item).All(acquired.Contains))
                            );
                    }
                    bool shouldAppearInPlaythrough(ItemObject io)
                    {
                        return io.IsRandomized && io.Item.DungeonEntrances() == null;
                    }
                    do
                    {
                        bool fakeItemsUpdated;
                        do
                        {
                            fakeItemsUpdated = false;

                            foreach (var io in ItemList.Where(io => !ioAcquired.Contains(io)))
                            {
                                var location = (Item)io.ID;
                                var mainLocation = location.MainLocation();
                                if (mainLocation.HasValue)
                                {
                                    if (shouldAppearInPlaythrough(ItemList[mainLocation.Value]))
                                    {
                                        continue;
                                    }
                                }
                                else if (shouldAppearInPlaythrough(io))
                                {
                                    continue;
                                }
                                if (hasAcquired(io))
                                {
                                    if (mainLocation.HasValue)
                                    {
                                        ioAcquired.Add(ItemList[mainLocation.Value]);
                                    }

                                    var item = ItemList.Single(x => (x.NewLocation ?? x.Item) == (mainLocation ?? location)).Item;
                                    acquired.Add(item);
                                    ioAcquired.Add(io);
                                    fakeItemsUpdated = true;
                                }
                            }

                        } while (fakeItemsUpdated);

                        spheresUpdated = false;
                        var currentSphere = new List<ItemLocationPair>();
                        var currentSphereItems = new List<Item>();
                        foreach (var io in ItemList.Where(io => !ioAcquired.Contains(io)))
                        {
                            var location = (Item)io.ID;
                            var mainLocation = location.MainLocation();
                            if (mainLocation.HasValue)
                            {
                                if (!shouldAppearInPlaythrough(ItemList[mainLocation.Value]))
                                {
                                    continue;
                                }
                            }
                            else if (!shouldAppearInPlaythrough(io))
                            {
                                continue;
                            }
                            if (hasAcquired(io))
                            {
                                ioAcquired.Add(io);

                                if (mainLocation.HasValue)
                                {
                                    ioAcquired.Add(ItemList[mainLocation.Value]);
                                    currentSphereItems.Add(location);
                                }

                                var item = ItemList.Single(x => x.NewLocation == (mainLocation ?? location)).Item;
                                currentSphereItems.Add(item);
                                if (location.DungeonEntrances() != null)
                                {
                                    currentSphere.Add(new ItemLocationPair
                                    {
                                        Item = item.Entrance() ?? item.ToString(),
                                        Location = location.Entrance() ?? location.ToString(),
                                    });
                                }
                                else if (_randomized.ImportantLocations.Contains(location))
                                {
                                    currentSphere.Add(new ItemLocationPair
                                    {
                                        Item = item.ProgressiveUpgradeName(_settings.ProgressiveUpgrades),
                                        Location = location.Location(ItemList) ?? location.ToString(),
                                    });
                                }
                            }
                        }
                        acquired.AddRange(currentSphereItems);

                        if (currentSphere.Any())
                        {
                            spheres.Add(currentSphere);
                            spheresUpdated = true;
                        }
                    } while (spheresUpdated);

                    _randomized.Spheres = spheres;
                }

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
