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
            if (settings.SmallKeyMode.HasFlag(SmallKeyMode.KeepWithinDungeon) && SmallKeys().Contains(item))
            {
                return true;
            }

            if (settings.BossKeyMode.HasFlag(BossKeyMode.KeepWithinDungeon) && BossKeys().Contains(item))
            {
                return true;
            }

            if (settings.StrayFairyMode.HasFlag(StrayFairyMode.KeepWithinDungeon) && DungeonStrayFairies().Contains(item))
            {
                return true;
            }

            if (settings.BossRemainsMode.HasFlag(BossRemainsMode.KeepWithinDungeon) && BossRemains().Contains(item))
            {
                return true;
            }

            if (settings.BossRemainsMode.HasFlag(BossRemainsMode.GreatFairyRewards) && BossRemains().Contains(item))
            {
                return true;
            }

            return false;
        }

        public static bool IsStartingLocation(Item location)
        {
            return location == Item.MaskDeku || location == Item.SongHealing
                || (location >= Item.StartingSword && location <= Item.StartingHeartContainer2);
        }

        public static bool IsSong(Item item)
        {
            return (item >= Item.SongTime
                && item <= Item.SongOath)
                || (item.MainLocation().HasValue && IsSong(item.MainLocation().Value));
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

        public static IEnumerable<Item> BossRemains()
        {
            return Enumerable.Range((int)Item.RemainsOdolwa, 4).Cast<Item>();
        }

        // todo cache
        public static IEnumerable<Item> DowngradableItems()
        {
            return Enum.GetValues(typeof(Item))
                .Cast<Item>()
                .Where(item => item.IsDowngradable());
        }

        // todo cache
        public static IEnumerable<Item> OverwritableItems()
        {
            return Enum.GetValues(typeof(Item))
                .Cast<Item>()
                .Where(item => item.IsOverwritable());
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

        public static List<Item> JunkItems { get; private set; }
        public static void PrepareJunkItems(List<ItemObject> itemList)
        {
            JunkItems = itemList.Where(io => io.Item != Item.CollectableIkanaGraveyardDay2Bats1 && io.Item.GetAttribute<ChestTypeAttribute>()?.Type == ChestTypeAttribute.ChestType.SmallWooden && !itemList.Any(other => (other.DependsOnItems?.Contains(io.Item) ?? false) || (other.Conditionals?.Any(c => c.Contains(io.Item)) ?? false))).Select(io => io.Item).ToList();
        }
        public static bool IsJunk(Item item)
        {
            return item == Item.RecoveryHeart || item == Item.IceTrap || JunkItems.Contains(item);
        }

        public static bool CanBeRequired(Item item)
        {
            return !item.Name().Contains("Heart")
                && !IsStrayFairy(item)
                && !IsSkulltulaToken(item)
                && item != Item.IceTrap;
        }

        public static bool IsRequired(Item item, Item locationForImportance, RandomizedResult randomizedResult)
        {
            return CanBeRequired(item) && randomizedResult.LocationsRequiredForMoonAccess.Contains(locationForImportance);
        }

        public static bool IsImportant(Item item, Item locationForImportance, RandomizedResult randomizedResult)
        {
            return !item.Name().Contains("Heart")
                && item != Item.IceTrap
                && randomizedResult.ImportantLocations.Contains(locationForImportance);
        }

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
