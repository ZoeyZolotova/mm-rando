using System;

namespace MMRando.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class ShopRoomAttribute : Attribute
    {
        public int RoomObjectAddress { get; private set; }

        public ShopRoomAttribute(Room room, int roomObjectOffset)
        {
            RoomObjectAddress = (int)room + roomObjectOffset;
        }

        public enum Room
        {
            TradingPost = 0x02683000,
            WitchShop = 0x01F66000,
            BombShop = 0x02D7A000,
            GoronShop = 0x0276E000,
            ZoraShop = 0x02A44000,
            CuriosityShop = 0x01FB5000,
        }
    }
}
