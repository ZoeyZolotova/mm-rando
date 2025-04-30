using System;
using System.Collections.Generic;

namespace MMR.Randomizer.Constants
{
    public static class MusicGroups
    {
        // There's a few cases of alternative names being allowed, and quite a bit of special handling for spaces and proper punctuation
        // They could be removed to have a more standardized list... After all, why not? Why shouldn't I keep it?
        public enum Group : int
        {
            // BGM Categories
            Fields       = 0x00,
            Towns        = 0x01,
            Dungeons     = 0x02,
            Indoors      = 0x03,
            Minigames    = 0x04,
            ActionThemes = 0x05,
            CalmThemes   = 0x06,
            Fights       = 0x07,

            // Fanfare Categories
            ItemFanfares  = 0x08,
            EventFanfares = 0x09,
            ClearFanfares = 0x10,

            // Special
            Cutscenes = 0x16,
        }

        // There are a few cases where alternate names are allowed, some people may make mistakes — especially end users
        public enum Individual : int
        {
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
            FairysFountain       = 0x128, // Just in case
            ZeldasTheme          = 0x129,
            RosaSistersTheme     = 0x12A,
            CuriosityShop        = 0x12C,
            MarineResearchLab    = 0x12C, // Just in case
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
            MysteryWoods         = 0x13E, // Just in case
            HorseRace            = 0x140,
            GormanBrosTheme      = 0x142,
            WitchesTheme         = 0x143,
            KoumeAndKotakesTheme = 0x143, // Just in case
            ItemShop             = 0x144,
            OwlsTheme            = 0x145,
            KaeporaGaeborasTheme = 0x145, // Just in case
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
            EventFail1        = 0x108,
            EventFail2        = 0x109,
            EventSuccess      = 0x119,
            GameOver          = 0x120,
            BossDefeated      = 0x121,
            ItemGet           = 0x122,
            HeartContainerGet = 0x124,
            OpenChest         = 0x12B,
            MaskGet           = 0x137,
            HeartPieceGet     = 0x139,
            TruthRevealed     = 0x13D,
            GoronRaceWin      = 0x13F,
            HorseRaceWin      = 0x141,
            SongGet           = 0x152,
            SoaringTheme      = 0x155,
            TempleAppears     = 0x177,
            TempleClearShort  = 0x178,
            TempleClearLong   = 0x179,
            GiantsLeave       = 0x17C,
            MoonDestroyed     = 0x17E,

            // Cutscenes
            GiantsAppear      = 0x170,
            TitleDemo         = 0x176,
        }

        // Allow people to utilize display names
        public static readonly Dictionary<string, Group> GroupDisplayNames = new(StringComparer.OrdinalIgnoreCase)
        {
            { "Action Themes", Group.ActionThemes },
            { "Calm Themes", Group.CalmThemes },
            { "Item Fanfares", Group.ItemFanfares },
            { "Event Fanfares", Group.EventFanfares },
            { "Clear Fanfares", Group.ClearFanfares},
        };

        // There is a lot of special handling for the individual categories... it might not be efficient, but I wanted to
        public static readonly Dictionary<string, Individual> IndividualDisplayNames = new(StringComparer.OrdinalIgnoreCase)
        {
            // BGM
            { "Termina Field", Individual.TerminaField },
            { "Pursuit Theme", Individual.PursuitTheme },

            { "Majora's Theme", Individual.MajorasTheme },
            { "Majoras Theme", Individual.MajorasTheme },

            { "Clock Tower", Individual.ClockTower },
            { "Stone Tower", Individual.StoneTower },
            { "Inverted Stone Tower", Individual.InvertedStoneTower },
            { "Healing Theme", Individual.HealingTheme },
            { "Southern Swamp", Individual.SouthernSwamp },

            { "Aliens' Theme", Individual.AliensTheme },
            { "Aliens Theme", Individual.AliensTheme },

            { "Boat Cruise", Individual.BoatCruise },

            { "Sharp's Curse", Individual.SharpsCurse },
            { "Sharps Curse", Individual.SharpsCurse },

            { "Great Bay", Individual.GreatBay },
            { "Ikana Canyon", Individual.IkanaCanyon },
            { "Deku Palace", Individual.DekuPalace },
            { "Snowhead", Individual.Snowhead },

            { "Pirates' Fortress", Individual.PiratesFortress },
            { "Pirates Fortress", Individual.PiratesFortress },

            { "Clock Town 1", Individual.ClockTown1 },
            { "Clock Town 2", Individual.ClockTown2 },
            { "Clock Town 3", Individual.ClockTown3 },
            { "File Select", Individual.FileSelect },
            { "Small Enemy", Individual.SmallEnemy },
            { "Boss Enemy", Individual.BossEnemy },
            { "Woodfall Temple", Individual.WoodfallTemple },
            { "House", Individual.House },
            { "Minigame", Individual.MinigameTheme },
            { "Goron Race", Individual.GoronRace },
            { "Music Box House", Individual.MusicBoxHouse },

            { "Great Fairy's Fountain", Individual.GreatFairysFountain },
            { "Great Fairys Fountain", Individual.GreatFairysFountain },
            { "Great Fairy Fountain", Individual.GreatFairysFountain },

            { "Fairy's Fountain", Individual.FairysFountain },
            { "Fairys Fountain", Individual.FairysFountain },
            { "Fairy Fountain", Individual.FairysFountain },

            { "Zelda's Theme", Individual.ZeldasTheme },
            { "Zeldas Theme", Individual.ZeldasTheme },

            { "Rosa Sisters' Theme", Individual.RosaSistersTheme },
            { "Rosa Sisters Theme", Individual.RosaSistersTheme },

            { "Curiosity Shop", Individual.CuriosityShop },
            { "Marine Research Lab", Individual.MarineResearchLab },

            { "Giants' Theme", Individual.GiantsTheme },
            { "Giants Theme", Individual.GiantsTheme },

            { "Guru-Guru's Theme", Individual.GuruGurusTheme },
            { "Guru Guru's Theme", Individual.GuruGurusTheme },
            { "Guru-Gurus Theme", Individual.GuruGurusTheme },
            { "Guru Gurus Theme", Individual.GuruGurusTheme },

            { "Romani Ranch", Individual.RomaniRanch },
            { "Goron Shrine", Individual.GoronShrine },

            { "Mayor's Office", Individual.MayorsOffice },
            { "Mayors Office", Individual.MayorsOffice },

            { "Zora Hall", Individual.ZoraHall },
            { "Big Enemy", Individual.BigEnemy },
            { "Astral Observatory", Individual.AstralObservatory },
            { "Secret Grotto", Individual.SecretGrotto },
            { "Milk Bar", Individual.MilkBar },
            { "Woods of Mystery", Individual.WoodsOfMystery },
            { "Mystery Woods", Individual.MysteryWoods },
            { "Horse Race", Individual.HorseRace },

            { "Gorman Bros.' Theme", Individual.GormanBrosTheme },
            { "Gorman Bros. Theme", Individual.GormanBrosTheme },
            { "Gorman Bros' Theme", Individual.GormanBrosTheme },
            { "Gorman Bros Theme", Individual.GormanBrosTheme },

            { "Witches' Theme", Individual.WitchesTheme },
            { "Witches Theme", Individual.WitchesTheme },

            { "Koume & Kotake's Theme", Individual.KotakeAndKoumesTheme },
            { "Koume and Kotake's Theme", Individual.KotakeAndKoumesTheme },
            { "Koume & Kotakes Theme", Individual.KotakeAndKoumesTheme },
            { "Koume and Kotakes Theme", Individual.KotakeAndKoumesTheme },

            { "Item Shop", Individual.ItemShop },

            { "Owl's Theme", Individual.OwlsTheme },
            { "Owls Theme", Individual.OwlsTheme },
            { "Kaepora Gaebora's Theme", Individual.KaeporaGaeborasTheme },
            { "Kaepora Gaeboras Theme", Individual.KaeporaGaeborasTheme },

            { "Minigame Shop", Individual.MinigameShop },
            { "Sword School", Individual.SwordSchool },
            { "Final Hours", Individual.FinalHours },
            { "Snowhead Temple", Individual.SnowheadTemple },
            { "Great Bay Temple", Individual.GreatBayTemple },

            { "Majora's Wrath", Individual.MajorasWrath },
            { "Majoras Wrath", Individual.MajorasWrath },

            { "Majora's Incarnation", Individual.MajorasIncarnation },
            { "Majoras Incarnation", Individual.MajorasIncarnation },

            { "Majora's Mask", Individual.MajorasMask },
            { "Majoras Mask", Individual.MajorasMask },

            { "Japas' Room", Individual.JapasRoom },
            { "Japas Room", Individual.JapasRoom },

            { "Tijo's Room", Individual.TijosRoom },
            { "Tijos Room", Individual.TijosRoom },

            { "Evan's Room", Individual.EvansRoom },
            { "Evans Room", Individual.EvansRoom },

            { "Ikana Castle", Individual.IkanaCastle },

            { "Kamaro's Theme", Individual.KamarosTheme },
            { "Kamaros Theme", Individual.KamarosTheme },

            { "Cremia's Theme", Individual.CremiasTheme },
            { "Cremias Theme", Individual.CremiasTheme },

            { "Keaton's Theme", Individual.KeatonsTheme },
            { "Keatons Theme", Individual.KeatonsTheme },

            { "Moon Enraged", Individual.MoonEnraged },
            { "Reunion Theme", Individual.ReunionTheme },

            // Fanfares
            { "Event Fail 1", Individual.EventFail1 },
            { "Event Fail 2", Individual.EventFail2 },
            { "Event Success", Individual.EventSuccess },
            { "Game Over", Individual.GameOver },
            { "Boss Defeated", Individual.BossDefeated },
            { "Item Get", Individual.ItemGet },
            { "Heart Container Get", Individual.HeartContainerGet },
            { "Open Chest", Individual.OpenChest },
            { "Mask Get", Individual.MaskGet },
            { "Heart Piece Get", Individual.HeartPieceGet },
            { "Truth Revealed", Individual.TruthRevealed },
            { "Goron Race Win", Individual.GoronRaceWin },
            { "Horse Race Win", Individual.HorseRaceWin },
            { "Song Get", Individual.SongGet },
            { "Soaring Theme", Individual.SoaringTheme },
            { "Temple Appears", Individual.TempleAppears },
            { "Temple Clear Short", Individual.TempleClearShort },
            { "Temple Clear Long", Individual.TempleClearLong },
            { "Giants Leave", Individual.GiantsLeave },
            { "Moon Destroyed", Individual.MoonDestroyed },

            // Cutscenes
            { "Giants Appear", Individual.GiantsAppear },
            { "Title Demo", Individual.TitleDemo },
        };
    }
}
