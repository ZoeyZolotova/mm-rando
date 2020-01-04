using System;

namespace MMRando.Models.Rom
{
    [Flags]
    public enum SequenceType
    {
        None                = 0x0000,
        Field               = 0x0001, //0
        Town                = 0x0002, //1
        Dungeon             = 0x0004, //2
        Indoors             = 0x0008, //3
        Minigame            = 0x0010, //4
        ActionEvent         = 0x0020, //5
        CalmEvent           = 0x0040, //6
        Boss                = 0x0080, //7
        FanfareEvent        = 0x0100, //8
        FanfareFailure      = 0x0200, //9
        FanfareAreaClear    = 0x0400, //10
        IsFanfareMask       = FanfareEvent | FanfareFailure | FanfareAreaClear,
        Ocarina             = 0x4000, //14
        CutsceneNoLoop      = 0x8000, //15
    }
}
