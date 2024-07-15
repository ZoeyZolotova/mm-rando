using MMR.Randomizer.Constants;
using System.Collections.Generic;
using MMR.Randomizer.GameObjects;
using System;
using System.Linq;
using MMR.Randomizer.Extensions;
using MMR.Randomizer.Attributes;
using System.Collections.ObjectModel;
using MMR.Randomizer.Models;
using MMR.Common.Extensions;
using MMR.Randomizer.Models.Settings;

namespace MMR.Randomizer.Utils
{
    public static class ItemUtils
    {
        public static bool IsShopItem(Item item)
        {
            return (item >= Item.ShopItemTradingPostRedPotion
                    && item <= Item.ShopItemZoraRedPotion)
                    || item == Item.ItemBombBag
                    || item == Item.UpgradeBigBombBag
                    || item == Item.MaskAllNight
                    || item == Item.ShopItemMilkBarChateau
                    || item == Item.ShopItemMilkBarMilk
                    || item == Item.ShopItemBusinessScrubMagicBean
                    || item == Item.ShopItemBusinessScrubGreenPotion
                    || item == Item.ShopItemBusinessScrubBluePotion
                    || item == Item.ShopItemGormanBrosMilk;
        }

        public static bool IsCowItem(Item item)
        {
            return (item >= Item.ItemRanchBarnMainCowMilk && item <= Item.ItemCoastGrottoCowMilk2);
        }

        public static bool IsSkulltulaToken(Item item)
        {
            return item >= Item.CollectibleSwampSpiderToken1 && item <= Item.CollectibleOceanSpiderToken30;
        }

        public static bool IsStrayFairy(Item item)
        {
            return item >= Item.CollectibleStrayFairyClockTown && item <= Item.CollectibleStrayFairyStoneTower15;
        }

        public static bool IsBottleCatchContent(Item item)
        {
            return item >= Item.BottleCatchFairy
                   && item <= Item.BottleCatchMushroom;
        }

        public static bool IsRegionRestricted(GameplaySettings settings, Item item)
        {
            if (settings.SmallKeyMode.HasFlag(SmallKeyMode.KeepWithinTemples) && SmallKeys().Contains(item))
            {
                return true;
            }

            if (settings.BossKeyMode.HasFlag(BossKeyMode.KeepWithinTemples) && BossKeys().Contains(item))
            {
                return true;
            }

            if (settings.StrayFairyMode.HasFlag(StrayFairyMode.KeepWithinTemples) && DungeonStrayFairies().Contains(item))
            {
                return true;
            }

            if (settings.BossRemainsMode.HasFlag(BossRemainsMode.KeepWithinTemples) && BossRemains().Contains(item))
            {
                return true;
            }

            if (settings.BossRemainsMode.HasFlag(BossRemainsMode.GreatFairyRewards) && BossRemains().Contains(item))
            {
                return true;
            }

            if (settings.DungeonNavigationMode.HasFlag(DungeonNavigationMode.KeepWithinTemples) && DungeonNavigation().Contains(item))
            {
                return true;
            }

            return false;
        }

        public static bool IsItemHinted(Item item, GameplaySettings settings)
        {
            if (settings.UpdateNPCText)
            {
                var npcTextHintedItems = new List<Item>
                {
                    Item.CollectibleStrayFairyClockTown, // Hinted by the Town Fairy Fountain
                    Item.ItemPowderKeg, // Hinted by the Bomb Shop Goron
                    Item.ItemBottleGoronRace, // Hinted by the Smithy
                    Item.ItemBottleBeavers, // Hinted by Evan
                    Item.MaskGaro, // Hinted by the Road to Ikana ghost
                    Item.SongTime, // Hinted by the Scarecrow
                    Item.SongSoaring, // Hinted by the southern swamp owl
                };

                if (npcTextHintedItems.Contains(item))
                {
                    return true;
                }
            }

            if (settings.OathHint)
            {
                if (item == Item.SongOath) // Hinted by the Giants
                {
                    return true;
                }
            }

            if (settings.RemainsHint)
            {
                if (BossRemains().Contains(item)) // Hinted by Tatl and Tael
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsStartingLocation(Item location)
        {
            return location == Item.MaskDeku || location == Item.SongHealing
                || (location >= Item.StartingSword && location <= Item.StartingHeartContainer2);
        }

        public static IEnumerable<Item> SmallKeys()
        {
            return new List<Item>
            {
                Item.ItemWoodfallKey1,
                Item.ItemSnowheadKey1,
                Item.ItemSnowheadKey2,
                Item.ItemSnowheadKey3,
                Item.ItemGreatBayKey1,
                Item.ItemStoneTowerKey1,
                Item.ItemStoneTowerKey2,
                Item.ItemStoneTowerKey3,
                Item.ItemStoneTowerKey4,
            }.AsEnumerable();
        }

        public static IEnumerable<Item> BossKeys()
        {
            return new List<Item>
            {
                Item.ItemWoodfallBossKey,
                Item.ItemSnowheadBossKey,
                Item.ItemGreatBayBossKey,
                Item.ItemStoneTowerBossKey,
            }.AsEnumerable();
        }

        public static IEnumerable<Item> DungeonStrayFairies()
        {
            return Enumerable.Range((int)Item.CollectibleStrayFairyWoodfall1, 60).Cast<Item>();
        }

        public static IEnumerable<Item> SwampSkulltulaTokens()
        {
            return Enumerable.Range((int)Item.CollectibleSwampSpiderToken1, 30).Cast<Item>();
        }

        public static IEnumerable<Item> OceanSkulltulaTokens()
        {
            return Enumerable.Range((int)Item.CollectibleOceanSpiderToken1, 30).Cast<Item>();
        }

        public static IEnumerable<Item> DungeonNavigation()
        {
            return new List<Item>
            {
                Item.ItemWoodfallMap,
                Item.ItemWoodfallCompass,
                Item.ItemSnowheadMap,
                Item.ItemSnowheadCompass,
                Item.ItemGreatBayMap,
                Item.ItemGreatBayCompass,
                Item.ItemStoneTowerMap,
                Item.ItemStoneTowerCompass,
            }.AsEnumerable();
        }

        public static IEnumerable<Item> BossRemains()
        {
            return Enumerable.Range((int)Item.RemainsOdolwa, 4).Cast<Item>();
        }

        public static IEnumerable<Item> GreatFairyRewards()
        {
            return new List<Item> { Item.FairySpinAttack, Item.FairyDoubleMagic, Item.FairyDoubleDefense, Item.ItemFairySword };
        }

        // todo cache
        public static IEnumerable<Item> DowngradableItems()
        {
            return Enum.GetValues(typeof(Item))
                .Cast<Item>()
                .Where(item => item.IsDowngradable());
        }

        // todo cache
        public static Dictionary<OverwritableAttribute.ItemSlot, ReadOnlyCollection<Item>> OverwriteableSlotItems()
        {
            return Enum.GetValues<Item>()
                .GroupBy(item => item.OverwriteableSlot())
                .Where(g => g.Key != OverwritableAttribute.ItemSlot.None)
                .ToDictionary(g => g.Key, g => g.ToList().AsReadOnly());
        }

        // todo cache
        public static IEnumerable<Item> CustomStartingItems()
        {
            return StartingItems().Where(item => !item.Name().Contains("Heart"));
        }

        // todo cache
        public static IEnumerable<Item> StartingItems()
        {
            return Enum.GetValues(typeof(Item))
                .Cast<Item>()
                .Where(item => item.HasAttribute<StartingItemAttribute>()
                    || item.HasAttribute<StartingTingleMapAttribute>()
                    || item.HasAttribute<StartingItemIdAttribute>());
        }

        // todo cache
        public static IEnumerable<Item> AllRupees()
        {
            return Enum.GetValues(typeof(Item))
                .Cast<Item>()
                .Where(item => item.Name()?.Contains("Rupee") == true);
        }
        
        private static List<Item> _allLocations;
        public static IEnumerable<Item> AllLocations()
        {
            return _allLocations ?? (_allLocations = Enum.GetValues<Item>().Where(item => item.Location() != null).ToList());
        }

        private static Dictionary<ClassicCategory, List<Item>> _itemsByClassicCategory;
        public static Dictionary<ClassicCategory, List<Item>> ItemsByClassicCategory()
        {
            return _itemsByClassicCategory ?? (_itemsByClassicCategory = AllLocations()
                .GroupBy(item => item.ClassicCategory())
                .Where(g => g.Key.HasValue)
                .ToDictionary(kvp => kvp.Key.Value, kvp => kvp.ToList()));
        }

        private static Dictionary<ItemCategory, List<Item>> _itemsByItemCategory;
        public static Dictionary<ItemCategory, List<Item>> ItemsByItemCategory()
        {
            return _itemsByItemCategory ?? (_itemsByItemCategory = AllLocations()
                .GroupBy(item => item.ItemCategory())
                .Where(g => g.Key.HasValue)
                .ToDictionary(kvp => kvp.Key.Value, kvp => kvp.ToList()));
        }

        private static Dictionary<LocationCategory, List<Item>> _itemsByLocationCategory;
        public static Dictionary<LocationCategory, List<Item>> ItemsByLocationCategory()
        {
            return _itemsByLocationCategory ?? (_itemsByLocationCategory = AllLocations()
                .GroupBy(item => item.LocationCategory())
                .Where(g => g.Key.HasValue)
                .ToDictionary(kvp => kvp.Key.Value, kvp => kvp.ToList()));
        }

        // todo cache
        public static IEnumerable<ushort> AllGetItemIndices()
        {
            return Enum.GetValues(typeof(Item))
                .Cast<Item>()
                .Where(item => item.HasAttribute<GetItemIndexAttribute>())
                .Select(item => item.GetAttribute<GetItemIndexAttribute>().Index);
        }

        // todo cache
        public static IEnumerable<int> AllGetBottleItemIndices()
        {
            return Enum.GetValues(typeof(Item))
                .Cast<Item>()
                .Where(item => item.HasAttribute<GetBottleItemIndicesAttribute>())
                .SelectMany(item => item.GetAttribute<GetBottleItemIndicesAttribute>().Indices);
        }

        public static ReadOnlyCollection<Item> JunkItems { get; private set; }
        public static ReadOnlyCollection<Item> LogicallyJunkItems { get; private set; }
        public static void PrepareJunkItems(GameplaySettings settings, List<ItemObject> itemList)
        {
            BlitzJunkLocations = new List<Item>();
            LogicallyJunkItems = itemList
                .Where(io => !itemList.Any(other => (other.DependsOnItems?.Contains(io.Item) ?? false) || (other.Conditionals?.Any(c => c.Contains(io.Item)) ?? false)))
                .Select(io => io.Item)
                .ToList()
                .AsReadOnly();

            IEnumerable<Item> getUniqueItems(IEnumerable<Item> items)
            {
                return items
                    .Where(item => item.Name() != null)
                    .GroupBy(item => item.Name())
                    .Where(g => g.Count() == 1)
                    .Select(g => g.Single());
            }

            var uniqueItems = getUniqueItems(settings.CustomItemList)
                .Union(getUniqueItems(Enum.GetValues<Item>()))
                .ToList();
            JunkItems = LogicallyJunkItems.Where(item => !uniqueItems.Contains(item)
                                          && item.ItemCategory() != ItemCategory.PiecesOfHeart
                                          && item.GetAttribute<ChestTypeAttribute>()?.Type == ChestTypeAttribute.ChestType.SmallWooden
                                      ).ToList().AsReadOnly();
        }

        public static bool IsJunk(Item item)
        {
            return item < 0 || JunkItems.Contains(item);
        }

        public static bool IsLogicallyJunk(Item item)
        {
            return item < 0 || LogicallyJunkItems.Contains(item);
        }

        private static List<Item> HintedJunkLocations;

        public static void PrepareHintedJunkLocations(GameplaySettings settings, Random random)
        {
            if (settings.OverrideHintPriorities != null && settings.OverrideHintItemCaps != null)
            {
                HintedJunkLocations = settings.OverrideHintPriorities.SelectMany((tier, i) =>
                {
                    var cap = settings.OverrideHintItemCaps.ElementAtOrDefault(i);
                    if (cap > 0)
                    {
                        var groupedLocations = tier
                            .Where(location => !IsLocationJunk(location, settings))
                            .GroupBy(location => ItemCombinableHints.GetValueOrDefault(location).name ?? location.ToString())
                            .ToList();
                        var numberOfLocationsToJunk = groupedLocations.Count - cap;
                        return groupedLocations
                            .Random(numberOfLocationsToJunk, random)
                            .SelectMany(g => g)
                            .ToList();
                    }
                    return new List<Item>();
                }).ToList();
            }
            else
            {
                HintedJunkLocations = new List<Item>();
            }
        }

        private static List<Item> BlitzJunkLocations;

        public static List<Item> PrepareBlitz(GameplaySettings settings, ItemList itemList, Random random)
        {
            var remainsAmountPool = new List<int>();
            if (settings.BossRemainsMode.HasFlag(BossRemainsMode.Blitz1))
            {
                remainsAmountPool.Add(1);
            }
            if (settings.BossRemainsMode.HasFlag(BossRemainsMode.Blitz2))
            {
                remainsAmountPool.Add(2);
            }
            if (settings.BossRemainsMode.HasFlag(BossRemainsMode.Blitz3))
            {
                remainsAmountPool.Add(3);
            }
            if (settings.BossRemainsMode.HasFlag(BossRemainsMode.Blitz4))
            {
                remainsAmountPool.Add(4);
            }
            if (remainsAmountPool.Count == 0)
            {
                return new List<Item>();
            }
            var remainsAmount = remainsAmountPool.Random(random, (amount) => 1.0f / amount);
            var extraStartingRemains = BossRemains().Intersect(settings.CustomStartingItemList).ToList();
            Item[] startingRemains;
            if (remainsAmount <= extraStartingRemains.Count)
            {
                startingRemains = extraStartingRemains.Random(remainsAmount, random);
            }
            else
            {
                startingRemains = extraStartingRemains.Union(BossRemains().Except(extraStartingRemains).ToList().Random(remainsAmount - extraStartingRemains.Count, random)).ToArray();
            }
            var result = new List<Item>();
            result.AddRange(startingRemains);
            var debugItemObjects = new List<ItemObject>();
            var remainsInLairs = new Dictionary<Item, Item>
            {
                { Item.RemainsOdolwa, Item.AreaOdolwasLair },
                { Item.RemainsGoht, Item.AreaGohtsLair },
                { Item.RemainsGyorg, Item.AreaGyorgsLair },
                { Item.RemainsTwinmold, Item.AreaTwinmoldsLair },
            };
            var lairsInDungeons = new Dictionary<Item, Item[]>
            {
                { Item.AreaOdolwasLair, new Item[] { Item.AreaWoodFallTempleAccess } },
                { Item.AreaGohtsLair, new Item[] { Item.AreaSnowheadTempleAccess } },
                { Item.AreaGyorgsLair, new Item[] { Item.AreaGreatBayTempleAccess } },
                { Item.AreaTwinmoldsLair, new Item[] { Item.AreaInvertedStoneTowerTempleAccess, Item.AreaStoneTowerTempleAccess } },
            };
            var remainsRegion = new Dictionary<Item, Region>
            {
                { Item.AreaWoodFallTempleAccess, Region.WoodfallTemple },
                { Item.AreaSnowheadTempleAccess, Region.SnowheadTemple },
                { Item.AreaGreatBayTempleAccess, Region.GreatBayTemple },
                { Item.AreaInvertedStoneTowerTempleAccess, Region.StoneTowerTemple },
            };
            IEnumerable<Item> filter(IEnumerable<Item> items)
            {
                return items
                    .Where(item => item.IsFake() || item.IsBottleCatchContent() || (!itemList[item].ItemOverride.HasValue && itemList[item].Item == itemList[item].NewLocation && !item.CanBeStartedWith()))
                    .Select(item => itemList[item].NewLocation ?? item);
            }
            foreach (var bossRemain in startingRemains)
            {
                var lairAccess = remainsInLairs[bossRemain];
                var newLairAccess = itemList[lairAccess].NewLocation ?? lairAccess;
                var dungeonAccesses = lairsInDungeons[newLairAccess];
                foreach (var dungeonAccess in dungeonAccesses)
                {
                    var newDungeonAccess = itemList[dungeonAccess].NewLocation ?? dungeonAccess;
                    BlitzJunkLocations.Add(newDungeonAccess);
                    bool updated;
                    do
                    {
                        updated = false;
                        foreach (var io in itemList)
                        {
                            var location = (Item)io.ID;
                            if (BlitzJunkLocations.Contains(location))
                            {
                                continue;
                            }
                            var dependsOnItems = filter(io.DependsOnItems);
                            var conditionals = io.Conditionals.Select(c => filter(c));
                            if (dependsOnItems.Intersect(BlitzJunkLocations).Any() || (conditionals.Any() && conditionals.All(c => c.Intersect(BlitzJunkLocations).Any())))
                            {
                                BlitzJunkLocations.Add(location);
                                debugItemObjects.Add(io);
                                updated = true;
                                if (!io.ItemOverride.HasValue && io.NewLocation == io.Item && io.Item.CanBeStartedWith() && !result.Contains(io.Item))
                                {
                                    result.Add(io.Item); // player starts with any non-randomized blitzed item
                                }
                            }
                            else
                            {
                                io.Conditionals.RemoveAll(c => filter(c).Intersect(BlitzJunkLocations).Any());
                            }
                        }
                    } while (updated);
                    if (remainsRegion.ContainsKey(newDungeonAccess))
                    {
                        Item[] dungeonItems = ItemUtils.AllLocations().Where(item => item.GetAttribute<RegionAttribute>()?.Region == remainsRegion[newDungeonAccess]).ToArray();
                        if (settings.StrayFairyMode.HasFlag(StrayFairyMode.KeepWithinTemples) && settings.StrayFairyMode.HasFlag(StrayFairyMode.KeepWithinArea))
                        {
                            result.AddRange(dungeonItems.Where(item => ItemUtils.IsStrayFairy(item)));
                        }
                        dungeonItems = ItemUtils.AllLocations().Where(item => item.GetAttribute<RegionAttribute>()?.Region == remainsRegion[dungeonAccess]).ToArray();
                        if (settings.DungeonNavigationMode.HasFlag(DungeonNavigationMode.KeepWithinTemples) && settings.DungeonNavigationMode.HasFlag(DungeonNavigationMode.KeepWithinArea))
                        {
                            result.AddRange(dungeonItems.Where(item => ItemUtils.DungeonNavigation().Contains(item)));
                        }
                        if (settings.SmallKeyMode.HasFlag(SmallKeyMode.KeepWithinTemples) && settings.SmallKeyMode.HasFlag(SmallKeyMode.KeepWithinArea))
                        {
                            result.AddRange(dungeonItems.Where(item => ItemUtils.SmallKeys().Contains(item)));
                        }
                        if (settings.BossKeyMode.HasFlag(BossKeyMode.KeepWithinTemples) && settings.BossKeyMode.HasFlag(BossKeyMode.KeepWithinArea))
                        {
                            result.AddRange(dungeonItems.Where(item => ItemUtils.BossKeys().Contains(item)));
                        }
                    }
                }
            }
            BlitzJunkLocations.RemoveAll(location => (location.Location() == null && location.MainLocation() == null) || location.Entrance() != null);
            return result;
        }

        private static List<Item> SettingJunkLocations;

        public static void PrepareTricksAndSettings(List<Item> initialJunkedLocations, ItemList itemList)
        {
            SettingJunkLocations = initialJunkedLocations;
            IEnumerable<Item> filter(IEnumerable<Item> items)
            {
                return items.Where(item => item.IsFake());
            }
            bool updated;
            do
            {
                updated = false;
                foreach (var io in itemList)
                {
                    var location = (Item)io.ID;
                    if (SettingJunkLocations.Contains(location))
                    {
                        continue;
                    }

                    var dependsOnItems = filter(io.DependsOnItems);
                    var conditionals = io.Conditionals.Select(c => filter(c));
                    if (dependsOnItems.Intersect(SettingJunkLocations).Any() || (conditionals.Any() && conditionals.All(c => c.Intersect(SettingJunkLocations).Any())))
                    {
                        if (location.IsBottleCatchContent() || location.CanBeStartedWith())
                        {
                            throw new Exception($"Unable to reach {location} given your current settings. Please check your logic file.");
                        }
                        SettingJunkLocations.Add(location);
                        updated = true;
                    }
                    else
                    {
                        io.Conditionals.RemoveAll(c => filter(c).Intersect(SettingJunkLocations).Any());
                    }
                }
            } while (updated);

            SettingJunkLocations.RemoveAll(location => !location.Region(itemList).HasValue || location.Entrance() != null);
        }

        public static bool IsLocationJunk(Item location, GameplaySettings settings)
        {
            return settings.CustomJunkLocations.Contains(location)
                || (HintedJunkLocations?.Contains(location) == true)
                || (BlitzJunkLocations?.Contains(location) == true)
                || (SettingJunkLocations?.Contains(location) == true);
        }

        public static bool CanBeRequired(Item item)
        {
            var itemCategory = item.ItemCategory();
            return item >= 0
                && itemCategory != ItemCategory.StrayFairies
                && itemCategory != ItemCategory.SkulltulaTokens
                && itemCategory != ItemCategory.HeartContainers
                && itemCategory != ItemCategory.PiecesOfHeart
                && itemCategory != ItemCategory.RecoveryHearts
                && itemCategory != ItemCategory.NotebookEntries;
        }

        public static bool IsRequired(Item item, Item locationForImportance, RandomizedResult randomizedResult, bool anythingCanBeRequired = false)
        {
            if (anythingCanBeRequired && randomizedResult.RequiredSongLocations?.Contains(locationForImportance) == true)
            {
                return true;
            }
            return (anythingCanBeRequired || (CanBeRequired(item) && !IsItemHinted(item, randomizedResult.Settings))) && randomizedResult.LocationsRequiredForMoonAccess?.Contains(locationForImportance) == true;
        }

        public static bool IsImportant(Item item, Item locationForImportance, RandomizedResult randomizedResult)
        {
            return item >= 0
                && randomizedResult.ImportantLocations?.Contains(locationForImportance) == true;
        }

        public static readonly ReadOnlyDictionary<string, ReadOnlyCollection<Item>> CombinableHints = new ReadOnlyDictionary<string, ReadOnlyCollection<Item>>(new Dictionary<string, ReadOnlyCollection<Item>>
        {
            {
                "Ranch Sisters Defense", new List<Item>
                {
                    Item.ItemBottleAliens,
                    Item.NotebookSaveTheCows,
                    Item.MaskRomani,
                    Item.NotebookProtectMilkDelivery,
                }.AsReadOnly()
            },
            {
                "Beaver Races", new List<Item>
                {
                    Item.ItemBottleBeavers,
                    Item.HeartPieceBeaverRace,
                }.AsReadOnly()
            },
            {
                "Town Archery", new List<Item>
                {
                    Item.UpgradeBigQuiver,
                    Item.HeartPieceTownArchery,
                }.AsReadOnly()
            },
            {
                "Swamp Archery", new List<Item>
                {
                    Item.UpgradeBiggestQuiver,
                    Item.HeartPieceSwampArchery,
                }.AsReadOnly()
            },
            {
                "Ocean Spider House", new List<Item>
                {
                    Item.UpgradeGiantWallet,
                    Item.MundaneItemOceanSpiderHouseDay2PurpleRupee,
                    Item.MundaneItemOceanSpiderHouseDay3RedRupee,
                }.AsReadOnly()
            },
            {
                "Inn Reservation", new List<Item>
                {
                    Item.TradeItemRoomKey,
                    Item.NotebookInnReservation,
                }.AsReadOnly()
            },
            {
                "Midnight Meeting", new List<Item>
                {
                    Item.TradeItemKafeiLetter,
                    Item.NotebookPromiseAnjuDelivery,
                }.AsReadOnly()
            },
            {
                "Letter to Kafei Delivery", new List<Item>
                {
                    Item.NotebookDepositLetterToKafei,
                    Item.TradeItemPendant,
                    Item.NotebookMeetKafei,
                    Item.NotebookPromiseKafei,
                    Item.MaskKeaton,
                    Item.TradeItemMamaLetter,
                    Item.NotebookCuriosityShopManSGift,
                    Item.NotebookPromiseCuriosityShopMan,
                }.AsReadOnly()
            },
            {
                "Deku Playground", new List<Item>
                {
                    Item.MundaneItemDekuPlaygroundPurpleRupee,
                    Item.HeartPieceDekuPlayground,
                }.AsReadOnly()
            },
            {
                "Honey and Darling", new List<Item>
                {
                    Item.MundaneItemHoneyAndDarlingPurpleRupee,
                    Item.HeartPieceHoneyAndDarling,
                }.AsReadOnly()
            },
            {
                "Romani's Game", new List<Item>
                {
                    Item.SongEpona,
                    Item.NotebookPromiseRomani,
                }.AsReadOnly()
            },
            {
                "Madame Aroma in Bar", new List<Item>
                {
                    Item.ItemBottleMadameAroma,
                    Item.NotebookDeliverLetterToMama,
                }.AsReadOnly()
            },
            {
                "Bombers' Hide and Seek", new List<Item>
                {
                    Item.ItemNotebook,
                    Item.NotebookMeetBombers,
                    Item.NotebookLearnBombersCode,
                }.AsReadOnly()
            },
            {
                "Mayor", new List<Item>
                {
                    Item.HeartPieceNotebookMayor,
                    Item.NotebookDotoursThanks,
                }.AsReadOnly()
            },
            {
                "Rosa Sisters", new List<Item>
                {
                    Item.HeartPieceNotebookRosa,
                    Item.NotebookRosaSistersThanks,
                }.AsReadOnly()
            },
            {
                "Toilet Hand", new List<Item>
                {
                    Item.HeartPieceNotebookHand,
                    Item.NotebookToiletHandSThanks,
                }.AsReadOnly()
            },
            {
                "Grandma Stories", new List<Item>
                {
                    Item.HeartPieceNotebookGran1,
                    Item.NotebookGrandmaShortStory,
                    Item.HeartPieceNotebookGran2,
                    Item.NotebookGrandmaLongStory,
                }.AsReadOnly()
            },
            {
                "Postman's Game", new List<Item>
                {
                    Item.HeartPieceNotebookPostman,
                    Item.NotebookPostmansGame,
                }.AsReadOnly()
            },
            {
                "Madame Aroma in Office", new List<Item>
                {
                    Item.MaskKafei,
                    Item.NotebookPromiseMadameAroma,
                }.AsReadOnly()
            },
            {
                "All-Night Mask Purchase", new List<Item>
                {
                    Item.MaskAllNight,
                    Item.NotebookPurchaseCuriosityShopItem,
                }.AsReadOnly()
            },
            {
                "Grog", new List<Item>
                {
                    Item.MaskBunnyHood,
                    Item.NotebookGrogsThanks,
                }.AsReadOnly()
            },
            {
                "Gorman Bros Race", new List<Item>
                {
                    Item.MaskGaro,
                    Item.NotebookDefeatGormanBrothers,
                }.AsReadOnly()
            },
            {
                "Gorman", new List<Item>
                {
                    Item.MaskCircusLeader,
                    Item.NotebookMovingGorman,
                }.AsReadOnly()
            },
            {
                "Postman's Freedom", new List<Item>
                {
                    Item.MaskPostmanHat,
                    Item.NotebookPostmansFreedom,
                }.AsReadOnly()
            },
            {
                "Anju and Kafei", new List<Item>
                {
                    Item.MaskCouple,
                    Item.NotebookUniteAnjuAndKafei,
                }.AsReadOnly()
            },
            {
                "Old Lady", new List<Item>
                {
                    Item.MaskBlast,
                    Item.NotebookSaveOldLady,
                }.AsReadOnly()
            },
            {
                "Kamaro", new List<Item>
                {
                    Item.MaskKamaro,
                    Item.NotebookPromiseKamaro,
                }.AsReadOnly()
            },
            {
                "Invisible Soldier", new List<Item>
                {
                    Item.MaskStone,
                    Item.NotebookMeetShiro,
                    Item.NotebookSaveInvisibleSoldier,
                }.AsReadOnly()
            },
            {
                "Guru-Guru", new List<Item>
                {
                    Item.MaskBremen,
                    Item.NotebookGuruGuru,
                }.AsReadOnly()
            },
        });

        public static readonly ReadOnlyDictionary<Item, (string name, ReadOnlyCollection<Item> locations)> ItemCombinableHints = new ReadOnlyDictionary<Item, (string, ReadOnlyCollection<Item>)>(CombinableHints.SelectMany(kvp => kvp.Value.Select(x => new { Name = kvp.Key, Locations = kvp.Value, Location = x })).ToDictionary(x => x.Location, x => (x.Name, x.Locations)));

        public static readonly ReadOnlyCollection<ReadOnlyCollection<Item>> ForbiddenStartTogether = new List<List<Item>>()
        {
            new List<Item>
            {
                Item.ItemBow,
                Item.UpgradeBigQuiver,
                Item.UpgradeBiggestQuiver,
            },
            new List<Item>
            {
                Item.ItemBombBag,
                Item.UpgradeBigBombBag,
                Item.UpgradeBiggestBombBag,
            },
            new List<Item>
            {
                Item.UpgradeAdultWallet,
                Item.UpgradeGiantWallet,
                Item.UpgradeRoyalWallet,
            },
            new List<Item>
            {
                Item.StartingSword,
                Item.UpgradeRazorSword,
                Item.UpgradeGildedSword,
            },
            new List<Item>
            {
                Item.StartingShield,
                Item.ShopItemTradingPostShield,
                Item.ShopItemZoraShield,
                Item.UpgradeMirrorShield,
            },
            new List<Item>
            {
                Item.FairyMagic,
                Item.FairyDoubleMagic,
            },
            new List<Item>
            {
                Item.SongLullabyIntro,
                Item.SongLullaby,
            },
        }.Select(list => list.AsReadOnly()).ToList().AsReadOnly();

        public static List<Item> ConvertStringToItemList(List<Item> baseItemList, string c)
        {
            if (string.IsNullOrWhiteSpace(c))
            {
                return new List<Item>();
            }
            var sectionCount = (int)Math.Ceiling(baseItemList.Count / 32.0);
            var result = new List<Item>();
            if (string.IsNullOrWhiteSpace(c))
            {
                return result;
            }
            try
            {
                string[] v = c.Split('-');
                int[] vi = new int[sectionCount];
                if (v.Length != vi.Length)
                {
                    return null;
                }
                for (int i = 0; i < sectionCount; i++)
                {
                    if (v[sectionCount - 1 - i] != "")
                    {
                        vi[i] = Convert.ToInt32(v[sectionCount - 1 - i], 16);
                    }
                }
                for (int i = 0; i < 32 * sectionCount; i++)
                {
                    int j = i / 32;
                    int k = i % 32;
                    if (((vi[j] >> k) & 1) > 0)
                    {
                        if (i >= baseItemList.Count)
                        {
                            throw new IndexOutOfRangeException();
                        }
                        result.Add(baseItemList[i]);
                    }
                }
            }
            catch
            {
                return null;
            }
            return result;
        }

        public static string ConvertItemListToString(List<Item> baseItemList, List<Item> itemList)
        {
            var groupCount = (int)Math.Ceiling(baseItemList.Count / 32.0);
            int[] n = new int[groupCount];
            string[] ns = new string[groupCount];
            foreach (var item in itemList)
            {
                var i = baseItemList.IndexOf(item);
                int j = i / 32;
                int k = i % 32;
                n[j] |= (int)(1 << k);
                ns[j] = Convert.ToString(n[j], 16);
            }

            return string.Join("-", ns.Reverse());
        }
    }
}
