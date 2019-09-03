using System.Collections.ObjectModel;
using MMRando.GameObjects;

namespace MMRando.Models
{
    public class Gossip
    {
        public string[] LocationMessage { get; set; }
        public string[] ItemMessage { get; set; }


        public static readonly ReadOnlyCollection<string> MessageStartSentences
            = new ReadOnlyCollection<string>(new string[]
            {
                "They say",
                "I hear",
                "It seems",
                "Apparently,",
                "It appears"
            });


        public static readonly ReadOnlyCollection<string> MessageMidSentences
            = new ReadOnlyCollection<string>(new string[]
            {
                "leads to",
                "yields",
                "brings",
                "holds",
                "conceals",
                "possesses"
            });


        public static readonly ReadOnlyCollection<string> JunkMessages
            = new ReadOnlyCollection<string>(new string[]
            {
                "\x1E\x69\x4FThey say that Jimmie1717's mod\x11lottery is \x01RIGGED!\x00\xBF",
                "\x1E\x69\x4FReal ZELDA players use HOLD targeting!\xBF",
                "\x1E\x69\x4FThey say items are random...\xBF",
                "\x1E\x69\x4FThey say the \x05" + "blue dog\x00 shall prevail...\xBF",
                "\x1E\x69\x4FMy body craves for the touch of\x11\x01mashed potatoes\x00...\xBF",
                "\x1E\x69\x2B" + "Dear Mario, please come to the \x11" + "castle. I've baked a cake for you.\x11Yours truly, Princess Toadstool\x11\x06Peach\x00\xBF",
                "\x1E\x69\x56I overheard something useful:\x11\xDF\xBF",
                "\x1E\x69\x56I overheard something useful:\x11\xD6\xBF",
                "\x1E\x69\x4FThey say the best button for bombchus\x11is \x04\xB7\x00...\xBF",
                "\x1E\x69\x4FThey say the key to victory is\x11" + "beating the game...\xBF",
                "\x1E\x38\x0BThey say a certain player once stole\x11their items back from Takkuri...\xBF",
                "\x1E\x69\x4FThey say wearing the \x01" + "Bremen Mask\x00\x11increases your chances of beating the\x11Gorman bros...\xBF",
                "\x1E\x69\x6FUse the boost to get through!\xBF",
                "\x1E\x69\x4FThey say the \x04gold dog\x00 cheats...\xBF"
            });

        public static readonly ReadOnlyCollection<string> HelpfulMessages
            = new ReadOnlyCollection<string>(new string[]
            {
                // todo
            });

        public static readonly ReadOnlyCollection<Item> GuaranteedLocationHints
            = new ReadOnlyCollection<Item>(new Item[]
            {
                Item.HeartPieceLabFish,
                Item.ItemBottleDampe,
                Item.HeartPieceDekuPlayground,
                Item.MaskTruth,
                Item.UpgradeGiantWallet,
                Item.MaskRomani,
                Item.MaskAllNight,
                //Item.MaskScents,
                Item.HeartPieceBoatArchery,
                Item.ItemGoldDust
            });
    }
}
