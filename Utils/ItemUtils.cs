﻿using MMRando.Constants;
using System.Collections.Generic;
using MMRando.GameObjects;
using System;
using System.Linq;
using MMRando.Extensions;
using MMRando.Attributes;
using System.Collections.ObjectModel;
using MMRando.Models;
using MMRando.Models.Settings;

namespace MMRando.Utils
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


        public static bool IsKey(Item item)
        {
            return item == Item.ItemWoodfallKey1
                || item == Item.ItemSnowheadKey1
                || item == Item.ItemSnowheadKey2
                || item == Item.ItemSnowheadKey3
                || item == Item.ItemGreatBayKey1
                || (item >= Item.ItemStoneTowerKey1 && item <= Item.ItemStoneTowerKey4);
        }

        public static bool IsBossKey(Item item)
        {
            return item == Item.ItemWoodfallBossKey
                || item == Item.ItemSnowheadBossKey
                || item == Item.ItemGreatBayBossKey
                || item == Item.ItemStoneTowerBossKey;
        }

        public static bool IsDungeonMapCompass(Item item)
        {
            return item == Item.ItemWoodfallMap
                || item == Item.ItemWoodfallCompass
                || item == Item.ItemSnowheadMap
                || item == Item.ItemSnowheadCompass
                || item == Item.ItemGreatBayMap
                || item == Item.ItemGreatBayCompass
                || item == Item.ItemStoneTowerMap
                || item == Item.ItemStoneTowerCompass;
        }

        public static bool IsDungeonItemMatchesAlgorithm(Item item, DungeonItemAlgorithm algo, SettingsObject settings)
        {
            if( IsDungeonMapCompass(item))
            {
                return settings.MapCompassPlacement == algo;
            }
            else if ( IsKey(item) )
            {
                return settings.KeyPlacement == algo;
            }
            else if ( IsBossKey(item))
            {
                return settings.BossKeyPlacement == algo;
            }
            else if ( IsStrayFairy(item) )
            {
                return settings.FairyPlacement == algo;
            }
            else
            {
                return false;
            }
        }

        public static bool IsInvertedST(Item item)
        {
            return item == Item.MaskGiant
                || item == Item.ItemStoneTowerBossKey
                || item == Item.ItemStoneTowerKey3
                || item == Item.ItemStoneTowerKey4
                || item == Item.CollectibleStrayFairyStoneTower6
                || item == Item.CollectibleStrayFairyStoneTower7
                || item == Item.CollectibleStrayFairyStoneTower9;
        }

        public static bool IsRemain(Item item)
        {
            return (int)item >= (int)Item.RemainOdolwa && (int)item <= (int)Item.RemainTwinmold;
        }

        public static int AddItemOffset(int itemId)
        {
            if (itemId >= (int)Item.AreaSouthAccess)
            {
                itemId += Items.NumberOfAreasAndOther;
            }
            if (itemId >= (int)Item.OtherOneMask)
            {
                itemId += 5;
            }
            return itemId;
        }

        public static int SubtractItemOffset(int itemId)
        {
            if (itemId >= (int)Item.OtherOneMask)
            {
                itemId -= 5;
            }
            if (itemId >= (int)Item.AreaSouthAccess)
            {
                itemId -= Items.NumberOfAreasAndOther;
            }
            return itemId;
        }

        public static bool IsBottleCatchContent(Item item)
        {
            return item >= Item.BottleCatchFairy
                   && item <= Item.BottleCatchMushroom;
        }

        public static bool IsMoonLocation(Item location)
        {
            return location >= Item.HeartPieceDekuTrial && location <= Item.ChestLinkTrialBombchu10;
        }

        public static bool IsStartingLocation(Item location)
        {
            return location == Item.MaskDeku || location == Item.SongHealing
                || (location >= Item.StartingSword && location <= Item.StartingHeartContainer2);
        }

        public static bool IsSong(Item item)
        {
            return item >= Item.SongHealing
                && item <= Item.SongOath;
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
                .Where(item => item.HasAttribute<StartingItemAttribute>());
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
            return _allLocations ?? (_allLocations = Enum.GetValues(typeof(Item)).Cast<Item>().Where(item => item.Location() != null).ToList());
        }

        // todo cache
        public static IEnumerable<int> AllGetItemIndices()
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
            JunkItems = itemList.Where(io => io.Item.GetAttribute<ChestTypeAttribute>()?.Type == ChestTypeAttribute.ChestType.SmallWooden && !itemList.Any(other => (other.DependsOnItems?.Contains(io.Item) ?? false) || (other.Conditionals?.Any(c => c.Contains(io.Item)) ?? false))).Select(io => io.Item).ToList();
        }
        public static bool IsJunk(Item item)
        {
            return JunkItems.Contains(item);
        }

        public static bool IsRequired(Item item, RandomizedResult randomizedResult)
        {
            return !item.Name().Contains("Heart")
                        && (randomizedResult.Settings.AddSongs || !IsSong(item))
                        && !IsStrayFairy(item)
                        && !IsSkulltulaToken(item)
                        && randomizedResult.ItemsRequiredForMoonAccess.Contains(item);
        }

        public static bool IsImportant(Item item, RandomizedResult randomizedResult)
        {
            return !item.Name().Contains("Heart") && randomizedResult.ImportantItems.Contains(item);
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
    }
}
