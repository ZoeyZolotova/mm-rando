using MMR.Randomizer.Attributes;
using MMR.Randomizer.Attributes.Entrance;
using MMR.Randomizer.Models;
using MMR.Randomizer.Models.Settings;

namespace MMR.Randomizer.GameObjects
{
    public enum Item
    {
        // free
        [StartingItem(0xC5CE41, 0x32)]
        [ItemName("Deku Mask"), LocationName("Starting Item"), Region(Region.Misc)]
        [GossipLocationHint("a new file", "a quest's inception"), GossipItemHint("a woodland spirit")]
        [ShopText("Wear it to assume Deku form.")]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [GetItemIndex(0x78), ItemPool(ItemCategory.Masks, LocationCategory.StartingItems)]
        MaskDeku,

        // items
        [Progressive]
        [StartingItem(0xC5CE25, 0x01)]
        [StartingItem(0xC5CE6F, 0x01)]
        [ItemName("Hero's Bow"), LocationName("Hero's Bow Chest"), Region(Region.WoodfallTemple)]
        [GossipLocationHint("Woodfall Temple", "the sleeping temple"), GossipItemHint("a projectile", "a ranged weapon")]
        [ShopText("Use it to shoot arrows.")]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold), Chest(0x02223000 + 0xAA, ChestAttribute.AppearanceType.AppearsClear)]
        [GetItemIndex(0x22), ItemPool(ItemCategory.MainInventory, LocationCategory.BossFights)]
        ItemBow,

        [StartingItem(0xC5CE26, 0x02)]
        [ItemName("Fire Arrow"), LocationName("Fire Arrow Chest"), Region(Region.SnowheadTemple)]
        [GossipLocationHint("Snowhead Temple", "an icy gale"), GossipItemHint("the power of fire", "a magical item")]
        [ShopText("Arm your bow with arrows that burst into flame.")]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold), Chest(0x02336000 + 0xCA, ChestAttribute.AppearanceType.AppearsClear)]
        [GetItemIndex(0x25), ItemPool(ItemCategory.MainInventory, LocationCategory.BossFights)]
        ItemFireArrow,

        [StartingItem(0xC5CE27, 0x03)]
        [ItemName("Ice Arrow"), LocationName("Ice Arrow Chest"), Region(Region.GreatBayTemple)]
        [GossipLocationHint("Great Bay Temple", "the ocean temple"), GossipItemHint("the power of ice", "a magical item")]
        [ShopText("Arm your bow with arrows that freeze.")]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold), Chest(0x0292F000 + 0x11E, ChestAttribute.AppearanceType.AppearsClear)]
        [GetItemIndex(0x26), ItemPool(ItemCategory.MainInventory, LocationCategory.BossFights)]
        ItemIceArrow,

        [StartingItem(0xC5CE28, 0x04)]
        [ItemName("Light Arrow"), LocationName("Light Arrow Chest"), Region(Region.StoneTowerTemple)]
        [GossipLocationHint("Stone Tower Temple", "the cursed temple"), GossipItemHint("the power of light", "a magical item")]
        [ShopText("Arm your bow with arrows infused with sacred light.")]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold), Chest(0x0212B000 + 0xB2, ChestAttribute.AppearanceType.AppearsSwitch, 0x02192000 + 0x8E)]
        [GetItemIndex(0x27), ItemPool(ItemCategory.MainInventory, LocationCategory.BossFights)]
        ItemLightArrow,

        [Progressive]
        [Purchaseable, Visible]
        [StartingItem(0xC5CE2A, 0x06)]
        [StartingItem(0xC5CE6F, 0x08)]
        [ItemName("Bomb Bag"), LocationName("Bomb Bag Purchase"), Region(Region.WestClockTown)]
        [GossipLocationHint("a town shop"), GossipItemHint("an item carrier", "a vessel of explosives")]
        [ShopInventory(ShopInventoryAttribute.ShopKeeper.BombShop, 0)]
        [ShopText("This can hold up to a maximum of 20 bombs.")]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [GetItemIndex(0x1B), ItemPool(ItemCategory.MainInventory, LocationCategory.Purchases)]
        ItemBombBag,

        [Repeatable, Temporary]
        [StartingItemId(0x0A)]
        [ItemName("Magic Bean"), LocationName("Bean Man"), Region(Region.DekuPalace)]
        [GossipLocationHint("a hidden merchant", "a gorging merchant"), GossipItemHint("a plant seed")]
        [ShopText("Plant it in soft soil.")]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [GetItemIndex(0x11E), ItemPool(ItemCategory.MainInventory, LocationCategory.Purchases)]
        ItemMagicBean,

        [Repeatable]
        [StartingItemId(0x0C)]
        [ItemName("Powder Keg"), LocationName("Powder Keg Challenge"), Region(Region.GoronVillage)]
        [GossipLocationHint("a large goron"), GossipItemHint("gunpowder", "a dangerous item", "an explosive barrel")]
        [ShopText("Both its power and its size are immense!")]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [HackContent(nameof(Resources.mods.fix_keg_check))]
        [GetItemIndex(0x123), ItemPool(ItemCategory.MainInventory, LocationCategory.NpcRewards)]
        ItemPowderKeg,

        [StartingItem(0xC5CE31, 0x0D)]
        [ItemName("Pictobox"), LocationName("Koume"), Region(Region.SouthernSwamp)]
        [GossipLocationHint("a witch"), GossipItemHint("a light recorder", "a capture device")]
        [ShopText("Use it to snap pictographs.")]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [GetItemIndex(0x43), ItemPool(ItemCategory.MainInventory, LocationCategory.NpcRewards)]
        ItemPictobox,

        [StartingItem(0xC5CE32, 0x0E)]
        [ItemName("Lens of Truth"), LocationName("Lens of Truth Chest"), Region(Region.GoronVillage)]
        [GossipLocationHint("a lonely peak"), GossipItemHint("eyeglasses", "the truth", "focused vision")]
        [ShopText("Uses magic to see what the naked eye cannot.")]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold), Chest(0x02EB8000 + 0x9A, ChestAttribute.AppearanceType.Normal)]
        [GetItemIndex(0x42), ItemPool(ItemCategory.MainInventory, LocationCategory.Chests)]
        ItemLens,

        [StartingItem(0xC5CE33, 0x0F)]
        [ItemName("Hookshot"), LocationName("Hookshot Chest"), Region(Region.PiratesFortressInterior)]
        [GossipLocationHint("the home of pirates"), GossipItemHint("a chain and grapple")]
        [ShopText("Use it to grapple objects.")]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold), Chest(0x0238B000 + 0x14A, ChestAttribute.AppearanceType.Normal)]
        [GetItemIndex(0x41), ItemPool(ItemCategory.MainInventory, LocationCategory.Chests)]
        ItemHookshot,

        [Progressive]
        [StartingItem(0xC5CDED, 0x30)]
        [StartingItem(0xC5CDF4, 0x01)]
        [ItemName("Magic Power"), LocationName("Town Great Fairy Non-Human"), Region(Region.NorthClockTown)]
        [GossipLocationHint("a magical being"), GossipItemHint("magic power")]
        [ShopText("Grants the ability to use magic.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [GetItemIndex(0x12C), ItemPool(ItemCategory.MagicPowers, LocationCategory.NpcRewards)]
        FairyMagic,
        
        [StartingItemId(0xA6)]
        [ItemName("Spin Attack"), LocationName("Woodfall Great Fairy"), Region(Region.Woodfall)]
        [GossipLocationHint("a magical being"), GossipItemHint("a magic attack"), GossipCompetitiveHint(4, ItemCategory.StrayFairies, false, nameof(GameplaySettings.StrayFairyMode), (int)StrayFairyMode.ChestsOnly, false)]
        [ShopText("Increases the power of your spin attack.", isDefinite: true)]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [GetItemIndex(0x12D), ItemPool(ItemCategory.MagicPowers, LocationCategory.NpcRewards)]
        FairySpinAttack,

        [Progressive]
        [StartingItem(0xC5CDED, 0x60)]
        [StartingItem(0xC5CDF4, 0x01)]
        [StartingItem(0xC5CDF5, 0x01)]
        [ItemName("Extended Magic Power"), LocationName("Snowhead Great Fairy"), Region(Region.Snowhead)]
        [GossipLocationHint("a magical being"), GossipItemHint("magic power"), GossipCompetitiveHint(4, ItemCategory.StrayFairies, false, nameof(GameplaySettings.StrayFairyMode), (int)StrayFairyMode.ChestsOnly, false)]
        [ShopText("Grants the ability to use lots of magic.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [GetItemIndex(0x12E), ItemPool(ItemCategory.MagicPowers, LocationCategory.NpcRewards)]
        FairyDoubleMagic,

        [StartingItem(0xC5CDF6, 0x01)]
        [StartingItem(0xC5CE87, 0x14)]
        [ItemName("Double Defense"), LocationName("Ocean Great Fairy"), Region(Region.ZoraCape)]
        [GossipLocationHint("a magical being"), GossipItemHint("magical defense"), GossipCompetitiveHint(4, ItemCategory.StrayFairies, false, nameof(GameplaySettings.StrayFairyMode), (int)StrayFairyMode.ChestsOnly, false)]
        [ShopText("Take half as much damage from enemies.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [GetItemIndex(0x12F), ItemPool(ItemCategory.MagicPowers, LocationCategory.NpcRewards)]
        FairyDoubleDefense,

        [StartingItem(0xC5CE34, 0x10)]
        [ItemName("Great Fairy's Sword"), LocationName("Ikana Great Fairy"), Region(Region.IkanaCanyon)]
        [GossipLocationHint("a magical being"), GossipItemHint("a black rose", "a powerful blade"), GossipCompetitiveHint(4, ItemCategory.StrayFairies, false, nameof(GameplaySettings.StrayFairyMode), (int)StrayFairyMode.ChestsOnly, false)]
        [ShopText("The most powerful sword has black roses etched in its blade.", isDefinite: true)]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [GetItemIndex(0x130), ItemPool(ItemCategory.MainInventory, LocationCategory.NpcRewards)]
        ItemFairySword,

        [StartingItemId(0x11)]
        [Repeatable, Temporary, Overwritable] // specially handled to turn into Red Potion on subsequent times
        [ItemName("Bottle with Red Potion"), LocationName("Kotake"), Region(Region.SouthernSwamp)]
        [GossipLocationHint("the sleeping witch"), GossipItemHint("a vessel of health", "bottled fortitude")]
        [ShopText("Replenishes your life energy.\u0009\u0001\u0000\u0000 Comes with an Empty Bottle.\u0009\u0002")]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [GetItemIndex(0x59), ItemPool(ItemCategory.MainInventory, LocationCategory.NpcRewards)]
        ItemBottleWitch,

        [StartingItemId(0x18)]
        [Repeatable, Temporary, Overwritable] // specially handled to turn into Milk on subsequent times
        [ItemName("Bottle with Milk"), LocationName("Aliens Defense"), Region(Region.RomaniRanch)]
        [GossipLocationHint("the ranch girl", "a good deed"), GossipItemHint("a dairy product", "the produce of cows")]
        [GossipCombineOrder(0), GossipCombine(MaskRomani, "Ranch Sisters Defense")]
        [ShopText("Recover five hearts with one drink. Contains two helpings.\u0009\u0001\u0000\u0000 Comes with an Empty Bottle.\u0009\u0002")]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [GetItemIndex(0x60), ItemPool(ItemCategory.MainInventory, LocationCategory.NpcRewards)]
        ItemBottleAliens,

        [RupeeRepeatable]
        [StartingItemId(0x22)]
        [Repeatable, Temporary, Overwritable] // specially handled to turn into Gold Dust on subsequent times
        [ItemName("Bottle with Gold Dust"), LocationName("Goron Race"), Region(Region.TwinIslands)]
        [GossipLocationHint("a sporting event"), GossipItemHint("a gleaming powder"), GossipCompetitiveHint(-2)]
        [ShopText("It's very high quality.\u0009\u0001\u0000\u0000 Comes with an Empty Bottle.\u0009\u0002")]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [GetItemIndex(0x6A), ItemPool(ItemCategory.MainInventory, LocationCategory.Minigames)]
        ItemBottleGoronRace,

        [StartingItemId(0x12)]
        [ItemName("Empty Bottle"), LocationName("Beaver Race #1"), Region(Region.ZoraCape)]
        [GossipLocationHint("a river dweller"), GossipItemHint("an empty vessel", "a glass container"), GossipCompetitiveHint(-2)]
        [GossipCombineOrder(0), GossipCombine(HeartPieceBeaverRace, "Beaver Races")]
        [ShopText("Carry various items in this.")]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [GetItemIndex(0x5A), ItemPool(ItemCategory.MainInventory, LocationCategory.Minigames)]
        ItemBottleBeavers,

        [StartingItemId(0x12)]
        [ItemName("Empty Bottle"), LocationName("Dampe Digging"), Region(Region.IkanaGraveyard)]
        [GossipLocationHint("a fearful basement"), GossipItemHint("an empty vessel", "a glass container"), GossipCompetitiveHint]
        [ShopText("Carry various items in this.")]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold), Chest(0x0261E000 + 0x1FE, ChestAttribute.AppearanceType.AppearsSwitch)]
        [GetItemIndex(0x64), ItemPool(ItemCategory.MainInventory, LocationCategory.BossFights)]
        ItemBottleDampe,

        [StartingItemId(0x25)]
        [Repeatable, Temporary, Overwritable] // specially handled to turn into Chateau Romani on subsequent times
        [ItemName("Bottle with Chateau Romani"), LocationName("Madame Aroma in Bar"), Region(Region.EastClockTown)]
        [GossipLocationHint("an important lady"), GossipItemHint("a dairy product", "an adult beverage")]
        [ShopText("Drink it to get lasting stamina for your magic power.\u0009\u0001\u0000\u0000 Comes with an Empty Bottle.\u0009\u0002")]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [GetItemIndex(0x6F), ItemPool(ItemCategory.MainInventory, LocationCategory.NpcRewards)]
        ItemBottleMadameAroma,

        [StartingItem(0xC5CE71, 0x04)]
        [ItemName("Bombers' Notebook"), LocationName("Bombers' Hide and Seek"), Region(Region.NorthClockTown)]
        [GossipLocationHint("a group of children", "a town game"), GossipItemHint("a handy notepad", "a quest logbook")]
        [ShopText("Allows you to keep track of people's schedules.")]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [GetItemIndex(0x50), ItemPool(ItemCategory.MainInventory, LocationCategory.NpcRewards)]
        ItemNotebook,

        //upgrades
        [Progressive]
        [Downgradable]
        [StartingItem(0xC5CE21, 0x02)]
        [StartingItem(0xC5CE00, 0x4E)]
        [ItemName("Razor Sword"), LocationName("Mountain Smithy Day 1"), Region(Region.MountainVillage)]
        [GossipLocationHint("the mountain smith"), GossipItemHint("a sharp blade")]
        [ShopText("A sharp sword forged at the smithy.")]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [GetItemIndex(0x38), ItemPool(ItemCategory.MainInventory, LocationCategory.NpcRewards)]
        UpgradeRazorSword,

        [Progressive]
        [Downgradable]
        [StartingItem(0xC5CE21, 0x03)]
        [StartingItem(0xC5CE00, 0x4F)]
        [ItemName("Gilded Sword"), LocationName("Mountain Smithy Day 2"), Region(Region.MountainVillage)]
        [GossipLocationHint("the mountain smith"), GossipItemHint("a sharp blade")]
        [ShopText("A very sharp sword forged from gold dust.")]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [GetItemIndex(0x39), ItemPool(ItemCategory.MainInventory, LocationCategory.NpcRewards)]
        UpgradeGildedSword,

        [Downgradable]
        [StartingItem(0xC5CE21, 0x20)]
        [ItemName("Mirror Shield"), LocationName("Mirror Shield Chest"), Region(Region.BeneathTheWell)]
        [GossipLocationHint("a hollow ground"), GossipItemHint("a reflective guard", "echoing protection")]
        [ShopText("It can reflect certain rays of light.")]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold), Chest(0x029FE000 + 0x1AA, ChestAttribute.AppearanceType.AppearsSwitch)]
        [GetItemIndex(0x33), ItemPool(ItemCategory.MainInventory, LocationCategory.Chests)]
        UpgradeMirrorShield,

        [RupeeRepeatable]
        [Progressive]
        [Downgradable]
        [StartingItem(0xC5CE25, 0x01)]
        [StartingItem(0xC5CE6F, 0x02)]
        [ItemName("Large Quiver"), LocationName("Town Archery #1"), Region(Region.EastClockTown)]
        [GossipLocationHint("a town activity"), GossipItemHint("a projectile", "a ranged weapon")]
        [GossipCombineOrder(0), GossipCombine(HeartPieceTownArchery, "Town Archery")]
        [ShopText("This can hold up to a maximum of 40 arrows.")]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [GetItemIndex(0x23), ItemPool(ItemCategory.MainInventory, LocationCategory.Minigames)]
        UpgradeBigQuiver,

        [RupeeRepeatable]
        [Progressive]
        [Downgradable]
        [StartingItem(0xC5CE25, 0x01)]
        [StartingItem(0xC5CE6F, 0x03)]
        [ItemName("Largest Quiver"), LocationName("Swamp Archery #1"), Region(Region.RoadToSouthernSwamp)]
        [GossipLocationHint("a swamp game"), GossipItemHint("a projectile", "a ranged weapon")]
        [GossipCombineOrder(0), GossipCombine(HeartPieceSwampArchery, "Swamp Archery")]
        [ShopText("This can hold up to a maximum of 50 arrows.", isDefinite: true)]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [GetItemIndex(0x24), ItemPool(ItemCategory.MainInventory, LocationCategory.Minigames)]
        UpgradeBiggestQuiver,

        [Progressive]
        [Downgradable]
        [Purchaseable, Visible]
        [StartingItem(0xC5CE2A, 0x06)]
        [StartingItem(0xC5CE6F, 0x10)]
        [ItemName("Big Bomb Bag"), LocationName("Big Bomb Bag Purchase"), Region(Region.WestClockTown)]
        [GossipLocationHint("a town shop"), GossipItemHint("an item carrier", "a vessel of explosives")]
        [ShopInventory(ShopInventoryAttribute.ShopKeeper.BombShop, 1)]
        [ShopInventory(ShopInventoryAttribute.ShopKeeper.CuriosityShop, 2)]
        [ShopText("This can hold up to a maximum of 30 bombs.")]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [GetItemIndex(0x1C), ItemPool(ItemCategory.MainInventory, LocationCategory.Purchases)]
        UpgradeBigBombBag,

        [Progressive]
        [Downgradable, Purchaseable]
        [StartingItem(0xC5CE2A, 0x06)]
        [StartingItem(0xC5CE6F, 0x18)]
        [ItemName("Biggest Bomb Bag"), LocationName("Biggest Bomb Bag Purchase"), MultiLocation(UpgradeBiggestBombBagInMountain, UpgradeBiggestBombBagInSwamp)]
        [GossipLocationHint("a northern merchant"), GossipItemHint("an item carrier", "a vessel of explosives")]
        [ShopText("This can hold up to a maximum of 40 bombs.", isDefinite: true)]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [GetItemIndex(0x1D), ItemPool(ItemCategory.MainInventory, LocationCategory.Purchases)]
        UpgradeBiggestBombBag,

        [Progressive]
        [StartingItem(0xC5CE6E, 0x10)]
        [ItemName("Adult Wallet"), LocationName("Bank Reward #1"), Region(Region.WestClockTown)]
        [GossipLocationHint("a keeper of wealth"), GossipItemHint("a coin case", "great wealth")]
        [ShopText("This can hold up to a maximum of 200 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [GetItemIndex(0x08), ItemPool(ItemCategory.MainInventory, LocationCategory.NpcRewards)]
        UpgradeAdultWallet,

        [Progressive]
        [Downgradable]
        [StartingItem(0xC5CE6E, 0x20)]
        [ItemName("Giant Wallet"), LocationName("Ocean Spider House Day 1 Reward"), Region(Region.GreatBayCoast)]
        [GossipLocationHint("a gold spider"), GossipItemHint("a coin case", "great wealth"), GossipCompetitiveHint(0, ItemCategory.SkulltulaTokens, false)]
        [GossipCombineOrder(0), GossipCombine(MundaneItemOceanSpiderHouseDay2PurpleRupee, "Ocean Spider House"), GossipCombine(MundaneItemOceanSpiderHouseDay3RedRupee, "Ocean Spider House")]
        [ShopText("This can hold up to a maximum of 500 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [GetItemIndex(0x09), ItemPool(ItemCategory.MainInventory, LocationCategory.NpcRewards)]
        UpgradeGiantWallet,

        [Progressive]
        [Downgradable]
        [StartingItem(0xC5CE6E, 0x30)]
        [ItemName("Royal Wallet"), LocationName("Removed by Royal Wallet"), Region(Region.Misc)]
        [GossipItemHint("a coin case", "great wealth")]
        [ShopText("This can hold up to a maximum of 999 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [GetItemIndex(0x44D), ItemPool(ItemCategory.RoyalWallet, LocationCategory.Fake)]
        UpgradeRoyalWallet,

        //trades
        [Visible]
        [Repeatable, Temporary, Overwritable]
        [ItemName("Moon's Tear"), LocationName("Astronomy Telescope"), Region(Region.TerminaField)]
        [GossipLocationHint("a falling star"), GossipItemHint("a lunar teardrop", "celestial sadness")]
        [ShopText("A shining stone from the moon.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x96), ItemPool(ItemCategory.TradeItems, LocationCategory.Events)]
        TradeItemMoonTear,

        [Repeatable, Temporary, Overwritable]
        [ItemName("Land Title Deed"), LocationName("Clock Town Scrub Trade"), Region(Region.SouthClockTown)]
        [GossipLocationHint("a town merchant"), GossipItemHint("a property deal")]
        [ShopText("The title deed to the Deku Flower in Clock Town.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x97), ItemPool(ItemCategory.TradeItems, LocationCategory.NpcRewards)]
        TradeItemLandDeed,

        [Repeatable, Temporary, Overwritable]
        [ItemName("Swamp Title Deed"), LocationName("Swamp Scrub Trade"), Region(Region.SouthernSwamp)]
        [GossipLocationHint("a southern merchant"), GossipItemHint("a property deal")]
        [ShopText("The title deed to the Deku Flower in Southern Swamp.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x98), ItemPool(ItemCategory.TradeItems, LocationCategory.NpcRewards)]
        TradeItemSwampDeed,

        [Repeatable, Temporary, Overwritable]
        [ItemName("Mountain Title Deed"), LocationName("Mountain Scrub Trade"), Region(Region.GoronVillage)]
        [GossipLocationHint("a northern merchant"), GossipItemHint("a property deal")]
        [ShopText("The title deed to the Deku Flower near Goron Village.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x99), ItemPool(ItemCategory.TradeItems, LocationCategory.NpcRewards)]
        TradeItemMountainDeed,

        [Repeatable, Temporary, Overwritable]
        [ItemName("Ocean Title Deed"), LocationName("Ocean Scrub Trade"), Region(Region.ZoraHall)]
        [GossipLocationHint("a western merchant"), GossipItemHint("a property deal")]
        [ShopText("The title deed to the Deku Flower in Zora Hall.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x9A), ItemPool(ItemCategory.TradeItems, LocationCategory.NpcRewards)]
        TradeItemOceanDeed,

        [Repeatable, Temporary, Overwritable]
        [ItemName("Room Key"), LocationName("Inn Reservation"), Region(Region.StockPotInn)]
        [GossipLocationHint("checking in", "check-in"), GossipItemHint("a door opener", "a lock opener")]
        [ShopText("With this, you can go in and out of the Stock Pot Inn at night.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0xA0), ItemPool(ItemCategory.TradeItems, LocationCategory.NpcRewards)]
        TradeItemRoomKey,

        [Repeatable, Temporary, Overwritable]
        [ItemName("Letter to Kafei"), LocationName("Midnight Meeting"), Region(Region.StockPotInn)]
        [GossipLocationHint("a late meeting"), GossipItemHint("a lover's plight", "a lover's letter")]
        [ShopText("A love letter from Anju to Kafei.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0xAA), ItemPool(ItemCategory.TradeItems, LocationCategory.NpcRewards)]
        TradeItemKafeiLetter,

        [Repeatable, Temporary, Overwritable]
        [ItemName("Pendant of Memories"), LocationName("Kafei"), Region(Region.LaundryPool)]
        [GossipLocationHint("a posted letter"), GossipItemHint("a cherished necklace", "a symbol of trust")]
        [ShopText("Kafei's symbol of trust for Anju.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0xAB), ItemPool(ItemCategory.TradeItems, LocationCategory.NpcRewards)]
        TradeItemPendant,

        [Repeatable, Temporary, Overwritable]
        [ItemName("Letter to Mama"), LocationName("Curiosity Shop Man #2"), Region(Region.LaundryPool)]
        [GossipLocationHint("a shady gentleman", "a dodgy seller", "a shady dealer"), GossipItemHint("an important note", "a special delivery")]
        [ShopText("It's a parcel for Kafei's mother.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0xA1), ItemPool(ItemCategory.TradeItems, LocationCategory.NpcRewards)]
        TradeItemMamaLetter,

        //notebook hp
        [StartingItem(0xC5CE70, 0x10, true)]
        [ItemName("Piece of Heart"), LocationName("Mayor"), Region(Region.EastClockTown)]
        [GossipLocationHint("a town leader", "an upstanding figure"), GossipItemHint("a segment of health")]
        [ShopText("Collect four to assemble a new Heart Container.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x03), ItemPool(ItemCategory.PiecesOfHeart, LocationCategory.NpcRewards)]
        HeartPieceNotebookMayor,

        [RupeeRepeatable]
        [StartingItem(0xC5CE70, 0x10, true)]
        [ItemName("Piece of Heart"), LocationName("Postman's Game"), Region(Region.WestClockTown)]
        [GossipLocationHint("a hard worker", "a delivery person"), GossipItemHint("a segment of health")]
        [ShopText("Collect four to assemble a new Heart Container.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0xCE), ItemPool(ItemCategory.PiecesOfHeart, LocationCategory.NpcRewards)]
        HeartPieceNotebookPostman,

        [StartingItem(0xC5CE70, 0x10, true)]
        [ItemName("Piece of Heart"), LocationName("Rosa Sisters"), Region(Region.WestClockTown)]
        [GossipLocationHint("traveling sisters", "twin entertainers"), GossipItemHint("a segment of health")]
        [ShopText("Collect four to assemble a new Heart Container.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2B), ItemPool(ItemCategory.PiecesOfHeart, LocationCategory.NpcRewards)]
        HeartPieceNotebookRosa,

        [StartingItem(0xC5CE70, 0x10, true)]
        [ItemName("Piece of Heart"), LocationName("Toilet Hand"), Region(Region.StockPotInn)]
        [GossipLocationHint("a mystery appearance", "a strange palm"), GossipItemHint("a segment of health")]
        [ShopText("Collect four to assemble a new Heart Container.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2C), ItemPool(ItemCategory.PiecesOfHeart, LocationCategory.NpcRewards)]
        HeartPieceNotebookHand,

        [StartingItem(0xC5CE70, 0x10, true)]
        [ItemName("Piece of Heart"), LocationName("Grandma Short Story"), Region(Region.StockPotInn)]
        [GossipLocationHint("an old lady"), GossipItemHint("a segment of health")]
        [ShopText("Collect four to assemble a new Heart Container.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2D), ItemPool(ItemCategory.PiecesOfHeart, LocationCategory.NpcRewards)]
        HeartPieceNotebookGran1,

        [StartingItem(0xC5CE70, 0x10, true)]
        [ItemName("Piece of Heart"), LocationName("Grandma Long Story"), Region(Region.StockPotInn)]
        [GossipLocationHint("an old lady"), GossipItemHint("a segment of health")]
        [ShopText("Collect four to assemble a new Heart Container.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2F), ItemPool(ItemCategory.PiecesOfHeart, LocationCategory.NpcRewards)]
        HeartPieceNotebookGran2,

        //other hp
        [RupeeRepeatable]
        [StartingItem(0xC5CE70, 0x10, true)]
        [ItemName("Piece of Heart"), LocationName("Keaton Quiz"), MultiLocation(HeartPieceKeatonQuizInNCT, HeartPieceKeatonQuizInMilkRoad, HeartPieceKeatonQuizInMountainVillage)]
        [GossipLocationHint("the ghost of a fox", "a mysterious fox"), GossipItemHint("a segment of health")]
        [ShopText("Collect four to assemble a new Heart Container.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x30), ItemPool(ItemCategory.PiecesOfHeart, LocationCategory.NpcRewards)]
        HeartPieceKeatonQuiz,

        [StartingItem(0xC5CE70, 0x10, true)]
        [ItemName("Piece of Heart"), LocationName("Deku Playground Three Days"), Region(Region.NorthClockTown)]
        [GossipLocationHint("a game for scrubs", "a playground", "a town game"), GossipItemHint("a segment of health"), GossipCompetitiveHint]
        [GossipCombineOrder(1), GossipCombine(MundaneItemDekuPlaygroundPurpleRupee, "Deku Playground")]
        [ShopText("Collect four to assemble a new Heart Container.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x31), ItemPool(ItemCategory.PiecesOfHeart, LocationCategory.Minigames)]
        HeartPieceDekuPlayground,

        [RupeeRepeatable]
        [StartingItem(0xC5CE70, 0x10, true)]
        [ItemName("Piece of Heart"), LocationName("Town Archery #2"), Region(Region.EastClockTown)]
        [GossipLocationHint("a town game"), GossipItemHint("a segment of health"), GossipCompetitiveHint(1, nameof(GameplaySettings.DoubleArcheryRewards), false)]
        [GossipCombineOrder(1), GossipCombine(UpgradeBigQuiver, "Town Archery")]
        [ShopText("Collect four to assemble a new Heart Container.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x90), ItemPool(ItemCategory.PiecesOfHeart, LocationCategory.Minigames)]
        HeartPieceTownArchery,

        [StartingItem(0xC5CE70, 0x10, true)]
        [ItemName("Piece of Heart"), LocationName("Honey and Darling Three Days"), Region(Region.EastClockTown)]
        [GossipLocationHint("a town game"), GossipItemHint("a segment of health"), GossipCompetitiveHint(-2)]
        [GossipCombineOrder(1), GossipCombine(MundaneItemHoneyAndDarlingPurpleRupee, "Honey and Darling")]
        [ShopText("Collect four to assemble a new Heart Container.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x94), ItemPool(ItemCategory.PiecesOfHeart, LocationCategory.Minigames)]
        HeartPieceHoneyAndDarling,

        [RupeeRepeatable]
        [StartingItem(0xC5CE70, 0x10, true)]
        [ItemName("Piece of Heart"), LocationName("Swordsman's School"), Region(Region.WestClockTown)]
        [GossipLocationHint("a town game"), GossipItemHint("a segment of health")]
        [ShopText("Collect four to assemble a new Heart Container.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x9F), ItemPool(ItemCategory.PiecesOfHeart, LocationCategory.Minigames)]
        HeartPieceSwordsmanSchool,

        [StartingItem(0xC5CE70, 0x10, true)]
        [ItemName("Piece of Heart"), LocationName("Postbox"), MultiLocation(HeartPiecePostBoxInECT, HeartPiecePostBoxInNCT, HeartPiecePostBoxInSCT)]
        [GossipLocationHint("an information carrier", "a correspondence box"), GossipItemHint("a segment of health")]
        [ShopText("Collect four to assemble a new Heart Container.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0xA2), ItemPool(ItemCategory.PiecesOfHeart, LocationCategory.NpcRewards)]
        HeartPiecePostBox,

        [StartingItem(0xC5CE70, 0x10, true)]
        [ItemName("Piece of Heart"), LocationName("Gossip Stones"), Region(Region.TerminaField)]
        [GossipLocationHint("mysterious stones"), GossipItemHint("a segment of health"), GossipCompetitiveHint(-2)]
        [ShopText("Collect four to assemble a new Heart Container.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0xA3), ItemPool(ItemCategory.PiecesOfHeart, LocationCategory.NpcRewards)]
        HeartPieceTerminaGossipStones,

        [Purchaseable]
        [StartingItem(0xC5CE70, 0x10, true)]
        [ItemName("Piece of Heart"), LocationName("Business Scrub Purchase"), Region(Region.TerminaField)]
        [GossipLocationHint("a hidden merchant"), GossipItemHint("a segment of health")]
        [ShopText("Collect four to assemble a new Heart Container.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0xA5), ItemPool(ItemCategory.PiecesOfHeart, LocationCategory.Purchases)]
        HeartPieceTerminaBusinessScrub,

        [RupeeRepeatable]
        [StartingItem(0xC5CE70, 0x10, true)]
        [ItemName("Piece of Heart"), LocationName("Swamp Archery #2"), Region(Region.RoadToSouthernSwamp)]
        [GossipLocationHint("a swamp game"), GossipItemHint("a segment of health"), GossipCompetitiveHint(1, nameof(GameplaySettings.DoubleArcheryRewards), false)]
        [GossipCombineOrder(1), GossipCombine(UpgradeBiggestQuiver, "Swamp Archery")]
        [ShopText("Collect four to assemble a new Heart Container.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0xA6), ItemPool(ItemCategory.PiecesOfHeart, LocationCategory.Minigames)]
        HeartPieceSwampArchery,

        [StartingItem(0xC5CE70, 0x10, true)]
        [ItemName("Piece of Heart"), LocationName("Pictograph Contest Winner"), Region(Region.SouthernSwamp)]
        [GossipLocationHint("a swamp game"), GossipItemHint("a segment of health")]
        [ShopText("Collect four to assemble a new Heart Container.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0xA7), ItemPool(ItemCategory.PiecesOfHeart, LocationCategory.NpcRewards)]
        HeartPiecePictobox,

        [RupeeRepeatable]
        [StartingItem(0xC5CE70, 0x10, true)]
        [ItemName("Piece of Heart"), LocationName("Boat Archery"), Region(Region.SouthernSwamp)]
        [GossipLocationHint("a swamp game"), GossipItemHint("a segment of health"), GossipCompetitiveHint]
        [ShopText("Collect four to assemble a new Heart Container.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0xA8), ItemPool(ItemCategory.PiecesOfHeart, LocationCategory.Minigames)]
        HeartPieceBoatArchery,

        [StartingItem(0xC5CE70, 0x10, true)]
        [ItemName("Piece of Heart"), LocationName("Frog Choir"), Region(Region.MountainVillage)]
        [GossipLocationHint("a reunion", "a chorus", "an amphibian choir"), GossipItemHint("a segment of health"), GossipCompetitiveHint(3)]
        [ShopText("Collect four to assemble a new Heart Container.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0xAC), ItemPool(ItemCategory.PiecesOfHeart, LocationCategory.NpcRewards)]
        HeartPieceChoir,

        [RupeeRepeatable]
        [StartingItem(0xC5CE70, 0x10, true)]
        [ItemName("Piece of Heart"), LocationName("Beaver Race #2"), Region(Region.ZoraCape)]
        [GossipLocationHint("a river dweller", "a race in the water"), GossipItemHint("a segment of health"), GossipCompetitiveHint(1)]
        [GossipCombineOrder(1), GossipCombine(ItemBottleBeavers, "Beaver Races")]
        [ShopText("Collect four to assemble a new Heart Container.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0xAD), ItemPool(ItemCategory.PiecesOfHeart, LocationCategory.Minigames)]
        HeartPieceBeaverRace,

        [StartingItem(0xC5CE70, 0x10, true)]
        [ItemName("Piece of Heart"), LocationName("Seahorses"), Region(Region.PinnacleRock)]
        [GossipLocationHint("a reunion"), GossipItemHint("a segment of health"), GossipCompetitiveHint(-2)]
        [ShopText("Collect four to assemble a new Heart Container.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0xAE), ItemPool(ItemCategory.PiecesOfHeart, LocationCategory.NpcRewards)]
        HeartPieceSeaHorse,

        [RupeeRepeatable]
        [StartingItem(0xC5CE70, 0x10, true)]
        [ItemName("Piece of Heart"), LocationName("Fisherman Game"), Region(Region.GreatBayCoast), GossipCompetitiveHint(-2)]
        [GossipLocationHint("an ocean game"), GossipItemHint("a segment of health")]
        [ShopText("Collect four to assemble a new Heart Container.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0xAF), ItemPool(ItemCategory.PiecesOfHeart, LocationCategory.Minigames)]
        HeartPieceFishermanGame,

        [StartingItem(0xC5CE70, 0x10, true)]
        [ItemName("Piece of Heart"), LocationName("Evan"), Region(Region.ZoraHall)]
        [GossipLocationHint("a muse", "a composition", "a musician", "plagiarism"), GossipItemHint("a segment of health")]
        [ShopText("Collect four to assemble a new Heart Container.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0xB0), ItemPool(ItemCategory.PiecesOfHeart, LocationCategory.NpcRewards)]
        HeartPieceEvan,

        [RupeeRepeatable]
        [StartingItem(0xC5CE70, 0x10, true)]
        [ItemName("Piece of Heart"), LocationName("Dog Race"), Region(Region.RomaniRanch)]
        [GossipLocationHint("a sporting event"), GossipItemHint("a segment of health")]
        [ShopText("Collect four to assemble a new Heart Container.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0xB1), ItemPool(ItemCategory.PiecesOfHeart, LocationCategory.Minigames)]
        HeartPieceDogRace,

        [RupeeRepeatable]
        [StartingItem(0xC5CE70, 0x10, true)]
        [ItemName("Piece of Heart"), LocationName("Poe Hut"), Region(Region.IkanaCanyon)]
        [GossipLocationHint("a game of ghosts"), GossipItemHint("a segment of health")]
        [ShopText("Collect four to assemble a new Heart Container.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0xB2), ItemPool(ItemCategory.PiecesOfHeart, LocationCategory.BossFights)]
        HeartPiecePoeHut,

        [RupeeRepeatable]
        [StartingItem(0xC5CE70, 0x10, true)]
        [ItemName("Piece of Heart"), LocationName("Treasure Chest Game Goron"), Region(Region.EastClockTown)]
        [GossipLocationHint("a town game"), GossipItemHint("a segment of health")]
        [ShopText("Collect four to assemble a new Heart Container.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), Chest(0x00F43F10 + 0xFAA, ChestAttribute.AppearanceType.AppearsSwitch)]
        [GetItemIndex(0x17), ItemPool(ItemCategory.PiecesOfHeart, LocationCategory.Minigames)]
        HeartPieceTreasureChestGame,

        [StartingItem(0xC5CE70, 0x10, true)]
        [ItemName("Piece of Heart"), LocationName("Peahat Grotto"), Region(Region.TerminaField)]
        [GossipLocationHint("a hollow ground"), GossipItemHint("a segment of health")]
        [ShopText("Collect four to assemble a new Heart Container.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), Chest(0x02ED3000 + 0x76, ChestAttribute.AppearanceType.AppearsClear)]
        [GetItemIndex(0x18), ItemPool(ItemCategory.PiecesOfHeart, LocationCategory.Chests)]
        HeartPiecePeahat,

        [StartingItem(0xC5CE70, 0x10, true)]
        [ItemName("Piece of Heart"), LocationName("Dodongo Grotto"), Region(Region.TerminaField)]
        [GossipLocationHint("a hollow ground"), GossipItemHint("a segment of health")]
        [ShopText("Collect four to assemble a new Heart Container.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), Chest(0x02EBD000 + 0x76, ChestAttribute.AppearanceType.AppearsClear)]
        [GetItemIndex(0x20), ItemPool(ItemCategory.PiecesOfHeart, LocationCategory.Chests)]
        HeartPieceDodong,

        [StartingItem(0xC5CE70, 0x10, true)]
        [ItemName("Piece of Heart"), LocationName("Woodfall Bridge Chest"), Region(Region.Woodfall)]
        [GossipLocationHint("the swamp lands", "an exposed chest"), GossipItemHint("a segment of health")]
        [ShopText("Collect four to assemble a new Heart Container.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), Chest(0x02884000 + 0x252, ChestAttribute.AppearanceType.Normal, 0x02884000 + 0xA52)]
        [GetItemIndex(0x29), ItemPool(ItemCategory.PiecesOfHeart, LocationCategory.Chests)]
        HeartPieceWoodFallChest,

        [StartingItem(0xC5CE70, 0x10, true)]
        [ItemName("Piece of Heart"), LocationName("Twin Islands Underwater Ramp Chest"), Region(Region.TwinIslands)]
        [GossipLocationHint("a spring treasure", "a defrosted land", "a submerged chest"), GossipItemHint("a segment of health")]
        [ShopText("Collect four to assemble a new Heart Container.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), Chest(0x02C23000 + 0x2B6, ChestAttribute.AppearanceType.Normal, 0x02C34000 + 0x19A)]
        [GetItemIndex(0x2E), ItemPool(ItemCategory.PiecesOfHeart, LocationCategory.Chests)]
        HeartPieceTwinIslandsChest,

        [StartingItem(0xC5CE70, 0x10, true)]
        [ItemName("Piece of Heart"), LocationName("Ocean Spider House Chest"), Region(Region.GreatBayCoast)]
        [GossipLocationHint("the strange masks", "coloured faces"), GossipItemHint("a segment of health")]
        [ShopText("Collect four to assemble a new Heart Container.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), Chest(0x024DB000 + 0x76, ChestAttribute.AppearanceType.Normal)]
        [GetItemIndex(0x14), ItemPool(ItemCategory.PiecesOfHeart, LocationCategory.Chests)]
        HeartPieceOceanSpiderHouse,

        [StartingItem(0xC5CE70, 0x10, true)]
        [ItemName("Piece of Heart"), LocationName("Iron Knuckle Chest"), Region(Region.IkanaGraveyard)]
        [GossipLocationHint("a hollow ground"), GossipItemHint("a segment of health")]
        [ShopText("Collect four to assemble a new Heart Container.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), Chest(0x01FAB000 + 0xBA, ChestAttribute.AppearanceType.AppearsClear)]
        [GetItemIndex(0x44), ItemPool(ItemCategory.PiecesOfHeart, LocationCategory.BossFights)]
        HeartPieceKnuckle,

        //mask
        [StartingItem(0xC5CE3C, 0x3E)]
        [ItemName("Postman's Hat"), LocationName("Postman's Freedom Reward"), Region(Region.EastClockTown)]
        [GossipLocationHint("a special delivery", "one last job"), GossipItemHint("a hard worker's hat")]
        [ShopText("You can look into mailboxes when you wear this.")]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [GetItemIndex(0x84), ItemPool(ItemCategory.Masks, LocationCategory.NpcRewards)]
        MaskPostmanHat,

        [Purchaseable, Visible]
        [StartingItem(0xC5CE3D, 0x38)]
        [ItemName("All-Night Mask"), LocationName("All-Night Mask Purchase"), Region(Region.WestClockTown)]
        [GossipLocationHint("a shady gentleman", "a dodgy seller", "a shady dealer"), GossipItemHint("insomnia"), GossipCompetitiveHint(0, nameof(GameplaySettings.UpdateShopAppearance), false)]
        [ShopInventory(ShopInventoryAttribute.ShopKeeper.CuriosityShop, 0)]
        [ShopText("When you wear it you don't get sleepy.")]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [GetItemIndex(0x7E), ItemPool(ItemCategory.Masks, LocationCategory.Purchases)]
        MaskAllNight,

        [StartingItem(0xC5CE3E, 0x47)]
        [ItemName("Blast Mask"), LocationName("Old Lady"), Region(Region.NorthClockTown)]
        [GossipLocationHint("a good deed", "an old lady's struggle"), GossipItemHint("a dangerous mask")]
        [ShopText("Wear it and detonate it...")]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [GetItemIndex(0x8D), ItemPool(ItemCategory.Masks, LocationCategory.NpcRewards)]
        MaskBlast,

        [StartingItem(0xC5CE3F, 0x45)]
        [ItemName("Stone Mask"), LocationName("Invisible Soldier"), Region(Region.RoadToIkana)]
        [GossipLocationHint("a hidden soldier", "a stone circle"), GossipItemHint("inconspicuousness")]
        [ShopText("Become as plain as stone so you can blend into your surroundings.")]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [GetItemIndex(0x8B), ItemPool(ItemCategory.Masks, LocationCategory.NpcRewards)]
        MaskStone,

        [StartingItem(0xC5CE40, 0x40)]
        [ItemName("Great Fairy's Mask"), LocationName("Town Great Fairy"), Region(Region.NorthClockTown)]
        [GossipLocationHint("a magical being"), GossipItemHint("a friend of fairies")]
        [ShopText("The mask's hair will shimmer when you're close to a Stray Fairy.", isDefinite: true)]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [GetItemIndex(0x131), ItemPool(ItemCategory.Masks, LocationCategory.NpcRewards)]
        MaskGreatFairy,

        [StartingItem(0xC5CE42, 0x3A)]
        [ItemName("Keaton Mask"), LocationName("Curiosity Shop Man #1"), Region(Region.LaundryPool)]
        [GossipLocationHint("a shady gentleman", "a dodgy seller", "a shady dealer"), GossipItemHint("a popular mask", "a fox's mask")]
        [ShopText("The mask of the ghost fox, Keaton.")]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [GetItemIndex(0x80), ItemPool(ItemCategory.Masks, LocationCategory.NpcRewards)]
        MaskKeaton,

        [StartingItem(0xC5CE43, 0x46)]
        [ItemName("Bremen Mask"), LocationName("Guru Guru"), Region(Region.LaundryPool)]
        [GossipLocationHint("a musician", "an entertainer"), GossipItemHint("a mask of leadership", "a bird's mask")]
        [ShopText("Wear it so young animals will mistake you for their leader.")]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [GetItemIndex(0x8C), ItemPool(ItemCategory.Masks, LocationCategory.NpcRewards)]
        MaskBremen,

        [StartingItem(0xC5CE44, 0x39)]
        [ItemName("Bunny Hood"), LocationName("Grog"), Region(Region.RomaniRanch)]
        [GossipLocationHint("an ugly but kind heart", "a lover of chickens"), GossipItemHint("the ears of the wild", "a rabbit's hearing")]
        [ShopText("Wear it to be filled with the speed and hearing of the wild.")]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [GetItemIndex(0x7F), ItemPool(ItemCategory.Masks, LocationCategory.NpcRewards)]
        MaskBunnyHood,

        [StartingItem(0xC5CE45, 0x42)]
        [ItemName("Don Gero's Mask"), LocationName("Hungry Goron"), Region(Region.MountainVillage)]
        [GossipLocationHint("a hungry goron", "a person in need"), GossipItemHint("a conductor's mask", "an amphibious mask")]
        [ShopText("When you wear it, you can call the Frog Choir members together.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [GetItemIndex(0x88), ItemPool(ItemCategory.Masks, LocationCategory.NpcRewards)]
        MaskDonGero,

        [RupeeRepeatable]
        [StartingItem(0xC5CE46, 0x48)]
        [ItemName("Mask of Scents"), LocationName("Butler"), Region(Region.DekuPalace)]
        [GossipLocationHint("a servant of royalty", "the royal servant"), GossipItemHint("heightened senses", "a pig's mask"), GossipCompetitiveHint(-1)]
        [ShopText("Wear it to heighten your sense of smell.", isDefinite: true)]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [GetItemIndex(0x8E), ItemPool(ItemCategory.Masks, LocationCategory.NpcRewards)]
        MaskScents,

        [StartingItem(0xC5CE48, 0x3C)]
        [ItemName("Romani's Mask"), LocationName("Cremia"), Region(Region.RomaniRanch)]
        [GossipLocationHint("the ranch lady", "an older sister"), GossipItemHint("proof of membership", "a cow's mask"), GossipCompetitiveHint]
        [GossipCombineOrder(1), GossipCombine(ItemBottleAliens, "Ranch Sisters Defense")]
        [ShopText("Wear it to show you're a member of the Milk Bar, Latte.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [GetItemIndex(0x82), ItemPool(ItemCategory.Masks, LocationCategory.NpcRewards)]
        MaskRomani,

        [StartingItem(0xC5CE49, 0x3D)]
        [ItemName("Circus Leader's Mask"), LocationName("Gorman"), Region(Region.EastClockTown)]
        [GossipLocationHint("an entertainer", "a miserable leader"), GossipItemHint("a mask of sadness")]
        [ShopText("People related to Gorman will react to this.", isDefinite: true)]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [GetItemIndex(0x83), ItemPool(ItemCategory.Masks, LocationCategory.NpcRewards)]
        MaskCircusLeader,

        [StartingItem(0xC5CE4A, 0x37)]
        [ItemName("Kafei's Mask"), LocationName("Madame Aroma in Office"), Region(Region.EastClockTown)]
        [GossipLocationHint("an important lady", "an esteemed woman"), GossipItemHint("the mask of a missing one", "a son's mask")]
        [ShopText("Wear it to inquire about Kafei's whereabouts.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [GetItemIndex(0x8F), ItemPool(ItemCategory.Masks, LocationCategory.NpcRewards)]
        MaskKafei,

        [StartingItem(0xC5CE4B, 0x3F)]
        [ItemName("Couple's Mask"), LocationName("Anju and Kafei"), Region(Region.StockPotInn)]
        [GossipLocationHint("a reunion", "a lovers' reunion"), GossipItemHint("a sign of love", "the mark of a couple"), GossipCompetitiveHint(3)]
        [ShopText("When you wear it, you can soften people's hearts.")]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [GetItemIndex(0x85), ItemPool(ItemCategory.Masks, LocationCategory.NpcRewards)]
        MaskCouple,

        [StartingItem(0xC5CE4C, 0x36)]
        [ItemName("Mask of Truth"), LocationName("Swamp Spider House Reward"), Region(Region.SouthernSwamp)]
        [GossipLocationHint("a gold spider"), GossipItemHint("a piercing gaze"), GossipCompetitiveHint(0, ItemCategory.SkulltulaTokens, false)]
        [ShopText("Wear it to read the thoughts of Gossip Stones and animals.", isDefinite: true)]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [GetItemIndex(0x8A), ItemPool(ItemCategory.Masks, LocationCategory.NpcRewards)]
        MaskTruth,

        [StartingItem(0xC5CE4E, 0x43)]
        [ItemName("Kamaro's Mask"), LocationName("Kamaro"), Region(Region.TerminaField)]
        [GossipLocationHint("a ghostly dancer", "a dancer"), GossipItemHint("dance moves")]
        [ShopText("Wear this to perform a mysterious dance.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [GetItemIndex(0x89), ItemPool(ItemCategory.Masks, LocationCategory.NpcRewards)]
        MaskKamaro,

        [Visible]
        [StartingItem(0xC5CE4F, 0x41)]
        [ItemName("Gibdo Mask"), LocationName("Pamela's Father"), Region(Region.IkanaCanyon)]
        [GossipLocationHint("a healed spirit", "a lost father"), GossipItemHint("a mask of monsters")]
        [ShopText("Even a real Gibdo will mistake you for its own kind.")]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [GetItemIndex(0x87), ItemPool(ItemCategory.Masks, LocationCategory.NpcRewards)]
        MaskGibdo,

        [RupeeRepeatable]
        [StartingItem(0xC5CE50, 0x3B)]
        [ItemName("Garo's Mask"), LocationName("Gorman Bros Race"), Region(Region.MilkRoad)]
        [GossipLocationHint("a sporting event"), GossipItemHint("the mask of spies")]
        [ShopText("This mask can summon the hidden Garo ninjas.")]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [GetItemIndex(0x81), ItemPool(ItemCategory.Masks, LocationCategory.Minigames)]
        MaskGaro,

        [StartingItem(0xC5CE51, 0x44)]
        [ItemName("Captain's Hat"), LocationName("Captain Keeta's Chest"), Region(Region.IkanaGraveyard)]
        [GossipLocationHint("a ghostly battle", "a skeletal leader"), GossipItemHint("a commanding presence")]
        [ShopText("Wear it to pose as Captain Keeta.", isDefinite: true)]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold), Chest(0x0280D000 + 0x392, ChestAttribute.AppearanceType.Normal, 0x0280D000 + 0x6FA)]
        [GetItemIndex(0x7C), ItemPool(ItemCategory.Masks, LocationCategory.BossFights)]
        MaskCaptainHat,

        [StartingItem(0xC5CE52, 0x49)]
        [ItemName("Giant's Mask"), LocationName("Giant's Mask Chest"), Region(Region.StoneTowerTemple)]
        [GossipLocationHint("Stone Tower Temple", "the cursed temple"), GossipItemHint("a growth spurt")]
        [ShopText("If you wear it in a certain room, you'll grow into a giant.")]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold), Chest(0x020F1000 + 0x1C2, ChestAttribute.AppearanceType.AppearsSwitch, 0x02164000 + 0x19E)]
        [GetItemIndex(0x7D), ItemPool(ItemCategory.Masks, LocationCategory.Chests)]
        MaskGiant,

        [Visible]
        [StartingItem(0xC5CE47, 0x33)]
        [ItemName("Goron Mask"), LocationName("Darmani"), Region(Region.MountainVillage)]
        [GossipLocationHint("a healed spirit", "the lost champion"), GossipItemHint("a mountain spirit")]
        [ShopText("Wear it to assume Goron form.")]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [GetItemIndex(0x79), ItemPool(ItemCategory.Masks, LocationCategory.NpcRewards)]
        MaskGoron,

        [Visible]
        [StartingItem(0xC5CE4D, 0x34)]
        [ItemName("Zora Mask"), LocationName("Mikau"), Region(Region.GreatBayCoast)]
        [GossipLocationHint("a healed spirit", "a fallen guitarist"), GossipItemHint("an ocean spirit")]
        [ShopText("Wear it to assume Zora form.")]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [GetItemIndex(0x7A), ItemPool(ItemCategory.Masks, LocationCategory.NpcRewards)]
        MaskZora,

        //song
        [Visible]
        [StartingItem(0xC5CE24, 0x00)]
        [ItemName("Ocarina of Time"), LocationName("Skull Kid"), Region(Region.ClockTowerRoof)]
        [GossipLocationHint("a stolen possession"), GossipItemHint("a musical instrument")]
        [ShopText("This musical instrument is filled with memories of Princess Zelda.")]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [GetItemIndex(0x4C), ItemPool(ItemCategory.TimeTravel, LocationCategory.BossFights)]
        ItemOcarina,

        [StartingItem(0xC5CE72, 0x10)]
        [ItemName("Song of Time"), LocationName("Skull Kid Song"), Region(Region.ClockTowerRoof)]
        [GossipLocationHint("a distant memory"), GossipItemHint("a forgotten melody")]
        [ShopText("This melody is a song of memories of Princess Zelda.", isDefinite: true)]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [GetItemIndex(0x44C), ItemPool(ItemCategory.TimeTravel, LocationCategory.BossFights)]
        SongTime,

        [StartingItem(0xC5CE72, 0x20)]
        [ItemName("Song of Healing"), LocationName("Starting Song"), Region(Region.Misc)]
        [GossipLocationHint("a new file", "a quest's inception"), GossipItemHint("a soothing melody")]
        [ShopText("This melody will soothe restless spirits.", isDefinite: true)]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [GetItemIndex(0x124), ItemPool(ItemCategory.Songs, LocationCategory.StartingItems)]
        SongHealing,

        [StartingItem(0xC5CE72, 0x80)]
        [ItemName("Song of Soaring"), LocationName("Swamp Music Statue"), Region(Region.SouthernSwamp)]
        [GossipLocationHint("a stone tablet"), GossipItemHint("white wings")]
        [ShopText("This melody sends you to a stone bird statue in an instant.", isDefinite: true)]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [GetItemIndex(0x70), ItemPool(ItemCategory.SongOfSoaring, LocationCategory.NpcRewards)]
        SongSoaring,

        [RupeeRepeatable]
        [StartingItem(0xC5CE72, 0x40)]
        [ItemName("Epona's Song"), LocationName("Romani's Game"), Region(Region.RomaniRanch)]
        [GossipLocationHint("a reunion"), GossipItemHint("a horse's song", "a song of the field")]
        [ShopText("This melody calls your horse, Epona.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [GetItemIndex(0x71), ItemPool(ItemCategory.Songs, LocationCategory.Minigames)]
        SongEpona,

        [StartingItem(0xC5CE71, 0x01)]
        [ItemName("Song of Storms"), LocationName("Day 1 Grave Tablet"), Region(Region.IkanaGraveyard)]
        [GossipLocationHint("a hollow ground", "a stone tablet"), GossipItemHint("rain and thunder", "stormy weather")]
        [ShopText("This melody is the turbulent tune that blows curses away.", isDefinite: true)]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [GetItemIndex(0x72), ItemPool(ItemCategory.Songs, LocationCategory.BossFights)]
        SongStorms,

        [StartingItem(0xC5CE73, 0x40)]
        [ItemName("Sonata of Awakening"), LocationName("Imprisoned Monkey"), Region(Region.DekuPalace)]
        [GossipLocationHint("a prisoner", "a false imprisonment"), GossipItemHint("a royal song", "an awakening melody")]
        [ShopText("This melody awakens those who have fallen into a deep sleep.", isDefinite: true)]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [GetItemIndex(0x73), ItemPool(ItemCategory.Songs, LocationCategory.NpcRewards)]
        SongSonata,

        [StartingItem(0xC5CE73, 0x80)]
        [ItemName("Goron Lullaby"), LocationName("Baby Goron"), Region(Region.GoronVillage)]
        [GossipLocationHint("a lonely child", "an elder's son"), GossipItemHint("a sleepy melody", "a father's lullaby")]
        [ShopText("This melody blankets listeners in calm while making eyelids grow heavy.", isDefinite: true)]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [GetItemIndex(0x74), ItemPool(ItemCategory.Songs, LocationCategory.NpcRewards)]
        SongLullaby,

        [StartingItem(0xC5CE72, 0x01)]
        [ItemName("New Wave Bossa Nova"), LocationName("Baby Zoras"), Region(Region.GreatBayCoast)]
        [GossipLocationHint("the lost children", "the pirates' loot"), GossipItemHint("an ocean roar", "a song of newborns"), GossipCompetitiveHint(2, nameof(GameplaySettings.AddSongs), true)]
        [ShopText("It's the melody taught by the Zora children that invigorates singing voices.", isDefinite: true)]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [GetItemIndex(0x75), ItemPool(ItemCategory.Songs, LocationCategory.NpcRewards)]
        SongNewWaveBossaNova,

        [StartingItem(0xC5CE72, 0x02)]
        [ItemName("Elegy of Emptiness"), LocationName("Ikana King"), Region(Region.IkanaCastle)]
        [GossipLocationHint("a fallen king", "a battle in darkness"), GossipItemHint("empty shells", "skin shedding")]
        [ShopText("It's a mystical song that allows you to shed a shell shaped in your current image.", isDefinite: true)]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [GetItemIndex(0x1CB), ItemPool(ItemCategory.Songs, LocationCategory.BossFights)] // 0x76 is a special value used for ice traps in chests
        SongElegy,

        [Visible]
        [StartingItem(0xC5CE72, 0x04)]
        [ItemName("Oath to Order"), LocationName("Boss Blue Warp"), MultiLocation(SongOathInWFT, SongOathInSHT, SongOathInGBT, SongOathInISTT)]
        [GossipLocationHint("cleansed evil", "a fallen evil"), GossipItemHint("a song of summoning", "a song of giants")]
        [ShopText("This melody will call the giants at the right moment.", isDefinite: true)]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [GetItemIndex(0x77), ItemPool(ItemCategory.Songs, LocationCategory.BossFights)]
        SongOath,

        //areas/other
        AreaSouthAccess,

        [EntranceName("Woodfall")]
        [DungeonEntrance(DungeonEntrance.EntranceWoodfallTempleFromWoodfall, DungeonEntrance.EntranceWoodfallFromWoodfallTempleEntrance)]
        AreaWoodFallTempleAccess,

        [DungeonEntrance(DungeonEntrance.EntranceWoodfallTemplePrisonFromOdolwasLair)]
        AreaWoodFallTempleClear,
        AreaNorthAccess,

        [EntranceName("Snowhead")]
        [DungeonEntrance(DungeonEntrance.EntranceSnowheadTempleFromSnowhead, DungeonEntrance.EntranceSnowheadFromSnowheadTemple)]
        AreaSnowheadTempleAccess,

        [DungeonEntrance(DungeonEntrance.EntranceMountainVillageFromSnowheadClear)]
        AreaSnowheadTempleClear,
        OtherEpona,
        AreaWestAccess,
        AreaPiratesFortressAccess,

        [EntranceName("Great Bay")]
        [DungeonEntrance(DungeonEntrance.EntranceGreatBayTempleFromZoraCape, DungeonEntrance.EntranceZoraCapeFromGreatBayTemple)]
        AreaGreatBayTempleAccess,

        [DungeonEntrance(DungeonEntrance.EntranceZoraCapeFromGreatBayTempleClear)]
        AreaGreatBayTempleClear,
        AreaEastAccess,
        AreaIkanaCanyonAccess,
        AreaStoneTowerTempleAccess,

        [EntranceName("Inverted Stone Tower")]
        [DungeonEntrance(DungeonEntrance.EntranceStoneTowerTempleInvertedFromStoneTowerInverted, DungeonEntrance.EntranceStoneTowerInvertedFromStoneTowerTempleInverted)]
        AreaInvertedStoneTowerTempleAccess,

        [DungeonEntrance(DungeonEntrance.EntranceIkanaCanyonFromIkanaClear)]
        AreaStoneTowerClear,
        OtherExplosive,
        OtherArrow,
        OtherMagicBean,
        OtherLimitlessBeans,
        OtherPlayDekuPlayground,
        OtherTimeTravel,
        AreaWoodfallNew,
        AreaSnowheadNew,
        AreaGreatBayNew,
        AreaLANew, // ??
        AreaInvertedStoneTowerNew, // Seemingly not used

        //keysanity items
        [StartingItem(0xC5CE74, 0x04)]
        [ItemName("Woodfall Map"), LocationName("Woodfall Map Chest"), Region(Region.WoodfallTemple)]
        [GossipLocationHint("Woodfall Temple", "the sleeping temple"), GossipItemHint("a navigation aid", "a paper guide")]
        [ShopText("The Dungeon Map for Woodfall Temple.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), Chest(0x0221F000 + 0x12A, ChestAttribute.AppearanceType.AppearsClear)]
        [GetItemIndex(0x3E), ItemPool(ItemCategory.Navigation, LocationCategory.Chests)]
        ItemWoodfallMap,

        [StartingItem(0xC5CE74, 0x02)]
        [ItemName("Woodfall Compass"), LocationName("Woodfall Compass Chest"), Region(Region.WoodfallTemple)]
        [GossipLocationHint("Woodfall Temple", "the sleeping temple"), GossipItemHint("a navigation aid", "a magnetic needle")]
        [ShopText("The Compass for Woodfall Temple.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), Chest(0x02215000 + 0xFA, ChestAttribute.AppearanceType.AppearsClear)]
        [GetItemIndex(0x3F), ItemPool(ItemCategory.Navigation, LocationCategory.Chests)]
        ItemWoodfallCompass,

        [Repeatable, Temporary(nameof(GameplaySettings.BossKeyMode), (int)SmallKeyMode.KeepThroughTime, false)]
        [ItemName("Woodfall Boss Key"), LocationName("Woodfall Boss Key Chest"), Region(Region.WoodfallTemple)]
        [GossipLocationHint("Woodfall Temple", "the sleeping temple"), GossipItemHint("an important key", "entry to evil's lair")]
        [ShopText("The key for the boss room in Woodfall Temple.")]
        [ChestType(ChestTypeAttribute.ChestType.BossKey), Chest(0x02227000 + 0x11A, ChestAttribute.AppearanceType.Normal)]
        [GetItemIndex(0x3D), ItemPool(ItemCategory.DungeonKeys, LocationCategory.BossFights)]
        ItemWoodfallBossKey,

        [Repeatable, Temporary(nameof(GameplaySettings.SmallKeyMode), (int)SmallKeyMode.KeepThroughTime, false), Returnable(nameof(GameplaySettings.SmallKeyMode), (int)SmallKeyMode.KeepThroughTime, true)]
        [ItemName("Woodfall Small Key"), LocationName("Woodfall Small Key Chest"), Region(Region.WoodfallTemple)]
        [GossipLocationHint("Woodfall Temple", "the sleeping temple"), GossipItemHint("access to a locked door", "a useful key")]
        [ShopText("A small key for use in Woodfall Temple.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold), Chest(0x02218000 + 0x1CA, ChestAttribute.AppearanceType.Normal)]
        [GetItemIndex(0x3C), ItemPool(ItemCategory.DungeonKeys, LocationCategory.Chests)]
        ItemWoodfallKey1,

        [StartingItem(0xC5CE75, 0x04)]
        [ItemName("Snowhead Map"), LocationName("Snowhead Map Chest"), Region(Region.SnowheadTemple)]
        [GossipLocationHint("Snowhead Temple", "an icy gale"), GossipItemHint("a navigation aid", "a paper guide")]
        [ShopText("The Dungeon Map for Snowhead Temple.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), Chest(0x02346000 + 0x13A, ChestAttribute.AppearanceType.Normal)]
        [GetItemIndex(0x54), ItemPool(ItemCategory.Navigation, LocationCategory.Chests)]
        ItemSnowheadMap,

        [StartingItem(0xC5CE75, 0x02)]
        [ItemName("Snowhead Compass"), LocationName("Snowhead Compass Chest"), Region(Region.SnowheadTemple)]
        [GossipLocationHint("Snowhead Temple", "an icy gale"), GossipItemHint("a navigation aid", "a magnetic needle")]
        [ShopText("The Compass for Snowhead Temple.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), Chest(0x022F2000 + 0x1BA, ChestAttribute.AppearanceType.Normal)]
        [GetItemIndex(0x57), ItemPool(ItemCategory.Navigation, LocationCategory.Chests)]
        ItemSnowheadCompass,

        [Repeatable, Temporary(nameof(GameplaySettings.BossKeyMode), (int)SmallKeyMode.KeepThroughTime, false)]
        [ItemName("Snowhead Boss Key"), LocationName("Snowhead Boss Key Chest"), Region(Region.SnowheadTemple)]
        [GossipLocationHint("Snowhead Temple", "an icy gale"), GossipItemHint("an important key", "entry to evil's lair")]
        [ShopText("The key for the boss room in Snowhead Temple.")]
        [ChestType(ChestTypeAttribute.ChestType.BossKey), Chest(0x0230C000 + 0x57A, ChestAttribute.AppearanceType.Normal)]
        [GetItemIndex(0x4E), ItemPool(ItemCategory.DungeonKeys, LocationCategory.BossFights)]
        ItemSnowheadBossKey,

        [Repeatable, Temporary(nameof(GameplaySettings.SmallKeyMode), (int)SmallKeyMode.KeepThroughTime, false), Returnable(nameof(GameplaySettings.SmallKeyMode), (int)SmallKeyMode.KeepThroughTime, true)]
        [ItemName("Snowhead Small Key"), LocationName("Snowhead Block Room Chest"), Region(Region.SnowheadTemple)]
        [GossipLocationHint("Snowhead Temple", "an icy gale"), GossipItemHint("access to a locked door", "a useful key")]
        [ShopText("A small key for use in Snowhead Temple.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold), Chest(0x02306000 + 0x12A, ChestAttribute.AppearanceType.Normal)]
        [GetItemIndex(0x46), ItemPool(ItemCategory.DungeonKeys, LocationCategory.Chests)]
        ItemSnowheadKey1,

        [Repeatable, Temporary(nameof(GameplaySettings.SmallKeyMode), (int)SmallKeyMode.KeepThroughTime, false), Returnable(nameof(GameplaySettings.SmallKeyMode), (int)SmallKeyMode.KeepThroughTime, true)]
        [ItemName("Snowhead Small Key"), LocationName("Snowhead Icicle Room Chest"), Region(Region.SnowheadTemple)]
        [GossipLocationHint("Snowhead Temple", "an icy gale"), GossipItemHint("access to a locked door", "a useful key")]
        [ShopText("A small key for use in Snowhead Temple.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold), Chest(0x0233A000 + 0x23A, ChestAttribute.AppearanceType.Normal)]
        [GetItemIndex(0x47), ItemPool(ItemCategory.DungeonKeys, LocationCategory.Chests)]
        ItemSnowheadKey2,

        [Repeatable, Temporary(nameof(GameplaySettings.SmallKeyMode), (int)SmallKeyMode.KeepThroughTime, false), Returnable(nameof(GameplaySettings.SmallKeyMode), (int)SmallKeyMode.KeepThroughTime, true)]
        [ItemName("Snowhead Small Key"), LocationName("Snowhead Bridge Room Chest"), Region(Region.SnowheadTemple)]
        [GossipLocationHint("Snowhead Temple", "an icy gale"), GossipItemHint("access to a locked door", "a useful key")]
        [ShopText("A small key for use in Snowhead Temple.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold), Chest(0x022F9000 + 0x19A, ChestAttribute.AppearanceType.AppearsClear)]
        [GetItemIndex(0x48), ItemPool(ItemCategory.DungeonKeys, LocationCategory.Chests)]
        ItemSnowheadKey3,

        [StartingItem(0xC5CE76, 0x04)]
        [ItemName("Great Bay Map"), LocationName("Great Bay Map Chest"), Region(Region.GreatBayTemple)]
        [GossipLocationHint("Great Bay Temple", "the ocean temple"), GossipItemHint("a navigation aid", "a paper guide")]
        [ShopText("The Dungeon Map for Great Bay Temple.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), Chest(0x02905000 + 0x19A, ChestAttribute.AppearanceType.Normal)]
        [GetItemIndex(0x55), ItemPool(ItemCategory.Navigation, LocationCategory.Chests)]
        ItemGreatBayMap,

        [StartingItem(0xC5CE76, 0x02)]
        [ItemName("Great Bay Compass"), LocationName("Great Bay Compass Chest"), Region(Region.GreatBayTemple)]
        [GossipLocationHint("Great Bay Temple", "the ocean temple"), GossipItemHint("a navigation aid", "a magnetic needle")]
        [ShopText("The Compass for Great Bay Temple.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), Chest(0x02914000 + 0x21A, ChestAttribute.AppearanceType.Normal)]
        [GetItemIndex(0x58), ItemPool(ItemCategory.Navigation, LocationCategory.Chests)]
        ItemGreatBayCompass,

        [Repeatable, Temporary(nameof(GameplaySettings.BossKeyMode), (int)SmallKeyMode.KeepThroughTime, false)]
        [ItemName("Great Bay Boss Key"), LocationName("Great Bay Boss Key Chest"), Region(Region.GreatBayTemple)]
        [GossipLocationHint("Great Bay Temple", "the ocean temple"), GossipItemHint("an important key", "entry to evil's lair")]
        [ShopText("The key for the boss room in Great Bay Temple.")]
        [ChestType(ChestTypeAttribute.ChestType.BossKey), Chest(0x02914000 + 0x1FA, ChestAttribute.AppearanceType.Normal)]
        [GetItemIndex(0x4F), ItemPool(ItemCategory.DungeonKeys, LocationCategory.BossFights)]
        ItemGreatBayBossKey,

        [Repeatable, Temporary(nameof(GameplaySettings.SmallKeyMode), (int)SmallKeyMode.KeepThroughTime, false), Returnable(nameof(GameplaySettings.SmallKeyMode), (int)SmallKeyMode.KeepThroughTime, true)]
        [ItemName("Great Bay Small Key"), LocationName("Great Bay Small Key Chest"), Region(Region.GreatBayTemple)]
        [GossipLocationHint("Great Bay Temple", "the ocean temple"), GossipItemHint("access to a locked door", "a useful key")]
        [ShopText("A small key for use in Great Bay Temple.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold), Chest(0x02914000 + 0x20A, ChestAttribute.AppearanceType.Normal)]
        [GetItemIndex(0x40), ItemPool(ItemCategory.DungeonKeys, LocationCategory.Chests)]
        ItemGreatBayKey1,

        [StartingItem(0xC5CE77, 0x04)]
        [ItemName("Stone Tower Map"), LocationName("Stone Tower Map Chest"), Region(Region.StoneTowerTemple)]
        [GossipLocationHint("Stone Tower Temple", "the cursed temple"), GossipItemHint("a navigation aid", "a paper guide")]
        [ShopText("The Dungeon Map for Stone Tower Temple.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), Chest(0x0210F000 + 0x222, ChestAttribute.AppearanceType.Normal, 0x02182000 + 0x21E)]
        [GetItemIndex(0x56), ItemPool(ItemCategory.Navigation, LocationCategory.Chests)]
        ItemStoneTowerMap,

        [StartingItem(0xC5CE77, 0x02)]
        [ItemName("Stone Tower Compass"), LocationName("Stone Tower Compass Chest"), Region(Region.StoneTowerTemple)]
        [GossipLocationHint("Stone Tower Temple", "the cursed temple"), GossipItemHint("a navigation aid", "a magnetic needle")]
        [ShopText("The Compass for Stone Tower Temple.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), Chest(0x02104000 + 0x292, ChestAttribute.AppearanceType.Normal, 0x02177000 + 0x2DE)]
        [GetItemIndex(0x6C), ItemPool(ItemCategory.Navigation, LocationCategory.Chests)]
        ItemStoneTowerCompass,

        [Repeatable, Temporary(nameof(GameplaySettings.BossKeyMode), (int)SmallKeyMode.KeepThroughTime, false)]
        [ItemName("Stone Tower Boss Key"), LocationName("Stone Tower Boss Key Chest"), Region(Region.StoneTowerTemple)]
        [GossipLocationHint("Stone Tower Temple", "the cursed temple"), GossipItemHint("an important key", "entry to evil's lair")]
        [ShopText("The key for the boss room in Stone Tower Temple.")]
        [ChestType(ChestTypeAttribute.ChestType.BossKey), Chest(0x02130000 + 0x82, ChestAttribute.AppearanceType.Normal, 0x02198000 + 0xCE)]
        [GetItemIndex(0x53), ItemPool(ItemCategory.DungeonKeys, LocationCategory.BossFights)]
        ItemStoneTowerBossKey,

        [Repeatable, Temporary(nameof(GameplaySettings.SmallKeyMode), (int)SmallKeyMode.KeepThroughTime, false), Returnable(nameof(GameplaySettings.SmallKeyMode), (int)SmallKeyMode.KeepThroughTime, true)]
        [ItemName("Stone Tower Small Key"), LocationName("Stone Tower Armos Room Chest"), Region(Region.StoneTowerTemple)]
        [GossipLocationHint("Stone Tower Temple", "the cursed temple"), GossipItemHint("access to a locked door", "a useful key")]
        [ShopText("A small key for use in Stone Tower Temple.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold), Chest(0x0210F000 + 0x202, ChestAttribute.AppearanceType.AppearsSwitch, 0x02182000 + 0x1FE)]
        [GetItemIndex(0x49), ItemPool(ItemCategory.DungeonKeys, LocationCategory.Chests)]
        ItemStoneTowerKey1,

        [Repeatable, Temporary(nameof(GameplaySettings.SmallKeyMode), (int)SmallKeyMode.KeepThroughTime, false), Returnable(nameof(GameplaySettings.SmallKeyMode), (int)SmallKeyMode.KeepThroughTime, true)]
        [ItemName("Stone Tower Small Key"), LocationName("Stone Tower Eyegore Room Chest"), Region(Region.StoneTowerTemple)]
        [GossipLocationHint("Stone Tower Temple", "the cursed temple"), GossipItemHint("access to a locked door", "a useful key")]
        [ShopText("A small key for use in Stone Tower Temple.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold), Chest(0x020F1000 + 0x1D2, ChestAttribute.AppearanceType.Normal, 0x02164000 + 0x1AE)]
        [GetItemIndex(0x4A), ItemPool(ItemCategory.DungeonKeys, LocationCategory.Chests)]
        ItemStoneTowerKey2,

        [Repeatable, Temporary(nameof(GameplaySettings.SmallKeyMode), (int)SmallKeyMode.KeepThroughTime, false), Returnable(nameof(GameplaySettings.SmallKeyMode), (int)SmallKeyMode.KeepThroughTime, true)]
        [ItemName("Stone Tower Small Key"), LocationName("Stone Tower Updraft Room Chest"), Region(Region.StoneTowerTemple)]
        [GossipLocationHint("Stone Tower Temple", "the cursed temple"), GossipItemHint("access to a locked door", "a useful key")]
        [ShopText("A small key for use in Stone Tower Temple.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold), Chest(0x02104000 + 0x282, ChestAttribute.AppearanceType.AppearsSwitch, 0x02177000 + 0x2CE)]
        [GetItemIndex(0x4B), ItemPool(ItemCategory.DungeonKeys, LocationCategory.Chests)]
        ItemStoneTowerKey3,

        [Repeatable, Temporary(nameof(GameplaySettings.SmallKeyMode), (int)SmallKeyMode.KeepThroughTime, false), Returnable(nameof(GameplaySettings.SmallKeyMode), (int)SmallKeyMode.KeepThroughTime, true)]
        [ItemName("Stone Tower Small Key"), LocationName("Stone Tower Death Armos Maze Chest"), Region(Region.StoneTowerTemple)]
        [GossipLocationHint("Stone Tower Temple", "the cursed temple"), GossipItemHint("access to a locked door", "a useful key")]
        [ShopText("A small key for use in Stone Tower Temple.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold), Chest(0x020FC000 + 0x252, ChestAttribute.AppearanceType.Normal, 0x0216E000 + 0x1CE)]
        [GetItemIndex(0x4D), ItemPool(ItemCategory.DungeonKeys, LocationCategory.Chests)]
        ItemStoneTowerKey4,

        //shop items
        [Purchaseable, Visible]
        [Repeatable, Temporary, Overwritable]
        [ItemName("Red Potion"), LocationName("Trading Post Red Potion"), Region(Region.WestClockTown)]
        [GossipLocationHint("a town merchant", "a convenience store", "a market"), GossipItemHint("consumable strength", "a hearty drink", "a red drink")]
        [ShopInventory(ShopInventoryAttribute.ShopKeeper.TradingPostMain, 7)]
        [ShopInventory(ShopInventoryAttribute.ShopKeeper.TradingPostPartTimer, 7)]
        [ShopText("Replenishes your life energy.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0xCD), ItemPool(ItemCategory.RedPotions, LocationCategory.Purchases)]
        ShopItemTradingPostRedPotion,

        [Purchaseable, Visible]
        [Repeatable, Temporary, Overwritable]
        [ItemName("Green Potion"), LocationName("Trading Post Green Potion"), Region(Region.WestClockTown)]
        [GossipLocationHint("a town merchant", "a convenience store", "a market"), GossipItemHint("a magic potion", "a green drink")]
        [ShopInventory(ShopInventoryAttribute.ShopKeeper.TradingPostMain, 2)]
        [ShopInventory(ShopInventoryAttribute.ShopKeeper.TradingPostPartTimer, 3)]
        [ShopText("Replenishes your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0xBB), ItemPool(ItemCategory.GreenPotions, LocationCategory.Purchases)]
        ShopItemTradingPostGreenPotion,

        [Purchaseable, Visible]
        [Repeatable]
        [ItemName("Hero's Shield"), LocationName("Trading Post Hero's Shield"), Region(Region.WestClockTown)]
        [GossipLocationHint("a town merchant", "a convenience store", "a market"), GossipItemHint("a basic guard", "protection")]
        [ShopInventory(ShopInventoryAttribute.ShopKeeper.TradingPostMain, 3)]
        [ShopInventory(ShopInventoryAttribute.ShopKeeper.TradingPostPartTimer, 6)]
        [ShopText("Use it to defend yourself.")]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [GetItemIndex(0xBC), ItemPool(ItemCategory.Shields, LocationCategory.Purchases)]
        ShopItemTradingPostShield,

        [Purchaseable, Visible]
        [Repeatable, Temporary, Overwritable]
        [ItemName("Fairy"), LocationName("Trading Post Fairy"), Region(Region.WestClockTown)]
        [GossipLocationHint("a town merchant", "a convenience store", "a market"), GossipItemHint("a winged friend", "a healer")]
        [ShopInventory(ShopInventoryAttribute.ShopKeeper.TradingPostMain, 0)]
        [ShopInventory(ShopInventoryAttribute.ShopKeeper.TradingPostPartTimer, 0)]
        [ShopText("Recovers life energy. If you run out of life energy you'll automatically use this.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0xBD), ItemPool(ItemCategory.Fairy, LocationCategory.Purchases)]
        ShopItemTradingPostFairy,

        [Purchaseable, Visible]
        [Repeatable, Temporary]
        [ItemName("Deku Stick"), LocationName("Trading Post Deku Stick"), Region(Region.WestClockTown)]
        [GossipLocationHint("a town merchant", "a convenience store", "a market"), GossipItemHint("a flammable weapon", "a flimsy weapon")]
        [ShopInventory(ShopInventoryAttribute.ShopKeeper.TradingPostMain, 4)]
        [ShopInventory(ShopInventoryAttribute.ShopKeeper.TradingPostPartTimer, 5)]
        [ShopText("Deku Sticks burn well. You can only carry 10.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0xBE), ItemPool(ItemCategory.DekuSticks, LocationCategory.Purchases)]
        ShopItemTradingPostStick,

        [Purchaseable, Visible]
        [Repeatable, Temporary]
        [ItemName("30 Arrows"), LocationName("Trading Post 30 Arrows"), Region(Region.WestClockTown)]
        [GossipLocationHint("a town merchant", "a convenience store", "a market"), GossipItemHint("a quiver refill", "a bundle of projectiles")]
        [ShopInventory(ShopInventoryAttribute.ShopKeeper.TradingPostMain, 5)]
        [ShopInventory(ShopInventoryAttribute.ShopKeeper.TradingPostPartTimer, 1)]
        [ShopText("Ammo for your bow.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0xBF), ItemPool(ItemCategory.Arrows, LocationCategory.Purchases)]
        ShopItemTradingPostArrow30,

        [Purchaseable, Visible]
        [Repeatable, Temporary]
        [ItemName("10 Deku Nuts"), LocationName("Trading Post 10 Deku Nuts"), Region(Region.WestClockTown)]
        [GossipLocationHint("a town merchant", "a convenience store", "a market"), GossipItemHint("a flashing impact")]
        [ShopInventory(ShopInventoryAttribute.ShopKeeper.TradingPostMain, 6)]
        [ShopInventory(ShopInventoryAttribute.ShopKeeper.TradingPostPartTimer, 4)]
        [ShopText("Its flash blinds enemies.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0xC0), ItemPool(ItemCategory.DekuNuts, LocationCategory.Purchases)]
        ShopItemTradingPostNut10,

        [Purchaseable, Visible]
        [Repeatable, Temporary]
        [ItemName("50 Arrows"), LocationName("Trading Post 50 Arrows"), Region(Region.WestClockTown)]
        [GossipLocationHint("a town merchant", "a convenience store", "a market"), GossipItemHint("a quiver refill", "a bundle of projectiles")]
        [ShopInventory(ShopInventoryAttribute.ShopKeeper.TradingPostMain, 1)]
        [ShopInventory(ShopInventoryAttribute.ShopKeeper.TradingPostPartTimer, 2)]
        [ShopText("Ammo for your bow.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0xC1), ItemPool(ItemCategory.Arrows, LocationCategory.Purchases)]
        ShopItemTradingPostArrow50,

        [Purchaseable, Visible]
        [Repeatable, Temporary, Overwritable]
        [ItemName("Blue Potion"), LocationName("Witch Shop Blue Potion"), Region(Region.SouthernSwamp)]
        [GossipLocationHint("a sleeping witch", "a southern merchant"), GossipItemHint("consumable strength", "a magic potion", "a blue drink")]
        [ShopInventory(ShopInventoryAttribute.ShopKeeper.WitchShop, 2)]
        [ShopText("Replenishes both life energy and magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [HackContent(nameof(Resources.mods.fix_shop_witch_bluepotion))]
        [GetItemIndex(0xC2), ItemPool(ItemCategory.BluePotions, LocationCategory.Purchases)]
        ShopItemWitchBluePotion,

        [Purchaseable, Visible]
        [Repeatable, Temporary, Overwritable]
        [ItemName("Red Potion"), LocationName("Witch Shop Red Potion"), Region(Region.SouthernSwamp)]
        [GossipLocationHint("a sleeping witch", "a southern merchant"), GossipItemHint("consumable strength", "a hearty drink", "a red drink")]
        [ShopInventory(ShopInventoryAttribute.ShopKeeper.WitchShop, 0)]
        [ShopText("Replenishes your life energy.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0xC3), ItemPool(ItemCategory.RedPotions, LocationCategory.Purchases)]
        ShopItemWitchRedPotion,

        [Purchaseable, Visible]
        [Repeatable, Temporary, Overwritable]
        [ItemName("Green Potion"), LocationName("Witch Shop Green Potion"), Region(Region.SouthernSwamp)]
        [GossipLocationHint("a sleeping witch", "a southern merchant"), GossipItemHint("a magic potion", "a green drink")]
        [ShopInventory(ShopInventoryAttribute.ShopKeeper.WitchShop, 1)]
        [ShopText("Replenishes your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0xC4), ItemPool(ItemCategory.GreenPotions, LocationCategory.Purchases)]
        ShopItemWitchGreenPotion,

        [Purchaseable, Visible]
        [Repeatable, Temporary]
        [ItemName("10 Bombs"), LocationName("Bomb Shop 10 Bombs"), Region(Region.WestClockTown)]
        [GossipLocationHint("a town merchant"), GossipItemHint("explosives")]
        [ShopInventory(ShopInventoryAttribute.ShopKeeper.BombShop, 3)]
        [ShopText("Explosives. You need a Bomb Bag to carry them.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0xC5), ItemPool(ItemCategory.Bombs, LocationCategory.Purchases)]
        ShopItemBombsBomb10,

        [Purchaseable, Visible]
        [Repeatable, Temporary]
        [ItemName("10 Bombchu"), LocationName("Bomb Shop 10 Bombchu"), Region(Region.WestClockTown)]
        [GossipLocationHint("a town merchant"), GossipItemHint("explosives")]
        [ShopInventory(ShopInventoryAttribute.ShopKeeper.BombShop, 2)]
        [ShopText("Mouse-shaped bombs that are practical, sleek and self-propelled.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0xC6), ItemPool(ItemCategory.Bombchu, LocationCategory.Purchases)]
        ShopItemBombsBombchu10,

        [Purchaseable, Visible]
        [Repeatable, Temporary]
        [ItemName("10 Bombs"), LocationName("Goron Shop 10 Bombs"), MultiLocation(ShopItemGoronBomb10InWinter, ShopItemGoronBomb10InSpring)]
        [GossipLocationHint("a northern merchant", "a bored goron"), GossipItemHint("explosives")]
        [ShopInventory(ShopInventoryAttribute.ShopKeeper.GoronShop, 0)]
        [ShopInventory(ShopInventoryAttribute.ShopKeeper.GoronShopSpring, 0)]
        [ShopText("Explosives. You need a Bomb Bag to carry them.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0xC7), ItemPool(ItemCategory.Bombs, LocationCategory.Purchases)]
        ShopItemGoronBomb10,

        [Purchaseable, Visible]
        [Repeatable, Temporary]
        [ItemName("10 Arrows"), LocationName("Goron Shop 10 Arrows"), MultiLocation(ShopItemGoronArrow10InWinter, ShopItemGoronArrow10InSpring)]
        [GossipLocationHint("a northern merchant", "a bored goron"), GossipItemHint("a quiver refill", "a bundle of projectiles")]
        [ShopInventory(ShopInventoryAttribute.ShopKeeper.GoronShop, 1)]
        [ShopInventory(ShopInventoryAttribute.ShopKeeper.GoronShopSpring, 1)]
        [ShopText("Ammo for your bow.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0xC8), ItemPool(ItemCategory.Arrows, LocationCategory.Purchases)]
        ShopItemGoronArrow10,

        [Purchaseable, Visible]
        [Repeatable, Temporary, Overwritable]
        [ItemName("Red Potion"), LocationName("Goron Shop Red Potion"), MultiLocation(ShopItemGoronRedPotionInWinter, ShopItemGoronRedPotionInSpring)]
        [GossipLocationHint("a northern merchant", "a bored goron"), GossipItemHint("consumable strength", "a hearty drink", "a red drink")]
        [ShopInventory(ShopInventoryAttribute.ShopKeeper.GoronShop, 2)]
        [ShopInventory(ShopInventoryAttribute.ShopKeeper.GoronShopSpring, 2)]
        [ShopText("Replenishes your life energy.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0xC9), ItemPool(ItemCategory.RedPotions, LocationCategory.Purchases)]
        ShopItemGoronRedPotion,

        [Purchaseable, Visible]
        [Repeatable]
        [ItemName("Hero's Shield"), LocationName("Zora Shop Hero's Shield"), Region(Region.ZoraHall)]
        [GossipLocationHint("a western merchant", "an aquatic shop"), GossipItemHint("a basic guard", "protection")]
        [ShopInventory(ShopInventoryAttribute.ShopKeeper.ZoraShop, 0)]
        [ShopText("Use it to defend yourself.")]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [GetItemIndex(0xCA), ItemPool(ItemCategory.Shields, LocationCategory.Purchases)]
        ShopItemZoraShield,

        [Purchaseable, Visible]
        [Repeatable, Temporary]
        [ItemName("10 Arrows"), LocationName("Zora Shop 10 Arrows"), Region(Region.ZoraHall)]
        [GossipLocationHint("a western merchant", "an aquatic shop"), GossipItemHint("a quiver refill", "a bundle of projectiles")]
        [ShopInventory(ShopInventoryAttribute.ShopKeeper.ZoraShop, 1)]
        [ShopText("Ammo for your bow.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0xCB), ItemPool(ItemCategory.Arrows, LocationCategory.Purchases)]
        ShopItemZoraArrow10,

        [Purchaseable, Visible]
        [Repeatable, Temporary, Overwritable]
        [ItemName("Red Potion"), LocationName("Zora Shop Red Potion"), Region(Region.ZoraHall)]
        [GossipLocationHint("a western merchant", "an aquatic shop"), GossipItemHint("consumable strength", "a hearty drink", "a red drink")]
        [ShopInventory(ShopInventoryAttribute.ShopKeeper.ZoraShop, 2)]
        [ShopText("Replenishes your life energy.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0xCC), ItemPool(ItemCategory.RedPotions, LocationCategory.Purchases)]
        ShopItemZoraRedPotion,

        //bottle catch
        [ItemName("Bottle: Fairy"), LocationName("Bottle: Fairy"), Region(Region.BottleCatch)]
        [GossipLocationHint("a wandering healer"), GossipItemHint("a winged friend", "a healer")]
        [GetBottleItemIndices(0x00, 0x0D), ItemPool(ItemCategory.ScoopedItems, LocationCategory.Scoops)]
        BottleCatchFairy,

        [ItemName("Bottle: Deku Princess"), LocationName("Bottle: Deku Princess"), Region(Region.BottleCatch)]
        [GossipLocationHint("a captured royal", "an imprisoned daughter"), GossipItemHint("a princess", "a woodland royal")]
        [GetBottleItemIndices(0x08), ItemPool(ItemCategory.ScoopedItems, LocationCategory.Scoops)]
        BottleCatchPrincess,

        [ItemName("Bottle: Fish"), LocationName("Bottle: Fish"), Region(Region.BottleCatch)]
        [GossipLocationHint("a swimming creature", "a water dweller"), GossipItemHint("something fresh")]
        [GetBottleItemIndices(0x01), ItemPool(ItemCategory.ScoopedItems, LocationCategory.Scoops)]
        BottleCatchFish,

        [ItemName("Bottle: Bug"), LocationName("Bottle: Bug"), Region(Region.BottleCatch)]
        [GossipLocationHint("an insect", "a scuttling creature"), GossipItemHint("an insect", "a scuttling creature")]
        [GetBottleItemIndices(0x02, 0x03), ItemPool(ItemCategory.ScoopedItems, LocationCategory.Scoops)]
        BottleCatchBug,

        [ItemName("Bottle: Poe"), LocationName("Bottle: Poe"), Region(Region.BottleCatch)]
        [GossipLocationHint("a wandering ghost"), GossipItemHint("a captured spirit")]
        [GetBottleItemIndices(0x0B), ItemPool(ItemCategory.ScoopedItems, LocationCategory.Scoops)]
        BottleCatchPoe,

        [ItemName("Bottle: Big Poe"), LocationName("Bottle: Big Poe"), Region(Region.BottleCatch)]
        [GossipLocationHint("a huge ghost"), GossipItemHint("a captured spirit")]
        [GetBottleItemIndices(0x0C), ItemPool(ItemCategory.ScoopedItems, LocationCategory.Scoops)]
        BottleCatchBigPoe,

        [ItemName("Bottle: Spring Water"), LocationName("Bottle: Spring Water"), Region(Region.BottleCatch)]
        [GossipLocationHint("a common liquid"), GossipItemHint("a common liquid", "a fresh drink")]
        [GetBottleItemIndices(0x04), ItemPool(ItemCategory.ScoopedItems, LocationCategory.Scoops)]
        BottleCatchSpringWater,

        [ItemName("Bottle: Hot Spring Water"), LocationName("Bottle: Hot Spring Water"), Region(Region.BottleCatch)]
        [GossipLocationHint("a hot liquid", "a boiling liquid"), GossipItemHint("a boiling liquid", "a hot liquid")]
        [GetBottleItemIndices(0x05, 0x06), ItemPool(ItemCategory.ScoopedItems, LocationCategory.Scoops)]
        BottleCatchHotSpringWater,

        [ItemName("Bottle: Zora Egg"), LocationName("Bottle: Zora Egg"), Region(Region.BottleCatch)]
        [GossipLocationHint("a lost child"), GossipItemHint("a lost child")]
        [GetBottleItemIndices(0x07), ItemPool(ItemCategory.ScoopedItems, LocationCategory.Scoops)]
        BottleCatchEgg,

        [ItemName("Bottle: Mushroom"), LocationName("Bottle: Mushroom"), Region(Region.BottleCatch)]
        [GossipLocationHint("a strange fungus"), GossipItemHint("a strange fungus")]
        [GetBottleItemIndices(0x0A), ItemPool(ItemCategory.ScoopedItems, LocationCategory.Scoops)]
        BottleCatchMushroom,

        //other chests and grottos
        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Lens Cave Invisible Chest"), Region(Region.GoronVillage)]
        [GossipLocationHint("a lonely peak"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), Chest(0x02EB8000 + 0xAA, ChestAttribute.AppearanceType.Invisible)]
        [GetItemIndex(0xDD), ItemPool(ItemCategory.RedRupees, LocationCategory.Chests)]
        ChestLensCaveRedRupee,

        [Repeatable]
        [ItemName("Purple Rupee"), LocationName("Lens Cave Rock Chest"), Region(Region.GoronVillage)]
        [GossipLocationHint("a lonely peak"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 50 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), Chest(0x02EB8000 + 0xDA, ChestAttribute.AppearanceType.Normal)]
        [GetItemIndex(0xF4), ItemPool(ItemCategory.PurpleRupees, LocationCategory.Chests)]
        ChestLensCavePurpleRupee,

        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Bean Grotto"), Region(Region.DekuPalace)]
        [GossipLocationHint("a merchant's cave"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), Chest(0x02ECC000 + 0xFA, ChestAttribute.AppearanceType.Normal)]
        [GetItemIndex(0xDE), ItemPool(ItemCategory.RedRupees, LocationCategory.Chests)]
        ChestBeanGrottoRedRupee,

        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Hot Spring Water Grotto"), Region(Region.TwinIslands)]
        [GossipLocationHint("a steaming cave"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), Chest(0x02ED7000 + 0xC6, ChestAttribute.AppearanceType.Normal)]
        [GetItemIndex(0xDF), ItemPool(ItemCategory.RedRupees, LocationCategory.Chests)]
        ChestHotSpringGrottoRedRupee,

        [Repeatable]
        [ItemName("Purple Rupee"), LocationName("Day 1 Grave Bats"), Region(Region.IkanaGraveyard)]
        [GossipLocationHint("a cloud of bats"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 50 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), Chest(0x01F97000 + 0xCE, ChestAttribute.AppearanceType.AppearsClear)]
        [GetItemIndex(0xF5), ItemPool(ItemCategory.PurpleRupees, LocationCategory.Chests)]
        ChestBadBatsGrottoPurpleRupee,

        [Repeatable, Temporary]
        [ItemName("5 Bombchu"), LocationName("Secret Shrine Grotto"), Region(Region.IkanaCanyon)]
        [GossipLocationHint("a waterfall cave"), GossipItemHint("explosive mice")]
        [ShopText("Mouse-shaped bombs that are practical, sleek and self-propelled.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), GrottoChest(0x02080000 + 0x93, 0x02080000 + 0x1E3, 0x02080000 + 0x2EB)]
        [GetItemIndex(0xD1), ItemPool(ItemCategory.Bombchu, LocationCategory.Chests)]
        ChestIkanaSecretShrineGrotto,

        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Pirates' Fortress Interior Lower Chest"), Region(Region.PiratesFortressInterior)]
        [GossipLocationHint("the home of pirates"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), Chest(0x020A2000 + 0x256, ChestAttribute.AppearanceType.Normal)]
        [GetItemIndex(0xE0), ItemPool(ItemCategory.RedRupees, LocationCategory.Chests)]
        ChestPiratesFortressRedRupee1,

        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Pirates' Fortress Interior Upper Chest"), Region(Region.PiratesFortressInterior)]
        [GossipLocationHint("the home of pirates"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), Chest(0x020A2000 + 0x266, ChestAttribute.AppearanceType.Normal)]
        [GetItemIndex(0xE1), ItemPool(ItemCategory.RedRupees, LocationCategory.Chests)]
        ChestPiratesFortressRedRupee2,

        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Pirates' Fortress Interior Tank Chest"), Region(Region.PiratesFortressInterior)]
        [GossipLocationHint("the home of pirates"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), Chest(0x023B7000 + 0x66, ChestAttribute.AppearanceType.Normal)]
        [GetItemIndex(0xE2), ItemPool(ItemCategory.RedRupees, LocationCategory.Chests)]
        ChestInsidePiratesFortressTankRedRupee,

        [Repeatable]
        [ItemName("Silver Rupee"), LocationName("Pirates' Fortress Interior Guard Room Chest"), Region(Region.PiratesFortressInterior)]
        [GossipLocationHint("the home of pirates"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 100 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), Chest(0x023BB000 + 0x56, ChestAttribute.AppearanceType.Normal)]
        [GetItemIndex(0xFB), ItemPool(ItemCategory.SilverRupees, LocationCategory.Chests)]
        ChestInsidePiratesFortressGuardSilverRupee,

        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Pirates' Fortress Cage Room Shallow Chest"), Region(Region.PiratesFortressSewer)]
        [GossipLocationHint("the home of pirates"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), Chest(0x023E6000 + 0x24E, ChestAttribute.AppearanceType.Normal)]
        [GetItemIndex(0xE3), ItemPool(ItemCategory.RedRupees, LocationCategory.Chests)]
        ChestInsidePiratesFortressHeartPieceRoomRedRupee,

        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Pirates' Fortress Cage Room Deep Chest"), Region(Region.PiratesFortressSewer)]
        [GossipLocationHint("the home of pirates"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), Chest(0x023E6000 + 0x25E, ChestAttribute.AppearanceType.Normal)]
        [GetItemIndex(0x105), ItemPool(ItemCategory.BlueRupees, LocationCategory.Chests)]
        ChestInsidePiratesFortressHeartPieceRoomBlueRupee,

        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Pirates' Fortress Maze Chest"), Region(Region.PiratesFortressSewer)]
        [GossipLocationHint("the home of pirates"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), Chest(0x023F0000 + 0xDE, ChestAttribute.AppearanceType.Normal)]
        [GetItemIndex(0xE4), ItemPool(ItemCategory.RedRupees, LocationCategory.Chests)]
        ChestInsidePiratesFortressMazeRedRupee,

        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Pinnacle Rock Lower Chest"), Region(Region.PinnacleRock)]
        [GossipLocationHint("a marine trench"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), Chest(0x02428000 + 0x24E, ChestAttribute.AppearanceType.Normal)]
        [GetItemIndex(0xE5), ItemPool(ItemCategory.RedRupees, LocationCategory.Chests)]
        ChestPinacleRockRedRupee1,

        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Pinnacle Rock Upper Chest"), Region(Region.PinnacleRock)]
        [GossipLocationHint("a marine trench"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), Chest(0x02428000 + 0x25E, ChestAttribute.AppearanceType.Normal)]
        [GetItemIndex(0xE6), ItemPool(ItemCategory.RedRupees, LocationCategory.Chests)]
        ChestPinacleRockRedRupee2,

        [Repeatable]
        [ItemName("Silver Rupee"), LocationName("Bombers' Hideout Chest"), Region(Region.EastClockTown)]
        [GossipLocationHint("a secret hideout"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 100 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), Chest(0x024F1000 + 0x1DE, ChestAttribute.AppearanceType.Normal)]
        [GetItemIndex(0xFC), ItemPool(ItemCategory.SilverRupees, LocationCategory.Chests)]
        ChestBomberHideoutSilverRupee,

        [Repeatable, Temporary]
        [ItemName("Bombchu"), LocationName("Termina Field Pillar Grotto"), Region(Region.TerminaField)]
        [GossipLocationHint("a hollow pillar"), GossipItemHint("explosive mice")]
        [ShopText("Mouse-shaped bomb that is practical, sleek and self-propelled.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), GrottoChest(0x025C5000 + 0x583)]
        [GetItemIndex(0xD7), ItemPool(ItemCategory.Bombchu, LocationCategory.Chests)]
        ChestTerminaGrottoBombchu,

        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Termina Field Grass Grotto"), Region(Region.TerminaField)]
        [GossipLocationHint("a grassy cave"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), GrottoChest(0x025C5000 + 0x593)]
        [GetItemIndex(0xDC), ItemPool(ItemCategory.RedRupees, LocationCategory.Chests)]
        ChestTerminaGrottoRedRupee,

        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Termina Field Underwater Chest"), Region(Region.TerminaField)]
        [GossipLocationHint("a sunken chest"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), Chest(0x025C5000 + 0xD52, ChestAttribute.AppearanceType.Normal)]
        [GetItemIndex(0xE7), ItemPool(ItemCategory.RedRupees, LocationCategory.Chests)]
        ChestTerminaUnderwaterRedRupee,

        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Termina Field Grass Chest"), Region(Region.TerminaField)]
        [GossipLocationHint("a grassy chest"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), Chest(0x025C5000 + 0xD62, ChestAttribute.AppearanceType.Normal)]
        [GetItemIndex(0xE8), ItemPool(ItemCategory.RedRupees, LocationCategory.Chests)]
        ChestTerminaGrassRedRupee,

        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Termina Field Stump Chest"), Region(Region.TerminaField)]
        [GossipLocationHint("a tree's chest"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), Chest(0x025C5000 + 0xD72, ChestAttribute.AppearanceType.Normal)]
        [GetItemIndex(0xE9), ItemPool(ItemCategory.RedRupees, LocationCategory.Chests)]
        ChestTerminaStumpRedRupee,

        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Great Bay Coast Grotto"), Region(Region.GreatBayCoast)]
        [GossipLocationHint("a beach cave"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), GrottoChest(0x026DE000 + 0x43F, 0x026DE000 + 0xFE3)]
        [GetItemIndex(0xD4), ItemPool(ItemCategory.RedRupees, LocationCategory.Chests)]
        ChestGreatBayCoastGrotto, //contents? 

        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Zora Cape Ledge Without Tree Chest"), Region(Region.ZoraCape)]
        [GossipLocationHint("a high place"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), Chest(0x02715000 + 0x42A, ChestAttribute.AppearanceType.Normal, 0x02715000 + 0xB16)]
        [GetItemIndex(0xEA), ItemPool(ItemCategory.RedRupees, LocationCategory.Chests)]
        ChestGreatBayCapeLedge1, //contents? 

        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Zora Cape Ledge With Tree Chest"), Region(Region.ZoraCape)]
        [GossipLocationHint("a high place"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), Chest(0x02715000 + 0x43A, ChestAttribute.AppearanceType.Normal, 0x02715000 + 0xB26)]
        [GetItemIndex(0xEB), ItemPool(ItemCategory.RedRupees, LocationCategory.Chests)]
        ChestGreatBayCapeLedge2, //contents? 

        [Repeatable, Temporary]
        [ItemName("Bombchu"), LocationName("Zora Cape Grotto"), Region(Region.ZoraCape)]
        [GossipLocationHint("a beach cave"), GossipItemHint("explosive mice")]
        [ShopText("Mouse-shaped bomb that is practical, sleek and self-propelled.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), GrottoChest(0x02715000 + 0x45B, 0x02715000 + 0xB47)]
        [GetItemIndex(0xD2), ItemPool(ItemCategory.Bombchu, LocationCategory.Chests)]
        ChestGreatBayCapeGrotto, //contents? 

        [Repeatable]
        [ItemName("Purple Rupee"), LocationName("Zora Cape Underwater Chest"), Region(Region.ZoraCape)]
        [GossipLocationHint("a sunken chest"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 50 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), Chest(0x02715000 + 0x48A, ChestAttribute.AppearanceType.Normal, 0x02715000 + 0xB56)]
        [GetItemIndex(0xF6), ItemPool(ItemCategory.PurpleRupees, LocationCategory.Chests)]
        ChestGreatBayCapeUnderwater, //contents? 

        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Pirates' Fortress Exterior Log Chest"), Region(Region.PiratesFortressExterior)]
        [GossipLocationHint("the home of pirates"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), Chest(0x02740000 + 0x196, ChestAttribute.AppearanceType.Normal)]
        [GetItemIndex(0xEC), ItemPool(ItemCategory.RedRupees, LocationCategory.Chests)]
        ChestPiratesFortressEntranceRedRupee1,

        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Pirates' Fortress Exterior Sand Chest"), Region(Region.PiratesFortressExterior)]
        [GossipLocationHint("the home of pirates"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), Chest(0x02740000 + 0x1A6, ChestAttribute.AppearanceType.Normal)]
        [GetItemIndex(0xED), ItemPool(ItemCategory.RedRupees, LocationCategory.Chests)]
        ChestPiratesFortressEntranceRedRupee2,

        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Pirates' Fortress Exterior Corner Chest"), Region(Region.PiratesFortressExterior)]
        [GossipLocationHint("the home of pirates"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), Chest(0x02740000 + 0x1B6, ChestAttribute.AppearanceType.Normal)]
        [GetItemIndex(0xEE), ItemPool(ItemCategory.RedRupees, LocationCategory.Chests)]
        ChestPiratesFortressEntranceRedRupee3,

        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Path to Swamp Grotto"), Region(Region.RoadToSouthernSwamp)]
        [GossipLocationHint("a southern cave"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), GrottoChest(0x027C1000 + 0x33B)]
        [GetItemIndex(0xDB), ItemPool(ItemCategory.RedRupees, LocationCategory.Chests)]
        ChestToSwampGrotto, //contents? 

        [Repeatable]
        [ItemName("Purple Rupee"), LocationName("Doggy Racetrack Roof Chest"), Region(Region.RomaniRanch)]
        [GossipLocationHint("a day at the races"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 50 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), Chest(0x027D4000 + 0xB6, ChestAttribute.AppearanceType.Normal)]
        [GetItemIndex(0xF7), ItemPool(ItemCategory.PurpleRupees, LocationCategory.Chests)]
        ChestDogRacePurpleRupee,

        [Repeatable, Temporary]
        [ItemName("5 Bombchu"), LocationName("Ikana Graveyard Grotto"), Region(Region.IkanaGraveyard)]
        [ShopText("Mouse-shaped bombs that are practical, sleek and self-propelled.", isMultiple: true)]
        [GossipLocationHint("a circled cave"), GossipItemHint("explosive mice")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), GrottoChest(0x0280D000 + 0x353, 0x0280D000 + 0x54B)]
        [GetItemIndex(0xD5), ItemPool(ItemCategory.Bombchu, LocationCategory.Chests)]
        ChestGraveyardGrotto, //contents? 

        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Near Swamp Spider House Grotto"), Region(Region.SouthernSwamp)]
        [GossipLocationHint("a southern cave"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), GrottoChest(0x01F3A000 + 0x227, 0x02855000 + 0x2AF)]
        [GetItemIndex(0xDA), ItemPool(ItemCategory.RedRupees, LocationCategory.Chests)]
        ChestSwampGrotto,  //contents? 

        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Behind Woodfall Owl Chest"), Region(Region.Woodfall)]
        [GossipLocationHint("a swamp chest"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), Chest(0x02884000 + 0x232, ChestAttribute.AppearanceType.Normal, 0x02884000 + 0xA62)]
        [GetItemIndex(0x106), ItemPool(ItemCategory.BlueRupees, LocationCategory.Chests)]
        ChestWoodfallBlueRupee,

        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Entrance to Woodfall Chest"), Region(Region.Woodfall)]
        [GossipLocationHint("a swamp chest"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), Chest(0x02884000 + 0x242, ChestAttribute.AppearanceType.Normal, 0x02884000 + 0xA32)]
        [GetItemIndex(0xEF), ItemPool(ItemCategory.RedRupees, LocationCategory.Chests)]
        ChestWoodfallRedRupee,

        [Repeatable]
        [ItemName("Purple Rupee"), LocationName("Well Right Path Chest"), Region(Region.BeneathTheWell)]
        [GossipLocationHint("a frightful exchange"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 50 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), Chest(0x029EA000 + 0xE6, ChestAttribute.AppearanceType.AppearsSwitch)]
        [GetItemIndex(0xF8), ItemPool(ItemCategory.PurpleRupees, LocationCategory.Chests)]
        ChestWellRightPurpleRupee,

        [Repeatable]
        [ItemName("Purple Rupee"), LocationName("Well Left Path Chest"), Region(Region.BeneathTheWell)]
        [GossipLocationHint("a frightful exchange"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 50 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), Chest(0x029F0000 + 0x106, ChestAttribute.AppearanceType.Invisible)]
        [GetItemIndex(0xF9), ItemPool(ItemCategory.PurpleRupees, LocationCategory.Chests)]
        ChestWellLeftPurpleRupee,

        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Mountain Waterfall Chest"), Region(Region.MountainVillage)]
        [GossipLocationHint("the springtime"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), Chest(0x02BDD000 + 0x2E2, ChestAttribute.AppearanceType.Invisible, 0x02BDD000 + 0x946)]
        [GetItemIndex(0xF0), ItemPool(ItemCategory.RedRupees, LocationCategory.Chests)]
        ChestMountainVillage, //contents? 

        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Mountain Spring Grotto"), Region(Region.MountainVillage)]
        [GossipLocationHint("the springtime"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), GrottoChest(0x02BFC000 + 0x1F3, 0x02BFC000 + 0x2B3)]
        [GetItemIndex(0xD8), ItemPool(ItemCategory.RedRupees, LocationCategory.Chests)]
        ChestMountainVillageGrottoRedRupee,

        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Path to Ikana Pillar Chest"), Region(Region.RoadToIkana)]
        [GossipLocationHint("a high chest"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), Chest(0x02B34000 + 0x442, ChestAttribute.AppearanceType.Normal)]
        [GetItemIndex(0xF1), ItemPool(ItemCategory.RedRupees, LocationCategory.Chests)]
        ChestToIkanaRedRupee,

        [Repeatable, Temporary]
        [ItemName("Bombchu"), LocationName("Path to Ikana Grotto"), Region(Region.RoadToIkana)]
        [GossipLocationHint("a blocked cave"), GossipItemHint("explosive mice")]
        [ShopText("Mouse-shaped bomb that is practical, sleek and self-propelled.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), GrottoChest(0x02B34000 + 0x523)]
        [GetItemIndex(0xD3), ItemPool(ItemCategory.Bombchu, LocationCategory.Chests)]
        ChestToIkanaGrotto, //contents? 

        [Repeatable]
        [ItemName("Silver Rupee"), LocationName("Inverted Stone Tower Right Chest"), Region(Region.StoneTower)]
        [GossipLocationHint("a sky below"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 100 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), Chest(0x02BC9000 + 0x236, ChestAttribute.AppearanceType.Normal)]
        [GetItemIndex(0xFD), ItemPool(ItemCategory.SilverRupees, LocationCategory.Chests)]
        ChestInvertedStoneTowerSilverRupee,

        [Repeatable, Temporary]
        [ItemName("10 Bombchu"), LocationName("Inverted Stone Tower Middle Chest"), Region(Region.StoneTower)]
        [GossipLocationHint("a sky below"), GossipItemHint("explosive mice")]
        [ShopText("Mouse-shaped bombs that are practical, sleek and self-propelled.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), Chest(0x02BC9000 + 0x246, ChestAttribute.AppearanceType.Normal)]
        [GetItemIndex(0x10A), ItemPool(ItemCategory.Bombchu, LocationCategory.Chests)]
        ChestInvertedStoneTowerBombchu10,

        [Repeatable, Temporary]
        [StartingItemId(0x0A)]
        [ItemName("Magic Bean"), LocationName("Inverted Stone Tower Left Chest"), Region(Region.StoneTower)]
        [GossipLocationHint("a sky below"), GossipItemHint("a plant seed")]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold), Chest(0x02BC9000 + 0x256, ChestAttribute.AppearanceType.Normal)]
        [ShopText("Plant it in soft soil.")]
        [GetItemIndex(0x109), ItemPool(ItemCategory.MainInventory, LocationCategory.Chests)]
        ChestInvertedStoneTowerBean,

        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Path to Snowhead Grotto"), Region(Region.PathToSnowhead)]
        [GossipLocationHint("a snowy cave"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), GrottoChest(0x02C04000 + 0xAF, 0x02C04000 + 0x487)]
        [GetItemIndex(0xD0), ItemPool(ItemCategory.RedRupees, LocationCategory.Chests)]
        ChestToSnowheadGrotto, //contents? 

        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Twin Islands Cave Chest"), Region(Region.TwinIslands)]
        [GossipLocationHint("the springtime"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), Chest(0x02C34000 + 0x13A, ChestAttribute.AppearanceType.Normal)]
        [GetItemIndex(0xF2), ItemPool(ItemCategory.RedRupees, LocationCategory.Chests)]
        ChestToGoronVillageRedRupee,

        [StartingItem(0xC5CE70, 0x10, true)]
        [ItemName("Piece of Heart"), LocationName("Secret Shrine Final Chest"), Region(Region.SecretShrine)]
        [GossipLocationHint("a secret place"), GossipItemHint("a segment of health")]
        [ShopText("Collect four to assemble a new Heart Container.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), Chest(0x02C57000 + 0xB6, ChestAttribute.AppearanceType.AppearsSwitch)]
        [GetItemIndex(0x107), ItemPool(ItemCategory.PiecesOfHeart, LocationCategory.BossFights)]
        ChestSecretShrineHeartPiece,

        [Repeatable]
        [ItemName("Silver Rupee"), LocationName("Secret Shrine Dinolfos Chest"), Region(Region.SecretShrine)]
        [GossipLocationHint("a secret place"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 100 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), Chest(0x02C61000 + 0x9A, ChestAttribute.AppearanceType.AppearsClear)]
        [GetItemIndex(0xFE), ItemPool(ItemCategory.SilverRupees, LocationCategory.BossFights)]
        ChestSecretShrineDinoGrotto,

        [Repeatable]
        [ItemName("Silver Rupee"), LocationName("Secret Shrine Wizzrobe Chest"), Region(Region.SecretShrine)]
        [GossipLocationHint("a secret place"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 100 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), Chest(0x02C69000 + 0xB2, ChestAttribute.AppearanceType.AppearsClear)]
        [GetItemIndex(0xFF), ItemPool(ItemCategory.SilverRupees, LocationCategory.BossFights)]
        ChestSecretShrineWizzGrotto,

        [Repeatable]
        [ItemName("Silver Rupee"), LocationName("Secret Shrine Wart Chest"), Region(Region.SecretShrine)]
        [GossipLocationHint("a secret place"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 100 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), Chest(0x02C71000 + 0xA6, ChestAttribute.AppearanceType.AppearsClear)]
        [GetItemIndex(0x100), ItemPool(ItemCategory.SilverRupees, LocationCategory.BossFights)]
        ChestSecretShrineWartGrotto,

        [Repeatable]
        [ItemName("Silver Rupee"), LocationName("Secret Shrine Garo Master Chest"), Region(Region.SecretShrine)]
        [GossipLocationHint("a secret place"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 100 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), Chest(0x02C75000 + 0x76, ChestAttribute.AppearanceType.AppearsClear)]
        [GetItemIndex(0x101), ItemPool(ItemCategory.SilverRupees, LocationCategory.BossFights)]
        ChestSecretShrineGaroGrotto,

        [Repeatable]
        [ItemName("Silver Rupee"), LocationName("Inn Staff Room Chest"), Region(Region.StockPotInn)]
        [GossipLocationHint("an employee room"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 100 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), Chest(0x02CAB000 + 0x10E, ChestAttribute.AppearanceType.Normal, 0x02CAB000 + 0x242)]
        [GetItemIndex(0x102), ItemPool(ItemCategory.SilverRupees, LocationCategory.Chests)]
        ChestInnStaffRoom, //contents? 

        [Repeatable]
        [ItemName("Silver Rupee"), LocationName("Inn Guest Room Chest"), Region(Region.StockPotInn)]
        [GossipLocationHint("a guest bedroom"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 100 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), Chest(0x02CB1000 + 0xDA, ChestAttribute.AppearanceType.Normal, 0x02CB1000 + 0x212)]
        [GetItemIndex(0x103), ItemPool(ItemCategory.SilverRupees, LocationCategory.Chests)]
        ChestInnGuestRoom, //contents? 

        [Repeatable]
        [ItemName("Purple Rupee"), LocationName("Mystery Woods Grotto"), Region(Region.SouthernSwamp)]
        [GossipLocationHint("a mystery cave"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 50 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), GrottoChest(0x02CFC000 + 0x5B)]
        [GetItemIndex(0xD9), ItemPool(ItemCategory.PurpleRupees, LocationCategory.Chests)]
        ChestWoodsGrotto, //contents? 

        [Repeatable]
        [ItemName("Silver Rupee"), LocationName("East Clock Town Chest"), Region(Region.EastClockTown)]
        [GossipLocationHint("a shop roof"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 100 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), Chest(0x02DE4000 + 0x442, ChestAttribute.AppearanceType.Normal)]
        [GetItemIndex(0x104), ItemPool(ItemCategory.SilverRupees, LocationCategory.Chests)]
        ChestEastClockTownSilverRupee,

        [Repeatable]
        [ItemName("Red Rupee"), LocationName("South Clock Town Straw Roof Chest"), Region(Region.SouthClockTown)]
        [GossipLocationHint("a straw roof"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), Chest(0x02E5C000 + 0x342, ChestAttribute.AppearanceType.Normal, 0x02E5C000 + 0x806)]
        [GetItemIndex(0xF3), ItemPool(ItemCategory.RedRupees, LocationCategory.Chests)]
        ChestSouthClockTownRedRupee,

        [Repeatable]
        [ItemName("Purple Rupee"), LocationName("South Clock Town Final Day Chest"), Region(Region.SouthClockTown)]
        [GossipLocationHint("a carnival tower"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 50 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), Chest(0x02E5C000 + 0x352, ChestAttribute.AppearanceType.Normal)]
        [GetItemIndex(0xFA), ItemPool(ItemCategory.PurpleRupees, LocationCategory.Chests)]
        ChestSouthClockTownPurpleRupee,

        [StartingItem(0xC5CE70, 0x10, true)]
        [ItemName("Piece of Heart"), LocationName("Bank Reward #3"), Region(Region.WestClockTown)]
        [GossipLocationHint("being rich"), GossipItemHint("a segment of health"), GossipCompetitiveHint(-2)]
        [ShopText("Collect four to assemble a new Heart Container.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x108), ItemPool(ItemCategory.PiecesOfHeart, LocationCategory.NpcRewards)]
        HeartPieceBank,

        //standing HPs
        [Visible]
        [StartingItem(0xC5CE70, 0x10, true)]
        [ItemName("Piece of Heart"), LocationName("Clock Tower Entrance"), Region(Region.SouthClockTown)]
        [GossipLocationHint("the tower doors"), GossipItemHint("a segment of health")]
        [ShopText("Collect four to assemble a new Heart Container.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x10B), ItemPool(ItemCategory.PiecesOfHeart, LocationCategory.Freestanding)]
        HeartPieceSouthClockTown,

        [Visible]
        [StartingItem(0xC5CE70, 0x10, true)]
        [ItemName("Piece of Heart"), LocationName("North Clock Town Tree"), Region(Region.NorthClockTown)]
        [GossipLocationHint("a town playground"), GossipItemHint("a segment of health")]
        [ShopText("Collect four to assemble a new Heart Container.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x10C), ItemPool(ItemCategory.PiecesOfHeart, LocationCategory.Freestanding)]
        HeartPieceNorthClockTown,

        [Visible]
        [StartingItem(0xC5CE70, 0x10, true)]
        [ItemName("Piece of Heart"), LocationName("Path to Swamp Tree"), Region(Region.RoadToSouthernSwamp)]
        [GossipLocationHint("a tree of bats"), GossipItemHint("a segment of health")]
        [ShopText("Collect four to assemble a new Heart Container.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x10D), ItemPool(ItemCategory.PiecesOfHeart, LocationCategory.Freestanding)]
        HeartPieceToSwamp,

        [Visible]
        [StartingItem(0xC5CE70, 0x10, true)]
        [ItemName("Piece of Heart"), LocationName("Swamp Tourist Center Roof"), Region(Region.SouthernSwamp)]
        [GossipLocationHint("a tourist centre"), GossipItemHint("a segment of health")]
        [ShopText("Collect four to assemble a new Heart Container.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x10E), ItemPool(ItemCategory.PiecesOfHeart, LocationCategory.Freestanding)]
        HeartPieceSwampScrub,

        [Visible]
        [StartingItem(0xC5CE70, 0x10, true)]
        [ItemName("Piece of Heart"), LocationName("Deku Palace West Garden"), Region(Region.DekuPalace)]
        [GossipLocationHint("the home of scrubs"), GossipItemHint("a segment of health")]
        [ShopText("Collect four to assemble a new Heart Container.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x10F), ItemPool(ItemCategory.PiecesOfHeart, LocationCategory.Freestanding)]
        HeartPieceDekuPalace,

        [Visible]
        [StartingItem(0xC5CE70, 0x10, true)]
        [ItemName("Piece of Heart"), LocationName("Goron Village Ledge"), Region(Region.GoronVillage)]
        [GossipLocationHint("a cold ledge"), GossipItemHint("a segment of health")]
        [ShopText("Collect four to assemble a new Heart Container.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x110), ItemPool(ItemCategory.PiecesOfHeart, LocationCategory.Freestanding)]
        HeartPieceGoronVillageScrub,

        [Visible]
        [StartingItem(0xC5CE70, 0x10, true)]
        [ItemName("Piece of Heart"), LocationName("Bio Baba Grotto"), Region(Region.TerminaField)]
        [GossipLocationHint("a beehive"), GossipItemHint("a segment of health")]
        [ShopText("Collect four to assemble a new Heart Container.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x111), ItemPool(ItemCategory.PiecesOfHeart, LocationCategory.Beehives)]
        HeartPieceZoraGrotto,

        [Visible]
        [StartingItem(0xC5CE70, 0x10, true)]
        [ItemName("Piece of Heart"), LocationName("Lab Fish"), Region(Region.GreatBayCoast)]
        [GossipLocationHint("feeding the fish"), GossipItemHint("a segment of health"), GossipCompetitiveHint(0, nameof(GameplaySettings.SpeedupLabFish), false)]
        [ShopText("Collect four to assemble a new Heart Container.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x112), ItemPool(ItemCategory.PiecesOfHeart, LocationCategory.NpcRewards)]
        HeartPieceLabFish,

        [Visible]
        [StartingItem(0xC5CE70, 0x10, true)]
        [ItemName("Piece of Heart"), LocationName("Zora Cape Like-Like"), Region(Region.ZoraCape)]
        [GossipLocationHint("a shield eater"), GossipItemHint("a segment of health")]
        [ShopText("Collect four to assemble a new Heart Container.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x113), ItemPool(ItemCategory.PiecesOfHeart, LocationCategory.EnemySpawn)]
        HeartPieceGreatBayCapeLikeLike,

        [Visible]
        [StartingItem(0xC5CE70, 0x10, true)]
        [ItemName("Piece of Heart"), LocationName("Pirates' Fortress Cage"), Region(Region.PiratesFortressSewer)]
        [GossipLocationHint("a timed door"), GossipItemHint("a segment of health")]
        [ShopText("Collect four to assemble a new Heart Container.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x114), ItemPool(ItemCategory.PiecesOfHeart, LocationCategory.Freestanding)]
        HeartPiecePiratesFortress,

        [Visible]
        [StartingItem(0xC5CE70, 0x10, true)]
        [ItemName("Piece of Heart"), LocationName("Lulu's Room Ledge"), Region(Region.ZoraHall)]
        [GossipLocationHint("the singer's room"), GossipItemHint("a segment of health")]
        [ShopText("Collect four to assemble a new Heart Container.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x115), ItemPool(ItemCategory.PiecesOfHeart, LocationCategory.Freestanding)]
        HeartPieceZoraHallScrub,

        [Visible]
        [StartingItem(0xC5CE70, 0x10, true)]
        [ItemName("Piece of Heart"), LocationName("Path to Snowhead Pillar"), Region(Region.PathToSnowhead)]
        [GossipLocationHint("a cold platform"), GossipItemHint("a segment of health")]
        [ShopText("Collect four to assemble a new Heart Container.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x116), ItemPool(ItemCategory.PiecesOfHeart, LocationCategory.Freestanding)]
        HeartPieceToSnowhead,

        [Visible]
        [StartingItem(0xC5CE70, 0x10, true)]
        [ItemName("Piece of Heart"), LocationName("Great Bay Coast Ledge"), Region(Region.GreatBayCoast)]
        [GossipLocationHint("a rock face"), GossipItemHint("a segment of health")]
        [ShopText("Collect four to assemble a new Heart Container.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x117), ItemPool(ItemCategory.PiecesOfHeart, LocationCategory.Freestanding)]
        HeartPieceGreatBayCoast,

        [Visible]
        [StartingItem(0xC5CE70, 0x10, true)]
        [ItemName("Piece of Heart"), LocationName("Ikana Canyon Ledge"), Region(Region.IkanaCanyon)]
        [GossipLocationHint("a thief's doorstep"), GossipItemHint("a segment of health")]
        [ShopText("Collect four to assemble a new Heart Container.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x118), ItemPool(ItemCategory.PiecesOfHeart, LocationCategory.Freestanding)]
        HeartPieceIkana,

        [Visible]
        [StartingItem(0xC5CE70, 0x10, true)]
        [ItemName("Piece of Heart"), LocationName("Ikana Castle Pillar"), Region(Region.IkanaCastle)]
        [GossipLocationHint("a fiery pillar"), GossipItemHint("a segment of health")]
        [ShopText("Collect four to assemble a new Heart Container.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x119), ItemPool(ItemCategory.PiecesOfHeart, LocationCategory.Freestanding)]
        HeartPieceCastle,

        [Visible]
        [StartingItem(0xC5CDE9, 0x10, true)] // add max health
        [StartingItem(0xC5CDEB, 0x10, true)] // add current health
        [StartingItem(0xC40E1B, 0x10, true)] // add respawn health
        [StartingItem(0xBDA683, 0x10, true)] // add minimum Song of Time health
        [StartingItem(0xBDA68F, 0x10, true)] // add minimum Song of Time health
        [ItemName("Heart Container"), LocationName("Odolwa Heart Container"), Region(Region.WoodfallTemple)]
        [GossipLocationHint("a masked evil"), GossipItemHint("increased life")]
        [ShopText("Permanently increases your life energy.")]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [GetItemIndex(0x11A), ItemPool(ItemCategory.HeartContainers, LocationCategory.BossFights)]
        HeartContainerWoodfall,

        [Visible]
        [StartingItem(0xC5CDE9, 0x10, true)] // add max health
        [StartingItem(0xC5CDEB, 0x10, true)] // add current health
        [StartingItem(0xC40E1B, 0x10, true)] // add respawn health
        [StartingItem(0xBDA683, 0x10, true)] // add minimum Song of Time health
        [StartingItem(0xBDA68F, 0x10, true)] // add minimum Song of Time health
        [ItemName("Heart Container"), LocationName("Goht Heart Container"), Region(Region.SnowheadTemple)]
        [GossipLocationHint("a masked evil"), GossipItemHint("increased life")]
        [ShopText("Permanently increases your life energy.")]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [GetItemIndex(0x11B), ItemPool(ItemCategory.HeartContainers, LocationCategory.BossFights)]
        HeartContainerSnowhead,

        [Visible]
        [StartingItem(0xC5CDE9, 0x10, true)] // add max health
        [StartingItem(0xC5CDEB, 0x10, true)] // add current health
        [StartingItem(0xC40E1B, 0x10, true)] // add respawn health
        [StartingItem(0xBDA683, 0x10, true)] // add minimum Song of Time health
        [StartingItem(0xBDA68F, 0x10, true)] // add minimum Song of Time health
        [ItemName("Heart Container"), LocationName("Gyorg Heart Container"), Region(Region.GreatBayTemple)]
        [GossipLocationHint("a masked evil"), GossipItemHint("increased life")]
        [ShopText("Permanently increases your life energy.")]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [GetItemIndex(0x11C), ItemPool(ItemCategory.HeartContainers, LocationCategory.BossFights)]
        HeartContainerGreatBay,

        [Visible]
        [StartingItem(0xC5CDE9, 0x10, true)] // add max health
        [StartingItem(0xC5CDEB, 0x10, true)] // add current health
        [StartingItem(0xC40E1B, 0x10, true)] // add respawn health
        [StartingItem(0xBDA683, 0x10, true)] // add minimum Song of Time health
        [StartingItem(0xBDA68F, 0x10, true)] // add minimum Song of Time health
        [ItemName("Heart Container"), LocationName("Twinmold Heart Container"), Region(Region.StoneTowerTemple)]
        [GossipLocationHint("a masked evil"), GossipItemHint("increased life")]
        [ShopText("Permanently increases your life energy.")]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [GetItemIndex(0x11D), ItemPool(ItemCategory.HeartContainers, LocationCategory.BossFights)]
        HeartContainerStoneTower,

        //maps
        [Purchaseable]
        [StartingTingleMap(TingleMap.Town)]
        [ItemName("Map of Clock Town"), LocationName("Clock Town Map Purchase"), MultiLocation(ItemTingleMapTownInTown, ItemTingleMapTownInCanyon)]
        [GossipLocationHint("a map maker", "a forest fairy"), GossipItemHint("a world map")]
        [ShopText("Map of Clock Town.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0xB4), ItemPool(ItemCategory.Navigation, LocationCategory.Purchases)]
        ItemTingleMapTown,

        [Purchaseable]
        [StartingTingleMap(TingleMap.Swamp)]
        [ItemName("Map of Woodfall"), LocationName("Woodfall Map Purchase"), MultiLocation(ItemTingleMapWoodfallInSwamp, ItemTingleMapWoodfallInTown)]
        [GossipLocationHint("a map maker", "a forest fairy"), GossipItemHint("a world map")]
        [ShopText("Map of the south.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0xB5), ItemPool(ItemCategory.Navigation, LocationCategory.Purchases)]
        ItemTingleMapWoodfall,

        [Purchaseable]
        [StartingTingleMap(TingleMap.Mountain)]
        [ItemName("Map of Snowhead"), LocationName("Snowhead Map Purchase"), MultiLocation(ItemTingleMapSnowheadInMountain, ItemTingleMapSnowheadInSwamp)]
        [GossipLocationHint("a map maker", "a forest fairy"), GossipItemHint("a world map")]
        [ShopText("Map of the north.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0xB6), ItemPool(ItemCategory.Navigation, LocationCategory.Purchases)]
        ItemTingleMapSnowhead,

        [Purchaseable]
        [StartingTingleMap(TingleMap.Ranch)]
        [ItemName("Map of Romani Ranch"), LocationName("Romani Ranch Map Purchase"), MultiLocation(ItemTingleMapRanchInRanch, ItemTingleMapRanchInMountain)]
        [GossipLocationHint("a map maker", "a forest fairy"), GossipItemHint("a world map")]
        [ShopText("Map of the ranch.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0xB7), ItemPool(ItemCategory.Navigation, LocationCategory.Purchases)]
        ItemTingleMapRanch,

        [Purchaseable]
        [StartingTingleMap(TingleMap.Ocean)]
        [ItemName("Map of Great Bay"), LocationName("Great Bay Map Purchase"), MultiLocation(ItemTingleMapGreatBayInOcean, ItemTingleMapGreatBayInRanch)]
        [GossipLocationHint("a map maker", "a forest fairy"), GossipItemHint("a world map")]
        [ShopText("Map of the west.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0xB8), ItemPool(ItemCategory.Navigation, LocationCategory.Purchases)]
        ItemTingleMapGreatBay,

        [Purchaseable]
        [StartingTingleMap(TingleMap.Canyon)]
        [ItemName("Map of Stone Tower"), LocationName("Stone Tower Map Purchase"), MultiLocation(ItemTingleMapStoneTowerInCanyon, ItemTingleMapStoneTowerInOcean)]
        [GossipLocationHint("a map maker", "a forest fairy"), GossipItemHint("a world map")]
        [ShopText("Map of the east.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0xB9), ItemPool(ItemCategory.Navigation, LocationCategory.Purchases)]
        ItemTingleMapStoneTower,

        //oops I forgot one
        [Repeatable, Temporary]
        [ItemName("Bombchu"), LocationName("Goron Racetrack Grotto"), Region(Region.TwinIslands)]
        [GossipLocationHint("a hidden cave"), GossipItemHint("explosive mice")]
        [ShopText("Mouse-shaped bomb that is practical, sleek and self-propelled.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), GrottoChest(0x02C23000 + 0x2D7, 0x02C34000 + 0x1DB)]
        [GetItemIndex(0xD6), ItemPool(ItemCategory.Bombchu, LocationCategory.Chests)]
        ChestToGoronRaceGrotto, //contents?

        [Repeatable]
        [ItemName("Gold Rupee"), LocationName("Canyon Scrub Trade"), Region(Region.IkanaCanyon)]
        [GossipLocationHint("an eastern merchant"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 200 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x125), ItemPool(ItemCategory.GoldRupees, LocationCategory.NpcRewards)]
        IkanaScrubGoldRupee,

        //moon items
        OtherOneMask,
        OtherTwoMasks,
        OtherThreeMasks,
        OtherFourMasks,
        AreaMoonAccess,

        [Visible]
        [StartingItem(0xC5CE70, 0x10, true)]
        [ItemName("Piece of Heart"), LocationName("Deku Trial Bonus"), Region(Region.TheMoon)]
        [GossipLocationHint("a masked child's game"), GossipItemHint("a segment of health")]
        [ShopText("Collect four to assemble a new Heart Container.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x11F), ItemPool(ItemCategory.PiecesOfHeart, LocationCategory.MoonItems)]
        HeartPieceDekuTrial,

        [Visible]
        [StartingItem(0xC5CE70, 0x10, true)]
        [ItemName("Piece of Heart"), LocationName("Goron Trial Bonus"), Region(Region.TheMoon)]
        [GossipLocationHint("a masked child's game"), GossipItemHint("a segment of health")]
        [ShopText("Collect four to assemble a new Heart Container.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x120), ItemPool(ItemCategory.PiecesOfHeart, LocationCategory.MoonItems)]
        HeartPieceGoronTrial,

        [Visible]
        [StartingItem(0xC5CE70, 0x10, true)]
        [ItemName("Piece of Heart"), LocationName("Zora Trial Bonus"), Region(Region.TheMoon)]
        [GossipLocationHint("a masked child's game"), GossipItemHint("a segment of health")]
        [ShopText("Collect four to assemble a new Heart Container.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x121), ItemPool(ItemCategory.PiecesOfHeart, LocationCategory.MoonItems)]
        HeartPieceZoraTrial,

        [Visible]
        [StartingItem(0xC5CE70, 0x10, true)]
        [ItemName("Piece of Heart"), LocationName("Link Trial Bonus"), Region(Region.TheMoon)]
        [GossipLocationHint("a masked child's game"), GossipItemHint("a segment of health")]
        [ShopText("Collect four to assemble a new Heart Container.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x122), ItemPool(ItemCategory.PiecesOfHeart, LocationCategory.MoonItems)]
        HeartPieceLinkTrial,

        [StartingItem(0xC5CE53, 0x35)]
        [ItemName("Fierce Deity's Mask"), LocationName("Majora Child"), Region(Region.TheMoon)]
        [GossipLocationHint("the lonely child"), GossipItemHint("the wrath of a god")]
        [ShopText("A mask that contains the merits of all masks.", isDefinite: true)]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [HackContent(nameof(Resources.mods.fix_fd_mask_reset))]
        [GetItemIndex(0x7B), ItemPool(ItemCategory.Masks, LocationCategory.MoonItems)]
        MaskFierceDeity,

        [Repeatable, Temporary]
        [ItemName("30 Arrows"), LocationName("Link Trial Garo Master Chest"), Region(Region.TheMoon)]
        [GossipLocationHint("a masked child's game"), GossipItemHint("a quiver refill", "a bundle of projectiles")]
        [ShopText("Ammo for your bow.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), Chest(0x02D4B000 + 0x76, ChestAttribute.AppearanceType.AppearsClear)]
        [GetItemIndex(0x126), ItemPool(ItemCategory.Arrows, LocationCategory.MoonItems)]
        ChestLinkTrialArrow30,

        [Repeatable, Temporary]
        [ItemName("10 Bombchu"), LocationName("Link Trial Iron Knuckle Chest"), Region(Region.TheMoon)]
        [GossipLocationHint("a masked child's game"), GossipItemHint("explosive mice")]
        [ShopText("Mouse-shaped bombs that are practical, sleek and self-propelled.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), Chest(0x02D4E000 + 0xC6, ChestAttribute.AppearanceType.AppearsClear)]
        [GetItemIndex(0x127), ItemPool(ItemCategory.Bombchu, LocationCategory.MoonItems)]
        ChestLinkTrialBombchu10,

        [Repeatable, Temporary]
        [ItemName("10 Deku Nuts"), LocationName("Pre-Clocktown Chest"), Region(Region.BeneathClocktown)]
        [GossipLocationHint("the first chest"), GossipItemHint("a flashing impact")]
        [ShopText("Its flash blinds enemies.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), Chest(0x021D2000 + 0x102, ChestAttribute.AppearanceType.Normal)]
        [GetItemIndex(0x128), ItemPool(ItemCategory.DekuNuts, LocationCategory.GlitchesRequired)]
        ChestPreClocktownDekuNut,

        [Progressive]
        [StartingItem(0xC5CE21, 0x01)]
        [StartingItem(0xC5CE00, 0x4D)]
        [ItemName("Kokiri Sword"), LocationName("Starting Sword"), Region(Region.Misc)]
        [GossipLocationHint("a new file", "a quest's inception"), GossipItemHint("a forest blade")]
        [ShopText("A sword created by forest folk.")]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [HackContent(nameof(Resources.mods.fix_sword_song_of_time))]
        [GetItemIndex(0x37), ItemPool(ItemCategory.MainInventory, LocationCategory.StartingItems)]
        StartingSword,

        [Repeatable]
        [StartingItem(0xC5CE21, 0x10)]
        [ItemName("Hero's Shield"), LocationName("Starting Shield"), Region(Region.Misc)]
        [GossipLocationHint("a new file", "a quest's inception"), GossipItemHint("a basic guard", "protection")]
        [ShopText("Use it to defend yourself.")]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [GetItemIndex(0x129), ItemPool(ItemCategory.Shields, LocationCategory.StartingItems)]
        StartingShield,

        [StartingItem(0xC5CDE9, 0x10, true)] // add max health
        [StartingItem(0xC5CDEB, 0x10, true)] // add current health
        [StartingItem(0xC40E1B, 0x10, true)] // add respawn health
        [StartingItem(0xBDA683, 0x10, true)] // add minimum Song of Time health
        [StartingItem(0xBDA68F, 0x10, true)] // add minimum Song of Time health
        [ItemName("Heart Container"), LocationName("Starting Heart Container #1"), Region(Region.Misc)]
        [GossipLocationHint("a new file", "a quest's inception"), GossipItemHint("increased life")]
        [ShopText("Permanently increases your life energy.")]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [GetItemIndex(0x12A), ItemPool(ItemCategory.HeartContainers, LocationCategory.StartingItems)]
        StartingHeartContainer1,

        [StartingItem(0xC5CDE9, 0x10, true)] // add max health
        [StartingItem(0xC5CDEB, 0x10, true)] // add current health
        [StartingItem(0xC40E1B, 0x10, true)] // add respawn health
        [StartingItem(0xBDA683, 0x10, true)] // add minimum Song of Time health
        [StartingItem(0xBDA68F, 0x10, true)] // add minimum Song of Time health
        [ItemName("Heart Container"), LocationName("Starting Heart Container #2"), Region(Region.Misc)]
        [GossipLocationHint("a new file", "a quest's inception"), GossipItemHint("increased life")]
        [ShopText("Permanently increases your life energy.")]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [GetItemIndex(0x12B), ItemPool(ItemCategory.HeartContainers, LocationCategory.StartingItems)]
        StartingHeartContainer2,

        [Repeatable, Temporary, Overwritable]
        [ItemName("Milk"), LocationName("Ranch Cow #1"), Region(Region.RomaniRanch)]
        [GossipLocationHint("a lost creature"), GossipItemHint("a dairy product", "the produce of cows")]
        [ShopText("Recover five hearts with one drink. Contains two helpings.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x132), ItemPool(ItemCategory.Milk, LocationCategory.NpcRewards)]
        ItemRanchBarnMainCowMilk,

        [Repeatable, Temporary, Overwritable]
        [ItemName("Milk"), LocationName("Ranch Cow #2"), Region(Region.RomaniRanch)]
        [GossipLocationHint("a lost creature"), GossipItemHint("a dairy product", "the produce of cows")]
        [ShopText("Recover five hearts with one drink. Contains two helpings.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x182), ItemPool(ItemCategory.Milk, LocationCategory.NpcRewards)]
        ItemRanchBarnOtherCowMilk1,

        [Repeatable, Temporary, Overwritable]
        [ItemName("Milk"), LocationName("Ranch Cow #3"), Region(Region.RomaniRanch)]
        [GossipLocationHint("a lost creature"), GossipItemHint("a dairy product", "the produce of cows")]
        [ShopText("Recover five hearts with one drink. Contains two helpings.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x1A2), ItemPool(ItemCategory.Milk, LocationCategory.NpcRewards)]
        ItemRanchBarnOtherCowMilk2,

        [Repeatable, Temporary, Overwritable]
        [ItemName("Milk"), LocationName("Cow Beneath the Well"), Region(Region.BeneathTheWell)]
        [GossipLocationHint("a lost creature"), GossipItemHint("a dairy product", "the produce of cows")]
        [ShopText("Recover five hearts with one drink. Contains two helpings.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x135), ItemPool(ItemCategory.Milk, LocationCategory.NpcRewards)]
        ItemWellCowMilk,

        [Repeatable, Temporary, Overwritable]
        [ItemName("Milk"), LocationName("Termina Grotto Cow #1"), Region(Region.TerminaField)]
        [GossipLocationHint("a lost creature"), GossipItemHint("a dairy product", "the produce of cows")]
        [ShopText("Recover five hearts with one drink. Contains two helpings.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x136), ItemPool(ItemCategory.Milk, LocationCategory.NpcRewards)]
        ItemTerminaGrottoCowMilk1,

        [Repeatable, Temporary, Overwritable]
        [ItemName("Milk"), LocationName("Termina Grotto Cow #2"), Region(Region.TerminaField)]
        [GossipLocationHint("a lost creature"), GossipItemHint("a dairy product", "the produce of cows")]
        [ShopText("Recover five hearts with one drink. Contains two helpings.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x137), ItemPool(ItemCategory.Milk, LocationCategory.NpcRewards)]
        ItemTerminaGrottoCowMilk2,

        [Repeatable, Temporary, Overwritable]
        [ItemName("Milk"), LocationName("Great Bay Coast Grotto Cow #1"), Region(Region.GreatBayCoast)]
        [GossipLocationHint("a lost creature"), GossipItemHint("a dairy product", "the produce of cows")]
        [ShopText("Recover five hearts with one drink. Contains two helpings.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x138), ItemPool(ItemCategory.Milk, LocationCategory.NpcRewards)]
        ItemCoastGrottoCowMilk1,

        [Repeatable, Temporary, Overwritable]
        [ItemName("Milk"), LocationName("Great Bay Coast Grotto Cow #2"), Region(Region.GreatBayCoast)]
        [GossipLocationHint("a lost creature"), GossipItemHint("a dairy product", "the produce of cows")]
        [ShopText("Recover five hearts with one drink. Contains two helpings.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x139), ItemPool(ItemCategory.Milk, LocationCategory.NpcRewards)]
        ItemCoastGrottoCowMilk2,

        [Visible]
        [ItemName("Swamp Skulltula Spirit"), LocationName("Swamp Skulltula Main Room Near Ceiling"), Region(Region.SwampSpiderHouseItems)]
        [GossipLocationHint("a golden spider"), GossipItemHint("a golden token")]
        [ShopText("Collect 30 to lift the curse in the swamp spider house.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x13A), ItemPool(ItemCategory.SkulltulaTokens, LocationCategory.EnemySpawn)]
        CollectibleSwampSpiderToken1,

        [Visible]
        [ItemName("Swamp Skulltula Spirit"), LocationName("Swamp Skulltula Gold Room Near Ceiling"), Region(Region.SwampSpiderHouseItems)]
        [GossipLocationHint("a golden spider"), GossipItemHint("a golden token")]
        [ShopText("Collect 30 to lift the curse in the swamp spider house.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x13B), ItemPool(ItemCategory.SkulltulaTokens, LocationCategory.EnemySpawn)]
        CollectibleSwampSpiderToken2,

        [Visible]
        [ItemName("Swamp Skulltula Spirit"), LocationName("Swamp Skulltula Monument Room Torch"), Region(Region.SwampSpiderHouseItems)]
        [GossipLocationHint("a golden spider"), GossipItemHint("a golden token")]
        [ShopText("Collect 30 to lift the curse in the swamp spider house.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x13C), ItemPool(ItemCategory.SkulltulaTokens, LocationCategory.EnemySpawn)]
        CollectibleSwampSpiderToken3,

        [Visible]
        [ItemName("Swamp Skulltula Spirit"), LocationName("Swamp Skulltula Gold Room Pillar"), Region(Region.SwampSpiderHouseItems)]
        [GossipLocationHint("a golden spider"), GossipItemHint("a golden token")]
        [ShopText("Collect 30 to lift the curse in the swamp spider house.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x13E), ItemPool(ItemCategory.SkulltulaTokens, LocationCategory.EnemySpawn)]
        CollectibleSwampSpiderToken4,

        [Visible]
        [ItemName("Swamp Skulltula Spirit"), LocationName("Swamp Skulltula Pot Room Jar"), Region(Region.SwampSpiderHouseItems)]
        [GossipLocationHint("a golden spider"), GossipItemHint("a golden token")]
        [ShopText("Collect 30 to lift the curse in the swamp spider house.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x13F), ItemPool(ItemCategory.SkulltulaTokens, LocationCategory.Jars)]
        CollectibleSwampSpiderToken5,

        [Visible]
        [ItemName("Swamp Skulltula Spirit"), LocationName("Swamp Skulltula Tree Room Grass 1"), Region(Region.SwampSpiderHouseItems)]
        [GossipLocationHint("a golden spider"), GossipItemHint("a golden token")]
        [ShopText("Collect 30 to lift the curse in the swamp spider house.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x140), ItemPool(ItemCategory.SkulltulaTokens, LocationCategory.EnemySpawn)]
        CollectibleSwampSpiderToken6,

        [Visible]
        [ItemName("Swamp Skulltula Spirit"), LocationName("Swamp Skulltula Tree Room Grass 2"), Region(Region.SwampSpiderHouseItems)]
        [GossipLocationHint("a golden spider"), GossipItemHint("a golden token")]
        [ShopText("Collect 30 to lift the curse in the swamp spider house.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x141), ItemPool(ItemCategory.SkulltulaTokens, LocationCategory.EnemySpawn)]
        CollectibleSwampSpiderToken7,

        [Visible]
        [ItemName("Swamp Skulltula Spirit"), LocationName("Swamp Skulltula Main Room Water"), Region(Region.SwampSpiderHouseItems)]
        [GossipLocationHint("a golden spider"), GossipItemHint("a golden token")]
        [ShopText("Collect 30 to lift the curse in the swamp spider house.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x142), ItemPool(ItemCategory.SkulltulaTokens, LocationCategory.EnemySpawn)]
        CollectibleSwampSpiderToken8,

        [Visible]
        [ItemName("Swamp Skulltula Spirit"), LocationName("Swamp Skulltula Main Room Lower Left Soft Soil"), Region(Region.SwampSpiderHouseItems)]
        [GossipLocationHint("a golden spider"), GossipItemHint("a golden token")]
        [ShopText("Collect 30 to lift the curse in the swamp spider house.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x143), ItemPool(ItemCategory.SkulltulaTokens, LocationCategory.SoftSoil)]
        CollectibleSwampSpiderToken9,

        [Visible]
        [ItemName("Swamp Skulltula Spirit"), LocationName("Swamp Skulltula Monument Room Crate 1"), Region(Region.SwampSpiderHouseItems)]
        [GossipLocationHint("a golden spider"), GossipItemHint("a golden token")]
        [ShopText("Collect 30 to lift the curse in the swamp spider house.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x144), ItemPool(ItemCategory.SkulltulaTokens, LocationCategory.Crates)]
        CollectibleSwampSpiderToken10,

        [Visible]
        [ItemName("Swamp Skulltula Spirit"), LocationName("Swamp Skulltula Main Room Upper Soft Soil"), Region(Region.SwampSpiderHouseItems)]
        [GossipLocationHint("a golden spider"), GossipItemHint("a golden token")]
        [ShopText("Collect 30 to lift the curse in the swamp spider house.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x145), ItemPool(ItemCategory.SkulltulaTokens, LocationCategory.SoftSoil)]
        CollectibleSwampSpiderToken11,

        [Visible]
        [ItemName("Swamp Skulltula Spirit"), LocationName("Swamp Skulltula Main Room Lower Right Soft Soil"), Region(Region.SwampSpiderHouseItems)]
        [GossipLocationHint("a golden spider"), GossipItemHint("a golden token")]
        [ShopText("Collect 30 to lift the curse in the swamp spider house.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x146), ItemPool(ItemCategory.SkulltulaTokens, LocationCategory.SoftSoil)]
        CollectibleSwampSpiderToken12,

        [Visible]
        [ItemName("Swamp Skulltula Spirit"), LocationName("Swamp Skulltula Monument Room Lower Wall"), Region(Region.SwampSpiderHouseItems)]
        [GossipLocationHint("a golden spider"), GossipItemHint("a golden token")]
        [ShopText("Collect 30 to lift the curse in the swamp spider house.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x147), ItemPool(ItemCategory.SkulltulaTokens, LocationCategory.EnemySpawn)]
        CollectibleSwampSpiderToken13,

        [Visible]
        [ItemName("Swamp Skulltula Spirit"), LocationName("Swamp Skulltula Monument Room On Monument"), Region(Region.SwampSpiderHouseItems)]
        [GossipLocationHint("a golden spider"), GossipItemHint("a golden token")]
        [ShopText("Collect 30 to lift the curse in the swamp spider house.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x148), ItemPool(ItemCategory.SkulltulaTokens, LocationCategory.EnemySpawn)]
        CollectibleSwampSpiderToken14,

        [Visible]
        [ItemName("Swamp Skulltula Spirit"), LocationName("Swamp Skulltula Main Room Pillar"), Region(Region.SwampSpiderHouseItems)]
        [GossipLocationHint("a golden spider"), GossipItemHint("a golden token")]
        [ShopText("Collect 30 to lift the curse in the swamp spider house.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x149), ItemPool(ItemCategory.SkulltulaTokens, LocationCategory.EnemySpawn)]
        CollectibleSwampSpiderToken15,

        [Visible]
        [ItemName("Swamp Skulltula Spirit"), LocationName("Swamp Skulltula Pot Room Pot 1"), Region(Region.SwampSpiderHouseItems)]
        [GossipLocationHint("a golden spider"), GossipItemHint("a golden token")]
        [ShopText("Collect 30 to lift the curse in the swamp spider house.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x14A), ItemPool(ItemCategory.SkulltulaTokens, LocationCategory.EnemySpawn)]
        CollectibleSwampSpiderToken16,

        [Visible]
        [ItemName("Swamp Skulltula Spirit"), LocationName("Swamp Skulltula Pot Room Pot 2"), Region(Region.SwampSpiderHouseItems)]
        [GossipLocationHint("a golden spider"), GossipItemHint("a golden token")]
        [ShopText("Collect 30 to lift the curse in the swamp spider house.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x14B), ItemPool(ItemCategory.SkulltulaTokens, LocationCategory.EnemySpawn)]
        CollectibleSwampSpiderToken17,

        [Visible]
        [ItemName("Swamp Skulltula Spirit"), LocationName("Swamp Skulltula Gold Room Hive"), Region(Region.SwampSpiderHouseItems)]
        [GossipLocationHint("a golden spider"), GossipItemHint("a golden token")]
        [ShopText("Collect 30 to lift the curse in the swamp spider house.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x14C), ItemPool(ItemCategory.SkulltulaTokens, LocationCategory.Beehives)]
        CollectibleSwampSpiderToken18,

        [Visible]
        [ItemName("Swamp Skulltula Spirit"), LocationName("Swamp Skulltula Main Room Upper Pillar"), Region(Region.SwampSpiderHouseItems)]
        [GossipLocationHint("a golden spider"), GossipItemHint("a golden token")]
        [ShopText("Collect 30 to lift the curse in the swamp spider house.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x14D), ItemPool(ItemCategory.SkulltulaTokens, LocationCategory.EnemySpawn)]
        CollectibleSwampSpiderToken19,

        [Visible]
        [ItemName("Swamp Skulltula Spirit"), LocationName("Swamp Skulltula Pot Room Behind Vines"), Region(Region.SwampSpiderHouseItems)]
        [GossipLocationHint("a golden spider"), GossipItemHint("a golden token")]
        [ShopText("Collect 30 to lift the curse in the swamp spider house.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x14E), ItemPool(ItemCategory.SkulltulaTokens, LocationCategory.EnemySpawn)]
        CollectibleSwampSpiderToken20,

        [Visible]
        [ItemName("Swamp Skulltula Spirit"), LocationName("Swamp Skulltula Tree Room Tree 1"), Region(Region.SwampSpiderHouseItems)]
        [GossipLocationHint("a golden spider"), GossipItemHint("a golden token")]
        [ShopText("Collect 30 to lift the curse in the swamp spider house.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x14F), ItemPool(ItemCategory.SkulltulaTokens, LocationCategory.EnemySpawn)]
        CollectibleSwampSpiderToken21,

        [Visible]
        [ItemName("Swamp Skulltula Spirit"), LocationName("Swamp Skulltula Pot Room Wall"), Region(Region.SwampSpiderHouseItems)]
        [GossipLocationHint("a golden spider"), GossipItemHint("a golden token")]
        [ShopText("Collect 30 to lift the curse in the swamp spider house.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x150), ItemPool(ItemCategory.SkulltulaTokens, LocationCategory.EnemySpawn)]
        CollectibleSwampSpiderToken22,

        [Visible]
        [ItemName("Swamp Skulltula Spirit"), LocationName("Swamp Skulltula Pot Room Hive 1"), Region(Region.SwampSpiderHouseItems)]
        [GossipLocationHint("a golden spider"), GossipItemHint("a golden token")]
        [ShopText("Collect 30 to lift the curse in the swamp spider house.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x151), ItemPool(ItemCategory.SkulltulaTokens, LocationCategory.Beehives)]
        CollectibleSwampSpiderToken23,

        [Visible]
        [ItemName("Swamp Skulltula Spirit"), LocationName("Swamp Skulltula Tree Room Tree 2"), Region(Region.SwampSpiderHouseItems)]
        [GossipLocationHint("a golden spider"), GossipItemHint("a golden token")]
        [ShopText("Collect 30 to lift the curse in the swamp spider house.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x152), ItemPool(ItemCategory.SkulltulaTokens, LocationCategory.EnemySpawn)]
        CollectibleSwampSpiderToken24,

        [Visible]
        [ItemName("Swamp Skulltula Spirit"), LocationName("Swamp Skulltula Gold Room Wall"), Region(Region.SwampSpiderHouseItems)]
        [GossipLocationHint("a golden spider"), GossipItemHint("a golden token")]
        [ShopText("Collect 30 to lift the curse in the swamp spider house.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x153), ItemPool(ItemCategory.SkulltulaTokens, LocationCategory.EnemySpawn)]
        CollectibleSwampSpiderToken25,

        [Visible]
        [ItemName("Swamp Skulltula Spirit"), LocationName("Swamp Skulltula Tree Room Hive"), Region(Region.SwampSpiderHouseItems)]
        [GossipLocationHint("a golden spider"), GossipItemHint("a golden token")]
        [ShopText("Collect 30 to lift the curse in the swamp spider house.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x154), ItemPool(ItemCategory.SkulltulaTokens, LocationCategory.Beehives)]
        CollectibleSwampSpiderToken26,

        [Visible]
        [ItemName("Swamp Skulltula Spirit"), LocationName("Swamp Skulltula Monument Room Crate 2"), Region(Region.SwampSpiderHouseItems)]
        [GossipLocationHint("a golden spider"), GossipItemHint("a golden token")]
        [ShopText("Collect 30 to lift the curse in the swamp spider house.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x155), ItemPool(ItemCategory.SkulltulaTokens, LocationCategory.Crates)]
        CollectibleSwampSpiderToken27,

        [Visible]
        [ItemName("Swamp Skulltula Spirit"), LocationName("Swamp Skulltula Pot Room Hive 2"), Region(Region.SwampSpiderHouseItems)]
        [GossipLocationHint("a golden spider"), GossipItemHint("a golden token")]
        [ShopText("Collect 30 to lift the curse in the swamp spider house.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x156), ItemPool(ItemCategory.SkulltulaTokens, LocationCategory.Beehives)]
        CollectibleSwampSpiderToken28,

        [Visible]
        [ItemName("Swamp Skulltula Spirit"), LocationName("Swamp Skulltula Tree Room Tree 3"), Region(Region.SwampSpiderHouseItems)]
        [GossipLocationHint("a golden spider"), GossipItemHint("a golden token")]
        [ShopText("Collect 30 to lift the curse in the swamp spider house.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x157), ItemPool(ItemCategory.SkulltulaTokens, LocationCategory.EnemySpawn)]
        CollectibleSwampSpiderToken29,

        [Visible]
        [ItemName("Swamp Skulltula Spirit"), LocationName("Swamp Skulltula Main Room Jar"), Region(Region.SwampSpiderHouseItems)]
        [GossipLocationHint("a golden spider"), GossipItemHint("a golden token")]
        [ShopText("Collect 30 to lift the curse in the swamp spider house.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x158), ItemPool(ItemCategory.SkulltulaTokens, LocationCategory.Jars)]
        CollectibleSwampSpiderToken30,

        [Visible]
        [ItemName("Ocean Skulltula Spirit"), LocationName("Ocean Skulltula Storage Room Behind Boat"), Region(Region.OceanSpiderHouseItems)]
        [GossipLocationHint("a golden spider"), GossipItemHint("a golden token")]
        [ShopText("Collect 30 to lift the curse in the ocean spider house.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x159), ItemPool(ItemCategory.SkulltulaTokens, LocationCategory.EnemySpawn)]
        CollectibleOceanSpiderToken1,

        [Visible]
        [ItemName("Ocean Skulltula Spirit"), LocationName("Ocean Skulltula Library Hole Behind Picture"), Region(Region.OceanSpiderHouseItems)]
        [GossipLocationHint("a golden spider"), GossipItemHint("a golden token")]
        [ShopText("Collect 30 to lift the curse in the ocean spider house.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x15A), ItemPool(ItemCategory.SkulltulaTokens, LocationCategory.EnemySpawn)]
        CollectibleOceanSpiderToken2,

        [Visible]
        [ItemName("Ocean Skulltula Spirit"), LocationName("Ocean Skulltula Library Hole Behind Cabinet"), Region(Region.OceanSpiderHouseItems)]
        [GossipLocationHint("a golden spider"), GossipItemHint("a golden token")]
        [ShopText("Collect 30 to lift the curse in the ocean spider house.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x15B), ItemPool(ItemCategory.SkulltulaTokens, LocationCategory.EnemySpawn)]
        CollectibleOceanSpiderToken3,

        [Visible]
        [ItemName("Ocean Skulltula Spirit"), LocationName("Ocean Skulltula Library On Corner Bookshelf"), Region(Region.OceanSpiderHouseItems)]
        [GossipLocationHint("a golden spider"), GossipItemHint("a golden token")]
        [ShopText("Collect 30 to lift the curse in the ocean spider house.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x15C), ItemPool(ItemCategory.SkulltulaTokens, LocationCategory.EnemySpawn)]
        CollectibleOceanSpiderToken4,

        [Visible]
        [ItemName("Ocean Skulltula Spirit"), LocationName("Ocean Skulltula 2nd Room Ceiling Edge"), Region(Region.OceanSpiderHouseItems)]
        [GossipLocationHint("a golden spider"), GossipItemHint("a golden token")]
        [ShopText("Collect 30 to lift the curse in the ocean spider house.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x15D), ItemPool(ItemCategory.SkulltulaTokens, LocationCategory.EnemySpawn)]
        CollectibleOceanSpiderToken5,

        [Visible]
        [ItemName("Ocean Skulltula Spirit"), LocationName("Ocean Skulltula 2nd Room Ceiling Plank"), Region(Region.OceanSpiderHouseItems)]
        [GossipLocationHint("a golden spider"), GossipItemHint("a golden token")]
        [ShopText("Collect 30 to lift the curse in the ocean spider house.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x15E), ItemPool(ItemCategory.SkulltulaTokens, LocationCategory.EnemySpawn)]
        CollectibleOceanSpiderToken6,

        [Visible]
        [ItemName("Ocean Skulltula Spirit"), LocationName("Ocean Skulltula Colored Skulls Ceiling Edge"), Region(Region.OceanSpiderHouseItems)]
        [GossipLocationHint("a golden spider"), GossipItemHint("a golden token")]
        [ShopText("Collect 30 to lift the curse in the ocean spider house.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x15F), ItemPool(ItemCategory.SkulltulaTokens, LocationCategory.EnemySpawn)]
        CollectibleOceanSpiderToken7,

        [Visible]
        [ItemName("Ocean Skulltula Spirit"), LocationName("Ocean Skulltula Library Ceiling Edge"), Region(Region.OceanSpiderHouseItems)]
        [GossipLocationHint("a golden spider"), GossipItemHint("a golden token")]
        [ShopText("Collect 30 to lift the curse in the ocean spider house.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x160), ItemPool(ItemCategory.SkulltulaTokens, LocationCategory.EnemySpawn)]
        CollectibleOceanSpiderToken8,

        [Visible]
        [ItemName("Ocean Skulltula Spirit"), LocationName("Ocean Skulltula Storage Room Ceiling Web"), Region(Region.OceanSpiderHouseItems)]
        [GossipLocationHint("a golden spider"), GossipItemHint("a golden token")]
        [ShopText("Collect 30 to lift the curse in the ocean spider house.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x161), ItemPool(ItemCategory.SkulltulaTokens, LocationCategory.EnemySpawn)]
        CollectibleOceanSpiderToken9,

        [Visible]
        [ItemName("Ocean Skulltula Spirit"), LocationName("Ocean Skulltula Storage Room Behind Crate"), Region(Region.OceanSpiderHouseItems)]
        [GossipLocationHint("a golden spider"), GossipItemHint("a golden token")]
        [ShopText("Collect 30 to lift the curse in the ocean spider house.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x162), ItemPool(ItemCategory.SkulltulaTokens, LocationCategory.EnemySpawn)]
        CollectibleOceanSpiderToken10,

        [Visible]
        [ItemName("Ocean Skulltula Spirit"), LocationName("Ocean Skulltula 2nd Room Jar"), Region(Region.OceanSpiderHouseItems)]
        [GossipLocationHint("a golden spider"), GossipItemHint("a golden token")]
        [ShopText("Collect 30 to lift the curse in the ocean spider house.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x163), ItemPool(ItemCategory.SkulltulaTokens, LocationCategory.Jars)]
        CollectibleOceanSpiderToken11,

        [Visible]
        [ItemName("Ocean Skulltula Spirit"), LocationName("Ocean Skulltula Entrance Right Wall"), Region(Region.OceanSpiderHouseItems)]
        [GossipLocationHint("a golden spider"), GossipItemHint("a golden token")]
        [ShopText("Collect 30 to lift the curse in the ocean spider house.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x164), ItemPool(ItemCategory.SkulltulaTokens, LocationCategory.EnemySpawn)]
        CollectibleOceanSpiderToken12,

        [Visible]
        [ItemName("Ocean Skulltula Spirit"), LocationName("Ocean Skulltula Entrance Left Wall"), Region(Region.OceanSpiderHouseItems)]
        [GossipLocationHint("a golden spider"), GossipItemHint("a golden token")]
        [ShopText("Collect 30 to lift the curse in the ocean spider house.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x165), ItemPool(ItemCategory.SkulltulaTokens, LocationCategory.EnemySpawn)]
        CollectibleOceanSpiderToken13,

        [Visible]
        [ItemName("Ocean Skulltula Spirit"), LocationName("Ocean Skulltula 2nd Room Webbed Hole"), Region(Region.OceanSpiderHouseItems)]
        [GossipLocationHint("a golden spider"), GossipItemHint("a golden token")]
        [ShopText("Collect 30 to lift the curse in the ocean spider house.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x166), ItemPool(ItemCategory.SkulltulaTokens, LocationCategory.EnemySpawn)]
        CollectibleOceanSpiderToken14,

        [Visible]
        [ItemName("Ocean Skulltula Spirit"), LocationName("Ocean Skulltula Entrance Web"), Region(Region.OceanSpiderHouseItems)]
        [GossipLocationHint("a golden spider"), GossipItemHint("a golden token")]
        [ShopText("Collect 30 to lift the curse in the ocean spider house.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x167), ItemPool(ItemCategory.SkulltulaTokens, LocationCategory.EnemySpawn)]
        CollectibleOceanSpiderToken15,

        [Visible]
        [ItemName("Ocean Skulltula Spirit"), LocationName("Ocean Skulltula Colored Skulls Chandelier 1"), Region(Region.OceanSpiderHouseItems)]
        [GossipLocationHint("a golden spider"), GossipItemHint("a golden token")]
        [ShopText("Collect 30 to lift the curse in the ocean spider house.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x168), ItemPool(ItemCategory.SkulltulaTokens, LocationCategory.EnemySpawn)]
        CollectibleOceanSpiderToken16,

        [Visible]
        [ItemName("Ocean Skulltula Spirit"), LocationName("Ocean Skulltula Colored Skulls Chandelier 2"), Region(Region.OceanSpiderHouseItems)]
        [GossipLocationHint("a golden spider"), GossipItemHint("a golden token")]
        [ShopText("Collect 30 to lift the curse in the ocean spider house.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x169), ItemPool(ItemCategory.SkulltulaTokens, LocationCategory.EnemySpawn)]
        CollectibleOceanSpiderToken17,

        [Visible]
        [ItemName("Ocean Skulltula Spirit"), LocationName("Ocean Skulltula Colored Skulls Chandelier 3"), Region(Region.OceanSpiderHouseItems)]
        [GossipLocationHint("a golden spider"), GossipItemHint("a golden token")]
        [ShopText("Collect 30 to lift the curse in the ocean spider house.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x16A), ItemPool(ItemCategory.SkulltulaTokens, LocationCategory.EnemySpawn)]
        CollectibleOceanSpiderToken18,

        [Visible]
        [ItemName("Ocean Skulltula Spirit"), LocationName("Ocean Skulltula Colored Skulls Behind Picture"), Region(Region.OceanSpiderHouseItems)]
        [GossipLocationHint("a golden spider"), GossipItemHint("a golden token")]
        [ShopText("Collect 30 to lift the curse in the ocean spider house.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x16B), ItemPool(ItemCategory.SkulltulaTokens, LocationCategory.EnemySpawn)]
        CollectibleOceanSpiderToken19,

        [Visible]
        [ItemName("Ocean Skulltula Spirit"), LocationName("Ocean Skulltula Library Behind Picture"), Region(Region.OceanSpiderHouseItems)]
        [GossipLocationHint("a golden spider"), GossipItemHint("a golden token")]
        [ShopText("Collect 30 to lift the curse in the ocean spider house.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x16C), ItemPool(ItemCategory.SkulltulaTokens, LocationCategory.EnemySpawn)]
        CollectibleOceanSpiderToken20,

        [Visible]
        [ItemName("Ocean Skulltula Spirit"), LocationName("Ocean Skulltula Library Behind Bookcase 1"), Region(Region.OceanSpiderHouseItems)]
        [GossipLocationHint("a golden spider"), GossipItemHint("a golden token")]
        [ShopText("Collect 30 to lift the curse in the ocean spider house.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x16D), ItemPool(ItemCategory.SkulltulaTokens, LocationCategory.EnemySpawn)]
        CollectibleOceanSpiderToken21,

        [Visible]
        [ItemName("Ocean Skulltula Spirit"), LocationName("Ocean Skulltula Storage Room Crate"), Region(Region.OceanSpiderHouseItems)]
        [GossipLocationHint("a golden spider"), GossipItemHint("a golden token")]
        [ShopText("Collect 30 to lift the curse in the ocean spider house.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x16E), ItemPool(ItemCategory.SkulltulaTokens, LocationCategory.Crates)]
        CollectibleOceanSpiderToken22,

        [Visible]
        [ItemName("Ocean Skulltula Spirit"), LocationName("Ocean Skulltula 2nd Room Webbed Pot"), Region(Region.OceanSpiderHouseItems)]
        [GossipLocationHint("a golden spider"), GossipItemHint("a golden token")]
        [ShopText("Collect 30 to lift the curse in the ocean spider house.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x16F), ItemPool(ItemCategory.SkulltulaTokens, LocationCategory.EnemySpawn)]
        CollectibleOceanSpiderToken23,

        [Visible]
        [ItemName("Ocean Skulltula Spirit"), LocationName("Ocean Skulltula 2nd Room Upper Pot"), Region(Region.OceanSpiderHouseItems)]
        [GossipLocationHint("a golden spider"), GossipItemHint("a golden token")]
        [ShopText("Collect 30 to lift the curse in the ocean spider house.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x170), ItemPool(ItemCategory.SkulltulaTokens, LocationCategory.EnemySpawn)]
        CollectibleOceanSpiderToken24,

        [Visible]
        [ItemName("Ocean Skulltula Spirit"), LocationName("Ocean Skulltula Colored Skulls Pot"), Region(Region.OceanSpiderHouseItems)]
        [GossipLocationHint("a golden spider"), GossipItemHint("a golden token")]
        [ShopText("Collect 30 to lift the curse in the ocean spider house.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x171), ItemPool(ItemCategory.SkulltulaTokens, LocationCategory.EnemySpawn)]
        CollectibleOceanSpiderToken25,

        [Visible]
        [ItemName("Ocean Skulltula Spirit"), LocationName("Ocean Skulltula Storage Room Jar"), Region(Region.OceanSpiderHouseItems)]
        [GossipLocationHint("a golden spider"), GossipItemHint("a golden token")]
        [ShopText("Collect 30 to lift the curse in the ocean spider house.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x172), ItemPool(ItemCategory.SkulltulaTokens, LocationCategory.Jars)]
        CollectibleOceanSpiderToken26,

        [Visible]
        [ItemName("Ocean Skulltula Spirit"), LocationName("Ocean Skulltula 2nd Room Lower Pot"), Region(Region.OceanSpiderHouseItems)]
        [GossipLocationHint("a golden spider"), GossipItemHint("a golden token")]
        [ShopText("Collect 30 to lift the curse in the ocean spider house.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x173), ItemPool(ItemCategory.SkulltulaTokens, LocationCategory.EnemySpawn)]
        CollectibleOceanSpiderToken27,

        [Visible]
        [ItemName("Ocean Skulltula Spirit"), LocationName("Ocean Skulltula Library Behind Bookcase 2"), Region(Region.OceanSpiderHouseItems)]
        [GossipLocationHint("a golden spider"), GossipItemHint("a golden token")]
        [ShopText("Collect 30 to lift the curse in the ocean spider house.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x174), ItemPool(ItemCategory.SkulltulaTokens, LocationCategory.EnemySpawn)]
        CollectibleOceanSpiderToken28,

        [Visible]
        [ItemName("Ocean Skulltula Spirit"), LocationName("Ocean Skulltula 2nd Room Behind Skull 1"), Region(Region.OceanSpiderHouseItems)]
        [GossipLocationHint("a golden spider"), GossipItemHint("a golden token")]
        [ShopText("Collect 30 to lift the curse in the ocean spider house.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x175), ItemPool(ItemCategory.SkulltulaTokens, LocationCategory.EnemySpawn)]
        CollectibleOceanSpiderToken29,

        [Visible]
        [ItemName("Ocean Skulltula Spirit"), LocationName("Ocean Skulltula 2nd Room Behind Skull 2"), Region(Region.OceanSpiderHouseItems)]
        [GossipLocationHint("a golden spider"), GossipItemHint("a golden token")]
        [ShopText("Collect 30 to lift the curse in the ocean spider house.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x176), ItemPool(ItemCategory.SkulltulaTokens, LocationCategory.EnemySpawn)]
        CollectibleOceanSpiderToken30,

        [Visible]
        [ItemName("Clock Town Stray Fairy"), LocationName("Clock Town Stray Fairy"), MultiLocation(CollectibleStrayFairyClockTownInLaundryPool, CollectibleStrayFairyClockTownInECT)]
        [GossipLocationHint("a lost creature"), GossipItemHint("a lost fairy")]
        [ShopText("Return it to the Fairy Fountain in North Clock Town.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x3B), ItemPool(ItemCategory.StrayFairies, LocationCategory.Freestanding)]
        //[GetItemIndex(0x1A1)] // used as a flag to track if the actual fairy has been collected.
        CollectibleStrayFairyClockTown,

        [Visible]
        [ItemName("Woodfall Stray Fairy"), LocationName("Woodfall Pre-Boss Lower Right Bubble"), Region(Region.WoodfallTemple)]
        [GossipLocationHint("a lost creature"), GossipItemHint("a lost fairy")]
        [ShopText("Collect 15 and return them to the Fairy Fountain in Woodfall.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x177), ItemPool(ItemCategory.StrayFairies, LocationCategory.Freestanding)]
        CollectibleStrayFairyWoodfall1,

        [Visible]
        [ItemName("Woodfall Stray Fairy"), LocationName("Woodfall Entrance Fairy"), Region(Region.WoodfallTemple)]
        [GossipLocationHint("a lost creature"), GossipItemHint("a lost fairy")]
        [ShopText("Collect 15 and return them to the Fairy Fountain in Woodfall.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x178), ItemPool(ItemCategory.StrayFairies, LocationCategory.Freestanding)]
        CollectibleStrayFairyWoodfall2,

        [Visible]
        [ItemName("Woodfall Stray Fairy"), LocationName("Woodfall Pre-Boss Upper Left Bubble"), Region(Region.WoodfallTemple)]
        [GossipLocationHint("a lost creature"), GossipItemHint("a lost fairy")]
        [ShopText("Collect 15 and return them to the Fairy Fountain in Woodfall.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x179), ItemPool(ItemCategory.StrayFairies, LocationCategory.Freestanding)]
        CollectibleStrayFairyWoodfall3,

        [Visible]
        [ItemName("Woodfall Stray Fairy"), LocationName("Woodfall Pre-Boss Pillar Bubble"), Region(Region.WoodfallTemple)]
        [GossipLocationHint("a lost creature"), GossipItemHint("a lost fairy")]
        [ShopText("Collect 15 and return them to the Fairy Fountain in Woodfall.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x17A), ItemPool(ItemCategory.StrayFairies, LocationCategory.Freestanding)]
        CollectibleStrayFairyWoodfall4,

        [Visible]
        [ItemName("Woodfall Stray Fairy"), LocationName("Woodfall Deku Baba"), Region(Region.WoodfallTemple)]
        [GossipLocationHint("a lost creature"), GossipItemHint("a lost fairy")]
        [ShopText("Collect 15 and return them to the Fairy Fountain in Woodfall.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x17B), ItemPool(ItemCategory.StrayFairies, LocationCategory.EnemySpawn)]
        CollectibleStrayFairyWoodfall5,

        [Visible]
        [ItemName("Woodfall Stray Fairy"), LocationName("Woodfall Poison Water Bubble"), Region(Region.WoodfallTemple)]
        [GossipLocationHint("a lost creature"), GossipItemHint("a lost fairy")]
        [ShopText("Collect 15 and return them to the Fairy Fountain in Woodfall.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x17C), ItemPool(ItemCategory.StrayFairies, LocationCategory.Freestanding)]
        CollectibleStrayFairyWoodfall6,

        [Visible]
        [ItemName("Woodfall Stray Fairy"), LocationName("Woodfall Main Room Bubble"), Region(Region.WoodfallTemple)]
        [GossipLocationHint("a lost creature"), GossipItemHint("a lost fairy")]
        [ShopText("Collect 15 and return them to the Fairy Fountain in Woodfall.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x17D), ItemPool(ItemCategory.StrayFairies, LocationCategory.Freestanding)]
        CollectibleStrayFairyWoodfall7,

        [Visible]
        [ItemName("Woodfall Stray Fairy"), LocationName("Woodfall Skulltula"), Region(Region.WoodfallTemple)]
        [GossipLocationHint("a lost creature"), GossipItemHint("a lost fairy")]
        [ShopText("Collect 15 and return them to the Fairy Fountain in Woodfall.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x17E), ItemPool(ItemCategory.StrayFairies, LocationCategory.EnemySpawn)]
        CollectibleStrayFairyWoodfall8,

        [Visible]
        [ItemName("Woodfall Stray Fairy"), LocationName("Woodfall Pre-Boss Upper Right Bubble"), Region(Region.WoodfallTemple)]
        [GossipLocationHint("a lost creature"), GossipItemHint("a lost fairy")]
        [ShopText("Collect 15 and return them to the Fairy Fountain in Woodfall.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x17F), ItemPool(ItemCategory.StrayFairies, LocationCategory.Freestanding)]
        CollectibleStrayFairyWoodfall9,

        [Visible]
        [ItemName("Woodfall Stray Fairy"), LocationName("Woodfall Main Room Switch"), Region(Region.WoodfallTemple)]
        [GossipLocationHint("a lost creature"), GossipItemHint("a lost fairy")]
        [ShopText("Collect 15 and return them to the Fairy Fountain in Woodfall.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold), Chest(0x021FB000 + 0x28A, ChestAttribute.AppearanceType.AppearsSwitch)]
        [GetItemIndex(0x184), ItemPool(ItemCategory.StrayFairies, LocationCategory.Chests)]
        CollectibleStrayFairyWoodfall10,

        [Visible]
        [ItemName("Woodfall Stray Fairy"), LocationName("Woodfall Entrance Platform"), Region(Region.WoodfallTemple)]
        [GossipLocationHint("a lost creature"), GossipItemHint("a lost fairy")]
        [ShopText("Collect 15 and return them to the Fairy Fountain in Woodfall.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold), Chest(0x02204000 + 0x23A, ChestAttribute.AppearanceType.Normal)]
        [GetItemIndex(0x185), ItemPool(ItemCategory.StrayFairies, LocationCategory.Chests)]
        CollectibleStrayFairyWoodfall11,

        [Visible]
        [ItemName("Woodfall Stray Fairy"), LocationName("Woodfall Dark Room"), Region(Region.WoodfallTemple)]
        [GossipLocationHint("a lost creature"), GossipItemHint("a lost fairy")]
        [ShopText("Collect 15 and return them to the Fairy Fountain in Woodfall.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold), Chest(0x0222E000 + 0x1AA, ChestAttribute.AppearanceType.AppearsClear)]
        [GetItemIndex(0x186), ItemPool(ItemCategory.StrayFairies, LocationCategory.Chests)]
        CollectibleStrayFairyWoodfall12,

        [Visible]
        [ItemName("Woodfall Stray Fairy"), LocationName("Woodfall Jar Fairy"), Region(Region.WoodfallTemple)]
        [GossipLocationHint("a lost creature"), GossipItemHint("a lost fairy")]
        [ShopText("Collect 15 and return them to the Fairy Fountain in Woodfall.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x189), ItemPool(ItemCategory.StrayFairies, LocationCategory.Jars)]
        CollectibleStrayFairyWoodfall13,

        [Visible]
        [ItemName("Woodfall Stray Fairy"), LocationName("Woodfall Bridge Room Hive"), Region(Region.WoodfallTemple)]
        [GossipLocationHint("a lost creature"), GossipItemHint("a lost fairy")]
        [ShopText("Collect 15 and return them to the Fairy Fountain in Woodfall.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x18A), ItemPool(ItemCategory.StrayFairies, LocationCategory.Beehives)]
        CollectibleStrayFairyWoodfall14,

        [Visible]
        [ItemName("Woodfall Stray Fairy"), LocationName("Woodfall Platform Room Hive"), Region(Region.WoodfallTemple)]
        [GossipLocationHint("a lost creature"), GossipItemHint("a lost fairy")]
        [ShopText("Collect 15 and return them to the Fairy Fountain in Woodfall.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x18B), ItemPool(ItemCategory.StrayFairies, LocationCategory.Beehives)]
        CollectibleStrayFairyWoodfall15,

        [Visible]
        [ItemName("Snowhead Stray Fairy"), LocationName("Snowhead Snow Room Bubble"), Region(Region.SnowheadTemple)]
        [GossipLocationHint("a lost creature"), GossipItemHint("a lost fairy")]
        [ShopText("Collect 15 and return them to the Fairy Fountain in Snowhead.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x18C), ItemPool(ItemCategory.StrayFairies, LocationCategory.Freestanding)]
        CollectibleStrayFairySnowhead1,

        [Visible]
        [ItemName("Snowhead Stray Fairy"), LocationName("Snowhead Ceiling Bubble"), Region(Region.SnowheadTemple)]
        [GossipLocationHint("a lost creature"), GossipItemHint("a lost fairy")]
        [ShopText("Collect 15 and return them to the Fairy Fountain in Snowhead.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x18D), ItemPool(ItemCategory.StrayFairies, LocationCategory.Freestanding)]
        CollectibleStrayFairySnowhead2,

        [Visible]
        [ItemName("Snowhead Stray Fairy"), LocationName("Snowhead Dinolfos 1"), Region(Region.SnowheadTemple)]
        [GossipLocationHint("a lost creature"), GossipItemHint("a lost fairy")]
        [ShopText("Collect 15 and return them to the Fairy Fountain in Snowhead.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x18E), ItemPool(ItemCategory.StrayFairies, LocationCategory.EnemySpawn)]
        CollectibleStrayFairySnowhead3,

        [Visible]
        [ItemName("Snowhead Stray Fairy"), LocationName("Snowhead Bridge Room Ledge Bubble"), Region(Region.SnowheadTemple)]
        [GossipLocationHint("a lost creature"), GossipItemHint("a lost fairy")]
        [ShopText("Collect 15 and return them to the Fairy Fountain in Snowhead.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x190), ItemPool(ItemCategory.StrayFairies, LocationCategory.Freestanding)]
        CollectibleStrayFairySnowhead4,

        [Visible]
        [ItemName("Snowhead Stray Fairy"), LocationName("Snowhead Bridge Room Pillar Bubble"), Region(Region.SnowheadTemple)]
        [GossipLocationHint("a lost creature"), GossipItemHint("a lost fairy")]
        [ShopText("Collect 15 and return them to the Fairy Fountain in Snowhead.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x191), ItemPool(ItemCategory.StrayFairies, LocationCategory.Freestanding)]
        CollectibleStrayFairySnowhead5,

        [Visible]
        [ItemName("Snowhead Stray Fairy"), LocationName("Snowhead Dinolfos 2"), Region(Region.SnowheadTemple)]
        [GossipLocationHint("a lost creature"), GossipItemHint("a lost fairy")]
        [ShopText("Collect 15 and return them to the Fairy Fountain in Snowhead.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x192), ItemPool(ItemCategory.StrayFairies, LocationCategory.EnemySpawn)]
        CollectibleStrayFairySnowhead6,

        [Visible]
        [ItemName("Snowhead Stray Fairy"), LocationName("Snowhead Map Room Fairy"), Region(Region.SnowheadTemple)]
        [GossipLocationHint("a lost creature"), GossipItemHint("a lost fairy")]
        [ShopText("Collect 15 and return them to the Fairy Fountain in Snowhead.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x193), ItemPool(ItemCategory.StrayFairies, LocationCategory.Freestanding)]
        CollectibleStrayFairySnowhead7,

        [Visible]
        [ItemName("Snowhead Stray Fairy"), LocationName("Snowhead Map Room Ledge"), Region(Region.SnowheadTemple)]
        [GossipLocationHint("a lost creature"), GossipItemHint("a lost fairy")]
        [ShopText("Collect 15 and return them to the Fairy Fountain in Snowhead.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold), Chest(0x02346000 + 0x12A, ChestAttribute.AppearanceType.Normal)]
        [GetItemIndex(0x194), ItemPool(ItemCategory.StrayFairies, LocationCategory.Chests)]
        CollectibleStrayFairySnowhead8,

        [Visible]
        [ItemName("Snowhead Stray Fairy"), LocationName("Snowhead Basement"), Region(Region.SnowheadTemple)]
        [GossipLocationHint("a lost creature"), GossipItemHint("a lost fairy")]
        [ShopText("Collect 15 and return them to the Fairy Fountain in Snowhead.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold), Chest(0x0230C000 + 0x56A, ChestAttribute.AppearanceType.AppearsSwitch)]
        [GetItemIndex(0x195), ItemPool(ItemCategory.StrayFairies, LocationCategory.Chests)]
        CollectibleStrayFairySnowhead9,

        [Visible]
        [ItemName("Snowhead Stray Fairy"), LocationName("Snowhead Twin Block"), Region(Region.SnowheadTemple)]
        [GossipLocationHint("a lost creature"), GossipItemHint("a lost fairy")]
        [ShopText("Collect 15 and return them to the Fairy Fountain in Snowhead.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold), Chest(0x02306000 + 0x11A, ChestAttribute.AppearanceType.AppearsSwitch)]
        [GetItemIndex(0x196), ItemPool(ItemCategory.StrayFairies, LocationCategory.Chests)]
        CollectibleStrayFairySnowhead10,

        [Visible]
        [ItemName("Snowhead Stray Fairy"), LocationName("Snowhead Icicle Room Wall"), Region(Region.SnowheadTemple)]
        [GossipLocationHint("a lost creature"), GossipItemHint("a lost fairy")]
        [ShopText("Collect 15 and return them to the Fairy Fountain in Snowhead.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold), Chest(0x0233A000 + 0x22A, ChestAttribute.AppearanceType.Normal)]
        [GetItemIndex(0x197), ItemPool(ItemCategory.StrayFairies, LocationCategory.Chests)]
        CollectibleStrayFairySnowhead11,

        [Visible]
        [ItemName("Snowhead Stray Fairy"), LocationName("Snowhead Main Room Wall"), Region(Region.SnowheadTemple)]
        [GossipLocationHint("a lost creature"), GossipItemHint("a lost fairy")]
        [ShopText("Collect 15 and return them to the Fairy Fountain in Snowhead.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold), Chest(0x0230C000 + 0x58A, ChestAttribute.AppearanceType.Normal)]
        [GetItemIndex(0x198), ItemPool(ItemCategory.StrayFairies, LocationCategory.Chests)]
        CollectibleStrayFairySnowhead12,

        [Visible]
        [ItemName("Snowhead Stray Fairy"), LocationName("Snowhead Pillar Freezards"), Region(Region.SnowheadTemple)]
        [GossipLocationHint("a lost creature"), GossipItemHint("a lost fairy")]
        [ShopText("Collect 15 and return them to the Fairy Fountain in Snowhead.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold), Chest(0x0232E000 + 0x20A, ChestAttribute.AppearanceType.AppearsClear)]
        [GetItemIndex(0x199), ItemPool(ItemCategory.StrayFairies, LocationCategory.Chests)]
        CollectibleStrayFairySnowhead13,

        [Visible]
        [ItemName("Snowhead Stray Fairy"), LocationName("Snowhead Ice Puzzle"), Region(Region.SnowheadTemple)]
        [GossipLocationHint("a lost creature"), GossipItemHint("a lost fairy")]
        [ShopText("Collect 15 and return them to the Fairy Fountain in Snowhead.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold), Chest(0x022F2000 + 0x1AA, ChestAttribute.AppearanceType.AppearsSwitch)]
        [GetItemIndex(0x19A), ItemPool(ItemCategory.StrayFairies, LocationCategory.Chests)]
        CollectibleStrayFairySnowhead14,

        [Visible]
        [ItemName("Snowhead Stray Fairy"), LocationName("Snowhead Crate"), Region(Region.SnowheadTemple)]
        [GossipLocationHint("a lost creature"), GossipItemHint("a lost fairy")]
        [ShopText("Collect 15 and return them to the Fairy Fountain in Snowhead.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x19F), ItemPool(ItemCategory.StrayFairies, LocationCategory.Crates)]
        CollectibleStrayFairySnowhead15,

        [Visible]
        [ItemName("Great Bay Stray Fairy"), LocationName("Great Bay Skulltula"), Region(Region.GreatBayTemple)]
        [GossipLocationHint("a lost creature"), GossipItemHint("a lost fairy")]
        [ShopText("Collect 15 and return them to the Fairy Fountain in Great Bay.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x1A7), ItemPool(ItemCategory.StrayFairies, LocationCategory.EnemySpawn)]
        CollectibleStrayFairyGreatBay1,

        [Visible]
        [ItemName("Great Bay Stray Fairy"), LocationName("Great Bay Pre-Boss Room Underwater Bubble"), Region(Region.GreatBayTemple)]
        [GossipLocationHint("a lost creature"), GossipItemHint("a lost fairy")]
        [ShopText("Collect 15 and return them to the Fairy Fountain in Great Bay.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x1A4), ItemPool(ItemCategory.StrayFairies, LocationCategory.Freestanding)]
        CollectibleStrayFairyGreatBay2,

        [Visible]
        [ItemName("Great Bay Stray Fairy"), LocationName("Great Bay Water Control Room Underwater Bubble"), Region(Region.GreatBayTemple)]
        [GossipLocationHint("a lost creature"), GossipItemHint("a lost fairy")]
        [ShopText("Collect 15 and return them to the Fairy Fountain in Great Bay.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x1A5), ItemPool(ItemCategory.StrayFairies, LocationCategory.Freestanding)]
        CollectibleStrayFairyGreatBay3,

        [Visible]
        [ItemName("Great Bay Stray Fairy"), LocationName("Great Bay Pre-Boss Room Bubble"), Region(Region.GreatBayTemple)]
        [GossipLocationHint("a lost creature"), GossipItemHint("a lost fairy")]
        [ShopText("Collect 15 and return them to the Fairy Fountain in Great Bay.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x1A6), ItemPool(ItemCategory.StrayFairies, LocationCategory.Freestanding)]
        CollectibleStrayFairyGreatBay4,

        // A8 empty

        [Visible]
        [ItemName("Great Bay Stray Fairy"), LocationName("Great Bay Waterwheel Room Upper"), Region(Region.GreatBayTemple)]
        [GossipLocationHint("a lost creature"), GossipItemHint("a lost fairy")]
        [ShopText("Collect 15 and return them to the Fairy Fountain in Great Bay.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold), Chest(0x02940000 + 0x23A, ChestAttribute.AppearanceType.Normal)]
        [GetItemIndex(0x1A9), ItemPool(ItemCategory.StrayFairies, LocationCategory.Chests)]
        CollectibleStrayFairyGreatBay5,

        [Visible]
        [ItemName("Great Bay Stray Fairy"), LocationName("Great Bay Green Valve"), Region(Region.GreatBayTemple)]
        [GossipLocationHint("a lost creature"), GossipItemHint("a lost fairy")]
        [ShopText("Collect 15 and return them to the Fairy Fountain in Great Bay.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold), Chest(0x02959000 + 0x18E, ChestAttribute.AppearanceType.Normal)]
        [GetItemIndex(0x1AA), ItemPool(ItemCategory.StrayFairies, LocationCategory.Chests)]
        CollectibleStrayFairyGreatBay6,

        [Visible]
        [ItemName("Great Bay Stray Fairy"), LocationName("Great Bay Seesaw Room"), Region(Region.GreatBayTemple)]
        [GossipLocationHint("a lost creature"), GossipItemHint("a lost fairy")]
        [ShopText("Collect 15 and return them to the Fairy Fountain in Great Bay.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold), Chest(0x02945000 + 0x24A, ChestAttribute.AppearanceType.Normal)]
        [GetItemIndex(0x1AB), ItemPool(ItemCategory.StrayFairies, LocationCategory.Chests)]
        CollectibleStrayFairyGreatBay7,

        [Visible]
        [ItemName("Great Bay Stray Fairy"), LocationName("Great Bay Waterwheel Room Lower"), Region(Region.GreatBayTemple)]
        [GossipLocationHint("a lost creature"), GossipItemHint("a lost fairy")]
        [ShopText("Collect 15 and return them to the Fairy Fountain in Great Bay.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold), Chest(0x02940000 + 0x24A, ChestAttribute.AppearanceType.Normal)]
        [GetItemIndex(0x1AC), ItemPool(ItemCategory.StrayFairies, LocationCategory.Chests)]
        CollectibleStrayFairyGreatBay8,

        [Visible]
        [ItemName("Great Bay Stray Fairy"), LocationName("Great Bay Entrance Torches"), Region(Region.GreatBayTemple)]
        [GossipLocationHint("a lost creature"), GossipItemHint("a lost fairy")]
        [ShopText("Collect 15 and return them to the Fairy Fountain in Great Bay.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold), Chest(0x02962000 + 0x1F2, ChestAttribute.AppearanceType.AppearsSwitch)]
        [GetItemIndex(0x1AD), ItemPool(ItemCategory.StrayFairies, LocationCategory.Chests)]
        CollectibleStrayFairyGreatBay9,

        [Visible]
        [ItemName("Great Bay Stray Fairy"), LocationName("Great Bay Bio Babas"), Region(Region.GreatBayTemple)]
        [GossipLocationHint("a lost creature"), GossipItemHint("a lost fairy")]
        [ShopText("Collect 15 and return them to the Fairy Fountain in Great Bay.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold), Chest(0x02911000 + 0xDA, ChestAttribute.AppearanceType.AppearsClear)]
        [GetItemIndex(0x1AE), ItemPool(ItemCategory.StrayFairies, LocationCategory.Chests)]
        CollectibleStrayFairyGreatBay10,

        [Visible]
        [ItemName("Great Bay Stray Fairy"), LocationName("Great Bay Underwater Barrel"), Region(Region.GreatBayTemple)]
        [GossipLocationHint("a lost creature"), GossipItemHint("a lost fairy")]
        [ShopText("Collect 15 and return them to the Fairy Fountain in Great Bay.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x1AF), ItemPool(ItemCategory.StrayFairies, LocationCategory.Barrels)]
        CollectibleStrayFairyGreatBay11,

        [Visible]
        [ItemName("Great Bay Stray Fairy"), LocationName("Great Bay Whirlpool Jar"), Region(Region.GreatBayTemple)]
        [GossipLocationHint("a lost creature"), GossipItemHint("a lost fairy")]
        [ShopText("Collect 15 and return them to the Fairy Fountain in Great Bay.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x1B0), ItemPool(ItemCategory.StrayFairies, LocationCategory.Jars)]
        CollectibleStrayFairyGreatBay12,

        [Visible]
        [ItemName("Great Bay Stray Fairy"), LocationName("Great Bay Whirlpool Barrel"), Region(Region.GreatBayTemple)]
        [GossipLocationHint("a lost creature"), GossipItemHint("a lost fairy")]
        [ShopText("Collect 15 and return them to the Fairy Fountain in Great Bay.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x1B1), ItemPool(ItemCategory.StrayFairies, LocationCategory.Barrels)]
        CollectibleStrayFairyGreatBay13,

        [Visible]
        [ItemName("Great Bay Stray Fairy"), LocationName("Great Bay Dexihands Jar"), Region(Region.GreatBayTemple)]
        [GossipLocationHint("a lost creature"), GossipItemHint("a lost fairy")]
        [ShopText("Collect 15 and return them to the Fairy Fountain in Great Bay.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x1B2), ItemPool(ItemCategory.StrayFairies, LocationCategory.Jars)]
        CollectibleStrayFairyGreatBay14,

        [Visible]
        [ItemName("Great Bay Stray Fairy"), LocationName("Great Bay Ledge Jar"), Region(Region.GreatBayTemple)]
        [GossipLocationHint("a lost creature"), GossipItemHint("a lost fairy")]
        [ShopText("Collect 15 and return them to the Fairy Fountain in Great Bay.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [GetItemIndex(0x1B3), ItemPool(ItemCategory.StrayFairies, LocationCategory.Jars)]
        CollectibleStrayFairyGreatBay15,

        [Visible]
        [ItemName("Stone Tower Stray Fairy"), LocationName("Stone Tower Mirror Sun Block"), Region(Region.StoneTowerTemple)]
        [GossipLocationHint("a lost creature"), GossipItemHint("a lost fairy")]
        [ShopText("Collect 15 and return them to the Fairy Fountain in Stone Tower.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold), Chest(0x02119000 + 0x282, ChestAttribute.AppearanceType.Normal, 0x0218B000 + 0x8A)]
        [GetItemIndex(0x1B4), ItemPool(ItemCategory.StrayFairies, LocationCategory.Chests)]
        CollectibleStrayFairyStoneTower1,

        [Visible]
        [ItemName("Stone Tower Stray Fairy"), LocationName("Stone Tower Eyegore"), Region(Region.StoneTowerTemple)]
        [GossipLocationHint("a lost creature"), GossipItemHint("a lost fairy")]
        [ShopText("Collect 15 and return them to the Fairy Fountain in Stone Tower.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold), Chest(0x020F1000 + 0x1A2, ChestAttribute.AppearanceType.AppearsSwitch, 0x02164000 + 0x17E)]
        [GetItemIndex(0x1B5), ItemPool(ItemCategory.StrayFairies, LocationCategory.Chests)]
        CollectibleStrayFairyStoneTower2,

        [Visible]
        [ItemName("Stone Tower Stray Fairy"), LocationName("Stone Tower Lava Room Fire Ring"), Region(Region.StoneTowerTemple)]
        [GossipLocationHint("a lost creature"), GossipItemHint("a lost fairy")]
        [ShopText("Collect 15 and return them to the Fairy Fountain in Stone Tower.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold), Chest(0x02122000 + 0x1F6, ChestAttribute.AppearanceType.Normal, 0x02191000 + 0x7A)]
        [GetItemIndex(0x1B6), ItemPool(ItemCategory.StrayFairies, LocationCategory.Chests)]
        CollectibleStrayFairyStoneTower3,

        [Visible]
        [ItemName("Stone Tower Stray Fairy"), LocationName("Stone Tower Updraft Fire Ring"), Region(Region.StoneTowerTemple)]
        [GossipLocationHint("a lost creature"), GossipItemHint("a lost fairy")]
        [ShopText("Collect 15 and return them to the Fairy Fountain in Stone Tower.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold), Chest(0x02104000 + 0x252, ChestAttribute.AppearanceType.AppearsSwitch, 0x02177000 + 0x29E)]
        [GetItemIndex(0x1B7), ItemPool(ItemCategory.StrayFairies, LocationCategory.Chests)]
        CollectibleStrayFairyStoneTower4,

        [Visible]
        [ItemName("Stone Tower Stray Fairy"), LocationName("Stone Tower Mirror Sun Switch"), Region(Region.StoneTowerTemple)]
        [GossipLocationHint("a lost creature"), GossipItemHint("a lost fairy")]
        [ShopText("Collect 15 and return them to the Fairy Fountain in Stone Tower.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold), Chest(0x02119000 + 0x272, ChestAttribute.AppearanceType.AppearsSwitch, 0x0218B000 + 0x7A)]
        [GetItemIndex(0x1B8), ItemPool(ItemCategory.StrayFairies, LocationCategory.Chests)]
        CollectibleStrayFairyStoneTower5,

        [Visible]
        [ItemName("Stone Tower Stray Fairy"), LocationName("Stone Tower Entrance Sun Switch"), Region(Region.StoneTowerTemple)]
        [GossipLocationHint("a lost creature"), GossipItemHint("a lost fairy")]
        [ShopText("Collect 15 and return them to the Fairy Fountain in Stone Tower.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold), Chest(0x020E2000 + 0x162, ChestAttribute.AppearanceType.AppearsSwitch, 0x02156000 + 0xFA)]
        [GetItemIndex(0x1B9), ItemPool(ItemCategory.StrayFairies, LocationCategory.Chests)]
        CollectibleStrayFairyStoneTower6,

        [Visible]
        [ItemName("Stone Tower Stray Fairy"), LocationName("Stone Tower Wizzrobe"), Region(Region.StoneTowerTemple)]
        [GossipLocationHint("a lost creature"), GossipItemHint("a lost fairy")]
        [ShopText("Collect 15 and return them to the Fairy Fountain in Stone Tower.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold), Chest(0x0210F000 + 0x1F2, ChestAttribute.AppearanceType.AppearsSwitch, 0x02182000 + 0x1EE)]
        [GetItemIndex(0x1BA), ItemPool(ItemCategory.StrayFairies, LocationCategory.Chests)]
        CollectibleStrayFairyStoneTower7,

        [Visible]
        [ItemName("Stone Tower Stray Fairy"), LocationName("Stone Tower Death Armos"), Region(Region.StoneTowerTemple)]
        [GossipLocationHint("a lost creature"), GossipItemHint("a lost fairy")]
        [ShopText("Collect 15 and return them to the Fairy Fountain in Stone Tower.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold), Chest(0x020E2000 + 0x172, ChestAttribute.AppearanceType.AppearsSwitch, 0x02156000 + 0x10A)]
        [GetItemIndex(0x1BB), ItemPool(ItemCategory.StrayFairies, LocationCategory.Chests)]
        CollectibleStrayFairyStoneTower8,

        [Visible]
        [ItemName("Stone Tower Stray Fairy"), LocationName("Stone Tower Updraft Frozen Eye"), Region(Region.StoneTowerTemple)]
        [GossipLocationHint("a lost creature"), GossipItemHint("a lost fairy")]
        [ShopText("Collect 15 and return them to the Fairy Fountain in Stone Tower.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold), Chest(0x02104000 + 0x262, ChestAttribute.AppearanceType.AppearsSwitch, 0x02177000 + 0x2AE)]
        [GetItemIndex(0x1BC), ItemPool(ItemCategory.StrayFairies, LocationCategory.Chests)]
        CollectibleStrayFairyStoneTower9,

        [Visible]
        [ItemName("Stone Tower Stray Fairy"), LocationName("Stone Tower Thin Bridge"), Region(Region.StoneTowerTemple)]
        [GossipLocationHint("a lost creature"), GossipItemHint("a lost fairy")]
        [ShopText("Collect 15 and return them to the Fairy Fountain in Stone Tower.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold), Chest(0x0211D000 + 0x1E2, ChestAttribute.AppearanceType.AppearsSwitch, 0x0218C000 + 0x25E)]
        [GetItemIndex(0x1BD), ItemPool(ItemCategory.StrayFairies, LocationCategory.Chests)]
        CollectibleStrayFairyStoneTower10,

        [Visible]
        [ItemName("Stone Tower Stray Fairy"), LocationName("Stone Tower Basement Ledge"), Region(Region.StoneTowerTemple)]
        [GossipLocationHint("a lost creature"), GossipItemHint("a lost fairy")]
        [ShopText("Collect 15 and return them to the Fairy Fountain in Stone Tower.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold), Chest(0x0210F000 + 0x212, ChestAttribute.AppearanceType.Normal, 0x02182000 + 0x20E)]
        [GetItemIndex(0x1BE), ItemPool(ItemCategory.StrayFairies, LocationCategory.Chests)]
        CollectibleStrayFairyStoneTower11,

        [Visible]
        [ItemName("Stone Tower Stray Fairy"), LocationName("Stone Tower Statue Eye"), Region(Region.StoneTowerTemple)]
        [GossipLocationHint("a lost creature"), GossipItemHint("a lost fairy")]
        [ShopText("Collect 15 and return them to the Fairy Fountain in Stone Tower.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold), Chest(0x020E2000 + 0x182, ChestAttribute.AppearanceType.AppearsSwitch, 0x02156000 + 0x11A)]
        [GetItemIndex(0x1BF), ItemPool(ItemCategory.StrayFairies, LocationCategory.Chests)]
        CollectibleStrayFairyStoneTower12,

        [Visible]
        [ItemName("Stone Tower Stray Fairy"), LocationName("Stone Tower Underwater"), Region(Region.StoneTowerTemple)]
        [GossipLocationHint("a lost creature"), GossipItemHint("a lost fairy")]
        [ShopText("Collect 15 and return them to the Fairy Fountain in Stone Tower.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold), Chest(0x02104000 + 0x272, ChestAttribute.AppearanceType.AppearsSwitch, 0x02177000 + 0x2BE)]
        [GetItemIndex(0x1C0), ItemPool(ItemCategory.StrayFairies, LocationCategory.Chests)]
        CollectibleStrayFairyStoneTower13,

        [Visible]
        [ItemName("Stone Tower Stray Fairy"), LocationName("Stone Tower Bridge Crystal"), Region(Region.StoneTowerTemple)]
        [GossipLocationHint("a lost creature"), GossipItemHint("a lost fairy")]
        [ShopText("Collect 15 and return them to the Fairy Fountain in Stone Tower.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold), Chest(0x020F1000 + 0x1B2, ChestAttribute.AppearanceType.AppearsSwitch, 0x02164000 + 0x18E)]
        [GetItemIndex(0x1C1), ItemPool(ItemCategory.StrayFairies, LocationCategory.Chests)]
        CollectibleStrayFairyStoneTower14,

        [Visible]
        [ItemName("Stone Tower Stray Fairy"), LocationName("Stone Tower Lava Room Ledge"), Region(Region.StoneTowerTemple)]
        [GossipLocationHint("a lost creature"), GossipItemHint("a lost fairy")]
        [ShopText("Collect 15 and return them to the Fairy Fountain in Stone Tower.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold), Chest(0x02122000 + 0x206, ChestAttribute.AppearanceType.Normal, 0x02191000 + 0x8A)]
        [GetItemIndex(0x1C2), ItemPool(ItemCategory.StrayFairies, LocationCategory.Chests)]
        CollectibleStrayFairyStoneTower15,

        [Purchaseable]
        [RupeeRepeatable]
        [Repeatable]
        [ItemName("Purple Rupee"), LocationName("Lottery"), Region(Region.WestClockTown)]
        [GossipLocationHint("a town game"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 50 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x86), ItemPool(ItemCategory.PurpleRupees, LocationCategory.NpcRewards)]
        MundaneItemLotteryPurpleRupee,

        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Bank Reward #2"), Region(Region.WestClockTown)]
        [GossipLocationHint("interest"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x13D), ItemPool(ItemCategory.BlueRupees, LocationCategory.NpcRewards)]
        MundaneItemBankBlueRupee,

        [Repeatable, Temporary, Overwritable, Purchaseable]
        [ItemName("Chateau Romani"), LocationName("Milk Bar Chateau"), Region(Region.EastClockTown)]
        [GossipLocationHint("a town shop"), GossipItemHint("a dairy product", "an adult beverage")]
        [ShopText("Drink it to get lasting stamina for your magic power.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [HackContent(nameof(Resources.mods.fix_shop_milkbar))]
        [GetItemIndex(0x180), ItemPool(ItemCategory.Chateau, LocationCategory.Purchases)]
        ShopItemMilkBarChateau,

        [Repeatable, Temporary, Overwritable, Purchaseable]
        [ItemName("Milk"), LocationName("Milk Bar Milk"), Region(Region.EastClockTown)]
        [GossipLocationHint("a town shop"), GossipItemHint("a dairy product", "the produce of cows")]
        [ShopText("Recover five hearts with one drink. Contains two helpings.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [HackContent(nameof(Resources.mods.fix_shop_milkbar))]
        [GetItemIndex(0x181), ItemPool(ItemCategory.Milk, LocationCategory.Purchases)]
        ShopItemMilkBarMilk,

        [RupeeRepeatable]
        [Repeatable]
        [ItemName("Purple Rupee"), LocationName("Deku Playground Any Day"), Region(Region.NorthClockTown)]
        [GossipLocationHint("a game for scrubs", "a playground", "a town game"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [GossipCombineOrder(0), GossipCombine(HeartPieceDekuPlayground, "Deku Playground")]
        [ShopText("This is worth 50 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x133), ItemPool(ItemCategory.PurpleRupees, LocationCategory.Minigames)]
        MundaneItemDekuPlaygroundPurpleRupee,

        [RupeeRepeatable]
        [Repeatable]
        [ItemName("Purple Rupee"), LocationName("Honey and Darling Any Day"), Region(Region.EastClockTown)]
        [GossipLocationHint("a town game"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [GossipCombineOrder(0), GossipCombine(HeartPieceHoneyAndDarling, "Honey and Darling")]
        [ShopText("This is worth 50 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x183), ItemPool(ItemCategory.PurpleRupees, LocationCategory.Minigames)]
        MundaneItemHoneyAndDarlingPurpleRupee,

        [RupeeRepeatable]
        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Kotake Mushroom Sale"), Region(Region.SouthernSwamp)]
        [GossipLocationHint("a sleeping witch", "a southern merchant"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x187), ItemPool(ItemCategory.RedRupees, LocationCategory.NpcRewards)]
        MundaneItemKotakeMushroomSaleRedRupee,

        [RupeeRepeatable]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Pictograph Contest Standard Photo"), Region(Region.SouthernSwamp)]
        [GossipLocationHint("a swamp game"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x188), ItemPool(ItemCategory.BlueRupees, LocationCategory.NpcRewards)]
        MundaneItemPictographContestBlueRupee,

        [RupeeRepeatable]
        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Pictograph Contest Good Photo"), Region(Region.SouthernSwamp)]
        [GossipLocationHint("a swamp game"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x18F), ItemPool(ItemCategory.RedRupees, LocationCategory.NpcRewards)]
        MundaneItemPictographContestRedRupee,

        [Repeatable, Temporary, Purchaseable]
        [StartingItemId(0x0A)]
        [ItemName("Magic Bean"), LocationName("Swamp Scrub Purchase"), MultiLocation(ShopItemBusinessScrubMagicBeanInSwamp, ShopItemBusinessScrubMagicBeanInTown)]
        [GossipLocationHint("a southern merchant"), GossipItemHint("a plant seed")]
        [ShopText("Plant it in soft soil.")]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [HackContent(nameof(Resources.mods.fix_shop_businessscrub_magicbean), false)]
        [GetItemIndex(0x19B), ItemPool(ItemCategory.MainInventory, LocationCategory.Purchases)]
        ShopItemBusinessScrubMagicBean,

        [Repeatable, Temporary, Overwritable, Purchaseable]
        [ItemName("Green Potion"), LocationName("Ocean Scrub Purchase"), MultiLocation(ShopItemBusinessScrubGreenPotionInOcean, ShopItemBusinessScrubGreenPotionInMountain)]
        [GossipLocationHint("a western merchant"), GossipItemHint("a magic potion", "a green drink")]
        [ShopText("Replenishes your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [HackContent(nameof(Resources.mods.fix_shop_businessscrub_greenpotion))]
        [GetItemIndex(0x19C), ItemPool(ItemCategory.GreenPotions, LocationCategory.Purchases)]
        ShopItemBusinessScrubGreenPotion,

        [Repeatable, Temporary, Overwritable, Purchaseable]
        [ItemName("Blue Potion"), LocationName("Canyon Scrub Purchase"), MultiLocation(ShopItemBusinessScrubBluePotionInCanyon, ShopItemBusinessScrubBluePotionInOcean)]
        [GossipLocationHint("an eastern merchant"), GossipItemHint("consumable strength", "a magic potion", "a blue drink")]
        [ShopText("Replenishes both life energy and magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [HackContent(nameof(Resources.mods.fix_shop_businessscrub_bluepotion))]
        [GetItemIndex(0x19D), ItemPool(ItemCategory.BluePotions, LocationCategory.Purchases)]
        ShopItemBusinessScrubBluePotion,

        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Zora Hall Stage Lights"), Region(Region.ZoraHall)]
        [GossipLocationHint("a good deed"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x19E), ItemPool(ItemCategory.BlueRupees, LocationCategory.NpcRewards)]
        MundaneItemZoraStageLightsBlueRupee,

        [Repeatable, Temporary, Overwritable, Purchaseable]
        [ItemName("Milk"), LocationName("Gorman Bros Milk Purchase"), Region(Region.MilkRoad)]
        [GossipLocationHint("a shady gentleman", "a dodgy seller", "a shady dealer"), GossipItemHint("a dairy product", "the produce of cows")]
        [ShopText("Recover five hearts with one drink. Contains two helpings.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [HackContent(nameof(Resources.mods.fix_shop_gorman_milk))]
        [GetItemIndex(0x1A0), ItemPool(ItemCategory.Milk, LocationCategory.Purchases)]
        ShopItemGormanBrosMilk,

        [Repeatable]
        [ItemName("Purple Rupee"), LocationName("Ocean Spider House Day 2 Reward"), Region(Region.GreatBayCoast)]
        [GossipLocationHint("a gold spider"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff"), GossipCompetitiveHint(0, ItemCategory.SkulltulaTokens, false)]
        [GossipCombineOrder(1), GossipCombine(UpgradeGiantWallet, "Ocean Spider House"), GossipCombine(MundaneItemOceanSpiderHouseDay3RedRupee, "Ocean Spider House")]
        [ShopText("This is worth 50 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x134), ItemPool(ItemCategory.PurpleRupees, LocationCategory.NpcRewards)]
        MundaneItemOceanSpiderHouseDay2PurpleRupee,

        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Ocean Spider House Day 3 Reward"), Region(Region.GreatBayCoast)]
        [GossipLocationHint("a gold spider"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff"), GossipCompetitiveHint(0, ItemCategory.SkulltulaTokens, false)]
        [GossipCombineOrder(2), GossipCombine(MundaneItemOceanSpiderHouseDay2PurpleRupee, "Ocean Spider House"), GossipCombine(UpgradeGiantWallet, "Ocean Spider House")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x1A3), ItemPool(ItemCategory.RedRupees, LocationCategory.NpcRewards)]
        MundaneItemOceanSpiderHouseDay3RedRupee,

        [RupeeRepeatable]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Bad Pictograph of Lulu"), Region(Region.ZoraHall)]
        [GossipLocationHint("a fan"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x1A8), ItemPool(ItemCategory.BlueRupees, LocationCategory.NpcRewards)]
        MundaneItemLuluBadPictographBlueRupee,

        [RupeeRepeatable]
        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Good Pictograph of Lulu"), Region(Region.ZoraHall)]
        [GossipLocationHint("a fan"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x1C3), ItemPool(ItemCategory.RedRupees, LocationCategory.NpcRewards)]
        MundaneItemLuluGoodPictographRedRupee,

        [RupeeRepeatable]
        [Repeatable]
        [ItemName("Purple Rupee"), LocationName("Treasure Chest Game Human"), Region(Region.EastClockTown)]
        [GossipLocationHint("a town game"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 50 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), Chest(0x00F43F10 + 0xFA8, ChestAttribute.AppearanceType.AppearsSwitch, 0x00F43F10 + 0xFB0)]
        [GetItemIndex(0x1C4), ItemPool(ItemCategory.PurpleRupees, LocationCategory.Minigames)]
        MundaneItemTreasureChestGamePurpleRupee,

        [RupeeRepeatable]
        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Treasure Chest Game Zora"), Region(Region.EastClockTown)]
        [GossipLocationHint("a town game"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), Chest(0x00F43F10 + 0xFAC, ChestAttribute.AppearanceType.AppearsSwitch)]
        [GetItemIndex(0x1C5), ItemPool(ItemCategory.RedRupees, LocationCategory.Minigames)]
        MundaneItemTreasureChestGameRedRupee,

        [RupeeRepeatable]
        [Repeatable, Temporary]
        [ItemName("10 Deku Nuts"), LocationName("Treasure Chest Game Deku"), Region(Region.EastClockTown)]
        [GossipLocationHint("a town game"), GossipItemHint("a flashing impact")]
        [ShopText("Its flash blinds enemies.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden), Chest(0x00F43F10 + 0xFAE, ChestAttribute.AppearanceType.AppearsSwitch)]
        [GetItemIndex(0x1C6), ItemPool(ItemCategory.DekuNuts, LocationCategory.Minigames)]
        MundaneItemTreasureChestGameDekuNuts,

        [RupeeRepeatable]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Curiosity Shop Blue Rupee"), Region(Region.WestClockTown)]
        [GossipLocationHint("a shady gentleman", "a dodgy seller", "a shady dealer"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x1C7), ItemPool(ItemCategory.BlueRupees, LocationCategory.NpcRewards)]
        MundaneItemCuriosityShopBlueRupee,

        [RupeeRepeatable]
        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Curiosity Shop Red Rupee"), Region(Region.WestClockTown)]
        [GossipLocationHint("a shady gentleman", "a dodgy seller", "a shady dealer"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x1C8), ItemPool(ItemCategory.RedRupees, LocationCategory.NpcRewards)]
        MundaneItemCuriosityShopRedRupee,

        [RupeeRepeatable]
        [Repeatable]
        [ItemName("Purple Rupee"), LocationName("Curiosity Shop Purple Rupee"), Region(Region.WestClockTown)]
        [GossipLocationHint("a shady gentleman", "a dodgy seller", "a shady dealer"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 50 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x1C9), ItemPool(ItemCategory.PurpleRupees, LocationCategory.NpcRewards)]
        MundaneItemCuriosityShopPurpleRupee,

        [RupeeRepeatable]
        [Repeatable]
        [ItemName("Gold Rupee"), LocationName("Curiosity Shop Gold Rupee"), Region(Region.WestClockTown)]
        [GossipLocationHint("a shady gentleman", "a dodgy seller", "a shady dealer"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 200 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x1CA), ItemPool(ItemCategory.GoldRupees, LocationCategory.NpcRewards)]
        MundaneItemCuriosityShopGoldRupee,

        [Visible, Purchaseable]
        [Repeatable, Temporary, Overwritable]
        [ItemName("Seahorse"), LocationName("Fisherman Pictograph"), Region(Region.GreatBayCoast)]
        [GossipLocationHint("a fisherman"), GossipItemHint("a sea creature")]
        [ShopText("It wants to go back home to Pinnacle Rock.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallGold)]
        [HackContent(nameof(Resources.mods.fix_fisherman))]
        [GetItemIndex(0x95), ItemPool(ItemCategory.Seahorse, LocationCategory.NpcRewards)]
        MundaneItemSeahorse,

        //[GetItemIndex(0x1A1)]

        [Visible]
        [Repeatable]
        [ItemName("10 Arrows"), LocationName("Ikana Castle Courtyard Grass"), Region(Region.IkanaCastle)]
        [GossipLocationHint("an ancient plant"), GossipItemHint("a quiver refill", "a bundle of projectiles")]
        [ShopText("Ammo for your bow.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x1CC), ItemPool(ItemCategory.Arrows, LocationCategory.Grass), CollectableIndex(0xEB7)]
        CollectableAncientCastleOfIkanaCastleExteriorGrass1,


        [Visible]
        [Repeatable]
        [ItemName("10 Arrows"), LocationName("Ikana Castle Courtyard Grass 2"), Region(Region.IkanaCastle)]
        [GossipLocationHint("an ancient plant"), GossipItemHint("a quiver refill", "a bundle of projectiles")]
        [ShopText("Ammo for your bow.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x1CD), ItemPool(ItemCategory.Arrows, LocationCategory.Grass), CollectableIndex(0xEBB)]
        CollectableAncientCastleOfIkanaCastleExteriorGrass2,


        [Visible]
        [Repeatable]
        [ItemName("10 Arrows"), LocationName("Night 1 Grave Pot"), Region(Region.IkanaGraveyard)]
        [GossipLocationHint("an unholy jar"), GossipItemHint("a quiver refill", "a bundle of projectiles")]
        [ShopText("Ammo for your bow.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x1CE), ItemPool(ItemCategory.Arrows, LocationCategory.Jars), CollectableIndex(0x623)]
        CollectableBeneathTheGraveyardMainAreaPot1,


        [Visible]
        [Repeatable]
        [ItemName("10 Arrows"), LocationName("Night 2 Grave Pot"), Region(Region.IkanaGraveyard)]
        [GossipLocationHint("an unholy jar"), GossipItemHint("a quiver refill", "a bundle of projectiles")]
        [ShopText("Ammo for your bow.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x1CF), ItemPool(ItemCategory.Arrows, LocationCategory.Jars), CollectableIndex(0x625)]
        CollectableBeneathTheGraveyardInvisibleRoomPot1,


        [Visible]
        [Repeatable]
        [ItemName("10 Arrows"), LocationName("Night 1 Grave Pot 2"), Region(Region.IkanaGraveyard)]
        [GossipLocationHint("an unholy jar"), GossipItemHint("a quiver refill", "a bundle of projectiles")]
        [ShopText("Ammo for your bow.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x1D0), ItemPool(ItemCategory.Arrows, LocationCategory.Jars), CollectableIndex(0x629)]
        CollectableBeneathTheGraveyardBadBatRoomPot1,


        [Visible]
        [Repeatable]
        [ItemName("10 Arrows"), LocationName("Cucco Shack Crate"), Region(Region.RomaniRanch)]
        [GossipLocationHint("a chicken crate"), GossipItemHint("a quiver refill", "a bundle of projectiles")]
        [ShopText("Ammo for your bow.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x1D1), ItemPool(ItemCategory.Arrows, LocationCategory.Crates), CollectableIndex(0x210A)]
        CollectableCuccoShackWoodenCrateLarge1,


        [Visible]
        [Repeatable]
        [ItemName("10 Arrows"), LocationName("Dampe's Basement Pot"), Region(Region.IkanaGraveyard)]
        [GossipLocationHint("an unholy jar"), GossipItemHint("a quiver refill", "a bundle of projectiles")]
        [ShopText("Ammo for your bow.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x1D2), ItemPool(ItemCategory.Arrows, LocationCategory.Jars), CollectableIndex(0x1807)]
        CollectableDampesHouseBasementPot1,


        [Visible]
        [Repeatable]
        [ItemName("10 Arrows"), LocationName("Dampe's Basement Pot 2"), Region(Region.IkanaGraveyard)]
        [GossipLocationHint("an unholy jar"), GossipItemHint("a quiver refill", "a bundle of projectiles")]
        [ShopText("Ammo for your bow.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x1D3), ItemPool(ItemCategory.Arrows, LocationCategory.Jars), CollectableIndex(0x180A)]
        CollectableDampesHouseBasementPot2,


        [Visible]
        [Repeatable]
        [ItemName("10 Arrows"), LocationName("Dampe's Basement Pot 3"), Region(Region.IkanaGraveyard)]
        [GossipLocationHint("an unholy jar"), GossipItemHint("a quiver refill", "a bundle of projectiles")]
        [ShopText("Ammo for your bow.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x1D4), ItemPool(ItemCategory.Arrows, LocationCategory.Jars), CollectableIndex(0x180C)]
        CollectableDampesHouseBasementPot3,


        [Visible]
        [Repeatable]
        [ItemName("10 Arrows"), LocationName("Dampe's Basement Pot 4"), Region(Region.IkanaGraveyard)]
        [GossipLocationHint("an unholy jar"), GossipItemHint("a quiver refill", "a bundle of projectiles")]
        [ShopText("Ammo for your bow.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x1D5), ItemPool(ItemCategory.Arrows, LocationCategory.Jars), CollectableIndex(0x180D)]
        CollectableDampesHouseBasementPot4,


        [Visible]
        [Repeatable]
        [ItemName("10 Arrows"), LocationName("Goron Village Small Snowball"), Region(Region.GoronVillage)]
        [GossipLocationHint("a village snowball"), GossipItemHint("a quiver refill", "a bundle of projectiles")]
        [ShopText("Ammo for your bow.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x1D6), ItemPool(ItemCategory.Arrows, LocationCategory.SmallSnowballs), CollectableIndex(0x26B3)]
        CollectableGoronVillageWinterSmallSnowball1,


        [Visible]
        [Repeatable]
        [ItemName("10 Arrows"), LocationName("Goron Village Small Snowball 2"), Region(Region.GoronVillage)]
        [GossipLocationHint("a village snowball"), GossipItemHint("a quiver refill", "a bundle of projectiles")]
        [ShopText("Ammo for your bow.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x1D7), ItemPool(ItemCategory.Arrows, LocationCategory.SmallSnowballs), CollectableIndex(0x26B4)]
        CollectableGoronVillageWinterSmallSnowball2,


        [Visible]
        [Repeatable]
        [ItemName("10 Arrows"), LocationName("Great Bay Coast Pot"), Region(Region.GreatBayCoast)]
        [GossipLocationHint("an ocean jar"), GossipItemHint("a quiver refill", "a bundle of projectiles")]
        [ShopText("Ammo for your bow.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x1D8), ItemPool(ItemCategory.Arrows, LocationCategory.Jars), CollectableIndex(0x1BA4)]
        CollectableGreatBayCoastPot1,


        [Visible]
        [Repeatable]
        [ItemName("10 Arrows"), LocationName("Great Bay Coast Pot 2"), Region(Region.GreatBayCoast)]
        [GossipLocationHint("an ocean jar"), GossipItemHint("a quiver refill", "a bundle of projectiles")]
        [ShopText("Ammo for your bow.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x1D9), ItemPool(ItemCategory.Arrows, LocationCategory.Jars), CollectableIndex(0x1BA6)]
        CollectableGreatBayCoastPot2,


        [Visible]
        [Repeatable]
        [ItemName("10 Arrows"), LocationName("Great Bay Coast Pot 3"), Region(Region.GreatBayCoast)]
        [GossipLocationHint("an ocean jar"), GossipItemHint("a quiver refill", "a bundle of projectiles")]
        [ShopText("Ammo for your bow.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x1DA), ItemPool(ItemCategory.Arrows, LocationCategory.Jars), CollectableIndex(0x1BA8)]
        CollectableGreatBayCoastPot3,


        [Visible]
        [Repeatable]
        [ItemName("10 Arrows"), LocationName("Great Bay Coast Pot 4"), Region(Region.GreatBayCoast)]
        [GossipLocationHint("an ocean jar"), GossipItemHint("a quiver refill", "a bundle of projectiles")]
        [ShopText("Ammo for your bow.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x1DB), ItemPool(ItemCategory.Arrows, LocationCategory.Jars), CollectableIndex(0x1BAA)]
        CollectableGreatBayCoastPot4,


        [Visible]
        [Repeatable]
        [ItemName("10 Arrows"), LocationName("Great Bay Temple Red Valve Barrel"), Region(Region.GreatBayTemple)]
        [GossipLocationHint("Great Bay Temple", "the ocean temple"), GossipItemHint("a quiver refill", "a bundle of projectiles")]
        [ShopText("Ammo for your bow.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x1DC), ItemPool(ItemCategory.Arrows, LocationCategory.Barrels), CollectableIndex(0x24A1)]
        CollectableGreatBayTempleBlueChuchuValveRoomBarrel1,


        [Visible]
        [Repeatable]
        [ItemName("10 Arrows"), LocationName("Ikana King Pot"), Region(Region.IkanaCastle)]
        [GossipLocationHint("an ancient jar"), GossipItemHint("a quiver refill", "a bundle of projectiles")]
        [ShopText("Ammo for your bow.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x1DD), ItemPool(ItemCategory.Arrows, LocationCategory.Jars), CollectableIndex(0x2B20)]
        CollectableIgosDuIkanaSLairIgosDuIkanaSRoomPot1,


        [Visible]
        [Repeatable]
        [ItemName("10 Arrows"), LocationName("Ikana King Pot 2"), Region(Region.IkanaCastle)]
        [GossipLocationHint("an ancient jar"), GossipItemHint("a quiver refill", "a bundle of projectiles")]
        [ShopText("Ammo for your bow.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x1DE), ItemPool(ItemCategory.Arrows, LocationCategory.Jars), CollectableIndex(0x2B23)]
        CollectableIgosDuIkanaSLairIgosDuIkanaSRoomPot2,


        [Visible]
        [Repeatable]
        [ItemName("10 Arrows"), LocationName("Ikana King Entry Pot"), Region(Region.IkanaCastle)]
        [GossipLocationHint("an ancient jar"), GossipItemHint("a quiver refill", "a bundle of projectiles")]
        [ShopText("Ammo for your bow.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x1DF), ItemPool(ItemCategory.Arrows, LocationCategory.Jars), CollectableIndex(0x2B2E)]
        CollectableIgosDuIkanaSLairPreBossRoomPot1,


        [Visible]
        [Repeatable]
        [ItemName("10 Arrows"), LocationName("Ikana King Entry Pot 2"), Region(Region.IkanaCastle)]
        [GossipLocationHint("an ancient jar"), GossipItemHint("a quiver refill", "a bundle of projectiles")]
        [ShopText("Ammo for your bow.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x1E0), ItemPool(ItemCategory.Arrows, LocationCategory.Jars), CollectableIndex(0x2B2F)]
        CollectableIgosDuIkanaSLairPreBossRoomPot2,


        [Visible]
        [Repeatable]
        [ItemName("10 Arrows"), LocationName("Ikana Graveyard Grass"), Region(Region.IkanaGraveyard)]
        [GossipLocationHint("unholy grass"), GossipItemHint("a quiver refill", "a bundle of projectiles")]
        [ShopText("Ammo for your bow.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x1E1), ItemPool(ItemCategory.Arrows, LocationCategory.Grass), CollectableIndex(0x21A9)]
        CollectableIkanaGraveyardIkanaGraveyardLowerGrass1,


        [Visible]
        [Repeatable]
        [ItemName("10 Arrows"), LocationName("Oceanside Spider House Entrance Pot"), Region(Region.OceanSpiderHouseItems)]
        [GossipLocationHint("a creepy basement pot"), GossipItemHint("a quiver refill", "a bundle of projectiles")]
        [ShopText("Ammo for your bow.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x1E2), ItemPool(ItemCategory.Arrows, LocationCategory.Jars), CollectableIndex(0x1430)]
        CollectableOceansideSpiderHouseEntrancePot1,


        [Visible]
        [Repeatable]
        [ItemName("10 Arrows"), LocationName("Oceanside Spider House Entrance Pot 2"), Region(Region.OceanSpiderHouseItems)]
        [GossipLocationHint("a creepy basement pot"), GossipItemHint("a quiver refill", "a bundle of projectiles")]
        [ShopText("Ammo for your bow.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x1E3), ItemPool(ItemCategory.Arrows, LocationCategory.Jars), CollectableIndex(0x1431)]
        CollectableOceansideSpiderHouseEntrancePot2,


        [Visible]
        [Repeatable]
        [ItemName("10 Arrows"), LocationName("Pirates' Fortress Sewer Gate Pot"), Region(Region.PiratesFortressSewer)]
        [GossipLocationHint("the home of pirates"), GossipItemHint("a quiver refill", "a bundle of projectiles")]
        [ShopText("Ammo for your bow.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x1E4), ItemPool(ItemCategory.Arrows, LocationCategory.Jars), CollectableIndex(0x11A5)]
        CollectablePiratesFortressInteriorWaterCurrentRoomPot1,


        [Visible]
        [Repeatable]
        [ItemName("10 Arrows"), LocationName("Pirates' Fortress Guarded Egg Pot"), Region(Region.PiratesFortressInterior)]
        [GossipLocationHint("the home of pirates"), GossipItemHint("a quiver refill", "a bundle of projectiles")]
        [ShopText("Ammo for your bow.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x1E5), ItemPool(ItemCategory.Arrows, LocationCategory.Jars), CollectableIndex(0x11AA)]
        CollectablePiratesFortressInterior100RupeeEggRoomPot1,


        [Visible]
        [Repeatable]
        [ItemName("10 Arrows"), LocationName("Pirates' Fortress Barrel Maze Egg Pot"), Region(Region.PiratesFortressInterior)]
        [GossipLocationHint("the home of pirates"), GossipItemHint("a quiver refill", "a bundle of projectiles")]
        [ShopText("Ammo for your bow.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x1E6), ItemPool(ItemCategory.Arrows, LocationCategory.Jars), CollectableIndex(0x11AB)]
        CollectablePiratesFortressInteriorBarrelRoomEggPot1,


        [Visible]
        [Repeatable]
        [ItemName("10 Arrows"), LocationName("Pirates' Fortress Sewer Exit Pot"), Region(Region.PiratesFortressSewer)]
        [GossipLocationHint("the home of pirates"), GossipItemHint("a quiver refill", "a bundle of projectiles")]
        [ShopText("Ammo for your bow.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x1E7), ItemPool(ItemCategory.Arrows, LocationCategory.Jars), CollectableIndex(0x11AC)]
        CollectablePiratesFortressInteriorTelescopeRoomPot1,


        [Visible]
        [Repeatable]
        [ItemName("10 Arrows"), LocationName("Secret Shrine Underwater Pot"), Region(Region.SecretShrine)]
        [GossipLocationHint("a secret place"), GossipItemHint("a quiver refill", "a bundle of projectiles")]
        [ShopText("Ammo for your bow.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x1E8), ItemPool(ItemCategory.Arrows, LocationCategory.Jars), CollectableIndex(0x307C)]
        CollectableSecretShrineMainRoomPot1,


        [Visible]
        [Repeatable]
        [ItemName("10 Arrows"), LocationName("Secret Shrine Underwater Pot 2"), Region(Region.SecretShrine)]
        [GossipLocationHint("a secret place"), GossipItemHint("a quiver refill", "a bundle of projectiles")]
        [ShopText("Ammo for your bow.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x1E9), ItemPool(ItemCategory.Arrows, LocationCategory.Jars), CollectableIndex(0x307D)]
        CollectableSecretShrineMainRoomPot2,


        [Visible]
        [Repeatable]
        [ItemName("10 Arrows"), LocationName("Snowhead Temple Icicle Room Snowball"), Region(Region.SnowheadTemple)]
        [GossipLocationHint("Snowhead Temple", "an icy gale"), GossipItemHint("a quiver refill", "a bundle of projectiles")]
        [ShopText("Ammo for your bow.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x1EA), ItemPool(ItemCategory.Arrows, LocationCategory.SmallSnowballs), CollectableIndex(0x10A0)]
        CollectableSnowheadTempleIceBlockRoomSmallSnowball1,


        [Visible]
        [Repeatable]
        [ItemName("10 Arrows"), LocationName("Snowhead Temple Icicle Room Snowball 2"), Region(Region.SnowheadTemple)]
        [GossipLocationHint("Snowhead Temple", "an icy gale"), GossipItemHint("a quiver refill", "a bundle of projectiles")]
        [ShopText("Ammo for your bow.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x1EB), ItemPool(ItemCategory.Arrows, LocationCategory.SmallSnowballs), CollectableIndex(0x10A1)]
        CollectableSnowheadTempleIceBlockRoomSmallSnowball2,


        [Visible]
        [Repeatable]
        [ItemName("10 Arrows"), LocationName("Stone Tower Upper Scarecrow Pot"), Region(Region.StoneTower)]
        [GossipLocationHint("a high tower"), GossipItemHint("a quiver refill", "a bundle of projectiles")]
        [ShopText("Ammo for your bow.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x1EC), ItemPool(ItemCategory.Arrows, LocationCategory.Jars), CollectableIndex(0x2C2F)]
        CollectableStoneTowerPot1,


        [Visible]
        [Repeatable]
        [ItemName("10 Arrows"), LocationName("Stone Tower Upper Scarecrow Pot 2"), Region(Region.StoneTower)]
        [GossipLocationHint("a high tower"), GossipItemHint("a quiver refill", "a bundle of projectiles")]
        [ShopText("Ammo for your bow.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x1ED), ItemPool(ItemCategory.Arrows, LocationCategory.Jars), CollectableIndex(0x2C33)]
        CollectableStoneTowerPot2,


        [Visible]
        [Repeatable]
        [ItemName("30 Arrows"), LocationName("Great Bay Coast Pot 5"), Region(Region.GreatBayCoast)]
        [GossipLocationHint("an ocean jar"), GossipItemHint("a quiver refill", "a bundle of projectiles")]
        [ShopText("Ammo for your bow.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x1EE), ItemPool(ItemCategory.Arrows, LocationCategory.Jars), CollectableIndex(0x1BAE)]
        CollectableGreatBayCoastPot5,


        [Visible]
        [Repeatable]
        [ItemName("30 Arrows"), LocationName("Great Bay Temple Seesaw Room Pot"), Region(Region.GreatBayTemple)]
        [GossipLocationHint("Great Bay Temple", "the ocean temple"), GossipItemHint("a quiver refill", "a bundle of projectiles")]
        [ShopText("Ammo for your bow.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x1EF), ItemPool(ItemCategory.Arrows, LocationCategory.Jars), CollectableIndex(0x24A0)]
        CollectableGreatBayTempleSeesawRoomPot1,


        [Visible]
        [Repeatable]
        [ItemName("30 Arrows"), LocationName("Great Bay Temple Green Pump Barrel"), Region(Region.GreatBayTemple)]
        [GossipLocationHint("Great Bay Temple", "the ocean temple"), GossipItemHint("a quiver refill", "a bundle of projectiles")]
        [ShopText("Ammo for your bow.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x1F0), ItemPool(ItemCategory.Arrows, LocationCategory.Barrels), CollectableIndex(0x24A4)]
        CollectableGreatBayTempleTopmostRoomWithGreenValveBarrel1,


        [Visible]
        [Repeatable]
        [ItemName("30 Arrows"), LocationName("Ikana Canyon Grass"), Region(Region.IkanaCanyon)]
        [GossipLocationHint("cursed grass"), GossipItemHint("a quiver refill", "a bundle of projectiles")]
        [ShopText("Ammo for your bow.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x1F1), ItemPool(ItemCategory.Arrows, LocationCategory.Grass), CollectableIndex(0x9C8)]
        CollectableIkanaCanyonMainAreaGrass1,


        [Visible]
        [Repeatable]
        [ItemName("30 Arrows"), LocationName("Milk Road Grass"), Region(Region.MilkRoad)]
        [GossipLocationHint("a roadside plant"), GossipItemHint("a quiver refill", "a bundle of projectiles")]
        [ShopText("Ammo for your bow.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x1F2), ItemPool(ItemCategory.Arrows, LocationCategory.Grass), CollectableIndex(0x1121)]
        CollectableMilkRoadGrass1,


        [Visible]
        [Repeatable]
        [ItemName("30 Arrows"), LocationName("Mountain Village Spring Snowball"), Region(Region.MountainVillage)]
        [GossipLocationHint("a spring snowball"), GossipItemHint("a quiver refill", "a bundle of projectiles")]
        [ShopText("Ammo for your bow.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x1F3), ItemPool(ItemCategory.Arrows, LocationCategory.SmallSnowballs), CollectableIndex(0x2D2A)]
        CollectableMountainVillageSpringSmallSnowball1,


        [Visible]
        [Repeatable]
        [ItemName("30 Arrows"), LocationName("Mountain Village Winter Small Snowball"), Region(Region.MountainVillage)]
        [GossipLocationHint("a village snowball"), GossipItemHint("a quiver refill", "a bundle of projectiles")]
        [ShopText("Ammo for your bow.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x1F4), ItemPool(ItemCategory.Arrows, LocationCategory.SmallSnowballs), CollectableIndex(0x2835)]
        CollectableMountainVillageWinterSmallSnowball1,


        [Visible]
        [Repeatable]
        [ItemName("30 Arrows"), LocationName("Pirates' Fortress Lone Guard Egg Pot"), Region(Region.PiratesFortressInterior)]
        [GossipLocationHint("the home of pirates"), GossipItemHint("a quiver refill", "a bundle of projectiles")]
        [ShopText("Ammo for your bow.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x1F5), ItemPool(ItemCategory.Arrows, LocationCategory.Jars), CollectableIndex(0x11A9)]
        CollectablePiratesFortressInteriorTwinBarrelEggRoomPot1,


        [Visible]
        [Repeatable]
        [ItemName("30 Arrows"), LocationName("Pirates' Fortress Cage Pot"), Region(Region.PiratesFortressSewer)]
        [GossipLocationHint("the home of pirates"), GossipItemHint("a quiver refill", "a bundle of projectiles")]
        [ShopText("Ammo for your bow.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x1F6), ItemPool(ItemCategory.Arrows, LocationCategory.Jars), CollectableIndex(0x1188)]
        CollectablePiratesFortressInteriorCellRoomWithPieceOfHeartPot1,


        [Visible]
        [Repeatable]
        [ItemName("30 Arrows"), LocationName("Ranch Crate"), Region(Region.RomaniRanch)]
        [GossipLocationHint("a ranch container"), GossipItemHint("a quiver refill", "a bundle of projectiles")]
        [ShopText("Ammo for your bow.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x1F7), ItemPool(ItemCategory.Arrows, LocationCategory.Crates), CollectableIndex(0x1AFF)]
        CollectableRomaniRanchWoodenCrateLarge1,


        [Visible]
        [Repeatable]
        [ItemName("30 Arrows"), LocationName("Snowhead Small Snowball"), Region(Region.Snowhead)]
        [GossipLocationHint("a mountain-top snowball"), GossipItemHint("a quiver refill", "a bundle of projectiles")]
        [ShopText("Ammo for your bow.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x1F8), ItemPool(ItemCategory.Arrows, LocationCategory.SmallSnowballs), CollectableIndex(0x2E21), CollectableIndex(0x3920)]
        CollectableSnowheadSmallSnowball1,


        [Visible]
        [Repeatable]
        [ItemName("30 Arrows"), LocationName("Stone Tower Owl Pot"), Region(Region.StoneTower)]
        [GossipLocationHint("a high tower"), GossipItemHint("a quiver refill", "a bundle of projectiles")]
        [ShopText("Ammo for your bow.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x1F9), ItemPool(ItemCategory.Arrows, LocationCategory.Jars), CollectableIndex(0x2C38)]
        CollectableStoneTowerPot3,


        [Visible]
        [Repeatable]
        [ItemName("30 Arrows"), LocationName("Zora Cape Owl Pot"), Region(Region.ZoraCape)]
        [GossipLocationHint("a cape jar"), GossipItemHint("a quiver refill", "a bundle of projectiles")]
        [ShopText("Ammo for your bow.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x1FA), ItemPool(ItemCategory.Arrows, LocationCategory.Jars), CollectableIndex(0x1C23)]
        CollectableZoraCapePot1,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Observatory Scarecrow Pot"), Region(Region.EastClockTown)]
        [GossipLocationHint("an underground jar"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x1FB), ItemPool(ItemCategory.BlueRupees, LocationCategory.Jars), CollectableIndex(0x14A4)]
        CollectableAstralObservatoryObservatoryBombersHideoutPot1,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Observatory Scarecrow Pot 2"), Region(Region.EastClockTown)]
        [GossipLocationHint("an underground jar"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x1FC), ItemPool(ItemCategory.BlueRupees, LocationCategory.Jars), CollectableIndex(0x14A5)]
        CollectableAstralObservatoryObservatoryBombersHideoutPot2,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Deku Palace Item"), Region(Region.DekuPalace)]
        [GossipLocationHint("a royal garden", "the home of scrubs"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x1FD), ItemPool(ItemCategory.BlueRupees, LocationCategory.Freestanding), CollectableIndex(0x1590)]
        CollectableDekuPalaceWestInnerGardenItem1,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Deku Palace Item 2"), Region(Region.DekuPalace)]
        [GossipLocationHint("a royal garden", "the home of scrubs"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x1FE), ItemPool(ItemCategory.BlueRupees, LocationCategory.Freestanding), CollectableIndex(0x1591)]
        CollectableDekuPalaceEastInnerGardenItem1,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Deku Palace Item 3"), Region(Region.DekuPalace)]
        [GossipLocationHint("a royal garden", "the home of scrubs"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x1FF), ItemPool(ItemCategory.BlueRupees, LocationCategory.Freestanding), CollectableIndex(0x1592)]
        CollectableDekuPalaceEastInnerGardenItem2,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Deku Palace Item 4"), Region(Region.DekuPalace)]
        [GossipLocationHint("a royal garden", "the home of scrubs"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x200), ItemPool(ItemCategory.BlueRupees, LocationCategory.Freestanding), CollectableIndex(0x158E)]
        CollectableDekuPalaceWestInnerGardenItem2,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Deku Palace Item 5"), Region(Region.DekuPalace)]
        [GossipLocationHint("a royal garden", "the home of scrubs"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x201), ItemPool(ItemCategory.BlueRupees, LocationCategory.Freestanding), CollectableIndex(0x158F)]
        CollectableDekuPalaceWestInnerGardenItem3,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Doggy Racetrack Pot"), Region(Region.RomaniRanch)]
        [GossipLocationHint("a sporting arena"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x202), ItemPool(ItemCategory.BlueRupees, LocationCategory.Jars), CollectableIndex(0x2081)]
        CollectableDoggyRacetrackPot1,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Doggy Racetrack Pot 2"), Region(Region.RomaniRanch)]
        [GossipLocationHint("a sporting arena"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x203), ItemPool(ItemCategory.BlueRupees, LocationCategory.Jars), CollectableIndex(0x2082)]
        CollectableDoggyRacetrackPot2,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Doggy Racetrack Pot 3"), Region(Region.RomaniRanch)]
        [GossipLocationHint("a sporting arena"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x204), ItemPool(ItemCategory.BlueRupees, LocationCategory.Jars), CollectableIndex(0x2083)]
        CollectableDoggyRacetrackPot3,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Doggy Racetrack Pot 4"), Region(Region.RomaniRanch)]
        [GossipLocationHint("a sporting arena"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x205), ItemPool(ItemCategory.BlueRupees, LocationCategory.Jars), CollectableIndex(0x2084)]
        CollectableDoggyRacetrackPot4,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Goron Village Large Snowball"), Region(Region.GoronVillage)]
        [GossipLocationHint("a village snowball"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x206), ItemPool(ItemCategory.BlueRupees, LocationCategory.LargeSnowballs), CollectableIndex(0x26A0)]
        CollectableGoronVillageWinterLargeSnowball1,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Goron Village Large Snowball 2"), Region(Region.GoronVillage)]
        [GossipLocationHint("a village snowball"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x207), ItemPool(ItemCategory.BlueRupees, LocationCategory.LargeSnowballs), CollectableIndex(0x26A2)]
        CollectableGoronVillageWinterLargeSnowball2,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Goron Village Large Snowball 3"), Region(Region.GoronVillage)]
        [GossipLocationHint("a village snowball"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x208), ItemPool(ItemCategory.BlueRupees, LocationCategory.LargeSnowballs), CollectableIndex(0x26A4)]
        CollectableGoronVillageWinterLargeSnowball3,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Great Bay Coast Ledge Pot"), Region(Region.GreatBayCoast)]
        [GossipLocationHint("a high ocean jar"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x209), ItemPool(ItemCategory.BlueRupees, LocationCategory.Jars), CollectableIndex(0x1B83)]
        CollectableGreatBayCoastPot6,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Great Bay Coast Ledge Pot 2"), Region(Region.GreatBayCoast)]
        [GossipLocationHint("a high ocean jar"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x20A), ItemPool(ItemCategory.BlueRupees, LocationCategory.Jars), CollectableIndex(0x1B84)]
        CollectableGreatBayCoastPot7,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Great Bay Coast Ledge Pot 3"), Region(Region.GreatBayCoast)]
        [GossipLocationHint("a high ocean jar"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x20B), ItemPool(ItemCategory.BlueRupees, LocationCategory.Jars), CollectableIndex(0x1B86)]
        CollectableGreatBayCoastPot8,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Great Bay Temple Water Control Room Item"), Region(Region.GreatBayTemple)]
        [GossipLocationHint("Great Bay Temple", "the ocean temple"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x20C), ItemPool(ItemCategory.BlueRupees, LocationCategory.Freestanding), CollectableIndex(0x2482)]
        CollectableGreatBayTempleWaterControlRoomItem1,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Great Bay Temple Water Control Room Item 2"), Region(Region.GreatBayTemple)]
        [GossipLocationHint("Great Bay Temple", "the ocean temple"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x20D), ItemPool(ItemCategory.BlueRupees, LocationCategory.Freestanding), CollectableIndex(0x2483)]
        CollectableGreatBayTempleWaterControlRoomItem2,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Bio Baba Grotto Hive"), Region(Region.TerminaField)]
        [GossipLocationHint("an underground hive"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x20E), ItemPool(ItemCategory.BlueRupees, LocationCategory.Beehives), CollectableIndex(0x383)]
        CollectableGrottosOceanHeartPieceGrottoBeehive1,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Laundry Pool Crate"), Region(Region.LaundryPool)]
        [GossipLocationHint("a town crate"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x20F), ItemPool(ItemCategory.BlueRupees, LocationCategory.Crates), CollectableIndex(0x3820)]
        CollectableLaundryPoolWoodenCrateSmall1,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Mountain Village Day 3 Snowball"), Region(Region.MountainVillage)]
        [GossipLocationHint("a village snowball"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x210), ItemPool(ItemCategory.BlueRupees, LocationCategory.LargeSnowballs), CollectableIndex(0x2828)]
        CollectableMountainVillageWinterLargeSnowball1,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Mountain Village Day 2 Snowball"), Region(Region.MountainVillage)]
        [GossipLocationHint("a village snowball"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x211), ItemPool(ItemCategory.BlueRupees, LocationCategory.LargeSnowballs), CollectableIndex(0x2829)]
        CollectableMountainVillageWinterLargeSnowball2,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Twin Islands Item"), Region(Region.TwinIslands)]
        [GossipLocationHint("a frozen lake"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x212), ItemPool(ItemCategory.BlueRupees, LocationCategory.Freestanding), CollectableIndex(0x2E81), CollectableIndex(0x2F01)]
        CollectablePathToGoronVillageWinterItem1,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Twin Islands Item 2"), Region(Region.TwinIslands)]
        [GossipLocationHint("a frozen lake"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x213), ItemPool(ItemCategory.BlueRupees, LocationCategory.Freestanding), CollectableIndex(0x2E82), CollectableIndex(0x2F02)]
        CollectablePathToGoronVillageWinterItem2,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Twin Islands Item 3"), Region(Region.TwinIslands)]
        [GossipLocationHint("a frozen lake"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x214), ItemPool(ItemCategory.BlueRupees, LocationCategory.Freestanding), CollectableIndex(0x2E83), CollectableIndex(0x2F03)]
        CollectablePathToGoronVillageWinterItem3,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Twin Islands Item 4"), Region(Region.TwinIslands)]
        [GossipLocationHint("a frozen lake"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x215), ItemPool(ItemCategory.BlueRupees, LocationCategory.Freestanding), CollectableIndex(0x2E84), CollectableIndex(0x2F04)]
        CollectablePathToGoronVillageWinterItem4,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Pirates' Fortress Barrel Maze Egg Pot 2"), Region(Region.PiratesFortressInterior)]
        [GossipLocationHint("the home of pirates"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x216), ItemPool(ItemCategory.BlueRupees, LocationCategory.Jars), CollectableIndex(0x11A3)]
        CollectablePiratesFortressInteriorBarrelRoomEggPot2,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Pirates' Fortress Sewer Exit Barrel"), Region(Region.PiratesFortressSewer)]
        [GossipLocationHint("the home of pirates"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x217), ItemPool(ItemCategory.BlueRupees, LocationCategory.Barrels), CollectableIndex(0x1185)]
        CollectablePiratesFortressInteriorTelescopeRoomItem1,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Pirates' Fortress Sewer Exit Barrel 2"), Region(Region.PiratesFortressSewer)]
        [GossipLocationHint("the home of pirates"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x218), ItemPool(ItemCategory.BlueRupees, LocationCategory.Barrels), CollectableIndex(0x1186)]
        CollectablePiratesFortressInteriorTelescopeRoomItem2,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Pirates' Fortress Sewer Exit Barrel 3"), Region(Region.PiratesFortressSewer)]
        [GossipLocationHint("the home of pirates"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x219), ItemPool(ItemCategory.BlueRupees, LocationCategory.Barrels), CollectableIndex(0x118A)]
        CollectablePiratesFortressInteriorTelescopeRoomItem3,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Pirates' Fortress Cage Room Barrel"), Region(Region.PiratesFortressSewer)]
        [GossipLocationHint("the home of pirates"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x21A), ItemPool(ItemCategory.BlueRupees, LocationCategory.Barrels), CollectableIndex(0x118B)]
        CollectablePiratesFortressInteriorCellRoomWithPieceOfHeartItem1,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Ranch Barn Hay Item"), Region(Region.RomaniRanch)]
        [GossipLocationHint("a bale of hay"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x21B), ItemPool(ItemCategory.BlueRupees, LocationCategory.Freestanding), CollectableIndex(0x801)]
        CollectableRanchHouseBarnBarnItem1,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Ranch Barn Hay Item 2"), Region(Region.RomaniRanch)]
        [GossipLocationHint("a bale of hay"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x21C), ItemPool(ItemCategory.BlueRupees, LocationCategory.Freestanding), CollectableIndex(0x802)]
        CollectableRanchHouseBarnBarnItem2,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Snowhead Temple Icicle Room Snowball 3"), Region(Region.SnowheadTemple)]
        [GossipLocationHint("Snowhead Temple", "an icy gale"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x21D), ItemPool(ItemCategory.BlueRupees, LocationCategory.SmallSnowballs), CollectableIndex(0x10A2)]
        CollectableSnowheadTempleIceBlockRoomSmallSnowball3,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Snowhead Temple Icicle Room Snowball 4"), Region(Region.SnowheadTemple)]
        [GossipLocationHint("Snowhead Temple", "an icy gale"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x21E), ItemPool(ItemCategory.BlueRupees, LocationCategory.SmallSnowballs), CollectableIndex(0x10A3)]
        CollectableSnowheadTempleIceBlockRoomSmallSnowball4,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Snowhead Temple Icicle Room Snowball 5"), Region(Region.SnowheadTemple)]
        [GossipLocationHint("Snowhead Temple", "an icy gale"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x21F), ItemPool(ItemCategory.BlueRupees, LocationCategory.SmallSnowballs), CollectableIndex(0x10A4)]
        CollectableSnowheadTempleIceBlockRoomSmallSnowball5,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Snowhead Temple Elevator Room Crate"), Region(Region.SnowheadTemple)]
        [GossipLocationHint("Snowhead Temple", "an icy gale"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x220), ItemPool(ItemCategory.BlueRupees, LocationCategory.Crates), CollectableIndex(0x10A8)]
        CollectableSnowheadTempleMapRoomWoodenCrateLarge1,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Snowhead Temple Elevator Room Crate 2"), Region(Region.SnowheadTemple)]
        [GossipLocationHint("Snowhead Temple", "an icy gale"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x221), ItemPool(ItemCategory.BlueRupees, LocationCategory.Crates), CollectableIndex(0x10A9)]
        CollectableSnowheadTempleMapRoomWoodenCrateLarge2,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Snowhead Temple Elevator Room Crate 3"), Region(Region.SnowheadTemple)]
        [GossipLocationHint("Snowhead Temple", "an icy gale"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x222), ItemPool(ItemCategory.BlueRupees, LocationCategory.Crates), CollectableIndex(0x10AA)]
        CollectableSnowheadTempleMapRoomWoodenCrateLarge3,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Snowhead Temple Elevator Room Crate 4"), Region(Region.SnowheadTemple)]
        [GossipLocationHint("Snowhead Temple", "an icy gale"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x223), ItemPool(ItemCategory.BlueRupees, LocationCategory.Crates), CollectableIndex(0x10AB)]
        CollectableSnowheadTempleMapRoomWoodenCrateLarge4,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Snowhead Temple Elevator Room Crate 5"), Region(Region.SnowheadTemple)]
        [GossipLocationHint("Snowhead Temple", "an icy gale"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x224), ItemPool(ItemCategory.BlueRupees, LocationCategory.Crates), CollectableIndex(0x10AC)]
        CollectableSnowheadTempleMapRoomWoodenCrateLarge5,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Snowhead Temple Safety Bridge Pot"), Region(Region.SnowheadTemple)]
        [GossipLocationHint("Snowhead Temple", "an icy gale"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x225), ItemPool(ItemCategory.BlueRupees, LocationCategory.Jars), CollectableIndex(0x10AD)]
        CollectableSnowheadTempleMainRoomPot1,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Snowhead Temple Safety Bridge Pot 2"), Region(Region.SnowheadTemple)]
        [GossipLocationHint("Snowhead Temple", "an icy gale"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x226), ItemPool(ItemCategory.BlueRupees, LocationCategory.Jars), CollectableIndex(0x10AE)]
        CollectableSnowheadTempleMainRoomPot2,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Cleared Swamp Potion Shop Pot"), Region(Region.SouthernSwamp)]
        [GossipLocationHint("a swamp jar"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x227), ItemPool(ItemCategory.BlueRupees, LocationCategory.Jars), CollectableIndex(0x21)]
        CollectableSouthernSwampClearMagicHagsPotionShopExteriorPot1,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Swamp Near Frog Item"), Region(Region.SouthernSwamp)]
        [GossipLocationHint("a swamp flower"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x228), ItemPool(ItemCategory.BlueRupees, LocationCategory.Freestanding), CollectableIndex(0x2295), CollectableIndex(0x15)]
        CollectableSouthernSwampPoisonedCentralSwampItem1,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Swamp Near Frog Item 2"), Region(Region.SouthernSwamp)]
        [GossipLocationHint("a swamp flower"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x229), ItemPool(ItemCategory.BlueRupees, LocationCategory.Freestanding), CollectableIndex(0x2296), CollectableIndex(0x16)]
        CollectableSouthernSwampPoisonedCentralSwampItem2,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Potion Shop Pot"), Region(Region.SouthernSwamp)]
        [GossipLocationHint("a swamp jar"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x22A), ItemPool(ItemCategory.BlueRupees, LocationCategory.Jars), CollectableIndex(0x22A6)]
        CollectableSouthernSwampPoisonedMagicHagsPotionShopExteriorPot1,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Stone Tower Temple Lava Room Item"), Region(Region.StoneTowerTemple)]
        [GossipLocationHint("Stone Tower Temple", "the cursed temple"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x22B), ItemPool(ItemCategory.BlueRupees, LocationCategory.Freestanding), CollectableIndex(0xB05)]
        CollectableStoneTowerTempleLavaRoomItem1,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Stone Tower Temple Lava Room Item 2"), Region(Region.StoneTowerTemple)]
        [GossipLocationHint("Stone Tower Temple", "the cursed temple"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x22C), ItemPool(ItemCategory.BlueRupees, LocationCategory.Freestanding), CollectableIndex(0xB06)]
        CollectableStoneTowerTempleLavaRoomItem2,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Stone Tower Temple Thin Bridge Item"), Region(Region.StoneTowerTemple)]
        [GossipLocationHint("Stone Tower Temple", "the cursed temple"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x22D), ItemPool(ItemCategory.BlueRupees, LocationCategory.Freestanding), CollectableIndex(0xB07)]
        CollectableStoneTowerTempleRoomAfterLightArrowsItem1,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Stone Tower Temple Thin Bridge Item 2"), Region(Region.StoneTowerTemple)]
        [GossipLocationHint("Stone Tower Temple", "the cursed temple"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x22E), ItemPool(ItemCategory.BlueRupees, LocationCategory.Freestanding), CollectableIndex(0xB08)]
        CollectableStoneTowerTempleRoomAfterLightArrowsItem2,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Stone Tower Temple Thin Bridge Item 3"), Region(Region.StoneTowerTemple)]
        [GossipLocationHint("Stone Tower Temple", "the cursed temple"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x22F), ItemPool(ItemCategory.BlueRupees, LocationCategory.Freestanding), CollectableIndex(0xB09)]
        CollectableStoneTowerTempleRoomAfterLightArrowsItem3,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Stone Tower Temple Thin Bridge Item 4"), Region(Region.StoneTowerTemple)]
        [GossipLocationHint("Stone Tower Temple", "the cursed temple"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x230), ItemPool(ItemCategory.BlueRupees, LocationCategory.Freestanding), CollectableIndex(0xB0A)]
        CollectableStoneTowerTempleRoomAfterLightArrowsItem4,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Stone Tower Temple Thin Bridge Item 5"), Region(Region.StoneTowerTemple)]
        [GossipLocationHint("Stone Tower Temple", "the cursed temple"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x231), ItemPool(ItemCategory.BlueRupees, LocationCategory.Freestanding), CollectableIndex(0xB0B)]
        CollectableStoneTowerTempleRoomAfterLightArrowsItem5,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Stone Tower Temple Thin Bridge Item 6"), Region(Region.StoneTowerTemple)]
        [GossipLocationHint("Stone Tower Temple", "the cursed temple"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x232), ItemPool(ItemCategory.BlueRupees, LocationCategory.Freestanding), CollectableIndex(0xB0C)]
        CollectableStoneTowerTempleRoomAfterLightArrowsItem6,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Stone Tower Temple Thin Bridge Item 7"), Region(Region.StoneTowerTemple)]
        [GossipLocationHint("Stone Tower Temple", "the cursed temple"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x233), ItemPool(ItemCategory.BlueRupees, LocationCategory.Freestanding), CollectableIndex(0xB0D)]
        CollectableStoneTowerTempleRoomAfterLightArrowsItem7,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Stone Tower Temple Thin Bridge Item 8"), Region(Region.StoneTowerTemple)]
        [GossipLocationHint("Stone Tower Temple", "the cursed temple"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x234), ItemPool(ItemCategory.BlueRupees, LocationCategory.Freestanding), CollectableIndex(0xB0E)]
        CollectableStoneTowerTempleRoomAfterLightArrowsItem8,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Inverted Stone Tower Temple Dexihand Item"), Region(Region.StoneTowerTemple)]
        [GossipLocationHint("Stone Tower Temple", "the cursed temple"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x235), ItemPool(ItemCategory.BlueRupees, LocationCategory.Freestanding), CollectableIndex(0xC01)]
        CollectableStoneTowerTempleInvertedEyegoreRoomItem1,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Inverted Stone Tower Temple Pre-Boss Closest Item"), Region(Region.StoneTowerTemple)]
        [GossipLocationHint("Stone Tower Temple", "the cursed temple"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x236), ItemPool(ItemCategory.BlueRupees, LocationCategory.Freestanding), CollectableIndex(0xC10)]
        CollectableStoneTowerTempleInvertedPreBossRoomItem1,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Inverted Stone Tower Temple Pre-Boss 2nd Closest Item"), Region(Region.StoneTowerTemple)]
        [GossipLocationHint("Stone Tower Temple", "the cursed temple"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x237), ItemPool(ItemCategory.BlueRupees, LocationCategory.Freestanding), CollectableIndex(0xC11)]
        CollectableStoneTowerTempleInvertedPreBossRoomItem2,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Inverted Stone Tower Temple Pre-Boss Item"), Region(Region.StoneTowerTemple)]
        [GossipLocationHint("Stone Tower Temple", "the cursed temple"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x238), ItemPool(ItemCategory.BlueRupees, LocationCategory.Freestanding), CollectableIndex(0xC07)]
        CollectableStoneTowerTempleInvertedPreBossRoomItem3,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Inverted Stone Tower Temple Pre-Boss Item 2"), Region(Region.StoneTowerTemple)]
        [GossipLocationHint("Stone Tower Temple", "the cursed temple"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x239), ItemPool(ItemCategory.BlueRupees, LocationCategory.Freestanding), CollectableIndex(0xC08)]
        CollectableStoneTowerTempleInvertedPreBossRoomItem4,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Inverted Stone Tower Temple Pre-Boss Item 3"), Region(Region.StoneTowerTemple)]
        [GossipLocationHint("Stone Tower Temple", "the cursed temple"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x23A), ItemPool(ItemCategory.BlueRupees, LocationCategory.Freestanding), CollectableIndex(0xC09)]
        CollectableStoneTowerTempleInvertedPreBossRoomItem5,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Inverted Stone Tower Temple Pre-Boss Furthest Item"), Region(Region.StoneTowerTemple)]
        [GossipLocationHint("Stone Tower Temple", "the cursed temple"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x23B), ItemPool(ItemCategory.BlueRupees, LocationCategory.Freestanding), CollectableIndex(0xC0A)]
        CollectableStoneTowerTempleInvertedPreBossRoomItem6,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Inverted Stone Tower Temple Pre-Boss Furthest Item 2"), Region(Region.StoneTowerTemple)]
        [GossipLocationHint("Stone Tower Temple", "the cursed temple"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x23C), ItemPool(ItemCategory.BlueRupees, LocationCategory.Freestanding), CollectableIndex(0xC0B)]
        CollectableStoneTowerTempleInvertedPreBossRoomItem7,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Inverted Stone Tower Temple Pre-Boss 2nd Furthest Item"), Region(Region.StoneTowerTemple)]
        [GossipLocationHint("Stone Tower Temple", "the cursed temple"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x23D), ItemPool(ItemCategory.BlueRupees, LocationCategory.Freestanding), CollectableIndex(0xC0C)]
        CollectableStoneTowerTempleInvertedPreBossRoomItem8,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Inverted Stone Tower Temple Pre-Boss 2nd Furthest Item 2"), Region(Region.StoneTowerTemple)]
        [GossipLocationHint("Stone Tower Temple", "the cursed temple"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x23E), ItemPool(ItemCategory.BlueRupees, LocationCategory.Freestanding), CollectableIndex(0xC0D)]
        CollectableStoneTowerTempleInvertedPreBossRoomItem9,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Inverted Stone Tower Temple Pre-Boss Closest Item 2"), Region(Region.StoneTowerTemple)]
        [GossipLocationHint("Stone Tower Temple", "the cursed temple"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x23F), ItemPool(ItemCategory.BlueRupees, LocationCategory.Freestanding), CollectableIndex(0xC0E)]
        CollectableStoneTowerTempleInvertedPreBossRoomItem10,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Inverted Stone Tower Temple Pre-Boss 2nd Closest Item 2"), Region(Region.StoneTowerTemple)]
        [GossipLocationHint("Stone Tower Temple", "the cursed temple"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x240), ItemPool(ItemCategory.BlueRupees, LocationCategory.Freestanding), CollectableIndex(0xC0F)]
        CollectableStoneTowerTempleInvertedPreBossRoomItem11,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Swordsman's School Pot"), Region(Region.WestClockTown)]
        [GossipLocationHint("cowering"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x241), ItemPool(ItemCategory.BlueRupees, LocationCategory.Jars), CollectableIndex(0x2A01)]
        CollectableSwordsmanSSchoolPot1,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Swordsman's School Pot 2"), Region(Region.WestClockTown)]
        [GossipLocationHint("cowering"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x242), ItemPool(ItemCategory.BlueRupees, LocationCategory.Jars), CollectableIndex(0x2A03)]
        CollectableSwordsmanSSchoolPot2,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Swordsman's School Pot 3"), Region(Region.WestClockTown)]
        [GossipLocationHint("cowering"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x243), ItemPool(ItemCategory.BlueRupees, LocationCategory.Jars), CollectableIndex(0x2A04)]
        CollectableSwordsmanSSchoolPot3,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Swordsman's School Pot 4"), Region(Region.WestClockTown)]
        [GossipLocationHint("cowering"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x244), ItemPool(ItemCategory.BlueRupees, LocationCategory.Jars), CollectableIndex(0x2A05)]
        CollectableSwordsmanSSchoolPot4,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Swordsman's School Pot 5"), Region(Region.WestClockTown)]
        [GossipLocationHint("cowering"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x245), ItemPool(ItemCategory.BlueRupees, LocationCategory.Jars), CollectableIndex(0x2A06)]
        CollectableSwordsmanSSchoolPot5,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Woodfall Item"), Region(Region.Woodfall)]
        [GossipLocationHint("a poisoned stump"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x246), ItemPool(ItemCategory.BlueRupees, LocationCategory.Freestanding), CollectableIndex(0x2301)]
        CollectableWoodfallItem1,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Woodfall Temple Entrance Hive"), Region(Region.WoodfallTemple)]
        [GossipLocationHint("Woodfall Temple", "the sleeping temple"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x247), ItemPool(ItemCategory.BlueRupees, LocationCategory.Beehives), CollectableIndex(0xDA0)]
        CollectableWoodfallTempleEntranceRoomBeehive1,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Woodfall Temple Gekko Room Pot"), Region(Region.WoodfallTemple)]
        [GossipLocationHint("Woodfall Temple", "the sleeping temple"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x248), ItemPool(ItemCategory.BlueRupees, LocationCategory.Jars), CollectableIndex(0xDA6)]
        CollectableWoodfallTempleGekkoRoomPot1,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Woodfall Temple Gekko Room Pot 2"), Region(Region.WoodfallTemple)]
        [GossipLocationHint("Woodfall Temple", "the sleeping temple"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x249), ItemPool(ItemCategory.BlueRupees, LocationCategory.Jars), CollectableIndex(0xDA7)]
        CollectableWoodfallTempleGekkoRoomPot2,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Woodfall Temple Gekko Room Pot 3"), Region(Region.WoodfallTemple)]
        [GossipLocationHint("Woodfall Temple", "the sleeping temple"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x24A), ItemPool(ItemCategory.BlueRupees, LocationCategory.Jars), CollectableIndex(0xDA8)]
        CollectableWoodfallTempleGekkoRoomPot3,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Woodfall Temple Gekko Room Pot 4"), Region(Region.WoodfallTemple)]
        [GossipLocationHint("Woodfall Temple", "the sleeping temple"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x24B), ItemPool(ItemCategory.BlueRupees, LocationCategory.Jars), CollectableIndex(0xDA9)]
        CollectableWoodfallTempleGekkoRoomPot4,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Woodfall Temple Pre-Boss Platform Item"), Region(Region.WoodfallTemple)]
        [GossipLocationHint("Woodfall Temple", "the sleeping temple"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x24C), ItemPool(ItemCategory.BlueRupees, LocationCategory.Freestanding), CollectableIndex(0xD83)]
        CollectableWoodfallTemplePreBossRoomItem1,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Woodfall Temple Pre-Boss Platform Item 2"), Region(Region.WoodfallTemple)]
        [GossipLocationHint("Woodfall Temple", "the sleeping temple"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x24D), ItemPool(ItemCategory.BlueRupees, LocationCategory.Freestanding), CollectableIndex(0xD84)]
        CollectableWoodfallTemplePreBossRoomItem2,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Woodfall Temple Pre-Boss Platform Item 3"), Region(Region.WoodfallTemple)]
        [GossipLocationHint("Woodfall Temple", "the sleeping temple"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x24E), ItemPool(ItemCategory.BlueRupees, LocationCategory.Freestanding), CollectableIndex(0xD85)]
        CollectableWoodfallTemplePreBossRoomItem3,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Woodfall Temple Pre-Boss Platform Item 4"), Region(Region.WoodfallTemple)]
        [GossipLocationHint("Woodfall Temple", "the sleeping temple"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x24F), ItemPool(ItemCategory.BlueRupees, LocationCategory.Freestanding), CollectableIndex(0xD86)]
        CollectableWoodfallTemplePreBossRoomItem4,


        [Visible]
        [Repeatable]
        [ItemName("5 Bombs"), LocationName("Well Left Path Pot"), Region(Region.BeneathTheWell)]
        [GossipLocationHint("a cursed pot"), GossipItemHint("explosives")]
        [ShopText("Explosives. You need a Bomb Bag to carry them.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x250), ItemPool(ItemCategory.Bombs, LocationCategory.Jars), CollectableIndex(0x25A0)]
        CollectableBeneathTheWellBugAndBombRoomPot1,


        [Visible]
        [Repeatable]
        [ItemName("5 Bombs"), LocationName("Well Left Path Pot 2"), Region(Region.BeneathTheWell)]
        [GossipLocationHint("a cursed pot"), GossipItemHint("explosives")]
        [ShopText("Explosives. You need a Bomb Bag to carry them.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x251), ItemPool(ItemCategory.Bombs, LocationCategory.Jars), CollectableIndex(0x25A1)]
        CollectableBeneathTheWellBugAndBombRoomPot2,


        [Visible]
        [Repeatable]
        [ItemName("5 Bombs"), LocationName("Well Left Path Pot 3"), Region(Region.BeneathTheWell)]
        [GossipLocationHint("a cursed pot"), GossipItemHint("explosives")]
        [ShopText("Explosives. You need a Bomb Bag to carry them.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x252), ItemPool(ItemCategory.Bombs, LocationCategory.Jars), CollectableIndex(0x25A2)]
        CollectableBeneathTheWellBugAndBombRoomPot3,


        [Visible]
        [Repeatable]
        [ItemName("5 Bombs"), LocationName("Well Left Path Pot 4"), Region(Region.BeneathTheWell)]
        [GossipLocationHint("a cursed pot"), GossipItemHint("explosives")]
        [ShopText("Explosives. You need a Bomb Bag to carry them.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x253), ItemPool(ItemCategory.Bombs, LocationCategory.Jars), CollectableIndex(0x25A3)]
        CollectableBeneathTheWellBugAndBombRoomPot4,


        [Visible]
        [Repeatable]
        [ItemName("5 Bombs"), LocationName("Well Left Path Pot 5"), Region(Region.BeneathTheWell)]
        [GossipLocationHint("a cursed pot"), GossipItemHint("explosives")]
        [ShopText("Explosives. You need a Bomb Bag to carry them.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x254), ItemPool(ItemCategory.Bombs, LocationCategory.Jars), CollectableIndex(0x25A4)]
        CollectableBeneathTheWellBugAndBombRoomPot5,


        [Visible]
        [Repeatable]
        [ItemName("5 Bombs"), LocationName("Goron Village Small Snowball 3"), Region(Region.GoronVillage)]
        [GossipLocationHint("a village snowball"), GossipItemHint("explosives")]
        [ShopText("Explosives. You need a Bomb Bag to carry them.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x255), ItemPool(ItemCategory.Bombs, LocationCategory.SmallSnowballs), CollectableIndex(0x26AB)]
        CollectableGoronVillageWinterSmallSnowball3,


        [Visible]
        [Repeatable]
        [ItemName("5 Bombs"), LocationName("Goron Village Small Snowball 4"), Region(Region.GoronVillage)]
        [GossipLocationHint("a village snowball"), GossipItemHint("explosives")]
        [ShopText("Explosives. You need a Bomb Bag to carry them.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x256), ItemPool(ItemCategory.Bombs, LocationCategory.SmallSnowballs), CollectableIndex(0x26B0)]
        CollectableGoronVillageWinterSmallSnowball4,


        [Visible]
        [Repeatable]
        [ItemName("5 Bombs"), LocationName("Great Bay Coast Pot 6"), Region(Region.GreatBayCoast)]
        [GossipLocationHint("an ocean jar"), GossipItemHint("explosives")]
        [ShopText("Explosives. You need a Bomb Bag to carry them.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x257), ItemPool(ItemCategory.Bombs, LocationCategory.Jars), CollectableIndex(0x1BAD)]
        CollectableGreatBayCoastPot9,


        [Visible]
        [Repeatable]
        [ItemName("5 Bombs"), LocationName("Great Bay Temple Red Valve Barrel 2"), Region(Region.GreatBayTemple)]
        [GossipLocationHint("Great Bay Temple", "the ocean temple"), GossipItemHint("explosives")]
        [ShopText("Explosives. You need a Bomb Bag to carry them.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x258), ItemPool(ItemCategory.Bombs, LocationCategory.Barrels), CollectableIndex(0x24A2)]
        CollectableGreatBayTempleBlueChuchuValveRoomBarrel2,


        [Visible]
        [Repeatable]
        [ItemName("5 Bombs"), LocationName("Great Bay Temple Green Pump Barrel 2"), Region(Region.GreatBayTemple)]
        [GossipLocationHint("Great Bay Temple", "the ocean temple"), GossipItemHint("explosives")]
        [ShopText("Explosives. You need a Bomb Bag to carry them.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x259), ItemPool(ItemCategory.Bombs, LocationCategory.Barrels), CollectableIndex(0x24A5)]
        CollectableGreatBayTempleTopmostRoomWithGreenValveBarrel2,


        [Visible]
        [Repeatable]
        [ItemName("5 Bombs"), LocationName("Ikana Canyon Grass 2"), Region(Region.IkanaCanyon)]
        [GossipLocationHint("cursed grass"), GossipItemHint("explosives")]
        [ShopText("Explosives. You need a Bomb Bag to carry them.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x25A), ItemPool(ItemCategory.Bombs, LocationCategory.Grass), CollectableIndex(0x9C7)]
        CollectableIkanaCanyonMainAreaGrass2,


        [Visible]
        [Repeatable]
        [ItemName("5 Bombs"), LocationName("Mountain Village Spring Snowball 2"), Region(Region.MountainVillage)]
        [GossipLocationHint("a spring snowball"), GossipItemHint("explosives")]
        [ShopText("Explosives. You need a Bomb Bag to carry them.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x25B), ItemPool(ItemCategory.Bombs, LocationCategory.SmallSnowballs), CollectableIndex(0x2D29)]
        CollectableMountainVillageSpringSmallSnowball2,


        [Visible]
        [Repeatable]
        [ItemName("5 Bombs"), LocationName("Mountain Village Winter Small Snowball 2"), Region(Region.MountainVillage)]
        [GossipLocationHint("a village snowball"), GossipItemHint("explosives")]
        [ShopText("Explosives. You need a Bomb Bag to carry them.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x25C), ItemPool(ItemCategory.Bombs, LocationCategory.SmallSnowballs), CollectableIndex(0x2834)]
        CollectableMountainVillageWinterSmallSnowball2,


        [Visible]
        [Repeatable]
        [ItemName("5 Bombs"), LocationName("Snowhead Small Snowball 2"), Region(Region.Snowhead)]
        [GossipLocationHint("a mountain-top snowball"), GossipItemHint("explosives")]
        [ShopText("Explosives. You need a Bomb Bag to carry them.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x25D), ItemPool(ItemCategory.Bombs, LocationCategory.SmallSnowballs), CollectableIndex(0x2E20), CollectableIndex(0x3929)]
        CollectableSnowheadSmallSnowball2,


        [Visible]
        [Repeatable]
        [ItemName("5 Bombs"), LocationName("Stone Tower Owl Pot 2"), Region(Region.StoneTower)]
        [GossipLocationHint("a high tower"), GossipItemHint("explosives")]
        [ShopText("Explosives. You need a Bomb Bag to carry them.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x25E), ItemPool(ItemCategory.Bombs, LocationCategory.Jars), CollectableIndex(0x2C37)]
        CollectableStoneTowerPot4,


        [Visible]
        [Repeatable]
        [ItemName("5 Bombs"), LocationName("Inverted Stone Tower Pot"), Region(Region.StoneTower)]
        [GossipLocationHint("a sky below"), GossipItemHint("explosives")]
        [ShopText("Explosives. You need a Bomb Bag to carry them.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x25F), ItemPool(ItemCategory.Bombs, LocationCategory.Jars), CollectableIndex(0x2CBA)]
        CollectableStoneTowerInvertedStoneTowerFlippedPot1,


        [Visible]
        [Repeatable]
        [ItemName("5 Bombs"), LocationName("Zora Cape Owl Pot 2"), Region(Region.ZoraCape)]
        [GossipLocationHint("a cape jar"), GossipItemHint("explosives")]
        [ShopText("Explosives. You need a Bomb Bag to carry them.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x260), ItemPool(ItemCategory.Bombs, LocationCategory.Jars), CollectableIndex(0x1C22)]
        CollectableZoraCapePot2,


        [Visible]
        [Repeatable]
        [ItemName("10 Deku Nuts"), LocationName("Ikana Castle Left Staircase Pot"), Region(Region.IkanaCastle)]
        [GossipLocationHint("an ancient jar"), GossipItemHint("a flashing impact")]
        [ShopText("Its flash blinds enemies.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x261), ItemPool(ItemCategory.DekuNuts, LocationCategory.Jars), CollectableIndex(0xEC4)]
        CollectableAncientCastleOfIkana1FWestStaircasePot1,


        [Visible]
        [Repeatable]
        [ItemName("10 Deku Nuts"), LocationName("Goron Village Small Snowball 5"), Region(Region.GoronVillage)]
        [GossipLocationHint("a village snowball"), GossipItemHint("a flashing impact")]
        [ShopText("Its flash blinds enemies.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x262), ItemPool(ItemCategory.DekuNuts, LocationCategory.SmallSnowballs), CollectableIndex(0x26AD)]
        CollectableGoronVillageWinterSmallSnowball5,


        [Visible]
        [Repeatable]
        [ItemName("10 Deku Nuts"), LocationName("Goron Village Small Snowball 6"), Region(Region.GoronVillage)]
        [GossipLocationHint("a village snowball"), GossipItemHint("a flashing impact")]
        [ShopText("Its flash blinds enemies.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x263), ItemPool(ItemCategory.DekuNuts, LocationCategory.SmallSnowballs), CollectableIndex(0x26B1)]
        CollectableGoronVillageWinterSmallSnowball6,


        [Visible]
        [Repeatable]
        [ItemName("10 Deku Nuts"), LocationName("Pirates' Fortress Sewer Exit Pot 2"), Region(Region.PiratesFortressSewer)]
        [GossipLocationHint("the home of pirates"), GossipItemHint("a flashing impact")]
        [ShopText("Its flash blinds enemies.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x264), ItemPool(ItemCategory.DekuNuts, LocationCategory.Jars), CollectableIndex(0x11A8)]
        CollectablePiratesFortressInteriorTelescopeRoomPot2,


        [Visible]
        [Repeatable]
        [ItemName("10 Deku Nuts"), LocationName("Woodfall Pot"), Region(Region.Woodfall)]
        [GossipLocationHint("a poisoned platform"), GossipItemHint("a flashing impact")]
        [ShopText("Its flash blinds enemies.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x265), ItemPool(ItemCategory.DekuNuts, LocationCategory.Jars), CollectableIndex(0x2322)]
        CollectableWoodfallPot1,


        [Visible]
        [Repeatable]
        [ItemName("Deku Stick"), LocationName("Goron Shrine Pot"), Region(Region.GoronVillage)]
        [GossipLocationHint("a crying child's jar"), GossipItemHint("a flammable weapon", "a flimsy weapon")]
        [ShopText("Deku Sticks burn well. You can only carry 10.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x266), ItemPool(ItemCategory.DekuSticks, LocationCategory.Jars), CollectableIndex(0x1960)]
        CollectableGoronShrineGoronKidSRoomPot1,


        [Visible]
        [Repeatable]
        [ItemName("Deku Stick"), LocationName("Goron Shrine Pot 2"), Region(Region.GoronVillage)]
        [GossipLocationHint("a crying child's jar"), GossipItemHint("a flammable weapon", "a flimsy weapon")]
        [ShopText("Deku Sticks burn well. You can only carry 10.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x267), ItemPool(ItemCategory.DekuSticks, LocationCategory.Jars), CollectableIndex(0x1961)]
        CollectableGoronShrineGoronKidSRoomPot2,


        [Visible]
        [Repeatable]
        [ItemName("Deku Stick"), LocationName("Goron Shrine Pot 3"), Region(Region.GoronVillage)]
        [GossipLocationHint("a crying child's jar"), GossipItemHint("a flammable weapon", "a flimsy weapon")]
        [ShopText("Deku Sticks burn well. You can only carry 10.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x268), ItemPool(ItemCategory.DekuSticks, LocationCategory.Jars), CollectableIndex(0x1963)]
        CollectableGoronShrineMainRoomPot1,


        [Visible]
        [Repeatable]
        [ItemName("Deku Stick"), LocationName("Goron Shrine Pot 4"), Region(Region.GoronVillage)]
        [GossipLocationHint("a crying child's jar"), GossipItemHint("a flammable weapon", "a flimsy weapon")]
        [ShopText("Deku Sticks burn well. You can only carry 10.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x269), ItemPool(ItemCategory.DekuSticks, LocationCategory.Jars), CollectableIndex(0x1966)]
        CollectableGoronShrineMainRoomPot2,


        [Visible]
        [Repeatable]
        [ItemName("Deku Stick"), LocationName("Goron Shrine Pot 5"), Region(Region.GoronVillage)]
        [GossipLocationHint("a crying child's jar"), GossipItemHint("a flammable weapon", "a flimsy weapon")]
        [ShopText("Deku Sticks burn well. You can only carry 10.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x26A), ItemPool(ItemCategory.DekuSticks, LocationCategory.Jars), CollectableIndex(0x196A)]
        CollectableGoronShrineMainRoomPot3,


        [Visible]
        [Repeatable]
        [ItemName("Deku Stick"), LocationName("Goron Village Small Snowball 7"), Region(Region.GoronVillage)]
        [GossipLocationHint("a village snowball"), GossipItemHint("a flammable weapon", "a flimsy weapon")]
        [ShopText("Deku Sticks burn well. You can only carry 10.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x26B), ItemPool(ItemCategory.DekuSticks, LocationCategory.SmallSnowballs), CollectableIndex(0x26AC)]
        CollectableGoronVillageWinterSmallSnowball7,


        [Visible]
        [Repeatable]
        [ItemName("Deku Stick"), LocationName("Goron Village Small Snowball 8"), Region(Region.GoronVillage)]
        [GossipLocationHint("a village snowball"), GossipItemHint("a flammable weapon", "a flimsy weapon")]
        [ShopText("Deku Sticks burn well. You can only carry 10.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x26C), ItemPool(ItemCategory.DekuSticks, LocationCategory.SmallSnowballs), CollectableIndex(0x26AF)]
        CollectableGoronVillageWinterSmallSnowball8,


        [Visible]
        [Repeatable]
        [ItemName("Deku Stick"), LocationName("Cleared Swamp Owl Grass"), Region(Region.SouthernSwamp)]
        [GossipLocationHint("swamp grass"), GossipItemHint("a flammable weapon", "a flimsy weapon")]
        [ShopText("Deku Sticks burn well. You can only carry 10.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x26D), ItemPool(ItemCategory.DekuSticks, LocationCategory.Grass), CollectableIndex(0x26)]
        CollectableSouthernSwampClearCentralSwampGrass1,


        [Visible]
        [Repeatable]
        [ItemName("Deku Stick"), LocationName("Southern Swamp Owl Grass"), Region(Region.SouthernSwamp)]
        [GossipLocationHint("swamp grass"), GossipItemHint("a flammable weapon", "a flimsy weapon")]
        [ShopText("Deku Sticks burn well. You can only carry 10.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x26E), ItemPool(ItemCategory.DekuSticks, LocationCategory.Grass), CollectableIndex(0x22AC)]
        CollectableSouthernSwampPoisonedCentralSwampGrass1,


        [Visible]
        [Repeatable]
        [ItemName("Deku Stick"), LocationName("Woodfall Pot 2"), Region(Region.Woodfall)]
        [GossipLocationHint("a poisoned platform"), GossipItemHint("a flammable weapon", "a flimsy weapon")]
        [ShopText("Deku Sticks burn well. You can only carry 10.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x26F), ItemPool(ItemCategory.DekuSticks, LocationCategory.Jars), CollectableIndex(0x2320)]
        CollectableWoodfallPot2,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Dampe's Basement Pot 5"), Region(Region.IkanaGraveyard)]
        [GossipLocationHint("an unholy jar"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x270), ItemPool(ItemCategory.GreenRupees, LocationCategory.Jars), CollectableIndex(0x1805)]
        CollectableDampesHouseBasementPot5,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Dampe's Basement Pot 6"), Region(Region.IkanaGraveyard)]
        [GossipLocationHint("an unholy jar"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x271), ItemPool(ItemCategory.GreenRupees, LocationCategory.Jars), CollectableIndex(0x1806)]
        CollectableDampesHouseBasementPot6,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Dampe's Basement Pot 7"), Region(Region.IkanaGraveyard)]
        [GossipLocationHint("an unholy jar"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x272), ItemPool(ItemCategory.GreenRupees, LocationCategory.Jars), CollectableIndex(0x1809)]
        CollectableDampesHouseBasementPot7,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Deku Palace Item 6"), Region(Region.DekuPalace)]
        [GossipLocationHint("a royal garden", "the home of scrubs"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x273), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), CollectableIndex(0x1581)]
        CollectableDekuPalaceEastInnerGardenItem3,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Deku Palace Item 7"), Region(Region.DekuPalace)]
        [GossipLocationHint("a royal garden", "the home of scrubs"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x274), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), CollectableIndex(0x1582)]
        CollectableDekuPalaceEastInnerGardenItem4,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Deku Palace Item 8"), Region(Region.DekuPalace)]
        [GossipLocationHint("a royal garden", "the home of scrubs"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x275), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), CollectableIndex(0x1583)]
        CollectableDekuPalaceEastInnerGardenItem5,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Deku Palace Item 9"), Region(Region.DekuPalace)]
        [GossipLocationHint("a royal garden", "the home of scrubs"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x276), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), CollectableIndex(0x1584)]
        CollectableDekuPalaceEastInnerGardenItem6,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Deku Palace Item 10"), Region(Region.DekuPalace)]
        [GossipLocationHint("a royal garden", "the home of scrubs"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x277), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), CollectableIndex(0x1585)]
        CollectableDekuPalaceEastInnerGardenItem7,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Deku Palace Item 11"), Region(Region.DekuPalace)]
        [GossipLocationHint("a royal garden", "the home of scrubs"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x278), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), CollectableIndex(0x1586)]
        CollectableDekuPalaceEastInnerGardenItem8,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Deku Palace Item 12"), Region(Region.DekuPalace)]
        [GossipLocationHint("a royal garden", "the home of scrubs"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x279), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), CollectableIndex(0x1587)]
        CollectableDekuPalaceWestInnerGardenItem4,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Deku Palace Item 13"), Region(Region.DekuPalace)]
        [GossipLocationHint("a royal garden", "the home of scrubs"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x27A), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), CollectableIndex(0x1588)]
        CollectableDekuPalaceWestInnerGardenItem5,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Deku Palace Item 14"), Region(Region.DekuPalace)]
        [GossipLocationHint("a royal garden", "the home of scrubs"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x27B), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), CollectableIndex(0x1589)]
        CollectableDekuPalaceWestInnerGardenItem6,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Deku Palace Out of Bounds Item"), Region(Region.DekuPalace)]
        [GossipLocationHint("a hidden royal treasure"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x27C), ItemPool(ItemCategory.GreenRupees, LocationCategory.GlitchesRequired), CollectableIndex(0x158A)]
        CollectableDekuPalaceWestInnerGardenItem7,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Deku Palace Item 15"), Region(Region.DekuPalace)]
        [GossipLocationHint("a royal garden", "the home of scrubs"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x27D), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), CollectableIndex(0x158B)]
        CollectableDekuPalaceWestInnerGardenItem8,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Deku Palace Item 16"), Region(Region.DekuPalace)]
        [GossipLocationHint("a royal garden", "the home of scrubs"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x27E), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), CollectableIndex(0x158C)]
        CollectableDekuPalaceWestInnerGardenItem9,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Deku Palace Item 17"), Region(Region.DekuPalace)]
        [GossipLocationHint("a royal garden", "the home of scrubs"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x27F), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), CollectableIndex(0x158D)]
        CollectableDekuPalaceWestInnerGardenItem10,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Butler Race Pillar Item"), Region(Region.ButlerRaceItems)]
        [GossipLocationHint("a royal race"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x280), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), CollectableIndex(0x2920)]
        CollectableDekuShrineGiantRoomFloor1Item1,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Butler Race Pillar Item 2"), Region(Region.ButlerRaceItems)]
        [GossipLocationHint("a royal race"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x281), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), CollectableIndex(0x2921)]
        CollectableDekuShrineGiantRoomFloor1Item2,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Butler Race Pillar Item 3"), Region(Region.ButlerRaceItems)]
        [GossipLocationHint("a royal race"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x282), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), CollectableIndex(0x2922)]
        CollectableDekuShrineGiantRoomFloor1Item3,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Butler Race Pillar Item 4"), Region(Region.ButlerRaceItems)]
        [GossipLocationHint("a royal race"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x283), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), CollectableIndex(0x2923)]
        CollectableDekuShrineGiantRoomFloor1Item4,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Butler Race Pillar Item 5"), Region(Region.ButlerRaceItems)]
        [GossipLocationHint("a royal race"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x284), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), CollectableIndex(0x2924)]
        CollectableDekuShrineGiantRoomFloor1Item5,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Butler Race Pillar Item 6"), Region(Region.ButlerRaceItems)]
        [GossipLocationHint("a royal race"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x285), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), CollectableIndex(0x2925)]
        CollectableDekuShrineGiantRoomFloor1Item6,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Butler Race River Item"), Region(Region.ButlerRaceItems)]
        [GossipLocationHint("a royal race"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x286), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), CollectableIndex(0x2926)]
        CollectableDekuShrineWaterRoomWithPlatformsItem1,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Butler Race River Item 2"), Region(Region.ButlerRaceItems)]
        [GossipLocationHint("a royal race"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x287), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), CollectableIndex(0x2927)]
        CollectableDekuShrineWaterRoomWithPlatformsItem2,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Butler Race River Item 3"), Region(Region.ButlerRaceItems)]
        [GossipLocationHint("a royal race"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x288), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), CollectableIndex(0x2928)]
        CollectableDekuShrineWaterRoomWithPlatformsItem3,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Butler Race River Item 4"), Region(Region.ButlerRaceItems)]
        [GossipLocationHint("a royal race"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x289), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), CollectableIndex(0x2929)]
        CollectableDekuShrineWaterRoomWithPlatformsItem4,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Butler Race River Item 5"), Region(Region.ButlerRaceItems)]
        [GossipLocationHint("a royal race"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x28A), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), CollectableIndex(0x292A)]
        CollectableDekuShrineWaterRoomWithPlatformsItem5,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Butler Race River Item 6"), Region(Region.ButlerRaceItems)]
        [GossipLocationHint("a royal race"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x28B), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), CollectableIndex(0x292B)]
        CollectableDekuShrineWaterRoomWithPlatformsItem6,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Butler Race Right Path Item"), Region(Region.ButlerRaceItems)]
        [GossipLocationHint("a royal race"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x28C), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), CollectableIndex(0x292C)]
        CollectableDekuShrineRoomBeforeFlameWallsItem1,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Butler Race Right Path Item 2"), Region(Region.ButlerRaceItems)]
        [GossipLocationHint("a royal race"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x28D), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), CollectableIndex(0x292D)]
        CollectableDekuShrineRoomBeforeFlameWallsItem2,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Butler Race Right Path Item 3"), Region(Region.ButlerRaceItems)]
        [GossipLocationHint("a royal race"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x28E), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), CollectableIndex(0x292E)]
        CollectableDekuShrineRoomBeforeFlameWallsItem3,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Butler Race Right Path Item 4"), Region(Region.ButlerRaceItems)]
        [GossipLocationHint("a royal race"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x28F), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), CollectableIndex(0x292F)]
        CollectableDekuShrineRoomBeforeFlameWallsItem4,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Butler Race Right Path Item 5"), Region(Region.ButlerRaceItems)]
        [GossipLocationHint("a royal race"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x290), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), CollectableIndex(0x2930)]
        CollectableDekuShrineRoomBeforeFlameWallsItem5,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Butler Race Right Path Item 6"), Region(Region.ButlerRaceItems)]
        [GossipLocationHint("a royal race"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x291), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), CollectableIndex(0x2931)]
        CollectableDekuShrineRoomBeforeFlameWallsItem6,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Butler Race Final Room Item"), Region(Region.ButlerRaceItems)]
        [GossipLocationHint("a royal race"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x292), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), CollectableIndex(0x2932)]
        CollectableDekuShrineDekuButlerSRoomItem1,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Butler Race Final Room Item 2"), Region(Region.ButlerRaceItems)]
        [GossipLocationHint("a royal race"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x293), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), CollectableIndex(0x2933)]
        CollectableDekuShrineDekuButlerSRoomItem2,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Butler Race Final Room Item 3"), Region(Region.ButlerRaceItems)]
        [GossipLocationHint("a royal race"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x294), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), CollectableIndex(0x2934)]
        CollectableDekuShrineDekuButlerSRoomItem3,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Butler Race Final Room Item 4"), Region(Region.ButlerRaceItems)]
        [GossipLocationHint("a royal race"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x295), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), CollectableIndex(0x2935)]
        CollectableDekuShrineDekuButlerSRoomItem4,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Butler Race Final Room Item 5"), Region(Region.ButlerRaceItems)]
        [GossipLocationHint("a royal race"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x296), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), CollectableIndex(0x2936)]
        CollectableDekuShrineDekuButlerSRoomItem5,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Butler Race Final Room Item 6"), Region(Region.ButlerRaceItems)]
        [GossipLocationHint("a royal race"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x297), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), CollectableIndex(0x2937)]
        CollectableDekuShrineDekuButlerSRoomItem6,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Butler Race Final Room Item 7"), Region(Region.ButlerRaceItems)]
        [GossipLocationHint("a royal race"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x298), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), CollectableIndex(0x2938)]
        CollectableDekuShrineDekuButlerSRoomItem7,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Butler Race Final Room Item 8"), Region(Region.ButlerRaceItems)]
        [GossipLocationHint("a royal race"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x299), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), CollectableIndex(0x2939)]
        CollectableDekuShrineDekuButlerSRoomItem8,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Butler Race Final Room Item 9"), Region(Region.ButlerRaceItems)]
        [GossipLocationHint("a royal race"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x29A), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), CollectableIndex(0x293A)]
        CollectableDekuShrineDekuButlerSRoomItem9,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Butler Race Final Room Item 10"), Region(Region.ButlerRaceItems)]
        [GossipLocationHint("a royal race"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x29B), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), CollectableIndex(0x293B)]
        CollectableDekuShrineDekuButlerSRoomItem10,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Butler Race Dual Pot"), Region(Region.ButlerRaceItems)]
        [GossipLocationHint("a royal race"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x29C), ItemPool(ItemCategory.GreenRupees, LocationCategory.Jars), CollectableIndex(0x297F)]
        CollectableDekuShrineGreyBoulderRoomPot1,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("East Clock Town Crate"), Region(Region.EastClockTown)]
        [GossipLocationHint("a town crate"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x29D), ItemPool(ItemCategory.GreenRupees, LocationCategory.Crates), CollectableIndex(0x3601)]
        CollectableEastClockTownWoodenCrateSmall1,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Great Bay Temple Water Control Room Item 3"), Region(Region.GreatBayTemple)]
        [GossipLocationHint("Great Bay Temple", "the ocean temple"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x29E), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), CollectableIndex(0x2484)]
        CollectableGreatBayTempleWaterControlRoomItem3,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Great Bay Temple Water Control Room Item 4"), Region(Region.GreatBayTemple)]
        [GossipLocationHint("Great Bay Temple", "the ocean temple"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x29F), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), CollectableIndex(0x2485)]
        CollectableGreatBayTempleWaterControlRoomItem4,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Ikana Graveyard Grass 2"), Region(Region.IkanaGraveyard)]
        [GossipLocationHint("unholy grass"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2A0), ItemPool(ItemCategory.GreenRupees, LocationCategory.Grass), CollectableIndex(0x21A5)]
        CollectableIkanaGraveyardIkanaGraveyardLowerGrass2,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Potion Shop Item"), Region(Region.SouthernSwamp)]
        [GossipLocationHint("a shop corner"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2A1), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), CollectableIndex(0x501)]
        CollectableMagicHagsPotionShopItem1,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Pirates' Fortress Cage Room Barrel 2"), Region(Region.PiratesFortressSewer)]
        [GossipLocationHint("the home of pirates"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2A2), ItemPool(ItemCategory.GreenRupees, LocationCategory.Barrels), CollectableIndex(0x1181)]
        CollectablePiratesFortressInteriorCellRoomWithPieceOfHeartItem2,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Pirates' Fortress Cage Room Barrel 3"), Region(Region.PiratesFortressSewer)]
        [GossipLocationHint("the home of pirates"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2A3), ItemPool(ItemCategory.GreenRupees, LocationCategory.Barrels), CollectableIndex(0x1183)]
        CollectablePiratesFortressInteriorCellRoomWithPieceOfHeartItem3,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Pirates' Fortress Cage Room Barrel 4"), Region(Region.PiratesFortressSewer)]
        [GossipLocationHint("the home of pirates"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2A4), ItemPool(ItemCategory.GreenRupees, LocationCategory.Barrels), CollectableIndex(0x1184)]
        CollectablePiratesFortressInteriorCellRoomWithPieceOfHeartItem4,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Pirates' Fortress Cage Room Barrel 5"), Region(Region.PiratesFortressSewer)]
        [GossipLocationHint("the home of pirates"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2A5), ItemPool(ItemCategory.GreenRupees, LocationCategory.Barrels), CollectableIndex(0x1187)]
        CollectablePiratesFortressInteriorCellRoomWithPieceOfHeartItem5,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Secret Shrine Floating Item"), Region(Region.SecretShrine)]
        [GossipLocationHint("a secret place"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2A6), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), CollectableIndex(0x3032)]
        CollectableSecretShrineEntranceRoomItem1,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Secret Shrine Floating Item 2"), Region(Region.SecretShrine)]
        [GossipLocationHint("a secret place"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2A7), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), CollectableIndex(0x3033)]
        CollectableSecretShrineEntranceRoomItem2,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Secret Shrine Floating Item 3"), Region(Region.SecretShrine)]
        [GossipLocationHint("a secret place"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2A8), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), CollectableIndex(0x3034)]
        CollectableSecretShrineEntranceRoomItem3,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Secret Shrine Floating Item 4"), Region(Region.SecretShrine)]
        [GossipLocationHint("a secret place"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2A9), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), CollectableIndex(0x3035)]
        CollectableSecretShrineEntranceRoomItem4,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Secret Shrine Floating Item 5"), Region(Region.SecretShrine)]
        [GossipLocationHint("a secret place"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2AA), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), CollectableIndex(0x3036)]
        CollectableSecretShrineEntranceRoomItem5,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Secret Shrine Floating Item 6"), Region(Region.SecretShrine)]
        [GossipLocationHint("a secret place"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2AB), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), CollectableIndex(0x3037)]
        CollectableSecretShrineEntranceRoomItem6,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Secret Shrine Floating Item 7"), Region(Region.SecretShrine)]
        [GossipLocationHint("a secret place"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2AC), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), CollectableIndex(0x3038)]
        CollectableSecretShrineEntranceRoomItem7,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Secret Shrine Floating Item 8"), Region(Region.SecretShrine)]
        [GossipLocationHint("a secret place"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2AD), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), CollectableIndex(0x3039)]
        CollectableSecretShrineEntranceRoomItem8,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Secret Shrine Floating Item 9"), Region(Region.SecretShrine)]
        [GossipLocationHint("a secret place"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2AE), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), CollectableIndex(0x303A)]
        CollectableSecretShrineEntranceRoomItem9,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Secret Shrine Floating Item 10"), Region(Region.SecretShrine)]
        [GossipLocationHint("a secret place"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2AF), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), CollectableIndex(0x303B)]
        CollectableSecretShrineEntranceRoomItem10,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Secret Shrine Floating Item 11"), Region(Region.SecretShrine)]
        [GossipLocationHint("a secret place"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2B0), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), CollectableIndex(0x303C)]
        CollectableSecretShrineEntranceRoomItem11,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Secret Shrine Floating Item 12"), Region(Region.SecretShrine)]
        [GossipLocationHint("a secret place"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2B1), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), CollectableIndex(0x303D)]
        CollectableSecretShrineEntranceRoomItem12,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Secret Shrine Floating Item 13"), Region(Region.SecretShrine)]
        [GossipLocationHint("a secret place"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2B2), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), CollectableIndex(0x303E)]
        CollectableSecretShrineEntranceRoomItem13,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Secret Shrine Floating Item 14"), Region(Region.SecretShrine)]
        [GossipLocationHint("a secret place"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2B3), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), CollectableIndex(0x303F)]
        CollectableSecretShrineEntranceRoomItem14,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Secret Shrine Floating Item 15"), Region(Region.SecretShrine)]
        [GossipLocationHint("a secret place"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2B4), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), CollectableIndex(0x3040)]
        CollectableSecretShrineEntranceRoomItem15,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Secret Shrine Floating Item 16"), Region(Region.SecretShrine)]
        [GossipLocationHint("a secret place"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2B5), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), CollectableIndex(0x3041)]
        CollectableSecretShrineEntranceRoomItem16,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Secret Shrine Floating Item 17"), Region(Region.SecretShrine)]
        [GossipLocationHint("a secret place"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2B6), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), CollectableIndex(0x3042)]
        CollectableSecretShrineEntranceRoomItem17,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Cleared Swamp Potion Shop Pot 2"), Region(Region.SouthernSwamp)]
        [GossipLocationHint("a swamp jar"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2B7), ItemPool(ItemCategory.GreenRupees, LocationCategory.Jars), CollectableIndex(0x22)]
        CollectableSouthernSwampClearMagicHagsPotionShopExteriorPot2,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Potion Shop Pot 2"), Region(Region.SouthernSwamp)]
        [GossipLocationHint("a swamp jar"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2B8), ItemPool(ItemCategory.GreenRupees, LocationCategory.Jars), CollectableIndex(0x22A7)]
        CollectableSouthernSwampPoisonedMagicHagsPotionShopExteriorPot2,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Stone Tower Temple Lava Room Item 3"), Region(Region.StoneTowerTemple)]
        [GossipLocationHint("Stone Tower Temple", "the cursed temple"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2B9), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), CollectableIndex(0xB02)]
        CollectableStoneTowerTempleLavaRoomItem3,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Stone Tower Temple Lava Room Item 4"), Region(Region.StoneTowerTemple)]
        [GossipLocationHint("Stone Tower Temple", "the cursed temple"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2BA), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), CollectableIndex(0xB03)]
        CollectableStoneTowerTempleLavaRoomItem4,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Stone Tower Temple Lava Room Item 5"), Region(Region.StoneTowerTemple)]
        [GossipLocationHint("Stone Tower Temple", "the cursed temple"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2BB), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), CollectableIndex(0xB04)]
        CollectableStoneTowerTempleLavaRoomItem5,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Inverted Stone Tower Temple Dexihand Item 2"), Region(Region.StoneTowerTemple)]
        [GossipLocationHint("Stone Tower Temple", "the cursed temple"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2BC), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), CollectableIndex(0xC03)]
        CollectableStoneTowerTempleInvertedEyegoreRoomItem2,


        [Visible]
        [Repeatable]
        [ItemName("Large Magic Jar"), LocationName("Clock Tower Rooftop Pot"), Region(Region.ClockTowerRoof)]
        [GossipLocationHint("a rooftop pot"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a large amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2BD), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0xCC0)]
        CollectableClockTowerRooftopPot1,


        [Visible]
        [Repeatable]
        [ItemName("Large Magic Jar"), LocationName("Clock Tower Rooftop Pot 2"), Region(Region.ClockTowerRoof)]
        [GossipLocationHint("a rooftop pot"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a large amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2BE), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0xCC1)]
        CollectableClockTowerRooftopPot2,


        [Visible]
        [Repeatable]
        [ItemName("Large Magic Jar"), LocationName("Clock Tower Rooftop Pot 3"), Region(Region.ClockTowerRoof)]
        [GossipLocationHint("a rooftop pot"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a large amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2BF), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0xCC2)]
        CollectableClockTowerRooftopPot3,


        [Visible]
        [Repeatable]
        [ItemName("Large Magic Jar"), LocationName("Clock Tower Rooftop Pot 4"), Region(Region.ClockTowerRoof)]
        [GossipLocationHint("a rooftop pot"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a large amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2C0), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0xCC3)]
        CollectableClockTowerRooftopPot4,


        [Visible]
        [Repeatable]
        [ItemName("Large Magic Jar"), LocationName("Goron Racetrack Pot"), Region(Region.GoronRaceItems)]
        [GossipLocationHint("a racetrack jar"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a large amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2C1), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0x35A0)]
        CollectableGoronRacetrackPot1,


        [Visible]
        [Repeatable]
        [ItemName("Large Magic Jar"), LocationName("Goron Racetrack Pot 2"), Region(Region.GoronRaceItems)]
        [GossipLocationHint("a racetrack jar"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a large amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2C2), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0x35A1)]
        CollectableGoronRacetrackPot2,


        [Visible]
        [Repeatable]
        [ItemName("Large Magic Jar"), LocationName("Goron Racetrack Pot 3"), Region(Region.GoronRaceItems)]
        [GossipLocationHint("a racetrack jar"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a large amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2C3), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0x35A2)]
        CollectableGoronRacetrackPot3,


        [Visible]
        [Repeatable]
        [ItemName("Large Magic Jar"), LocationName("Goron Racetrack Pot 4"), Region(Region.GoronRaceItems)]
        [GossipLocationHint("a racetrack jar"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a large amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2C4), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0x35A3)]
        CollectableGoronRacetrackPot4,


        [Visible]
        [Repeatable]
        [ItemName("Large Magic Jar"), LocationName("Goron Racetrack Pot 5"), Region(Region.GoronRaceItems)]
        [GossipLocationHint("a racetrack jar"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a large amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2C5), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0x35A4)]
        CollectableGoronRacetrackPot5,


        [Visible]
        [Repeatable]
        [ItemName("Large Magic Jar"), LocationName("Goron Racetrack Pot 6"), Region(Region.GoronRaceItems)]
        [GossipLocationHint("a racetrack jar"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a large amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2C6), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0x35A5)]
        CollectableGoronRacetrackPot6,


        [Visible]
        [Repeatable]
        [ItemName("Large Magic Jar"), LocationName("Goron Racetrack Pot 7"), Region(Region.GoronRaceItems)]
        [GossipLocationHint("a racetrack jar"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a large amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2C7), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0x35A6)]
        CollectableGoronRacetrackPot7,


        [Visible]
        [Repeatable]
        [ItemName("Large Magic Jar"), LocationName("Goron Racetrack Pot 8"), Region(Region.GoronRaceItems)]
        [GossipLocationHint("a racetrack jar"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a large amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2C8), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0x35A7)]
        CollectableGoronRacetrackPot8,


        [Visible]
        [Repeatable]
        [ItemName("Large Magic Jar"), LocationName("Goron Racetrack Pot 9"), Region(Region.GoronRaceItems)]
        [GossipLocationHint("a racetrack jar"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a large amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2C9), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0x35A8)]
        CollectableGoronRacetrackPot9,


        [Visible]
        [Repeatable]
        [ItemName("Large Magic Jar"), LocationName("Goron Racetrack Pot 10"), Region(Region.GoronRaceItems)]
        [GossipLocationHint("a racetrack jar"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a large amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2CA), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0x35A9)]
        CollectableGoronRacetrackPot10,


        [Visible]
        [Repeatable]
        [ItemName("Large Magic Jar"), LocationName("Goron Racetrack Pot 11"), Region(Region.GoronRaceItems)]
        [GossipLocationHint("a racetrack jar"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a large amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2CB), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0x35AA)]
        CollectableGoronRacetrackPot11,


        [Visible]
        [Repeatable]
        [ItemName("Large Magic Jar"), LocationName("Goron Racetrack Pot 12"), Region(Region.GoronRaceItems)]
        [GossipLocationHint("a racetrack jar"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a large amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2CC), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0x35AB)]
        CollectableGoronRacetrackPot12,


        [Visible]
        [Repeatable]
        [ItemName("Large Magic Jar"), LocationName("Goron Racetrack Pot 13"), Region(Region.GoronRaceItems)]
        [GossipLocationHint("a racetrack jar"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a large amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2CD), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0x35AC)]
        CollectableGoronRacetrackPot13,


        [Visible]
        [Repeatable]
        [ItemName("Large Magic Jar"), LocationName("Goron Racetrack Pot 14"), Region(Region.GoronRaceItems)]
        [GossipLocationHint("a racetrack jar"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a large amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2CE), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0x35AD)]
        CollectableGoronRacetrackPot14,


        [Visible]
        [Repeatable]
        [ItemName("Large Magic Jar"), LocationName("Goron Racetrack Pot 15"), Region(Region.GoronRaceItems)]
        [GossipLocationHint("a racetrack jar"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a large amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2CF), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0x35AE)]
        CollectableGoronRacetrackPot15,


        [Visible]
        [Repeatable]
        [ItemName("Large Magic Jar"), LocationName("Goron Racetrack Pot 16"), Region(Region.GoronRaceItems)]
        [GossipLocationHint("a racetrack jar"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a large amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2D0), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0x35AF)]
        CollectableGoronRacetrackPot16,


        [Visible]
        [Repeatable]
        [ItemName("Large Magic Jar"), LocationName("Goron Racetrack Pot 17"), Region(Region.GoronRaceItems)]
        [GossipLocationHint("a racetrack jar"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a large amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2D1), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0x35B0)]
        CollectableGoronRacetrackPot17,


        [Visible]
        [Repeatable]
        [ItemName("Large Magic Jar"), LocationName("Goron Racetrack Pot 18"), Region(Region.GoronRaceItems)]
        [GossipLocationHint("a racetrack jar"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a large amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2D2), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0x35B1)]
        CollectableGoronRacetrackPot18,


        [Visible]
        [Repeatable]
        [ItemName("Large Magic Jar"), LocationName("Goron Racetrack Pot 19"), Region(Region.GoronRaceItems)]
        [GossipLocationHint("a racetrack jar"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a large amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2D3), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0x35B2)]
        CollectableGoronRacetrackPot19,


        [Visible]
        [Repeatable]
        [ItemName("Large Magic Jar"), LocationName("Goron Racetrack Pot 20"), Region(Region.GoronRaceItems)]
        [GossipLocationHint("a racetrack jar"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a large amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2D4), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0x35B3)]
        CollectableGoronRacetrackPot20,


        [Visible]
        [Repeatable]
        [ItemName("Large Magic Jar"), LocationName("Goron Racetrack Pot 21"), Region(Region.GoronRaceItems)]
        [GossipLocationHint("a racetrack jar"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a large amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2D5), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0x35B4)]
        CollectableGoronRacetrackPot21,


        [Visible]
        [Repeatable]
        [ItemName("Large Magic Jar"), LocationName("Goron Racetrack Pot 22"), Region(Region.GoronRaceItems)]
        [GossipLocationHint("a racetrack jar"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a large amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2D6), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0x35B5)]
        CollectableGoronRacetrackPot22,


        [Visible]
        [Repeatable]
        [ItemName("Large Magic Jar"), LocationName("Goron Racetrack Pot 23"), Region(Region.GoronRaceItems)]
        [GossipLocationHint("a racetrack jar"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a large amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2D7), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0x35B6)]
        CollectableGoronRacetrackPot23,


        [Visible]
        [Repeatable]
        [ItemName("Large Magic Jar"), LocationName("Goron Racetrack Pot 24"), Region(Region.GoronRaceItems)]
        [GossipLocationHint("a racetrack jar"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a large amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2D8), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0x35B7)]
        CollectableGoronRacetrackPot24,


        [Visible]
        [Repeatable]
        [ItemName("Large Magic Jar"), LocationName("Goron Racetrack Pot 25"), Region(Region.GoronRaceItems)]
        [GossipLocationHint("a racetrack jar"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a large amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2D9), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0x35B8)]
        CollectableGoronRacetrackPot25,


        [Visible]
        [Repeatable]
        [ItemName("Large Magic Jar"), LocationName("Goron Racetrack Pot 26"), Region(Region.GoronRaceItems)]
        [GossipLocationHint("a racetrack jar"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a large amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2DA), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0x35B9)]
        CollectableGoronRacetrackPot26,


        [Visible]
        [Repeatable]
        [ItemName("Large Magic Jar"), LocationName("Goron Racetrack Pot 27"), Region(Region.GoronRaceItems)]
        [GossipLocationHint("a racetrack jar"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a large amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2DB), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0x35BD)]
        CollectableGoronRacetrackPot27,


        [Visible]
        [Repeatable]
        [ItemName("Large Magic Jar"), LocationName("Goron Shrine Pot 6"), Region(Region.GoronVillage)]
        [GossipLocationHint("a crying child's jar"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a large amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2DC), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0x1962)]
        CollectableGoronShrineGoronKidSRoomPot3,


        [Visible]
        [Repeatable]
        [ItemName("Large Magic Jar"), LocationName("Goron Shrine Pot 7"), Region(Region.GoronVillage)]
        [GossipLocationHint("a crying child's jar"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a large amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2DD), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0x1967)]
        CollectableGoronShrineMainRoomPot4,


        [Visible]
        [Repeatable]
        [ItemName("Large Magic Jar"), LocationName("Goron Shrine Pot 8"), Region(Region.GoronVillage)]
        [GossipLocationHint("a crying child's jar"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a large amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2DE), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0x1968)]
        CollectableGoronShrineMainRoomPot5,


        [Visible]
        [Repeatable]
        [ItemName("Large Magic Jar"), LocationName("Goron Shrine Pot 9"), Region(Region.GoronVillage)]
        [GossipLocationHint("a crying child's jar"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a large amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2DF), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0x1969)]
        CollectableGoronShrineMainRoomPot6,


        [Visible]
        [Repeatable]
        [ItemName("Large Magic Jar"), LocationName("Great Bay Coast Pot 7"), Region(Region.GreatBayCoast)]
        [GossipLocationHint("an ocean jar"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a large amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2E0), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0x1BAC)]
        CollectableGreatBayCoastPot10,


        [Visible]
        [Repeatable]
        [ItemName("Large Magic Jar"), LocationName("Great Bay Temple Red Valve Crate"), Region(Region.GreatBayTemple)]
        [GossipLocationHint("Great Bay Temple", "the ocean temple"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a large amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2E1), ItemPool(ItemCategory.MagicJars, LocationCategory.Crates), CollectableIndex(0x24B2)]
        CollectableGreatBayTempleBlueChuchuValveRoomWoodenCrateLarge1,


        [Visible]
        [Repeatable]
        [ItemName("Large Magic Jar"), LocationName("Ikana King Pot 3"), Region(Region.IkanaCastle)]
        [GossipLocationHint("an ancient jar"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a large amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2E2), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0x2B21)]
        CollectableIgosDuIkanaSLairIgosDuIkanaSRoomPot3,


        [Visible]
        [Repeatable]
        [ItemName("Large Magic Jar"), LocationName("Ikana Canyon Grass 3"), Region(Region.IkanaCanyon)]
        [GossipLocationHint("cursed grass"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a large amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2E3), ItemPool(ItemCategory.MagicJars, LocationCategory.Grass), CollectableIndex(0x9C6)]
        CollectableIkanaCanyonMainAreaGrass3,


        [Visible]
        [Repeatable]
        [ItemName("Large Magic Jar"), LocationName("Milk Road Grass 2"), Region(Region.MilkRoad)]
        [GossipLocationHint("a roadside plant"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a large amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2E4), ItemPool(ItemCategory.MagicJars, LocationCategory.Grass), CollectableIndex(0x1122)]
        CollectableMilkRoadGrass2,


        [Visible]
        [Repeatable]
        [ItemName("Large Magic Jar"), LocationName("Mountain Village Spring Snowball 3"), Region(Region.MountainVillage)]
        [GossipLocationHint("a spring snowball"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a large amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2E5), ItemPool(ItemCategory.MagicJars, LocationCategory.SmallSnowballs), CollectableIndex(0x2D2C)]
        CollectableMountainVillageSpringSmallSnowball3,


        [Visible]
        [Repeatable]
        [ItemName("Large Magic Jar"), LocationName("Goron Graveyard Snowball"), Region(Region.MountainVillage)]
        [GossipLocationHint("a high snowball"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a large amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2E6), ItemPool(ItemCategory.MagicJars, LocationCategory.SmallSnowballs), CollectableIndex(0x282D)]
        CollectableMountainVillageWinterSmallSnowball3,


        [Visible]
        [Repeatable]
        [ItemName("Large Magic Jar"), LocationName("Goron Graveyard Snowball 2"), Region(Region.MountainVillage)]
        [GossipLocationHint("a high snowball"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a large amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2E7), ItemPool(ItemCategory.MagicJars, LocationCategory.SmallSnowballs), CollectableIndex(0x282E)]
        CollectableMountainVillageWinterSmallSnowball4,


        [Visible]
        [Repeatable]
        [ItemName("Large Magic Jar"), LocationName("Mountain Village Winter Small Snowball 3"), Region(Region.MountainVillage)]
        [GossipLocationHint("a village snowball"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a large amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2E8), ItemPool(ItemCategory.MagicJars, LocationCategory.SmallSnowballs), CollectableIndex(0x2837)]
        CollectableMountainVillageWinterSmallSnowball5,


        [Visible]
        [Repeatable]
        [ItemName("Large Magic Jar"), LocationName("Snowhead Small Snowball 3"), Region(Region.Snowhead)]
        [GossipLocationHint("a mountain-top snowball"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a large amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2E9), ItemPool(ItemCategory.MagicJars, LocationCategory.SmallSnowballs), CollectableIndex(0x2E23), CollectableIndex(0x3922)]
        CollectableSnowheadSmallSnowball3,


        [Visible]
        [Repeatable]
        [ItemName("Large Magic Jar"), LocationName("Stone Tower Owl Pot 3"), Region(Region.StoneTower)]
        [GossipLocationHint("a high tower"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a large amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2EA), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0x2C3A)]
        CollectableStoneTowerPot5,


        [Visible]
        [Repeatable]
        [ItemName("Large Magic Jar"), LocationName("Inverted Stone Tower Pot 2"), Region(Region.StoneTower)]
        [GossipLocationHint("a sky below"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a large amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2EB), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0x2CB8)]
        CollectableStoneTowerInvertedStoneTowerFlippedPot2,


        [Visible]
        [Repeatable]
        [ItemName("Large Magic Jar"), LocationName("Link Trial Pot"), Region(Region.TheMoon)]
        [GossipLocationHint("a trial jar"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a large amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2EC), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0x3320)]
        CollectableTheMoonLinkTrialEntrancePot1,


        [Visible]
        [Repeatable]
        [ItemName("Large Magic Jar"), LocationName("Link Trial Pot 2"), Region(Region.TheMoon)]
        [GossipLocationHint("a trial jar"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a large amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2ED), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0x3321)]
        CollectableTheMoonLinkTrialEntrancePot2,


        [Visible]
        [Repeatable]
        [ItemName("Large Magic Jar"), LocationName("Link Trial Pot 3"), Region(Region.TheMoon)]
        [GossipLocationHint("a trial jar"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a large amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2EE), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0x3322)]
        CollectableTheMoonLinkTrialEntrancePot3,


        [Visible]
        [Repeatable]
        [ItemName("Large Magic Jar"), LocationName("Link Trial Pot 4"), Region(Region.TheMoon)]
        [GossipLocationHint("a trial jar"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a large amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2EF), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0x3323)]
        CollectableTheMoonLinkTrialEntrancePot4,


        [Visible]
        [Repeatable]
        [ItemName("Large Magic Jar"), LocationName("Zora Cape Owl Pot 3"), Region(Region.ZoraCape)]
        [GossipLocationHint("a cape jar"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a large amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2F0), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0x1C25)]
        CollectableZoraCapePot3,


        [Visible]
        [Repeatable]
        [ItemName("Purple Rupee"), LocationName("Dampe's Basement Pot 8"), Region(Region.IkanaGraveyard)]
        [GossipLocationHint("an unholy jar"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 50 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2F1), ItemPool(ItemCategory.PurpleRupees, LocationCategory.Jars), CollectableIndex(0x180B)]
        CollectableDampesHouseBasementPot8,


        [Visible]
        [Repeatable]
        [ItemName("Recovery Heart"), LocationName("Pirates' Fortress Item"), Region(Region.PiratesFortressInterior)]
        [GossipLocationHint("the home of pirates"), GossipItemHint("health")]
        [ShopText("Replenishes a small amount of your life energy.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2F2), ItemPool(ItemCategory.RecoveryHearts, LocationCategory.Freestanding), CollectableIndex(0xA20)]
        CollectablePiratesFortressItem1,


        [Visible]
        [Repeatable]
        [ItemName("Recovery Heart"), LocationName("Pirates' Fortress Item 2"), Region(Region.PiratesFortressInterior)]
        [GossipLocationHint("the home of pirates"), GossipItemHint("health")]
        [ShopText("Replenishes a small amount of your life energy.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2F3), ItemPool(ItemCategory.RecoveryHearts, LocationCategory.Freestanding), CollectableIndex(0xA21)]
        CollectablePiratesFortressItem2,


        [Visible]
        [Repeatable]
        [ItemName("Recovery Heart"), LocationName("Pirates' Fortress Item 3"), Region(Region.PiratesFortressInterior)]
        [GossipLocationHint("the home of pirates"), GossipItemHint("health")]
        [ShopText("Replenishes a small amount of your life energy.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2F4), ItemPool(ItemCategory.RecoveryHearts, LocationCategory.Freestanding), CollectableIndex(0xA22)]
        CollectablePiratesFortressItem3,


        [Visible]
        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Butler Race Pillar Item 7"), Region(Region.ButlerRaceItems)]
        [GossipLocationHint("a royal race"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2F5), ItemPool(ItemCategory.RedRupees, LocationCategory.Freestanding), CollectableIndex(0x293C)]
        CollectableDekuShrineGiantRoomFloor1Item7,


        [Visible]
        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Butler Race Pillar Item 8"), Region(Region.ButlerRaceItems)]
        [GossipLocationHint("a royal race"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2F6), ItemPool(ItemCategory.RedRupees, LocationCategory.Freestanding), CollectableIndex(0x293D)]
        CollectableDekuShrineGiantRoomFloor1Item8,


        [Visible]
        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Great Bay Temple Water Control Room Item 5"), Region(Region.GreatBayTemple)]
        [GossipLocationHint("Great Bay Temple", "the ocean temple"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2F7), ItemPool(ItemCategory.RedRupees, LocationCategory.Freestanding), CollectableIndex(0x2481)]
        CollectableGreatBayTempleWaterControlRoomItem5,


        [Visible]
        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Great Bay Temple Dexihand Item"), Region(Region.GreatBayTemple)]
        [GossipLocationHint("Great Bay Temple", "the ocean temple"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2F8), ItemPool(ItemCategory.RedRupees, LocationCategory.Freestanding), CollectableIndex(0x2486)]
        CollectableGreatBayTempleCompassBossKeyRoomItem1,


        [Visible]
        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Great Bay Temple Dexihand Item 2"), Region(Region.GreatBayTemple)]
        [GossipLocationHint("Great Bay Temple", "the ocean temple"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2F9), ItemPool(ItemCategory.RedRupees, LocationCategory.Freestanding), CollectableIndex(0x2487)]
        CollectableGreatBayTempleCompassBossKeyRoomItem2,


        [Visible]
        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Great Bay Temple Green Pump Item"), Region(Region.GreatBayTemple)]
        [GossipLocationHint("Great Bay Temple", "the ocean temple"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2FA), ItemPool(ItemCategory.RedRupees, LocationCategory.Freestanding), CollectableIndex(0x2488)]
        CollectableGreatBayTempleTopmostRoomWithGreenValveItem1,


        [Visible]
        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Great Bay Temple Green Pump Item 2"), Region(Region.GreatBayTemple)]
        [GossipLocationHint("Great Bay Temple", "the ocean temple"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2FB), ItemPool(ItemCategory.RedRupees, LocationCategory.Freestanding), CollectableIndex(0x2489)]
        CollectableGreatBayTempleTopmostRoomWithGreenValveItem2,


        [Visible]
        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Laundry Pool Item"), Region(Region.LaundryPool)]
        [GossipLocationHint("a floating item"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2FC), ItemPool(ItemCategory.RedRupees, LocationCategory.Freestanding), CollectableIndex(0x3803)]
        CollectableLaundryPoolItem1,


        [Visible]
        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Laundry Pool Item 2"), Region(Region.LaundryPool)]
        [GossipLocationHint("a floating item"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2FD), ItemPool(ItemCategory.RedRupees, LocationCategory.Freestanding), CollectableIndex(0x3804)]
        CollectableLaundryPoolItem2,


        [Visible]
        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Laundry Pool Item 3"), Region(Region.LaundryPool)]
        [GossipLocationHint("a floating item"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2FE), ItemPool(ItemCategory.RedRupees, LocationCategory.Freestanding), CollectableIndex(0x3805)]
        CollectableLaundryPoolItem3,


        [Visible]
        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Mountain Village Spring Stair Item"), Region(Region.MountainVillage)]
        [GossipLocationHint("an item under the stairs"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x2FF), ItemPool(ItemCategory.RedRupees, LocationCategory.Rocks), CollectableIndex(0x2809), CollectableIndex(0x2D09)]
        CollectableMountainVillageWinterMountainVillageSpringItem1,


        [Visible]
        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Snowhead Temple Icicle Room Frozen Item"), Region(Region.SnowheadTemple)]
        [GossipLocationHint("Snowhead Temple", "an icy gale"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x300), ItemPool(ItemCategory.RedRupees, LocationCategory.Freestanding), CollectableIndex(0x1081)]
        CollectableSnowheadTempleIceBlockRoomItem1,


        [Visible]
        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Snowhead Temple Icicle Room Frozen Item 2"), Region(Region.SnowheadTemple)]
        [GossipLocationHint("Snowhead Temple", "an icy gale"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x301), ItemPool(ItemCategory.RedRupees, LocationCategory.Freestanding), CollectableIndex(0x1082)]
        CollectableSnowheadTempleIceBlockRoomItem2,


        [Visible]
        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Snowhead Temple Icicle Room Frozen Item 3"), Region(Region.SnowheadTemple)]
        [GossipLocationHint("Snowhead Temple", "an icy gale"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x302), ItemPool(ItemCategory.RedRupees, LocationCategory.Freestanding), CollectableIndex(0x1083)]
        CollectableSnowheadTempleIceBlockRoomItem3,


        [Visible]
        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Swamp Near Frog Hive"), Region(Region.SouthernSwamp)]
        [GossipLocationHint("a swamp hive"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x303), ItemPool(ItemCategory.RedRupees, LocationCategory.Beehives), CollectableIndex(0x22A8)]
        CollectableSouthernSwampPoisonedCentralSwampBeehive1,


        [Visible]
        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Stone Tower Temple Lava Room Item 6"), Region(Region.StoneTowerTemple)]
        [GossipLocationHint("Stone Tower Temple", "the cursed temple"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x304), ItemPool(ItemCategory.RedRupees, LocationCategory.Freestanding), CollectableIndex(0xB01)]
        CollectableStoneTowerTempleLavaRoomItem6,


        [Visible]
        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Stone Tower Temple Eyegore Room Item"), Region(Region.StoneTowerTemple)]
        [GossipLocationHint("Stone Tower Temple", "the cursed temple"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x305), ItemPool(ItemCategory.RedRupees, LocationCategory.Freestanding), CollectableIndex(0xB10)]
        CollectableStoneTowerTempleEyegoreRoomItem1,


        [Visible]
        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Stone Tower Temple Mirror Room Crate"), Region(Region.StoneTowerTemple)]
        [GossipLocationHint("Stone Tower Temple", "the cursed temple"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x306), ItemPool(ItemCategory.RedRupees, LocationCategory.Crates), CollectableIndex(0xB25)]
        CollectableStoneTowerTempleMirrorRoomWoodenCrateLarge1,


        [Visible]
        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Stone Tower Temple Mirror Room Crate 2"), Region(Region.StoneTowerTemple)]
        [GossipLocationHint("Stone Tower Temple", "the cursed temple"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x307), ItemPool(ItemCategory.RedRupees, LocationCategory.Crates), CollectableIndex(0xB26)]
        CollectableStoneTowerTempleMirrorRoomWoodenCrateLarge2,


        [Visible]
        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Stone Tower Temple Eyegore Room Item 2"), Region(Region.StoneTowerTemple)]
        [GossipLocationHint("Stone Tower Temple", "the cursed temple"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x308), ItemPool(ItemCategory.RedRupees, LocationCategory.Freestanding), CollectableIndex(0xB0F)]
        CollectableStoneTowerTempleEyegoreRoomItem2,


        [Visible]
        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Inverted Stone Tower Temple Dexihand Item 3"), Region(Region.StoneTowerTemple)]
        [GossipLocationHint("Stone Tower Temple", "the cursed temple"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x309), ItemPool(ItemCategory.RedRupees, LocationCategory.Freestanding), CollectableIndex(0xC02)]
        CollectableStoneTowerTempleInvertedEyegoreRoomItem3,


        [Visible]
        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Inverted Stone Tower Temple Updraft Room Item"), Region(Region.StoneTowerTemple)]
        [GossipLocationHint("Stone Tower Temple", "the cursed temple"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x30A), ItemPool(ItemCategory.RedRupees, LocationCategory.Freestanding), CollectableIndex(0xC04)]
        CollectableStoneTowerTempleInvertedAirRoomItem1,


        [Visible]
        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Inverted Stone Tower Temple Updraft Room Item 2"), Region(Region.StoneTowerTemple)]
        [GossipLocationHint("Stone Tower Temple", "the cursed temple"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x30B), ItemPool(ItemCategory.RedRupees, LocationCategory.Freestanding), CollectableIndex(0xC05)]
        CollectableStoneTowerTempleInvertedAirRoomItem2,


        [Visible]
        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Termina Field Pillar Item"), Region(Region.TerminaField)]
        [GossipLocationHint("a pillar"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x30C), ItemPool(ItemCategory.RedRupees, LocationCategory.Freestanding), CollectableIndex(0x1682)]
        CollectableTerminaFieldItem1,


        [Visible]
        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Woodfall Temple Pre-Boss Left Pillar Item"), Region(Region.WoodfallTemple)]
        [GossipLocationHint("Woodfall Temple", "the sleeping temple"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x30D), ItemPool(ItemCategory.RedRupees, LocationCategory.Freestanding), CollectableIndex(0xD81)]
        CollectableWoodfallTemplePreBossRoomItem5,


        [Visible]
        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Woodfall Temple Pre-Boss Right Pillar Item"), Region(Region.WoodfallTemple)]
        [GossipLocationHint("Woodfall Temple", "the sleeping temple"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x30E), ItemPool(ItemCategory.RedRupees, LocationCategory.Freestanding), CollectableIndex(0xD82)]
        CollectableWoodfallTemplePreBossRoomItem6,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Ikana Castle Courtyard Grass 3"), Region(Region.IkanaCastle)]
        [GossipLocationHint("an ancient plant"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x30F), ItemPool(ItemCategory.MagicJars, LocationCategory.Grass), CollectableIndex(0xEB4)]
        CollectableAncientCastleOfIkanaCastleExteriorGrass3,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Ikana Castle Courtyard Grass 4"), Region(Region.IkanaCastle)]
        [GossipLocationHint("an ancient plant"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x310), ItemPool(ItemCategory.MagicJars, LocationCategory.Grass), CollectableIndex(0xEB6)]
        CollectableAncientCastleOfIkanaCastleExteriorGrass4,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Ikana Castle Fire Ceiling Room Pot"), Region(Region.IkanaCastle)]
        [GossipLocationHint("an ancient jar"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x311), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0xEC1)]
        CollectableAncientCastleOfIkanaFireCeilingRoomPot1,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Ikana Castle Hole Room Pot"), Region(Region.IkanaCastle)]
        [GossipLocationHint("an ancient jar"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x312), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0xECF)]
        CollectableAncientCastleOfIkanaHoleRoomPot1,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Ikana Castle Hole Room Pot 2"), Region(Region.IkanaCastle)]
        [GossipLocationHint("an ancient jar"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x313), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0xEDA)]
        CollectableAncientCastleOfIkanaHoleRoomPot2,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Observatory Balloon Pot"), Region(Region.EastClockTown)]
        [GossipLocationHint("an underground jar"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x314), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0x14A1)]
        CollectableAstralObservatorySewerPot1,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Observatory Balloon Pot 2"), Region(Region.EastClockTown)]
        [GossipLocationHint("an underground jar"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x315), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0x14A2)]
        CollectableAstralObservatorySewerPot2,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Observatory Scarecrow Pot 3"), Region(Region.EastClockTown)]
        [GossipLocationHint("an underground jar"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x316), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0x14A6)]
        CollectableAstralObservatoryObservatoryBombersHideoutPot3,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Night 2 Grave Pot 2"), Region(Region.IkanaGraveyard)]
        [GossipLocationHint("an unholy jar"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x317), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0x62C)]
        CollectableBeneathTheGraveyardMainAreaPot2,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Deku Palace Pot"), Region(Region.DekuPalace)]
        [GossipLocationHint("a royal pathway", "the home of scrubs"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x318), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0x15B2)]
        CollectableDekuPalaceEastInnerGardenPot1,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Deku Palace Pot 2"), Region(Region.DekuPalace)]
        [GossipLocationHint("a royal pathway", "the home of scrubs"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x319), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0x15B3)]
        CollectableDekuPalaceEastInnerGardenPot2,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Goron Racetrack Pot 28"), Region(Region.GoronRaceItems)]
        [GossipLocationHint("a racetrack jar"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x31A), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0x35BA)]
        CollectableGoronRacetrackPot28,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Goron Racetrack Pot 29"), Region(Region.GoronRaceItems)]
        [GossipLocationHint("a racetrack jar"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x31B), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0x35BB)]
        CollectableGoronRacetrackPot29,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Goron Racetrack Pot 30"), Region(Region.GoronRaceItems)]
        [GossipLocationHint("a racetrack jar"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x31C), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0x35BC)]
        CollectableGoronRacetrackPot30,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Goron Shrine Pot 10"), Region(Region.GoronVillage)]
        [GossipLocationHint("a crying child's jar"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x31D), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0x1964)]
        CollectableGoronShrineMainRoomPot7,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Goron Shrine Pot 11"), Region(Region.GoronVillage)]
        [GossipLocationHint("a crying child's jar"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x31E), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0x1965)]
        CollectableGoronShrineMainRoomPot8,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Goron Village Large Snowball 4"), Region(Region.GoronVillage)]
        [GossipLocationHint("a village snowball"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x31F), ItemPool(ItemCategory.MagicJars, LocationCategory.LargeSnowballs), CollectableIndex(0x26A1)]
        CollectableGoronVillageWinterLargeSnowball4,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Goron Village Large Snowball 5"), Region(Region.GoronVillage)]
        [GossipLocationHint("a village snowball"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x320), ItemPool(ItemCategory.MagicJars, LocationCategory.LargeSnowballs), CollectableIndex(0x26A3)]
        CollectableGoronVillageWinterLargeSnowball5,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Goron Village Large Snowball 6"), Region(Region.GoronVillage)]
        [GossipLocationHint("a village snowball"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x321), ItemPool(ItemCategory.MagicJars, LocationCategory.LargeSnowballs), CollectableIndex(0x26A5)]
        CollectableGoronVillageWinterLargeSnowball6,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Goron Village Small Snowball 9"), Region(Region.GoronVillage)]
        [GossipLocationHint("a village snowball"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x322), ItemPool(ItemCategory.MagicJars, LocationCategory.SmallSnowballs), CollectableIndex(0x26AE)]
        CollectableGoronVillageWinterSmallSnowball9,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Goron Village Small Snowball 10"), Region(Region.GoronVillage)]
        [GossipLocationHint("a village snowball"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x323), ItemPool(ItemCategory.MagicJars, LocationCategory.SmallSnowballs), CollectableIndex(0x26B2)]
        CollectableGoronVillageWinterSmallSnowball10,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Ikana King Entry Pot 3"), Region(Region.IkanaCastle)]
        [GossipLocationHint("an ancient jar"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x324), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0x2B30)]
        CollectableIgosDuIkanaSLairPreBossRoomPot3,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Ikana Graveyard Grass 3"), Region(Region.IkanaGraveyard)]
        [GossipLocationHint("unholy grass"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x325), ItemPool(ItemCategory.MagicJars, LocationCategory.Grass), CollectableIndex(0x21A6)]
        CollectableIkanaGraveyardIkanaGraveyardLowerGrass3,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Mountain Village Winter Small Snowball 4"), Region(Region.MountainVillage)]
        [GossipLocationHint("a village snowball"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x326), ItemPool(ItemCategory.MagicJars, LocationCategory.SmallSnowballs), CollectableIndex(0x2821)]
        CollectableMountainVillageWinterSmallSnowball6,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Mountain Village Winter Small Snowball 5"), Region(Region.MountainVillage)]
        [GossipLocationHint("a village snowball"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x327), ItemPool(ItemCategory.MagicJars, LocationCategory.SmallSnowballs), CollectableIndex(0x2827)]
        CollectableMountainVillageWinterSmallSnowball7,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Mountain Village Day 1 Snowball"), Region(Region.MountainVillage)]
        [GossipLocationHint("a village snowball"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x328), ItemPool(ItemCategory.MagicJars, LocationCategory.LargeSnowballs), CollectableIndex(0x282A)]
        CollectableMountainVillageWinterLargeSnowball3,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Mountain Village Day 2 Snowball 2"), Region(Region.MountainVillage)]
        [GossipLocationHint("a village snowball"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x329), ItemPool(ItemCategory.MagicJars, LocationCategory.LargeSnowballs), CollectableIndex(0x282C)]
        CollectableMountainVillageWinterLargeSnowball4,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Oceanside Spider House Main Room Pot"), Region(Region.OceanSpiderHouseItems)]
        [GossipLocationHint("a creepy basement pot"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x32A), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0x1426)]
        CollectableOceansideSpiderHouseMainRoomPot1,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Oceanside Spider House Entrance Pot 3"), Region(Region.OceanSpiderHouseItems)]
        [GossipLocationHint("a creepy basement pot"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x32B), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0x142E)]
        CollectableOceansideSpiderHouseEntrancePot3,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Oceanside Spider House Main Room Pot 2"), Region(Region.OceanSpiderHouseItems)]
        [GossipLocationHint("a creepy basement pot"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x32C), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0x1433)]
        CollectableOceansideSpiderHouseMainRoomPot2,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Oceanside Spider House Storage Room Pot"), Region(Region.OceanSpiderHouseItems)]
        [GossipLocationHint("a creepy basement pot"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x32D), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0x1439)]
        CollectableOceansideSpiderHouseStorageRoomPot1,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Twin Islands Day 3 Snowball"), Region(Region.TwinIslands)]
        [GossipLocationHint("a travelling snowball"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x32E), ItemPool(ItemCategory.MagicJars, LocationCategory.LargeSnowballs), CollectableIndex(0x2EA0)]
        CollectablePathToGoronVillageWinterLargeSnowball1,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Twin Islands Day 3 Snowball 2"), Region(Region.TwinIslands)]
        [GossipLocationHint("a travelling snowball"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x32F), ItemPool(ItemCategory.MagicJars, LocationCategory.LargeSnowballs), CollectableIndex(0x2EA2)]
        CollectablePathToGoronVillageWinterLargeSnowball2,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Twin Islands Day 3 Snowball 3"), Region(Region.TwinIslands)]
        [GossipLocationHint("a travelling snowball"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x330), ItemPool(ItemCategory.MagicJars, LocationCategory.LargeSnowballs), CollectableIndex(0x2EA3)]
        CollectablePathToGoronVillageWinterLargeSnowball3,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Twin Islands Day 3 Snowball 4"), Region(Region.TwinIslands)]
        [GossipLocationHint("a travelling snowball"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x331), ItemPool(ItemCategory.MagicJars, LocationCategory.LargeSnowballs), CollectableIndex(0x2EA4)]
        CollectablePathToGoronVillageWinterLargeSnowball4,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Twin Islands Day 3 Snowball 5"), Region(Region.TwinIslands)]
        [GossipLocationHint("a travelling snowball"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x332), ItemPool(ItemCategory.MagicJars, LocationCategory.LargeSnowballs), CollectableIndex(0x2EA5)]
        CollectablePathToGoronVillageWinterLargeSnowball5,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Twin Islands Day 2 Snowball"), Region(Region.TwinIslands)]
        [GossipLocationHint("a travelling snowball"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x333), ItemPool(ItemCategory.MagicJars, LocationCategory.LargeSnowballs), CollectableIndex(0x2EA8)]
        CollectablePathToGoronVillageWinterLargeSnowball6,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Twin Islands Day 2 Snowball 2"), Region(Region.TwinIslands)]
        [GossipLocationHint("a travelling snowball"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x334), ItemPool(ItemCategory.MagicJars, LocationCategory.LargeSnowballs), CollectableIndex(0x2EAA)]
        CollectablePathToGoronVillageWinterLargeSnowball7,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Twin Islands Day 2 Snowball 3"), Region(Region.TwinIslands)]
        [GossipLocationHint("a travelling snowball"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x335), ItemPool(ItemCategory.MagicJars, LocationCategory.LargeSnowballs), CollectableIndex(0x2EAB)]
        CollectablePathToGoronVillageWinterLargeSnowball8,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Twin Islands Day 2 Snowball 4"), Region(Region.TwinIslands)]
        [GossipLocationHint("a travelling snowball"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x336), ItemPool(ItemCategory.MagicJars, LocationCategory.LargeSnowballs), CollectableIndex(0x2EAC)]
        CollectablePathToGoronVillageWinterLargeSnowball9,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Twin Islands Day 1 Snowball"), Region(Region.TwinIslands)]
        [GossipLocationHint("a travelling snowball"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x337), ItemPool(ItemCategory.MagicJars, LocationCategory.LargeSnowballs), CollectableIndex(0x2EAE)]
        CollectablePathToGoronVillageWinterLargeSnowball10,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Twin Islands Day 1 Snowball 2"), Region(Region.TwinIslands)]
        [GossipLocationHint("a travelling snowball"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x338), ItemPool(ItemCategory.MagicJars, LocationCategory.LargeSnowballs), CollectableIndex(0x2EB0)]
        CollectablePathToGoronVillageWinterLargeSnowball11,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Twin Islands Day 1 Snowball 3"), Region(Region.TwinIslands)]
        [GossipLocationHint("a travelling snowball"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x339), ItemPool(ItemCategory.MagicJars, LocationCategory.LargeSnowballs), CollectableIndex(0x2EB1)]
        CollectablePathToGoronVillageWinterLargeSnowball12,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Twin Islands Day 1 Snowball 4"), Region(Region.TwinIslands)]
        [GossipLocationHint("a travelling snowball"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x33A), ItemPool(ItemCategory.MagicJars, LocationCategory.LargeSnowballs), CollectableIndex(0x2EB3)]
        CollectablePathToGoronVillageWinterLargeSnowball13,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Twin Islands Day 1 Snowball 5"), Region(Region.TwinIslands)]
        [GossipLocationHint("a travelling snowball"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x33B), ItemPool(ItemCategory.MagicJars, LocationCategory.LargeSnowballs), CollectableIndex(0x2EB5)]
        CollectablePathToGoronVillageWinterLargeSnowball14,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Twin Islands Small Snowball"), Region(Region.TwinIslands)]
        [GossipLocationHint("a lake snowball"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x33C), ItemPool(ItemCategory.MagicJars, LocationCategory.SmallSnowballs), CollectableIndex(0x2EB6)]
        CollectablePathToGoronVillageWinterSmallSnowball1,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Twin Islands Small Snowball 2"), Region(Region.TwinIslands)]
        [GossipLocationHint("a lake snowball"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x33D), ItemPool(ItemCategory.MagicJars, LocationCategory.SmallSnowballs), CollectableIndex(0x2EB8)]
        CollectablePathToGoronVillageWinterSmallSnowball2,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Twin Islands Ramp Snowball"), Region(Region.TwinIslands)]
        [GossipLocationHint("a lake snowball"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x33E), ItemPool(ItemCategory.MagicJars, LocationCategory.SmallSnowballs), CollectableIndex(0x2EB9)]
        CollectablePathToGoronVillageWinterSmallSnowball3,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Path to Mountain Village Small Snowball"), Region(Region.PathToMountainVillage)]
        [GossipLocationHint("a foothill snowball"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x33F), ItemPool(ItemCategory.MagicJars, LocationCategory.SmallSnowballs), CollectableIndex(0xE23)]
        CollectablePathToMountainVillageSmallSnowball1,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Path to Snowhead Large Snowball"), Region(Region.PathToSnowhead)]
        [GossipLocationHint("a treacherous snowball"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x340), ItemPool(ItemCategory.MagicJars, LocationCategory.LargeSnowballs), CollectableIndex(0x2DE0)]
        CollectablePathToSnowheadLargeSnowball1,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Path to Snowhead Large Snowball 2"), Region(Region.PathToSnowhead)]
        [GossipLocationHint("a treacherous snowball"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x341), ItemPool(ItemCategory.MagicJars, LocationCategory.LargeSnowballs), CollectableIndex(0x2DE1)]
        CollectablePathToSnowheadLargeSnowball2,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Path to Snowhead Large Snowball 3"), Region(Region.PathToSnowhead)]
        [GossipLocationHint("a treacherous snowball"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x342), ItemPool(ItemCategory.MagicJars, LocationCategory.LargeSnowballs), CollectableIndex(0x2DE2)]
        CollectablePathToSnowheadLargeSnowball3,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Path to Snowhead Large Snowball 4"), Region(Region.PathToSnowhead)]
        [GossipLocationHint("a treacherous snowball"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x343), ItemPool(ItemCategory.MagicJars, LocationCategory.LargeSnowballs), CollectableIndex(0x2DE3)]
        CollectablePathToSnowheadLargeSnowball4,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Pinnacle Rock Pot"), Region(Region.PinnacleRock)]
        [GossipLocationHint("a deep jar"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x344), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0x12A0)]
        CollectablePinnacleRockPot1,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Pinnacle Rock Pot 2"), Region(Region.PinnacleRock)]
        [GossipLocationHint("a deep jar"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x345), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0x12A6)]
        CollectablePinnacleRockPot2,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Pinnacle Rock Pot 3"), Region(Region.PinnacleRock)]
        [GossipLocationHint("a deep jar"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x346), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0x12AB)]
        CollectablePinnacleRockPot3,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Pinnacle Rock Pot 4"), Region(Region.PinnacleRock)]
        [GossipLocationHint("a deep jar"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x347), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0x12AD)]
        CollectablePinnacleRockPot4,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Secret Shrine Underwater Pot 3"), Region(Region.SecretShrine)]
        [GossipLocationHint("a secret place"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x348), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0x307B)]
        CollectableSecretShrineMainRoomPot3,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Secret Shrine Underwater Pot 4"), Region(Region.SecretShrine)]
        [GossipLocationHint("a secret place"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x349), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0x307F)]
        CollectableSecretShrineMainRoomPot4,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Snowhead Large Snowball"), Region(Region.Snowhead)]
        [GossipLocationHint("a mountain-top snowball"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x34A), ItemPool(ItemCategory.MagicJars, LocationCategory.LargeSnowballs), CollectableIndex(0x2E24)]
        CollectableSnowheadLargeSnowball1,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Snowhead Large Snowball 2"), Region(Region.Snowhead)]
        [GossipLocationHint("a mountain-top snowball"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x34B), ItemPool(ItemCategory.MagicJars, LocationCategory.LargeSnowballs), CollectableIndex(0x2E25)]
        CollectableSnowheadLargeSnowball2,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Snowhead Large Snowball 3"), Region(Region.Snowhead)]
        [GossipLocationHint("a mountain-top snowball"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x34C), ItemPool(ItemCategory.MagicJars, LocationCategory.LargeSnowballs), CollectableIndex(0x2E26), CollectableIndex(0x3925)]
        CollectableSnowheadLargeSnowball3,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Snowhead Large Snowball 4"), Region(Region.Snowhead)]
        [GossipLocationHint("a mountain-top snowball"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x34D), ItemPool(ItemCategory.MagicJars, LocationCategory.LargeSnowballs), CollectableIndex(0x2E27)]
        CollectableSnowheadLargeSnowball4,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Snowhead Large Snowball 5"), Region(Region.Snowhead)]
        [GossipLocationHint("a mountain-top snowball"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x34E), ItemPool(ItemCategory.MagicJars, LocationCategory.LargeSnowballs), CollectableIndex(0x2E28), CollectableIndex(0x3927)]
        CollectableSnowheadLargeSnowball5,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Snowhead Large Snowball 6"), Region(Region.Snowhead)]
        [GossipLocationHint("a mountain-top snowball"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x34F), ItemPool(ItemCategory.MagicJars, LocationCategory.LargeSnowballs), CollectableIndex(0x2E29), CollectableIndex(0x3928)]
        CollectableSnowheadLargeSnowball6,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Stone Tower Lower Scarecrow Pot"), Region(Region.StoneTower)]
        [GossipLocationHint("a high tower"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x350), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0x2C20)]
        CollectableStoneTowerPot6,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Stone Tower Lower Scarecrow Pot 2"), Region(Region.StoneTower)]
        [GossipLocationHint("a high tower"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x351), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0x2C22)]
        CollectableStoneTowerPot7,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Stone Tower Upper Scarecrow Pot 3"), Region(Region.StoneTower)]
        [GossipLocationHint("a high tower"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x352), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0x2C2E)]
        CollectableStoneTowerPot8,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Stone Tower Upper Scarecrow Pot 4"), Region(Region.StoneTower)]
        [GossipLocationHint("a high tower"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x353), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0x2C31)]
        CollectableStoneTowerPot9,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Stone Tower Lower Scarecrow Pot 3"), Region(Region.StoneTower)]
        [GossipLocationHint("a high tower"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x354), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0x2C34)]
        CollectableStoneTowerPot10,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Zora Cape Waterfall Pot"), Region(Region.ZoraCape)]
        [GossipLocationHint("a cape jar"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x355), ItemPool(ItemCategory.MagicJars, LocationCategory.Jars), CollectableIndex(0x1C21)]
        CollectableZoraCapePot4,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Ranch Fence Item"), Region(Region.RomaniRanch)]
        [GossipLocationHint("a fence"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x356), ItemPool(ItemCategory.BlueRupees, LocationCategory.InvisibleItems), NullableItem]
        CollectableRomaniRanchInvisibleItem1,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Ranch Fence Item 2"), Region(Region.RomaniRanch)]
        [GossipLocationHint("a fence"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x357), ItemPool(ItemCategory.BlueRupees, LocationCategory.InvisibleItems), NullableItem]
        CollectableRomaniRanchInvisibleItem2,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Ranch Fence Item 3"), Region(Region.RomaniRanch)]
        [GossipLocationHint("a fence"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x358), ItemPool(ItemCategory.GreenRupees, LocationCategory.InvisibleItems), NullableItem]
        CollectableRomaniRanchInvisibleItem3,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Ranch Fence Item 4"), Region(Region.RomaniRanch)]
        [GossipLocationHint("a fence"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x359), ItemPool(ItemCategory.GreenRupees, LocationCategory.InvisibleItems), NullableItem]
        CollectableRomaniRanchInvisibleItem4,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Ranch Fence Item 5"), Region(Region.RomaniRanch)]
        [GossipLocationHint("a fence"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x35A), ItemPool(ItemCategory.GreenRupees, LocationCategory.InvisibleItems), NullableItem]
        CollectableRomaniRanchInvisibleItem5,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Ranch Fence Item 6"), Region(Region.RomaniRanch)]
        [GossipLocationHint("a fence"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x35B), ItemPool(ItemCategory.GreenRupees, LocationCategory.InvisibleItems), NullableItem]
        CollectableRomaniRanchInvisibleItem6,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Termina Field Above Cow Grotto Invisible Item"), Region(Region.TerminaField)]
        [GossipLocationHint("a hidden item"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x35D), ItemPool(ItemCategory.BlueRupees, LocationCategory.InvisibleItems), NullableItem]
        CollectableTerminaFieldInvisibleItem1,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Termina Field Invisible Item 2"), Region(Region.TerminaField)]
        [GossipLocationHint("a hidden item"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x35F), ItemPool(ItemCategory.GreenRupees, LocationCategory.InvisibleItems), NullableItem]
        CollectableTerminaFieldInvisibleItem2,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Termina Field Invisible Item 3"), Region(Region.TerminaField)]
        [GossipLocationHint("a hidden item"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x363), ItemPool(ItemCategory.GreenRupees, LocationCategory.InvisibleItems), NullableItem]
        CollectableTerminaFieldInvisibleItem3,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Termina Field Invisible Item 4"), Region(Region.TerminaField)]
        [GossipLocationHint("a hidden item"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x364), ItemPool(ItemCategory.GreenRupees, LocationCategory.InvisibleItems), NullableItem]
        CollectableTerminaFieldInvisibleItem4,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Termina Field Invisible Item 5"), Region(Region.TerminaField)]
        [GossipLocationHint("a hidden item"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x365), ItemPool(ItemCategory.GreenRupees, LocationCategory.InvisibleItems), NullableItem]
        CollectableTerminaFieldInvisibleItem5,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Termina Field Invisible Item 6"), Region(Region.TerminaField)]
        [GossipLocationHint("a hidden item"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x366), ItemPool(ItemCategory.GreenRupees, LocationCategory.InvisibleItems), NullableItem]
        CollectableTerminaFieldInvisibleItem6,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Termina Field Invisible Item 7"), Region(Region.TerminaField)]
        [GossipLocationHint("a hidden item"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x367), ItemPool(ItemCategory.BlueRupees, LocationCategory.InvisibleItems), NullableItem]
        CollectableTerminaFieldInvisibleItem7,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Termina Field Invisible Item 8"), Region(Region.TerminaField)]
        [GossipLocationHint("a hidden item"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x368), ItemPool(ItemCategory.BlueRupees, LocationCategory.InvisibleItems), NullableItem]
        CollectableTerminaFieldInvisibleItem8,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Termina Field Northern Ramp Invisible Item"), Region(Region.TerminaField)]
        [GossipLocationHint("a hidden item"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x369), ItemPool(ItemCategory.BlueRupees, LocationCategory.InvisibleItems), NullableItem]
        CollectableTerminaFieldInvisibleItem9,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Termina Field Invisible Item 10"), Region(Region.TerminaField)]
        [GossipLocationHint("a hidden item"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x36A), ItemPool(ItemCategory.BlueRupees, LocationCategory.InvisibleItems), NullableItem]
        CollectableTerminaFieldInvisibleItem10,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Termina Field Invisible Item 11"), Region(Region.TerminaField)]
        [GossipLocationHint("a hidden item"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x36B), ItemPool(ItemCategory.BlueRupees, LocationCategory.InvisibleItems), NullableItem]
        CollectableTerminaFieldInvisibleItem11,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Swamp Spider House Invisible Item"), Region(Region.SwampSpiderHouseItems)]
        [GossipLocationHint("a large jar"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x371), ItemPool(ItemCategory.GreenRupees, LocationCategory.InvisibleItems), NullableItem]
        CollectableSwampSpiderHouseInvisibleItem1,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Swamp Spider House Invisible Item 2"), Region(Region.SwampSpiderHouseItems)]
        [GossipLocationHint("a large jar"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x372), ItemPool(ItemCategory.GreenRupees, LocationCategory.InvisibleItems), NullableItem]
        CollectableSwampSpiderHouseInvisibleItem2,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Swamp Spider House Invisible Item 3"), Region(Region.SwampSpiderHouseItems)]
        [GossipLocationHint("a large jar"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x373), ItemPool(ItemCategory.GreenRupees, LocationCategory.InvisibleItems), NullableItem]
        CollectableSwampSpiderHouseInvisibleItem3,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Swamp Spider House Invisible Item 4"), Region(Region.SwampSpiderHouseItems)]
        [GossipLocationHint("a large jar"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x374), ItemPool(ItemCategory.BlueRupees, LocationCategory.InvisibleItems), NullableItem]
        CollectableSwampSpiderHouseInvisibleItem4,


        [Visible]
        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Swamp Spider House Invisible Item 5"), Region(Region.SwampSpiderHouseItems)]
        [GossipLocationHint("a large jar"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x375), ItemPool(ItemCategory.RedRupees, LocationCategory.InvisibleItems), NullableItem]
        CollectableSwampSpiderHouseInvisibleItem5,


        [Visible]
        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Termina Field Tree Item"), Region(Region.TerminaField)]
        [GossipLocationHint("a tree"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x360), ItemPool(ItemCategory.RedRupees, LocationCategory.Freestanding), NullableItem]
        CollectableTerminaFieldTreeItem1,


        [Visible]
        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Termina Field Pillar Spawned Item"), Region(Region.TerminaField)]
        [GossipLocationHint("a pillar"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x361), ItemPool(ItemCategory.RedRupees, LocationCategory.Freestanding), NullableItem]
        CollectableTerminaFieldPillarItem1,


        [Visible]
        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Termina Field Telescope Guay"), Region(Region.TerminaField)]
        [GossipLocationHint("bird droppings"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x362), ItemPool(ItemCategory.RedRupees, LocationCategory.Events), NullableItem]
        CollectableTerminaFieldTelescopeGuay1,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Swordsman School Gong"), Region(Region.WestClockTown)]
        [GossipLocationHint("timekeeping"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x35C), ItemPool(ItemCategory.BlueRupees, LocationCategory.Events), NullableItem]
        CollectableSwordsmanSchoolGong1,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Bean Grotto Soft Soil"), Region(Region.DekuPalace)]
        [GossipLocationHint("underground soil"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x35E), ItemPool(ItemCategory.BlueRupees, LocationCategory.SoftSoil), NullableItem]
        CollectableBeanGrottoSoftSoil1,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Deku Palace Soft Soil"), Region(Region.DekuPalace)]
        [GossipLocationHint("royal soil", "the home of scrubs"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x36C), ItemPool(ItemCategory.BlueRupees, LocationCategory.SoftSoil), NullableItem]
        CollectableDekuPalaceSoftSoil1,


        [Visible]
        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Doggy Racetrack Soft Soil"), Region(Region.RomaniRanch)]
        [GossipLocationHint("a sporting arena"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x36D), ItemPool(ItemCategory.RedRupees, LocationCategory.SoftSoil), NullableItem]
        CollectableDoggyRacetrackSoftSoil1,


        [Visible]
        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Great Bay Coast Soft Soil"), Region(Region.GreatBayCoast)]
        [GossipLocationHint("ocean soil"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x36E), ItemPool(ItemCategory.RedRupees, LocationCategory.SoftSoil), NullableItem]
        CollectableGreatBayCoastSoftSoil1,


        [Visible]
        [Repeatable]
        [ItemName("30 Arrows"), LocationName("Ranch Day 1 Soil"), Region(Region.RomaniRanch)]
        [GossipLocationHint("early soil"), GossipItemHint("a quiver refill", "a bundle of projectiles")]
        [ShopText("Ammo for your bow.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x376), ItemPool(ItemCategory.Arrows, LocationCategory.SoftSoil), NullableItem]
        CollectableRomaniRanchSoftSoil1,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Ranch Day 2 or 3 Soil"), Region(Region.RomaniRanch)]
        [GossipLocationHint("late soil"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x370), ItemPool(ItemCategory.BlueRupees, LocationCategory.SoftSoil), NullableItem]
        CollectableRomaniRanchSoftSoil2,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Secret Shrine Soft Soil"), Region(Region.SecretShrine)]
        [GossipLocationHint("secret soil"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x36F), ItemPool(ItemCategory.BlueRupees, LocationCategory.SoftSoil), NullableItem]
        CollectableSecretShrineSoftSoil1,


        [Visible]
        [Repeatable]
        [ItemName("30 Arrows"), LocationName("Stone Tower Soft Soil Lower"), Region(Region.StoneTower)]
        [GossipLocationHint("high soil"), GossipItemHint("a quiver refill", "a bundle of projectiles")]
        [ShopText("Ammo for your bow.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x377), ItemPool(ItemCategory.Arrows, LocationCategory.SoftSoil), NullableItem]
        CollectableStoneTowerSoftSoil1,


        [Visible]
        [Repeatable]
        [ItemName("30 Arrows"), LocationName("Stone Tower Soft Soil Upper"), Region(Region.StoneTower)]
        [GossipLocationHint("high soil"), GossipItemHint("a quiver refill", "a bundle of projectiles")]
        [ShopText("Ammo for your bow.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x378), ItemPool(ItemCategory.Arrows, LocationCategory.SoftSoil), NullableItem]
        CollectableStoneTowerSoftSoil2,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Swamp Spider House Rock Soft Soil"), Region(Region.SwampSpiderHouseItems)]
        [GossipLocationHint("rock soil"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x379), ItemPool(ItemCategory.BlueRupees, LocationCategory.SoftSoil), NullableItem]
        CollectableSwampSpiderHouseSoftSoil1,


        [Visible]
        [Repeatable]
        [ItemName("30 Arrows"), LocationName("Swamp Spider House Gold Room Soft Soil"), Region(Region.SwampSpiderHouseItems)]
        [GossipLocationHint("gold soil"), GossipItemHint("a quiver refill", "a bundle of projectiles")]
        [ShopText("Ammo for your bow.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x37A), ItemPool(ItemCategory.Arrows, LocationCategory.SoftSoil), NullableItem]
        CollectableSwampSpiderHouseSoftSoil2,


        [Visible]
        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Termina Field Stump Soft Soil"), Region(Region.TerminaField)]
        [GossipLocationHint("field soil"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x37B), ItemPool(ItemCategory.RedRupees, LocationCategory.SoftSoil), NullableItem]
        CollectableTerminaFieldSoftSoil1,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Termina Field Observatory Soft Soil"), Region(Region.TerminaField)]
        [GossipLocationHint("field soil"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x37C), ItemPool(ItemCategory.BlueRupees, LocationCategory.SoftSoil), NullableItem]
        CollectableTerminaFieldSoftSoil2,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Termina Field South Wall Soft Soil"), Region(Region.TerminaField)]
        [GossipLocationHint("wall soil"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x37D), ItemPool(ItemCategory.BlueRupees, LocationCategory.SoftSoil), NullableItem]
        CollectableTerminaFieldSoftSoil3,


        [Visible]
        [Repeatable]
        [ItemName("30 Arrows"), LocationName("Termina Field Pillar Soft Soil"), Region(Region.TerminaField)]
        [GossipLocationHint("field soil"), GossipItemHint("a quiver refill", "a bundle of projectiles")]
        [ShopText("Ammo for your bow.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x37E), ItemPool(ItemCategory.Arrows, LocationCategory.SoftSoil), NullableItem]
        CollectableTerminaFieldSoftSoil4,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Termina Field Guay #1"), Region(Region.TerminaField)]
        [GossipLocationHint("bird droppings"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x37F), ItemPool(ItemCategory.GreenRupees, LocationCategory.Events), NullableItem]
        CollectableTerminaFieldGuay1,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Termina Field Guay #2"), Region(Region.TerminaField)]
        [GossipLocationHint("bird droppings"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x380), ItemPool(ItemCategory.GreenRupees, LocationCategory.Events), NullableItem]
        CollectableTerminaFieldGuay2,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Termina Field Guay #3"), Region(Region.TerminaField)]
        [GossipLocationHint("bird droppings"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x381), ItemPool(ItemCategory.GreenRupees, LocationCategory.Events), NullableItem]
        CollectableTerminaFieldGuay3,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Termina Field Guay #4"), Region(Region.TerminaField)]
        [GossipLocationHint("bird droppings"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x382), ItemPool(ItemCategory.GreenRupees, LocationCategory.Events), NullableItem]
        CollectableTerminaFieldGuay4,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Termina Field Guay #5"), Region(Region.TerminaField)]
        [GossipLocationHint("bird droppings"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x383), ItemPool(ItemCategory.GreenRupees, LocationCategory.Events), NullableItem]
        CollectableTerminaFieldGuay5,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Termina Field Guay #6"), Region(Region.TerminaField)]
        [GossipLocationHint("bird droppings"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x384), ItemPool(ItemCategory.GreenRupees, LocationCategory.Events), NullableItem]
        CollectableTerminaFieldGuay6,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Termina Field Guay #7"), Region(Region.TerminaField)]
        [GossipLocationHint("bird droppings"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x385), ItemPool(ItemCategory.GreenRupees, LocationCategory.Events), NullableItem]
        CollectableTerminaFieldGuay7,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Termina Field Guay #8"), Region(Region.TerminaField)]
        [GossipLocationHint("bird droppings"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x386), ItemPool(ItemCategory.GreenRupees, LocationCategory.Events), NullableItem]
        CollectableTerminaFieldGuay8,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Termina Field Guay #9"), Region(Region.TerminaField)]
        [GossipLocationHint("bird droppings"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x387), ItemPool(ItemCategory.GreenRupees, LocationCategory.Events), NullableItem]
        CollectableTerminaFieldGuay9,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Termina Field Guay #10"), Region(Region.TerminaField)]
        [GossipLocationHint("bird droppings"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x388), ItemPool(ItemCategory.GreenRupees, LocationCategory.Events), NullableItem]
        CollectableTerminaFieldGuay10,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Termina Field Guay #11"), Region(Region.TerminaField)]
        [GossipLocationHint("bird droppings"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x389), ItemPool(ItemCategory.GreenRupees, LocationCategory.Events), NullableItem]
        CollectableTerminaFieldGuay11,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Termina Field Guay #12"), Region(Region.TerminaField)]
        [GossipLocationHint("bird droppings"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x38A), ItemPool(ItemCategory.GreenRupees, LocationCategory.Events), NullableItem]
        CollectableTerminaFieldGuay12,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Termina Field Guay #13"), Region(Region.TerminaField)]
        [GossipLocationHint("bird droppings"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x38B), ItemPool(ItemCategory.GreenRupees, LocationCategory.Events), NullableItem]
        CollectableTerminaFieldGuay13,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Termina Field Guay #14"), Region(Region.TerminaField)]
        [GossipLocationHint("bird droppings"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x38C), ItemPool(ItemCategory.GreenRupees, LocationCategory.Events), NullableItem]
        CollectableTerminaFieldGuay14,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Termina Field Guay #15"), Region(Region.TerminaField)]
        [GossipLocationHint("bird droppings"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x38D), ItemPool(ItemCategory.GreenRupees, LocationCategory.Events), NullableItem]
        CollectableTerminaFieldGuay15,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Termina Field Guay #16"), Region(Region.TerminaField)]
        [GossipLocationHint("bird droppings"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x38E), ItemPool(ItemCategory.GreenRupees, LocationCategory.Events), NullableItem]
        CollectableTerminaFieldGuay16,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Termina Field Guay #17"), Region(Region.TerminaField)]
        [GossipLocationHint("bird droppings"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x38F), ItemPool(ItemCategory.GreenRupees, LocationCategory.Events), NullableItem]
        CollectableTerminaFieldGuay17,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Termina Field Guay #18"), Region(Region.TerminaField)]
        [GossipLocationHint("bird droppings"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x390), ItemPool(ItemCategory.GreenRupees, LocationCategory.Events), NullableItem]
        CollectableTerminaFieldGuay18,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Termina Field Guay #19"), Region(Region.TerminaField)]
        [GossipLocationHint("bird droppings"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x391), ItemPool(ItemCategory.GreenRupees, LocationCategory.Events), NullableItem]
        CollectableTerminaFieldGuay19,


        [Visible]
        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Termina Field Guay #20"), Region(Region.TerminaField)]
        [GossipLocationHint("bird droppings"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x392), ItemPool(ItemCategory.RedRupees, LocationCategory.Events), NullableItem]
        CollectableTerminaFieldGuay20,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Termina Field Guay #5a"), Region(Region.TerminaField)]
        [GossipLocationHint("bird droppings"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x393), ItemPool(ItemCategory.BlueRupees, LocationCategory.Events), NullableItem]
        CollectableTerminaFieldGuay21,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Termina Field Guay #10a"), Region(Region.TerminaField)]
        [GossipLocationHint("bird droppings"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x394), ItemPool(ItemCategory.BlueRupees, LocationCategory.Events), NullableItem]
        CollectableTerminaFieldGuay22,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Termina Field Guay #15a"), Region(Region.TerminaField)]
        [GossipLocationHint("bird droppings"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x395), ItemPool(ItemCategory.BlueRupees, LocationCategory.Events), NullableItem]
        CollectableTerminaFieldGuay23,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Deku Palace Rupee Cluster"), Region(Region.DekuPalace)]
        [GossipLocationHint("a royal circle", "the home of scrubs"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x396), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), NullableItem]
        CollectableDekuPalaceRupeeCluster1,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Deku Palace Rupee Cluster 2"), Region(Region.DekuPalace)]
        [GossipLocationHint("a royal circle", "the home of scrubs"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x397), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), NullableItem]
        CollectableDekuPalaceRupeeCluster2,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Deku Palace Rupee Cluster 3"), Region(Region.DekuPalace)]
        [GossipLocationHint("a royal circle", "the home of scrubs"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x398), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), NullableItem]
        CollectableDekuPalaceRupeeCluster3,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Deku Palace Rupee Cluster 4"), Region(Region.DekuPalace)]
        [GossipLocationHint("a royal circle", "the home of scrubs"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x399), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), NullableItem]
        CollectableDekuPalaceRupeeCluster4,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Deku Palace Rupee Cluster 5"), Region(Region.DekuPalace)]
        [GossipLocationHint("a royal circle", "the home of scrubs"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x39A), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), NullableItem]
        CollectableDekuPalaceRupeeCluster5,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Deku Palace Rupee Cluster 6"), Region(Region.DekuPalace)]
        [GossipLocationHint("a royal circle", "the home of scrubs"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x39B), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), NullableItem]
        CollectableDekuPalaceRupeeCluster6,


        [Visible]
        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Deku Palace Rupee Cluster 7"), Region(Region.DekuPalace)]
        [GossipLocationHint("a royal circle", "the home of scrubs"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x39C), ItemPool(ItemCategory.RedRupees, LocationCategory.Freestanding), NullableItem]
        CollectableDekuPalaceRupeeCluster7,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Ikana Graveyard Rupee Cluster"), Region(Region.IkanaGraveyard)]
        [GossipLocationHint("an unholy circle"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x39D), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), NullableItem]
        CollectableBeneathTheGraveyardRupeeCluster1,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Ikana Graveyard Rupee Cluster 2"), Region(Region.IkanaGraveyard)]
        [GossipLocationHint("an unholy circle"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x39E), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), NullableItem]
        CollectableBeneathTheGraveyardRupeeCluster2,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Ikana Graveyard Rupee Cluster 3"), Region(Region.IkanaGraveyard)]
        [GossipLocationHint("an unholy circle"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x39F), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), NullableItem]
        CollectableBeneathTheGraveyardRupeeCluster3,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Ikana Graveyard Rupee Cluster 4"), Region(Region.IkanaGraveyard)]
        [GossipLocationHint("an unholy circle"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3A0), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), NullableItem]
        CollectableBeneathTheGraveyardRupeeCluster4,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Ikana Graveyard Rupee Cluster 5"), Region(Region.IkanaGraveyard)]
        [GossipLocationHint("an unholy circle"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3A1), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), NullableItem]
        CollectableBeneathTheGraveyardRupeeCluster5,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Ikana Graveyard Rupee Cluster 6"), Region(Region.IkanaGraveyard)]
        [GossipLocationHint("an unholy circle"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3A2), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), NullableItem]
        CollectableBeneathTheGraveyardRupeeCluster6,


        [Visible]
        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Ikana Graveyard Rupee Cluster 7"), Region(Region.IkanaGraveyard)]
        [GossipLocationHint("an unholy circle"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3A3), ItemPool(ItemCategory.RedRupees, LocationCategory.Freestanding), NullableItem]
        CollectableBeneathTheGraveyardRupeeCluster7,


        [Visible]
        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Termina Field Song Wall Dawn"), Region(Region.TerminaField)]
        [GossipLocationHint("musical notes"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3A4), ItemPool(ItemCategory.RedRupees, LocationCategory.Events), NullableItem]
        CollectableTerminaFieldSongWall1,


        [Visible]
        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Termina Field Song Wall Dawn 2"), Region(Region.TerminaField)]
        [GossipLocationHint("musical notes"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3A5), ItemPool(ItemCategory.RedRupees, LocationCategory.Events), NullableItem]
        CollectableTerminaFieldSongWall2,


        [Visible]
        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Termina Field Song Wall Dawn 3"), Region(Region.TerminaField)]
        [GossipLocationHint("musical notes"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3A6), ItemPool(ItemCategory.RedRupees, LocationCategory.Events), NullableItem]
        CollectableTerminaFieldSongWall3,


        [Visible]
        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Termina Field Song Wall 0 / 8 / 12 / 16"), Region(Region.TerminaField)]
        [GossipLocationHint("musical notes"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3A7), ItemPool(ItemCategory.RedRupees, LocationCategory.Events), NullableItem]
        CollectableTerminaFieldSongWall4,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Termina Field Song Wall 0 / 8 / 12 / 16 2"), Region(Region.TerminaField)]
        [GossipLocationHint("musical notes"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3A8), ItemPool(ItemCategory.GreenRupees, LocationCategory.Events), NullableItem]
        CollectableTerminaFieldSongWall5,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Termina Field Song Wall 0 / 8 / 12 / 16 3"), Region(Region.TerminaField)]
        [GossipLocationHint("musical notes"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3A9), ItemPool(ItemCategory.GreenRupees, LocationCategory.Events), NullableItem]
        CollectableTerminaFieldSongWall6,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Termina Field Song Wall 2 / 10 / 14 / 18 / 22"), Region(Region.TerminaField)]
        [GossipLocationHint("musical notes"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3AA), ItemPool(ItemCategory.BlueRupees, LocationCategory.Events), NullableItem]
        CollectableTerminaFieldSongWall7,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Termina Field Song Wall 2 / 10 / 14 / 18 / 22 2"), Region(Region.TerminaField)]
        [GossipLocationHint("musical notes"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3AB), ItemPool(ItemCategory.BlueRupees, LocationCategory.Events), NullableItem]
        CollectableTerminaFieldSongWall8,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Termina Field Song Wall 2 / 10 / 14 / 18 / 22 3"), Region(Region.TerminaField)]
        [GossipLocationHint("musical notes"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3AC), ItemPool(ItemCategory.BlueRupees, LocationCategory.Events), NullableItem]
        CollectableTerminaFieldSongWall9,


        [Visible]
        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Termina Field Song Wall 4 / 20"), Region(Region.TerminaField)]
        [GossipLocationHint("musical notes"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3AD), ItemPool(ItemCategory.RedRupees, LocationCategory.Events), NullableItem]
        CollectableTerminaFieldSongWall10,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Termina Field Song Wall 4 / 20 2"), Region(Region.TerminaField)]
        [GossipLocationHint("musical notes"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3AE), ItemPool(ItemCategory.BlueRupees, LocationCategory.Events), NullableItem]
        CollectableTerminaFieldSongWall11,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Termina Field Song Wall 4 / 20 3"), Region(Region.TerminaField)]
        [GossipLocationHint("musical notes"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3AF), ItemPool(ItemCategory.BlueRupees, LocationCategory.Events), NullableItem]
        CollectableTerminaFieldSongWall12,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Termina Field Song Wall Odd Hours"), Region(Region.TerminaField)]
        [GossipLocationHint("musical notes"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3B0), ItemPool(ItemCategory.GreenRupees, LocationCategory.Events), NullableItem]
        CollectableTerminaFieldSongWall13,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Termina Field Song Wall Odd Hours 2"), Region(Region.TerminaField)]
        [GossipLocationHint("musical notes"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3B1), ItemPool(ItemCategory.GreenRupees, LocationCategory.Events), NullableItem]
        CollectableTerminaFieldSongWall14,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Termina Field Song Wall Odd Hours 3"), Region(Region.TerminaField)]
        [GossipLocationHint("musical notes"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3B2), ItemPool(ItemCategory.GreenRupees, LocationCategory.Events), NullableItem]
        CollectableTerminaFieldSongWall15,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Deku Playground Day 2 Item"), Region(Region.DekuPlaygroundItems)]
        [GossipLocationHint("a game for scrubs", "a playground", "a town game"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3B3), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), NullableItem]
        CollectableDekuPlaygroundItem1,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Deku Playground Day 2 Item 2"), Region(Region.DekuPlaygroundItems)]
        [GossipLocationHint("a game for scrubs", "a playground", "a town game"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3B4), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), NullableItem]
        CollectableDekuPlaygroundItem2,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Deku Playground Day 2 Item 3"), Region(Region.DekuPlaygroundItems)]
        [GossipLocationHint("a game for scrubs", "a playground", "a town game"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3B5), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), NullableItem]
        CollectableDekuPlaygroundItem3,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Deku Playground Day 2 Item 4"), Region(Region.DekuPlaygroundItems)]
        [GossipLocationHint("a game for scrubs", "a playground", "a town game"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3B6), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), NullableItem]
        CollectableDekuPlaygroundItem4,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Deku Playground Day 2 Item 5"), Region(Region.DekuPlaygroundItems)]
        [GossipLocationHint("a game for scrubs", "a playground", "a town game"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3B7), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), NullableItem]
        CollectableDekuPlaygroundItem5,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Deku Playground Day 2 Item 6"), Region(Region.DekuPlaygroundItems)]
        [GossipLocationHint("a game for scrubs", "a playground", "a town game"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3B8), ItemPool(ItemCategory.BlueRupees, LocationCategory.Freestanding), NullableItem]
        CollectableDekuPlaygroundItem6,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Deku Playground Day 1 Item"), Region(Region.DekuPlaygroundItems)]
        [GossipLocationHint("a game for scrubs", "a playground", "a town game"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3B9), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), NullableItem]
        CollectableDekuPlaygroundItem7,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Deku Playground Day 1 Item 2"), Region(Region.DekuPlaygroundItems)]
        [GossipLocationHint("a game for scrubs", "a playground", "a town game"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3BA), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), NullableItem]
        CollectableDekuPlaygroundItem8,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Deku Playground Day 1 Item 3"), Region(Region.DekuPlaygroundItems)]
        [GossipLocationHint("a game for scrubs", "a playground", "a town game"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3BB), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), NullableItem]
        CollectableDekuPlaygroundItem9,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Deku Playground Day 1 Item 4"), Region(Region.DekuPlaygroundItems)]
        [GossipLocationHint("a game for scrubs", "a playground", "a town game"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3BC), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), NullableItem]
        CollectableDekuPlaygroundItem10,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Deku Playground Day 1 Item 5"), Region(Region.DekuPlaygroundItems)]
        [GossipLocationHint("a game for scrubs", "a playground", "a town game"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3BD), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), NullableItem]
        CollectableDekuPlaygroundItem11,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Deku Playground Day 1 Item 6"), Region(Region.DekuPlaygroundItems)]
        [GossipLocationHint("a game for scrubs", "a playground", "a town game"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3BE), ItemPool(ItemCategory.BlueRupees, LocationCategory.Freestanding), NullableItem]
        CollectableDekuPlaygroundItem12,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Deku Playground Day 3 Item"), Region(Region.DekuPlaygroundItems)]
        [GossipLocationHint("a game for scrubs", "a playground", "a town game"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3BF), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), NullableItem]
        CollectableDekuPlaygroundItem13,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Deku Playground Day 3 Item 2"), Region(Region.DekuPlaygroundItems)]
        [GossipLocationHint("a game for scrubs", "a playground", "a town game"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3C0), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), NullableItem]
        CollectableDekuPlaygroundItem14,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Deku Playground Day 3 Item 3"), Region(Region.DekuPlaygroundItems)]
        [GossipLocationHint("a game for scrubs", "a playground", "a town game"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3C1), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), NullableItem]
        CollectableDekuPlaygroundItem15,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Deku Playground Day 3 Item 4"), Region(Region.DekuPlaygroundItems)]
        [GossipLocationHint("a game for scrubs", "a playground", "a town game"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3C2), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), NullableItem]
        CollectableDekuPlaygroundItem16,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Deku Playground Day 3 Item 5"), Region(Region.DekuPlaygroundItems)]
        [GossipLocationHint("a game for scrubs", "a playground", "a town game"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3C3), ItemPool(ItemCategory.BlueRupees, LocationCategory.Freestanding), NullableItem]
        CollectableDekuPlaygroundItem17,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Deku Playground Day 3 Item 6"), Region(Region.DekuPlaygroundItems)]
        [GossipLocationHint("a game for scrubs", "a playground", "a town game"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3C4), ItemPool(ItemCategory.GreenRupees, LocationCategory.Freestanding), NullableItem]
        CollectableDekuPlaygroundItem18,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Pirates' Fortress Skull Flag Left Eye"), Region(Region.PiratesFortressInterior)]
        [GossipLocationHint("a pirate flag"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3C5), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectablePiratesFortressHitTag1,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Pirates' Fortress Skull Flag Left Eye 2"), Region(Region.PiratesFortressInterior)]
        [GossipLocationHint("a pirate flag"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3C6), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectablePiratesFortressHitTag2,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Pirates' Fortress Skull Flag Left Eye 3"), Region(Region.PiratesFortressInterior)]
        [GossipLocationHint("a pirate flag"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3C7), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectablePiratesFortressHitTag3,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Pirates' Fortress Skull Flag Right Eye"), Region(Region.PiratesFortressInterior)]
        [GossipLocationHint("a pirate flag"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3C8), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectablePiratesFortressHitTag4,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Pirates' Fortress Skull Flag Right Eye 2"), Region(Region.PiratesFortressInterior)]
        [GossipLocationHint("a pirate flag"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3C9), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectablePiratesFortressHitTag5,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Pirates' Fortress Skull Flag Right Eye 3"), Region(Region.PiratesFortressInterior)]
        [GossipLocationHint("a pirate flag"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3CA), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectablePiratesFortressHitTag6,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Hookshot Room Skull Flag Forehead"), Region(Region.PiratesFortressInterior)]
        [GossipLocationHint("a pirate flag"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3CB), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectablePiratesFortressInteriorHookshotRoomHitTag1,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Hookshot Room Skull Flag Forehead 2"), Region(Region.PiratesFortressInterior)]
        [GossipLocationHint("a pirate flag"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3CC), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectablePiratesFortressInteriorHookshotRoomHitTag2,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Hookshot Room Skull Flag Forehead 3"), Region(Region.PiratesFortressInterior)]
        [GossipLocationHint("a pirate flag"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3CD), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectablePiratesFortressInteriorHookshotRoomHitTag3,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Swamp Spider House Blue Gem"), Region(Region.SwampSpiderHouseItems)]
        [GossipLocationHint("a blue gem"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3CE), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectableSwampSpiderHouseHitTag1,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Swamp Spider House Blue Gem 2"), Region(Region.SwampSpiderHouseItems)]
        [GossipLocationHint("a blue gem"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3CF), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectableSwampSpiderHouseHitTag2,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Swamp Spider House Blue Gem 3"), Region(Region.SwampSpiderHouseItems)]
        [GossipLocationHint("a blue gem"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3D0), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectableSwampSpiderHouseHitTag3,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Swamp Spider House Blue Gem 4"), Region(Region.SwampSpiderHouseItems)]
        [GossipLocationHint("a blue gem"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3D1), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectableSwampSpiderHouseHitTag4,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Swamp Spider House Blue Gem 5"), Region(Region.SwampSpiderHouseItems)]
        [GossipLocationHint("a blue gem"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3D2), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectableSwampSpiderHouseHitTag5,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Swamp Spider House Blue Gem 6"), Region(Region.SwampSpiderHouseItems)]
        [GossipLocationHint("a blue gem"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3D3), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectableSwampSpiderHouseHitTag6,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Swamp Spider House Blue Gem 7"), Region(Region.SwampSpiderHouseItems)]
        [GossipLocationHint("a blue gem"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3D4), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectableSwampSpiderHouseHitTag7,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Swamp Spider House Blue Gem 8"), Region(Region.SwampSpiderHouseItems)]
        [GossipLocationHint("a blue gem"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3D5), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectableSwampSpiderHouseHitTag8,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Swamp Spider House Blue Gem 9"), Region(Region.SwampSpiderHouseItems)]
        [GossipLocationHint("a blue gem"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3D6), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectableSwampSpiderHouseHitTag9,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Swamp Spider House Blue Gem 10"), Region(Region.SwampSpiderHouseItems)]
        [GossipLocationHint("a blue gem"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3D7), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectableSwampSpiderHouseHitTag10,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Swamp Spider House Blue Gem 11"), Region(Region.SwampSpiderHouseItems)]
        [GossipLocationHint("a blue gem"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3D8), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectableSwampSpiderHouseHitTag11,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Swamp Spider House Blue Gem 12"), Region(Region.SwampSpiderHouseItems)]
        [GossipLocationHint("a blue gem"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3D9), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectableSwampSpiderHouseHitTag12,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Oceanside Spider House Mask"), Region(Region.OceanSpiderHouseItems)]
        [GossipLocationHint("a creepy basement mask"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3DA), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectableOceansideSpiderHouseHitTag1,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Oceanside Spider House Mask 2"), Region(Region.OceanSpiderHouseItems)]
        [GossipLocationHint("a creepy basement mask"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3DB), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectableOceansideSpiderHouseHitTag2,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Oceanside Spider House Mask 3"), Region(Region.OceanSpiderHouseItems)]
        [GossipLocationHint("a creepy basement mask"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3DC), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectableOceansideSpiderHouseHitTag3,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Oceanside Spider House Mask 4"), Region(Region.OceanSpiderHouseItems)]
        [GossipLocationHint("a creepy basement mask"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3DD), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectableOceansideSpiderHouseHitTag4,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Oceanside Spider House Mask 5"), Region(Region.OceanSpiderHouseItems)]
        [GossipLocationHint("a creepy basement mask"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3DE), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectableOceansideSpiderHouseHitTag5,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Oceanside Spider House Mask 6"), Region(Region.OceanSpiderHouseItems)]
        [GossipLocationHint("a creepy basement mask"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3DF), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectableOceansideSpiderHouseHitTag6,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Oceanside Spider House Mask 7"), Region(Region.OceanSpiderHouseItems)]
        [GossipLocationHint("a creepy basement mask"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3E0), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectableOceansideSpiderHouseHitTag7,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Oceanside Spider House Mask 8"), Region(Region.OceanSpiderHouseItems)]
        [GossipLocationHint("a creepy basement mask"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3E1), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectableOceansideSpiderHouseHitTag8,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Oceanside Spider House Mask 9"), Region(Region.OceanSpiderHouseItems)]
        [GossipLocationHint("a creepy basement mask"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3E2), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectableOceansideSpiderHouseHitTag9,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Termina Field Clam"), Region(Region.TerminaField)]
        [GossipLocationHint("an accurate shot"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3E3), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectableTerminaFieldHitTag1,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Termina Field Clam 2"), Region(Region.TerminaField)]
        [GossipLocationHint("an accurate shot"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3E4), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectableTerminaFieldHitTag2,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Termina Field Clam 3"), Region(Region.TerminaField)]
        [GossipLocationHint("an accurate shot"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3E5), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectableTerminaFieldHitTag3,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Termina Field Wall"), Region(Region.TerminaField)]
        [GossipLocationHint("a wall"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3E6), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectableTerminaFieldHitTag4,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Termina Field Wall 2"), Region(Region.TerminaField)]
        [GossipLocationHint("a wall"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3E7), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectableTerminaFieldHitTag5,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Termina Field Wall 3"), Region(Region.TerminaField)]
        [GossipLocationHint("a wall"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3E8), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectableTerminaFieldHitTag6,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Termina Field Skull Kid Drawing"), Region(Region.TerminaField)]
        [GossipLocationHint("a drawing"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3E9), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectableTerminaFieldHitTag7,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Termina Field Skull Kid Drawing 2"), Region(Region.TerminaField)]
        [GossipLocationHint("a drawing"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3EA), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectableTerminaFieldHitTag8,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Termina Field Skull Kid Drawing 3"), Region(Region.TerminaField)]
        [GossipLocationHint("a drawing"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3EB), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectableTerminaFieldHitTag9,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Cucco Shack Diamond Hole"), Region(Region.RomaniRanch)]
        [GossipLocationHint("a diamond gap"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3EC), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectableCuccoShackHitTag1,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Cucco Shack Diamond Hole 2"), Region(Region.RomaniRanch)]
        [GossipLocationHint("a diamond gap"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3ED), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectableCuccoShackHitTag2,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Cucco Shack Diamond Hole 3"), Region(Region.RomaniRanch)]
        [GossipLocationHint("a diamond gap"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3EE), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectableCuccoShackHitTag3,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Cucco Shack Diamond Hole 4"), Region(Region.RomaniRanch)]
        [GossipLocationHint("a diamond gap"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3EF), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectableCuccoShackHitTag4,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Cucco Shack Diamond Hole 5"), Region(Region.RomaniRanch)]
        [GossipLocationHint("a diamond gap"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3F0), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectableCuccoShackHitTag5,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Cucco Shack Diamond Hole 6"), Region(Region.RomaniRanch)]
        [GossipLocationHint("a diamond gap"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3F1), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectableCuccoShackHitTag6,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Ikana Graveyard Lantern"), Region(Region.IkanaGraveyard)]
        [GossipLocationHint("a lantern"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3F2), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectableIkanaGraveyardHitTag1,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Ikana Graveyard Lantern 2"), Region(Region.IkanaGraveyard)]
        [GossipLocationHint("a lantern"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3F3), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectableIkanaGraveyardHitTag2,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Ikana Graveyard Lantern 3"), Region(Region.IkanaGraveyard)]
        [GossipLocationHint("a lantern"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3F4), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectableIkanaGraveyardHitTag3,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Ikana Graveyard Lantern 4"), Region(Region.IkanaGraveyard)]
        [GossipLocationHint("a lantern"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3F5), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectableIkanaGraveyardHitTag4,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Ikana Graveyard Lantern 5"), Region(Region.IkanaGraveyard)]
        [GossipLocationHint("a lantern"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3F6), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectableIkanaGraveyardHitTag5,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Ikana Graveyard Lantern 6"), Region(Region.IkanaGraveyard)]
        [GossipLocationHint("a lantern"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3F7), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectableIkanaGraveyardHitTag6,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Ikana Graveyard Lantern 7"), Region(Region.IkanaGraveyard)]
        [GossipLocationHint("a lantern"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3F8), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectableIkanaGraveyardHitTag7,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Ikana Graveyard Lantern 8"), Region(Region.IkanaGraveyard)]
        [GossipLocationHint("a lantern"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3F9), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectableIkanaGraveyardHitTag8,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Ikana Graveyard Lantern 9"), Region(Region.IkanaGraveyard)]
        [GossipLocationHint("a lantern"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3FA), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectableIkanaGraveyardHitTag9,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Ikana Graveyard Lantern 10"), Region(Region.IkanaGraveyard)]
        [GossipLocationHint("a lantern"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3FB), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectableIkanaGraveyardHitTag10,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Ikana Graveyard Lantern 11"), Region(Region.IkanaGraveyard)]
        [GossipLocationHint("a lantern"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3FC), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectableIkanaGraveyardHitTag11,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Ikana Graveyard Lantern 12"), Region(Region.IkanaGraveyard)]
        [GossipLocationHint("a lantern"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3FD), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectableIkanaGraveyardHitTag12,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Stock Pot Inn Mask"), Region(Region.StockPotInn)]
        [GossipLocationHint("a town mask"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3FE), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectableStockPotInnHitTag1,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Stock Pot Inn Mask 2"), Region(Region.StockPotInn)]
        [GossipLocationHint("a town mask"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x3FF), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectableStockPotInnHitTag2,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Stock Pot Inn Mask 3"), Region(Region.StockPotInn)]
        [GossipLocationHint("a town mask"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x400), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectableStockPotInnHitTag3,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("East Clock Town Target"), Region(Region.EastClockTown)]
        [GossipLocationHint("an accurate shot"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x401), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectableEastClockTownHitTag1,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("East Clock Town Target 2"), Region(Region.EastClockTown)]
        [GossipLocationHint("an accurate shot"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x402), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectableEastClockTownHitTag2,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("East Clock Town Target 3"), Region(Region.EastClockTown)]
        [GossipLocationHint("an accurate shot"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x403), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectableEastClockTownHitTag3,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("East Clock Town Target 4"), Region(Region.EastClockTown)]
        [GossipLocationHint("an accurate shot"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x404), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectableEastClockTownHitTag4,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("East Clock Town Target 5"), Region(Region.EastClockTown)]
        [GossipLocationHint("an accurate shot"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x405), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectableEastClockTownHitTag5,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("East Clock Town Target 6"), Region(Region.EastClockTown)]
        [GossipLocationHint("an accurate shot"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x406), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectableEastClockTownHitTag6,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("East Clock Town Basket"), Region(Region.EastClockTown)]
        [GossipLocationHint("an accurate shot"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x407), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectableEastClockTownHitTag7,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("East Clock Town Basket 2"), Region(Region.EastClockTown)]
        [GossipLocationHint("an accurate shot"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x408), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectableEastClockTownHitTag8,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("East Clock Town Basket 3"), Region(Region.EastClockTown)]
        [GossipLocationHint("an accurate shot"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x409), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectableEastClockTownHitTag9,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Clock Tower Clock"), Region(Region.SouthClockTown)]
        [GossipLocationHint("an accurate shot"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x40A), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectableSouthClockTownHitTag1,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Clock Tower Clock 2"), Region(Region.SouthClockTown)]
        [GossipLocationHint("an accurate shot"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x40B), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectableSouthClockTownHitTag2,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Clock Tower Clock 3"), Region(Region.SouthClockTown)]
        [GossipLocationHint("an accurate shot"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x40C), ItemPool(ItemCategory.GreenRupees, LocationCategory.HitSpots), NullableItem]
        CollectableSouthClockTownHitTag3,


        [Visible]
        [Repeatable]
        [ItemName("Gold Rupee"), LocationName("Takkuri"), Region(Region.TerminaField)]
        [GossipLocationHint("a thief"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 200 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x40D), ItemPool(ItemCategory.GoldRupees, LocationCategory.EnemySpawn), NullableItem]
        CollectableTerminaFieldEnemy1,



        [Visible]
        [Repeatable]
        [ItemName("10 Arrows"), LocationName("Hookshot Room Pot"), Region(Region.PiratesFortressInterior)]
        [GossipLocationHint("the home of pirates"), GossipItemHint("a quiver refill", "a bundle of projectiles")]
        [ShopText("Ammo for your bow.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x40E), ItemPool(ItemCategory.Arrows, LocationCategory.Jars), CollectableIndex(0x11AD)]
        CollectablePiratesFortressInteriorHookshotRoomPot1,


        [Visible]
        [Repeatable]
        [ItemName("10 Arrows"), LocationName("Hookshot Room Pot 2"), Region(Region.PiratesFortressInterior)]
        [GossipLocationHint("the home of pirates"), GossipItemHint("a quiver refill", "a bundle of projectiles")]
        [ShopText("Ammo for your bow.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x40F), ItemPool(ItemCategory.Arrows, LocationCategory.Jars), CollectableIndex(0x11AE)]
        CollectablePiratesFortressInteriorHookshotRoomPot2,


        [Visible]
        [Repeatable]
        [ItemName("30 Arrows"), LocationName("Termina Field Rock"), Region(Region.TerminaField)]
        [GossipLocationHint("a rock on a wall"), GossipItemHint("a quiver refill", "a bundle of projectiles")]
        [ShopText("Ammo for your bow.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x410), ItemPool(ItemCategory.Arrows, LocationCategory.Rocks), CollectableIndex(0x1694)]
        CollectableTerminaFieldRock1,


        [Visible]
        [Repeatable]
        [ItemName("30 Arrows"), LocationName("Termina Field Rock 2"), Region(Region.TerminaField)]
        [GossipLocationHint("a rock on a wall"), GossipItemHint("a quiver refill", "a bundle of projectiles")]
        [ShopText("Ammo for your bow.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x411), ItemPool(ItemCategory.Arrows, LocationCategory.Rocks), CollectableIndex(0x1695)]
        CollectableTerminaFieldRock2,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Ikana Graveyard Highest Rock"), Region(Region.IkanaGraveyard)]
        [GossipLocationHint("a rock on a wall"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x412), ItemPool(ItemCategory.BlueRupees, LocationCategory.Rocks), CollectableIndex(0x21A0)]
        CollectableIkanaGraveyardIkanaGraveyardUpperRock1,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Ikana Graveyard Lowest Rock"), Region(Region.IkanaGraveyard)]
        [GossipLocationHint("a rock on a wall"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x413), ItemPool(ItemCategory.BlueRupees, LocationCategory.Rocks), CollectableIndex(0x21A1)]
        CollectableIkanaGraveyardIkanaGraveyardUpperRock2,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Ikana Graveyard 2nd Lowest Rock"), Region(Region.IkanaGraveyard)]
        [GossipLocationHint("a rock on a wall"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x414), ItemPool(ItemCategory.BlueRupees, LocationCategory.Rocks), CollectableIndex(0x21A4)]
        CollectableIkanaGraveyardIkanaGraveyardUpperRock3,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Termina Field Rock 3"), Region(Region.TerminaField)]
        [GossipLocationHint("a rock on a wall"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x415), ItemPool(ItemCategory.BlueRupees, LocationCategory.Rocks), CollectableIndex(0x1690)]
        CollectableTerminaFieldRock3,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Termina Field Rock 4"), Region(Region.TerminaField)]
        [GossipLocationHint("a rock on a wall"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x416), ItemPool(ItemCategory.BlueRupees, LocationCategory.Rocks), CollectableIndex(0x1691)]
        CollectableTerminaFieldRock4,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Termina Field Rock 5"), Region(Region.TerminaField)]
        [GossipLocationHint("a rock on a wall"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x417), ItemPool(ItemCategory.BlueRupees, LocationCategory.Rocks), CollectableIndex(0x1692)]
        CollectableTerminaFieldRock5,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Termina Field Rock 6"), Region(Region.TerminaField)]
        [GossipLocationHint("a rock on a wall"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x418), ItemPool(ItemCategory.BlueRupees, LocationCategory.Rocks), CollectableIndex(0x1696)]
        CollectableTerminaFieldRock6,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Termina Field Rock 7"), Region(Region.TerminaField)]
        [GossipLocationHint("a rock on a wall"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x419), ItemPool(ItemCategory.BlueRupees, LocationCategory.Rocks), CollectableIndex(0x168F)]
        CollectableTerminaFieldRock7,


        [Visible]
        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Ikana Graveyard 2nd Highest Rock"), Region(Region.IkanaGraveyard)]
        [GossipLocationHint("a rock on a wall"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x41A), ItemPool(ItemCategory.RedRupees, LocationCategory.Rocks), CollectableIndex(0x21A2)]
        CollectableIkanaGraveyardIkanaGraveyardUpperRock4,


        [Visible]
        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Ikana Graveyard Middle Rock"), Region(Region.IkanaGraveyard)]
        [GossipLocationHint("a rock on a wall"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x41B), ItemPool(ItemCategory.RedRupees, LocationCategory.Rocks), CollectableIndex(0x21A3)]
        CollectableIkanaGraveyardIkanaGraveyardUpperRock5,


        [Visible]
        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Termina Field Rock 8"), Region(Region.TerminaField)]
        [GossipLocationHint("a rock on a wall"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x41C), ItemPool(ItemCategory.RedRupees, LocationCategory.Rocks), CollectableIndex(0x168D)]
        CollectableTerminaFieldRock8,


        [Visible]
        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Termina Field Rock 9"), Region(Region.TerminaField)]
        [GossipLocationHint("a rock on a wall"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x41D), ItemPool(ItemCategory.RedRupees, LocationCategory.Rocks), CollectableIndex(0x168E)]
        CollectableTerminaFieldRock9,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Milk Road Keaton Grass"), Region(Region.MilkRoad)]
        [GossipLocationHint("a living plant"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x41E), ItemPool(ItemCategory.GreenRupees, LocationCategory.Grass), NullableItem]
        CollectableMilkRoadKeatonGrass1,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Milk Road Keaton Grass 2"), Region(Region.MilkRoad)]
        [GossipLocationHint("a living plant"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x41F), ItemPool(ItemCategory.GreenRupees, LocationCategory.Grass), NullableItem]
        CollectableMilkRoadKeatonGrass2,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Milk Road Keaton Grass 3"), Region(Region.MilkRoad)]
        [GossipLocationHint("a living plant"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x420), ItemPool(ItemCategory.GreenRupees, LocationCategory.Grass), NullableItem]
        CollectableMilkRoadKeatonGrass3,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Milk Road Keaton Grass 4"), Region(Region.MilkRoad)]
        [GossipLocationHint("a living plant"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x421), ItemPool(ItemCategory.GreenRupees, LocationCategory.Grass), NullableItem]
        CollectableMilkRoadKeatonGrass4,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Milk Road Keaton Grass 5"), Region(Region.MilkRoad)]
        [GossipLocationHint("a living plant"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x422), ItemPool(ItemCategory.GreenRupees, LocationCategory.Grass), NullableItem]
        CollectableMilkRoadKeatonGrass5,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Milk Road Keaton Grass 6"), Region(Region.MilkRoad)]
        [GossipLocationHint("a living plant"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x423), ItemPool(ItemCategory.GreenRupees, LocationCategory.Grass), NullableItem]
        CollectableMilkRoadKeatonGrass6,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Milk Road Keaton Grass 7"), Region(Region.MilkRoad)]
        [GossipLocationHint("a living plant"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x424), ItemPool(ItemCategory.GreenRupees, LocationCategory.Grass), NullableItem]
        CollectableMilkRoadKeatonGrass7,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Milk Road Keaton Grass 8"), Region(Region.MilkRoad)]
        [GossipLocationHint("a living plant"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x425), ItemPool(ItemCategory.GreenRupees, LocationCategory.Grass), NullableItem]
        CollectableMilkRoadKeatonGrass8,


        [Visible]
        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Milk Road Keaton Grass 9"), Region(Region.MilkRoad)]
        [GossipLocationHint("a living plant"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x426), ItemPool(ItemCategory.RedRupees, LocationCategory.Grass), NullableItem]
        CollectableMilkRoadKeatonGrass9,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("North Clock Town Keaton Grass"), Region(Region.NorthClockTown)]
        [GossipLocationHint("a living plant"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x427), ItemPool(ItemCategory.GreenRupees, LocationCategory.Grass), NullableItem]
        CollectableNorthClockTownKeatonGrass1,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("North Clock Town Keaton Grass 2"), Region(Region.NorthClockTown)]
        [GossipLocationHint("a living plant"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x428), ItemPool(ItemCategory.GreenRupees, LocationCategory.Grass), NullableItem]
        CollectableNorthClockTownKeatonGrass2,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("North Clock Town Keaton Grass 3"), Region(Region.NorthClockTown)]
        [GossipLocationHint("a living plant"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x429), ItemPool(ItemCategory.GreenRupees, LocationCategory.Grass), NullableItem]
        CollectableNorthClockTownKeatonGrass3,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("North Clock Town Keaton Grass 4"), Region(Region.NorthClockTown)]
        [GossipLocationHint("a living plant"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x42A), ItemPool(ItemCategory.GreenRupees, LocationCategory.Grass), NullableItem]
        CollectableNorthClockTownKeatonGrass4,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("North Clock Town Keaton Grass 5"), Region(Region.NorthClockTown)]
        [GossipLocationHint("a living plant"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x42B), ItemPool(ItemCategory.GreenRupees, LocationCategory.Grass), NullableItem]
        CollectableNorthClockTownKeatonGrass5,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("North Clock Town Keaton Grass 6"), Region(Region.NorthClockTown)]
        [GossipLocationHint("a living plant"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x42C), ItemPool(ItemCategory.GreenRupees, LocationCategory.Grass), NullableItem]
        CollectableNorthClockTownKeatonGrass6,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("North Clock Town Keaton Grass 7"), Region(Region.NorthClockTown)]
        [GossipLocationHint("a living plant"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x42D), ItemPool(ItemCategory.GreenRupees, LocationCategory.Grass), NullableItem]
        CollectableNorthClockTownKeatonGrass7,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("North Clock Town Keaton Grass 8"), Region(Region.NorthClockTown)]
        [GossipLocationHint("a living plant"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x42E), ItemPool(ItemCategory.GreenRupees, LocationCategory.Grass), NullableItem]
        CollectableNorthClockTownKeatonGrass8,


        [Visible]
        [Repeatable]
        [ItemName("Red Rupee"), LocationName("North Clock Town Keaton Grass 9"), Region(Region.NorthClockTown)]
        [GossipLocationHint("a living plant"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x42F), ItemPool(ItemCategory.RedRupees, LocationCategory.Grass), NullableItem]
        CollectableNorthClockTownKeatonGrass9,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Mountain Village Spring Keaton Grass"), Region(Region.MountainVillage)]
        [GossipLocationHint("a living plant"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x430), ItemPool(ItemCategory.GreenRupees, LocationCategory.Grass), NullableItem]
        CollectableMountainVillageSpringKeatonGrass1,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Mountain Village Spring Keaton Grass 2"), Region(Region.MountainVillage)]
        [GossipLocationHint("a living plant"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x431), ItemPool(ItemCategory.GreenRupees, LocationCategory.Grass), NullableItem]
        CollectableMountainVillageSpringKeatonGrass2,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Mountain Village Spring Keaton Grass 3"), Region(Region.MountainVillage)]
        [GossipLocationHint("a living plant"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x432), ItemPool(ItemCategory.GreenRupees, LocationCategory.Grass), NullableItem]
        CollectableMountainVillageSpringKeatonGrass3,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Mountain Village Spring Keaton Grass 4"), Region(Region.MountainVillage)]
        [GossipLocationHint("a living plant"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x433), ItemPool(ItemCategory.GreenRupees, LocationCategory.Grass), NullableItem]
        CollectableMountainVillageSpringKeatonGrass4,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Mountain Village Spring Keaton Grass 5"), Region(Region.MountainVillage)]
        [GossipLocationHint("a living plant"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x434), ItemPool(ItemCategory.GreenRupees, LocationCategory.Grass), NullableItem]
        CollectableMountainVillageSpringKeatonGrass5,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Mountain Village Spring Keaton Grass 6"), Region(Region.MountainVillage)]
        [GossipLocationHint("a living plant"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x435), ItemPool(ItemCategory.GreenRupees, LocationCategory.Grass), NullableItem]
        CollectableMountainVillageSpringKeatonGrass6,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Mountain Village Spring Keaton Grass 7"), Region(Region.MountainVillage)]
        [GossipLocationHint("a living plant"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x436), ItemPool(ItemCategory.GreenRupees, LocationCategory.Grass), NullableItem]
        CollectableMountainVillageSpringKeatonGrass7,


        [Visible]
        [Repeatable]
        [ItemName("Green Rupee"), LocationName("Mountain Village Spring Keaton Grass 8"), Region(Region.MountainVillage)]
        [GossipLocationHint("a living plant"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 1 rupee.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x437), ItemPool(ItemCategory.GreenRupees, LocationCategory.Grass), NullableItem]
        CollectableMountainVillageSpringKeatonGrass8,


        [Visible]
        [Repeatable]
        [ItemName("Red Rupee"), LocationName("Mountain Village Spring Keaton Grass 9"), Region(Region.MountainVillage)]
        [GossipLocationHint("a living plant"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 20 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x438), ItemPool(ItemCategory.RedRupees, LocationCategory.Grass), NullableItem]
        CollectableMountainVillageSpringKeatonGrass9,



        [Visible]
        [Repeatable]
        [ItemName("10 Arrows"), LocationName("Oceanside Spider House Mask Room Pot"), Region(Region.OceanSpiderHouseItems)]
        [GossipLocationHint("a creepy basement pot"), GossipItemHint("a quiver refill", "a bundle of projectiles")]
        [ShopText("Ammo for your bow.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x439), ItemPool(ItemCategory.Arrows, LocationCategory.Jars), CollectableIndex(0x143A)]
        CollectableOceansideSpiderHouseMaskRoomPot1,


        [Visible]
        [Repeatable]
        [ItemName("10 Arrows"), LocationName("Oceanside Spider House Mask Room Pot 2"), Region(Region.OceanSpiderHouseItems)]
        [GossipLocationHint("a creepy basement pot"), GossipItemHint("a quiver refill", "a bundle of projectiles")]
        [ShopText("Ammo for your bow.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x43A), ItemPool(ItemCategory.Arrows, LocationCategory.Jars), CollectableIndex(0x143B)]
        CollectableOceansideSpiderHouseMaskRoomPot2,


        [Visible]
        [Repeatable]
        [ItemName("30 Arrows"), LocationName("Ikana Canyon Cleared Grass"), Region(Region.IkanaCanyon)]
        [GossipLocationHint("uncursed grass"), GossipItemHint("a quiver refill", "a bundle of projectiles")]
        [ShopText("Ammo for your bow.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x43B), ItemPool(ItemCategory.Arrows, LocationCategory.Grass), CollectableIndex(0x9A2)]
        CollectableIkanaCanyonMainAreaGrass4,



        [Visible]
        [Repeatable]
        [ItemName("5 Bombs"), LocationName("Ikana Canyon Cleared Grass 2"), Region(Region.IkanaCanyon)]
        [GossipLocationHint("uncursed grass"), GossipItemHint("explosives")]
        [ShopText("Explosives. You need a Bomb Bag to carry them.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x43C), ItemPool(ItemCategory.Bombs, LocationCategory.Grass), CollectableIndex(0x9A1)]
        CollectableIkanaCanyonMainAreaGrass5,



        [Visible]
        [Repeatable]
        [ItemName("Large Magic Jar"), LocationName("Ikana Canyon Cleared Grass 3"), Region(Region.IkanaCanyon)]
        [GossipLocationHint("uncursed grass"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a large amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x43D), ItemPool(ItemCategory.MagicJars, LocationCategory.Grass), CollectableIndex(0x9A0)]
        CollectableIkanaCanyonMainAreaGrass6,


        [Visible]
        [Repeatable]
        [ItemName("Large Magic Jar"), LocationName("Path to Snowhead Spring Snowball"), Region(Region.PathToSnowhead)]
        [GossipLocationHint("a melting snowball"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a large amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x43E), ItemPool(ItemCategory.MagicJars, LocationCategory.SmallSnowballs), CollectableIndex(0x2DA0)]
        CollectablePathToSnowheadSmallSnowball1,


        [Visible]
        [Repeatable]
        [ItemName("Large Magic Jar"), LocationName("Path to Snowhead Spring Snowball 2"), Region(Region.PathToSnowhead)]
        [GossipLocationHint("a melting snowball"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a large amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x43F), ItemPool(ItemCategory.MagicJars, LocationCategory.SmallSnowballs), CollectableIndex(0x2DA1)]
        CollectablePathToSnowheadSmallSnowball2,


        [Visible]
        [Repeatable]
        [ItemName("Large Magic Jar"), LocationName("Path to Snowhead Spring Snowball 3"), Region(Region.PathToSnowhead)]
        [GossipLocationHint("a melting snowball"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a large amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x440), ItemPool(ItemCategory.MagicJars, LocationCategory.SmallSnowballs), CollectableIndex(0x2DA2)]
        CollectablePathToSnowheadSmallSnowball3,


        [Visible]
        [Repeatable]
        [ItemName("Large Magic Jar"), LocationName("Path to Snowhead Spring Snowball 4"), Region(Region.PathToSnowhead)]
        [GossipLocationHint("a melting snowball"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a large amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x441), ItemPool(ItemCategory.MagicJars, LocationCategory.SmallSnowballs), CollectableIndex(0x2DA3)]
        CollectablePathToSnowheadSmallSnowball4,



        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Path to Mountain Village Spring Snowball"), Region(Region.PathToMountainVillage)]
        [GossipLocationHint("a melting snowball"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x442), ItemPool(ItemCategory.MagicJars, LocationCategory.SmallSnowballs), CollectableIndex(0x38A1)]
        CollectablePathToMountainVillageSmallSnowball2,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Path to Mountain Village Spring Snowball 2"), Region(Region.PathToMountainVillage)]
        [GossipLocationHint("a melting snowball"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x443), ItemPool(ItemCategory.MagicJars, LocationCategory.SmallSnowballs), CollectableIndex(0x38A2)]
        CollectablePathToMountainVillageSmallSnowball3,


        [Visible]
        [Repeatable]
        [ItemName("Small Magic Jar"), LocationName("Path to Mountain Village Spring Snowball 3"), Region(Region.PathToMountainVillage)]
        [GossipLocationHint("a melting snowball"), GossipItemHint("a magic refill")]
        [ShopText("Replenishes a small amount of your magic power.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x444), ItemPool(ItemCategory.MagicJars, LocationCategory.SmallSnowballs), CollectableIndex(0x38A5)]
        CollectablePathToMountainVillageSmallSnowball4,


        [Purchaseable]
        [Repeatable]
        [ItemName("Silver Rupee"), LocationName("Zora Cape Jar Game"), Region(Region.ZoraCape)]
        [GossipLocationHint("an ocean game"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 100 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x445), ItemPool(ItemCategory.SilverRupees, LocationCategory.NpcRewards)]
        CollectableZoraCapeJarGame1,


        [Visible]
        [Repeatable]
        [ItemName("Crimson Rupee"), LocationName("Ikana Graveyard Day 2 Bats"), Region(Region.IkanaGraveyard)]
        [GossipLocationHint("a swarm of bats"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 30 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x446), ItemPool(ItemCategory.RedRupees, LocationCategory.NpcRewards)]
        [ExclusiveItemMessage(0x9001, "\u0017You got a \u0001Crimson Rupee\u0000!\u0018\u0011It's worth \u000130 Rupees\u0000!\u0011What a pleasant surprise!\u00BF")]
        CollectableIkanaGraveyardDay2Bats1,


        [Visible]
        [Repeatable]
        [ItemName("Blue Rupee"), LocationName("Cucco Shack Potted Plant"), Region(Region.RomaniRanch)]
        [GossipLocationHint("a potted plant"), GossipItemHint("currency", "money", "cash", "wealth", "riches and stuff")]
        [ShopText("This is worth 5 rupees.")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [GetItemIndex(0x447), ItemPool(ItemCategory.BlueRupees, LocationCategory.Jars), CollectableIndex(0x210B)]
        CollectableCuccoShackPottedPlant1,


        [Visible]
        [StartingItem(0xC5CE73, 0x01)]
        [ItemName("Odolwa's Remains"), LocationName("Woodfall Boss Warp"), Region(Region.WoodfallTemple)]
        [GossipLocationHint("a masked evil"), GossipItemHint("an evil mask")]
        [ShopText("The remains of the boss in Woodfall Temple.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [GetItemIndex(0x448), ItemPool(ItemCategory.BossRemains, LocationCategory.BossFights)]
        RemainsOdolwa,


        [Visible]
        [StartingItem(0xC5CE73, 0x02)]
        [ItemName("Goht's Remains"), LocationName("Snowhead Boss Warp"), Region(Region.SnowheadTemple)]
        [GossipLocationHint("a masked evil"), GossipItemHint("an evil mask")]
        [ShopText("The remains of the boss in Snowhead Temple.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [GetItemIndex(0x449), ItemPool(ItemCategory.BossRemains, LocationCategory.BossFights)]
        RemainsGoht,


        [Visible]
        [StartingItem(0xC5CE73, 0x04)]
        [ItemName("Gyorg's Remains"), LocationName("Great Bay Boss Warp"), Region(Region.GreatBayTemple)]
        [GossipLocationHint("a masked evil"), GossipItemHint("an evil mask")]
        [ShopText("The remains of the boss in Great Bay Temple.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [GetItemIndex(0x44A), ItemPool(ItemCategory.BossRemains, LocationCategory.BossFights)]
        RemainsGyorg,


        [Visible]
        [StartingItem(0xC5CE73, 0x08)]
        [ItemName("Twinmold's Remains"), LocationName("Stone Tower Boss Warp"), Region(Region.StoneTowerTemple)]
        [GossipLocationHint("a masked evil"), GossipItemHint("an evil mask")]
        [ShopText("The remains of the boss in Stone Tower Temple.", isMultiple: true)]
        [ChestType(ChestTypeAttribute.ChestType.LargeGold)]
        [GetItemIndex(0x44B), ItemPool(ItemCategory.BossRemains, LocationCategory.BossFights)]
        RemainsTwinmold,



        //multilocation items
        [MainLocation(ItemTingleMapTown), Region(Region.NorthClockTown)]
        ItemTingleMapTownInTown,

        [MainLocation(ItemTingleMapTown), Region(Region.IkanaCanyon)]
        ItemTingleMapTownInCanyon,

        [MainLocation(ItemTingleMapWoodfall), Region(Region.RoadToSouthernSwamp)]
        ItemTingleMapWoodfallInSwamp,

        [MainLocation(ItemTingleMapWoodfall), Region(Region.NorthClockTown)]
        ItemTingleMapWoodfallInTown,

        [MainLocation(ItemTingleMapSnowhead), Region(Region.TwinIslands)]
        ItemTingleMapSnowheadInMountain,

        [MainLocation(ItemTingleMapSnowhead), Region(Region.RoadToSouthernSwamp)]
        ItemTingleMapSnowheadInSwamp,

        [MainLocation(ItemTingleMapRanch), Region(Region.MilkRoad)]
        ItemTingleMapRanchInRanch,

        [MainLocation(ItemTingleMapRanch), Region(Region.TwinIslands)]
        ItemTingleMapRanchInMountain,

        [MainLocation(ItemTingleMapGreatBay), Region(Region.GreatBayCoast)]
        ItemTingleMapGreatBayInOcean,

        [MainLocation(ItemTingleMapGreatBay), Region(Region.MilkRoad)]
        ItemTingleMapGreatBayInRanch,

        [MainLocation(ItemTingleMapStoneTower), Region(Region.IkanaCanyon)]
        ItemTingleMapStoneTowerInCanyon,

        [MainLocation(ItemTingleMapStoneTower), Region(Region.GreatBayCoast)]
        ItemTingleMapStoneTowerInOcean,

        [MainLocation(HeartPiecePostBox), Region(Region.SouthClockTown)]
        HeartPiecePostBoxInSCT,

        [MainLocation(HeartPiecePostBox), Region(Region.NorthClockTown)]
        HeartPiecePostBoxInNCT,

        [MainLocation(HeartPiecePostBox), Region(Region.EastClockTown)]
        HeartPiecePostBoxInECT,

        [MainLocation(HeartPieceKeatonQuiz), Region(Region.NorthClockTown)]
        HeartPieceKeatonQuizInNCT,

        [MainLocation(HeartPieceKeatonQuiz), Region(Region.MilkRoad)]
        HeartPieceKeatonQuizInMilkRoad,

        [MainLocation(HeartPieceKeatonQuiz), Region(Region.MountainVillage)]
        HeartPieceKeatonQuizInMountainVillage,

        [MainLocation(SongOath), Region(Region.WoodfallTemple)]
        SongOathInWFT,

        [MainLocation(SongOath), Region(Region.SnowheadTemple)]
        SongOathInSHT,

        [MainLocation(SongOath), Region(Region.GreatBayTemple)]
        SongOathInGBT,

        [MainLocation(SongOath), Region(Region.StoneTowerTemple)]
        SongOathInISTT,

        [MainLocation(ShopItemGoronBomb10), Region(Region.GoronVillage)]
        ShopItemGoronBomb10InWinter,

        [MainLocation(ShopItemGoronBomb10), Region(Region.GoronVillage)]
        ShopItemGoronBomb10InSpring,

        [MainLocation(ShopItemGoronArrow10), Region(Region.GoronVillage)]
        ShopItemGoronArrow10InWinter,

        [MainLocation(ShopItemGoronArrow10), Region(Region.GoronVillage)]
        ShopItemGoronArrow10InSpring,

        [MainLocation(ShopItemGoronRedPotion), Region(Region.GoronVillage)]
        ShopItemGoronRedPotionInWinter,

        [MainLocation(ShopItemGoronRedPotion), Region(Region.GoronVillage)]
        ShopItemGoronRedPotionInSpring,

        [MainLocation(ShopItemBusinessScrubMagicBean), Region(Region.SouthernSwamp)]
        ShopItemBusinessScrubMagicBeanInSwamp,

        [MainLocation(ShopItemBusinessScrubMagicBean), Region(Region.SouthClockTown)]
        ShopItemBusinessScrubMagicBeanInTown,

        [MainLocation(UpgradeBiggestBombBag), Region(Region.GoronVillage)]
        UpgradeBiggestBombBagInMountain,

        [MainLocation(UpgradeBiggestBombBag), Region(Region.SouthernSwamp)]
        UpgradeBiggestBombBagInSwamp,

        [MainLocation(ShopItemBusinessScrubGreenPotion), Region(Region.ZoraHall)]
        ShopItemBusinessScrubGreenPotionInOcean,

        [MainLocation(ShopItemBusinessScrubGreenPotion), Region(Region.GoronVillage)]
        ShopItemBusinessScrubGreenPotionInMountain,

        [MainLocation(ShopItemBusinessScrubBluePotion), Region(Region.IkanaCanyon)]
        ShopItemBusinessScrubBluePotionInCanyon,

        [MainLocation(ShopItemBusinessScrubBluePotion), Region(Region.ZoraHall)]
        ShopItemBusinessScrubBluePotionInOcean,

        [MainLocation(CollectibleStrayFairyClockTown), Region(Region.LaundryPool)]
        CollectibleStrayFairyClockTownInLaundryPool,

        [MainLocation(CollectibleStrayFairyClockTown), Region(Region.EastClockTown)]
        CollectibleStrayFairyClockTownInECT,


        GossipTerminaSouth,
        GossipSwampPotionShop,
        GossipMountainSpringPath,
        GossipMountainPath,
        GossipOceanZoraGame,
        GossipCanyonRoad,
        GossipCanyonDock,
        GossipCanyonSpiritHouse,
        GossipTerminaMilk,
        GossipTerminaWest,
        GossipTerminaNorth,
        GossipTerminaEast,
        GossipRanchTree,
        GossipRanchBarn,
        GossipMilkRoad,
        GossipOceanFortress,
        GossipSwampRoad,
        GossipTerminaObservatory,
        GossipRanchCuccoShack,
        GossipRanchRacetrack,
        GossipRanchEntrance,
        GossipCanyonRavine,
        GossipMountainSpringFrog,
        GossipSwampSpiderHouse,
        GossipTerminaGossipLarge,
        GossipTerminaGossipGuitar,
        GossipTerminaGossipPipes,
        GossipTerminaGossipDrums,

        [Repeatable]
        [ItemName("Ice Trap")]
        [GossipItemHint("a cold surprise", "an icy breeze")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [ExclusiveItem(0xB0)]
        [ExclusiveItemGraphic(0, 0)]
        [ExclusiveItemMessage(0x9000, "\u0017You are a \u0003FOOL\u0000!\u0018\u00BF")]
        IceTrap = -1,

        [ItemName("Recovery Heart")]
        [GossipItemHint("health")]
        [ChestType(ChestTypeAttribute.ChestType.SmallWooden)]
        [ShopText("Replenishes a small amount of your life energy.")]
        [GetItemIndex(0x0A), ItemPool(ItemCategory.Fake, LocationCategory.Fake)]
        RecoveryHeart = -2,

    }
}
