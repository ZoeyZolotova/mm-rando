using System;

namespace MMRando.GameObjects
{
    public enum TingleText
    {
        [TingleShop(Item.ItemTingleMapTown, 5, Item.ItemTingleMapWoodfall, 40)]
        Town = 0x1D11,

        [TingleShop(Item.ItemTingleMapWoodfall, 20, Item.ItemTingleMapSnowhead, 40)]
        Swamp = 0x1D12,

        [TingleShop(Item.ItemTingleMapSnowhead, 20, Item.ItemTingleMapRanch, 40)]
        Mountain = 0x1D13,

        [TingleShop(Item.ItemTingleMapRanch, 20, Item.ItemTingleMapGreatBay, 40)]
        Ranch = 0x1D14,

        [TingleShop(Item.ItemTingleMapGreatBay, 20, Item.ItemTingleMapStoneTower, 40)]
        Ocean = 0x1D15,

        [TingleShop(Item.ItemTingleMapStoneTower, 20, Item.ItemTingleMapTown, 40)]
        Canyon = 0x1D16
    }

    public class TingleShopAttribute : Attribute
    {
        public Item[] Items { get; private set; }
        public int[] Prices { get; private set; }

        public TingleShopAttribute(Item item1, int price1, Item item2, int price2)
        {
            Items = new Item[] { item1, item2 };
            Prices = new int[] { price1, price2 };
        }
    }
}
