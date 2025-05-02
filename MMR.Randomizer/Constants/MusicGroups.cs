using System;
using System.Collections.Generic;

namespace MMR.Randomizer.Constants
{
    public static class MusicGroups
    {
        // There's a few cases of alternative names being allowed, and quite a bit of special handling for spaces and proper punctuation
        // They could be removed to have a more standardized list... After all, why not? Why shouldn't I keep it?
        public enum Type
        {
            Bgm,
            Fanfare
        }

        public enum Category : int
        {
            // Group BGM Categories
            Fields               = 0x00,
            Towns                = 0x01,
            Dungeons             = 0x02,
            Indoors              = 0x03,
            Minigames            = 0x04,
            ActionThemes         = 0x05,
            CalmThemes           = 0x06,
            Fights               = 0x07,

            // Group Fanfare Categories
            ItemFanfares         = 0x08,
            EventFanfares        = 0x09,
            ClearFanfares        = 0x10,

            // Group Cutscene Categories
            Cutscenes            = 0x16,

            // Individual Categories
            // BGM
            TerminaField         = 0x102,
            PursuitTheme         = 0x103,
            MajorasTheme         = 0x104,
            ClockTower           = 0x105,
            StoneTower           = 0x106,
            InvertedStoneTower   = 0x107,
            HealingTheme         = 0x10B,
            SouthernSwamp        = 0x10C,
            AliensTheme          = 0x10D,
            BoatCruise           = 0x10E,
            SharpsCurse          = 0x10F,
            GreatBay             = 0x110,
            IkanaCanyon          = 0x111,
            DekuPalace           = 0x112,
            Snowhead             = 0x113,
            PiratesFortress      = 0x114,
            ClockTown1           = 0x115,
            ClockTown2           = 0x116,
            ClockTown3           = 0x117,
            FileSelect           = 0x118,
            SmallEnemy           = 0x11A,
            BossEnemy            = 0x11B,
            WoodfallTemple       = 0x11C,
            House                = 0x11F,
            MinigameTheme        = 0x125,
            GoronRace            = 0x126,
            MusicBoxHouse        = 0x127,
            GreatFairysFountain  = 0x128,
            FairysFountain       = GreatFairysFountain, // Just in case
            ZeldasTheme          = 0x129,
            RosaSistersTheme     = 0x12A,
            CuriosityShop        = 0x12C,
            MarineResearchLab    = CuriosityShop, // Just in case
            GiantsTheme          = 0x12D,
            GuruGurusTheme       = 0x12E,
            RomaniRanch          = 0x12F,
            GoronShrine          = 0x130,
            MayorsOffice         = 0x131,
            ZoraHall             = 0x136,
            BigEnemy             = 0x138,
            AstralObservatory    = 0x13A,
            SecretGrotto         = 0x13B,
            MilkBar              = 0x13C,
            WoodsOfMystery       = 0x13E,
            MysteryWoods         = WoodsOfMystery, // Just in case
            HorseRace            = 0x140,
            GormanBrosTheme      = 0x142,
            WitchesTheme         = 0x143,
            KoumeAndKotakesTheme = WitchesTheme, // Just in case
            ItemShop             = 0x144,
            OwlsTheme            = 0x145,
            KaeporaGaeborasTheme = OwlsTheme, // Just in case
            MinigameShop         = 0x146,
            SwordSchool          = 0x150,
            FinalHours           = 0x157,
            SnowheadTemple       = 0x165,
            GreatBayTemple       = 0x166,
            MajorasWrath         = 0x169,
            MajorasIncarnation   = 0x16A,
            MajorasMask          = 0x16B,
            JapasRoom            = 0x16C,
            TijosRoom            = 0x16D,
            EvansRoom            = 0x16E,
            IkanaCastle          = 0x16F,
            KamarosTheme         = 0x171,
            CremiasTheme         = 0x172,
            KeatonsTheme         = 0x173,
            MoonEnraged          = 0x17B,
            ReunionTheme         = 0x17D,

            // Fanfares
            EventFail1           = 0x108,
            EventFail2           = 0x109,
            EventSuccess         = 0x119,
            GameOver             = 0x120,
            BossDefeated         = 0x121,
            ItemGet              = 0x122,
            HeartContainerGet    = 0x124,
            OpenChest            = 0x12B,
            MaskGet              = 0x137,
            HeartPieceGet        = 0x139,
            TruthRevealed        = 0x13D,
            GoronRaceWin         = 0x13F,
            HorseRaceWin         = 0x141,
            SongGet              = 0x152,
            SoaringTheme         = 0x155,
            TempleAppears        = 0x177,
            TempleClearShort     = 0x178,
            TempleClearLong      = 0x179,
            GiantsLeave          = 0x17C,
            MoonDestroyed        = 0x17E,

            // Cutscenes
            GiantsAppear         = 0x170,
            TitleDemo            = 0x176,
        }

        // Stores the type for each category
        public static readonly Dictionary<Category, Type> CategoryTypes = new()
        {
            // Group BGM Category
            { Category.Fields,               Type.Bgm },
            { Category.Towns,                Type.Bgm },
            { Category.Dungeons,             Type.Bgm },
            { Category.Indoors,              Type.Bgm },
            { Category.Minigames,            Type.Bgm },
            { Category.ActionThemes,         Type.Bgm },
            { Category.CalmThemes,           Type.Bgm },
            { Category.Fights,               Type.Bgm },

            // Group Fanfares Category
            { Category.ItemFanfares,         Type.Fanfare },
            { Category.EventFanfares,        Type.Fanfare },
            { Category.ClearFanfares,        Type.Fanfare },

            // Group Cutscenes Category
            { Category.Cutscenes,            Type.Bgm },

            // Individual BGM Categories
            { Category.TerminaField,         Type.Bgm },
            { Category.PursuitTheme,         Type.Bgm },
            { Category.MajorasTheme,         Type.Bgm },
            { Category.ClockTower,           Type.Bgm },
            { Category.StoneTower,           Type.Bgm },
            { Category.InvertedStoneTower,   Type.Bgm },
            { Category.HealingTheme,         Type.Bgm },
            { Category.SouthernSwamp,        Type.Bgm },
            { Category.AliensTheme,          Type.Bgm },
            { Category.BoatCruise,           Type.Bgm },
            { Category.SharpsCurse,          Type.Bgm },
            { Category.GreatBay,             Type.Bgm },
            { Category.IkanaCanyon,          Type.Bgm },
            { Category.DekuPalace,           Type.Bgm },
            { Category.Snowhead,             Type.Bgm },
            { Category.PiratesFortress,      Type.Bgm },
            { Category.ClockTown1,           Type.Bgm },
            { Category.ClockTown2,           Type.Bgm },
            { Category.ClockTown3,           Type.Bgm },
            { Category.FileSelect,           Type.Bgm },
            { Category.SmallEnemy,           Type.Bgm },
            { Category.BossEnemy,            Type.Bgm },
            { Category.WoodfallTemple,       Type.Bgm },
            { Category.House,                Type.Bgm },
            { Category.MinigameTheme,        Type.Bgm },
            { Category.GoronRace,            Type.Bgm },
            { Category.MusicBoxHouse,        Type.Bgm },
            { Category.GreatFairysFountain,  Type.Bgm },
            { Category.ZeldasTheme,          Type.Bgm },
            { Category.RosaSistersTheme,     Type.Bgm },
            { Category.CuriosityShop,        Type.Bgm },
            { Category.GiantsTheme,          Type.Bgm },
            { Category.GuruGurusTheme,       Type.Bgm },
            { Category.RomaniRanch,          Type.Bgm },
            { Category.GoronShrine,          Type.Bgm },
            { Category.MayorsOffice,         Type.Bgm },
            { Category.ZoraHall,             Type.Bgm },
            { Category.BigEnemy,             Type.Bgm },
            { Category.AstralObservatory,    Type.Bgm },
            { Category.SecretGrotto,         Type.Bgm },
            { Category.MilkBar,              Type.Bgm },
            { Category.WoodsOfMystery,       Type.Bgm },
            { Category.HorseRace,            Type.Bgm },
            { Category.GormanBrosTheme,      Type.Bgm },
            { Category.WitchesTheme,         Type.Bgm },
            { Category.ItemShop,             Type.Bgm },
            { Category.OwlsTheme,            Type.Bgm },
            { Category.MinigameShop,         Type.Bgm },
            { Category.SwordSchool,          Type.Bgm },
            { Category.FinalHours,           Type.Bgm },
            { Category.SnowheadTemple,       Type.Bgm },
            { Category.GreatBayTemple,       Type.Bgm },
            { Category.MajorasWrath,         Type.Bgm },
            { Category.MajorasIncarnation,   Type.Bgm },
            { Category.MajorasMask,          Type.Bgm },
            { Category.JapasRoom,            Type.Bgm },
            { Category.TijosRoom,            Type.Bgm },
            { Category.EvansRoom,            Type.Bgm },
            { Category.IkanaCastle,          Type.Bgm },
            { Category.KamarosTheme,         Type.Bgm },
            { Category.CremiasTheme,         Type.Bgm },
            { Category.KeatonsTheme,         Type.Bgm },
            { Category.MoonEnraged,          Type.Bgm },
            { Category.ReunionTheme,         Type.Bgm },

            // Individual Fanfare Categories
            { Category.EventFail1,           Type.Fanfare },
            { Category.EventFail2,           Type.Fanfare },
            { Category.EventSuccess,         Type.Fanfare },
            { Category.GameOver,             Type.Fanfare },
            { Category.BossDefeated,         Type.Fanfare },
            { Category.ItemGet,              Type.Fanfare },
            { Category.HeartContainerGet,    Type.Fanfare },
            { Category.OpenChest,            Type.Fanfare },
            { Category.MaskGet,              Type.Fanfare },
            { Category.HeartPieceGet,        Type.Fanfare },
            { Category.TruthRevealed,        Type.Fanfare },
            { Category.GoronRaceWin,         Type.Fanfare },
            { Category.HorseRaceWin,         Type.Fanfare },
            { Category.SongGet,              Type.Fanfare },
            { Category.SoaringTheme,         Type.Fanfare },
            { Category.TempleAppears,        Type.Fanfare },
            { Category.TempleClearShort,     Type.Fanfare },
            { Category.TempleClearLong,      Type.Fanfare },
            { Category.GiantsLeave,          Type.Fanfare },
            { Category.MoonDestroyed,        Type.Fanfare },

            // Individual Cutscene Categories
            { Category.GiantsAppear,         Type.Bgm },
            { Category.TitleDemo,            Type.Bgm },
        };


        // Allow people to utilize display names
        public static readonly Dictionary<string, Category> CategoryDisplayNames = new(StringComparer.OrdinalIgnoreCase)
        {
            // Group Categories
            { "Action Themes",            Category.ActionThemes },
            { "Calm Themes",              Category.CalmThemes },
            { "Item Fanfares",            Category.ItemFanfares },
            { "Event Fanfares",           Category.EventFanfares },
            { "Clear Fanfares",           Category.ClearFanfares},

            // Individual BGM Categories
            { "Termina Field",            Category.TerminaField },
            { "Pursuit Theme",            Category.PursuitTheme },
            { "Majora's Theme",           Category.MajorasTheme },
            { "Majoras Theme",            Category.MajorasTheme },
            { "Clock Tower",              Category.ClockTower },
            { "Stone Tower",              Category.StoneTower },
            { "Inverted Stone Tower",     Category.InvertedStoneTower },
            { "Healing Theme",            Category.HealingTheme },
            { "Southern Swamp",           Category.SouthernSwamp },
            { "Aliens' Theme",            Category.AliensTheme },
            { "Aliens Theme",             Category.AliensTheme },
            { "Boat Cruise",              Category.BoatCruise },
            { "Sharp's Curse",            Category.SharpsCurse },
            { "Sharps Curse",             Category.SharpsCurse },
            { "Great Bay",                Category.GreatBay },
            { "Ikana Canyon",             Category.IkanaCanyon },
            { "Deku Palace",              Category.DekuPalace },
            { "Snowhead",                 Category.Snowhead },
            { "Pirates' Fortress",        Category.PiratesFortress },
            { "Pirates Fortress",         Category.PiratesFortress },
            { "Clock Town 1",             Category.ClockTown1 },
            { "Clock Town 2",             Category.ClockTown2 },
            { "Clock Town 3",             Category.ClockTown3 },
            { "File Select",              Category.FileSelect },
            { "Small Enemy",              Category.SmallEnemy },
            { "Boss Enemy",               Category.BossEnemy },
            { "Woodfall Temple",          Category.WoodfallTemple },
            { "House",                    Category.House },
            { "Minigame",                 Category.MinigameTheme },
            { "Goron Race",               Category.GoronRace },
            { "Music Box House",          Category.MusicBoxHouse },
            { "Great Fairy's Fountain",   Category.GreatFairysFountain },
            { "Great Fairys Fountain",    Category.GreatFairysFountain },
            { "Great Fairy Fountain",     Category.GreatFairysFountain },
            { "Fairy's Fountain",         Category.FairysFountain },
            { "Fairys Fountain",          Category.FairysFountain },
            { "Fairy Fountain",           Category.FairysFountain },
            { "Zelda's Theme",            Category.ZeldasTheme },
            { "Zeldas Theme",             Category.ZeldasTheme },
            { "Rosa Sisters' Theme",      Category.RosaSistersTheme },
            { "Rosa Sisters Theme",       Category.RosaSistersTheme },
            { "Curiosity Shop",           Category.CuriosityShop },
            { "Marine Research Lab",      Category.MarineResearchLab },
            { "Giants' Theme",            Category.GiantsTheme },
            { "Giants Theme",             Category.GiantsTheme },
            { "Guru-Guru's Theme",        Category.GuruGurusTheme },
            { "Guru Guru's Theme",        Category.GuruGurusTheme },
            { "Guru-Gurus Theme",         Category.GuruGurusTheme },
            { "Guru Gurus Theme",         Category.GuruGurusTheme },
            { "Romani Ranch",             Category.RomaniRanch },
            { "Goron Shrine",             Category.GoronShrine },
            { "Mayor's Office",           Category.MayorsOffice },
            { "Mayors Office",            Category.MayorsOffice },
            { "Zora Hall",                Category.ZoraHall },
            { "Big Enemy",                Category.BigEnemy },
            { "Astral Observatory",       Category.AstralObservatory },
            { "Secret Grotto",            Category.SecretGrotto },
            { "Milk Bar",                 Category.MilkBar },
            { "Woods of Mystery",         Category.WoodsOfMystery },
            { "Mystery Woods",            Category.MysteryWoods },
            { "Horse Race",               Category.HorseRace },
            { "Gorman Bros.' Theme",      Category.GormanBrosTheme },
            { "Gorman Bros. Theme",       Category.GormanBrosTheme },
            { "Gorman Bros' Theme",       Category.GormanBrosTheme },
            { "Gorman Bros Theme",        Category.GormanBrosTheme },
            { "Witches' Theme",           Category.WitchesTheme },
            { "Witches Theme",            Category.WitchesTheme },
            { "Koume & Kotake's Theme",   Category.KoumeAndKotakesTheme },
            { "Koume and Kotake's Theme", Category.KoumeAndKotakesTheme },
            { "Koumee & Kotakes Theme",   Category.KoumeAndKotakesTheme },
            { "Koume and Kotakes Theme",  Category.KoumeAndKotakesTheme },
            { "Item Shop",                Category.ItemShop },
            { "Owl's Theme",              Category.OwlsTheme },
            { "Owls Theme",               Category.OwlsTheme },
            { "Kaepora Gaebora's Theme",  Category.KaeporaGaeborasTheme },
            { "Kaepora Gaeboras Theme",   Category.KaeporaGaeborasTheme },
            { "Minigame Shop",            Category.MinigameShop },
            { "Sword School",             Category.SwordSchool },
            { "Final Hours",              Category.FinalHours },
            { "Snowhead Temple",          Category.SnowheadTemple },
            { "Great Bay Temple",         Category.GreatBayTemple },
            { "Majora's Wrath",           Category.MajorasWrath },
            { "Majoras Wrath",            Category.MajorasWrath },
            { "Majora's Incarnation",     Category.MajorasIncarnation },
            { "Majoras Incarnation",      Category.MajorasIncarnation },
            { "Majora's Mask",            Category.MajorasMask },
            { "Majoras Mask",             Category.MajorasMask },
            { "Japas' Room",              Category.JapasRoom },
            { "Japas Room",               Category.JapasRoom },
            { "Tijo's Room",              Category.TijosRoom },
            { "Tijos Room",               Category.TijosRoom },
            { "Evan's Room",              Category.EvansRoom },
            { "Evans Room",               Category.EvansRoom },
            { "Ikana Castle",             Category.IkanaCastle },
            { "Kamaro's Theme",           Category.KamarosTheme },
            { "Kamaros Theme",            Category.KamarosTheme },
            { "Cremia's Theme",           Category.CremiasTheme },
            { "Cremias Theme",            Category.CremiasTheme },
            { "Keaton's Theme",           Category.KeatonsTheme },
            { "Keatons Theme",            Category.KeatonsTheme },
            { "Moon Enraged",             Category.MoonEnraged },
            { "Reunion Theme",            Category.ReunionTheme },

            // Individual Fanfare Categories
            { "Event Fail 1",             Category.EventFail1 },
            { "Event Fail 2",             Category.EventFail2 },
            { "Event Success",            Category.EventSuccess },
            { "Game Over",                Category.GameOver },
            { "Boss Defeated",            Category.BossDefeated },
            { "Item Get",                 Category.ItemGet },
            { "Heart Container Get",      Category.HeartContainerGet },
            { "Open Chest",               Category.OpenChest },
            { "Mask Get",                 Category.MaskGet },
            { "Heart Piece Get",          Category.HeartPieceGet },
            { "Truth Revealed",           Category.TruthRevealed },
            { "Goron Race Win",           Category.GoronRaceWin },
            { "Horse Race Win",           Category.HorseRaceWin },
            { "Song Get",                 Category.SongGet },
            { "Soaring Theme",            Category.SoaringTheme },
            { "Temple Appears",           Category.TempleAppears },
            { "Temple Clear Short",       Category.TempleClearShort },
            { "Temple Clear Long",        Category.TempleClearLong },
            { "Giants Leave",             Category.GiantsLeave },
            { "Moon Destroyed",           Category.MoonDestroyed },

            // Individual Cutscene Categories
            { "Giants Appear",            Category.GiantsAppear },
            { "Title Demo",               Category.TitleDemo },
        };

        public static Type GetCategoryType(int categoryValue)
        {
            if (Enum.IsDefined(typeof(Category), categoryValue))
            {
                var category = (Category)categoryValue;

                if (CategoryTypes.TryGetValue(category, out var type))
                {
                    return type;
                }
            }

            // Default to Bgm if undefined
            return Type.Bgm;
        }

        public static bool IsBgmCategory(int categoryValue)
        {
            return GetCategoryType(categoryValue) == Type.Bgm;
        }

        public static bool IsFanfareCategory(int categoryValue)
        {
            return GetCategoryType(categoryValue) == Type.Fanfare;
        }

    }
}
