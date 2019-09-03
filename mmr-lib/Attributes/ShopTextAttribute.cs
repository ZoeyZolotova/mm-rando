using System;

namespace MMRando.Attributes
{
    public class ShopTextAttribute : Attribute
    {
        public string Default { get; private set; }
        public string WitchShop { get; private set; }
        public string TradingPostMain { get; private set; }
        public string TradingPostPartTimer { get; private set; }
        public string CuriosityShop { get; private set; }
        public string BombShop { get; private set; }
        public string ZoraShop { get; private set; }
        public string GoronShop { get; private set; }
        public string GoronShopSpring { get; private set; }

        public bool IsMultiple { get; private set; }

        public ShopTextAttribute(string defaultText,
            string witchShop = null,
            string tradingPostMain = null,
            string tradingPostPartTimer = null,
            string curiosityShop = null,
            string bombShop = null,
            string zoraShop = null,
            string goronShop = null,
            string goronSpringShop = null,
            bool isMultiple = false)
        {
            Default = defaultText;
            WitchShop = witchShop;
            TradingPostMain = tradingPostMain;
            TradingPostPartTimer = tradingPostPartTimer;
            CuriosityShop = curiosityShop;
            BombShop = bombShop;
            ZoraShop = zoraShop;
            GoronShop = goronShop;
            GoronShopSpring = goronSpringShop;
            IsMultiple = isMultiple;
        }
    }
}
