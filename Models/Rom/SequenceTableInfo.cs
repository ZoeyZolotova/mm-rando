using System.Collections.Generic;
using static MMRando.Models.Rom.SequenceType;
using static MMRando.Models.Rom.SequenceId;

namespace MMRando.Models.Rom
{
    public class SequenceTableInfo
    {
        public SequenceId Id { get; set; }
        public SequenceType Type { get; set; }
        public int Instrument { get; set; }
        public string Name => Id.ToString();


        public static Dictionary<SequenceId, SequenceTableInfo> Table = new Dictionary<SequenceId, SequenceTableInfo>()
        {
            /* 0x02 */ [mm_terminafield]        = new SequenceTableInfo { Id = mm_terminafield,        Instrument =  3, Type = Field },
            /* 0x03 */ [mm_chase]               = new SequenceTableInfo { Id = mm_chase,               Instrument =  3, Type = ActionEvent },
            /* 0x04 */ [mm_skullkid]            = new SequenceTableInfo { Id = mm_skullkid,            Instrument = 17, Type = ActionEvent | CalmEvent },
            /* 0x05 */ [mm_clocktower]          = new SequenceTableInfo { Id = mm_clocktower,          Instrument = 23, Type = Indoors },
            /* 0x06 */ [mm_stonetower]          = new SequenceTableInfo { Id = mm_stonetower,          Instrument = 25, Type = Dungeon },
            /* 0x07 */ [mm_invertedstonetower]  = new SequenceTableInfo { Id = mm_invertedstonetower,  Instrument = 25, Type = Dungeon },
            /* 0x08 */ [mm_f_chasefail]         = new SequenceTableInfo { Id = mm_f_chasefail,         Instrument =  3, Type = FanfareFailure },
            /* 0x09 */ [mm_f_fail]              = new SequenceTableInfo { Id = mm_f_fail,              Instrument =  3, Type = FanfareFailure },
            /* 0x0B */ [mm_healed]              = new SequenceTableInfo { Id = mm_healed,              Instrument = 23, Type = CalmEvent | Indoors },
            /* 0x0C */ [mm_swamp]               = new SequenceTableInfo { Id = mm_swamp,               Instrument = 28, Type = Field | Town },
            /* 0x0D */ [mm_aliens]              = new SequenceTableInfo { Id = mm_aliens,              Instrument = 22, Type = ActionEvent | CalmEvent },
            /* 0x0E */ [mm_boatcruise]          = new SequenceTableInfo { Id = mm_boatcruise,          Instrument =  5, Type = Minigame | Indoors | CalmEvent },
            /* 0x0F */ [mm_sharp]               = new SequenceTableInfo { Id = mm_sharp,               Instrument =  3, Type = ActionEvent | CalmEvent },
            /* 0x10 */ [mm_greatbay]            = new SequenceTableInfo { Id = mm_greatbay,            Instrument = 29, Type = Field | Town },
            /* 0x11 */ [mm_ikanacanyon]         = new SequenceTableInfo { Id = mm_ikanacanyon,         Instrument = 30, Type = Field | Town },
            /* 0x12 */ [mm_dekupalace]          = new SequenceTableInfo { Id = mm_dekupalace,          Instrument = 25, Type = Town },
            /* 0x13 */ [mm_snowhead]            = new SequenceTableInfo { Id = mm_snowhead,            Instrument = 21, Type = Field | Town },
            /* 0x14 */ [mm_piratefortress]      = new SequenceTableInfo { Id = mm_piratefortress,      Instrument =  3, Type = Dungeon | Field },
            /* 0x15 */ [mm_clocktown1]          = new SequenceTableInfo { Id = mm_clocktown1,          Instrument = 25, Type = Town },
            /* 0x16 */ [mm_clocktown2]          = new SequenceTableInfo { Id = mm_clocktown2,          Instrument = 25, Type = Town },
            /* 0x17 */ [mm_clocktown3]          = new SequenceTableInfo { Id = mm_clocktown3,          Instrument = 25, Type = Town },
            /* 0x18 */ [mm_fileselect]          = new SequenceTableInfo { Id = mm_fileselect,          Instrument =  6, Type = CalmEvent },
            /* 0x19 */ [mm_f_clearshort]        = new SequenceTableInfo { Id = mm_f_clearshort,        Instrument =  3, Type = FanfareEvent },
            /* 0x1B */ [mm_boss]                = new SequenceTableInfo { Id = mm_boss,                Instrument =  3, Type = Boss },
            /* 0x1C */ [mm_woodfalltemple]      = new SequenceTableInfo { Id = mm_woodfalltemple,      Instrument = 20, Type = Dungeon },
            /* 0x1F */ [mm_house]               = new SequenceTableInfo { Id = mm_house,               Instrument =  3, Type = Indoors },
            /* 0x20 */ [mm_f_gameover]          = new SequenceTableInfo { Id = mm_f_gameover,          Instrument = 15, Type = FanfareFailure },
            /* 0x21 */ [mm_f_bossdown]          = new SequenceTableInfo { Id = mm_f_bossdown,          Instrument =  3, Type = FanfareAreaClear | FanfareFailure },
            /* 0x22 */ [mm_f_gotitem]           = new SequenceTableInfo { Id = mm_f_gotitem,           Instrument = 15, Type = FanfareEvent },
            /* 0x24 */ [mm_f_heart]             = new SequenceTableInfo { Id = mm_f_heart,             Instrument = 15, Type = FanfareEvent },
            /* 0x25 */ [mm_minigame]            = new SequenceTableInfo { Id = mm_minigame,            Instrument =  3, Type = Minigame },
            /* 0x26 */ [mm_goronrace]           = new SequenceTableInfo { Id = mm_goronrace,           Instrument = 38, Type = Minigame },
            /* 0x27 */ [mm_musicbox]            = new SequenceTableInfo { Id = mm_musicbox,            Instrument =  5, Type = Indoors | Minigame | CalmEvent },
            /* 0x28 */ [mm_fairyfountain]       = new SequenceTableInfo { Id = mm_fairyfountain,       Instrument =  6, Type = CalmEvent | Town | Indoors },
            /* 0x29 */ [mm_zelda]               = new SequenceTableInfo { Id = mm_zelda,               Instrument =  6, Type = CalmEvent | Town | Indoors },
            /* 0x2C */ [mm_laboratory]          = new SequenceTableInfo { Id = mm_laboratory,          Instrument = 13, Type = Indoors | CalmEvent },
            /* 0x2D */ [mm_giants]              = new SequenceTableInfo { Id = mm_giants,              Instrument = 18, Type = CalmEvent | Indoors },
            /* 0x2E */ [mm_guruguru]            = new SequenceTableInfo { Id = mm_guruguru,            Instrument =  5, Type = Indoors },
            /* 0x2F */ [mm_romaniranch]         = new SequenceTableInfo { Id = mm_romaniranch,         Instrument =  7, Type = Town },
            /* 0x30 */ [mm_goronshrine]         = new SequenceTableInfo { Id = mm_goronshrine,         Instrument = 38, Type = Town },
            /* 0x31 */ [mm_meeting]             = new SequenceTableInfo { Id = mm_meeting,             Instrument =  3, Type = ActionEvent | CalmEvent | Indoors },
            /* 0x36 */ [mm_zorahall]            = new SequenceTableInfo { Id = mm_zorahall,            Instrument = 11, Type = Town },
            /* 0x37 */ [mm_f_mask]              = new SequenceTableInfo { Id = mm_f_mask,              Instrument = 15, Type = FanfareEvent },
            /* 0x38 */ [mm_miniboss]            = new SequenceTableInfo { Id = mm_miniboss,            Instrument =  3, Type = Boss },
            /* 0x39 */ [mm_f_smallitem]         = new SequenceTableInfo { Id = mm_f_smallitem,         Instrument = 15, Type = FanfareEvent },
            /* 0x3A */ [mm_observatory]         = new SequenceTableInfo { Id = mm_observatory,         Instrument = 23, Type = Indoors },
            /* 0x3B */ [mm_caves]               = new SequenceTableInfo { Id = mm_caves,               Instrument = 26, Type = Dungeon },
            /* 0x3C */ [mm_milkbar]             = new SequenceTableInfo { Id = mm_milkbar,             Instrument = 32, Type = Indoors | Minigame },
            /* 0x3D */ [mm_f_meet]              = new SequenceTableInfo { Id = mm_f_meet,              Instrument =  3, Type = FanfareEvent },
            /* 0x3E */ [mm_mysterywoods]        = new SequenceTableInfo { Id = mm_mysterywoods,        Instrument =  4, Type = CalmEvent | Town | Indoors | Minigame },
            /* 0x3F */ [mm_f_goronwin]          = new SequenceTableInfo { Id = mm_f_goronwin,          Instrument = 38, Type = FanfareEvent },
            /* 0x40 */ [mm_horserace]           = new SequenceTableInfo { Id = mm_horserace,           Instrument =  8, Type = Minigame },
            /* 0x41 */ [mm_f_horsewin]          = new SequenceTableInfo { Id = mm_f_horsewin,          Instrument =  8, Type = FanfareEvent },
            /* 0x42 */ [mm_gormanbros]          = new SequenceTableInfo { Id = mm_gormanbros,          Instrument =  8, Type = CalmEvent | Town },
            /* 0x43 */ [mm_witches]             = new SequenceTableInfo { Id = mm_witches,             Instrument = 14, Type = CalmEvent | Indoors },
            /* 0x44 */ [mm_shop]                = new SequenceTableInfo { Id = mm_shop,                Instrument = 12, Type = Indoors },
            /* 0x45 */ [mm_kaepora]             = new SequenceTableInfo { Id = mm_kaepora,             Instrument = 16, Type = CalmEvent },
            /* 0x46 */ [mm_shootinggallery]     = new SequenceTableInfo { Id = mm_shootinggallery,     Instrument = 10, Type = Indoors | Minigame },
            /* 0x50 */ [mm_swordschool]         = new SequenceTableInfo { Id = mm_swordschool,         Instrument = 24, Type = ActionEvent | Indoors },
            /* 0x52 */ [mm_f_song]              = new SequenceTableInfo { Id = mm_f_song,              Instrument =  6, Type = FanfareEvent },
            /* 0x55 */ [mm_f_soar]              = new SequenceTableInfo { Id = mm_f_soar,              Instrument =  9, Type = FanfareEvent | FanfareFailure },
            /* 0x65 */ [mm_snowheadtemple]      = new SequenceTableInfo { Id = mm_snowheadtemple,      Instrument = 21, Type = Dungeon },
            /* 0x66 */ [mm_greatbaytemple]      = new SequenceTableInfo { Id = mm_greatbaytemple,      Instrument = 22, Type = Dungeon },
            /* 0x69 */ [mm_wrath]               = new SequenceTableInfo { Id = mm_wrath,               Instrument = 17, Type = Boss },
            /* 0x6A */ [mm_incarnation]         = new SequenceTableInfo { Id = mm_incarnation,         Instrument = 17, Type = Boss },
            /* 0x6B */ [mm_mask]                = new SequenceTableInfo { Id = mm_mask,                Instrument = 17, Type = Boss },
            /* 0x6F */ [mm_ikanacastle]         = new SequenceTableInfo { Id = mm_ikanacastle,         Instrument = 33, Type = Dungeon | Field },
            /* 0x70 */ [mm_c_giantscs]          = new SequenceTableInfo { Id = mm_c_giantscs,          Instrument = 33, Type = CutsceneNoLoop },
            /* 0x72 */ [mm_wagonride]           = new SequenceTableInfo { Id = mm_wagonride,           Instrument =  8, Type = Minigame | Indoors | CalmEvent },
            /* 0x73 */ [mm_keaton]              = new SequenceTableInfo { Id = mm_keaton,              Instrument = 39, Type = CalmEvent },
            /* 0x76 */ [mm_c_titlescreen]       = new SequenceTableInfo { Id = mm_c_titlescreen,       Instrument = 35, Type = CutsceneNoLoop },
            /* 0x77 */ [mm_f_dungeonopen]       = new SequenceTableInfo { Id = mm_f_dungeonopen,       Instrument = 15, Type = FanfareFailure | FanfareEvent },
            /* 0x78 */ [mm_f_dungeonclearshort] = new SequenceTableInfo { Id = mm_f_dungeonclearshort, Instrument = 15, Type = FanfareAreaClear },
            /* 0x79 */ [mm_f_dungeonclearlong]  = new SequenceTableInfo { Id = mm_f_dungeonclearlong,  Instrument = 15, Type = FanfareAreaClear },
            /* 0x7B */ [mm_maskreveal]          = new SequenceTableInfo { Id = mm_maskreveal,          Instrument = 17, Type = ActionEvent },
            /* 0x7C */ [mm_f_giantsleave]       = new SequenceTableInfo { Id = mm_f_giantsleave,       Instrument = 33, Type = FanfareAreaClear },
            /* 0x7D */ [mm_reunion]             = new SequenceTableInfo { Id = mm_reunion,             Instrument = 33, Type = CalmEvent },
            /* 0x7E */ [mm_f_moonclear]         = new SequenceTableInfo { Id = mm_f_moonclear,         Instrument = 33, Type = FanfareAreaClear },
        };
    }
}
