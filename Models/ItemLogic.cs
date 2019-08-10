using MMRando.Extensions;
using MMRando.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using MMRando.GameObjects;

namespace MMRando.Models
{
    [DataContract]
    public class ItemLogic
    {
        [DataMember]
        public int ItemId;

        [DataMember]
        public List<int> RequiredItemIds;

        [DataMember]
        public List<List<int>> ConditionalItemIds;

        [DataMember]
        public bool Acquired;

        [DataMember]
        public bool IsFakeItem;

        public ItemLogic(ItemObject itemObject)
        {
            ItemId = itemObject.ID;
            RequiredItemIds = itemObject.DependsOnItems?.Cast<int>().ToList();
            ConditionalItemIds = itemObject.Conditionals?.Select(c => c.Cast<int>().ToList()).ToList();
            IsFakeItem = itemObject.Item.IsFake() && (itemObject.Item.Entrance() == null || !itemObject.IsRandomized);

            // Remove fake requirements
            switch (itemObject.Item)
            {
                case Item.UpgradeBigBombBag:
                case Item.MaskBlast:
                    RequiredItemIds?.Remove((int)Item.TradeItemKafeiLetter);
                    RequiredItemIds?.Remove((int)Item.TradeItemPendant);
                    break;
                case Item.BottleCatchPrincess:
                case Item.BottleCatchBigPoe:
                    RequiredItemIds?.Remove((int)Item.BottleCatchEgg);
                    RequiredItemIds?.Remove((int)Item.BottleCatchBug);
                    RequiredItemIds?.Remove((int)Item.BottleCatchFish);
                    break;
                case Item.BottleCatchEgg:
                    RequiredItemIds?.Remove((int)Item.BottleCatchFish);
                    break;
            }
        }
    }
}
