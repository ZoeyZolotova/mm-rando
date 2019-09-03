using System.ComponentModel;

namespace MMRando.Models
{
    public enum Character
    {
        [Description("Default")]
        LinkMM,
        [Description("Child Link (OoT)")]
        LinkOOT,
        [Description("Adult Link")]
        AdultLink,
        Kafei
    }
}
