using MMR.Randomizer.Attributes;
using MMR.Randomizer.GameObjects;
using MMR.Common.Extensions;
using MMR.Randomizer.Models.Rom;
using MMR.Randomizer.Attributes.Entrance;
using System.Collections.Generic;
using System.Linq;
using MMR.Randomizer.Models.Settings;

namespace MMR.Randomizer.Extensions
{
    public static class ItemExtensions
    {
        public static ushort? GetItemIndex(this Item item)
        {
            return item.GetAttribute<GetItemIndexAttribute>()?.Index;
        }

        public static IEnumerable<ushort> GetCollectableIndices(this Item item)
        {
            return item.GetAttributes<CollectableIndexAttribute>().Select(x => x.Index);
        }

        public static bool IsNullableItem(this Item item)
        {
            return item.HasAttribute<NullableItemAttribute>();
        }

        public static int[] GetBottleItemIndices(this Item item)
        {
            return item.GetAttribute<GetBottleItemIndicesAttribute>()?.Indices;
        }

        public static string Name(this Item item)
        {
            return item.GetAttribute<ItemNameAttribute>()?.Name;
        }

        public static string ProgressiveUpgradeName(this Item item, bool progressiveUpgradesEnabled)
        {
            if (progressiveUpgradesEnabled)
            {
                if (item == Item.StartingSword || item == Item.UpgradeRazorSword || item == Item.UpgradeGildedSword)
                {
                    return "Sword Upgrade";
                }
                if (item == Item.FairyMagic || item == Item.FairyDoubleMagic)
                {
                    return "Magic Power Upgrade";
                }
                if (item == Item.UpgradeAdultWallet || item == Item.UpgradeGiantWallet || item == Item.UpgradeRoyalWallet)
                {
                    return "Wallet Upgrade";
                }
                if (item == Item.ItemBombBag || item == Item.UpgradeBigBombBag || item == Item.UpgradeBiggestBombBag)
                {
                    return "Bomb Bag Upgrade";
                }
                if (item == Item.ItemBow || item == Item.UpgradeBigQuiver || item == Item.UpgradeBiggestQuiver)
                {
                    return "Bow Upgrade";
                }
            }
            return item.Name();
        }

        public static string Location(this Item item)
        {
            return item.GetAttribute<LocationNameAttribute>()?.Name;
        }

        public static Region? Region(this Item item)
        {
            return item.GetAttribute<RegionAttribute>()?.Region;
        }

        public static Item? MainLocation(this Item item)
        {
            return item.GetAttribute<MainLocationAttribute>()?.Location;
        }

        public static string Entrance(this Item item)
        {
            return item.GetAttribute<EntranceNameAttribute>()?.Name;
        }

        public static ShopTextAttribute ShopTexts(this Item item)
        {
            return item.GetAttribute<ShopTextAttribute>();
        }

        public static string[] ItemHints(this Item item)
        {
            return item.GetAttribute<GossipItemHintAttribute>().Values;
        }

        public static string[] LocationHints(this Item item)
        {
            return item.GetAttribute<GossipLocationHintAttribute>().Values;
        }

        public static bool IsRepeatable(this Item item)
        {
            return item.HasAttribute<RepeatableAttribute>();
        }

        public static bool IsReturnable(this Item item, GameplaySettings settings)
        {
            return item.GetAttribute<ReturnableAttribute>()?.Condition(settings) ?? false;
        }

        public static bool IsRupeeRepeatable(this Item item)
        {
            return item.HasAttribute<RupeeRepeatableAttribute>();
        }

        public static bool IsDowngradable(this Item item)
        {
            return item.HasAttribute<DowngradableAttribute>();
        }

        public static bool IsTemporary(this Item item, GameplaySettings settings)
        {
            return item.GetAttribute<TemporaryAttribute>()?.Condition(settings) ?? false;
        }

        public static ItemCategory? ItemCategory(this Item item)
        {
            return item.GetAttribute<ItemPoolAttribute>()?.ItemCategory;
        }

        public static LocationCategory? LocationCategory(this Item item)
        {
            return item.GetAttribute<ItemPoolAttribute>()?.LocationCategory;
        }

        public static bool IsFake(this Item item)
        {
            return item.Name() == null;
        }

        public static IList<DungeonEntrance> DungeonEntrances(this Item item)
        {
            if (!item.HasAttribute<DungeonEntranceAttribute>())
            {
                return null;
            }
            var result = new List<DungeonEntrance>();
            var attr = item.GetAttribute<DungeonEntranceAttribute>();
            result.Add(attr.Entrance);
            if (attr.Pair.HasValue)
            {
                result.Add(attr.Pair.Value);
            }
            return result;
        }

        public static bool IsOverwritable(this Item item)
        {
            return item.HasAttribute<OverwritableAttribute>();
        }

        public static bool IsSong(this Item item)
        {
            return (Item.SongTime <= item && item <= Item.SongOath);
        }

        public static ChestTypeAttribute.ChestType ChestType(this Item item)
        {
            return item.GetAttribute<ChestTypeAttribute>().Type;
        }

        public static bool IsPurchaseable(this Item item)
        {
            return item.HasAttribute<PurchaseableAttribute>();
        }

        public static bool IsVisible(this Item item)
        {
            return item.HasAttribute<VisibleAttribute>();
        }

        public static bool IsExclusiveItem(this Item item)
        {
            return item.HasAttribute<ExclusiveItemAttribute>();
        }

        public static GetItemEntry ExclusiveItemEntry(this Item item)
        {
            return new GetItemEntry
            {
                ItemGained = item.GetAttribute<ExclusiveItemAttribute>().Item,
                Flag = item.GetAttribute<ExclusiveItemAttribute>().Flags,
                Index = item.GetAttribute<ExclusiveItemGraphicAttribute>().Graphic,
                Type = item.GetAttribute<ExclusiveItemAttribute>().Type,
                Message = (short)item.GetAttribute<ExclusiveItemMessageAttribute>().Id,
                Object = (short)item.GetAttribute<ExclusiveItemGraphicAttribute>().Object,
            };
        }

        public static string ExclusiveItemMessage(this Item item)
        {
            return item.GetAttribute<ExclusiveItemMessageAttribute>().Message;
        }

        public static bool IsBottleCatchContent(this Item item)
        {
            return item >= Item.BottleCatchFairy
                   && item <= Item.BottleCatchMushroom;
        }

        public static bool IsSameType(this Item item, Item other)
        {
            return item.IsBottleCatchContent() == other.IsBottleCatchContent();
        }
    }
}
