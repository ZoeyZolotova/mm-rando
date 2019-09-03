using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMRando.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class ShopInventoryAttribute : Attribute
    {
        private const int BaseShopInventoryDataAddress = 0x00CDDC60;

        public int ShopItemAddress { get; private set; }
        public ShopKeeper Keeper { get; private set; }

        public ShopInventoryAttribute(ShopKeeper shopKeeper, int shopItemIndex)
        {
            ShopItemAddress = BaseShopInventoryDataAddress + (int)shopKeeper + (shopItemIndex * 0x20);
            Keeper = shopKeeper;
        }

        public enum ShopKeeper
        {
            WitchShop = 0x11E0,
            TradingPostMain = 0x1240,
            TradingPostPartTimer = 0x1340,
            CuriosityShop = 0x1440,
            BombShop = 0x14C0,
            ZoraShop = 0x1540,
            GoronShop = 0x15A0,
            GoronShopSpring = 0x1600,
        }
    }
}
