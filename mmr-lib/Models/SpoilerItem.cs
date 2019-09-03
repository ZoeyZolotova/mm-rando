using MMRando.Extensions;
using MMRando.GameObjects;

namespace MMRando.Models
{
    public class SpoilerItem
    {
        public Item Item { get; private set; }

        public int Id { get; private set; }
        public string Name { get; private set; }

        public int NewLocationId { get; private set; }
        public string NewLocationName { get; private set; }

        public string Region { get; private set; }

        public bool IsJunk { get; private set; }

        public SpoilerItem(ItemObject itemObject)
        {
            Item = itemObject.Item;
            Id = itemObject.ID;
            Name = itemObject.Item.Name() ?? itemObject.Name;
            NewLocationId = (int)itemObject.NewLocation.Value;
            NewLocationName = itemObject.NewLocation.Value.Location();
            Region = itemObject.NewLocation.Value.Region();
            IsJunk = Name.Contains("Rupee") || Name.Contains("Heart");
        }
    }
}
